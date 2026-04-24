using AdminSystem_v2.Models;

namespace AdminSystem_v2.Services
{
    /// <summary>Formats and prints a POS receipt to the default system printer.</summary>
    public interface IReceiptPrintService
    {
        /// <summary>Print the receipt for a completed POS sale.</summary>
        void PrintReceipt(POSOrderResult result);
    }
}
