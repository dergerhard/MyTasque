using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MyTasque.Lib.Backend
{
	/// <summary>
	/// Backend manager.
	/// </summary>
	public class BackendManager
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MyTasque.Lib.Backend.BackendManager"/> class.
		/// </summary>
		/// <param name="backendInfos">Backend infos.</param>
		public BackendManager (IList<BackendInfo> backendInfos)
		{
			if (backendInfos == null)
				throw new ArgumentNullException ("backendInfos");
			if (backendInfos.Count<=0)
				throw new ArgumentException ("backendInfos must at least have one element");

			AvailableBackends = new ReadOnlyCollection<BackendInfo> (backendInfos);
			this.Backend = null;
		}

		/// <summary>
		/// Gets the available backends.
		/// </summary>
		/// <value>The available backends.</value>
		public ReadOnlyCollection<BackendInfo> AvailableBackends { get; private set; }

		/// <summary>
		/// The current backend info.
		/// </summary>
		private BackendInfo currentBackendInfo;

		/// <summary>
		/// Gets the backend.
		/// </summary>
		/// <value>The backend.</value>
		public IBackend Backend { get; private set; }

		/// <summary>
		/// Gets or sets the current backend.
		/// </summary>
		/// <value>The current backend.</value>
		public BackendInfo CurrentBackend {
			get { return currentBackendInfo; }
			set {
				if (!AvailableBackends.Contains (value))
					throw new ArgumentException ("The provided BackendInfo doesn't exist.", "currentBackend");
				if (value == currentBackendInfo) return;

				if (currentBackendInfo != null)
					Backend.Dispose ();

				currentBackendInfo = value;
				Backend = value != null ? currentBackendInfo.CreateInstance () : null;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the current backend is initialized.
		/// </summary>
		/// <value>
		/// <c>true</c> if backend is initialized; otherwise, <c>false</c>.
		/// </value>
		public bool IsCurrentBackendInitialized {
			get { return Backend != null && Backend.IsInitialized;}
		}





	}
}

