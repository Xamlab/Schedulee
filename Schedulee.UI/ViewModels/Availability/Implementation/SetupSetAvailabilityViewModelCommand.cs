using System.ComponentModel;
using Schedulee.UI.ViewModels.Base.Implementation;

namespace Schedulee.UI.ViewModels.Availability.Implementation
{
    internal class SetupSetAvailabilityViewModelCommand : Command
    {
        private readonly SetAvailabilityViewModel _viewModel;

        public SetupSetAvailabilityViewModelCommand(SetAvailabilityViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if(args.PropertyName == nameof(ISetAvailabilityViewModel.IsLoading) ||
               args.PropertyName == nameof(ISetAvailabilityViewModel.IsSaving))
            {
                _viewModel.InProgress = _viewModel.IsSaving || _viewModel.IsLoading;
            }
        }
    }
}