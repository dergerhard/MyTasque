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


namespace MyTasque
{
	[Activity (Label = "Tasque.Android", MainLauncher = true)]
	public class MainActivity : FragmentActivity, ActionBar.ITabListener
	{

		private ViewPager mViewPager;

		private TaskListPagerAdapter mTaskListPageAdapter;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.HomeScreen);

			//Initialize Backend/TaskRepository
			BackendManager ma = TaskRepository.Instance.Manager;

			TaskRepository.Instance.SetActiveBackendFromPreferences (this.GetSharedPreferences("preferences.xml", FileCreationMode.WorldWriteable));
			TaskRepository.Instance.Manager.Backend.Sync ();
			//ma.RefreshTaskLists ();

			//ma.TaskLists [0].CreateTask ("abcHHHHHHHH");

			//Initialize libtasque Translator
			//Translator.Instance.AppContext = this;

			// Create the adapter that will return a fragment for each of the three primary sections
			// of the app.
			mTaskListPageAdapter = new TaskListPagerAdapter(this.SupportFragmentManager);

			// Set up the action bar.
			ActionBar actionBar = this.ActionBar;

			// Specify that the Home/Up button should not be enabled, since there is no hierarchical
			// parent.
			actionBar.SetHomeButtonEnabled(false);

			// Specify that we will be displaying tabs in the action bar.
			actionBar.NavigationMode = ActionBarNavigationMode.Tabs;

			// Set up the ViewPager, attaching the adapter and setting up a listener for when the
			// user swipes between sections.

			mViewPager = (ViewPager) this.FindViewById<ViewPager>(Resource.Id.pager);
			mViewPager.Adapter = mTaskListPageAdapter;
			mViewPager.SetOnPageChangeListener (new PageChangeListener (actionBar));


			// For each of the sections in the app, add a tab to the action bar.
			for (int i = 0; i < mTaskListPageAdapter.Count; i++) 
			{
				// Create a tab with text corresponding to the page title defined by the adapter.
				// Also specify this Activity object, which implements the TabListener interface, as the
				// listener for when this tab is selected.
				actionBar.AddTab (actionBar.NewTab ()
				                  .SetText (mTaskListPageAdapter.GetTaskListName (i))
				                  .SetTabListener (this));
			}
			Log.Debug ("MainActivity", "OnCreate ready");

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

				case Resource.Id.menuChangeTaskListName:
				break;

				case Resource.Id.menuAbout:
				break;
			}
			return true;
		}


		/*
			 * int id = item.getItemId();
    Log.d("item ID : ", "onOptionsItemSelected Item ID" + id);
    if (id == android.R.id.home) {
        rbmView.toggleMenu();

        return true;

    } else {
        return super.onOptionsItemSelected(item);
    }*/


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

