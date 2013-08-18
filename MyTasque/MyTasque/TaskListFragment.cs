using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Fragment = Android.Support.V4.App.Fragment;
using MyTasque.Lib;
using MyTasque.Lib.Backend;

namespace MyTasque
{
	/// <summary>
	/// Task list fragment.
	/// </summary>
	public class TaskListFragment : Fragment 
	{
		/// <summary>
		/// The section number.
		/// </summary>
		public static String SectionNumber = "section_number";

		/// <summary>
		/// The list view for the tasks.
		/// </summary>
		private ListView lvTasks;

		/// <summary>
		/// Gets the current task list.
		/// </summary>
		/// <value>The current task list.</value>
		public ITaskList CurrentTaskList { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="MyTasque.TaskListFragment"/> class.
		/// </summary>
		/// <param name="tasklist">Tasklist.</param>
		public TaskListFragment(ITaskList tasklist)
		{
			this.CurrentTaskList = tasklist;
		}

		/// <summary>
		/// Raises the create view event.
		/// </summary>
		/// <param name="inflater">Inflater.</param>
		/// <param name="container">Container.</param>
		/// <param name="savedInstanceState">Saved instance state.</param>
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) 
		{
			var v = inflater.Inflate(Resource.Layout.TaskList, container, false);
			lvTasks = v.FindViewById<ListView> (Resource.Id.lvTasks); 

			// populate the listview with data
			lvTasks.Adapter = new TaskListAdapter (this.Activity, CurrentTaskList);


			ImageButton btAddNewTask = v.FindViewById<ImageButton> (Resource.Id.btAddNewTask);
			btAddNewTask.Click += BtAddTaskClicked;

			return v;
		}

		/// <summary>
		/// btAddTask.Click event. Adds a new task to the list.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		public void BtAddTaskClicked(object sender, EventArgs e)
		{
			IBackend backend = TaskRepository.Instance.Manager.Backend;

			AlertDialog.Builder dialog = new AlertDialog.Builder (this.Activity);
			dialog.SetTitle (this.GetString(Resource.String.createNewTaskTitle));
			string errorString = "";
			EditText input = new EditText (this.Activity);
			input.SetSingleLine ();
			input.SetHint (Resource.String.edNewTaskHint);
			dialog.SetView(input);
			dialog.SetPositiveButton(GetString(Resource.String.btOk), (sender2, args2) =>
			                         {
				if (input.Text.Count() >0)
				{
					try 
					{
						ITask t = CurrentTaskList.CreateTask(input.Text, DateTime.Now, false);
						((TaskListAdapter)lvTasks.Adapter).AddToFilteredList(t);
						backend.Sync();
					}
					catch (Exception ex)
					{
						errorString = ex.Message.ToString();
					}
				}
			});

			dialog.SetNegativeButton(GetString(Resource.String.btCancel), (sender3, args3) =>
			                         {
				dialog.Dispose();
			});
			dialog.Show();

			if (!errorString.Equals (""))
				Toast.MakeText (this.Activity, errorString, ToastLength.Long).Show();

		}
	}
}

