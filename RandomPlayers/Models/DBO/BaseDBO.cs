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
    public class BaseDBO {

        public string Id { get; set; }

        [JsonIgnore]
        public string Name { get; set; }

        [JsonIgnore]
        public DateTime CreatedAt { get; set; }

        [JsonIgnore]
        public DateTime UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }

    }
}