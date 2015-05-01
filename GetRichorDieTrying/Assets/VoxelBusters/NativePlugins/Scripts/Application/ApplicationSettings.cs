using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	/// <summary>
	/// Application Settings provides interface to configure properties related to this application.
	/// </summary>
	[System.Serializable]
	public class ApplicationSettings 
	{
		#region iOS Settings

		/// <summary>
		/// Application Settings specific to iOS platform.
		/// </summary>
		[System.Serializable]
		public class iOSSettings
		{
			[SerializeField, Tooltip("Identifier used to identify your app in App Store.")]
			private string			m_storeIdentifier;
			/// <summary>
			/// Gets or sets the store identifier.
			/// </summary>
			/// <value>The store identifier for this application.</value>
			public string			StoreIdentifier
			{
				get
				{
					return m_storeIdentifier;
				}
				
				set
				{
					m_storeIdentifier	= value;
				}
			}
		}

		#endregion

		#region Android Settings

		/// <summary>
		/// Application Settings specific to Android platform.
		/// </summary>
		[System.Serializable]
		public class AndroidSettings
		{
			[SerializeField, Tooltip("Identifier used to identify your app in Google Play Store.")]
			private string			m_storeIdentifier;
			/// <summary>
			/// Gets or sets the store identifier.
			/// </summary>
			/// <value>The store identifier for this application.</value>
			public string			StoreIdentifier
			{
				get
				{
					return m_storeIdentifier;
				}
				
				set
				{
					m_storeIdentifier	= value;
				}
			}
		}

		#endregion

		#region Properties

		[SerializeField, Tooltip("Determines whether Native Plugin is in debug mode.")]
		private bool				m_isDebugMode;
		/// <summary>
		/// Gets or sets a value indicating whether Native plugin is in debug mode.
		/// </summary>
		/// <value><c>true</c> if this instance is debug mode; otherwise, <c>false</c>.</value>
		public bool					IsDebugMode
		{
			get
			{
				return m_isDebugMode;
			}

			set
			{
				m_isDebugMode	= value;
			}
		}

		[SerializeField]
		private iOSSettings			m_iOS;
		/// <summary>
		/// Gets or sets the Application Settings specific to iOS platform.
		/// </summary>
		/// <value>The Application Settings specific to iOS platform.</value>
		public iOSSettings			IOS
		{
			get
			{
				return m_iOS;
			}
			
			set
			{
				m_iOS	= value;
			}
		}

		[SerializeField]
		private AndroidSettings		m_android;
		/// <summary>
		/// Gets or sets the Application Settings specific to Android platform.
		/// </summary>
		/// <value>The Application Settings specific to Android platform.</value>
		public AndroidSettings		Android
		{
			get
			{
				return m_android;
			}
			
			set
			{
				m_android	= value;
			}
		}

		/// <summary>
		/// Gets the store identifier for current build platform.
		/// </summary>
		/// <value>The store identifier for current build platform.</value>
		public string StoreIdentifier
		{
			get
			{
#if UNITY_ANDROID
				return m_android.StoreIdentifier;
#else
				return m_iOS.StoreIdentifier;
#endif
			}
		}

		#endregion 
	}
}