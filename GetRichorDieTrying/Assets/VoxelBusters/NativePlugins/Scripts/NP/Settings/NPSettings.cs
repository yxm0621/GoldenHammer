using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;
using VoxelBusters.Product;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VoxelBusters.NativePlugins
{
	using Internal;

#if UNITY_EDITOR
	[InitializeOnLoad]
#endif
	public class NPSettings : ProductDashboard <NPSettings> 
	{
		#region Properties

		public override string 						ProductName 
		{
			get 
			{
				return kProductName;
			}
		}
		
		public override string 						ProductVersion
		{
			get 
			{
				return kProductVersion;
			}
		}

		[SerializeField]
		private Texture2D							m_logoTexture;
		public override Texture2D 					LogoTexture 
		{
			get 
			{
				return m_logoTexture;
			}
		}

		[SerializeField]
		private ApplicationSettings					m_applicationSettings			= new ApplicationSettings();
		/// <summary>
		/// Gets the application settings.
		/// </summary>
		/// <value>The application settings.</value>
		public static ApplicationSettings			Application
		{
			get 
			{ 
				return Instance.m_applicationSettings; 
			}
		}

		[SerializeField]
		private BillingSettings						m_billingSettings				= new BillingSettings();
		/// <summary>
		/// Gets the billing settings.
		/// </summary>
		/// <value>The billing settings.</value>
		public static BillingSettings				Billing
		{
			get 
			{ 
				return Instance.m_billingSettings; 
			}
		}
		
		[SerializeField]
		private MediaLibrarySettings				m_mediaLibrarySettings			= new MediaLibrarySettings();
		/// <summary>
		/// Gets the media library settings.
		/// </summary>
		/// <value>The media library settings.</value>
		public static MediaLibrarySettings			MediaLibrary
		{
			get 
			{ 
				return Instance.m_mediaLibrarySettings; 
			}
		}

		[SerializeField]
		private NetworkConnectivitySettings			m_networkConnectivitySettings	= new NetworkConnectivitySettings();
		/// <summary>
		/// Gets the network connectivity settings.
		/// </summary>
		/// <value>The network connectivity settings.</value>
		public static NetworkConnectivitySettings	NetworkConnectivity
		{
			get 
			{ 
				return Instance.m_networkConnectivitySettings; 
			}
		}

		[SerializeField]
		private NotificationServiceSettings			m_notificationSettings			= new NotificationServiceSettings();
		/// <summary>
		/// Gets the notification settings.
		/// </summary>
		/// <value>The notification settings.</value>
		public static NotificationServiceSettings	Notification
		{
			get 
			{ 
				return Instance.m_notificationSettings; 
			}
		}

		[SerializeField]
		private TwitterSettings						m_twitterSettings				= new TwitterSettings();
		/// <summary>
		/// Gets the twitter settings.
		/// </summary>
		/// <value>The twitter settings.</value>
		public static TwitterSettings				Twitter
		{
			get 
			{ 
				return Instance.m_twitterSettings; 
			}
		}

		[SerializeField]
		private UtilitySettings						m_utilitySettings				= new UtilitySettings();
		/// <summary>
		/// Gets the utility settings.
		/// </summary>
		/// <value>The utility settings.</value>
		public static UtilitySettings				Utility
		{
			get 
			{ 
				return Instance.m_utilitySettings; 
			}
		}

		#endregion

		#region Constants

		private const string kProductName		= "Native Plugins";
		private const string kProductVersion	= "1.0";

		#endregion

		#region Constructor

		static NPSettings ()
		{
			EditorInvoke.Invoke(()=>{
#pragma warning disable
				NPSettings _instance	= NPSettings.Instance;
#pragma warning restore 
			}, 1f);
		}

		#endregion

		#region Unity Methods

		protected override void Reset ()
		{
			base.Reset();

#if UNITY_EDITOR
			m_logoTexture	= AssetDatabase.LoadAssetAtPath(Constants.kLogoPath, typeof(Texture2D)) as Texture2D;
#endif
		}

		protected override void OnEnable ()
		{
			base.OnEnable ();

#if UNITY_EDITOR
			m_logoTexture	= AssetDatabase.LoadAssetAtPath(Constants.kLogoPath, typeof(Texture2D)) as Texture2D;
#endif

			// Set debug mode
			if (m_applicationSettings.IsDebugMode)
				DebugPRO.Console.RemoveIgnoreTag(Constants.kDebugTag);
			else
				DebugPRO.Console.AddIgnoreTag(Constants.kDebugTag);
		}

		#endregion
	}
}