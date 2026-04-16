using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AdminSystem_v2.Views
{
    public partial class StorePaymentAccountView : UserControl
    {
        private static readonly Regex NonDigit = new(@"\D");

        public StorePaymentAccountView()
        {
            InitializeComponent();
        }

        private void AccountNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (NonDigit.IsMatch(e.Text)) e.Handled = true;
        }

        private void AccountNumber_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (!e.DataObject.GetDataPresent(typeof(string))) { e.CancelCommand(); return; }

            string text = (string)e.DataObject.GetData(typeof(string));
            if (NonDigit.IsMatch(text)) e.CancelCommand();
        }
    }
}
