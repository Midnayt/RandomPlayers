﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using RandomPlayers.Contracts;
using RandomPlayers.DBO;
using RandomPlayers.Extentions;
using Newtonsoft.Json;

namespace RandomPlayers.Services {
    public class ApiService : BaseAPI, IFirestoreProvider{

        const string ApiUrl = "https://firestore.googleapis.com/v1beta1/projects/random-players/databases/(default)";

        public async Task<ApiResponse> RegisterNew(User user) {


            var k = JsonConvert.SerializeObject(user, Formatting.Indented);
            k = k.Replace(":", ":{ "+'"'+"stringValue"+'"' + ":");
            k =k.Replace(",", "},");
            dynamic body = new { fields = k };

            

            var url = $"{ApiUrl}/documents/users?documentId={user.Id}";
            ApiResponse response = await PostAsync(url, body);
            //firestore.googleapis.com/v1beta1/projects/random-players/databases/(default)/documents/users?documentId=22&key={YOUR_API_KEY}
            return response;

        }
    }
}