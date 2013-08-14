using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MyTasque.Lib.Backend
{
	public class BackendManager
	{
		public BackendManager (IList<BackendInfo> backendInfos)
		{
			if (backendInfos == null)
				throw new ArgumentNullException ("backendInfos");
			if (backendInfos.Count<=0)
				throw new ArgumentException ("backendInfos must at least have one element");

			AvailableBackends = new ReadOnlyCollection<BackendInfo> (backendInfos);
		}

		public ReadOnlyCollection<BackendInfo> AvailableBackends { get; private set; }

		private BackendInfo currentBackendInfo;

		public IBackend Backend { get; private set; }

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

