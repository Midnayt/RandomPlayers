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

namespace RandomPlayers.Contracts {
    public interface IFileHelper {
        string GetDatabasePath(string filename);

        bool FileExists(string path);

        byte[] ReadAllBytes(string path);

        void WriteBytes(string path, byte[] bytes);

        void DeleteFile(string path);

        string GetFileName(string fullPath);

        bool Exists(string path);

        string GetPersonalPath(string filename, string folder = null);
    }
}