using System;
using System.Collections.Generic;
using System.Text;

using MyTasque.Lib;

namespace MyTasque.Backends
{
	public class LocalTask : MyTasqueObject, ITask
	{
		public LocalTask()
		{
			Notes = new List<INote> ();
			DeletedNotes = new List<INote> ();
		}

		/// <summary>
		/// Gets the notes.
		/// </summary>
		/// <value>The notes.</value>
		public List<INote> Notes { get; private set; }

		/// <summary>
		/// Gets or sets the deleted notes.
		/// </summary>
		/// <value>The deleted notes.</value>
		private List<INote> DeletedNotes { get; set; }

		/// <Docs>The item to add to the current collection.</Docs>
		/// <para>Adds an item to the current collection.</para>
		/// <remarks>To be added.</remarks>
		/// <exception cref="System.NotSupportedException">The current collection is read-only.</exception>
		/// <summary>
		/// Add the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		public void Add (INote item)
		{
			item.Change = ChangeType.Created;
			Notes.Add (item);
		}

		/// <summary>
		/// Clear this instance.
		/// </summary>
		public void Clear ()
		{
			DeletedNotes.AddRange (Notes);
			Notes.Clear ();
		}

		/// <Docs>The object to locate in the current collection.</Docs>
		/// <para>Determines whether the current collection contains a specific value.</para>
		/// <summary>
		/// Contains the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		public bool Contains (INote item)
		{
			return Notes.Contains (item);
		}

		/// <summary>
		/// Copies to.
		/// </summary>
		/// <param name="array">Array.</param>
		/// <param name="arrayIndex">Array index.</param>
		public void CopyTo (INote[] array, int arrayIndex)
		{
			Notes.CopyTo (array, arrayIndex);
		}

		/// <Docs>The item to remove from the current collection.</Docs>
		/// <para>Removes the first occurrence of an item from the current collection.</para>
		/// <summary>
		/// Remove the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		public bool Remove (INote item)
		{
			item.Change = ChangeType.Deleted;
			DeletedNotes.Add (item);
			return Notes.Remove (item);
		}

		/// <summary>
		/// Gets the count.
		/// </summary>
		/// <value>The count.</value>
		public int Count { get { return Notes.Count; } }

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
		public IEnumerator<INote> GetEnumerator ()
		{
			return Notes.GetEnumerator ();
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
		/// Gets or sets the due date.
		/// </summary>
		/// <value>The due date.</value>
		public DateTime DueDate { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="MyTasque.Backends.LocalTask"/> is completed.
		/// </summary>
		/// <value><c>true</c> if completed; otherwise, <c>false</c>.</value>
		public bool Completed { get; set; }

		/// <summary>
		/// Creates the note.
		/// </summary>
		/// <returns>The note.</returns>
		/// <param name="text">Text.</param>
		public INote CreateNote(string text)
		{
			INote n = new LocalNote (text);
			this.Add (n);
			return n;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MyTasque.Backends.LocalTask"/> class.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="dueDate">Due date.</param>
		/// <param name="completed">If set to <c>true</c> completed.</param>
		public LocalTask(string name, DateTime dueDate, bool completed=false)
		{
			this.Name = name;
			this.DueDate = dueDate;
			this.Completed = completed;
			this.Change = ChangeType.NoChange;
			this.Notes = new List<INote> ();
			this.DeletedNotes = new List<INote> ();
		}


	}

}

