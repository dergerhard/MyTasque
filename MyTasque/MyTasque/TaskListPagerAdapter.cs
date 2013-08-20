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
using FragmentStatePagerAdapter = Android.Support.V4.App.FragmentStatePagerAdapter;
using MyTasque.Lib;


namespace MyTasque
{
	/// <summary>
	/// Task list pager adapter. Responsible for retreiving the right TaskList fragment (swiping)
	/// </summary>
	public class TaskListPagerAdapter : FragmentStatePagerAdapter 
	{
		/// <summary>
		/// The task list fragments created in this adapter
		/// </summary>
		private List<TaskListFragment> TaskListFragments; 

		/// <summary>
		/// Initializes a new instance of the <see cref="MyTasque.TaskListPagerAdapter"/> class.
		/// </summary>
		/// <param name="fm">Fm.</param>
		public TaskListPagerAdapter(FragmentManager fm) : base(fm)
		{
			this.TaskListFragments = new List<TaskListFragment> ();
			this.ReloadData ();
		}

		/// <summary>
		/// Reloads the data from the backend and creates new fragments
		/// </summary>
		public void ReloadData()
		{
			foreach (var tlf in TaskListFragments)
				tlf.Dispose ();

			this.TaskListFragments.Clear ();
			foreach (ITaskList tl in TaskRepository.Instance.AllTaskLists) 
			{
				TaskListFragments.Add(new TaskListFragment(tl));
			}
		}

		/// <summary>
		/// Gets the item.
		/// </summary>
		/// <returns>The item.</returns>
		/// <param name="i">The index.</param>
		public override Fragment GetItem(int i) 
		{
			TaskListFragment tf = TaskListFragments.ElementAt (i);
			Bundle args = new Bundle();
			args.PutInt(TaskListFragment.SectionNumber, i + 1);
			tf.Arguments = args;
			return tf;

		}

		/// <summary>
		/// Gets the count.
		/// </summary>
		/// <value>The count.</value>
		public override int Count { get { return TaskListFragments.Count; } }

		/// <summary>
		/// Gets the name of the task list.
		/// </summary>
		/// <returns>The task list name.</returns>
		/// <param name="i">The index.</param>
		public string GetTaskListName(int i)
		{
			return TaskListFragments.ElementAt (i).CurrentTaskList.Name;
		}		
	}
}

