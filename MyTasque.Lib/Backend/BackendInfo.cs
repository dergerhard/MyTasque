using System;

namespace MyTasque.Lib.Backend
{
	/// <summary>
	/// Backend info.
	/// </summary>
	public class BackendInfo
	{
		public BackendInfo (string backendName, Func<IBackend> createFunc)
		{
			if (backendName == null)
				throw new ArgumentNullException ("backendName");
			if (string.IsNullOrWhiteSpace (backendName))
				throw new ArgumentException ("backendName must not be white space only", "backendName");
			if (createFunc == null)
				throw new ArgumentNullException ("createFunc");
			BackendName = backendName;
			CreateInstance = createFunc;
		}

		public string BackendName { get; private set; }

		public Func<IBackend> CreateInstance { get; private set; }
	}
}

