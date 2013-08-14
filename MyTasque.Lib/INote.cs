using System;
using System.Collections.Generic;
using System.Text;

namespace MyTasque.Lib
{
	public interface INote
	{
		string Text { get; set; }

		DateTime CreationDate { get; set; }

		ChangeType Change { get; set; }
	}
}

