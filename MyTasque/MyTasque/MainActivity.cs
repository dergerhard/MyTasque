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
	[Activity (Label = "@string/app_name", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainActivity : FragmentActivity, ActionBar.ITabListener
	{
		/// <summary>
		/// The view pager of the HomeScreen.
		/// </summary>
		private ViewPager mViewPager;

		/// <summary>
		/// The task list page adapter for horizontal swiping.
		/// </summary>
		private TaskListPagerAdapter mTaskListPageAdapter;

		/// <summary>
		/// The backend. For easier acces (avoiding TaskRepository.Instance.Manager.Backend)
		/// </summary>
		//DEL private IBackend backend;

		/// <summary>
		/// Raises the create event.
		/// </summary>
		/// <param name="bundle">Bundle.</param>
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			this.SetContentView (Resource.Layout.HomeScreen);

			//define platform specific translator
			Translator.Instance.ConcreteTranslator = new AndroidTranslator (this);

			//Initialize Backend/TaskRepository
			//DEL BackendManager ma = TaskRepository.Instance.Manager;
			TaskRepository.Instance.Activity = this;

			//Load the correct backend
			TaskRepository.Instance.SetActiveBackendFromPreferencesAndInitializeAndSync ();
			//DEL backend = TaskRepository.Instance.Manager.Backend;

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

		/// <summary>
		/// Raises the pause event. Use to sync Tasks.
		/// </summary>
		protected override void OnPause ()
		{
			base.OnPause ();
			TaskRepository.Instance.Sync ();
		}

		/// <summary>
		/// Creates/Reloads the TaskLists swipe fragments
		/// </summary>
		public void RealoadTaskListView()
		{
			//ensure all is configured correctly and synced
			//DEL backend = TaskRepository.Instance.Manager.Backend;

			if (mTaskListPageAdapter!=null)
				mTaskListPageAdapter.Dispose ();


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

		}

		/// <summary>
		/// Restart this instance.
		/// </summary>
		public void Restart()
		{
			Intent intent = this.Intent;
			this.Finish();
			this.StartActivity(intent);
		}

		/// <summary>
		/// Raises the create options menu event.
		/// </summary>
		/// <param name="menu">Menu.</param>
		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater inflater = this.MenuInflater;
			inflater.Inflate(Resource.Menu.ActionBar, menu);
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
			case Resource.Id.menuSettings:
				this.StartActivity (typeof(TasquePreferenceActivity));
				break;

			case Resource.Id.menuAddTaskList:
				this.TaskListCreateChange(GetString (Resource.String.adEnterTaskListName)); 
				break;

			case Resource.Id.menuChangeTaskListName:
				this.TaskListCreateChange (GetString (Resource.String.adEnterTaskListName), TaskRepository.Instance.AllTaskLists[mViewPager.CurrentItem].Name);
				break;

			case Resource.Id.menuDeleteTaskList:
				try
				{
					AlertDialog.Builder dialog = new AlertDialog.Builder (this);
					dialog.SetTitle (this.GetString(Resource.String.deleteTaskListTitle));
					dialog.SetMessage(this.GetString(Resource.String.deleteTaskListMessage));

					dialog.SetPositiveButton(GetString(Resource.String.btOk), (sender, args) =>
					                         {
						TaskRepository.Instance.DeleteTaskList(TaskRepository.Instance.AllTaskLists[mViewPager.CurrentItem]);
						this.RealoadTaskListView();
						TaskRepository.Instance.Sync ();
					});

					dialog.SetNegativeButton(GetString(Resource.String.btCancel), (sender, args) =>
					                         {
						dialog.Dispose();
					});
					dialog.Show();

				}
				catch (Exception e) {
					Toast.MakeText (this, e.Message.ToString (), ToastLength.Long).Show ();
				}
				break;

			case Resource.Id.menuAbout:
				this.StartActivity (typeof(AboutActivity));
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
							TaskRepository.Instance.CreateTaskList(input.Text);
						}
						catch (Exception e)
						{
							errorString = e.Message.ToString();
						}
					}
					else 
					{
						TaskRepository.Instance.AllTaskLists.Single(x => x.Name.Equals(previousName)).Name=input.Text;
					}

					try {
						this.RealoadTaskListView();
						TaskRepository.Instance.Sync();
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
		/// Raises the tab unselected event.
		/// </summary>
		/// <param name="tab">Tab.</param>
		/// <param name="fragmentTransaction">Fragment transaction.</param>
		public void OnTabUnselected(ActionBar.Tab tab, FragmentTransaction fragmentTransaction) 
		{
			//nothing todo here
		}

		/// <summary>
		/// Raises the tab selected event.
		/// </summary>
		/// <param name="tab">Tab.</param>
		/// <param name="fragmentTransaction">Fragment transaction.</param>
		public void OnTabSelected(ActionBar.Tab tab, FragmentTransaction fragmentTransaction) 
		{
			// When the given tab is selected, switch to the corresponding page in the ViewPager.
			mViewPager.CurrentItem = tab.Position;
		}

		/// <summary>
		/// Raises the tab reselected event.
		/// </summary>
		/// <param name="tab">Tab.</param>
		/// <param name="fragmentTransaction">Fragment transaction.</param>
		public void OnTabReselected(ActionBar.Tab tab, FragmentTransaction fragmentTransaction) 
		{
		}

	}




	/// <summary>
	/// Page change listener. 
	/// </summary>
	public class PageChangeListener : ViewPager.SimpleOnPageChangeListener
	{
		/// <summary>
		/// The action bar for selecting the right tab on page change.
		/// </summary>
		ActionBar actionBar;

		/// <summary>
		/// Initializes a new instance of the <see cref="MyTasque.PageChangeListener"/> class.
		/// </summary>
		/// <param name="ab">Ab.</param>
		public PageChangeListener(ActionBar ab)
		{
			this.actionBar = ab;
		}

		/// <summary>
		/// Raises the page selected event.
		/// </summary>
		/// <param name="position">Position.</param>
		public override void OnPageSelected(int position) 
		{
			// When swiping between different app sections, select the corresponding tab.
			// We can also use ActionBar.Tab#select() to do this if we have a reference to the
			// Tab.
			actionBar.SetSelectedNavigationItem(position);
		}

	}
}

