using System.Linq;
using FluentValidation;
using Schedulee.UI.Resources.Strings.Availability;
using Schedulee.UI.ViewModels.Base.Implementation;

namespace Schedulee.UI.ViewModels.Availability.Implementation
{
    internal class SetAvailabilityValidator : BaseViewModelValidator<SetAvailabilityViewModel>
    {
        public SetAvailabilityValidator(SetAvailabilityViewModel setAvailabilityViewModel) : base(setAvailabilityViewModel)
        {
            RuleFor(viewModel => viewModel.DaysOfWeek).Must(days => days?.Any(day => day.IsSelected) == true)
                                                      .WithMessage(Strings.DaysOfWeekRequiredValidationError);
            RuleFor(viewModel => viewModel.TimePeriods).NotEmpty().WithMessage(Strings.TimePeriodRequiredValidationError);
            RuleFor(viewModel => viewModel.IsIntersecting).Must(isIntersecting => isIntersecting == false)
                                                          .WithMessage(Strings.TimePeriodsIntersectingValidationError);
        }
    }
}