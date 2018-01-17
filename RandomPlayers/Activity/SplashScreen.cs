using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

namespace RandomPlayers {
    [Activity(Label = "Casual play", MainLauncher = true, NoHistory =true, Icon = "@drawable/firebase", Theme = "@style/AppTheme")]
    public class SplashScreen : AppCompatActivity {

        GifImageView gifImageView;
        ProgressBar progressBar;

        
        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SplashScreen);

            gifImageView = (GifImageView)FindViewById(Resource.Id.gifImageView);
            progressBar = (ProgressBar)FindViewById(Resource.Id.progressBar);

            Stream input = Assets.Open("splashscreen.gif");
            byte[] bytes = ConvertFileToByteArray(input);
            gifImageView.SetBytes(bytes);
            gifImageView.StartAnimation();


            Timer timer = new Timer();
            timer.Interval = 3000;
            timer.AutoReset = false;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            var auth = FirebaseAuth.Instance;
            var firebaseAnalytics = FirebaseAnalytics.GetInstance(this);


        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e) {
            
            StartActivity(new Intent(this,typeof(MainActivity)));
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