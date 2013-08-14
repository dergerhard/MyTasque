using System;
using System.Collections.Generic;
using System.Text;

using MyTasque.Lib;

namespace MyTasque.Backends
{
	public class DummyNote : MyTasqueObject, INote
	{
		public DummyNote()
		{
		}

		public override int Id {get; set;}

		/// <summary>
		/// Gets or sets the text.
		/// </summary>
		/// <value>The text.</value>
		public string Text { 
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the creation date.
		/// </summary>
		/// <value>The creation date.</value>
		public DateTime CreationDate { 
			get;
			set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MyTasque.Backends.DummyNote"/> class.
		/// </summary>
		/// <param name="text">Text.</param>
		public DummyNote(string text)
		{
			this.Text = text;
			this.CreationDate = DateTime.MinValue;
			this.Change = ChangeType.NoChange;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MyTasque.Backends.DummyNote"/> class.
		/// </summary>
		/// <param name="text">Text.</param>
		/// <param name="creationDate">Creation date.</param>
		public DummyNote(string text, DateTime creationDate)
		{
			this.Text = text;
			this.CreationDate = creationDate;
			this.Change = ChangeType.NoChange;
		}


	}
}

