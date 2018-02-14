using System.Threading.Tasks;
using Schedulee.UI.Resources.Strings.Common;
using Schedulee.UI.Services;

namespace Schedulee.UI.ViewModels.Base.Implementation
{
    public static class ViewModelExtensions
    {
        public static async Task<bool> ValidateAndShowErrorsAsync(this IViewModelValidator validator, IDialogService dialogService)
        {
            if(validator != null && !validator.Validate())
            {
                var errors = validator.GetAllErrorsInString();
                var message = Strings.FixValidationErrors + "\n" + string.Join("\n", errors);
                await dialogService.ShowNotificationAsync(message, Strings.Ok);
                return false;
            }

            return true;
        }
    }
}
