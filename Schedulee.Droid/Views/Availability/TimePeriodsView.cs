using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Schedulee.Droid.Controls;
using Schedulee.UI.ViewModels.Availability;

namespace Schedulee.Droid.Views.Availability
{
    public class TimePeriodsView : StackListView<ITimePeriodViewModel>
    {
        public TimePeriodsView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public TimePeriodsView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Create(context);
        }

        public TimePeriodsView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Create(context);
        }

        public TimePeriodsView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Create(context);
        }

        public TimePeriodsView(Context context) : base(context)
        {
            Create(context);
        }

        protected override LayoutParams GetViewLayoutParametersAtIndex(int index)
        {
            return new LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
        }

        private void Create(Context context)
        {
            Root.Orientation = Orientation.Vertical;
            Root.LayoutParameters = new LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
            Itemtemplate = new DataTemplate(ctx => new TimePeriodView(ctx));
            ItemClickable = true;
        }

    }
}