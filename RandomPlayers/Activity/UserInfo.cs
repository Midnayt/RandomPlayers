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
using Firebase.Auth;
using Java.Interop;
using RandomPlayers.Contracts;
using RandomPlayers.DBO;
using RandomPlayers.Fragments;
using RandomPlayers.Services;
using static Android.Views.View;

namespace RandomPlayers {
    [Activity(Label = "UserInfo", Theme = "@style/AppTheme")]
    public class UserInfo : AppCompatActivity {


        EditText input_first_name, input_last_name, input_country, input_city, input_birth_date;
        RelativeLayout activity_user_info;
        FirebaseAuth auth;
        User User;
        string dateOfBirth;
        IFirestoreProvider AccountsApi;

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.UserInfo);

            auth = FirebaseAuth.Instance;

            input_first_name = FindViewById<EditText>(Resource.Id.info_first_name);
            input_last_name = FindViewById<EditText>(Resource.Id.info_last_name);
            input_country = FindViewById<EditText>(Resource.Id.info_country);
            input_city = FindViewById<EditText>(Resource.Id.info_city);
            input_birth_date = FindViewById<EditText>(Resource.Id.info_birth_date);
            activity_user_info = FindViewById<RelativeLayout>(Resource.Id.activity_user_info);

            AccountsApi = new ApiService();
        }

        [Export("OnRegisterButtonClick")]
        public void OnRegisterButtonClick(View v) {
            CreateUser();
        }

        [Export("birthDateTextClick")]
        public void birthDateTextClick(View v) {
            var frag = DatePickerFragment.NewInstance(delegate (DateTime time) {
                input_birth_date.Text = time.ToLongDateString();
                dateOfBirth = time.ToString("yyyy-MM-ddThh:mm:ss.000Z");
            });

            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        async void CreateUser() {
            User = new User {
                Email = auth.CurrentUser.Email,
                Id = auth.CurrentUser.Uid.ToString(),
                FirstName = input_first_name.Text,
                LastName = input_last_name.Text,
                Country = input_country.Text,
                City = input_city.Text,
                DateOfBirth = DateTime.ParseExact(dateOfBirth, "yyyy-MM-ddThh:mm:ss.000Z", CultureInfo.InvariantCulture)

            };
            var response = await AccountsApi.RegisterNew(User);
            if (response.Succeed) {
                StartActivity(new Intent(this, typeof(DashBoard)));
                Finish();
            }


        }

    }
}