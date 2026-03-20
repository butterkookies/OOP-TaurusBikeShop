using System.Windows;
using System.Windows.Controls;
using AdminSystem.Helpers;
using AdminSystem.Models;
using AdminSystem.ViewModels;

namespace AdminSystem.Views
{
    /// <summary>
    /// Code-behind for the Support Tickets management UserControl.
    /// Delegates all data operations to SupportViewModel.
    /// </summary>
    public partial class SupportView : UserControl
    {
        private readonly SupportViewModel _vm;

        public SupportView(SupportViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = _vm;
        }

        // ── Called by MainWindow.Navigate ─────────────────────────────────
        public void Refresh()
        {
            _vm.Load();
            DgTickets.ItemsSource = _vm.Tickets;
            ClearDetail();
            ShowListError(_vm.HasError ? _vm.ErrorMessage : null);
        }

        // ── Filter buttons ────────────────────────────────────────────────
        private void FilterBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            foreach (Button b in new[] {
                BtnFilterAll, BtnFilterOpen, BtnFilterInProgress,
                BtnFilterAwaiting, BtnFilterResolved })
            {
                b.Background = AppColors.CardBorder;
                b.Foreground = AppColors.NavText;
            }
            btn.Background = AppColors.Accent;
            btn.Foreground = System.Windows.Media.Brushes.White;

            _vm.StatusFilter = btn.Tag?.ToString() ?? string.Empty;
            DgTickets.ItemsSource = _vm.Tickets;
            ShowListError(_vm.HasError ? _vm.ErrorMessage : null);
            ClearDetail();
        }

        private void BtnRefreshTickets_Click(object sender, RoutedEventArgs e)
            => Refresh();

        // ── Selection changed ─────────────────────────────────────────────
        private void DgTickets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SupportTicket ticket = DgTickets.SelectedItem as SupportTicket;
            _vm.SelectedTicket = ticket;
            UpdateDetailPanel(ticket);
            HideFeedback();
        }

        // ── Reply ─────────────────────────────────────────────────────────
        private void BtnSendReply_Click(object sender, RoutedEventArgs e)
        {
            _vm.ReplyText = TbReplyText.Text;
            _vm.ReplyCommand.Execute(null);
            if (_vm.HasError)
                ShowError(_vm.ErrorMessage);
            else
            {
                ShowSuccess(_vm.SuccessMessage ?? "Reply sent.");
                TbReplyText.Text = string.Empty;
                UpdateDetailPanel(_vm.SelectedTicket);
            }
        }

        // ── Resolve ───────────────────────────────────────────────────────
        private void BtnResolve_Click(object sender, RoutedEventArgs e)
        {
            _vm.ResolveCommand.Execute(null);
            if (_vm.HasError)
                ShowError(_vm.ErrorMessage);
            else
            {
                ShowSuccess(_vm.SuccessMessage ?? "Ticket resolved.");
                DgTickets.ItemsSource = _vm.Tickets;
                UpdateDetailPanel(_vm.SelectedTicket);
            }
        }

        // ── Add task ──────────────────────────────────────────────────────
        private void BtnAddTask_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selected = CbTaskType.SelectedItem as ComboBoxItem;
            _vm.NewTaskType = selected?.Tag?.ToString() ?? string.Empty;
            _vm.AddTaskCommand.Execute(null);
            if (_vm.HasError)
                ShowError(_vm.ErrorMessage);
            else
            {
                ShowSuccess(_vm.SuccessMessage ?? "Task added.");
                UpdateDetailPanel(_vm.SelectedTicket);
            }
        }

        // ── Detail panel helpers ──────────────────────────────────────────
        private void UpdateDetailPanel(SupportTicket ticket)
        {
            if (ticket == null) { ClearDetail(); return; }
            TbTicketSubject.Text = ticket.Subject;
            TbTicketMeta.Text    = string.Format(
                "#{0}  ·  {1}  ·  {2}  ·  {3}",
                ticket.TicketId,
                ticket.CustomerName ?? "—",
                ticket.TicketCategory,
                ticket.TicketStatus);
            LbReplies.ItemsSource = ticket.Replies;
            LbTasks.ItemsSource   = ticket.Tasks;
            BtnResolve.IsEnabled  =
                ticket.TicketStatus != TicketStatuses.Resolved &&
                ticket.TicketStatus != TicketStatuses.Closed;
        }

        private void ClearDetail()
        {
            TbTicketSubject.Text  = string.Empty;
            TbTicketMeta.Text     = string.Empty;
            LbReplies.ItemsSource = null;
            LbTasks.ItemsSource   = null;
            TbReplyText.Text      = string.Empty;
        }

        private void ShowListError(string msg)
        {
            if (string.IsNullOrWhiteSpace(msg))
            {
                TicketErrorBar.Visibility = Visibility.Collapsed;
            }
            else
            {
                TbTicketError.Text        = msg;
                TicketErrorBar.Visibility = Visibility.Visible;
            }
        }

        private void ShowSuccess(string msg)
        {
            TbSupportSuccess.Text       = msg;
            TbSupportSuccess.Visibility = Visibility.Visible;
            TbSupportError.Visibility   = Visibility.Collapsed;
        }

        private void ShowError(string msg)
        {
            TbSupportError.Text         = msg;
            TbSupportError.Visibility   = Visibility.Visible;
            TbSupportSuccess.Visibility = Visibility.Collapsed;
        }

        private void HideFeedback()
        {
            TbSupportSuccess.Visibility = Visibility.Collapsed;
            TbSupportError.Visibility   = Visibility.Collapsed;
        }
    }
}
