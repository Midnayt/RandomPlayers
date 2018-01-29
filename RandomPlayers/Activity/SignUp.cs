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

namespace RandomPlayers {
    [Activity(Label = "SignUp", Theme = "@style/AppTheme")]
    public class SignUp : AppCompatActivity {

        ProgressBar progressBar;
        EditText input_email, input_password, input_password_confirm;
        RelativeLayout activity_sign_up;


        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SignUp);

            progressBar = (ProgressBar)FindViewById(Resource.Id.LoadingProgressBar);
            input_email = FindViewById<EditText>(Resource.Id.signup_email);
            input_password = FindViewById<EditText>(Resource.Id.signup_password);
            input_password_confirm = FindViewById<EditText>(Resource.Id.signup_password_confirm);
            activity_sign_up = FindViewById<RelativeLayout>(Resource.Id.activity_user_info);

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
            if (input_password.Text == input_password_confirm.Text) {
                SignUpUser(input_email.Text, input_password.Text);


            } else {
                //FragmentTransaction ft = FragmentManager.BeginTransaction();
                ////Remove fragment else it will crash as it is already added to backstack
                //Fragment prev = FragmentManager.FindFragmentByTag("dialog");
                //if (prev != null) {
                //    ft.Remove(prev);
                //}

                //ft.AddToBackStack(null);

                // Create and show the dialog.
                var newFragment = new MessageAlert("Паролі не співпадають");

                //Add fragment
                newFragment.Show(FragmentManager.BeginTransaction(), "dialog");
            }
        }


        private async void SignUpUser(string email, string password) {
            progressBar.Visibility = ViewStates.Visible;
            try {
                var user = await FirebaseAuth.Instance.CreateUserWithEmailAndPasswordAsync(email, password);

                if (user != null) {

                    StartActivity(new Intent(this, typeof(UserInfo)));
                    Finish();
                }
            } catch (Exception ex) {
                var newFragment = new MessageAlert(ex.Message);

                //Add fragment
                newFragment.Show(FragmentManager.BeginTransaction(), "dialog");
            }
            //FirebaseAuth.Instance.CreateUserWithEmailAndPassword(email, password).AddOnCompleteListener(this, this);
            progressBar.Visibility = ViewStates.Gone;

        }

    }
}
