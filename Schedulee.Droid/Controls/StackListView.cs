using System;
using System.ComponentModel;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Schedulee.Droid.Controls
{
    public class StackListView<T> : LimitedItemsView<T>
        where T : INotifyPropertyChanged
    {
        private LinearLayout Root { get; set; }

        public StackListView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public StackListView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Create(context);
        }

        public StackListView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Create(context);
        }

        public StackListView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Create(context);
        }

        public StackListView(Context context) : base(context)
        {
            Create(context);
        }

        public override void AddChildView(View view, int index)
        {
            Root.AddView(view, index);
        }

        public override void RemoveChildView(int index)
        {
            Root.RemoveViewAt(index);
        }

        protected override void Clear()
        {
            base.Clear();
            Root.RemoveAllViews();
            var layoutParams = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            AddView(Root, 0, layoutParams);
        }

        private void Create(Context context)
        {
            Root = new LinearLayout(context)
                   {
                       Orientation = Orientation.Vertical
                   };

            var layoutParams = new LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            AddView(Root, 0, layoutParams);
        }
    }
}