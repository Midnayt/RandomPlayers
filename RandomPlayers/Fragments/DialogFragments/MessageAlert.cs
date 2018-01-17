using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace RandomPlayers.Fragments.DialogFragments {
    public class MessageAlert : DialogFragment {

        Action closeAction;
        string Message;
        string Title;
        string Button;

        public MessageAlert(string message, string title = "Помилка", string button = "OK", Action closeAction = null) {
            this.closeAction = closeAction;
            this.Message = message;
            this.Title = title;
            this.Button = button;
        }

        public override void Dismiss() {
            closeAction?.Invoke();
            base.Dismiss();
        }

        //public static MessageAlert NewInstance(Bundle bundle) {
        //    var fragment = new MessageAlert();
        //    fragment.Arguments = bundle;
        //    return fragment;
        //}

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            View view = inflater.Inflate(Resource.Layout.dialogFragment_MessageAlert, container, false);
            this.Dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);

            var title = view.FindViewById<TextView>(Resource.Id.textTitle);
            var message = view.FindViewById<TextView>(Resource.Id.textMessage);
            var button = view.FindViewById<Button>(Resource.Id.CloseButton);

            title.Text = Title;
            message.Text = Message;
            button.Text = Button;

            button.Click += delegate {
                Dismiss();
            };
            return view;
        }
    }
}