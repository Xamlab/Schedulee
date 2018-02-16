using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using NSubstitute;
using Schedulee.Core.Extensions;
using Schedulee.Core.Models;
using Schedulee.Core.Services;
using Schedulee.UI.Tests.Base;
using Schedulee.UI.Tests.Extensions;
using Schedulee.UI.ViewModels.Availability;
using Schedulee.UI.ViewModels.Availability.Implementation;
using Shouldly;
using TechTalk.SpecFlow;

namespace Schedulee.UI.Tests.Availability
{
    [Binding, Scope(Feature = "Set Availability")]
    public class SetAvailabilityTestSteps : BaseTestSteps
    {
        private ISetAvailabilityViewModel _viewModel;
        private List<UserAvailablity> _userAvailabilities;
        private List<Tuple<string, string>> _expectedAvailabilities;
        private readonly Regex _periodRegEx = new Regex("(\\d\\d:\\d\\d) - (\\d\\d:\\d\\d)");
        private bool _didSaveAvailability;

        protected override void ConfigureServices()
        {
            var apiClient = Substitute.For<IApiClient>();
            apiClient.FetchUserAvailablities(Arg.Any<CancellationToken>()).Returns(info => Task.FromResult((IEnumerable<UserAvailablity>) _userAvailabilities));
            apiClient.CreateAvailabilityAsync(Arg.Any<UserAvailablity>(), Arg.Any<CancellationToken>()).Returns(info =>
                                                                                                                {
                                                                                                                    _didSaveAvailability = true;
                                                                                                                    return Task.CompletedTask;
                                                                                                                });
            Container.RegisterSingleton(apiClient);
        }

        public override void AfterScenarioCleanup()
        {
            _didSaveAvailability = false;
        }

        //Loading user availability data
        [Given(@"I have some availability data already stored")]
        public void GivenIHaveSomeAvailabilityDataAlreadyStored(Table table)
        {
            Load(table);
        }

        [Given(@"I have navigated to set availability page")]
        public void GivenIHaveNavigatedToSetAvailabilityPage()
        {
            _viewModel = Container.Resolve<ISetAvailabilityViewModel>();
        }

        [When(@"the page completes loading")]
        public void WhenThePageCompletesLoading()
        {
            _viewModel.LoadCommand.ExecuteAsync(null);
        }

        [Then(@"I should see the correct availability information")]
        public void ThenIShouldSeeTheCorrectAvailabilityInformation()
        {
            var actual = _viewModel.Items.ToArray();
            var expected = _expectedAvailabilities.ToArray();
            actual.Length.ShouldBe(expected.Length);
            for(int i = 0; i < actual.Length; i++)
            {
                actual[i].FormattedDaysOfWeek.ShouldBe(expected[i].Item1);
                actual[i].FormattedTimePeriods.ShouldBe(expected[i].Item2);
            }
        }

        //Starting adding availability process
        [When(@"I start adding availability process")]
        public void WhenIStartAddingAvailabilityProcess()
        {
            _viewModel.AddTimeAvailableCommand.Execute(null);
        }

        [Then(@"I should no longer be able to start another add availability process until this one finishes")]
        public void ThenIShouldNoLongerBeAbleToStartAnotherAddAvailabilityProcessUntilThisOneFinishes()
        {
            Misc.Retry(() => _viewModel.AddTimeAvailableCommand.CanExecute(null).ShouldBe(false));
        }

        [Then(@"I should see the week days, and none should be selected")]
        public void ThenIShouldSeeTheWeekDaysAndNoneShouldBeSelected(Table table)
        {
            var expectedDays = table.Rows[0].Values.ElementAt(0).Split(',').Select(day => day.Trim()).ToArray();
            var actualDays = _viewModel.DaysOfWeek.Select(day => day.FormattedDay).ToArray();
            expectedDays.Length.ShouldBe(actualDays.Length);
            for(int i = 0; i < expectedDays.Length; i++)
            {
                actualDays[i].ShouldBe(expectedDays[i]);
            }

            foreach(var daysOfWeek in _viewModel.DaysOfWeek)
            {
                daysOfWeek.IsSelected.ShouldBeFalse();
            }
        }

        [Then(@"I Should already be able to perform following availability manipulation operations")]
        public void ThenIShouldAlreadyBeAbleToPerformFollowingAvailabilityManipulationOperations(Table table)
        {
            foreach(var tableRow in table.Rows)
            {
                var command = (ICommand) _viewModel.GetPropertyValue(tableRow.Values.ElementAt(0));
                command.CanExecute(null).ShouldBeTrue();
            }
        }

        [Then(@"view should be marked as non-stale")]
        public void ThenViewShouldBeMarkedAsNon_Stale()
        {
            _viewModel.StaleMonitor.IsStale.ShouldBeFalse();
        }

        //Trying to start adding availability process while there's another in progress
        [Given(@"I have started adding availability process")]
        public void GivenIHaveStartedAddingAvailabilityProcess()
        {
            _viewModel.AddTimeAvailableCommand.Execute(null);
        }

