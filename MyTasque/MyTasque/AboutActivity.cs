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

namespace MyTasque
{
	/// <summary>
	/// About activity.
	/// </summary>
	[Activity (Label = "@string/AboutActivity")]			
	public class AboutActivity : Activity
	{
		/// <summary>
		/// Raises the create event.
		/// </summary>
		/// <param name="bundle">Bundle.</param>
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			this.SetContentView (Resource.Layout.About);
		}
	}
}

