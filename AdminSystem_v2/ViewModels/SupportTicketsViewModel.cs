using AdminSystem_v2.Helpers;
using AdminSystem_v2.Models;
using AdminSystem_v2.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Linq;

namespace AdminSystem_v2.ViewModels
{
    public class SupportTicketsViewModel : BaseViewModel
    {
        private readonly ISupportTicketService _ticketService;
        private readonly IDialogService _dialogService;

        public ObservableCollection<SupportTicket> Tickets { get; set; } = new ObservableCollection<SupportTicket>();
        public ObservableCollection<SupportTicketReply> SelectedTicketReplies { get; set; } = new ObservableCollection<SupportTicketReply>();

        private SupportTicket? _selectedTicket;
        public SupportTicket? SelectedTicket
        {
            get => _selectedTicket;
            set
            {
                _selectedTicket = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsTicketSelected));
                _ = LoadRepliesAsync();
            }
        }

        public bool IsTicketSelected => SelectedTicket != null;

        private string _replyMessage = string.Empty;
        public string ReplyMessage
        {
            get => _replyMessage;
            set { _replyMessage = value; OnPropertyChanged(); }
        }

        public ICommand LoadTicketsCommand { get; }
        public ICommand ReplyCommand { get; }
        public ICommand MarkResolvedCommand { get; }

        public SupportTicketsViewModel(ISupportTicketService ticketService, IDialogService dialogService)
        {
            _ticketService = ticketService;
            _dialogService = dialogService;

            LoadTicketsCommand = new RelayCommand(async _ => await LoadTicketsAsync());
            ReplyCommand = new RelayCommand(async _ => await ReplyAsync(), _ => CanReply());
            MarkResolvedCommand = new RelayCommand(async _ => await MarkResolvedAsync(), _ => CanMarkResolved());
        }

        public async Task LoadTicketsAsync()
        {
            try
            {
                var ticketsRaw = await _ticketService.GetAllTicketsAsync();
                Tickets.Clear();
                foreach (var ticket in ticketsRaw)
                {
                    Tickets.Add(ticket);
                }
            }
            catch (System.Exception ex)
            {
                _dialogService.ShowInfo("Error loading tickets: " + ex.Message, "Error");
            }
        }

        private async Task LoadRepliesAsync()
        {
            if (SelectedTicket == null)
            {
                SelectedTicketReplies.Clear();
                return;
            }

            try
            {
                var repliesRaw = await _ticketService.GetRepliesAsync(SelectedTicket.TicketId);
                SelectedTicketReplies.Clear();
                foreach (var r in repliesRaw)
                {
                    SelectedTicketReplies.Add(r);
                }
            }
            catch (System.Exception ex)
            {
                _dialogService.ShowInfo("Error loading replies: " + ex.Message, "Error");
            }
        }

        private bool CanReply()
        {
            if (SelectedTicket == null) return false;
            if (string.IsNullOrWhiteSpace(ReplyMessage)) return false;
            return SelectedTicket.TicketStatus != "Closed" && SelectedTicket.TicketStatus != "Resolved";
        }

        private async Task ReplyAsync()
        {
            if (!CanReply()) return;

            var user = App.CurrentUser;
            if (user == null)
            {
                _dialogService.ShowInfo("You must be logged in to reply.", "Error");
                return;
            }

            try
            {
                await _ticketService.AddReplyAsync(SelectedTicket.TicketId, user.UserId, ReplyMessage);
                
                // Update Status to AwaitingResponse if it was Open
                await _ticketService.UpdateStatusAsync(SelectedTicket.TicketId, "AwaitingResponse", user.UserId);

                ReplyMessage = string.Empty;

                int ticketId = SelectedTicket.TicketId;
                await LoadTicketsAsync();
                var refreshed = Tickets.FirstOrDefault(t => t.TicketId == ticketId);
                if (refreshed != null)
                {
                    SelectedTicket = refreshed;
                }
                else
                {
                    await LoadRepliesAsync();
                }
            }
            catch (System.Exception ex)
            {
                _dialogService.ShowInfo("Error adding reply: " + ex.Message, "Error");
            }
        }

        private bool CanMarkResolved()
        {
            if (SelectedTicket == null) return false;
            return SelectedTicket.TicketStatus != "Closed" && SelectedTicket.TicketStatus != "Resolved";
        }

        private async Task MarkResolvedAsync()
        {
            if (!CanMarkResolved()) return;

            var user = App.CurrentUser;
            if (user == null) return;

            if (_dialogService.Confirm("Are you sure you want to mark this ticket as resolved?", "Resolve Ticket"))
            {
                try
                {
                    int ticketId = SelectedTicket.TicketId;
                    await _ticketService.ResolveTicketAsync(ticketId, user.UserId);
                    await LoadTicketsAsync();
                    var refreshed = Tickets.FirstOrDefault(t => t.TicketId == ticketId);
                    if (refreshed != null)
                    {
                        SelectedTicket = refreshed;
                    }
                }
                catch (System.Exception ex)
                {
                    _dialogService.ShowInfo("Error resolving ticket: " + ex.Message, "Error");
                }
            }
        }
    }
}
