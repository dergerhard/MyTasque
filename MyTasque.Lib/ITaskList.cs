using System;
using System.Collections.Generic;
using System.Text;

namespace MyTasque.Lib
{
	/// <summary>
	/// Order by type.
	/// </summary>
	public enum OrderByType { Name, DueDate }

	/// <summary>
	/// I task list.
	/// </summary>
	public interface ITaskList : ICollection<ITask>
	{
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		string Name { get; set; }

		/// <summary>
		/// Creates the task.
		/// </summary>
		/// <returns>The task.</returns>
		/// <param name="name">Name.</param>
		/// <param name="dueDate">Due date.</param>
		/// <param name="completed">If set to <c>true</c> completed.</param>
		ITask CreateTask (string name, DateTime dueDate, bool completed=false);

		/// <summary>
		/// Gets or sets the change.
		/// </summary>
		/// <value>The change.</value>
		ChangeType Change { get; set; }

		/// <summary>
		/// Orders the by.
		/// </summary>
		/// <param name="type">Type.</param>
		void OrderTasksBy (OrderByType type);
	}
}

