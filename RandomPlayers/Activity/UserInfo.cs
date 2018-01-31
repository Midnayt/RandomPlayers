using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Dmax.Dialog;
using Firebase.Auth;
using Java.Interop;
using RandomPlayers.Contracts;
using RandomPlayers.DBO;
using RandomPlayers.Fragments;
using RandomPlayers.Services;
using static Android.Views.View;

namespace RandomPlayers.Activity {
    [Activity(Label = "UserInfo", Theme = "@style/AppTheme")]
    public class UserInfo : AppCompatActivity {


        EditText firstName, lastName, country, city;
        TextView birthDate;
        FirebaseAuth auth;
        User user;
        string dateOfBirth;
        IFirestoreProvider AccountsApi;
        ILocalProvider LocalProvider;

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.UserInfo);

            auth = FirebaseAuth.Instance;

            firstName = FindViewById<EditText>(Resource.Id.firstName);
            lastName = FindViewById<EditText>(Resource.Id.lastName);
            country = FindViewById<EditText>(Resource.Id.country);
            city = FindViewById<EditText>(Resource.Id.city);
            birthDate = FindViewById<TextView>(Resource.Id.birthDate);
            
            AccountsApi = new ApiService();
            LocalProvider = new LocalProviderService();
        }

        [Export("OnRegisterButtonClick")]
        public void OnRegisterButtonClick(View v) {
            CreateUser();
        }

        [Export("birthDateTextClick")]
        public void birthDateTextClick(View v) {
            var frag = DatePickerFragment.NewInstance(delegate (DateTime time) {
                birthDate.Text = time.ToLongDateString();
                dateOfBirth = time.ToString("yyyy-MM-ddThh:mm:ss.000Z");
            });

            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        async void CreateUser() {
            Android.App.AlertDialog dialog = new SpotsDialog(this);
            dialog.Show();
            user = new User {
                Email = auth.CurrentUser.Email,
                Id = auth.CurrentUser.Uid.ToString(),
                FirstName = firstName.Text,
                LastName = lastName.Text,
                Country = country.Text,
                City = city.Text,
                DateOfBirth = DateTime.ParseExact(dateOfBirth, "yyyy-MM-ddThh:mm:ss.000Z", CultureInfo.InvariantCulture)

            };
            var response = await AccountsApi.RegisterNew(user);
            if (response.Succeed) {
                LocalProvider.SetCurrentUser(user);
                StartActivity(new Intent(this, typeof(DashBoard)));
                Finish();
            }
            dialog.Dismiss();

        }

    }
}