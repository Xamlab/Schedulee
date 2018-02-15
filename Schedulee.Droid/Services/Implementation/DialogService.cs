using System.Threading.Tasks;
using Acr.UserDialogs;
using Schedulee.UI.Services;

namespace Schedulee.Droid.Services.Implementation
{
    public class DialogService : IDialogService
    {
        public Task ShowNotificationAsync(string message, string cancelText = "OK", string title = null)
        {
            return UserDialogs.Instance.AlertAsync(message, title, cancelText);
        }

        public Task<bool> ShowConfirmationDialogAsync(string message, string confirmText, string cancelText)
        {
            return UserDialogs.Instance.ConfirmAsync(message, okText:confirmText, cancelText:cancelText);
        }

        public Task<bool> ShowConfirmationDialogAsync(string title, string message, string confirmText, string cancelText)
        {
            return UserDialogs.Instance.ConfirmAsync(message, title, confirmText, cancelText);
        }

        public Task<string> ShowOptionsAsync(string title, string cancel, string destruction, params string[] options)
        {
            return UserDialogs.Instance.ActionSheetAsync(title, cancel, destruction, buttons:options);
        }
    }
}