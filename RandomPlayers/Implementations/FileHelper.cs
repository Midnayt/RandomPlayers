using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
//using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RandomPlayers.Contracts;

namespace RandomPlayers.Implementations {
    public class FileHelper : IFileHelper {

        public string GetDatabasePath(string filename) {
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(docPath, filename);
            return path;
        }

        public bool FileExists(string path) {
            return File.Exists(path);
        }

        public byte[] ReadAllBytes(string path) {
            return File.ReadAllBytes(path);
        }

        public void WriteBytes(string path, byte[] bytes) {
            File.WriteAllBytes(path, bytes);
        }

        public void DeleteFile(string path) {
            File.Delete(path);
        }

        public string GetFileName(string fullPath) {
            return Path.GetFileName(fullPath);
        }

        public bool Exists(string path) {
            return File.Exists(path);
        }

        public string GetPersonalPath(string filename, string folder = null) {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), folder ?? "", filename);
        }

        public string GetCachedPath(string filename, string folder = null) {
            return Path.Combine(Android.App.Application.Context.CacheDir.Path, folder ?? "", filename);
        }
    }
}