        [When(@"I try to start adding availability process one more time")]
        public void WhenITryToStartAddingAvailabilityProcessOneMoreTime()
        {
            _viewModel.AddTimeAvailableCommand.Execute(null);
        }

        [Then(@"I should see the following message")]
        public void ThenIShouldSeeTheFollowingMessage(Table table)
        {
            DialogService.DialogMessage.ShouldBe(table.Rows[0].Values.ElementAt(0));
        }

        //Trying to perform availability relating operation while availability process isn't started
        [When(@"I try to perform availability related '(.*)'")]
        public void WhenITryToPerformAvailabilityRelated(string operation)
        {
            var command = (ICommand) _viewModel.GetPropertyValue(operation);
            command.Execute(null);
        }

        //Adding new time period
        [When(@"I try to add following time period")]
        public void WhenITryToAddFollowingTimePeriod(Table table)
        {
            AddTimePeriod(table);
        }

        [Then(@"the time period should be added to the list")]
        public void ThenTheTimePeriodShouldBeAddedToTheList()
        {
            var expected = GetTimePeriod();
            _viewModel.TimePeriods.FirstOrDefault(period => period.Start == expected.Start && period.End == expected.End)
                      .ShouldNotBeNull();
        }

        //Deleting time period
        [Given(@"I have added following time period")]
        public void GivenIHaveAddedFollowingTimePeriod(Table table)
        {
            AddTimePeriod(table);
        }

        [When(@"I try to delete that time period")]
        public void WhenITryToDeleteThatTimePeriod()
        {
            var expected = GetTimePeriod();
            var timePeriod = _viewModel.TimePeriods.First(period => period.Start == expected.Start && period.End == expected.End);
            _viewModel.DeleteTimePeriodCommand.Execute(timePeriod);
        }

        [Then(@"the time period should be removed from the list")]
        public void ThenTheTimePeriodShouldBeRemovedFromTheList()
        {
            var expected = GetTimePeriod();
            _viewModel.TimePeriods.FirstOrDefault(period => period.Start == expected.Start && period.End == expected.End)
                      .ShouldBeNull();
        }

        //Selecting day of week
        [When(@"I try to select following day of week")]
        public void WhenITryToSelectFollowingDayOfWeek(Table table)
        {
            ToggleDayOfWeek(table);
        }

        [Then(@"That day of week should be selected")]
        public void ThenThatDayOfWeekShouldBeSelected()
        {
            var dayOfWeek = GetDayOfWeek();
            dayOfWeek.IsSelected.ShouldBeTrue();
        }

        //Deselecting day of week
        [Given(@"I have already selected following day of week")]
        public void GivenIHaveAlreadySelectedFollowingDayOfWeek(Table table)
        {
            ToggleDayOfWeek(table);
        }

        [When(@"I try to deselect that day")]
        public void WhenITryToDeselectThatDay()
        {
            var dayOfWeek = GetDayOfWeek();
            _viewModel.ToggleDayCommand.Execute(dayOfWeek);
        }

        [Then(@"That day of week should be deselected")]
        public void ThenThatDayOfWeekShouldBeDeselected()
        {
            var dayOfWeek = GetDayOfWeek();
            dayOfWeek.IsSelected.ShouldBeFalse();
        }

        //Try to cancel adding availability process
        [When(@"I cancel the adding availability process")]
        public void WhenICancelTheAddingAvailabilityProcess()
        {
            _viewModel.CancelCommand.Execute(null);
        }

        //Confirming the cancellation of adding availability process
        [When(@"I cancel the adding availability process and I confirm that I want to discard the changes")]
        public void WhenICancelTheAddingAvailabilityProcessAndIConfirmThatIWantToDiscardTheChanges()
        {
            DialogService.ShouldConfirmDialog = true;
            _viewModel.CancelCommand.Execute(null);
        }

        [Then(@"all added time periods should be cleared")]
        public void ThenAllAddedTimePeriodsShouldBeCleared()
        {
            (_viewModel.TimePeriods?.Any() ?? false).ShouldBeFalse();
        }

        [Then(@"all days of week should be deselected")]
        public void ThenAllDaysOfWeekShouldBeDeselected()
        {
            foreach(var daysOfWeek in _viewModel.DaysOfWeek)
            {
                daysOfWeek.IsSelected.ShouldBeFalse();
            }
        }

        [Then(@"I should be able to start another adding availability process")]
        public void ThenIShouldBeAbleToStartAnotherAddingAvailabilityProcess()
        {
            _viewModel.AddTimeAvailableCommand.CanExecute(null).ShouldBeTrue();
        }

        //Saving the availability information
        [When(@"I save the availability information")]
        public void WhenISaveTheAvailabilityInformation()
        {
            _viewModel.SaveCommand.ExecuteAsync(null).Wait();
        }

