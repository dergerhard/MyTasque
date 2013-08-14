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
using Android.Preferences;

namespace MyTasque
{
	[Activity (Label = "TasquePreferenceActivity")]			
	public class TasquePreferenceActivity : PreferenceActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			this.AddPreferencesFromResource (Resource.Xml.preferences);
		}
	}
}

