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
using RandomPlayers.Extentions;

namespace RandomPlayers.DBO {
    public class BaseDBO : NotifyPropertyChanged {

        string _id;
        //[PrimaryKey]
        public string Id {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        bool _isDeleted;
        public bool IsDeleted {
            get { return _isDeleted; }
            set { SetProperty(ref _isDeleted, value); }
        }

        DateTime _createdAt;
        public DateTime CreatedAt {
            get { return _createdAt; }
            set { SetProperty(ref _createdAt, value); }
        }

        DateTime _updateAt;
        public DateTime UpdateAt {
            get { return _updateAt; }
            set { SetProperty(ref _updateAt, value); }
        }
        
    }
}