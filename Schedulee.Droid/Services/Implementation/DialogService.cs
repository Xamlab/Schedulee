using System.Threading.Tasks;
using Acr.UserDialogs;
using Plugin.CurrentActivity;
using Schedulee.Droid.Controls;
using Schedulee.UI.Services;

namespace Schedulee.Droid.Services.Implementation
{
    public class DialogService : IDialogService
    {
        public async Task ShowNotificationAsync(string message, string cancelText = "OK", string title = null)
        {
            var dialog = new ScheduleeNotificationDialog(CrossCurrentActivity.Current.Activity);
            await dialog.ShowAsync(title, message, cancelText);
            dialog.Dispose();
        }

        public Task<bool> ShowConfirmationDialogAsync(string message, string confirmText, string cancelText)
        {
            return ShowConfirmationDialogAsync(null, message, confirmText, cancelText);
        }

        public async Task<bool> ShowConfirmationDialogAsync(string title, string message, string confirmText, string cancelText)
        {
            var dialog = new ScheduleeConfirmationDialog(CrossCurrentActivity.Current.Activity);
            var result = await dialog.ShowAsync(title, message, cancelText, confirmText);
            dialog.Dispose();
            return result;
        }

        public Task<string> ShowOptionsAsync(string title, string cancel, string destruction, params string[] options)
        {
            return UserDialogs.Instance.ActionSheetAsync(title, cancel, destruction, buttons:options);
        }
    }
}