using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Schedulee.Core.Services;
using Schedulee.UI.ViewModels.Base.Implementation;

namespace Schedulee.UI.ViewModels.Reservations.Implementation
{
    internal class LoadReservationsCommand : LoadAsyncCommand
    {
        private readonly ReservationsViewModel _viewModel;
        private readonly IApiClient _apiClient;
        private readonly ITimeProvider _timeProvider;

        public LoadReservationsCommand(ReservationsViewModel viewModel, IApiClient apiClient, ITimeProvider timeProvider) : base(viewModel)
        {
            _timeProvider = timeProvider;
            _apiClient = apiClient;
            _viewModel = viewModel;
        }

        protected override async Task ExecuteCoreAsync(object param, CancellationToken token = default(CancellationToken))
        {
            var reservations = await _apiClient.FetchReservationsAsync(token);
            if(reservations == null)
            {
                _viewModel.Items = null;
                return;
            }

            var now = _timeProvider.DateTimeOffsetNow;
            //Go back to the beginning of the month
            var start = new DateTime(now.Year, now.Month, 1, 0, 0, 0);
            var end = start.AddMonths(1);
            var month = new List<DateTime>();
            while(start < end)
            {
                month.Add(start);
                start = start.AddDays(1);
            }

            _viewModel.Items = month.GroupJoin(reservations,
                                               date => date,
                                               reservation => reservation.Start.Date,
                                               (date, reservation) => new {Date = date, Reservations = reservation})
                                    .Select(grouping => new DateViewModel
                                                        {
                                                            Date = grouping.Date,
                                                            Reservations = grouping.Reservations?.ToList(),
                                                            DayOfWeek = grouping.Date.ToString("ddd"),
                                                            Day = grouping.Date.Day.ToString()
                                                        });
        }
    }
}