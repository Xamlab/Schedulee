using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Java.Lang;
using Schedulee.UI.ViewModels.Base;

namespace Schedulee.Droid.Controls
{
    public class BaseValidateableView : BindableTextInputLayout
    {
        private IViewModelValidator _validationContext;
        public IViewModelValidator Validator { get; set; }

        protected BaseValidateableView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public BaseValidateableView(Context context) : base(context)
        {
        }

        public BaseValidateableView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public BaseValidateableView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public string[] ValidationIds { get; set; }

        private IList _errors;

        public IList Errors
        {
            get => _errors;
            private set
            {
                if(!Equals(_errors, value))
                {
                    _errors = value;
                    OnPropertyChanged(nameof(Errors));
                }
            }
        }

        protected override void OnBindingContextChanged()
        {
            if(_validationContext != null)
            {
                _validationContext.ErrorsChanged -= OnErrorsChanged;
            }

            _validationContext = Validator ?? (BindingContext as IValidateableViewModel)?.Validator;
            if(_validationContext != null)
            {
                _validationContext.ErrorsChanged += OnErrorsChanged;
            }

            base.OnBindingContextChanged();
        }

        private void OnErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            if(_validationContext != null && (string.IsNullOrEmpty(e.PropertyName) || ValidationIds.Contains(e.PropertyName)))
            {
                Errors = _validationContext.GetAllErrors(ValidationIds);
                var hasErrors = Errors != null && Errors.Count > 0;
                UpdateErrors(hasErrors);
            }
        }

        protected virtual void UpdateErrors(bool hasErrors)
        {
            if(hasErrors && ValidationIds != null)
            {
                var builder = new StringBuilder();
                foreach(var error in Errors)
                {
                    builder.Append(Environment.NewLine);
                    builder.Append(error.ToString());
                }

                Error = builder.ToString();
            }
            else Error = "";
        }
    }
}