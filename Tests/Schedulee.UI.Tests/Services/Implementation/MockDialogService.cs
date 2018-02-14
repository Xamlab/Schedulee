using System.Threading.Tasks;
using Schedulee.UI.Services;

namespace Schedulee.UI.Tests.Services.Implementation
{
    public class MockDialogService : IDialogService
    {
        public string DialogMessage { get; private set; }
        public bool ShouldConfirmDialog { get; set; }
        public string[] Options { get; set; }
        public int SelectedOption { get; set; }

        public void SetRootPage(object rootPage)
        {
        }

        public Task ShowNotificationAsync(string message, string cancelText = "OK", string title = null)
        {
            DialogMessage = message;
            return Task.CompletedTask;
        }

        public Task<bool> ShowConfirmationDialogAsync(string message, string confirmText, string cancelText)
        {
            DialogMessage = message;
            return Task.FromResult(ShouldConfirmDialog);
        }

        public Task<bool> ShowConfirmationDialogAsync(string title, string message, string confirmText, string cancelText)
        {
            DialogMessage = message;
            return Task.FromResult(ShouldConfirmDialog);
        }

        public Task<string> ShowOptionsAsync(string title, string cancel, string destruction, params string[] options)
        {
            DialogMessage = title;
            Options = options;
            return Task.FromResult(Options[SelectedOption]);
        }
    }
}