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
	public class TasquePreferenceFragment : PreferenceFragment
	{
		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			this.AddPreferencesFromResource (Resource.Xml.preferences);

			// Create your fragment here
		}
	}
}

