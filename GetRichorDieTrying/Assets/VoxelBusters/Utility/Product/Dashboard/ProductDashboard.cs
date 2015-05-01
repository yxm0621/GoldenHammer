using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.Utility;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VoxelBusters.Product
{
	using Internal;

	public class ProductDashboard <T> : AdvancedScriptableObject <T>, IProductDashboard where T : ScriptableObject
	{
		#region Properties

		public virtual string 				ProductName
		{	
			get
			{
				return string.Empty;
			}
		}

		public virtual string				ProductVersion
		{
			get
			{
				return string.Empty;
			}
		}

		public virtual Texture2D			LogoTexture
		{
			get
			{
				return null;
			}
		}

		private ProductUpdateInfo			m_productUpdateInfo;
		private bool						m_isAutomaticCheck;
		private GETRequest					m_updateGETRequest;

		#endregion

		#region Constants

		private const string 				kServerBaseAddress				= "https://unity3dplugindashboard.herokuapp.com";
		private const string				kProductDetailsPathFormat		= "products/{0}/details";

		// Related to update dialog
		private const string 				kCheckUpdatesFailedMessage		= "{0} update server could not be connected. Please try again after sometime.";
		private const string 				kAlreadyUptoDateMessage			= "You are using latest version of {0}. Please check back for updates at a later time.";
		private const string 				kNewVersionAvailableMessage		= "Newer version of {0} is available for download.";
		private const string 				kButtonSkipVersion				= "Skip";
		private const string 				kButtonDownloadFromAssetStore	= "Go To Asset Store";
		private const string 				kButtonDownloadFromOurServer	= "Download";

		// Related to update request
		private const int					kCheckForUpdatesAfterMinutes	= 360;	
		private const string 				kSkippedVersionPrefix			= "version-skipped";

		#endregion

		#region Constructor

		protected ProductDashboard ()
		{
#if UNITY_EDITOR
			float _repeatRate		= kCheckForUpdatesAfterMinutes * 60f;
			float _firstCheckAfter	= _repeatRate;

			// Check at unity application launch
			if (EditorApplication.timeSinceStartup < 100f)
			{
				_firstCheckAfter	= 1f;
			}

			EditorInvoke.InvokeRepeating(AutoCheckForUpdates, _firstCheckAfter, _repeatRate);
#endif
		}

		#endregion

		#region Product Updates Method

		private string GetGlobalIdentificationForThisProduct ()
		{
			return ProductName.Replace(" ", "-").ToLower();
		}

#if UNITY_EDITOR

		public void AutoCheckForUpdates ()
		{
			// If already a request is going on, then no need to auto check for updates
			if (m_updateGETRequest != null)
			{
				return;
			}

			// Check for updates
			CheckForUpdates(true);
		}
		
		public void CheckForUpdates (bool isAutoCheck = false) 
		{
			// Mark if request is auto or manual check
			m_isAutomaticCheck			= isAutoCheck;

			string _productName			= GetGlobalIdentificationForThisProduct();
			string _productDetailsPath	= string.Format(kProductDetailsPathFormat, _productName);
			URL _URL					= URL.URLWithString(kServerBaseAddress, _productDetailsPath);

			// Start asynchronous request
			GETRequest _request			= GETRequest.CreateAsyncRequest(_URL, null);
			_request.OnSuccess			= RequestForUpdatesSuccess;
			_request.OnFailure			= RequestForUpdatesFailed;

			// Start request
			_request.StartRequest();

			// Cache request
			m_updateGETRequest			= _request;
		}

		private void RequestForUpdatesSuccess (WebResponse _response)
		{
			ProductUpdateInfo _updateInfo;

			if (_response.Status == 200)
			{
				_updateInfo = new ProductUpdateInfo(ProductVersion, _response.Data);
			}
			else
			{
				_updateInfo	= new ProductUpdateInfo(false);
			}

			// Process update info data
			OnReceivingUpdateInfo(_updateInfo);

			// Reset
			ResetFieldsRelatedToUpdateCheck();
		}

		private void RequestForUpdatesFailed (WebResponse _response)
		{
			string _message	= string.Format(kCheckUpdatesFailedMessage, ProductName);

			// Show dialog
			if (!m_isAutomaticCheck)
			{
				ShowUpdatePrompt(ProductName, _message);
			}

			// Reset
			ResetFieldsRelatedToUpdateCheck();
		}

		private void OnReceivingUpdateInfo (ProductUpdateInfo _updateInfo)
		{
			// New update is not available
			if (!_updateInfo.NewUpdateAvailable)
			{
				if (!m_isAutomaticCheck)
				{
					string _uptoDateMessage	= string.Format(kAlreadyUptoDateMessage, ProductName);

					// Show update prompt dialog
					ShowUpdatePrompt(ProductName, _uptoDateMessage);
				}

				return;
			}
			
			// Cache update info
			m_productUpdateInfo				= _updateInfo;

			// User has already skipped download for this version
			string _versionNO				= m_productUpdateInfo.VersionNumber;

			if (m_isAutomaticCheck && EditorPrefs.GetBool(GetKeyForSkippedVersion(_versionNO), false))
			{
				return;
			}

			// New update is available
			string _updateAvailableMessage	= string.Format(kNewVersionAvailableMessage, ProductName);
			string _releaseNote				= "Release Note:\n\n" + _updateInfo.ReleaseNote;
			List<string> _buttonNames		= new List<string>();

			// Check if download from asset store is allowed
			if (!string.IsNullOrEmpty(_updateInfo.AssetStoreLink))
			{
				_buttonNames.Add(kButtonDownloadFromAssetStore);
			}

			// Check if download from our server is allowed
			if (!string.IsNullOrEmpty(_updateInfo.DownloadLink))
			{
				_buttonNames.Add(kButtonDownloadFromOurServer);
			}
			
			// It will have skip button
			_buttonNames.Add(kButtonSkipVersion);

			// Show update prompt dialog
			ShowUpdatePrompt(ProductName, _updateAvailableMessage, _releaseNote, _buttonNames.ToArray(), OnUpdatePromptDismissed);
		}

		private void OnUpdatePromptDismissed (string _buttonName)
		{
			// User pressed download from our server
			if (kButtonDownloadFromOurServer.Equals(_buttonName))
			{
				OpenLink(m_productUpdateInfo.DownloadLink);
				return;
			}

			// User pressed download from asset store
			if (kButtonDownloadFromAssetStore.Equals(_buttonName))
			{
				OpenLink(m_productUpdateInfo.AssetStoreLink);
				return;
			}

			// User presses skip this version
			if (kButtonSkipVersion.Equals(_buttonName))
			{
				EditorPrefs.SetBool(GetKeyForSkippedVersion(m_productUpdateInfo.VersionNumber), true);
				return;
			}
		}

		private string GetKeyForSkippedVersion (string _version)
		{
			string _key	= string.Format("{0}-{1}-{2}", GetGlobalIdentificationForThisProduct(), kSkippedVersionPrefix, _version);
			return _key;
		}

		private void OpenLink (string _link)
		{
			if (!string.IsNullOrEmpty(_link))
			{					
				Application.OpenURL(_link);
			}
		}

		private void ResetFieldsRelatedToUpdateCheck ()
		{
			m_isAutomaticCheck	= false;
			m_updateGETRequest	= null;
		}

		private void ShowUpdatePrompt (string _title, string _message, string _description = null, string[] _buttons = null, System.Action<string> _callback = null)
		{
			UpdatePromptWindow _newWindow	= EditorWindow.CreateInstance<UpdatePromptWindow>();

			// Set properties
			_newWindow.title				= _title;
			_newWindow.Message				= _message;
			_newWindow.Description			= _description;
			_newWindow.Buttons				= _buttons;
			_newWindow.LogoTexture			= LogoTexture;
			_newWindow.CallbackOnDismiss	= _callback;

			// Show window
			_newWindow.ShowUtility();
		}

#endif
		#endregion
	}
}
