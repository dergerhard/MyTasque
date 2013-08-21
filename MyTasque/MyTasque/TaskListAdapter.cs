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
using Android.Preferences;

namespace MyTasque
{
	/// <summary>
	/// Task list adapter. Responsible for showing UI-Representation of TaskLists
	/// </summary>
	public class TaskListAdapter : BaseAdapter<ITask> {

		/// <summary>
		/// The context.
		/// </summary>
		private Activity context;

		/// <summary>
		/// The task list.
		/// </summary>
		private ITaskList TaskListUsed;

		/// <summary>
		/// The show completed.
		/// </summary>
		private bool showCompleted;

		/// <summary>
		/// The filtered tasks.
		/// </summary>
		private List<ITask> filteredTasks;

		/// <summary>
		/// Initializes a new instance of the <see cref="MyTasque.TaskListAdapter"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="taskList">Task list.</param>
		public TaskListAdapter(Activity context, ITaskList taskList)
			: base()
		{
			this.context = context;
			this.TaskListUsed = taskList;
			this.showCompleted=true;

			this.filteredTasks = TaskListUsed.ToList ();

			ISharedPreferences pref = PreferenceManager.GetDefaultSharedPreferences (context);

			showCompleted = pref.GetBoolean ("showCompleted", true);
			if (!showCompleted)
				filteredTasks.RemoveAll (x => x.Completed == true);
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
		/// Gets the <see cref="MyTasque.TaskListAdapter"/> with the specified position.
		/// </summary>
		/// <param name="position">Position.</param>
		public override ITask this[int position]
		{   
			get { return filteredTasks.ElementAt(position); }
		}

		/// <summary>
		/// Gets the count.
		/// </summary>
		/// <value>The count.</value>
		public override int Count {
			get { return filteredTasks.Count; }

		}

		/// <summary>
		/// Adds to filtered list.
		/// </summary>
		/// <param name="t">T.</param>
		public void AddToFilteredList(ITask t)
		{
			if (t.Completed == false)
				filteredTasks.Add (t);
			this.NotifyDataSetChanged ();
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
			View v = vi.Inflate (Resource.Layout.TaskItem, null);

			TextView tvHeading = v.FindViewById<TextView> (Resource.Id.tvHeading);
			TextView tvDetail = v.FindViewById<TextView> (Resource.Id.tvDetail); 
			CheckBox cbDone = v.FindViewById<CheckBox> (Resource.Id.cbDone);
			RelativeLayout rlTask = v.FindViewById<RelativeLayout> (Resource.Id.rlTask);
			ImageButton btDeleteTask = v.FindViewById<ImageButton> (Resource.Id.btDeleteTask);

			ITask item = filteredTasks.ElementAt(position);
			//list.OrderBy(x => x.Product.Name).toList()

			string notes = "";
			if (item.Count () == 1)
				notes = ", 1 " + context.GetString (Resource.String.note);
			else if (item.Count () > 1)
				notes = ", " + item.Count ().ToString () + " " + context.GetString (Resource.String.notes);

			string heading = item.Name;
			string subheading = context.GetString(Resource.String.dueOn) + item.DueDate.ToShortDateString() + ", " + item.DueDate.ToShortTimeString() + notes;

			tvHeading.Text = heading;
			tvDetail.Text = subheading;
			cbDone.Checked = item.Completed;

			// open TaskEditActivity
			rlTask.Click  += delegate {
				Intent intent = new Intent(context, typeof(TaskEditActivity));
				//save Task to singleton, so it can be accessed from TaskEditActivity
				TaskRepository.Instance.TaskBuffer = item;
				TaskRepository.Instance.TaskListAdapterBuffer = this;
				context.StartActivity(intent);
			};

			// delete task
			btDeleteTask.Click += delegate {
				try 
				{
					AlertDialog.Builder dialog = new AlertDialog.Builder (context);
					dialog.SetTitle (this.context.GetString(Resource.String.deleteTaskTitle));
					dialog.SetMessage(this.context.GetString(Resource.String.deleteTaskMessage));

					dialog.SetPositiveButton(this.context.GetString(Resource.String.btOk), (sender, args) =>
					                         {
						TaskListUsed.Remove(filteredTasks.ElementAt(position));
						filteredTasks.Remove(filteredTasks.ElementAt(position));
						this.NotifyDataSetChanged();
					});

					dialog.SetNegativeButton(this.context.GetString(Resource.String.btCancel), (sender, args) =>
					                         {
						dialog.Dispose();
					});
					dialog.Show();
				} catch (Exception ex) 
				{
					Toast.MakeText(context, ex.Message.ToString(), ToastLength.Long);
				}
			};

			// check finished
			cbDone.Click += delegate {
				filteredTasks.ElementAt(position).Completed = cbDone.Checked;

				if (!showCompleted)
					filteredTasks.Remove(filteredTasks.ElementAt(position));

				this.NotifyDataSetChanged();
			};

			return v;
		}
	}
}

