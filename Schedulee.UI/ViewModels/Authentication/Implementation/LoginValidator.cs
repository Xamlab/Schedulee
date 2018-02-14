using FluentValidation;
using Schedulee.UI.Resources.Strings.Authentication;
using Schedulee.UI.ViewModels.Base.Implementation;
using CommonStrings = Schedulee.UI.Resources.Strings.Common.Strings;
namespace Schedulee.UI.ViewModels.Authentication.Implementation
{
    internal class LoginValidator : BaseViewModelValidator<LoginViewModel>
    {
        public LoginValidator(LoginViewModel logiViewModel) : base(logiViewModel)
        {
            RuleFor(viewModel => viewModel.Email).NotEmpty()
                                                 .WithMessage(string.Format(CommonStrings.FieldRequired, Strings.Email))
                                                 .EmailAddress().WithMessage(Strings.InvalidEmail);

            RuleFor(viewModel => viewModel.Password).NotEmpty()
                                                    .WithMessage(string.Format(CommonStrings.FieldRequired, Strings.Password));
        }
    }
}
