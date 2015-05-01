using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.Utility;
using VoxelBusters.DebugPRO;

#if UNITY_ANDROID
namespace VoxelBusters.NativePlugins
{
	using Internal;

	public class NetworkConnectivityAndroid : NetworkConnectivity 
	{
		#region Key Constants //The same keys are used  by Native code as well
		
		private const string 	kHostName				= "host-name";
		private const string 	kTimeOutPeriod			= "time-out-period";
		private const string 	kMaxRetryCount			= "max-retry-count";
		private const string	kTimeGapBetweenPolling	= "time-gap-between-polling";

		#endregion

		#region Platform Native Info
		
		class NativeInfo
		{
			// Handler class name
			public class Class
			{
				public const string NAME				= "com.voxelbusters.nativeplugins.features.reachability.NetworkReachabilityHandler";
			}
			
			// For holding method names
			public class Methods
			{
				public const string INITIALIZE			= "initialize";
			}
		}
		
		#endregion
		
		#region  Required Variables
		
		private AndroidJavaObject 	m_plugin;
		private AndroidJavaObject  	Plugin
		{
			get 
			{ 
				if(m_plugin == null)
				{
					Console.LogError(Constants.kDebugTag, "[NetworkConnectivity] Plugin class not intialized!");
				}
				return m_plugin; 
			}
			
			set
			{
				m_plugin = value;
			}
		}
		
		#endregion
		
		#region Constructors
		
		NetworkConnectivityAndroid()
		{
			Plugin = AndroidPluginUtility.GetSingletonInstance(NativeInfo.Class.NAME);
		}
		
		#endregion

		#region API

		protected override void Initialise (NetworkConnectivitySettings _settings)
		{
			NetworkConnectivitySettings.AndroidSettings _androidSettings = _settings.Android;

			Dictionary<string, string> _configInfo = new Dictionary<string, string>();
	
			_configInfo.Add(kHostName				, _settings.HostName);
			_configInfo.Add(kTimeOutPeriod			, _androidSettings.TimeOutPeriod.ToString());
			_configInfo.Add(kMaxRetryCount			, _androidSettings.MaxRetryCount.ToString());
			_configInfo.Add(kTimeGapBetweenPolling	, _androidSettings.TimeGapBetweenPolling.ToString());
			
			Plugin.Call(NativeInfo.Methods.INITIALIZE,_configInfo.ToJSON());
		}	

		#endregion
	}
}
#endif