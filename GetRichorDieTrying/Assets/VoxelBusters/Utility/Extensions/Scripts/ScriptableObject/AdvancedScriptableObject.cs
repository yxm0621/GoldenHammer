using UnityEngine;
using System.Collections;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VoxelBusters.Utility
{
	public class AdvancedScriptableObject <T> : ScriptableObject where T : ScriptableObject
	{
		#region Properties

		private static T					instance							= null;
		public static T 					Instance
		{
			get 
			{ 
				if (instance == null)
					instance	= GetAsset(typeof(T).Name);		

				return instance;
			}
		}

		#endregion

		#region Constants

		// Related to asset
		private const string 				kAssetsFolderName					= "Assets";
		private const string 				kResourcesFolderName				= "Resources";
		private const string 				kRelativePathToResources			= "Assets/Resources";
		private const string 				kRelativePathToAssetsGroup			= "Assets/Resources/VoxelBusters";
		private const string 				kResourcesPathToAssetsGroup			= "VoxelBusters";

		#endregion

		#region Methods

		protected virtual void Reset ()
		{}

		protected virtual void OnEnable ()
		{
			if (instance == null)
				instance	= this as T;
		}

		protected virtual void OnDisable ()
		{}

		protected virtual void OnDestroy ()
		{}

		public void Save ()
		{
#if UNITY_EDITOR
			if (EditorApplication.isPlaying || EditorApplication.isPaused)
				return;

			EditorUtility.SetDirty(this);

			// Save
			AssetDatabase.SaveAssets();

			// Refresh
			AssetDatabase.Refresh();
#endif
		}

		#endregion

		#region Static Methods

		public static T GetAsset (string _assetName)
		{
#if !UNITY_EDITOR
			// Find assets in "Resources/Group" folder
			string _pathToAssetInResourcesFolder	= string.Format("{0}/{1}", kResourcesPathToAssetsGroup, _assetName);

			return Resources.Load<T>(_pathToAssetInResourcesFolder);
#else
//			string[] _matchingGUIDS		= AssetDatabase.FindAssets(_assetName, new string[1]{ kRelativePathToAssetsGroup });
//			
//			if (_matchingGUIDS != null)
//				Debug.Log("AdvancedScriptableObject GetAsset " + _matchingGUIDS.ToJSON());
//
			string _assetSavedAtPath	= string.Format("{0}/{1}.asset", kRelativePathToAssetsGroup, _assetName);
			T _scriptableObject 		= AssetDatabase.LoadAssetAtPath(_assetSavedAtPath, typeof(T)) as T;

			if (_scriptableObject == null)
			{
				string _absolutePathToResources		= AssetsUtility.AssetPathToAbsolutePath(kRelativePathToResources);
				string _absolutePathToAssetsGroup	= AssetsUtility.AssetPathToAbsolutePath(kRelativePathToAssetsGroup);

				// Need to create folder "Assets/Resources", if it doesnt exist
				if (!Directory.Exists(_absolutePathToResources))
				{
					AssetDatabase.CreateFolder(kAssetsFolderName, kResourcesFolderName);
				}

				// Need to create folder "Assets/Resources/Group", if it doesnt exist
				if (!Directory.Exists(_absolutePathToAssetsGroup))
				{
					AssetDatabase.CreateFolder(kRelativePathToResources, kResourcesPathToAssetsGroup);
				}
				
				// Refresh
				AssetDatabase.Refresh();
				
				// Create asset
				T _newAsset					= ScriptableObject.CreateInstance<T>();

				AssetDatabase.CreateAsset(_newAsset, AssetDatabase.GenerateUniqueAssetPath(_assetSavedAtPath));

				// Save
				(_newAsset as AdvancedScriptableObject<T>).Save();

				// Assign reference of newly created asset
				_scriptableObject	= _newAsset;
			}
			
			return _scriptableObject;
#endif
		}

		#endregion
	}
}