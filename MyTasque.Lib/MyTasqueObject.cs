using System;
using System.Collections.Generic;
using System.Text;

namespace MyTasque.Lib
{
	public class MyTasqueObject
	{
		/// <summary>
		/// The identifier counter.
		/// </summary>
		private static int idCounter;

		/// <summary>
		/// Initializes a new instance of the <see cref="MyTasque.Lib.MyTasqueObject"/> class.
		/// </summary>
		public MyTasqueObject ()
		{
			Id = ++idCounter;
		}

		/// <summary>
		/// Gets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		public int Id { get; private set; }

		/// <summary>
		/// Gets or sets the change - specifies, whether an object was changed and needs to be synced or not.
		/// </summary>
		/// <value>The change.</value>
		public ChangeType Change { get; set; }
	}
}

