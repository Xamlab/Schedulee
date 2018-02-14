using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Schedulee.Core.Extensions;

namespace Schedulee.UI.ViewModels.Base.Implementation
{
    public class CompositeValidator : IViewModelValidator
    {
        private readonly List<IViewModelValidator> _validators;

        public CompositeValidator(IEnumerable<IViewModelValidator> validators)
        {
            _validators = new List<IViewModelValidator>();
            if(validators != null)
            {
                foreach(var validator in validators)
                {
                    _validators.Add(validator);
                    validator.ErrorsChanged += ValidatorOnErrorsChanged;
                }
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if(_validators == null) return null;
            List<object> errors = new List<object>();
            foreach(var validator in _validators)
            {
                errors.AddRange(validator.GetErrors(propertyName));
            }

            return errors;
        }

        public bool HasErrors => _validators?.Any(validator => validator.HasErrors) ?? false;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool Validate()
        {
            foreach(var validator in _validators)
            {
                validator.Validate();
            }

            return !HasErrors;
        }

        public IList GetAllErrors(params string[] propertyNames)
        {
            if(_validators == null) return null;
            List<object> errors = new List<object>();
            foreach(var validator in _validators)
            {
                var allErrors = validator.GetAllErrors(propertyNames);
                if(allErrors != null) errors.AddRange(allErrors);
            }

            return errors;
        }

        public List<string> GetAllErrorsInString()
        {
            if(_validators == null) return null;
            List<string> errors = new List<string>();
            foreach(var validator in _validators)
            {
                var allErrors = validator.GetAllErrorsInString();
                if(allErrors != null) errors.AddRange(allErrors);
            }

            return errors;
        }

        public List<string> GetErrorsInString(string propertyName)
        {
            if(_validators == null)
                return null;
            List<string> errors = new List<string>();
            foreach(var validator in _validators)
            {
                var allErrors = validator.GetErrorsInString(propertyName);
                if(allErrors != null) errors.AddRange(allErrors);
            }

            return errors;
        }

        private void ValidatorOnErrorsChanged(object sender, DataErrorsChangedEventArgs args)
        {
            ErrorsChanged?.Invoke(sender, args);
        }
    }
}
