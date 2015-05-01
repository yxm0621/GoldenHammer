using UnityEngine;
using System.Collections;
using System.Reflection;
using VoxelBusters.DesignPatterns;
using VoxelBusters.NativePlugins;
using VoxelBusters.NativePlugins.Internal;

[RequireComponent (typeof (PlatformBindingHelper))]
public class NPBinding : SingletonPattern <NPBinding>
{
	#region Properties

	/// <summary>
	/// Provides unique interface to access Address Book for all platforms.
	/// </summary>
	/// <value>The address book.</value>
	public static AddressBook 			AddressBook 	
	{ 
		get;  
		private set; 
	}
	
	public static Billing 				Billing 			
	{ 
		get;  
		private set; 
	}

	public static Utility 				Utility 		
	{ 
		get;  
		private set; 
	}

	public static MediaLibrary 			MediaLibrary 	
	{ 
		get;  
		private set; 
	}

	public static NetworkConnectivity 	NetworkConnectivity 	
	{ 
		get;  
		private set; 
	}
	
	public static NotificationService 	NotificationService 	
	{ 
		get;  
		private set; 
	}
	
	public static Sharing 				Sharing 		
	{ 
		get;  
		private set; 
	}

	public static Twitter 				Twitter 		
	{ 
		get;  
		private set; 
	}

	public static UI					UI 		
	{ 
		get;  
		private set;
	}

	public static WebViewNative 		WebView 			
	{ 
		get;  
		private set;
	}
	
	#endregion
	
	#region Overriden Methods
	
	protected override void Awake ()
	{
		base.Awake ();

		// Not interested in non singleton instance
		if (instance != this)
			return;

		// Create instances 
		AddressBook				= AddComponentBasedOnLicense<AddressBook>();
		Billing					= AddComponentBasedOnLicense<Billing>();
		Utility					= AddComponentBasedOnLicense<Utility>();
		MediaLibrary			= AddComponentBasedOnLicense<MediaLibrary>();
		NetworkConnectivity		= AddComponentBasedOnLicense<NetworkConnectivity>();
		NotificationService		= AddComponentBasedOnLicense<NotificationService>();
		Sharing					= AddComponentBasedOnLicense<Sharing>();
		Twitter					= AddComponentBasedOnLicense<Twitter>();
		UI						= AddComponentBasedOnLicense<UI>();
		WebView					= AddComponentBasedOnLicense<WebViewNative>();
	}
	
	#endregion
	
	#region Create Component Methods
	
	private T AddComponentBasedOnLicense <T> () where T : MonoBehaviour
	{
		System.Type _basicVersionType	= typeof(T);
		string _basicVersionClassName	= _basicVersionType.ToString();
		
		// We are using pro license copy, so try to invoke pro license platform specific classes
		if (Application.HasProLicense())
		{
			System.Type _proVersionClassType	= GetPlatformSpecificClassType(_basicVersionClassName, true);
			
			if (_proVersionClassType != null)
			{
				return CachedGameObject.AddComponent(_proVersionClassType) as T;
			}
		}
		
		// Check if we have free version specific class
		System.Type _basicVersionClassType		= GetPlatformSpecificClassType(_basicVersionClassName, false);
		
		if (_basicVersionClassType != null)
		{
			return CachedGameObject.AddComponent(_basicVersionClassType) as T;
		}
		
		return CachedGameObject.AddComponent<T>();
	}
	
	private System.Type GetPlatformSpecificClassType (string _basicVersionClassName, bool _isProVersion)
	{
		_isProVersion	= true;

#pragma warning restore
		string _platformSpecificClassName		= null;
			
#if UNITY_EDITOR
		_platformSpecificClassName	= _basicVersionClassName + "Editor";	
#elif UNITY_IOS 
		_platformSpecificClassName	= _basicVersionClassName + "IOS";	
#elif UNITY_ANDROID
		_platformSpecificClassName	= _basicVersionClassName + "Android";
#endif

		// Check if we have non-null class name
		if (!string.IsNullOrEmpty(_platformSpecificClassName))
		{
			System.Type _platformSpecificClassType	= Assembly.GetExecutingAssembly().GetType(_platformSpecificClassName, false);
			return _platformSpecificClassType;
		}
		
		return null;
	}
	
	#endregion
}