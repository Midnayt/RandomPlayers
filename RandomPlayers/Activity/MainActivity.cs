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

namespace RandomPlayers {
    [Activity(Label = "RandomPlayers", MainLauncher = true, Icon = "@drawable/icon",Theme ="@style/AppTheme")]
    public class MainActivity : AppCompatActivity,IOnClickListener,IOnCompleteListener
    {
        Button btnLogin;
        EditText input_email, input_password;
        TextView btnSignUp, btnForgotPassword;

        RelativeLayout activity_main;
        
        FirebaseAuth auth;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView (Resource.Layout.Main);

            
            auth = FirebaseAuth.Instance;

            //View
            btnLogin = FindViewById<Button>(Resource.Id.login_btn_login);
            input_email = FindViewById<EditText>(Resource.Id.login_email);
            input_password = FindViewById<EditText>(Resource.Id.login_password);
            btnSignUp = FindViewById<TextView>(Resource.Id.login_btn_sign_up);
            btnForgotPassword = FindViewById<TextView>(Resource.Id.login_btn_forgot_password);
            activity_main = FindViewById<RelativeLayout>(Resource.Id.activity_main);

            btnSignUp.SetOnClickListener(this);
            btnLogin.SetOnClickListener(this);
            btnForgotPassword.SetOnClickListener(this);
        }

        public void OnClick(View v)
        {
            if(v.Id == Resource.Id.login_btn_forgot_password)
            {
                StartActivity(new Android.Content.Intent(this, typeof(ForgotPassword)));
                Finish();
            }
            else if (v.Id == Resource.Id.login_btn_sign_up)
            {
                StartActivity(new Android.Content.Intent(this, typeof(SignUp)));
                Finish();
            }
            else if (v.Id == Resource.Id.login_btn_login)
            {
                LoginUser(input_email.Text, input_password.Text);
            }
        }

        private void LoginUser(string email, string password)
        {
            auth.SignInWithEmailAndPassword(email, password)
                .AddOnCompleteListener(this);
        }

        public void OnComplete(Task task)
        {
           if(task.IsSuccessful)
            {
                StartActivity(new Android.Content.Intent(this, typeof(DashBoard)));
                Finish();
            }
           else
            {
                Snackbar snackBar = Snackbar.Make(activity_main, "Login Failed ", Snackbar.LengthShort);
                snackBar.Show();
            }
        }
    }
}

