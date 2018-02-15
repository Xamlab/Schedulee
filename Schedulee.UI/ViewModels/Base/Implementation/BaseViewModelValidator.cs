using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;

namespace Schedulee.UI.ViewModels.Base.Implementation
{
    public abstract class BaseViewModelValidator<T> : AbstractValidator<T>, IViewModelValidator
        where T : INotifyPropertyChanged
    {
        private readonly T _viewModel;
        private Dictionary<string, IList<ValidationFailure>> _errors;

        protected BaseViewModelValidator(T viewModel)
        {
            _viewModel = viewModel;
            _viewModel.PropertyChanged += OnPropertyChanged;
        }

        public bool Validate()
        {
            var result = Validate(_viewModel);
            HasErrors = !result.IsValid;
            if(result.IsValid)
            {
                _errors = null;
            }
            else
            {
                _errors = result.Errors
                                .GroupBy(error => error.PropertyName, error => error)
                                .ToDictionary(errors => errors.Key, errors => errors.ToList() as IList<ValidationFailure>);
            }

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(null));
            return result.IsValid;
        }

        public IList GetAllErrors(string[] propertyNames = null)
        {
            if(propertyNames != null)
            {
                return _errors?.Where(error => propertyNames.Contains(error.Key)).SelectMany(error => error.Value).ToList();
            }

            return _errors?.SelectMany(error => error.Value).ToList();
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if(_errors?.Any() != true) return null;

            return _errors.TryGetValue(propertyName, out IList<ValidationFailure> errors) ? errors : null;
        }

        public List<string> GetAllErrorsInString()
        {
            if(_errors?.Any() != true) return null;
            return _errors.SelectMany(error => error.Value).Select(error => error.ErrorMessage).ToList();
        }

        public List<string> GetErrorsInString(string propertyName)
        {
            if(_errors?.Any() != true) return null;

            if(!_errors.TryGetValue(propertyName, out IList<ValidationFailure> errors)) return null;
            return errors.Select(error => error.ErrorMessage).ToList();
        }

        public bool HasErrors { get; private set; }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(ShouldValidate(e.PropertyName))
            {
                ValidateProperty(e.PropertyName);
            }
        }

        protected virtual bool ShouldValidate(string propertyName)
        {
            foreach(var rule in this)
            {
                var property = rule as PropertyRule;
                if(property?.PropertyName == propertyName) return true;
            }

            return false;
        }

        private void ValidateProperty(string propertyName)
        {
            var properties = new[] {propertyName};
            var context = new ValidationContext<T>(_viewModel, new PropertyChain(), new MemberNameValidatorSelector(properties));
            var result = Validate(context);
            if(result.IsValid)
            {
                if(_errors?.ContainsKey(propertyName) == true)
                {
                    _errors.Remove(propertyName);
                }
            }
            else
            {
                _errors = _errors ?? new Dictionary<string, IList<ValidationFailure>>();
                _errors[propertyName] = result.Errors;
            }

            HasErrors = _errors?.Any() == true;
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}