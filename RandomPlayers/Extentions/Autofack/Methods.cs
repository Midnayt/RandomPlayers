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

namespace RandomPlayers.Extentions {
    public static class Methods {

        public static T GetService<T>() {
            return Init.GetService<T>();
        }

        public static void RegisterServicesForClass(object _class) {
            Init.AddInstance(_class);
        }

        public static string GetPropertyName<T>(Expression<Func<T>> propertyDelegate) {
            var expression = (MemberExpression)propertyDelegate.Body;
            return expression.Member.Name;
        }

        public static void Initialize() {
        }

        public static void Report(string message, Exception ex = null) {
            System.Diagnostics.Debug.WriteLine($"MVVMForms.Report:{message}");
            if (ex != null)
                System.Diagnostics.Debug.WriteLine($"MVVMForms.Exception:{ex.Message}\n{ex.StackTrace}");
        }
    }
}