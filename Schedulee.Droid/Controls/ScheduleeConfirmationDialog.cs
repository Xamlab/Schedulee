﻿using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Schedulee.Droid.Controls
{
    public class ScheduleeConfirmationDialog : Dialog, View.IOnClickListener
    {
        private Button _okButton;
        private Button _cancelButton;
        private TextView _title;
        private Context _context;

        protected ScheduleeConfirmationDialog(Context context, bool cancelable, EventHandler cancelHandler) : base(context, cancelable, cancelHandler)
        {
            _context = context;
        }

        protected ScheduleeConfirmationDialog(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public ScheduleeConfirmationDialog(Context context) : base(context)
        {
            _context = context;
        }

        protected ScheduleeConfirmationDialog(Context context, bool cancelable, IDialogInterfaceOnCancelListener cancelListener) : base(context, cancelable, cancelListener)
        {
            _context = context;
        }

        public ScheduleeConfirmationDialog(Context context, int themeResId) : base(context, themeResId)
        {
            _context = context;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RequestWindowFeature((int)WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.confirmation_dialog_layout);
            _okButton = FindViewById<Button>(Resource.Id.confirmation_dialog_ok_button);
            _cancelButton = FindViewById<Button>(Resource.Id.confirmation_dialog_cancel_button);
            _title = FindViewById<TextView>(Resource.Id.confirmation_dialog_title_text);
            CreateDialog();

            _okButton.SetOnClickListener(this);
            _cancelButton.SetOnClickListener(this);
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
        }

        protected virtual void OnAccept()
        {
            Dismiss();
        }

        protected virtual void CreateDialog(string title = "title", string confirmText = "yes", string cancelText = "no")
        {
            _title.Text = cancelText;
            _okButton.Text = confirmText;
            _cancelButton.Text = cancelText;
        }
    }
}