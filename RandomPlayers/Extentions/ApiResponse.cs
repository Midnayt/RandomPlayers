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

namespace RandomPlayers.Extentions {
    public class ApiResponse<T> where T : new() {

        public bool Succeed { get; set; }

        public T ResponseObject { get; set; }

        public string Errors { get; set; }

    }

    public class ApiResponse {

        public bool Succeed { get; set; }

        public string Errors { get; set; }

        public string ResponseStr { get; set; }

    }
}