using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	/// <summary>
	/// Check if the device is connected to internet.
	/// </summary>
	public partial class NetworkConnectivity : MonoBehaviour 
	{
		#region Properties

		/// <summary>
		/// Gets a value indicating whether network is connected.
		/// </summary>
		/// <value><c>true</c> if network is connected; otherwise, <c>false</c>.</value>
		public bool 			IsConnected
		{
			get;
		 	protected set;
		}

		#endregion

		#region Unity Methods

		private void Start ()
		{
			NetworkConnectivitySettings _settings	= NPSettings.NetworkConnectivity;

			if (string.IsNullOrEmpty(_settings.HostName))
				_settings.HostName	= "http://www.google.com";

			// Initialise component
			Initialise(_settings);
		}

		#endregion

		#region API
		
		protected virtual void Initialise (NetworkConnectivitySettings _settings)
		{}
		
		#endregion
	}
}
