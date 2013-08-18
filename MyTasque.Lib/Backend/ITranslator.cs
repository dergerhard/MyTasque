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

namespace MyTasque.Lib
{
	/// <summary>
	/// Translator Interface. Every platform sepcific translator must implement this interface.
	/// </summary>
	public interface ITranslator
	{
		string GetString (string key);
	}
}

