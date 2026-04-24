using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using AdminSystem_v2.Models;

namespace AdminSystem_v2.Services
{
    /// <summary>
    /// Builds a WPF FlowDocument receipt and sends it to the printer via PrintDialog.
    /// Uses WPF Table layout for columns — no character-padding hacks.
    /// </summary>
    public class ReceiptPrintService : IReceiptPrintService
    {
        private const double PageMargin = 14;
        private const double ValueColW  = 92;   // right column for label-value rows (fits ₱99,999.99)
        private const double QtyColW    = 28;
        private const double PriceColW  = 70;
        private const double TotalColW  = 70;

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
            var dlg = new PrintDialog();
            if (dlg.ShowDialog() != true) return;

            double pageW = dlg.PrintableAreaWidth;
            double pageH = dlg.PrintableAreaHeight;

            var doc = BuildReceipt(result, pageW);
            var paginator = ((IDocumentPaginatorSource)doc).DocumentPaginator;
            paginator.PageSize = new Size(pageW, pageH);

            dlg.PrintDocument(paginator, $"Receipt #{result.OrderNumber}");
        }

        // ── Document builder ─────────────────────────────────────────────────

        private static FlowDocument BuildReceipt(POSOrderResult result, double pageWidth)
        {
            var doc = new FlowDocument
            {
                PageWidth   = pageWidth,
                PagePadding = new Thickness(PageMargin),
                FontFamily  = new FontFamily("Consolas, Courier New"),
                FontSize    = 10,
                Foreground  = Brushes.Black,
                ColumnWidth = double.MaxValue
            };

            // Header
            AddCenteredBold(doc, "TAURUS BIKE SHOP", 14);
            AddCentered(doc, "Your trusted bike partner", 8);
            AddSeparator(doc);

            // Order info
            AddLabelValue(doc, "Order #:",   result.OrderNumber);
            AddLabelValue(doc, "Date:",      result.CompletedAt.ToString("MMM dd, yyyy hh:mm tt"));
            AddLabelValue(doc, "Cashier:",   result.CashierName);
            AddLabelValue(doc, "Customer:",  result.CustomerName);
            AddSeparator(doc);

            // Items
            doc.Blocks.Add(BuildItemsTable(result.Items));
            AddSeparator(doc);

            // Totals
            AddLabelValue(doc, "Subtotal:", $"₱{result.Subtotal:N2}");

            if (result.HasDiscount)
            {
                var discLabel = result.HasVoucher
                    ? $"Discount ({result.VoucherCode}):"
                    : "Discount:";
                AddLabelValue(doc, discLabel, $"-₱{result.DiscountAmount:N2}");
            }

            AddThinSeparator(doc);
            AddLabelValueBold(doc, "TOTAL:", $"₱{result.GrandTotal:N2}", 12);

            // Payment
            AddLabelValue(doc, "Payment:", result.PaymentMethod);

            if (result.IsCashMethod)
            {
                AddLabelValue(doc, "Cash Received:", $"₱{result.CashReceived:N2}");
                AddLabelValue(doc, "Change:",        $"₱{result.Change:N2}");
            }

            AddSeparator(doc);
            AddCentered(doc, "Thank you for your purchase!", 10);
            AddCentered(doc, "Please come again", 8);

            return doc;
        }

        // ── Items table ──────────────────────────────────────────────────────

