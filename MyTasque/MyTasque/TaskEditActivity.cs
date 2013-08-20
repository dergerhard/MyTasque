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
	/// Task edit activity - responsible for editing a task.
	/// </summary>
	[Activity (Label = "@string/TaskEditActivity")]			
	public class TaskEditActivity : Activity, Android.App.DatePickerDialog.IOnDateSetListener, Android.App.TimePickerDialog.IOnTimeSetListener
	{
		/// <summary>
		/// Gets the current task.
		/// </summary>
		/// <value>The current task.</value>
		public ITask CurrentTask { get; private set; }

		/// <summary>
		/// The task date - for the date picker
		/// </summary>
		private DateTime taskDate;

		/// <summary>
		/// The task time - for the time picker
		/// </summary>
		private DateTime taskTime;

		/// <summary>
		/// The save task. If true, the user left with the save button and changes are saved, if not, changes are discarded.
		/// </summary>
		private bool saveTask = false;

		/// <summary>
		/// Raises the create event.
		/// </summary>
		/// <param name="bundle">Bundle.</param>
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			this.RequestWindowFeature (WindowFeatures.ActionBar);
			this.SetContentView (Resource.Layout.TaskEditScreen);

			CurrentTask = TaskRepository.Instance.TaskBuffer;
			//this has to be stored, because there is no DateTimePicker --> DueDate is generated from these two
			taskDate = CurrentTask.DueDate;
			taskTime = CurrentTask.DueDate;

			// populate the listview with data
			ListView lvNotes = this.FindViewById<ListView> (Resource.Id.lvNotes); 
			lvNotes.Adapter = new NoteAdapter (this, CurrentTask);

			EditText edTaskText = this.FindViewById<EditText> (Resource.Id.edTaskText);
			TextView tvTaskDueDate = this.FindViewById<TextView> (Resource.Id.tvTaskDueDate);
			TextView tvTaskDueTime = this.FindViewById<TextView> (Resource.Id.tvTaskDueTime);
			CheckBox cbTaskCompleted = this.FindViewById<CheckBox> (Resource.Id.cbTaskCompleted);
			ImageButton btAddNote = this.FindViewById<ImageButton> (Resource.Id.btAddNewNote);


			//Date picker
			tvTaskDueDate.Click += delegate {
				var dialog = new DatePickerDialogFragment(this, taskDate, this);
				dialog.Show(FragmentManager, null);
			};

			//Time picker
			tvTaskDueTime.Click += delegate {
				var dialog = new TimePickerDialogFragment(this, taskTime, this);
				dialog.Show(FragmentManager, null);
			};

			edTaskText.Text = CurrentTask.Name;
			tvTaskDueDate.Text = taskDate.ToShortDateString ();
			tvTaskDueTime.Text = taskTime.ToShortTimeString ();
			cbTaskCompleted.Checked = CurrentTask.Completed;

			//add note
			btAddNote.Click += OnBtAddNoteClick;
		}

		/// <summary>
		/// Raises the bt add note click event. Shows a dialog to create a new note.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="args">Arguments.</param>
		public void OnBtAddNoteClick(object sender, EventArgs args)
		{
			AlertDialog.Builder dialog = new AlertDialog.Builder (this);
			dialog.SetTitle (this.GetString(Resource.String.createNoteTitle));
			EditText input = new EditText (this);
			dialog.SetView(input);
			dialog.SetPositiveButton(this.GetString(Resource.String.btOk), (sender2, args2) =>
			                         {
				string text = input.Text.Trim();
				if (text.Count() >0)
				{
					CurrentTask.CreateNote(text);
					((NoteAdapter)this.FindViewById<ListView>(Resource.Id.lvNotes).Adapter).NotifyDataSetChanged();
				}
			});

			dialog.SetNegativeButton(this.GetString(Resource.String.btCancel), (sender2, args2) =>
			                         {
				dialog.Dispose();
			});
			dialog.Show();

		}

		/// <summary>
		/// Raises the date set event. Raised, when DatePicker is finished.
		/// </summary>
		/// <param name="view">View.</param>
		/// <param name="year">Year.</param>
		/// <param name="monthOfYear">Month of year.</param>
		/// <param name="dayOfMonth">Day of month.</param>
		public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
		{
			taskDate = new DateTime(year, monthOfYear + 1, dayOfMonth);
			this.FindViewById<TextView>(Resource.Id.tvTaskDueDate).Text = taskDate.ToShortDateString();
			CurrentTask.DueDate = new DateTime (taskDate.Year, taskTime.Month, taskDate.Day, taskTime.Hour, taskTime.Minute, taskTime.Second);
		}


		/// <summary>
		/// Raises the time set event. Raised when TimePicker is finished.
		/// </summary>
		/// <param name="view">View.</param>
		/// <param name="hourOfDay">Hour of day.</param>
		/// <param name="minute">Minute.</param>
		public void OnTimeSet (TimePicker view, int hourOfDay, int minute)
		{
			taskTime = new DateTime (taskTime.Year, taskTime.Month, taskTime.Day, hourOfDay, minute, 0);
			this.FindViewById<TextView>(Resource.Id.tvTaskDueTime).Text = taskTime.ToShortTimeString();
			CurrentTask.DueDate = new DateTime (taskDate.Year, taskTime.Month, taskDate.Day, taskTime.Hour, taskTime.Minute, taskTime.Second);
		}

		/// <summary>
		/// Raises the create options menu event.
		/// </summary>
		/// <param name="menu">Menu.</param>
		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate (Resource.Menu.ActionBarTaskEdit, menu);
			return true;
		}

		/// <summary>
		/// Raises the options item selected event.
		/// </summary>
		/// <param name="item">Item.</param>
		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
			case Resource.Id.menuTaskEditSave:
				if (! this.FindViewById<EditText> (Resource.Id.edTaskText).Text.Equals(""))
				{
					this.saveTask = true;
					this.Finish ();
				}
				else 
				{
					Toast.MakeText (this, Resource.String.taskNameCannotBeEmpty, ToastLength.Long);
				}
				break;
			}
			return true;
		}
		

		/// <summary>
		/// Raises the destroy event. Gives back the updated task
		/// </summary>
		protected override void OnDestroy ()
		{
			base.OnDestroy ();
			if (saveTask) {
				CurrentTask.Name = this.FindViewById<EditText> (Resource.Id.edTaskText).Text;
				CurrentTask.Completed = this.FindViewById<CheckBox> (Resource.Id.cbTaskCompleted).Checked;
				//todo: notes
				TaskRepository.Instance.TaskBuffer = CurrentTask;
				TaskRepository.Instance.Sync ();
				TaskRepository.Instance.TaskListAdapterBuffer.NotifyDataSetChanged ();
			}
		}


	}
}

