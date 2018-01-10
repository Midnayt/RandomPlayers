using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Firebase;
using Firebase.Auth;

namespace RandomPlayers {
    [Activity(Label = "RandomPlayers", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity {

        public static FirebaseApp app;
        FirebaseAuth mAuth;
        
        
        EditText Email;
        EditText Password;

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            mAuth = FirebaseAuth.GetInstance(app);
        }

        protected override void OnStart() {
            FirebaseAuth.GetInstance(app).SignOut();
            base.OnStart();
            FirebaseUser curentUser = mAuth.CurrentUser;
            
        }

    }
}