        [Then(@"Availability should be saved in database")]
        public void ThenAvailabilityShouldBeSavedInDatabase()
        {
            _didSaveAvailability.ShouldBeTrue();
        }

        [Then(@"The newly created availability should be added to the list of availabilities")]
        public void ThenTheNewlyCreatedAvailabilityShouldBeAddedToTheListOfAvailabilities()
        {
            var dayOfWeek = GetDayOfWeek();
            var timePeriod = GetTimePeriod();
            var availability = _viewModel.Items.FirstOrDefault();
            availability.ShouldNotBeNull();
            availability.Availablity.DaysOfWeek[0].ShouldBe(dayOfWeek.Day);
            availability.Availablity.TimePeriods[0].Start.ShouldBe(timePeriod.Start);
            availability.Availablity.TimePeriods[0].End.ShouldBe(timePeriod.End);
        }

        ///////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////

        //Trying to add time availability while entering invalid data
        [Given(@"the availability information is loaded")]
        public void GivenTheAvailabilityInformationIsLoaded(Table table)
        {
            Load(table);
            _viewModel.LoadCommand.ExecuteAsync(null).Wait();
        }

        [Given(@"I have entered invalid '(.*)' into '(.*)'")]
        public void GivenIHaveEnteredInvalidInto(string data, string field)
        {
            switch(field)
            {
                case nameof(ISetAvailabilityViewModel.TimePeriods):
                    foreach(var timePeriod in _viewModel.TimePeriods)
                    {
                        _viewModel.DeleteTimePeriodCommand.Execute(timePeriod);
                    }

                    break;
                case nameof(ISetAvailabilityViewModel.DaysOfWeek):
                    if(data == "NoDateSelected")
                    {
                        foreach(var dayOfWeek in _viewModel.DaysOfWeek)
                        {
                            ((DayOfWeekViewModel) dayOfWeek).IsSelected = false;
                        }
                    }

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
        [When(@"I modify '(.*)'")]
        public void WhenIModify(string field)
        {
            switch(field)
            {
                case nameof(ISetAvailabilityViewModel.TimePeriods):
                    _viewModel.AddTimePeriodCommand.Execute(new TimePeriod(DateTime.Now, DateTime.Now.AddHours(1)));
                    break;
                case nameof(ISetAvailabilityViewModel.DaysOfWeek):
                    _viewModel.ToggleDayCommand.Execute(_viewModel.DaysOfWeek.First());
                    break;
            }
        }

        [Then(@"When I try to dismiss the screen I should see a warning notifying if I would like to discard pending changes")]
        public void ThenWhenITryToDismissTheScreenIShouldSeeAWarningNotifyingIfIWouldLikeToDiscardPendingChanges()
        {
            _viewModel.StaleMonitor.IsStale.ShouldBeTrue();
        }

        private void Load(Table table)
        {
            _userAvailabilities = new List<UserAvailablity>();
            _expectedAvailabilities = new List<Tuple<string, string>>();
            foreach(var row in table.Rows)
            {
                var formattedDaysOfWeek = row.Values.ElementAt(0);
                var formattedTimePeriods = row.Values.ElementAt(1);
                _expectedAvailabilities.Add(new Tuple<string, string>(formattedDaysOfWeek, formattedTimePeriods));
                var days = formattedDaysOfWeek.Split(',').Select(day => Helpers.DaysOfWeek.IndexOf(day.Trim())).ToArray();
                var timePeriods = formattedTimePeriods.Split(',').Select(ParseTimePeriod).ToArray();
                _userAvailabilities.Add(new UserAvailablity {DaysOfWeek = days, TimePeriods = timePeriods});
            }
        }

        private TimePeriod ParseTimePeriod(string period)
        {
            var match = _periodRegEx.Matches(period)[0];
            var start = DateTime.Parse(match.Groups[1].Value);
            var end = DateTime.Parse(match.Groups[2].Value);
            return new TimePeriod(start, end);
        }

        private void AddTimePeriod(Table table)
        {
            var timePeriod = ParseTimePeriod(table.Rows[0].Values.ElementAt(0));
            ScenarioContext.Current["timePeriod"] = timePeriod;
            _viewModel.AddTimePeriodCommand.Execute(timePeriod);
        }

        private static TimePeriod GetTimePeriod()
        {
            return (TimePeriod) ScenarioContext.Current["timePeriod"];
        }

        private void ToggleDayOfWeek(Table table)
        {
            var dayOfWeek = _viewModel.DaysOfWeek.First(day => day.FormattedDay == table.Rows[0].Values.ElementAt(0));
            ScenarioContext.Current["dayOfWeek"] = dayOfWeek;
            _viewModel.ToggleDayCommand.Execute(dayOfWeek);
        }

        private static IDayOfWeekViewModel GetDayOfWeek()
        {
            return (IDayOfWeekViewModel) ScenarioContext.Current["dayOfWeek"];
        }
    }
}