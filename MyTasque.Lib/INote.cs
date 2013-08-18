using System;
using System.Collections.Generic;
using System.Text;

namespace MyTasque.Lib
{
	/// <summary>
	/// I note.
	/// </summary>
	public interface INote
	{
		/// <summary>
		/// Gets or sets the text.
		/// </summary>
		/// <value>The text.</value>
		string Text { get; set; }

		/// <summary>
		/// Gets or sets the creation date.
		/// </summary>
		/// <value>The creation date.</value>
		DateTime CreationDate { get; set; }

		/// <summary>
		/// Gets or sets the change.
		/// </summary>
		/// <value>The change.</value>
		ChangeType Change { get; set; }
	}
}

