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
using Android.Util;
using MyTasque.Lib;

namespace MyTasque
{
	/// <summary>
	/// Task list adapter. Responsible for showing UI-Representation of TaskLists
	/// </summary>
	public class TaskListAdapter : BaseAdapter<ITask> {

		private Activity context;

		private ITaskList TaskListUsed;

		public TaskListAdapter(Activity context, ITaskList taskList=null)
			: base()
		{
			this.context = context;
			this.TaskListUsed = taskList;

		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override ITask this[int position]
		{   
			get { return TaskListUsed.ElementAt(position); }
			//get { return new MyTask ("dd", DateTime.Now, true);}
			//get { return TaskList.[position]; } 
		}

		public override int Count {
			get { return TaskListUsed!=null ? TaskListUsed.Count : 0; }

		}

		/*public void addItem(MyTask item)
		{

		}*/

		public override View GetView(int position, View convertView, ViewGroup parent)
		{

			LayoutInflater vi = LayoutInflater.From (context);
			View v = vi.Inflate (Resource.Layout.TaskItem, null);

			TextView tvHeading = v.FindViewById<TextView> (Resource.Id.tvHeading);
			TextView tvDetail = v.FindViewById<TextView> (Resource.Id.tvDetail); 
			CheckBox cbDone = v.FindViewById<CheckBox> (Resource.Id.cbDone);

			ITask item = TaskListUsed.ElementAt(position);
			//list.OrderBy(x => x.Product.Name).toList()

			string heading = item.Name;
			string subheading = context.GetString(Resource.String.dueOn) + item.DueDate.ToShortDateString();

			tvHeading.Text = heading;
			tvDetail.Text = subheading;
			cbDone.Checked = item.Completed;

			tvHeading.Click += delegate {
				Intent intent = new Intent(context, typeof(TaskEditActivity));
				//save Task to singleton, so it can be accessed from TaskEditActivity
				TaskRepository.Instance.TaskBuffer = item;
				TaskRepository.Instance.TaskListAdapterBuffer = this;
				context.StartActivity(intent);

			};


			return v;
		}
	}
}

