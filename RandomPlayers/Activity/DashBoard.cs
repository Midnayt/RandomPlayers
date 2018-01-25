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
using Java.Interop;

namespace RandomPlayers {
    [Activity(Label = "DashBoard", Theme = "@style/AppTheme")]
    public class DashBoard : AppCompatActivity, IOnCompleteListener {
        TextView txtWelcome;
        EditText input_new_password;
        RelativeLayout activity_dashboard;

        FirebaseAuth auth;
        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DashBoard);

            //Init Firebase
            auth = FirebaseAuth.Instance;

            //View
            txtWelcome = FindViewById<TextView>(Resource.Id.dashboard_welcome);
            input_new_password = FindViewById<EditText>(Resource.Id.dashboard_newpassword);
            activity_dashboard = FindViewById<RelativeLayout>(Resource.Id.activity_dashboard);

            //Check session
            if (auth.CurrentUser != null)
                txtWelcome.Text = "Welcome , " + auth.CurrentUser.Email;
        }

        [Export("OnChangePasswordButtonClick")]
        public void OnChangePasswordButtonClick(View view) {
            var newPassword = input_new_password.Text;
            
            if (!string.IsNullOrEmpty(newPassword)) {
                ChangePassword(newPassword);

            } else {
                Snackbar snackBar = Snackbar.Make(activity_dashboard, "¬вед≥ть пароль", Snackbar.LengthShort);
                snackBar.Show();
            }
        }

        [Export("OnLogOutButtonClick")]
        public void OnLogOutButtonClick(View view) {
            LogoutUser();
        }


        private void LogoutUser() {
            auth.SignOut();
            if (auth.CurrentUser == null) {
                StartActivity(new Intent(this, typeof(MainActivity)));
                Finish();
            }
        }

        private void ChangePassword(string newPassword) {
            FirebaseUser user = auth.CurrentUser;
            user.UpdatePassword(newPassword)
                .AddOnCompleteListener(this);

        }

        public void OnComplete(Task task) {
            if (task.IsSuccessful == true) {
                Snackbar snackBar = Snackbar.Make(activity_dashboard, "Password has been changed", Snackbar.LengthShort);
                snackBar.Show();
            }
        }
    }
}