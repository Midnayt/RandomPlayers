﻿using Android.App;
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
using Dmax.Dialog;
using System.Runtime.Remoting.Contexts;

namespace RandomPlayers.Activity {
    [Activity(Label = "Login", MainLauncher = false, Icon = "@drawable/dice", Theme = "@style/AppTheme")]
    public class Login : AppCompatActivity {

        
        EditText email, password;

        

        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Login);

            //View
            
            email = FindViewById<EditText>(Resource.Id.textEmail);
            password = FindViewById<EditText>(Resource.Id.textPassword);
        }

        [Export("OnLoginButtonClick")]
        public void OnLoginButtonClick(View view) {
            LoginUser(email.Text, password.Text);

        }

        //[Export("OnForgotPasswordTextClick")]
        //public void OnForgotPasswordTextClick(View view) {
        //    StartActivity(new Android.Content.Intent(this, typeof(UserInfo)));
        //    Finish();
        //}

        [Export("OnSignUpClick")]
        public void OnSignUpClick(View view) {
            StartActivity(new Android.Content.Intent(this, typeof(SignUp)));
            Finish();
        }


        private async void LoginUser(string email, string password) {
            Android.App.AlertDialog dialog = new SpotsDialog(this);
            dialog.Show();

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
            dialog.Dismiss();
        }

    }
}
