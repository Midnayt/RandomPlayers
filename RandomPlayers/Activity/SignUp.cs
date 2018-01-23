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

namespace RandomPlayers {
    [Activity(Label = "SignUp", Theme = "@style/AppTheme")]
    public class SignUp : AppCompatActivity, IOnClickListener, IOnCompleteListener {

        Button btnSignup;
        TextView btnLogin, btnForgotPass;
        EditText input_email, input_password, input_password_confirm;
        RelativeLayout activity_sign_up;



        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SignUp);

            //InitFirebase


            //View
            btnSignup = FindViewById<Button>(Resource.Id.signup_btn_register);
            btnLogin = FindViewById<TextView>(Resource.Id.signup_btn_login);
            btnForgotPass = FindViewById<TextView>(Resource.Id.signup_btn_forgot_password);
            input_email = FindViewById<EditText>(Resource.Id.signup_email);
            input_password = FindViewById<EditText>(Resource.Id.signup_password);
            input_password_confirm = FindViewById<EditText>(Resource.Id.signup_password_confirm);
            activity_sign_up = FindViewById<RelativeLayout>(Resource.Id.activity_user_info);
            btnLogin.SetOnClickListener(this);
            btnForgotPass.SetOnClickListener(this);
            btnSignup.SetOnClickListener(this);

            

        }

        public void OnClick(View v) {
            if (v.Id == Resource.Id.signup_btn_login) {
                StartActivity(new Intent(this, typeof(MainActivity)));
                Finish();
            } else if (v.Id == Resource.Id.signup_btn_forgot_password) {
                StartActivity(new Intent(this, typeof(ForgotPassword)));
                Finish();
            } else if (v.Id == Resource.Id.signup_btn_register) {
                if (input_password.Text == input_password_confirm.Text) {
                    SignUpUser(input_email.Text, input_password.Text);
                    StartActivity(new Intent(this, typeof(UserInfo)));

                    Finish();
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
        }


        private void SignUpUser(string email, string password) {
            FirebaseAuth.Instance.CreateUserWithEmailAndPassword(email, password)
                .AddOnCompleteListener(this, this);

        }

        public void OnComplete(Task task) {
            if (task.IsSuccessful == true) {
                Snackbar snackBar = Snackbar.Make(activity_sign_up, "Register successfully", Snackbar.LengthShort);
                snackBar.Show();
            } else {
                Snackbar snackBar = Snackbar.Make(activity_sign_up, task.Exception.Message, Snackbar.LengthShort);
                snackBar.Show();
            }
        }        

    }
}
