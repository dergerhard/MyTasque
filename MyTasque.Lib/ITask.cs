using System;
using System.Collections.Generic;
using System.Text;

namespace MyTasque.Lib
{
	/// <summary>
	/// I task.
	/// </summary>
	public interface ITask : ICollection<INote>
	{
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		string Name { get; set; }

		/// <summary>
		/// Gets or sets the due date.
		/// </summary>
		/// <value>The due date.</value>
		DateTime DueDate { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="MyTasque.Lib.ITask"/> is completed.
		/// </summary>
		/// <value><c>true</c> if completed; otherwise, <c>false</c>.</value>
		bool Completed { get; set; }

		/// <summary>
		/// Gets or sets the change.
		/// </summary>
		/// <value>The change.</value>
		ChangeType Change { get; set; }

		/// <summary>
		/// Creates the note.
		/// </summary>
		/// <returns>The note.</returns>
		/// <param name="text">Text.</param>
		INote CreateNote(string text);

	}
}

