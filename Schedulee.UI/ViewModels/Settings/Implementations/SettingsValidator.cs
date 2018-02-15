using FluentValidation;
using Schedulee.UI.Resources.Strings.Settings;
using Schedulee.UI.ViewModels.Base.Implementation;
using CommonStrings = Schedulee.UI.Resources.Strings.Common.Strings;

namespace Schedulee.UI.ViewModels.Settings.Implementations
{
    internal class SettingsValidator : BaseViewModelValidator<SettingsViewModel>
    {
        public SettingsValidator(SettingsViewModel settingsViewModel) : base(settingsViewModel)
        {
            RuleFor(viewModel => viewModel.FirstName).NotEmpty()
                                                     .WithMessage(string.Format(CommonStrings.FieldRequired, Strings.FirstName));
            RuleFor(viewModel => viewModel.LastName).NotEmpty()
                                                    .WithMessage(string.Format(CommonStrings.FieldRequired, Strings.LastName));
            RuleFor(viewModel => viewModel.Location).NotEmpty()
                                                    .WithMessage(string.Format(CommonStrings.FieldRequired, Strings.Location));
            RuleFor(viewModel => viewModel.SetTravelTime).GreaterThan(0)
                                                         .WithMessage(Strings.SetTravelTimeValidationError);
        }
    }
}