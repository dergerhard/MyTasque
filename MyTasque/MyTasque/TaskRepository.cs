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
		private ISharedPreferences preferences;

		/// <summary>
		/// The backend infos.
		/// </summary>
		private List<BackendInfo> backendInfos;

		/// <summary>
		/// Initializes a new instance of the <see cref="Tasque.TaskRepository"/> class.
		/// </summary>
		private TaskRepository()
		{
			//Func<DummyBackend> createDummyBackend = () => new DummyBackend ();
			backendInfos = new List<BackendInfo> ();

			//Attention: Backend names must match the names in string-array-resourse "backendKeys"
			BackendInfo dummyInfo = new BackendInfo ("Dummy Backend",  () => new DummyBackend ());
			backendInfos.Add (dummyInfo);
			// OTHER BACKENDS HERE

			//BackendInfo rtmInfo = new BackendInfo ("RTM Backend", "", () => new RtmBackend ());
			//backendInfos.Add (rtmInfo);

			Manager = new BackendManager (backendInfos);

			//set the first as default
			Manager.CurrentBackend = Manager.AvailableBackends [0];
		}

		/// <summary>
		/// Sets the active backend from preferences.The preferences only have to be supplied the first time.
		/// </summary>
		/// <param name="pref">Preference.</param>
		public void SetActiveBackendFromPreferences(ISharedPreferences pref = null)
		{
			if (pref != null)
				this.preferences = pref;

			string selectedBackend = preferences.GetString ("backendKeys", "");
			foreach (BackendInfo info in Manager.AvailableBackends)
			{
				if (info.BackendName.Equals(selectedBackend))
				{
					Manager.CurrentBackend = info;
					break;
				}
			}
			BackendContext ctx = new BackendContext ();
			//ctx.Username = "";

			Manager.Backend.Initialize (ctx);
			Manager.Backend.Sync ();
		}


		/*manager.CurrentBackendInitialized += delegate { Console.WriteLine ("Initialized"); };
			manager.InitializeCurrentBackend ();
			manager.RefreshTaskLists ();

			foreach (var item in manager.TaskLists) 
			{
				Console.WriteLine (item.Name);
				foreach (var task in item) 
				{
					Console.WriteLine ("\t" + task.Text);
					foreach (var note in task.Notes) 
					{
						Console.WriteLine ("\t\t" + note.Title);
						Console.WriteLine ("\t\t" + note.Text);
					}
				}
			}*/

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

