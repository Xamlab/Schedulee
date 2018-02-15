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

        public LoadReservationsCommand(ReservationsViewModel viewModel, IApiClient apiClient) : base(viewModel)
        {
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

            var now = DateTimeOffset.Now;
            //Go back to the beginning of the month
            var start = new DateTime(now.Year, now.Month, 1, 0, 0, 0);
            var end = start.AddMonths(1);
            var month = new List<DateTime>();
            while(start < end)
            {
                month.Add(start);
                start = start.AddDays(1);
            }

            _viewModel.Items = month.Join(reservations,
                                          date => date,
                                          reservation => reservation.Start.Date,
                                          (date, reservation) => new {Date = date, Reservation = reservation})
                                    .GroupBy(result => result.Date)
                                    .Select(grouping => new DateViewModel
                                                        {
                                                            Date = grouping.Key,
                                                            Reservations = grouping.Select(g => g.Reservation).ToList(),
                                                            Month = grouping.Key.ToString("MMM"),
                                                            Day = grouping.Key.Day.ToString()
                                                        });
        }
    }
}