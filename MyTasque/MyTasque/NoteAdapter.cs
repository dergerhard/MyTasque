using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MyTasque.Lib;


namespace MyTasque
{
	/// <summary>
	/// Note adapter for displaying multiple notes.
	/// </summary>
	public class NoteAdapter : BaseAdapter<INote> {

		/// <summary>
		/// The context - needed for opening dialogues
		/// </summary>
		private Activity context;

		/// <summary>
		/// The task used in this view.
		/// </summary>
		private ITask TaskUsed;

		/// <summary>
		/// Initializes a new instance of the <see cref="MyTasque.NoteAdapter"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="task">Task.</param>
		public NoteAdapter(Activity context, ITask task=null)
			: base()
		{
			this.context = context;
			this.TaskUsed = task;
		}

		/// <summary>
		/// Gets the item identifier.
		/// </summary>
		/// <returns>The item identifier.</returns>
		/// <param name="position">Position.</param>
		public override long GetItemId(int position)
		{
			return position;
		}

		/// <summary>
		/// Gets the <see cref="MyTasque.NoteAdapter"/> with the specified position.
		/// </summary>
		/// <param name="position">Position.</param>
		public override INote this[int position]
		{   
			get { return TaskUsed.ElementAt(position); } 
		}

		/// <summary>
		/// Gets the count.
		/// </summary>
		/// <value>The count.</value>
		public override int Count {
			get { return TaskUsed!=null ? TaskUsed.Count : 0; }

		}

		/// <summary>
		/// Gets the view.
		/// </summary>
		/// <returns>The view.</returns>
		/// <param name="position">Position.</param>
		/// <param name="convertView">Convert view.</param>
		/// <param name="parent">Parent.</param>
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			LayoutInflater vi = LayoutInflater.From (context);
			View v = vi.Inflate (Resource.Layout.NoteItem, null);

			TextView tvNote = v.FindViewById<TextView> (Resource.Id.tvNote);
			ImageButton btDeleteNote = v.FindViewById<ImageButton> (Resource.Id.btDeleteNote); 

			INote item = TaskUsed.ElementAt(position);

			tvNote.Text = item.Text;
			btDeleteNote.Click += delegate {
				TaskUsed.Remove(TaskUsed.ElementAt(position));
				this.NotifyDataSetInvalidated();
				this.NotifyDataSetChanged();
			};

			//edit note
			tvNote.Click += delegate {
				AlertDialog.Builder dialog = new AlertDialog.Builder (context);
				dialog.SetTitle (context.GetString(Resource.String.editNoteTitle));
				EditText input = new EditText (context);
				input.SetText (context.FindViewById<TextView> (Resource.Id.tvNote).Text, TextView.BufferType.Normal);
				dialog.SetView(input);
				dialog.SetPositiveButton(context.GetString(Resource.String.btOk), (sender, args) =>
				                         {
					if (input.Text.Count() >0)
					{
						TaskUsed.ElementAt(position).Text = input.Text;
					}
				});

				dialog.SetNegativeButton(context.GetString(Resource.String.btCancel), (sender, args) =>
				                         {
					dialog.Dispose();
				});
				dialog.Show();
			};

			return v;
		}
	}
}

