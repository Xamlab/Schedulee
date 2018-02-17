using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Schedulee.Droid.Controls
{
    public class ScheduleeNotificationDialog : Dialog, View.IOnClickListener
    {
        private Button _okButton;
        private TextView _title;
        private TextView _message;

        protected ScheduleeNotificationDialog(Context context, bool cancelable, EventHandler cancelHandler) : base(context, cancelable, cancelHandler)
        {
        }

        protected ScheduleeNotificationDialog(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public ScheduleeNotificationDialog(Context context) : base(context)
        {
        }

        protected ScheduleeNotificationDialog(Context context, bool cancelable, IDialogInterfaceOnCancelListener cancelListener) : base(context, cancelable, cancelListener)
        {
        }

        public ScheduleeNotificationDialog(Context context, int themeResId) : base(context, themeResId)
        {
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RequestWindowFeature((int) WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.notification_dialog_layout);
            _okButton = FindViewById<Button>(Resource.Id.notification_dialog_ok_button);
            _title = FindViewById<TextView>(Resource.Id.notification_dialog_title_text);
            _message = FindViewById<TextView>(Resource.Id.notification_dialog_title_text);
            CreateDialog();

            _okButton.SetOnClickListener(this);
            SetCancelable(false);
        }

        public void OnClick(View view)
        {
            OnAccept();
        }

        
        protected virtual void OnAccept()
        {
            Dismiss();
        }

        protected virtual void CreateDialog(string title = "title", string message = "message", string confirmText = "yes")
        {
            _title.Text = title;
            _okButton.Text = confirmText;
            _message.Text = message;
        }
    }
}