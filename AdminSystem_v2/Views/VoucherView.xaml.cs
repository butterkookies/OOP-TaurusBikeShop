using AdminSystem_v2.ViewModels;

namespace AdminSystem_v2.Views
{
    public partial class VoucherView : System.Windows.Controls.UserControl
    {
        public VoucherView()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
            Unloaded           += OnUnloaded;
        }

        private VoucherViewModel? _vm;

        private void OnDataContextChanged(object sender,
            System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (_vm != null)
                _vm.FocusUserSearchRequested = null;

            _vm = DataContext as VoucherViewModel;

            if (_vm != null)
                _vm.FocusUserSearchRequested = () =>
                    UserSearchBox?.Focus();
        }

        private void OnUnloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_vm != null)
                _vm.FocusUserSearchRequested = null;
        }
    }
}
