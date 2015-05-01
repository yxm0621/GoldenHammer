using UnityEngine;
using System.Collections;
using VoxelBusters.Utility.UnityGUI.MENU;
using VoxelBusters.Utility;
using VoxelBusters.NativePlugins;

namespace VoxelBusters.NativePlugins.Demo
{
	public class NetworkConnectivityDemo : NPDemoSubMenu 
	{
		#region API Calls
		
		private bool IsConnected()
		{
			return  NPBinding.NetworkConnectivity.IsConnected;
		}
		
		#endregion

		#region API Callbacks

		private void NetworkConnectivityChangedEvent (bool _isConnected)
		{
			AddNewResult("Received NetworkConnectivityChangedEvent");
			AppendResult("IsConnected = " + _isConnected);
		}

		#endregion

		#region Enable/Disable Callbacks
		
		private void OnEnable ()
		{
			// Register to event
			NetworkConnectivity.NetworkConnectivityChangedEvent	+= NetworkConnectivityChangedEvent;
			
			// Info text
			AddNewResult("Callbacks" +
			             "\nNetworkConnectivityChangedEvent: Triggered when connectivity state changes");
		}
		
		private void OnDisable ()
		{			
			// Deregister to event
			NetworkConnectivity.NetworkConnectivityChangedEvent	-= NetworkConnectivityChangedEvent;
		}
		
		#endregion


		#region UI

		protected override void OnGUIWindow()
		{
			base.OnGUIWindow();
			
			GUILayout.BeginVertical();
			{
				if(GUILayout.Button("Is Network Reachable?"))
				{
					bool _isConnected = IsConnected();

					if(_isConnected)
					{
						AddNewResult("Network is Reachable.");
					}
					else
					{
						AddNewResult("Network is Unreachable.");
					}
				}				

			}
			GUILayout.EndVertical();
	
			GUILayout.FlexibleSpace();

			DrawResults();

			DrawPopButton();
		}

		#endregion

		
		
	}
}