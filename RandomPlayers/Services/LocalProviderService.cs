using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RandomPlayers.Contracts;
using RandomPlayers.DBO;
using RandomPlayers.Implementations;
using SQLite;

namespace RandomPlayers.Services {
    public class LocalProviderService : ILocalProvider {

        #region variables

        public static object locker = new object();

        IFileHelper FileHelper;

        private SQLiteConnection __connection;
        private SQLiteConnection _connection {
            get {
                try {
                    if (__connection == null) {
                        if (FileHelper == null)
                            FileHelper = new FileHelper();
                        __connection = new SQLiteConnection(FileHelper.GetDatabasePath("data.db3"));
                        __connection.CreateTable<User>();
                        
                    }
                    return __connection;
                } catch (Exception ex) {
                    __connection = null;
                    Report($"Create SQLConnection", ex);
                    return null;
                }
            }
        }

        #endregion
        #region CRUD

        private bool InsertItem<T>(T item) {
            lock (locker) {
                try {
                    return _connection.Insert(item) > 0;
                } catch (Exception ex) {
                    Report($"Insert({item})", ex);
                    return false;
                }
            }
        }

        private bool InsertAll<T>(List<T> items) {
            lock (locker) {
                try {
                    return _connection.InsertAll(items) > 0;
                } catch (Exception ex) {
                    Report($"InsertAll<{typeof(T)?.Name}>({items})", ex);
                    return false;
                }
            }
        }

        private bool UpdateItem<T>(T item) {
            lock (locker) {
                try {
                    return _connection.Update(item) > 0;
                } catch (Exception ex) {
                    Report($"Insert({item})", ex);
                    return false;
                }
            }
        }

        private T GetItem<T>(string id) where T : BaseDBO, new() {
            lock (locker) {
                try {
                    return _connection.Table<T>()?.Where(x => x.Id == id)?.FirstOrDefault();
                } catch (Exception ex) {
                    Report($"GetItem<{typeof(T)?.Name}>({id})", ex);
                    return null;
                }
            }
        }

        private List<T> GetItems<T>(Expression<Func<T, bool>> where = null) where T : class, new() {
            lock (locker) {
                try {
                    TableQuery<T> items;
                    if (where != null)
                        items = _connection.Table<T>().Where(where);
                    else
                        items = _connection.Table<T>();
                    return items.ToList();
                } catch (Exception ex) {
                    Report($"GetItems<{typeof(T)?.Name}>({where})", ex);
                    return null;
                }
            }
        }

        private bool Delete<T>(T item) {
            lock (locker) {
                try {
                    return _connection.Delete(item) > 0;
                } catch (Exception ex) {
                    Report($"Delete<{typeof(T)?.Name}>({item})", ex);
                    return false;
                }
            }
        }

        private bool DeleteAll<T>() {
            lock (locker) {
                try {
                    return _connection.DeleteAll<T>() > 0;
                } catch (Exception ex) {
                    Report($"DeleteAll<{typeof(T)?.Name}>()", ex);
                    return false;
                }
            }
        }

        #endregion
        #region Report

        void Report(string message, Exception ex = null) {
            if (ex != null)
                System.Diagnostics.Debug.WriteLine($"LOCALPROVIDERSERVICE\t\t{message}: \n{ex?.Message}\n{ex?.StackTrace}");
            else
                System.Diagnostics.Debug.WriteLine($"LOCALPROVIDERSERVICE\t\t{message}");
        }

        #endregion
        #region users

        public void SetCurrentUser(User user) {
            DeleteAll<User>();
            InsertItem(user);
        }

        public User GetCurrentUser() {
            var users = GetItems<User>();
            if (users?.Any() == true)
                return users.First();
            return null;
        }

        public void ClearCurrentUser() {
            DeleteAll<User>();
        }

        #endregion
    }
}