using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Schedulee.Droid.Controls;
using Schedulee.UI.ViewModels.Availability;

namespace Schedulee.Droid.Views.Availability
{
    public class AvailabilitiesView : StackListView<IAvailabilityViewModel>
    {
        public AvailabilitiesView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public AvailabilitiesView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Create(context);
        }

        public AvailabilitiesView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Create(context);
        }

        public AvailabilitiesView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Create(context);
        }

        public AvailabilitiesView(Context context) : base(context)
        {
            Create(context);
        }

        private void Create(Context context)
        {
            Itemtemplate = new DataTemplate(ctx => new AvailabilityView(ctx));
            ItemClickable = true;
        }
    }
}