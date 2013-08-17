using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text;

namespace MyTasque.Lib
{
	public class MyTasqueObject
	{
		[XmlIgnore]
		private static int idCounter;

		public MyTasqueObject ()
		{
			Id = ++idCounter;
		}

		[XmlIgnore]
		public int Id { get; private set; }

		/// <summary>
		/// Gets or sets the change - specifies, whether an object was changed and needs to be synced or not.
		/// </summary>
		/// <value>The change.</value>
		[XmlElement("ChangeType")]
		public ChangeType Change { get; set; }
	}
}

