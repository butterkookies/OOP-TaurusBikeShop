using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using AdminSystem_v2.Models;

namespace AdminSystem_v2.Services
{
    /// <summary>
    /// Builds a WPF FlowDocument receipt and sends it to the default printer
    /// using PrintDialog (silent print for POS speed).
    /// Works with thermal printers (58mm/80mm) and standard printers (Epson L3210, etc.).
    /// </summary>
    public class ReceiptPrintService : IReceiptPrintService
    {
        // 80mm thermal paper ≈ 302px at 96 DPI (with ~4mm margins on each side)
        private const double PageWidth  = 302;
        private const double PageMargin = 10;

        public void PrintReceipt(POSOrderResult result)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() => PrintOnUIThread(result));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ReceiptPrint] Failed: {ex.Message}");
            }
        }

        private void PrintOnUIThread(POSOrderResult result)
        {
            var doc = BuildReceipt(result);

            // Use PrintDialog to print silently to default printer
            var printDialog = new PrintDialog();

            // Get default printer without showing the dialog
            var paginator = ((IDocumentPaginatorSource)doc).DocumentPaginator;
            paginator.PageSize = new Size(PageWidth, double.MaxValue);

            try
            {
                printDialog.PrintDocument(paginator, $"Receipt #{result.OrderNumber}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ReceiptPrint] Printer error: {ex.Message}");
            }
        }

        // ── Receipt document builder ─────────────────────────────────────────

        private FlowDocument BuildReceipt(POSOrderResult result)
        {
            var doc = new FlowDocument
            {
                PageWidth     = PageWidth,
                PagePadding   = new Thickness(PageMargin),
                FontFamily    = new FontFamily("Consolas, Courier New, monospace"),
                FontSize      = 10,
                Foreground    = Brushes.Black,
                ColumnWidth   = double.MaxValue  // single-column layout
            };

            // ── Store header ─────────────────────────────────────────────────
            AddCenteredBold(doc, "TAURUS BIKE SHOP", 14);
            AddCentered(doc, "Your trusted bike partner", 8);
            AddSeparator(doc);

            // ── Order info ───────────────────────────────────────────────────
            AddRow(doc, "Order #:", result.OrderNumber);
            AddRow(doc, "Date:",    result.CompletedAt.ToString("MMM dd, yyyy hh:mm tt"));
            AddRow(doc, "Cashier:", result.CashierName);
            AddRow(doc, "Customer:", result.CustomerName);
            AddSeparator(doc);

            // ── Items header ─────────────────────────────────────────────────
            var headerRow = new Paragraph
            {
                FontWeight = FontWeights.Bold,
                FontSize   = 9,
                Margin     = new Thickness(0, 2, 0, 2)
            };
            headerRow.Inlines.Add(new Run(FormatItemLine("ITEM", "QTY", "PRICE", "TOTAL")));
            doc.Blocks.Add(headerRow);
            AddThinSeparator(doc);

            // ── Items ────────────────────────────────────────────────────────
            foreach (var item in result.Items)
            {
                var name = TruncateText(item.DisplayName, 16);
                var line = FormatItemLine(
                    name,
                    item.Quantity.ToString(),
                    item.UnitPrice.ToString("N2"),
                    item.LineTotal.ToString("N2"));

                var p = new Paragraph(new Run(line))
                {
                    FontSize = 9,
                    Margin   = new Thickness(0, 1, 0, 1)
                };
                doc.Blocks.Add(p);
            }

            AddSeparator(doc);

            // ── Totals ───────────────────────────────────────────────────────
            AddRow(doc, "Subtotal:", $"₱{result.Subtotal:N2}");

            if (result.HasDiscount)
            {
                var discountLabel = result.HasVoucher
                    ? $"Discount ({result.VoucherCode}):"
                    : "Discount:";
                AddRow(doc, discountLabel, $"-₱{result.DiscountAmount:N2}");
            }

            AddThinSeparator(doc);

            // Grand total — bold and larger
            var totalPara = new Paragraph
            {
                FontWeight = FontWeights.Bold,
                FontSize   = 12,
                Margin     = new Thickness(0, 4, 0, 4)
            };
            totalPara.Inlines.Add(new Run(PadRow("TOTAL:", $"₱{result.GrandTotal:N2}")));
            doc.Blocks.Add(totalPara);

            // ── Payment details ──────────────────────────────────────────────
            AddRow(doc, "Payment:", result.PaymentMethod);

            if (result.IsCashMethod)
            {
                AddRow(doc, "Cash Received:", $"₱{result.CashReceived:N2}");
                AddRow(doc, "Change:",        $"₱{result.Change:N2}");
            }

            AddSeparator(doc);

            // ── Footer ───────────────────────────────────────────────────────
            AddCentered(doc, "Thank you for your purchase!", 10);
            AddCentered(doc, "Please come again", 8);

            return doc;
        }

        // ── Helper methods ───────────────────────────────────────────────────

        private static void AddCenteredBold(FlowDocument doc, string text, double fontSize)
        {
            var p = new Paragraph(new Run(text))
            {
                TextAlignment = TextAlignment.Center,
                FontWeight    = FontWeights.Bold,
                FontSize      = fontSize,
                Margin        = new Thickness(0, 2, 0, 0)
            };
            doc.Blocks.Add(p);
        }

        private static void AddCentered(FlowDocument doc, string text, double fontSize)
        {
            var p = new Paragraph(new Run(text))
            {
                TextAlignment = TextAlignment.Center,
                FontSize      = fontSize,
                Margin        = new Thickness(0, 0, 0, 2)
            };
            doc.Blocks.Add(p);
        }

        private static void AddRow(FlowDocument doc, string label, string value)
        {
            var p = new Paragraph(new Run(PadRow(label, value)))
            {
                FontSize = 9,
                Margin   = new Thickness(0, 1, 0, 1)
            };
            doc.Blocks.Add(p);
        }

        private static void AddSeparator(FlowDocument doc)
        {
            var p = new Paragraph(new Run(new string('─', 36)))
            {
                FontSize = 8,
                Margin   = new Thickness(0, 4, 0, 4)
            };
            doc.Blocks.Add(p);
        }

        private static void AddThinSeparator(FlowDocument doc)
        {
            var p = new Paragraph(new Run(new string('·', 36)))
            {
                FontSize = 8,
                Margin   = new Thickness(0, 2, 0, 2)
            };
            doc.Blocks.Add(p);
        }

        private static string PadRow(string left, string right)
        {
            const int totalWidth = 36;
            int padding = totalWidth - left.Length - right.Length;
            if (padding < 1) padding = 1;
            return left + new string(' ', padding) + right;
        }

        private static string FormatItemLine(string name, string qty, string price, string total)
        {
            // Layout: NAME(16) QTY(4) PRICE(8) TOTAL(8)  = 36 chars
            return $"{name,-16}{qty,4}{price,8}{total,8}";
        }

        private static string TruncateText(string text, int maxLen)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            return text.Length <= maxLen ? text : text[..(maxLen - 1)] + "…";
        }
    }
}
