using System;
using System.Threading;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using NSubstitute;
using Schedulee.Core.Managers;
using Schedulee.Core.Models;
using Schedulee.Core.Services;
using Schedulee.UI.Tests.Base;
using Schedulee.UI.ViewModels.Settings;
using Shouldly;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Schedulee.UI.Tests.Settings
{
    [Binding, Scope(Feature = "Settings")]
    public class SettingsTestSteps : BaseTestSteps
    {
        private ISettingsViewModel _viewModel;
        private Token _token;
        private MockSecureSettingsManager _settingsManager;
        private IApiClient _apiClient;
        private User _expectedUser;
        protected override void ConfigureServices()
        {
            base.ConfigureServices();

            _settingsManager = new MockSecureSettingsManager();
            _token = Builder<Token>.CreateNew().Build();
            _settingsManager.SetAccount(_token);
            Container.RegisterSingleton<ISecureSettingsManager>(_settingsManager);

            _apiClient = Substitute.For<IApiClient>();
            _apiClient.SaveAccountAsync(Arg.Any<string>(), 
                                        Arg.Any<string>(), 
                                        Arg.Any<string>(), 
                                        Arg.Any<int>(), 
                                        Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
            Container.RegisterSingleton(_apiClient);
        }

        //Initial loading of settings page
        [Given(@"I am logged in as")]
        public void GivenIAmLoggedInAs(Table table)
        {
            _token.User = table.CreateInstance<User>();
        }
        
        [Given(@"I have navigated to settings page")]
        public void GivenIHaveNavigatedToSettingsPage()
        {
            _viewModel = Container.Resolve<ISettingsViewModel>();
            _viewModel.SetupCommand.Execute(null);
        }
        
        [Then(@"I should see the user data populated correctly")]
        public void ThenIShouldSeeTheUserDataPopulatedCorrectly()
        {
            _viewModel.FirstName.ShouldBe(_token.User.FirstName);
            _viewModel.LastName.ShouldBe(_token.User.LastName);
            _viewModel.Username.ShouldBe(_token.User.Email);
            _viewModel.Location.ShouldBe(_token.User.Location);
            _viewModel.SetTravelTime.ShouldBe(_token.User.SetTravelTime);
        }

        //Trying to save settings while entering invalid data
        [Given(@"I have entered invalid '(.*)' into '(.*)'")]
        public void GivenIHaveEnteredInvalidInto(string data, string field)
        {
            switch(field)
            {
                case nameof(ISettingsViewModel.FirstName):
                    _viewModel.FirstName = data == "Empty" ? "" : data;
                    break;
                case nameof(ISettingsViewModel.LastName):
                    _viewModel.LastName = data == "Empty" ? "" : data;
                    break;
                case nameof(ISettingsViewModel.Location):
                    _viewModel.Location = data == "Empty" ? "" : data;
                    break;
                case nameof(ISettingsViewModel.SetTravelTime):
                    _viewModel.SetTravelTime = int.Parse(data);
                    break;
            }
        }

        [When(@"I try to save changes")]
        public void WhenITryToSaveChanges()
        {
            _viewModel.SaveCommand.ExecuteAsync(null).Wait();
        }

        [Then(@"I should see '(.*)' detailing the '(.*)' validation issue for '(.*)'")]
        public void ThenIShouldSeeDetailingTheValidationIssueFor(string validationError, string data, string field)
        {
            CheckValidationError(_viewModel.Validator, field, validationError);
        }

        //Settings stale properties
        [When(@"I modify '(.*)' with '(.*)'")]
        public void WhenIModifyWith(string field, string value)
        {
            switch (field)
            {
                case nameof(ISettingsViewModel.FirstName):
                    _viewModel.FirstName = value;
                    break;
                case nameof(ISettingsViewModel.LastName):
                    _viewModel.LastName = value;
                    break;
                case nameof(ISettingsViewModel.Location):
                    _viewModel.Location = value;
                    break;
                case nameof(ISettingsViewModel.SetTravelTime):
                    _viewModel.SetTravelTime = int.Parse(value);
                    break;
            }
        }

        [Then(@"When I try to dismiss the screen I should see a warning notifying if I would like to discard pending changes")]
        public void ThenWhenITryToDismissTheScreenIShouldSeeAWarningNotifyingIfIWouldLikeToDiscardPendingChanges()
        {
            _viewModel.StaleMonitor.IsStale.ShouldBeTrue();
        }

        //
        [Given(@"I have updated my user information with this data")]
        public void GivenIHaveUpdatedMyUserInformationWithThisData(Table table)
        {
            _expectedUser = table.CreateInstance<User>();
            _viewModel.FirstName = _expectedUser.FirstName;
            _viewModel.LastName = _expectedUser.LastName;
            _viewModel.Location = _expectedUser.Location;
            _viewModel.SetTravelTime = _expectedUser.SetTravelTime;
        }

        [When(@"I save the data and saving completes successfully")]
        public void WhenISaveTheDataAndSavingCompletesSuccessfully()
        {
            _viewModel.SaveCommand.ExecuteAsync(null).Wait();
        }

        [Then(@"the current user information should be updated")]
        public void ThenTheCurrentUserInformationShouldBeUpdated()
        {
            var user = _settingsManager.GetAccount().User;
            user.FirstName.ShouldBe(_expectedUser.FirstName);
            user.LastName.ShouldBe(_expectedUser.LastName);
            user.Location.ShouldBe(_expectedUser.Location);
            user.SetTravelTime.ShouldBe(_expectedUser.SetTravelTime);
        }

    }
}
