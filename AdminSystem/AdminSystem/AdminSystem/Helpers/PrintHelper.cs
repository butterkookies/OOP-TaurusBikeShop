// AdminSystem/Helpers/PrintHelper.cs

using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace AdminSystem.Helpers
{
    /// <summary>
    /// Handles receipt and document printing for POS sessions and orders.
    /// Uses WPF's built-in PrintDialog — no third-party print library needed.
    /// </summary>
    public static class PrintHelper
    {
        /// <summary>
        /// Prints a POS receipt for a walk-in sale.
        /// </summary>
        /// <param name="session">The POS session to print a receipt for.</param>
        public static void PrintPOSReceipt(Models.POS_Session session)
        {
            if (session == null) return;

            FlowDocument doc = BuildPOSReceiptDocument(session);
            PrintDocument(doc, $"TBS Receipt - {session.POSSessionId}");
        }

        /// <summary>
        /// Shows a print dialog and prints the given FlowDocument.
        /// </summary>
        public static void PrintDocument(FlowDocument document, string jobName = "Print")
        {
            PrintDialog dialog = new PrintDialog();
            if (dialog.ShowDialog() != true) return;

            document.PageWidth = dialog.PrintableAreaWidth;
            document.PageHeight = dialog.PrintableAreaHeight;
            document.PagePadding = new Thickness(20);

            IDocumentPaginatorSource source = document;
            dialog.PrintDocument(source.DocumentPaginator, jobName);
        }

        private static FlowDocument BuildPOSReceiptDocument(Models.POS_Session session)
        {
            FlowDocument doc = new FlowDocument
            {
                FontFamily = new FontFamily("Segoe UI"),
                FontSize = 12,
                PagePadding = new Thickness(20)
            };

            // Header
            Paragraph header = new Paragraph(new Run("TAURUS BIKE SHOP"))
            {
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center
            };
            doc.Blocks.Add(header);

            Paragraph sub = new Paragraph(new Run("Official Receipt"))
            {
                TextAlignment = TextAlignment.Center,
                Foreground = AppColors.PrintGray
            };
            doc.Blocks.Add(sub);

            doc.Blocks.Add(new Paragraph(new Run(new string('-', 40)))
            {
                TextAlignment = TextAlignment.Center
            });

            // Session details
            doc.Blocks.Add(new Paragraph(
                new Run($"Date: {session.StartTime:MMMM d, yyyy  h:mm tt}")));
            doc.Blocks.Add(new Paragraph(
                new Run($"Session #: {session.POSSessionId}")));

            doc.Blocks.Add(new Paragraph(new Run(new string('-', 40)))
            {
                TextAlignment = TextAlignment.Center
            });

            // Total
            Paragraph total = new Paragraph(
                new Run($"TOTAL: ₱{session.TotalSales:N2}"))
            {
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Right
            };
            doc.Blocks.Add(total);

            doc.Blocks.Add(new Paragraph(new Run(new string('-', 40)))
            {
                TextAlignment = TextAlignment.Center
            });

            doc.Blocks.Add(new Paragraph(new Run("Thank you for shopping at Taurus Bike Shop!"))
            {
                TextAlignment = TextAlignment.Center,
                Foreground = AppColors.PrintGray
            });

            return doc;
        }
    }
}