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
using Android.Support.V4.App;
using Android.Support.V4.View;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentActivity = Android.Support.V4.App.FragmentActivity;
using FragmentManager = Android.Support.V4.App.FragmentManager;
using ViewPager = Android.Support.V4.View.ViewPager;
using FragmentPagerAdapter = Android.Support.V4.App.FragmentPagerAdapter;
using FragmentTransaction = Android.App.FragmentTransaction;
using Android.Util;
using MyTasque.Lib.Backend;
using MyTasque.Lib;
using Android.Content.PM;


namespace MyTasque
{
	[Activity (Label = "Tasque.Android", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainActivity : FragmentActivity, ActionBar.ITabListener
	{

		private ViewPager mViewPager;

		private TaskListPagerAdapter mTaskListPageAdapter;

		private IBackend backend;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.HomeScreen);

			//Initialize Backend/TaskRepository
			BackendManager ma = TaskRepository.Instance.Manager;

			TaskRepository.Instance.SetActiveBackendFromPreferences (this.GetSharedPreferences("preferences.xml", FileCreationMode.WorldWriteable));
			TaskRepository.Instance.Manager.Backend.Sync ();
			backend = TaskRepository.Instance.Manager.Backend;

			//Load data
			backend.Sync ();

			// find viewpager
			mViewPager = (ViewPager) this.FindViewById<ViewPager>(Resource.Id.pager);

			// Set up the action bar.
			ActionBar actionBar = this.ActionBar;

			// Specify that the Home/Up button should not be enabled, since there is no hierarchical
			// parent.
			actionBar.SetHomeButtonEnabled(false);

			// Specify that we will be displaying tabs in the action bar.
			actionBar.NavigationMode = ActionBarNavigationMode.Tabs;

			//Create tabbed view
			this.RealoadTaskListView ();

		}

		public void RealoadTaskListView()
		{
			// Create the adapter that will return a fragment for each of the three primary sections
			// of the app.
			mTaskListPageAdapter = new TaskListPagerAdapter(this.SupportFragmentManager);

			// Set up the ViewPager, attaching the adapter and setting up a listener for when the
			// user swipes between sections.
			mViewPager.Adapter = mTaskListPageAdapter;
			mViewPager.SetOnPageChangeListener (new PageChangeListener (ActionBar));

			//clear existing tabs
			ActionBar.RemoveAllTabs();

			// For each of the sections in the app, add a tab to the action bar.
			for (int i = 0; i < mTaskListPageAdapter.Count; i++) 
			{
				// Create a tab with text corresponding to the page title defined by the adapter.
				// Also specify this Activity object, which implements the TabListener interface, as the
				// listener for when this tab is selected.
				ActionBar.AddTab (ActionBar.NewTab ()
				                  .SetText (mTaskListPageAdapter.GetTaskListName (i))
				                  .SetTabListener (this));
			}

			this.FindViewById<Button> (Resource.Id.btAddTask).Click += BtAddTaskClicked;
		}


		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater inflater = this.MenuInflater;
			inflater.Inflate(Resource.Menu.ActionBar, menu);
			return true;
		}


		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
			case Resource.Id.menuSettings:
				this.StartActivity (typeof(TasquePreferenceActivity));
				break;

			case Resource.Id.menuAddTaskList:
				this.TaskListCreateChange(GetString (Resource.String.adEnterTaskListName)); 
				break;

			case Resource.Id.menuChangeTaskListName:
				this.TaskListCreateChange (GetString (Resource.String.adEnterTaskListName), backend.AllTaskLists[mViewPager.CurrentItem].Name);
				break;

			case Resource.Id.menuDeleteTaskList:
				try
				{
					backend.DeleteTaskList(backend.AllTaskLists[mViewPager.CurrentItem]);
					this.RealoadTaskListView();
					this.backend.Sync();
				}
				catch (Exception e) {
					Toast.MakeText (this, e.Message.ToString (), ToastLength.Long).Show ();
				}
				break;

				case Resource.Id.menuAbout:
				break;
			}
			return true;
		}


		/// <summary>
		/// Shows a dialog to change the name of a task list or create a new one
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="previousName">Previous name.</param>
		public void TaskListCreateChange(string title, string previousName="")
		{
			AlertDialog.Builder dialog = new AlertDialog.Builder (this);
			dialog.SetTitle (title);
			string errorString = "";
			EditText input = new EditText (this);
			input.SetSingleLine ();
			input.SetText (previousName, TextView.BufferType.Normal);
			dialog.SetView(input);
			dialog.SetPositiveButton(GetString(Resource.String.btOk), (sender, args) =>
			{
				if (input.Text.Count() >0)
				{
					if (previousName=="")
					{
						try 
						{
							backend.CreateTaskList(input.Text);
						}
						catch (Exception e)
						{
							errorString = e.Message.ToString();
						}
					}
					else 
					{
						backend.AllTaskLists.Single(x => x.Name.Equals(previousName)).Name=input.Text;
					}

					try {
						this.RealoadTaskListView();
						backend.Sync();
					} catch (Exception ex) {
						Toast.MakeText(this, ex.Message.ToString(), ToastLength.Long).Show();
					}
				}
			});

			dialog.SetNegativeButton(GetString(Resource.String.btCancel), (sender, args) =>
			{
				dialog.Dispose();
			});
			dialog.Show();

			if (!errorString.Equals (""))
				Toast.MakeText (this, errorString, ToastLength.Long).Show();
		}

	
		/// <summary>
		/// btAddTask.Click event. Adds a new task to the list.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		public void BtAddTaskClicked(object sender, EventArgs e)
		{
			ITaskList tl = backend.AllTaskLists [mViewPager.CurrentItem];
			try
			{
				tl.CreateTask (this.FindViewById<EditText> (Resource.Id.edNewTask).Text, DateTime.Now, false);
			}
			catch (Exception ex)
			{
				Toast.MakeText(this, ex.Message.ToString(), ToastLength.Long).Show();
			}
		}


		public void OnTabUnselected(ActionBar.Tab tab, FragmentTransaction fragmentTransaction) 
		{
		}

		public void OnTabSelected(ActionBar.Tab tab, FragmentTransaction fragmentTransaction) 
		{
			// When the given tab is selected, switch to the corresponding page in the ViewPager.
			mViewPager.CurrentItem = tab.Position;
		}

		public void OnTabReselected(ActionBar.Tab tab, FragmentTransaction fragmentTransaction) 
		{
		}







	}




	/// <summary>
	/// Page change listener. nee
	/// </summary>
	public class PageChangeListener : ViewPager.SimpleOnPageChangeListener
	{
		ActionBar actionBar;

		public PageChangeListener(ActionBar ab)
		{
			this.actionBar = ab;
		}

		public override void OnPageSelected(int position) 
		{
			// When swiping between different app sections, select the corresponding tab.
			// We can also use ActionBar.Tab#select() to do this if we have a reference to the
			// Tab.
			actionBar.SetSelectedNavigationItem(position);
		}

	}
}