        private static Table BuildItemsTable(List<POSCartItem> items)
        {
            var table = new Table { FontSize = 9, Margin = new Thickness(0, 2, 0, 2), CellSpacing = 0 };

            table.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) });
            table.Columns.Add(new TableColumn { Width = new GridLength(QtyColW) });
            table.Columns.Add(new TableColumn { Width = new GridLength(PriceColW) });
            table.Columns.Add(new TableColumn { Width = new GridLength(TotalColW) });

            var headerGroup = new TableRowGroup();
            var headerRow   = new TableRow { FontWeight = FontWeights.Bold };
            headerRow.Cells.Add(Cell("ITEM",  TextAlignment.Left));
            headerRow.Cells.Add(Cell("QTY",   TextAlignment.Center));
            headerRow.Cells.Add(Cell("PRICE", TextAlignment.Right));
            headerRow.Cells.Add(Cell("TOTAL", TextAlignment.Right));
            headerGroup.Rows.Add(headerRow);
            table.RowGroups.Add(headerGroup);

            var bodyGroup = new TableRowGroup();
            foreach (var item in items)
            {
                var row = new TableRow();
                row.Cells.Add(Cell(item.DisplayName,               TextAlignment.Left));
                row.Cells.Add(Cell(item.Quantity.ToString(),        TextAlignment.Center));
                row.Cells.Add(Cell($"₱{item.UnitPrice:N2}",   TextAlignment.Right));
                row.Cells.Add(Cell($"₱{item.LineTotal:N2}",   TextAlignment.Right));
                bodyGroup.Rows.Add(row);
            }
            table.RowGroups.Add(bodyGroup);

            return table;
        }

        private static TableCell Cell(string text, TextAlignment align) =>
            new(new Paragraph(new Run(text))
            {
                TextAlignment = align,
                Margin        = new Thickness(0, 1, 2, 1)
            });

        // ── Label-value rows ─────────────────────────────────────────────────

        private static void AddLabelValue(FlowDocument doc, string label, string value)
        {
            var table = TwoColTable(9, new Thickness(0, 1, 0, 1));
            var rg    = new TableRowGroup();
            var row   = new TableRow();
            row.Cells.Add(Cell(label, TextAlignment.Left));
            row.Cells.Add(Cell(value, TextAlignment.Right));
            rg.Rows.Add(row);
            table.RowGroups.Add(rg);
            doc.Blocks.Add(table);
        }

        private static void AddLabelValueBold(FlowDocument doc, string label, string value, double fontSize)
        {
            var table = TwoColTable(fontSize, new Thickness(0, 4, 0, 4));
            table.FontWeight = FontWeights.Bold;
            var rg   = new TableRowGroup();
            var row  = new TableRow();
            row.Cells.Add(Cell(label, TextAlignment.Left));
            row.Cells.Add(Cell(value, TextAlignment.Right));
            rg.Rows.Add(row);
            table.RowGroups.Add(rg);
            doc.Blocks.Add(table);
        }

        private static Table TwoColTable(double fontSize, Thickness margin)
        {
            var t = new Table { FontSize = fontSize, Margin = margin, CellSpacing = 0 };
            t.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) });
            t.Columns.Add(new TableColumn { Width = new GridLength(ValueColW) });
            return t;
        }

        // ── Decorative ───────────────────────────────────────────────────────

        private static void AddCenteredBold(FlowDocument doc, string text, double fontSize)
        {
            doc.Blocks.Add(new Paragraph(new Run(text))
            {
                TextAlignment = TextAlignment.Center,
                FontWeight    = FontWeights.Bold,
                FontSize      = fontSize,
                Margin        = new Thickness(0, 2, 0, 0)
            });
        }

        private static void AddCentered(FlowDocument doc, string text, double fontSize)
        {
            doc.Blocks.Add(new Paragraph(new Run(text))
            {
                TextAlignment = TextAlignment.Center,
                FontSize      = fontSize,
                Margin        = new Thickness(0, 0, 0, 2)
            });
        }

        private static void AddSeparator(FlowDocument doc)
        {
            doc.Blocks.Add(new BlockUIContainer(new Rectangle
            {
                Height              = 1,
                Fill                = Brushes.Black,
                HorizontalAlignment = HorizontalAlignment.Stretch
            })
            { Margin = new Thickness(0, 5, 0, 5) });
        }

        private static void AddThinSeparator(FlowDocument doc)
        {
            doc.Blocks.Add(new BlockUIContainer(new Rectangle
            {
                Height              = 0.5,
                Fill                = Brushes.DimGray,
                HorizontalAlignment = HorizontalAlignment.Stretch
            })
            { Margin = new Thickness(0, 3, 0, 3) });
        }
    }
}
