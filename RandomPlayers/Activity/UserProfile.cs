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
using RandomPlayers.DBO;
using RandomPlayers.Contracts;
using RandomPlayers.Services;
using Dmax.Dialog;
using RandomPlayers.Fragments.DialogFragments;
using RandomPlayers.Extentions;

namespace RandomPlayers.Activity {
    [Activity(Label = "DashBoard", Theme = "@style/AppTheme")]
    public class UserProfile : AppCompatActivity  {
        TextView txtWelcome, firstName, lastName, City, Country, birthDate;        
        LinearLayout linearLayout;

        ILocalProvider LocalProvider;
        //IFirestoreProvider AccountsApi;
        FirebaseAuth auth;
        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity_UserProfile);

            //Init Firebase
            auth = FirebaseAuth.Instance;

            //View
            txtWelcome = FindViewById<TextView>(Resource.Id.welcome);
            firstName = FindViewById<TextView>(Resource.Id.firstName);
            lastName = FindViewById<TextView>(Resource.Id.lastName);
            City = FindViewById<TextView>(Resource.Id.city);
            Country = FindViewById<TextView>(Resource.Id.country);
            birthDate = FindViewById<TextView>(Resource.Id.birthDate);            
            linearLayout = FindViewById<LinearLayout>(Resource.Id.activity_dashboard);

            //Check session
            //if (auth.CurrentUser != null)
                
            //AccountsApi = new ApiService();
            LocalProvider = Methods.GetService<ILocalProvider>();
            GetUser();
        }

        [Export("OnUpdateUserProfileButtonClick")]
        public void OnUpdateUserProfileButtonClick(View view) {
            //var newPass = newPassword.Text;

            //if (!string.IsNullOrEmpty(newPass)) {
            //    ChangePassword(newPass);

            //} else {
            //    var newFragment = new MessageAlert("Enter Passwor");
            //    newFragment.Show(FragmentManager.BeginTransaction(), "dialog");
            //}
            StartActivity(new Intent(this, typeof(EditUserProfile)));
        }

        [Export("OnLogOutButtonClick")]
        public void OnLogOutButtonClick(View view) {
            LogoutUser();
        }


        private void LogoutUser() {
            auth.SignOut();
            if (auth.CurrentUser == null) {
            LocalProvider.ClearCurrentUser();
                StartActivity(new Intent(this, typeof(Login)));                
            }
        }        

        

        void GetUser() {
            Android.App.AlertDialog dialog = new SpotsDialog(this);
            dialog.Show();
            try {
                //var response = await AccountsApi.GetCurentUser();
                var user = LocalProvider.GetCurrentUser();
                using(var p = new Handler(Looper.MainLooper)) {
                    p.Post(() => {
                        txtWelcome.Text = "Welcome , " + user.Email;
                        firstName.Text = user.FirstName;
                        lastName.Text = user.LastName;
                        City.Text = user.City;
                        Country.Text = user.Country;
                        birthDate.Text = user.DateOfBirth?.ToString("dd MMMM yyyy"); 
                        linearLayout.Invalidate();

                    });
                }                
            } catch (Exception ex) { }
            dialog.Dismiss();
        }

    }
}