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
using Android.Support.V7.App;
using Firebase.Auth;
using static Android.Views.View;
using Android.Gms.Tasks;
using Android.Support.Design.Widget;
using Firebase;
using RandomPlayers.Fragments.DialogFragments;
using Java.Interop;
using Dmax.Dialog;

namespace RandomPlayers.Activity {
    [Activity(Label = "SignUp", Theme = "@style/AppTheme")]
    public class SignUp : AppCompatActivity {

        EditText input_email, input_password, input_password_confirm;


        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity_SignUp);

            input_email = FindViewById<EditText>(Resource.Id.signup_email);
            input_password = FindViewById<EditText>(Resource.Id.signup_password);
            input_password_confirm = FindViewById<EditText>(Resource.Id.signup_password_confirm);
        }

        [Export("OnForgotPasswordTextClick")]
        public void OnForgotPasswordTextClick(View view) {
            StartActivity(new Intent(this, typeof(ForgotPassword)));
            Finish();
        }

        [Export("OnLoginTextClick")]
        public void OnLoginTextClick(View view) {
            StartActivity(new Intent(this, typeof(Login)));
            Finish();
        }

        [Export("OnRegisterButtonClick")]
        public void OnRegisterButtonClick(View view) {
            if (String.IsNullOrEmpty(input_email?.Text)) {
                var newFragment = new MessageAlert(Resources.GetText(Resource.String.emptyEmail));
                newFragment.Show(FragmentManager.BeginTransaction(), "dialog");
                return;
            }
            if (!IsValidEmail(input_email?.Text)) {
                var newFragment = new MessageAlert(Resources.GetText(Resource.String.ERROR_INVALID_EMAIL));
                newFragment.Show(FragmentManager.BeginTransaction(), "dialog");
                return;
            }
            if (String.IsNullOrEmpty(input_password?.Text) || String.IsNullOrEmpty(input_password_confirm?.Text)) {
                var newFragment = new MessageAlert(Resources.GetText(Resource.String.emptyPassword));
                newFragment.Show(FragmentManager.BeginTransaction(), "dialog");
                return;
            }
            if (input_password?.Length() < 6) {
                var newFragment = new MessageAlert(Resources.GetText(Resource.String.weakPassword));
                newFragment.Show(FragmentManager.BeginTransaction(), "dialog");
                return;
            }
            if (input_password?.Text != input_password_confirm?.Text) {
                var newFragment = new MessageAlert(Resources.GetText(Resource.String.passwordMistmatch));
                newFragment.Show(FragmentManager.BeginTransaction(), "dialog");
                return;
            }

            SignUpUser(input_email?.Text, input_password?.Text);
        }


        private async void SignUpUser(string email, string password) {
            Android.App.AlertDialog dialog = new SpotsDialog(this);
            dialog.Show();
            try {
                var user = await FirebaseAuth.Instance.CreateUserWithEmailAndPasswordAsync(email, password);

                if (user != null) {

                    StartActivity(new Intent(this, typeof(UserInfo)));
                    Finish();
                }
            } catch (FirebaseAuthException ex) {
                var eee = ex.ErrorCode;
                int errID = Resources.GetIdentifier(eee, "string", PackageName);


                var newFragment = new MessageAlert(Resources.GetText(errID));
                newFragment.Show(FragmentManager.BeginTransaction(), "dialog");
            }
            dialog.Dismiss();

        }

        bool IsValidEmail(string email) {
            return Android.Util.Patterns.EmailAddress.Matcher(email).Matches();
        }

    }
}