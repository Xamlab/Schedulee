using System;
using System.Threading.Tasks;
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
        private TextView _titleTextView;
        private TextView _messageTextView;

        private TaskCompletionSource<bool> _showTask;
        private string _title;
        private string _message;
        private string _dismissText;

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

        public Task ShowAsync(string title, string message, string dismissText)
        {
            _title = title;
            _dismissText = dismissText;
            _message = message;
            _showTask = new TaskCompletionSource<bool>();

            Show();
            return _showTask.Task;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RequestWindowFeature((int) WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.notification_dialog_layout);
            _okButton = FindViewById<Button>(Resource.Id.notification_dialog_ok_button);
            _titleTextView = FindViewById<TextView>(Resource.Id.notification_dialog_title_text);
            _messageTextView = FindViewById<TextView>(Resource.Id.notification_dialog_message_text);
            if(!string.IsNullOrEmpty(_title))
            {
                _titleTextView.Text = _title;
            }
            else
            {
                _titleTextView.Visibility = ViewStates.Gone;
            }

            if(!string.IsNullOrEmpty(_message))
            {
                _messageTextView.Text = _message;
            }
            else
            {
                _messageTextView.Visibility = ViewStates.Gone;
            }

            _okButton.Text = !string.IsNullOrEmpty(_dismissText) ? _dismissText : "OK";

            _okButton.SetOnClickListener(this);
            SetCancelable(false);
        }

        public void OnClick(View view)
        {
            Dismiss();
        }

        public override void Dismiss()
        {
            base.Dismiss();
            _showTask?.TrySetResult(true);
        }

        protected virtual void CreateDialog(string title, string message, string confirmText)
        {
            _titleTextView.Text = title;
            _okButton.Text = confirmText;
            _messageTextView.Text = message;
        }
    }
}