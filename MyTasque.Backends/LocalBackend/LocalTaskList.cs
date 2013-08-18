using System;
using System.Collections.Generic;
using System.Text;
using MyTasque.Lib;

namespace MyTasque.Backends
{
	/// <summary>
	/// Dummy task list. For testing purpose only.
	/// </summary>
	public class LocalTaskList : MyTasqueObject, ITaskList
	{
		public LocalTaskList()
		{
		}

		//Tasks are being saved here
		private List<ITask> Tasks = new List<ITask>();

		private List<ITask> DeletedTasks = new List<ITask> ();

		/// <summary>
		/// Initializes a new instance of the <see cref="MyTasque.Backends.LocalTaskList"/> class.
		/// </summary>
		/// <param name="name">Name.</param>
		public LocalTaskList(string name)
		{
			this.Name = name;
			this.Change = ChangeType.NoChange;
		}

		/// <Docs>The item to add to the current collection.</Docs>
		/// <para>Adds an item to the current collection.</para>
		/// <remarks>To be added.</remarks>
		/// <exception cref="System.NotSupportedException">The current collection is read-only.</exception>
		/// <summary>
		/// Add the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		public void Add (ITask item)
		{
			foreach (ITask tl in Tasks)
				if (tl.Name.Equals(item.Name))
					throw new ArgumentException ("A task with the same name already exists");

			//for some reason this does not work here... 
			//if (Tasks.Any (x => x.Name.Equals (item.Name)))

			item.Change = ChangeType.Created;
			Tasks.Add (item);
		}

		/// <summary>
		/// Clear this instance.
		/// </summary>
		public void Clear ()
		{
			foreach (var t in Tasks)
				DeletedTasks.Add (t);
			Tasks.Clear ();
		}

		/// <Docs>The object to locate in the current collection.</Docs>
		/// <para>Determines whether the current collection contains a specific value.</para>
		/// <summary>
		/// Contains the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		public bool Contains (ITask item)
		{
			return Tasks.Contains (item);
		}

		/// <summary>
		/// Copies to.
		/// </summary>
		/// <param name="array">Array.</param>
		/// <param name="arrayIndex">Array index.</param>
		public void CopyTo (ITask[] array, int arrayIndex)
		{
			Tasks.CopyTo (array, arrayIndex);
		}

		/// <Docs>The item to remove from the current collection.</Docs>
		/// <para>Removes the first occurrence of an item from the current collection.</para>
		/// <summary>
		/// Remove the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		public bool Remove (ITask item)
		{
			item.Change = ChangeType.Deleted;
			DeletedTasks.Add (item);
			return Tasks.Remove (item);
		}

		/// <summary>
		/// Gets the count.
		/// </summary>
		/// <value>The count.</value>
		public int Count { get { return Tasks.Count; } }

		/// <summary>
		/// Gets a value indicating whether this instance is read only.
		/// </summary>
		/// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
		public bool IsReadOnly {
			get {
				throw new NotImplementedException ();
			}
		}

		/// <summary>
		/// Gets the enumerator.
		/// </summary>
		/// <returns>The enumerator.</returns>
		public IEnumerator<ITask> GetEnumerator ()
		{
			return Tasks.GetEnumerator();
		}

		/// <summary>
		/// Gets the enumerator.
		/// </summary>
		/// <returns>The enumerator.</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			throw new NotImplementedException ();
		}

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; set; }

		/// <summary>
		/// Creates the task.
		/// </summary>
		/// <returns>The task.</returns>
		/// <param name="name">Name.</param>
		/// <param name="dueDate">Due date.</param>
		/// <param name="completed">If set to <c>true</c> completed.</param>
		public ITask CreateTask (string name, DateTime dueDate, bool completed=false)
		{
			ITask t = new LocalTask (name, dueDate, completed);
			this.Add (t);
			return t;
		}

		/// <summary>
		/// Orders by OrderByType.
		/// </summary>
		/// <param name="type">Type.</param>
		public void OrderTasksBy(OrderByType type)
		{
			switch (type) {
				case OrderByType.Name:
				Tasks.Sort ((x,y) => string.Compare (x.Name, y.Name));
				break;

				case OrderByType.DueDate:
				Tasks.Sort ((x,y) => DateTime.Compare (x.DueDate, y.DueDate));
				break;
			}
		}



	}
}

