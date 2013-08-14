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

namespace MyTasque
{

	public class TaskListFragment : Fragment 
	{
		public static String ARG_SECTION_NUMBER = "section_number";

		private ListView lvTasks;

		public ITaskList CurrentTaskList { get; private set; }

		public TaskListFragment(ITaskList tasklist)
		{
			this.CurrentTaskList = tasklist;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) 
		{
			var v = inflater.Inflate(Resource.Layout.TaskList, container, false);
			lvTasks = v.FindViewById<ListView> (Resource.Id.lvTasks); 
			// populate the listview with data
			lvTasks.Adapter = new TaskListAdapter (this.Activity, CurrentTaskList);

			return v;
		}
	}
}

