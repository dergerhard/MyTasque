using System;
using System.Collections.Generic;
using System.Text;

namespace MyTasque.Lib
{
	public abstract class MyTasqueObject
	{
		static int idCounter;

		public MyTasqueObject ()
		{
			Id = ++idCounter;
		}

		public int Id { get; private set; }

		/// <summary>
		/// Gets or sets the change - specifies, whether an object was changed and needs to be synced or not.
		/// </summary>
		/// <value>The change.</value>
		public virtual ChangeType Change { get; set; }
	}
}

