using System.Collections.ObjectModel;
using AdminSystem.Models;
using AdminSystem.Services;

namespace AdminSystem.ViewModels
{
    public class SupportViewModel : BaseViewModel
    {
        private readonly ISupportService _supportService;

        public SupportViewModel(ISupportService supportService)
        {
            _supportService = supportService;
            Tickets = new ObservableCollection<SupportTicket>();

            LoadCommand    = new RelayCommand(Load);
            ReplyCommand   = new RelayCommand(AddReply,
                _ => SelectedTicket != null
                     && !string.IsNullOrWhiteSpace(ReplyText));
            ResolveCommand = new RelayCommand(ResolveTicket,
                _ => SelectedTicket != null
                     && SelectedTicket.TicketStatus != TicketStatuses.Resolved
                     && SelectedTicket.TicketStatus != TicketStatuses.Closed);
            AddTaskCommand = new RelayCommand(AddTask,
                _ => SelectedTicket != null
                     && !string.IsNullOrWhiteSpace(NewTaskType));
        }

        // ── Collections ─────────────────────────────────────────────────
        public ObservableCollection<SupportTicket> Tickets { get; }

        // ── Selected ────────────────────────────────────────────────────
        private SupportTicket _selectedTicket;
        public SupportTicket SelectedTicket
        {
            get { return _selectedTicket; }
            set
            {
                SetField(ref _selectedTicket, value, "SelectedTicket");
                LoadTicketDetail();
            }
        }

        private string _statusFilter;
        public string StatusFilter
        {
            get { return _statusFilter; }
            set { SetField(ref _statusFilter, value, "StatusFilter"); Load(); }
        }

        private string _replyText;
        public string ReplyText
        {
            get { return _replyText; }
            set { SetField(ref _replyText, value, "ReplyText"); }
        }

        private string _newTaskType;
        public string NewTaskType
        {
            get { return _newTaskType; }
            set { SetField(ref _newTaskType, value, "NewTaskType"); }
        }

        // ── Commands ────────────────────────────────────────────────────
        public RelayCommand LoadCommand    { get; }
        public RelayCommand ReplyCommand   { get; }
        public RelayCommand ResolveCommand { get; }
        public RelayCommand AddTaskCommand { get; }

        // ── Methods ─────────────────────────────────────────────────────
        public void Load()
        {
            IsLoading = true;
            ClearMessages();
            try
            {
                Tickets.Clear();
                System.Collections.Generic.IEnumerable<SupportTicket> result =
                    string.IsNullOrWhiteSpace(StatusFilter)
                        ? _supportService.GetAllTickets()
                        : _supportService.GetTicketsByStatus(StatusFilter);
                foreach (SupportTicket t in result) Tickets.Add(t);
            }
            catch (System.Exception ex) { ShowError(ex.Message); }
            finally { IsLoading = false; }
        }

        private void LoadTicketDetail()
        {
            if (_selectedTicket == null) return;
            try
            {
                SupportTicket detail =
                    _supportService.GetTicketById(_selectedTicket.TicketId);
                if (detail != null)
                    SetField(ref _selectedTicket, detail, "SelectedTicket");
            }
            catch (System.Exception ex) { ShowError(ex.Message); }
        }

        private void AddReply(object param)
        {
            if (SelectedTicket == null || string.IsNullOrWhiteSpace(ReplyText))
                return;
            ClearMessages();
            try
            {
                _supportService.AddReply(
                    SelectedTicket.TicketId, ReplyText.Trim(), true);
                ShowSuccess("Reply sent.");
                ReplyText = string.Empty;
                LoadTicketDetail();
            }
            catch (System.Exception ex) { ShowError(ex.Message); }
        }

        private void ResolveTicket(object param)
        {
            if (SelectedTicket == null) return;
            ClearMessages();
            try
            {
                _supportService.ResolveTicket(SelectedTicket.TicketId);
                ShowSuccess("Ticket resolved.");
                Load();
            }
            catch (System.Exception ex) { ShowError(ex.Message); }
        }

        private void AddTask(object param)
        {
            if (SelectedTicket == null || string.IsNullOrWhiteSpace(NewTaskType))
                return;
            ClearMessages();
            try
            {
                _supportService.AddTask(new SupportTask
                {
                    TicketId   = SelectedTicket.TicketId,
                    TaskType   = NewTaskType,
                    TaskStatus = SupportTaskStatuses.Pending
                });
                ShowSuccess("Task added.");
                NewTaskType = string.Empty;
                LoadTicketDetail();
            }
            catch (System.Exception ex) { ShowError(ex.Message); }
        }
    }
}
