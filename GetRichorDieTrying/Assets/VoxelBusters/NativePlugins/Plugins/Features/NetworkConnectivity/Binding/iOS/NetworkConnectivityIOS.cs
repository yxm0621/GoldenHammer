using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

#if UNITY_IOS
namespace VoxelBusters.NativePlugins
{
	public class NetworkConnectivityIOS : NetworkConnectivity
	{
		#region Native Methods

		[DllImport("__Internal")]
		private static extern void initNetworkConnectivity (string _hostName);

		#endregion

		#region API

		protected override void Initialise (NetworkConnectivitySettings _settings)
		{
			initNetworkConnectivity(_settings.HostName);
		}

		#endregion
	}
}
#endif
