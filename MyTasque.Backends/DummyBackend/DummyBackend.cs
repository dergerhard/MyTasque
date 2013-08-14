using System;
using System.Collections.Generic;
using System.Text;
using MyTasque.Lib.Backend;
using MyTasque.Lib;


namespace MyTasque.Backends
{
	public class DummyBackend : IBackend
	{

		/// <summary>
		/// The server task lists. Represents a mocked List on the (fake)-server.
		/// </summary>
		private List<ITaskList> ServerTaskLists = new List<ITaskList>();

		/// <summary>
		/// Initialize this instance.
		/// </summary>
		/// <param name="ctx">Context for backend Configuration</param>
		public void Initialize (BackendContext ctx)
		{
			this.Context = ctx;
			ServerTaskLists.Clear ();

			ITaskList home = new DummyTaskList ("Home");
			home.Add (new DummyTask ("clean floor", DateTime.Now.AddDays (5), false));
			home.Add (new DummyTask ("clean kitchen", DateTime.Now.AddDays (3), false));
			home.Add (new DummyTask ("bring out trash", DateTime.Now.AddDays (-2), true));
			home.Add (new DummyTask ("feed cat",  DateTime.Now.AddDays (1), false));
			DummyTask dt = new DummyTask ("cook cheescake", DateTime.Now.AddHours (-2), true);
			dt.Add (new DummyNote ("buy curd"));
			home.Add (dt);

			ServerTaskLists.Add (home);

			ITaskList work = new DummyTaskList ("Work");
			ITask writeLib = new DummyTask ("write library", DateTime.Now.AddDays (-5), false);
			writeLib.Add (new DummyNote ("kick collegues ass"));
			writeLib.Add (new DummyNote ("ask again and again"));
			writeLib.Add (new DummyNote ("won't respond"));
			work.Add (writeLib);
			work.Add (new DummyTask ("prepare presentation", DateTime.Now.AddDays (10), false));

			ServerTaskLists.Add (work);

			this.IsInitialized = true;
		}

		/// <summary>
		/// The dummy task lists. Represents the local copy of the server tasks.
		/// </summary>
		private List<ITaskList> DummyTaskLists = new List<ITaskList>();

		/// <summary>
		/// The deleted dummy task lists.
		/// </summary>
		private List<ITaskList> DeletedDummyTaskLists = new List<ITaskList> ();

		/// <summary>
		/// Instantiates a new instance of the <see cref="MyTasque.Backends.DummyBackend"/> class. 
		/// </summary>
		public DummyBackend()
		{

		}

		/// <summary>
		/// Sync the local task list with the server task lists. At the moment our list is the valid one. Changes on the server are ignored.
		/// </summary>
		public void Sync ()
		{
			if (DummyTaskLists.Count > 0) {
				ServerTaskLists.Clear ();
				ServerTaskLists.AddRange (DummyTaskLists);
			} else {
				DummyTaskLists.AddRange (ServerTaskLists);
			}
			DeletedDummyTaskLists.Clear ();
		}

		/// <summary>
		/// Gets all task lists.
		/// </summary>
		/// <value>All task lists.</value>
		public IList<ITaskList> AllTaskLists {
			get {
				if (this.IsInitialized)
					return DummyTaskLists;
				else 
					throw new InvalidOperationException ("Can't call AllTaskLists because the backend is not initialized");
			}
		}

		/// <summary>
		/// Creates the task list and returns it
		/// </summary>
		/// <returns>The task list.</returns>
		/// <param name="name">Name.</param>
		public ITaskList CreateTaskList (string name)
		{
			if (!IsInitialized)
				throw new InvalidOperationException ("Can't call CreateTaskList because the backend is not initialized");

			DummyTaskList tl = new DummyTaskList(name);
			tl.Change = ChangeType.Created;
			DummyTaskLists.Add(tl);
			return tl;
		}

		/// <summary>
		/// Deletes the task list.
		/// </summary>
		/// <param name="tasklist">Tasklist.</param>
		public void DeleteTaskList (ITaskList tasklist)
		{
			if (!IsInitialized)
				throw new InvalidOperationException ("Can't call DeleteTaskList because the backend is not initialized");

			if (!DummyTaskLists.Contains (tasklist))
				throw new ArgumentException ("You can't delete this tasklist, because it does not exist", "tasklist");

			DeletedDummyTaskLists.Add (tasklist);
			DummyTaskLists.Remove (tasklist);
		}

		/// <summary>
		/// Gets or sets the context.
		/// </summary>
		/// <value>The context.</value>
		public BackendContext Context { get; set; }

		/// <summary>
		/// Gets a value indicating whether this instance is initialized.
		/// </summary>
		/// <value>true</value>
		/// <c>false</c>
		public bool IsInitialized { get; private set; }


		/// <summary>
		/// Releases all resource used by the <see cref="MyTasque.Backends.DummyBackend"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="MyTasque.Backends.DummyBackend"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="MyTasque.Backends.DummyBackend"/> in an unusable state. After
		/// calling <see cref="Dispose"/>, you must release all references to the <see cref="MyTasque.Backends.DummyBackend"/>
		/// so the garbage collector can reclaim the memory that the <see cref="MyTasque.Backends.DummyBackend"/> was occupying.</remarks>
		public void Dispose ()
		{
			// Cleanup and disconnect from backend
			// Nothing to do for Dummy Backend
		}

	}
}

