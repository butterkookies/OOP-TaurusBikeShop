using AdminSystem_v2.Helpers;
using AdminSystem_v2.Models;
using AdminSystem_v2.Services;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace AdminSystem_v2.ViewModels
{
    public class SupportTicketsViewModel : BaseViewModel
    {
        private readonly ISupportTicketService _ticketService;
        private readonly IDialogService _dialogService;

        // ── Backing collection (never bound to directly) ──────────────────────
        private readonly ObservableCollection<SupportTicket> _allTickets = new();

        // ── Filtered view (ListView binds here) ───────────────────────────────
        public ICollectionView FilteredTickets { get; }

        // ── Replies ───────────────────────────────────────────────────────────
        public ObservableCollection<SupportTicketReply> SelectedTicketReplies { get; } = new();

        // ── Selected ticket ───────────────────────────────────────────────────
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

        // ── Search ────────────────────────────────────────────────────────────
        private string _searchQuery = string.Empty;
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged();
                FilteredTickets.Refresh();
                OnPropertyChanged(nameof(FilteredCount));
            }
        }

        /// <summary>Number of tickets currently visible (after filtering).</summary>
        public int FilteredCount => FilteredTickets.Cast<SupportTicket>().Count();

        // ── Reply composer ────────────────────────────────────────────────────
        private string _replyMessage = string.Empty;
        public string ReplyMessage
        {
            get => _replyMessage;
            set { _replyMessage = value; OnPropertyChanged(); }
        }

        // ── Commands ──────────────────────────────────────────────────────────
        public ICommand LoadTicketsCommand { get; }
        public ICommand ReplyCommand { get; }
        public ICommand MarkResolvedCommand { get; }
        public ICommand ClearSearchCommand { get; }

        // ── Constructor ───────────────────────────────────────────────────────
        public SupportTicketsViewModel(ISupportTicketService ticketService, IDialogService dialogService)
        {
            _ticketService = ticketService;
            _dialogService = dialogService;

            // Build the filtered view over _allTickets
            FilteredTickets = CollectionViewSource.GetDefaultView(_allTickets);
            FilteredTickets.Filter = FilterPredicate;
            ((INotifyCollectionChanged)FilteredTickets).CollectionChanged += (_, _) =>
                OnPropertyChanged(nameof(FilteredCount));

            LoadTicketsCommand  = new RelayCommand(async _ => await LoadTicketsAsync());
            ReplyCommand        = new RelayCommand(async _ => await ReplyAsync(), _ => CanReply());
            MarkResolvedCommand = new RelayCommand(async _ => await MarkResolvedAsync(), _ => CanMarkResolved());
            ClearSearchCommand  = new RelayCommand(_ => SearchQuery = string.Empty);
        }

        // ── Filter predicate ──────────────────────────────────────────────────
        private bool FilterPredicate(object item)
        {
            if (item is not SupportTicket ticket) return false;
            if (string.IsNullOrWhiteSpace(_searchQuery)) return true;

            var q = _searchQuery.Trim();
            return Contains(ticket.Subject, q)
                || Contains(ticket.UserFullName, q)
                || Contains(ticket.UserEmail, q)
                || Contains(ticket.TicketCategory, q)
                || Contains(ticket.TicketStatus, q)
                || Contains(ticket.Description, q);

            static bool Contains(string? source, string query) =>
                source?.Contains(query, System.StringComparison.OrdinalIgnoreCase) == true;
        }

        // ── Data loading ──────────────────────────────────────────────────────
        public async Task LoadTicketsAsync()
        {
            try
            {
                var ticketsRaw = await _ticketService.GetAllTicketsAsync();
                _allTickets.Clear();
                foreach (var ticket in ticketsRaw)
                    _allTickets.Add(ticket);

                FilteredTickets.Refresh();
                OnPropertyChanged(nameof(FilteredCount));
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
                    SelectedTicketReplies.Add(r);
            }
            catch (System.Exception ex)
            {
                _dialogService.ShowInfo("Error loading replies: " + ex.Message, "Error");
            }
        }

        // ── Reply ─────────────────────────────────────────────────────────────
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
                await _ticketService.AddReplyAsync(SelectedTicket!.TicketId, user.UserId, ReplyMessage);
                await _ticketService.UpdateStatusAsync(SelectedTicket.TicketId, "AwaitingResponse", user.UserId);

                ReplyMessage = string.Empty;

                int ticketId = SelectedTicket.TicketId;
                await LoadTicketsAsync();
                var refreshed = _allTickets.FirstOrDefault(t => t.TicketId == ticketId);
                if (refreshed != null)
                    SelectedTicket = refreshed;
                else
                    await LoadRepliesAsync();
            }
            catch (System.Exception ex)
            {
                _dialogService.ShowInfo("Error adding reply: " + ex.Message, "Error");
            }
        }

        // ── Resolve ───────────────────────────────────────────────────────────
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
                    int ticketId = SelectedTicket!.TicketId;
                    await _ticketService.ResolveTicketAsync(ticketId, user.UserId);
                    await LoadTicketsAsync();
                    var refreshed = _allTickets.FirstOrDefault(t => t.TicketId == ticketId);
                    if (refreshed != null)
                        SelectedTicket = refreshed;
                }
                catch (System.Exception ex)
                {
                    _dialogService.ShowInfo("Error resolving ticket: " + ex.Message, "Error");
                }
            }
        }
    }
}
