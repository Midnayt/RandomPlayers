using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RandomPlayers.DBO;
using RandomPlayers.Extentions;


namespace RandomPlayers.Contracts {
    public interface IFirestoreProvider {

        Task<ApiResponse> RegisterNew(User user);
        Task<ApiResponse<User>> GetCurentUser(User user);
        Task<ApiResponse> UpdateCurentUser(User user);


        //Task<ApiResponse<AccessToken>> SignIn(string email, string password);

    }
}