using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.IO;
using MyTasque.Lib.Backend;
using MyTasque.Lib;

namespace MyTasque.Backends
{
	enum SyncOperation { Read, Write };

	public class LocalBackend : IBackend
	{

		/// <summary>
		/// The local task lists.
		/// </summary>
		private List<ITaskList> LocalTaskLists = new List<ITaskList>();

		/// <summary>
		/// The file path to the data file.
		/// </summary>
		private string filePath;

		/// <summary>
		/// The next sync operation - either read or write
		/// </summary>
		private SyncOperation nextSyncOperation;

		/// <summary>
		/// Initialize this instance.
		/// </summary>
		/// <param name="ctx">Context for backend Configuration</param>
		public void Initialize (BackendContext ctx)
		{
			this.Context = ctx;
			LocalTaskLists.Clear ();

			nextSyncOperation = SyncOperation.Read;

			//storage dir
			string dir = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), ".MyTasque");

			if (!Directory.Exists(dir))
			    System.IO.Directory.CreateDirectory(dir);

			//set storagefile path
			filePath = Path.Combine (dir, "tasklists.xml");

			this.IsInitialized = true;
		}

		/// <summary>
		/// Reads the task lists.
		/// </summary>
		private void ReadTaskLists()
		{
			if (File.Exists (filePath)) 
			{
				LocalTaskLists.Clear ();

				XDocument doc = XDocument.Parse(File.ReadAllText (filePath));
				XElement root = doc.Root;

				foreach (XElement tl in root.Elements("TaskList")) 
				{
					LocalTaskList lTl = new LocalTaskList (tl.Attribute("Name").Value.ToString());
				
					foreach (XElement t in tl.Elements("Task")) 
					{
						LocalTask lT = new LocalTask (
							t.Attribute ("Name").Value.ToString (),
							new DateTime (long.Parse (t.Element ("DueDate").Value.ToString ())),
							t.Element ("Completed").Value.ToString ().Equals ("true") ? true : false);

						XElement notes = t.Element ("Notes");

						foreach (XElement n in notes.Elements("Note"))
						{
							lT.Add (new LocalNote (
								n.Attribute ("Text").Value.ToString (), 
								new DateTime(long.Parse (n.Attribute ("CreationDate").Value.ToString ()))));
						}
						lTl.Add (lT);
					}
					LocalTaskLists.Add (lTl);
				}
			}
		}

		/// <summary>
		/// Writes the task lists.
		/// </summary>
		private void WriteTaskLists()
		{
			File.Delete (filePath);

			XDocument doc = new XDocument (
				new XDeclaration ("1.0", "utf-8", "yes"),
				new XElement ("TaskLists"));

			foreach (ITaskList tl in LocalTaskLists) 
			{
				XElement xTl = new XElement ("TaskList");
				xTl.Add (new XAttribute ("Name", tl.Name));

				foreach (ITask t in tl) 
				{
					XElement xT = new XElement ("Task");
					xT.Add (new XAttribute ("Name", t.Name));
					xT.Add (new XElement ("DueDate", t.DueDate.Ticks.ToString()));
					xT.Add (new XElement ("Completed", t.Completed));
					XElement notes = new XElement ("Notes");
					foreach (INote n in t) 
					{
						XElement xN = new XElement ("Note");
						xN.Add (new XAttribute("Text", n.Text));
						xN.Add (new XAttribute ("CreationDate", n.CreationDate.Ticks.ToString()));
						notes.Add (xN);
					}
					xT.Add (notes);
					xTl.Add (xT);
				}
				doc.Root.Add (xTl);
			}

			using (var file = File.Open(filePath, FileMode.Create, FileAccess.Write))
			using (var strm = new StreamWriter(file))
			{
				doc.Save (strm.BaseStream);
			}
		}


		/// <summary>
		/// Instantiates a new instance of the <see cref="MyTasque.Backends.LocalBackend"/> class. 
		/// </summary>
		public LocalBackend()
		{

		}

		/// <summary>
		/// Sync the local task list. This routine reads/writes the data from/to a file.
		/// </summary>
		public void Sync ()
		{
			if (nextSyncOperation == SyncOperation.Read) {
				this.ReadTaskLists ();
				nextSyncOperation = SyncOperation.Write;
			} else {
				this.WriteTaskLists ();
			}
		}

		/// <summary>
		/// Gets all task lists.
		/// </summary>
		/// <value>All task lists.</value>
		public IList<ITaskList> AllTaskLists {
			get {
				if (this.IsInitialized)
					return LocalTaskLists;
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

			foreach (ITaskList tll in LocalTaskLists)
				if (tll.Name.Equals (name))
					throw new ArgumentException ("A task list with the same name already exists");

			LocalTaskList tl = new LocalTaskList(name);
			tl.Change = ChangeType.Created;
			LocalTaskLists.Add (tl);
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

			if (!LocalTaskLists.Contains (tasklist))
				throw new ArgumentException ("You can't delete this tasklist, because it does not exist", "tasklist");

			LocalTaskLists.Remove (tasklist);
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
		/// Releases all resource used by the <see cref="MyTasque.Backends.LocalBackend"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="MyTasque.Backends.LocalBackend"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="MyTasque.Backends.LocalBackend"/> in an unusable state. After
		/// calling <see cref="Dispose"/>, you must release all references to the <see cref="MyTasque.Backends.LocalBackend"/>
		/// so the garbage collector can reclaim the memory that the <see cref="MyTasque.Backends.LocalBackend"/> was occupying.</remarks>
		public void Dispose ()
		{
			// Cleanup and disconnect from backend
			// Nothing to do for Local Backend
		}

	}
}

