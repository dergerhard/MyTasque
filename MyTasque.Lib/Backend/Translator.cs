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
	public sealed class Translator
	{

		/// <summary>
		/// The _instance. Used for correct singleton implementation.
		/// </summary>
		private static readonly Lazy<Translator> _instance = new Lazy<Translator> (() => new Translator());

		/// <summary>
		/// Gets or sets the concrete translator.
		/// </summary>
		/// <value>The concrete translator.</value>
		public ITranslator ConcreteTranslator { get; set; }

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <value>The instance.</value>
		public static Translator Instance {
			get {
				return _instance.Value;
			}
		}

		/// <summary>
		/// Gets the string.
		/// </summary>
		/// <returns>The string.</returns>
		/// <param name="key">Key.</param>
		public string GetString(string key)
		{
			if (ConcreteTranslator == null)
				throw new InvalidOperationException ("No translator defined");
			return ConcreteTranslator.GetString (key);
		}
	}
}

