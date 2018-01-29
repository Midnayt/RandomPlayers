using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace RandomPlayers.Fragments {
    public class DatePickerFragment : DialogFragment, DatePickerDialog.IOnDateSetListener {

        public static readonly string TAG = "X:" + typeof(DatePickerFragment).Name.ToUpper();

        Action<DateTime> _dateSelectedHandler = delegate { };

        public static DatePickerFragment NewInstance (Action<DateTime> onDateSelected) {
            var frag = new DatePickerFragment();
            frag._dateSelectedHandler = onDateSelected;
            return frag;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState) {
            var curently = DateTime.Now;
            var dialog = new DatePickerDialog(Activity, this, curently.Year, curently.Month - 1, curently.Day);
            return dialog;
        }

        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth) {
            var selectedDate = new DateTime(year, month + 1, dayOfMonth);
            Log.Debug(TAG, selectedDate.ToLongDateString());
            _dateSelectedHandler(selectedDate);
        }
    }
}