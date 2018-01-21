using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace RandomPlayers.Extentions {
    public class NotifyPropertyChanged : INotifyPropertyChanged{

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged<T>(Expression<Func<T>> propertyDelegate) {
            var expression = (MemberExpression)propertyDelegate.Body;
            var name = expression.Member.Name;
            RaisePropertyChanged(name);
        }

        public void RaisePropertyChanged(string name) {
            try {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine($"{this?.GetType()?.Name}.RaisePropertyChanged({name}): {ex.Message}\n{ex.StackTrace}");
            }
        }

        public bool SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = "") {
            if (!Object.Equals(property, value)) {
                property = value;
                RaisePropertyChanged(propertyName);
                return true;
            }
            return false;
        }

    }
}