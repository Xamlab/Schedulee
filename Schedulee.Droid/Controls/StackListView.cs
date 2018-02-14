using System.ComponentModel;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace Schedulee.Droid.Controls
{
    public class StackListView<T> : LimitedItemsView<T>
        where T : INotifyPropertyChanged
    {
        private LinearLayout Root { get; }

        public StackListView(Context context) : base(context)
        {
            Root = new LinearLayout(context)
                   {
                       Orientation = Orientation.Vertical,
                       
                   };


            var layoutParams = new LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            AddView(Root, 0, layoutParams);
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
    }
}