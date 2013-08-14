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
	[Activity (Label = "TaskEditActivity")]//, MainLauncher = true)]			
	public class TaskEditActivity : Activity
	{
		public ITask CurrentTask { get; private set; }

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			this.RequestWindowFeature (WindowFeatures.ActionBar);
			this.SetContentView (Resource.Layout.TaskEditScreen);

			CurrentTask = TaskRepository.Instance.TaskBuffer;

			// populate the listview with data
			ListView lvNotes = this.FindViewById<ListView> (Resource.Id.lvNotes); 
			lvNotes.Adapter = new NoteAdapter (this, CurrentTask);

			EditText edTaskText = this.FindViewById<EditText> (Resource.Id.edTaskText);
			EditText edTaskDueDate = this.FindViewById<EditText> (Resource.Id.edTaskDueDate);
			CheckBox cbTaskCompleted = this.FindViewById<CheckBox> (Resource.Id.cbTaskCompleted);

			edTaskText.Text = CurrentTask.Name;
			edTaskDueDate.Text = CurrentTask.DueDate.ToString ();
			cbTaskCompleted.Checked = CurrentTask.Completed;

		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate (Resource.Menu.ActionBarTaskEdit, menu);
			return true;
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
			case Resource.Id.menuTaskEditSave:
				this.Finish ();
				break;
			}
			return true;
		}

		protected override void OnDestroy ()
		{
			base.OnDestroy ();
			TaskRepository.Instance.TaskBuffer = CurrentTask;
			TaskRepository.Instance.Manager.Backend.Sync ();
			TaskRepository.Instance.TaskListAdapterBuffer.NotifyDataSetChanged ();
			Log.Debug ("TaskEditActivity", "onDestroy");
		}


	}
}

