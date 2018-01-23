using System;
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
using Newtonsoft.Json.Serialization;

namespace RandomPlayers.Services {
    public class ApiService : BaseAPI, IFirestoreProvider{

        const string ApiUrl = "https://firestore.googleapis.com/v1beta1/projects/random-players/databases/(default)";

        public async Task<ApiResponse> RegisterNew(User user) {

            var settings = new JsonSerializerSettings {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };

            var k = JsonConvert.SerializeObject(user, Formatting.Indented, new KeysJsonConverter(typeof(User)));
            //var k = JsonConvert.SerializeObject(user, Formatting.Indented, settings);
            //k = k.Replace(":", ":{ "+'"'+"stringValue"+'"' + ":");
            //k =k.Replace("", "},");


            dynamic body = new { fields = k };

            var keys = new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string> ("fields", k)
            };

            var url = $"{ApiUrl}/documents/users?documentId={user.Id}";
            var t = new { fields = k };
            ApiResponse response = await PostAsync(url, $"{{ \"fields\" : {k} }}");
            //firestore.googleapis.com/v1beta1/projects/random-players/databases/(default)/documents/users?documentId=22&key={YOUR_API_KEY}
            return response;

        }
    }
}