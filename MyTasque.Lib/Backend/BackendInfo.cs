using System;

namespace MyTasque.Lib.Backend
{
	/// <summary>
	/// Backend info.
	/// </summary>
	public class BackendInfo
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MyTasque.Lib.Backend.BackendInfo"/> class.
		/// </summary>
		/// <param name="backendName">Backend name.</param>
		/// <param name="createFunc">Create func.</param>
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

		/// <summary>
		/// Gets the name of the backend.
		/// </summary>
		/// <value>The name of the backend.</value>
		public string BackendName { get; private set; }

		/// <summary>
		/// Gets the create instance.
		/// </summary>
		/// <value>The create instance.</value>
		public Func<IBackend> CreateInstance { get; private set; }
	}
}

