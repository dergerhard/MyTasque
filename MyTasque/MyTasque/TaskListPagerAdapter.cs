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
using Fragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;
using FragmentPagerAdapter = Android.Support.V4.App.FragmentPagerAdapter;
using MyTasque.Lib;


namespace MyTasque
{
	/// <summary>
	/// Task list pager adapter. Responsible for retreiving the right TaskList fragment (swiping)
	/// </summary>
	public class TaskListPagerAdapter : FragmentPagerAdapter 
	{
		private List<TaskListFragment> TaskListFragments; 

		public TaskListPagerAdapter(FragmentManager fm) : base(fm)
		{
			this.TaskListFragments = new List<TaskListFragment> ();
			this.ReloadData ();
		}

		public void ReloadData()
		{
			this.TaskListFragments.Clear ();
			foreach (ITaskList tl in TaskRepository.Instance.Manager.Backend.AllTaskLists) 
			{
				TaskListFragments.Add(new TaskListFragment(tl));
			}
		}


		public override Fragment GetItem(int i) 
		{
			TaskListFragment tf = TaskListFragments.ElementAt (i);
			Bundle args = new Bundle();
			args.PutInt(TaskListFragment.ARG_SECTION_NUMBER, i + 1);
			tf.Arguments = args;
			return tf;

		}

		public override int Count { get { return TaskListFragments.Count; } }

		public string GetTaskListName(int i)
		{
			return TaskListFragments.ElementAt (i).CurrentTaskList.Name;
		}

	
	}
}

