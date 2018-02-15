using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using NSubstitute;
using Schedulee.Core.Models;
using Schedulee.Core.Services;
using Schedulee.UI.Tests.Base;
using Schedulee.UI.ViewModels.Reservations;
using Schedulee.UI.ViewModels.Reservations.Implementation;
using Shouldly;
using TechTalk.SpecFlow;

namespace Schedulee.UI.Tests.Reservations
{
    [Binding, Scope(Feature = "Reservations")]
    public class ReservationsTestSteps : BaseTestSteps
    {
        private IReservationsViewModel _viewModel;

        private readonly DateTime[] _mockDates =
        {
            new DateTime(2018, 2, 1),
            new DateTime(2018, 2, 5)
        };

        private Reservation[] _mockReservations;
        private DateViewModel[] _expected;

        protected override void ConfigureServices()
        {
            base.ConfigureServices();
            _mockReservations = Builder<Reservation>.CreateListOfSize(5).All()
                                                    .With(reservation => reservation.Client = Builder<Client>.CreateNew().Build())
                                                    .Build().ToArray();
            _mockReservations[0].Start = _mockReservations[1].Start = _mockReservations[2].Start = _mockDates[0];
            _mockReservations[0].End = _mockReservations[1].End = _mockReservations[2].End = _mockDates[0].AddHours(1);

            _mockReservations[3].Start = _mockReservations[4].Start = _mockDates[1];
            _mockReservations[3].End = _mockReservations[4].End = _mockDates[1].AddHours(1);
            _expected = new[]
                        {
                            new DateViewModel
                            {
                                Date = new DateTime(2018, 2, 1),
                                Day = "1",
                                DayOfWeek = "2",
                                Reservations = new[]
                                               {
                                                   _mockReservations[0], _mockReservations[1], _mockReservations[2]
                                               }
                            },
                            new DateViewModel {Date = new DateTime(2018, 2, 2), Day = "2", DayOfWeek = "2"},
                            new DateViewModel {Date = new DateTime(2018, 2, 3), Day = "3", DayOfWeek = "2"},
                            new DateViewModel {Date = new DateTime(2018, 2, 4), Day = "4", DayOfWeek = "2"},
                            new DateViewModel
                            {
                                Date = new DateTime(2018, 2, 5),
                                Day = "5",
                                DayOfWeek = "2",
                                Reservations = new[]
                                               {
                                                   _mockReservations[3], _mockReservations[4]
                                               }
                            },
                            new DateViewModel {Date = new DateTime(2018, 2, 6), Day = "6"},
                            new DateViewModel {Date = new DateTime(2018, 2, 7), Day = "7"},
                            new DateViewModel {Date = new DateTime(2018, 2, 8), Day = "8"},
                            new DateViewModel {Date = new DateTime(2018, 2, 9), Day = "9"},
                            new DateViewModel {Date = new DateTime(2018, 2, 10), Day = "10"},
                            new DateViewModel {Date = new DateTime(2018, 2, 11), Day = "11"},
                            new DateViewModel {Date = new DateTime(2018, 2, 12), Day = "12"},
                            new DateViewModel {Date = new DateTime(2018, 2, 13), Day = "13"},
                            new DateViewModel {Date = new DateTime(2018, 2, 14), Day = "14"},
                            new DateViewModel {Date = new DateTime(2018, 2, 15), Day = "15"},
                            new DateViewModel {Date = new DateTime(2018, 2, 16), Day = "16"},
                            new DateViewModel {Date = new DateTime(2018, 2, 17), Day = "17"},
                            new DateViewModel {Date = new DateTime(2018, 2, 18), Day = "18"},
                            new DateViewModel {Date = new DateTime(2018, 2, 19), Day = "19"},
                            new DateViewModel {Date = new DateTime(2018, 2, 20), Day = "20"},
                            new DateViewModel {Date = new DateTime(2018, 2, 21), Day = "21"},
                            new DateViewModel {Date = new DateTime(2018, 2, 22), Day = "22"},
                            new DateViewModel {Date = new DateTime(2018, 2, 23), Day = "23"},
                            new DateViewModel {Date = new DateTime(2018, 2, 24), Day = "24"},
                            new DateViewModel {Date = new DateTime(2018, 2, 25), Day = "25"},
                            new DateViewModel {Date = new DateTime(2018, 2, 26), Day = "26"},
                            new DateViewModel {Date = new DateTime(2018, 2, 27), Day = "27"},
                            new DateViewModel {Date = new DateTime(2018, 2, 28), Day = "28"}
                        };
            var apiClient = Substitute.For<IApiClient>();
            apiClient.FetchReservationsAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult((IEnumerable<Reservation>) _mockReservations));
            var timeProvider = Substitute.For<ITimeProvider>();
            timeProvider.DateTimeOffsetNow.Returns(_mockDates[0]);
            timeProvider.DateTimeNow.Returns(_mockDates[0]);
            Container.RegisterSingleton(apiClient);
            Container.RegisterSingleton(timeProvider);
        }

        [Given(@"I have navigated to reservations page")]
        public void GivenIHaveNavigatedToReservationsPage()
        {
            _viewModel = Container.Resolve<IReservationsViewModel>();
        }

        [When(@"Reservations page is loaded")]
        public void WhenReservationsPageIsLoaded()
        {
            _viewModel.LoadCommand.ExecuteAsync(null).Wait();
        }

        [Then(@"I should see reservations grouped by the day of month")]
        public void ThenIShouldSeeReservationsGroupedByTheDayOfMonth()
        {
            var items = _viewModel.Items.ToArray();
            items.Length.ShouldBe(_expected.Length);
            for(int i = 0; i < _expected.Length; i++)
            {
                items[i].Date.ShouldBe(_expected[i].Date);
                items[i].Day.ShouldBe(_expected[i].Day);
                items[i].DayOfWeek.ShouldBe(_expected[i].Date.ToString("ddd"));
                var expectedReservations = _expected[i].Reservations?.ToArray();
                var actualReservations = items[i].Reservations?.ToArray();
                (expectedReservations?.Any() ?? false).ShouldBe(actualReservations?.Any() ?? false);
                if(expectedReservations?.Any() == true)
                {
                    actualReservations.Length.ShouldBe(expectedReservations.Length);
                    for(int k = 0; k < expectedReservations.Length; k++)
                    {
                        actualReservations[k].Id.ShouldBe(expectedReservations[k].Id);
                        actualReservations[k].Start.ShouldBe(expectedReservations[k].Start);
                        actualReservations[k].End.ShouldBe(expectedReservations[k].End);
                        actualReservations[k].RatePerHour.ShouldBe(expectedReservations[k].RatePerHour);
                        actualReservations[k].Client.Id.ShouldBe(expectedReservations[k].Client.Id);
                        actualReservations[k].Client.FirstName.ShouldBe(expectedReservations[k].Client.FirstName);
                        actualReservations[k].Client.LastName.ShouldBe(expectedReservations[k].Client.LastName);
                        actualReservations[k].Client.Location.ShouldBe(expectedReservations[k].Client.Location);
                        actualReservations[k].Client.PhoneNumber.ShouldBe(expectedReservations[k].Client.PhoneNumber);
                    }
                }
            }
        }
    }
}