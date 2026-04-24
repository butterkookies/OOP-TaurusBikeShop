using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using AdminSystem_v2.Models;

namespace AdminSystem_v2.Services
{
    /// <summary>
    /// Silently prints a receipt to the default printer on A4 paper — no dialog shown.
    /// Layout is fixed to A4 dimensions so output is identical on every printer.
    /// </summary>
    public class ReceiptPrintService : IReceiptPrintService
    {
        // A4 at WPF's 96 DPI (device-independent pixels): 210mm × 297mm
        private const double A4Width  = 793.7;
        private const double A4Height = 1122.5;

        // We simulate an 80mm wide thermal receipt. 80mm ~ 302 pixels (at 96 DPI).
        // We'll use 320 pixels for a comfortable layout without breaking words unnecessarily.
        private const double ReceiptWidth = 320.0;

        // Side margins are calculated to perfectly center the 320px column on the A4 sheet
        private static readonly double SideMargin = (A4Width - ReceiptWidth) / 2.0;

        private static Thickness BlockMargin(double top, double bottom)
        {
            return new Thickness(SideMargin, top, SideMargin, bottom);
        }

        // Keep page padding strict to vertical only, letting block margins handle horizontal bounds
        private static readonly Thickness PagePadding = new(0, 50, 0, 50);

        // Table column widths sized for 12pt font on narrow 320px layout
        private const double ValueColW = 100;   // right column for totals/values rows
        private const double QtyColW   = 35;
        private const double PriceColW = 75;
        private const double TotalColW = 75;

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
            var paginator = ((IDocumentPaginatorSource)doc).DocumentPaginator;
            paginator.PageSize = new Size(A4Width, A4Height);

            // Create dialog for default printer but DO NOT show it — prints silently
            var dlg = new PrintDialog();
            dlg.PrintTicket.PageMediaSize   = new PageMediaSize(PageMediaSizeName.ISOA4);
            dlg.PrintTicket.PageOrientation = PageOrientation.Portrait;
            dlg.PrintDocument(paginator, $"Receipt #{result.OrderNumber}");
        }

        // ── Document builder ─────────────────────────────────────────────────

        private static FlowDocument BuildReceipt(POSOrderResult result)
        {
            var doc = new FlowDocument
            {
                PageWidth   = A4Width,
                PagePadding = PagePadding,
                FontFamily  = new FontFamily("Segoe UI, Arial"),
                FontSize    = 12,
                Foreground  = Brushes.Black,
                ColumnWidth = double.MaxValue
            };

            // ── Header ───────────────────────────────────────────────────────
            AddCenteredBold(doc, "TAURUS BIKE SHOP", 20);
            AddCentered(doc, "Your trusted bike partner", 11);
            AddSeparator(doc);

            // ── Order info ───────────────────────────────────────────────────
            AddLabelValue(doc, "Order #:",  result.OrderNumber);
            AddLabelValue(doc, "Date:",     result.CompletedAt.ToString("MMM dd, yyyy hh:mm tt"));
            AddLabelValue(doc, "Cashier:",  result.CashierName);
            AddLabelValue(doc, "Customer:", result.CustomerName);
            AddSeparator(doc);

            // ── Items ────────────────────────────────────────────────────────
            doc.Blocks.Add(BuildItemsTable(result.Items));
            AddSeparator(doc);

            // ── Totals ───────────────────────────────────────────────────────
            AddLabelValue(doc, "Subtotal:", $"₱{result.Subtotal:N2}");

            if (result.HasDiscount)
            {
                var discLabel = result.HasVoucher
                    ? $"Discount ({result.VoucherCode}):"
                    : "Discount:";
                AddLabelValue(doc, discLabel, $"-₱{result.DiscountAmount:N2}");
            }

            AddThinSeparator(doc);
            AddLabelValueBold(doc, "TOTAL:", $"₱{result.GrandTotal:N2}", 15);

            // ── Payment ──────────────────────────────────────────────────────
            AddLabelValue(doc, "Payment:", result.PaymentMethod);

            if (result.IsCashMethod)
            {
                AddLabelValue(doc, "Cash Received:", $"₱{result.CashReceived:N2}");
                AddLabelValue(doc, "Change:",        $"₱{result.Change:N2}");
            }

            AddSeparator(doc);
            AddCentered(doc, "Thank you for your purchase!", 12);
            AddCentered(doc, "Please come again", 10);

            return doc;
        }

        // ── Items table ──────────────────────────────────────────────────────

        private static Table BuildItemsTable(List<POSCartItem> items)
        {
            var table = new Table { FontSize = 11, Margin = BlockMargin(4, 4), CellSpacing = 0 };

            // Explicit width for ItemCol to prevent any table stretching
            double itemColW = ReceiptWidth - QtyColW - PriceColW - TotalColW;
            table.Columns.Add(new TableColumn { Width = new GridLength(itemColW) });
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
                row.Cells.Add(Cell(item.DisplayName,         TextAlignment.Left));
                row.Cells.Add(Cell(item.Quantity.ToString(), TextAlignment.Center));
                row.Cells.Add(Cell($"₱{item.UnitPrice:N2}", TextAlignment.Right));
                row.Cells.Add(Cell($"₱{item.LineTotal:N2}", TextAlignment.Right));
                bodyGroup.Rows.Add(row);
            }
            table.RowGroups.Add(bodyGroup);

            return table;
        }

        private static TableCell Cell(string text, TextAlignment align) =>
            new(new Paragraph(new Run(text))
            {
                TextAlignment = align,
                Margin        = new Thickness(0, 2, 4, 2)
            });

        // ── Label-value rows ─────────────────────────────────────────────────

        private static void AddLabelValue(FlowDocument doc, string label, string value)
        {
            var table = TwoColTable(12, 2, 2);
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
            var table = TwoColTable(fontSize, 6, 6);
            table.FontWeight = FontWeights.Bold;
            var rg   = new TableRowGroup();
            var row  = new TableRow();
            row.Cells.Add(Cell(label, TextAlignment.Left));
            row.Cells.Add(Cell(value, TextAlignment.Right));
            rg.Rows.Add(row);
            table.RowGroups.Add(rg);
            doc.Blocks.Add(table);
        }

        private static Table TwoColTable(double fontSize, double top, double bottom)
        {
            var t = new Table { FontSize = fontSize, Margin = BlockMargin(top, bottom), CellSpacing = 0 };
            double labelColW = ReceiptWidth - ValueColW;
            t.Columns.Add(new TableColumn { Width = new GridLength(labelColW) });
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
                Margin        = BlockMargin(4, 0)
            });
        }

        private static void AddCentered(FlowDocument doc, string text, double fontSize)
        {
            doc.Blocks.Add(new Paragraph(new Run(text))
            {
                TextAlignment = TextAlignment.Center,
                FontSize      = fontSize,
                Margin        = BlockMargin(0, 4)
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
            { Margin = BlockMargin(6, 6) });
        }

        private static void AddThinSeparator(FlowDocument doc)
        {
            doc.Blocks.Add(new BlockUIContainer(new Rectangle
            {
                Height              = 0.5,
                Fill                = Brushes.DimGray,
                HorizontalAlignment = HorizontalAlignment.Stretch
            })
            { Margin = BlockMargin(4, 4) });
        }
    }
}
