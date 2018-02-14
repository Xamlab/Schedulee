using System;
using System.Drawing;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Schedulee.Droid.Extensions;

namespace Schedulee.Droid.Controls
{
    public class ViewButton : ViewGroup
    {
        public event EventHandler<EventArgs> Clicked;
        private float _deviceWidth;

        protected ViewButton(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public ViewButton(Context context) : base(context)
        {
            Initialize(context);
        }

        public ViewButton(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        private void Initialize(Context context)
        {
            var metrics = Resources.DisplayMetrics;
            _deviceWidth = context.ConvertDpToPixel(metrics.Xdpi);
        }

        private void InvokeClicked(object sender, EventArgs args)
        {
            Clicked?.Invoke(sender, args);
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            AddTouchListener();
        }

        protected override void OnDetachedFromWindow()
        {
            base.OnDetachedFromWindow();
            RemoveTouchListener();
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            // Measurement will ultimately be computing these values.
            int maxHeight = 0;
            int maxWidth = 0;
            int childState = 0;
            int mLeftWidth = 0;
            int rowCount = 0;

            // Iterate through all children, measuring them and computing our dimensions
            // from their size.
            for(int i = 0; i < ChildCount; i++)
            {
                View child = GetChildAt(i);

                if(child.Visibility == ViewStates.Gone)
                    continue;

                // Measure the child.
                MeasureChild(child, widthMeasureSpec, heightMeasureSpec);
                maxWidth += Math.Max(maxWidth, child.MeasuredWidth);
                mLeftWidth += child.MeasuredWidth;

                if(mLeftWidth / _deviceWidth > rowCount)
                {
                    maxHeight += child.MeasuredHeight;
                    rowCount++;
                }
                else
                {
                    maxHeight = Math.Max(maxHeight, child.MeasuredHeight);
                }

                childState = CombineMeasuredStates(childState, child.MeasuredState);
            }

            // Check against our minimum height and width
            maxHeight = Math.Max(maxHeight, SuggestedMinimumHeight);
            maxWidth = Math.Max(maxWidth, SuggestedMinimumWidth);

            // Report our final dimensions.
            SetMeasuredDimension(ResolveSizeAndState(maxWidth, widthMeasureSpec, childState),
                                 ResolveSizeAndState(maxHeight, heightMeasureSpec, childState << MeasuredHeightStateShift));
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            //get the available size of child view
            int childRight = MeasuredWidth - PaddingRight;
            int childBottom = MeasuredHeight - PaddingBottom;
            int childWidth = childRight - PaddingLeft;
            int childHeight = childBottom - PaddingTop;

            var maxHeight = 0;
            var curLeft = PaddingLeft;
            var curTop = PaddingTop;

            for(int i = 0; i < ChildCount; i++)
            {
                View child = GetChildAt(i);

                if(child.Visibility == ViewStates.Gone)
                    return;

                //Get the maximum size of the child
                child.Measure(MeasureSpec.MakeMeasureSpec(childWidth, MeasureSpecMode.AtMost),
                              MeasureSpec.MakeMeasureSpec(childHeight, MeasureSpecMode.AtMost));
                var curWidth = child.MeasuredWidth;
                var curHeight = child.MeasuredHeight;
                //wrap is reach to the end
                if(curLeft + curWidth >= childRight)
                {
                    curLeft = PaddingLeft;
                    curTop += maxHeight;
                    maxHeight = 0;
                }

                //do the layout
                child.Layout(curLeft, curTop, curLeft + curWidth, curTop + curHeight);
                //store the max height
                if(maxHeight < curHeight)
                    maxHeight = curHeight;
                curLeft += curWidth;

                this.DisableChildren();
            }

            this.DisableChildren();
        }

        private void AddTouchListener()
        {
            Touch += OnTouch;
        }

        private void RemoveTouchListener()
        {
            Touch -= OnTouch;
        }

        private void OnTouch(object sender, TouchEventArgs args)
        {
            if(args.Event.Action == MotionEventActions.Down)
            {
                Alpha = 0.5f;
            }
            else if(args.Event.Action == MotionEventActions.Up || args.Event.Action == MotionEventActions.Cancel)
            {
                Alpha = 1;

                if(args.Event.Action != MotionEventActions.Up) return;
                var x = args.Event.GetX();
                var y = args.Event.GetY();
                if(ContainsPoint(x, y))
                {
                    InvokeClicked(this, EventArgs.Empty);
                }
            }
        }

        private bool ContainsPoint(float x, float y)
        {
            float px = Context.ConvertPixelsToDp(x);
            float py = Context.ConvertPixelsToDp(y);
            var frame = new Rectangle(0, 0, Width, Height);
            var contains = frame.Contains((int) px, (int) py);
            return contains;
        }
    }
}