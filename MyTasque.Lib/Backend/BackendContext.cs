using System;
using System.Collections.Generic;
using System.Text;

namespace MyTasque.Lib
{
	/// <summary>
	/// Backend context.
	/// </summary>
	public class BackendContext
	{
		/// <summary>
		/// Gets or sets the username.
		/// </summary>
		/// <value>The username.</value>
		public string Username { get; set; }

		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		/// <value>The password.</value>
		public string Password { get; set; }

		/// <summary>
		/// Gets or sets the URL.
		/// </summary>
		/// <value>The URL.</value>
		public string Url { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="MyTasque.Lib.BackendContext"/> class.
		/// </summary>
		public BackendContext()
		{
		}
	}
}

