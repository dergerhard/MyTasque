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
using MyTasque.Lib;

namespace MyTasque
{
	/// <summary>
	/// Android translator - platform specific translator for the lib.
	/// </summary>
	public class AndroidTranslator : ITranslator
	{
		/// <summary>
		/// The context.
		/// </summary>
		private Activity context;

		public AndroidTranslator(Activity ctx)
		{
			this.context = ctx;
		}

		public string GetString (string key)
		{
			return (string)context.Resources.GetText (context.Resources.GetIdentifier (key, "string", "MyTasque.MyTasque"));
		}

	}
}

