using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;

namespace VoxelBusters.Product.Internal
{
	public struct ProductUpdateInfo
	{
		#region Constants
		
		private const string 	kVersionNumberKey			= "version_number";
		private const string 	kDownloadLinkKey			= "download_link";
		private const string	kAssetStoreLink				= "asset_store_link";
		private const string	kReleaseNoteKey				= "release_notes";
		
		#endregion
		
		#region Properties
		
		public bool				NewUpdateAvailable
		{
			get;
			private set;
		}
		
		public string			VersionNumber
		{
			get;
			private set;
		}
		
		public string			DownloadLink
		{
			get;
			private set;
		}
		
		public string			AssetStoreLink
		{
			get;
			private set;
		}
		
		public string			ReleaseNote
		{
			get;
			private set;
		}
		
		#endregion
		
		#region Constructor
		
		public ProductUpdateInfo (bool _newUpdateAvailable)
		{
			NewUpdateAvailable			= false;
			VersionNumber				= null;
			DownloadLink				= null;
			AssetStoreLink				= null;
			ReleaseNote					= null;
		}
		
		public ProductUpdateInfo (string _currentVersion, IDictionary _dataDict)
		{
			string _availableVersion	= _dataDict.GetIfAvailable<string>(kVersionNumberKey);
			string _downloadLink		= _dataDict.GetIfAvailable<string>(kDownloadLinkKey);
			string _assetStoreLink		= _dataDict.GetIfAvailable<string>(kAssetStoreLink);
			string _releaseNote			= _dataDict.GetIfAvailable<string>(kReleaseNoteKey);
			
			// Update class properties
			NewUpdateAvailable			= (_availableVersion.CompareTo(_currentVersion) > 0);
			VersionNumber				= _availableVersion;
			DownloadLink				= _downloadLink;
			AssetStoreLink				= _assetStoreLink;
			ReleaseNote					= _releaseNote;
		}
		
		#endregion
	}
}