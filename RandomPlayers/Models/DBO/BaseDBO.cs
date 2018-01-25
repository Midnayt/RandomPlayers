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
using Newtonsoft.Json;
using RandomPlayers.Extentions;
using SQLite;

namespace RandomPlayers.DBO {
    public class BaseDBO : NotifyPropertyChanged {

        string _id;
        [PrimaryKey]
        public string Id {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        DateTime _createdAt;
        [JsonIgnore]
        DateTime CreatedAt {
            get { return _createdAt; }
            set { SetProperty(ref _createdAt, value); }
        }

        DateTime _updatedAt;
        [JsonIgnore]
        DateTime UpdatedAt {
            get { return _updatedAt; }
            set { SetProperty(ref _updatedAt, value); }
        }

        bool _isDeleted;
        public bool IsDeleted {
            get { return _isDeleted; }
            set { SetProperty(ref _isDeleted, value); }
        }               
        
    }
}