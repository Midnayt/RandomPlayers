using System;
using System.Collections.Generic;
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
using RandomPlayers.Contracts;
using RandomPlayers.DBO;
using RandomPlayers.Services;
using static Android.Views.View;

namespace RandomPlayers {
    [Activity(Label = "UserInfo", Theme = "@style/AppTheme")]
    public class UserInfo : AppCompatActivity, IOnClickListener, IOnCompleteListener {

        Button btnRegister;
        EditText input_first_name, input_last_name, input_country, input_city, input_birth_date;
        RelativeLayout activity_user_info;
        FirebaseAuth auth;
        User User;
        IFirestoreProvider AccountsApi;

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.UserInfo);

            auth = FirebaseAuth.Instance;

            btnRegister = FindViewById<Button>(Resource.Id.info_btn_register);
            input_first_name = FindViewById<EditText>(Resource.Id.info_first_name);
            input_last_name = FindViewById<EditText>(Resource.Id.info_last_name);
            input_country = FindViewById<EditText>(Resource.Id.info_country);
            input_city = FindViewById<EditText>(Resource.Id.info_city);
            input_birth_date = FindViewById<EditText>(Resource.Id.info_birth_date);
            activity_user_info = FindViewById<RelativeLayout>(Resource.Id.activity_user_info);


            btnRegister.SetOnClickListener(this);
            AccountsApi = new ApiService();
        }

        public async void OnClick(View v) {
            if (v.Id == Resource.Id.info_btn_register) {
                CreateUser();

            }
        }

        public void OnComplete(Task task) {
            if (task.IsSuccessful == true) {
                StartActivity(new Intent(this, typeof(MainActivity)));
                Finish();
                Snackbar snackBar = Snackbar.Make(activity_user_info, "Register successfully", Snackbar.LengthShort);
                snackBar.Show();
            } else {
                Snackbar snackBar = Snackbar.Make(activity_user_info, task.Exception.Message, Snackbar.LengthShort);
                snackBar.Show();
            }
        }

        async void CreateUser() {
            User = new User {
                Email = auth.CurrentUser.Email,
                Id = auth.CurrentUser.Uid.ToString(),
                FirstName = input_first_name.Text,
                LastName = input_last_name.Text,
                Country = input_country.Text,
                City = input_city.Text,
                //DateOfBirth = DateTime.Parse(input_birth_date.Text),

            };
            var response = await AccountsApi.RegisterNew(User);
            

        }

    }
}