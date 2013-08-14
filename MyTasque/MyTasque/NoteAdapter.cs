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
	public class NoteAdapter : BaseAdapter<INote> {

		private Activity context;

		private ITask TaskUsed;

		public NoteAdapter(Activity context, ITask task=null)
			: base()
		{
			this.context = context;
			this.TaskUsed = task;

		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override INote this[int position]
		{   
			get { return TaskUsed.ElementAt(position); } 
		}

		public override int Count {
			get { return TaskUsed!=null ? TaskUsed.Count : 0; }

		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			LayoutInflater vi = LayoutInflater.From (context);
			View v = vi.Inflate (Resource.Layout.NoteItem, null);

			TextView tvNote = v.FindViewById<TextView> (Resource.Id.tvNote);
			ImageButton btDeleteNote = v.FindViewById<ImageButton> (Resource.Id.btDeleteNote); 

			INote item = TaskUsed.ElementAt(position);

			tvNote.Text = item.Text;
			//tvNote.Click += 
			//btDeleteNote.Click += 

			return v;
		}
	}
}

