using System.Threading.Tasks;

namespace Schedulee.UI.Services
{
    public interface IDialogService
    {
        Task ShowNotificationAsync(string message, string cancelText = "OK", string title = null);
        Task<bool> ShowConfirmationDialogAsync(string message, string confirmText, string cancelText);
        Task<bool> ShowConfirmationDialogAsync(string title, string message, string confirmText, string cancelText);
        Task<string> ShowOptionsAsync(string title, string cancel, string destruction, params string[] options);
    }
}