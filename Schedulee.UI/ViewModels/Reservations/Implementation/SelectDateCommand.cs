using Schedulee.UI.ViewModels.Base.Implementation;

namespace Schedulee.UI.ViewModels.Reservations.Implementation
{
    internal class SelectDateCommand : Command
    {
        private readonly ReservationsViewModel _viewModel;

        public SelectDateCommand(ReservationsViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            if(_viewModel.SelectedDate != null)
            {
                _viewModel.SelectedDate.IsSelected = false;
                _viewModel.SelectedDate = null;
            }

            if(parameter != null && parameter is IDateViewModel date)
            {
                _viewModel.SelectedDate = date;
                date.IsSelected = true;
            }
        }
    }
}