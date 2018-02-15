using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Android.Content;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Util;

namespace Schedulee.Droid.Controls
{
    public class BindableTextInputLayout : TextInputLayout, IBindableObject
    {
        protected BindableTextInputLayout(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public BindableTextInputLayout(Context context) : base(context)
        {
        }

        public BindableTextInputLayout(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public BindableTextInputLayout(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private object _bindingContext;

        public object BindingContext
        {
            get => _bindingContext;
            set
            {
                if(_bindingContext != value)
                {
                    _bindingContext = value;
                    OnBindingContextChanged();
                }
            }
        }

        protected virtual void OnBindingContextChanged()
        {
        }
    }
}