using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Firebase;
using Firebase.Auth;
using System;
using static Android.Views.View;
using Android.Views;
using Android.Gms.Tasks;
using Android.Support.Design.Widget;
using System.Net.Http;
using System.Net.Http.Headers;
using Java.Interop;
using RandomPlayers.Fragments.DialogFragments;

namespace RandomPlayers {
    [Activity(Label = "MainActivity", MainLauncher = false, Icon = "@drawable/icon", Theme = "@style/AppTheme")]
    public class MainActivity : AppCompatActivity {

        ProgressBar progressBar;
        EditText input_email, input_password;
        RelativeLayout activity_main;

        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            //View
            progressBar = FindViewById<ProgressBar>(Resource.Id.LoadingProgressBar);
            input_email = FindViewById<EditText>(Resource.Id.login_email);
            input_password = FindViewById<EditText>(Resource.Id.login_password);
            activity_main = FindViewById<RelativeLayout>(Resource.Id.activity_main);


        }

        [Export("OnLoginButtonClick")]
        public void OnLoginButtonClick(View view) {
            LoginUser(input_email.Text, input_password.Text);

        }

        [Export("OnForgotPasswordTextClick")]
        public void OnForgotPasswordTextClick(View view) {
            StartActivity(new Android.Content.Intent(this, typeof(ForgotPassword)));
            Finish();
        }

        [Export("OnSignUpTextClick")]
        public void OnSignUpTextClick(View view) {
            StartActivity(new Android.Content.Intent(this, typeof(SignUp)));
            Finish();
        }


        private async void LoginUser(string email, string password) {
            progressBar.Visibility = ViewStates.Visible;
            try {
                var user = await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(email, password);

                if (user != null) {

                    StartActivity(new Android.Content.Intent(this, typeof(DashBoard)));
                    Finish();

                } else {
                    var newFragment = new MessageAlert("Немає достуду до користувача");
                    newFragment.Show(FragmentManager.BeginTransaction(), "dialog");
                }
            } catch (Exception ex) {
                var newFragment = new MessageAlert(ex.Message);
                newFragment.Show(FragmentManager.BeginTransaction(), "dialog");
            };
            progressBar.Visibility = ViewStates.Gone;
        }

    }
}

