using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Dmax.Dialog;
using Firebase.Auth;
using Java.Interop;
using RandomPlayers.Contracts;
using RandomPlayers.DBO;
using RandomPlayers.Extentions;

namespace RandomPlayers.Activity {
    [Activity(Label = "EditUserProfile", Theme = "@style/AppTheme")]
    public class EditUserProfile : AppCompatActivity {

        EditText firstName, lastName, country, city;
        TextView birthDate;
        LinearLayout linearLayout;;
        FirebaseAuth auth;
        User user;
        string dateOfBirth;
        IFirestoreProvider AccountsApi;
        ILocalProvider LocalProvider;

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity_EditUserProgile);
            // Create your application here

            firstName = FindViewById<EditText>(Resource.Id.firstName);
            lastName = FindViewById<EditText>(Resource.Id.lastName);
            country = FindViewById<EditText>(Resource.Id.country);
            city = FindViewById<EditText>(Resource.Id.city);
            birthDate = FindViewById<TextView>(Resource.Id.birthDate);
            linearLayout = FindViewById<LinearLayout>(Resource.Id.userEditProfile);


            AccountsApi = Methods.GetService<IFirestoreProvider>();
            LocalProvider = Methods.GetService<ILocalProvider>();
            GetUser();
        }

        [Export("OnUpdateButtonClick")]
        public void OnUpdateButtonClick(View v) {
            UpdateUser();
        }

        void GetUser() {
            Android.App.AlertDialog dialog = new SpotsDialog(this);
            dialog.Show();
            try {
                
                var user = LocalProvider.GetCurrentUser();
                using (var p = new Handler(Looper.MainLooper)) {
                    p.Post(() => {                        
                        firstName.Text = user.FirstName;
                        lastName.Text = user.LastName;
                        city.Text = user.City;
                        country.Text = user.Country;
                        birthDate.Text = user.DateOfBirth?.ToString("dd-MMM-yyyy");
                        linearLayout.Invalidate();

                    });
                }
            } catch (Exception ex) { }
            dialog.Dismiss();
        }

        async void UpdateUser() {
            Android.App.AlertDialog dialog = new SpotsDialog(this);
            dialog.Show();

            dialog.Dismiss();

        }

    }
}