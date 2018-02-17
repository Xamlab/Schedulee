using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace Schedulee.Droid.Controls
{
    public class ScheduleeConfirmationDialog : Dialog, View.IOnClickListener
    {
        private Button _okButton;
        private Button _cancelButton;
        private TextView _titleTextView;
        private TextView _messageTextView;

        private TaskCompletionSource<bool> _showTask;
        private string _title;
        private string _message;
        private string _dismissText;
        private string _confirmText;

        public ScheduleeConfirmationDialog(Context context) : base(context, false, (EventHandler) null)
        {
        }

        public Task<bool> ShowAsync(string title, string message, string dismissText, string confirmText)
        {
            _title = title;
            _message = message;
            _dismissText = dismissText;
            _confirmText = confirmText;
            _showTask = new TaskCompletionSource<bool>();

            Show();
            return _showTask.Task;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RequestWindowFeature((int) WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.confirmation_dialog_layout);
            _okButton = FindViewById<Button>(Resource.Id.confirmation_dialog_ok_button);
            _cancelButton = FindViewById<Button>(Resource.Id.confirmation_dialog_cancel_button);
            _titleTextView = FindViewById<TextView>(Resource.Id.confirmation_dialog_title_text);
            _messageTextView = FindViewById<TextView>(Resource.Id.confirmation_dialog_message_text);
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

            _okButton.Text = !string.IsNullOrEmpty(_confirmText) ? _confirmText : "OK";
            _cancelButton.Text = !string.IsNullOrEmpty(_dismissText) ? _dismissText : "CANCEL";

            _okButton.SetOnClickListener(this);
            _cancelButton.SetOnClickListener(this);
            SetCancelable(false);
        }

        public void OnClick(View view)
        {
            switch(view.Id)
            {
                case Resource.Id.confirmation_dialog_ok_button:
                    OnAccept();
                    break;
                case Resource.Id.confirmation_dialog_cancel_button:
                    OnCancel();
                    break;
            }
        }

        protected virtual void OnCancel()
        {
            Dismiss();
            _showTask?.TrySetResult(false);
        }

        protected virtual void OnAccept()
        {
            Dismiss();
            _showTask?.TrySetResult(true);
        }
    }
}