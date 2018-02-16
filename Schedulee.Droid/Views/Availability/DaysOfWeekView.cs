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
    public class DaysOfWeekView : StackListView<IDayOfWeekViewModel>
    {
        public DaysOfWeekView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public DaysOfWeekView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Create(context);
        }

        public DaysOfWeekView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Create(context);
        }

        public DaysOfWeekView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Create(context);
        }

        public DaysOfWeekView(Context context) : base(context)
        {
            Create(context);
        }

        protected override LayoutParams GetViewLayoutParametersAtIndex(int index)
        {
            return new LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent, 1f);
        }

        private void Create(Context context)
        {
            Root.Orientation = Orientation.Horizontal;
            Root.WeightSum = 7;
            Root.LayoutParameters = new LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
            Itemtemplate = new DataTemplate(ctx => new DayOfWeekView(ctx));
            ItemClickable = true;
        }
    }
}