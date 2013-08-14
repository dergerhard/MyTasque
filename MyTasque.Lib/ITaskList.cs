using System;
using System.Collections.Generic;
using System.Text;

namespace MyTasque.Lib
{
	public interface ITaskList : ICollection<ITask>
	{
		string Name { get; set; }

		ITask CreateTask (string name, DateTime dueDate, bool completed=false);

		ChangeType Change { get; set; }
	}
}

