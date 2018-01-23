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

namespace RandomPlayers.DBO {
    public class User : BaseDBO {

        string _email;
        public string Email {
            get {return _email; }
            set {SetProperty(ref _email, value); }
        }

        string _firstName;
        public string FirstName {
            get { return _firstName; }
            set { SetProperty(ref _firstName, value); }
        }

        string _lastName;
        public string LastName {
            get { return _lastName; }
            set { SetProperty(ref _lastName, value); }
        }

        DateTime? _dateOfBirth;
        public DateTime? DateOfBirth {
            get { return _dateOfBirth; }
            set { SetProperty(ref _dateOfBirth, value); }
        }

        string _image;
        public string Image {
            get { return _image; }
            set { SetProperty(ref _image, value); }
        }

        string _city;
        public string City {
            get { return _city; }
            set { SetProperty(ref _city, value); }
        }

        string _country;
        public string Country {
            get { return _country; }
            set { SetProperty(ref _country, value); }
        }
                
    }
}