using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;

namespace Schedulee.Droid.Controls
{
    [Register("schedulee.droid.controls.MyButton")]
    public class MyButton : Button
    {
        protected MyButton(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Text = "MyButton";
        }

        public MyButton(Context context) : base(context)
        {
            Text = "MyButton";
        }

        public MyButton(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Text = "MyButton";
        }

        public MyButton(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Text = "MyButton";
        }

        public MyButton(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Text = "MyButton";
        }
    }
}