using System;
using System.Collections.Generic;
using System.Text;

namespace MyTasque.Lib
{
	public interface ITask : ICollection<INote>
	{
		string Name { get; set; }

		DateTime DueDate { get; set; }

		bool Completed { get; set; }

		ChangeType Change { get; set; }

		INote CreateNote(string text);

	}
}

