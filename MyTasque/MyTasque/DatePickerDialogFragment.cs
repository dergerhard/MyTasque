using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace MyTasque
{
	/// <summary>
	/// Date picker dialog fragment.
	/// </summary>
	public class DatePickerDialogFragment : DialogFragment
	{
		/// <summary>
		/// The context.
		/// </summary>
		private readonly Context ctx;

		/// <summary>
		/// Gets the date.
		/// </summary>
		/// <value>The date.</value>
		public  DateTime Date { get; private set; }

		/// <summary>
		/// The DateSetListener.
		/// </summary>
		private readonly Android.App.DatePickerDialog.IOnDateSetListener listener;

		/// <summary>
		/// Initializes a new instance of the <see cref="MyTasque.DatePickerDialogFragment"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="date">Date.</param>
		/// <param name="listener">Listener.</param>
		public DatePickerDialogFragment(Context context, DateTime date, Android.App.DatePickerDialog.IOnDateSetListener listener  )
		{
			this.ctx = context;
			this.Date = date;
			this.listener = listener;
		}

		/// <summary>
		/// Raises the create dialog event.
		/// </summary>
		/// <param name="savedState">Saved state.</param>
		public override Dialog OnCreateDialog(Bundle savedState)
		{
			var dialog = new Android.App.DatePickerDialog(ctx, listener, Date.Year, Date.Month - 1, Date.Day);
			return dialog;
		}
	}
}

