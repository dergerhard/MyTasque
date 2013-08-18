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
using Android.Preferences;

namespace MyTasque
{
	/// <summary>
	/// Tasque preference fragment.
	/// </summary>
	public class TasquePreferenceFragment : PreferenceFragment
	{
		/// <summary>
		/// Raises the create event.
		/// </summary>
		/// <param name="savedInstanceState">Saved instance state.</param>
		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			this.AddPreferencesFromResource (Resource.Xml.preferences);
		}
	}
}

