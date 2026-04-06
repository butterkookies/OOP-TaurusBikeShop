namespace AdminSystem_v2.Services
{
    public interface IDialogService
    {
        /// <summary>Shows a Yes/No confirmation dialog. Returns true if the user clicked Yes.</summary>
        bool Confirm(string message, string title);

        /// <summary>Shows a modal info message.</summary>
        void ShowInfo(string message, string title);
    }
}
