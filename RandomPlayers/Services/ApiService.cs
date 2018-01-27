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
using Newtonsoft.Json.Linq;

namespace RandomPlayers.Services {
    public class ApiService : BaseAPI, IFirestoreProvider {

        const string ApiUrl = "https://firestore.googleapis.com/v1beta1/projects/random-players/databases/(default)";
        User user;

        public async Task<ApiResponse> RegisterNew(User user) {

            var k = JsonConvert.SerializeObject(user, Formatting.Indented, new KeysJsonConverter(typeof(User)));

            var keys = new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string> ("fields", k)
            };

            var url = $"{ApiUrl}/documents/users?documentId={user.Id}";

            ApiResponse response = await PostAsync(url, $"{{ \"fields\" : {k} }}");
            return response;

        }

        public async Task<ApiResponse<User>> GetCurentUser() {

            var id = FirebaseAuth.Instance.Uid;
            var url = $"{ApiUrl}/documents/users/{id}";
            var response = await GetAsync<User>(url);
            return response;
        }

        public Task<ApiResponse> UpdateCurentUser(User user) {
            throw new NotImplementedException();
        }

        
    }
}