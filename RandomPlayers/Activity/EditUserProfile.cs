using System;
using System.Collections.Generic;
using System.Globalization;
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
using RandomPlayers.Fragments;
using RandomPlayers.Fragments.DialogFragments;

namespace RandomPlayers.Activity {
    [Activity(Label = "EditUserProfile", Theme = "@style/AppTheme")]
    public class EditUserProfile : AppCompatActivity {

        EditText firstName, lastName, country, city;
        TextView birthDate;
        LinearLayout linearLayout;
        FirebaseAuth auth;
        User user;
        DateTime? dateOfBirth;
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

        [Export("birthDateTextClick")]
        public void birthDateTextClick(View v) {
            var frag = DatePickerFragment.NewInstance(delegate (DateTime time) {
                birthDate.Text = time.ToLongDateString();
                dateOfBirth = time;
            });

            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        void GetUser() {
            Android.App.AlertDialog dialog = new SpotsDialog(this);
            dialog.Show();
            try {
                
                user = LocalProvider.GetCurrentUser();
                using (var p = new Handler(Looper.MainLooper)) {
                    p.Post(() => {                        
                        firstName.Text = user.FirstName;
                        lastName.Text = user.LastName;
                        city.Text = user.City;
                        country.Text = user.Country;
                        birthDate.Text = user.DateOfBirth?.ToString("dd MMMM yyyy");
                        linearLayout.Invalidate();

                    });
                }
            } catch (Exception ex) { }
            dialog.Dismiss();
        }

        async void UpdateUser() {
            Android.App.AlertDialog dialog = new SpotsDialog(this);
            dialog.Show();
            try {
                user.FirstName = firstName.Text;
                user.LastName = lastName.Text;
                user.Country = country.Text;
                user.City = city.Text;
                if (dateOfBirth != null)
                    user.DateOfBirth = dateOfBirth;

                var response = await AccountsApi.UpdateCurentUser(user);
                if (response.Succeed) {
                    LocalProvider.SetCurrentUser(user);
                    StartActivity(new Intent(this, typeof(UserProfile)));
                    Finish();
                } else {
                    var newFragment = new MessageAlert(response.Errors);
                    newFragment.Show(FragmentManager.BeginTransaction(), "dialog");
                }
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine($"{this.GetType().Name}: Exception: {ex.Message}");
            }
            dialog.Dismiss();

        }

    }
}