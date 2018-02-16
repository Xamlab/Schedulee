using System.Threading;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using NSubstitute;
using Schedulee.Core.Models;
using Schedulee.Core.Services;
using Schedulee.UI.Resources.Strings.Authentication;
using Schedulee.UI.Tests.Base;
using Schedulee.UI.ViewModels.Authentication;
using Shouldly;
using TechTalk.SpecFlow;
using CommonStrings = Schedulee.UI.Resources.Strings.Common.Strings;

namespace Schedulee.UI.Tests.Authentication
{
    [Binding, Scope(Feature = "Login")]
    public class LoginTestSteps : BaseTestSteps
    {
        private ILoginViewModel _viewModel;
        private bool _didReturnAccountInfo;
        private Token _token;
        private IApiClient _apiClient;

        public enum EmailValidationError
        {
            FieldRequired,
            InvalidEmail
        }

        public enum PasswordValidationError
        {
            FieldRequired
        }

        public override void AfterScenarioCleanup()
        {
            _didReturnAccountInfo = false;
        }

        protected override void ConfigureServices()
        {
            base.ConfigureServices();
            _token = Builder<Token>.CreateNew().Build();
            _token.User = Builder<User>.CreateNew().Build();
            _apiClient = Substitute.For<IApiClient>();
            _apiClient.LoginAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(_token));
            Container.RegisterSingleton(_apiClient);
        }

        //Trying to log in while entering invalid email
        [Given(@"I have navigated to login page")]
        public void GivenIHaveNavigatedToLoginPage()
        {
            _viewModel = Container.Resolve<ILoginViewModel>();
        }

        [When(@"I try to log in")]
        public void WhenITryToLogIn()
        {
            _viewModel.SaveCommand.ExecuteAsync(null).Wait();
        }

        [Given(@"I have entered invalid email '(.*)'")]
        public void GivenIHaveEnteredInvalidEmail(string email)
        {
            switch(email)
            {
                case "Empty":
                    _viewModel.Email = "";
                    break;
                default:
                    _viewModel.Email = email;
                    break;
            }
        }

        [Then(@"I should see '(.*)' detailing the email validation issue")]
        public void ThenIShouldSeeDetailingTheEmailValidationIssue(EmailValidationError validationError)
        {
            string expectedMessage = "";
            switch(validationError)
            {
                case EmailValidationError.FieldRequired:
                    expectedMessage = string.Format(CommonStrings.FieldRequired, Strings.Email);
                    break;
                case EmailValidationError.InvalidEmail:
                    expectedMessage = Strings.InvalidEmail;
                    break;
            }

            CheckValidationError(_viewModel.Validator, nameof(ILoginViewModel.Email), expectedMessage);
        }

        //Trying to log in while entering invalid password
        [Given(@"I have entered invalid password '(.*)'")]
        public void GivenIHaveEnteredInvalidPassword(string password)
        {
            switch(password)
            {
                case "Empty":
                    _viewModel.Password = "";
                    break;
                default:
                    _viewModel.Password = password;
                    break;
            }
        }

        [Then(@"I should see '(.*)' detailing the password validation issue")]
        public void ThenIShouldSeeDetailingThePasswordValidationIssue(PasswordValidationError validationError)
        {
            string expectedMessage = "";
            switch(validationError)
            {
                case PasswordValidationError.FieldRequired:
                    expectedMessage = string.Format(CommonStrings.FieldRequired, Strings.Password);
                    break;
            }

            CheckValidationError(_viewModel.Validator, nameof(ILoginViewModel.Password), expectedMessage);
        }

        //Login stale properties
        [When(@"I modify '(.*)'")]
        public void WhenIModify(string property)
        {
            switch(property)
            {
                case nameof(ILoginViewModel.Email):
                    _viewModel.Email = "new@email.com";
                    break;
                case nameof(ILoginViewModel.Password):
                    _viewModel.Password = "new_password";
                    break;
            }
        }

        [Then(@"When I try to dismiss the screen I should see a warning notifying if I would like to discard pending changes")]
        public void ThenWhenITryToDismissTheScreenIShouldSeeAWarningNotifyingIfIWouldLikeToDiscardPendingChanges()
        {
            _viewModel.StaleMonitor.IsStale.ShouldBeTrue();
        }

        //Login completes successfully 
        [Given(@"I have entered valid email and password")]
        public void GivenIHaveEnteredValidEmailAndPassword()
        {
            _viewModel.Email = "test@test.com";
            _viewModel.Password = "password";
            _viewModel.LoginCompleted += ViewModelOnLoginCompleted;
        }

        [When(@"I try to log in and the process completes successfully")]
        public void WhenITryToLogInAndTheProcessCompletesSuccessfully()
        {
            _viewModel.SaveCommand.ExecuteAsync(null).Wait();
        }

        [Then(@"the application should be notified about successful login")]
        public void ThenTheApplicationShouldBeNotifiedAboutSuccessfulLogin()
        {
            _didReturnAccountInfo.ShouldBeTrue();
        }

        private void ViewModelOnLoginCompleted(object sender, Token token)
        {
            _didReturnAccountInfo = true;
        }
    }
}