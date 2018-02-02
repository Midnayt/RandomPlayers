using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Felipecsl.GifImageViewLibrary;
using Firebase.Analytics;
using Firebase.Auth;
using RandomPlayers.Contracts;
using RandomPlayers.Extentions;
using RandomPlayers.Services;

namespace RandomPlayers.Activity {
    [Activity(Label = "Casual play", MainLauncher = true, NoHistory =true, Icon = "@drawable/dice", Theme = "@style/Theme.Splash")]
    public class SplashScreen : AppCompatActivity {

        GifImageView gifImageView;
        ProgressBar progressBar;
        ILocalProvider LocalProvider;

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SplashScreen);

            gifImageView = (GifImageView)FindViewById(Resource.Id.gifImageView);
            progressBar = (ProgressBar)FindViewById(Resource.Id.progressBar);            
            Stream input = Assets.Open("splashscreen.gif");
            byte[] bytes = ConvertFileToByteArray(input);
            gifImageView.SetBytes(bytes);
            gifImageView.StartAnimation();
        }

        protected override void OnResume() {
            base.OnResume();
            Init.Initialize(new CustomInit());

            LocalProvider = Methods.GetService<ILocalProvider>();



            //Task.Factory.StartNew(async () => {
            //    await Task.Delay(2000);
            var user = LocalProvider.GetCurrentUser();
                if (user != null) {
                    StartActivity(new Intent(this, typeof(DashBoard)));
                    Finish();
                } else {
                    StartActivity(new Intent(this, typeof(Login)));
                    Finish();
                }
           
                //var auth = FirebaseAuth.Instance;
                //var firebaseAnalytics = FirebaseAnalytics.GetInstance(this);

            //});



        }

        byte[] ConvertFileToByteArray(Stream input) {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream()) {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    ms.Write(buffer, 0, read);
                return ms.ToArray();
            }
        }

    }
}