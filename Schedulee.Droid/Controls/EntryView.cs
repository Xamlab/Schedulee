using System;
using Android.Content;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Util;
using Android.Views;

namespace Schedulee.Droid.Controls
{
    public class EntryView : BaseValidateableView
    {
        public TextInputEditText Entry { get; private set; }

        private string _title;

        public string Title
        {
            private get => _title;
            set
            {
                _title = value;
                OnTitleChanged();
            }
        }

        protected EntryView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public EntryView(Context context) : base(context)
        {
            Initialize(context);
        }

        public EntryView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize(context);
        }

        public EntryView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Initialize(context);
        }

        private void Initialize(Context context)
        {
            Entry = new TextInputEditText(context);
            AddView(Entry, new LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent));
        }

        private void OnTitleChanged()
        {
            Hint = Title;
        }
    }
}