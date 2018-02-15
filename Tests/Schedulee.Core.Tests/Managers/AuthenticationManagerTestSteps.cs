using System;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using Schedulee.Core.Extensions.PubSub;
using Schedulee.Core.Managers;
using Schedulee.Core.Managers.Implementation;
using Schedulee.Core.Messages;
using Schedulee.Core.Models;
using Schedulee.Core.Services;
using Shouldly;
using TechTalk.SpecFlow;

namespace Schedulee.Core.Tests.Managers
{
    [Binding, Scope(Feature = "Authentication Manager")]
    public class AuthenticationManagerTestSteps
    {
        private IAuthenticationManager _authManager;
        private IAuthenticationService _authService;
        private MockSecureSettingsManager _secureSettings;
        private Token _token;
        private bool _didLogIn;
        private bool _didLogOut;

        [BeforeScenario]
        public void Setup()
        {
            this.Subscribe<SessionStateChangedMessage>(OnSessionStateChanged);
        }

        private void OnSessionStateChanged(SessionStateChangedMessage state)
        {
            _didLogIn = state.State == SessionState.LoggedIn;
            _didLogOut = state.State == SessionState.LoggedOut;
        }

        [AfterScenario]
        public void Cleanup()
        {
            _didLogIn = false;
            _didLogOut = false;
        }

        //Sign in process completes successfully

        [When(@"sign in process completes successfully")]
        public void WhenSignInProcessCompletesSuccessfully()
        {
            SignIn();
        }

        [Then(@"The user account should be stored in the secure storage")]
        public void ThenTheUserAccountShouldBeStoredInTheSecureStorage()
        {
            var account = _secureSettings.GetAccount();
            account.AccessToken.ShouldBe(_token.AccessToken);
            account.RefreshToken.ShouldBe(_token.RefreshToken);
            account.Created.ShouldBe(_token.Created);
            account.ExpiresIn.ShouldBe(_token.ExpiresIn);
            account.User.Id.ShouldBe(_token.User.Id);
            account.User.FirstName.ShouldBe(_token.User.FirstName);
            account.User.LastName.ShouldBe(_token.User.LastName);
            account.User.Email.ShouldBe(_token.User.Email);
            account.User.ProfilePictureUrl.ShouldBe(_token.User.ProfilePictureUrl);
        }

        [Then(@"session state should be changed to LoggedIn")]
        public void ThenSessionStateShouldBeChangedToLoggedIn()
        {
            _authManager.State.ShouldBe(SessionState.LoggedIn);
        }

        [Then(@"application should be notified that session state was changed to LoggedIn")]
        public void ThenApplicationShouldBeNotifiedThatSessionStateWasChangedToLoggedIn()
        {
            _didLogIn.ShouldBeTrue();
        }

        //Sign in process completes and no account is returned
        [When(@"Sign in process completes and no account is returned")]
        public void WhenSignInProcessCompletesAndNoAccountIsReturned()
        {
            _authService = Substitute.For<IAuthenticationService>();
            _authService.Authenticate().Returns(Task.FromResult((Token) null));
            _secureSettings = new MockSecureSettingsManager();
            _authManager = new AuthenticationManager(_authService, _secureSettings);
            _authManager.SignIn().Wait();
        }

        [Then(@"session state should be changed to LoggedOut")]
        public void ThenSessionStateShouldBeChangedToLoggedOut()
        {
            _authManager.State.ShouldBe(SessionState.LoggedOut);
        }

        //Sign in process fails due to exception
        [When(@"Sign in process fails due to exception")]
        public void WhenSignInProcessFailsDueToException()
        {
            _authService = Substitute.For<IAuthenticationService>();
            _authService.Authenticate().Throws(new Exception());
            _secureSettings = new MockSecureSettingsManager();
            _authManager = new AuthenticationManager(_authService, _secureSettings);
        }

        [Then(@"Exception should be re-thrown")]
        public void ThenExceptionShouldBeRe_Thrown()
        {
            Assert.That(() => _authManager.SignIn().Wait(), Throws.Exception);
        }

        //Signing out
        [Given(@"I am signed in")]
        public void GivenIAmSignedIn()
        {
            SignIn();
        }

        [When(@"I try to sign out")]
        public void WhenITryToSignOut()
        {
            _authManager.SignOut();
        }

        [Then(@"account information should be cleared")]
        public void ThenAccountInformationShouldBeCleared()
        {
            _secureSettings.GetAccount().ShouldBeNull();
        }

        [Then(@"application should be notified that session state was changed to LoggedOut")]
        public void ThenApplicationShouldBeNotifiedThatSessionStateWasChangedToLoggedOut()
        {
            _didLogOut.ShouldBeTrue();
        }

        //Restoring session when there is stored session
        [Given(@"There is stored session")]
        public void GivenThereIsStoredSession()
        {
            CreateValidAuthenticationServices();
            _secureSettings.SetAccount(_token);
        }

        [When(@"I try to restore the session")]
        public void WhenITryToRestoreTheSession()
        {
            _authManager.RestoreSession();
        }

        //Restoring session when there is no stored session
        [Given(@"There is no stored session")]
        public void GivenThereIsNoStoredSession()
        {
            CreateValidAuthenticationServices();
            _secureSettings.Clear();
        }

        private void SignIn()
        {
            CreateValidAuthenticationServices();
            _authManager.SignIn().Wait();
        }

        private void CreateValidAuthenticationServices()
        {
            _token = Builder<Token>.CreateNew().Build();
            _token.User = Builder<User>.CreateNew().Build();
            _authService = Substitute.For<IAuthenticationService>();
            _authService.Authenticate().Returns(Task.FromResult(_token));
            _secureSettings = new MockSecureSettingsManager();
            _authManager = new AuthenticationManager(_authService, _secureSettings);
        }
    }
}