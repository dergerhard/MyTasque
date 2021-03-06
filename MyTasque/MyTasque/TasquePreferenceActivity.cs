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
	/// <summary>
	/// Tasque preference activity - Standard implementation for managing preferences
	/// </summary>
	[Activity (Label = "@string/TasquePreferenceActivity")]			
	public class TasquePreferenceActivity : PreferenceActivity
	{
		/// <summary>
		/// Raises the create event.
		/// </summary>
		/// <param name="bundle">Bundle.</param>
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			this.AddPreferencesFromResource (Resource.Xml.preferences);
		}

		/// <summary>
		/// Raises the destroy event.
		/// </summary>
		protected override void OnDestroy ()
		{
			base.OnDestroy ();
			TaskRepository.Instance.Activity.Restart ();
		}
	}
}

