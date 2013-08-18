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

using MyTasque.Backends;
using MyTasque.Lib;
using MyTasque.Lib.Backend;
using Android.Preferences;

namespace MyTasque
{
	/// <summary>
	/// Task repository. Singleton Class that sets up the BackendManager and provides access to it. 
	/// </summary>
	public sealed class TaskRepository
	{
		/// <summary>
		/// The _instance. Used for correct singleton implementation.
		/// </summary>
		private static readonly Lazy<TaskRepository> _instance = new Lazy<TaskRepository> (() => new TaskRepository());

		/// <summary>
		/// Gets or sets the manager.
		/// </summary>
		/// <value>The manager.</value>
		public BackendManager Manager { get; private set; }

		/// <summary>
		/// Gets or sets the task buffer. Used for communication between Aktivities.
		/// </summary>
		/// <value>The task buffer.</value>
		public ITask TaskBuffer { get; set; }

		public TaskListAdapter TaskListAdapterBuffer { get; set; }

		/// <summary>
		/// The preferences.
		/// </summary>
		private ISharedPreferences preferences = null;

		/// <summary>
		/// The backend infos.
		/// </summary>
		private List<BackendInfo> backendInfos;

		/// <summary>
		/// Gets or sets the MainActivity, for notifying operations.
		/// </summary>
		/// <value>The activity.</value>
		public MainActivity Activity { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Tasque.TaskRepository"/> class.
		/// </summary>
		private TaskRepository()
		{
			//Func<DummyBackend> createDummyBackend = () => new DummyBackend ();
			backendInfos = new List<BackendInfo> ();

			//Attention: Backend names must match the names in string-array-resourse "backendKeys"
			BackendInfo dummyInfo = new BackendInfo ("dummy",  () => new DummyBackend ());
			BackendInfo localInfo = new BackendInfo ("local",  () => new LocalBackend ());

			//TODO: these still are not implemented
			BackendInfo rtmInfo = new BackendInfo ("rtm",  () => new DummyBackend ());
			BackendInfo googleInfo = new BackendInfo ("google",  () => new DummyBackend ());

			backendInfos.Add (dummyInfo);
			backendInfos.Add (localInfo);
			backendInfos.Add (rtmInfo);
			backendInfos.Add (googleInfo);
			// OTHER BACKENDS HERE

			Manager = new BackendManager (backendInfos);

			//set the first as default
			Manager.CurrentBackend = Manager.AvailableBackends [0];
		}

		/// <summary>
		/// Sets the active backend from preferences.The preferences only have to be supplied the first time.
		/// </summary>
		/// <param name="pref">Preference.</param>
		public void SetActiveBackendFromPreferencesAndInitializeAndSync()
		{
			if (preferences == null) 
				this.preferences = this.preferences = PreferenceManager.GetDefaultSharedPreferences (this.Activity);

			if (this.Activity == null)
				throw new InvalidOperationException ("TaskRepository must be initialized with the MainActivity");

			this.preferences = PreferenceManager.GetDefaultSharedPreferences (this.Activity);

			if (this.Manager.Backend != null)
				this.Manager.Backend.Dispose ();

			string selectedBackend = preferences.GetString ("backend", Activity.GetString(Resource.String.backendDefault));
			bool foundBackend = false;
			foreach (BackendInfo info in Manager.AvailableBackends) 
			{
				if (info.BackendName.Equals (selectedBackend)) 
				{
					Manager.CurrentBackend = info;
					foundBackend = true;
					break;
				}
			}

			if (!foundBackend)
				throw new InvalidOperationException ("The configured backend was not found");
			BackendContext ctx = new BackendContext ();

			Manager.Backend.Initialize (ctx);
			Manager.Backend.Sync ();

			this.OrderTasks ();
		}

		/// <summary>
		/// Orders the tasks according to the preferences.
		/// </summary>
		public void OrderTasks()
		{
			if (preferences.GetString ("orderBy", "name").Equals ("name"))
				foreach (var tl in Manager.Backend.AllTaskLists)
					tl.OrderTasksBy (OrderByType.Name);
			else 
				foreach (var tl in Manager.Backend.AllTaskLists)
					tl.OrderTasksBy (OrderByType.DueDate);
		}
		
		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <value>The instance.</value>
		public static TaskRepository Instance {
			get {
				return _instance.Value;
			}
		}

	}
}

