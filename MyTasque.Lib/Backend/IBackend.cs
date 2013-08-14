using System;
using System.Collections.Generic;
using System.Text;

using MyTasque.Lib;

namespace MyTasque.Lib.Backend
{
	/// <summary>
	/// IBackend defines how a Backend has to look like.
	/// </summary>
	public interface IBackend : IDisposable
	{
		/// <summary>
		/// Gets all task lists.
		/// </summary>
		/// <value>All task lists.</value>
		IList<ITaskList> AllTaskLists { get;  }

		/// <summary>
		/// Creates the task list and returns it
		/// </summary>
		/// <returns>The task list.</returns>
		/// <param name="name">Name.</param>
		ITaskList CreateTaskList (string name);

		/// <summary>
		/// Deletes the task list.
		/// </summary>
		/// <param name="tasklist">Tasklist.</param>
		void DeleteTaskList (ITaskList tasklist);

		/// <summary>
		/// Gets or sets the context.
		/// </summary>
		/// <value>The context.</value>
		BackendContext Context { get; set; }

		/// <summary>
		/// Initialize this instance.
		/// </summary>
		void Initialize(BackendContext ctx);

		/// <summary>
		/// Gets a value indicating whether this instance is initialized.
		/// </summary>
		/// <value><c>true</c> if this instance is initialized; otherwise, <c>false</c>.</value>
		bool IsInitialized { get; }

		/// <summary>
		/// Sync the Task lists if it is initialized
		/// </summary>
		void Sync ();
		
		//TaskListRepository TaskListRepository { get; }
		//TaskRepository TaskRepository { get; }
		//NoteRepository NoteRepository { get; }

	}
}



