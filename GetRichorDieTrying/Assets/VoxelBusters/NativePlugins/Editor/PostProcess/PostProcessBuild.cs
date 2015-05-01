﻿using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using VoxelBusters.Utility;
using VoxelBusters.ThirdParty.XUPorter;

namespace VoxelBusters.NativePlugins
{
	public class PostProcessBuild
	{
		#region Constants

		// Fabric
		private const string 	kFabricKitJsonStringFormat				= "{{\"Fabric\":{{\"APIKey\":\"{0}\",\"Kits\":[{{\"KitInfo\":{{\"consumerKey\":\"\",\"consumerSecret\":\"\"}},\"KitName\":\"Twitter\"}}]}}}}";
		private const string 	kFabricKitRootKey 						= "Fabric";

		// Path
		private const string 	kInfoPlistFileRelativePath				= "Info.plist";
		private const string 	kInfoPlistBackupFileRelativePath		= "Info.backup.plist";
		private const string 	kPrecompiledFileRelativeDirectoryPath	= "Classes/";
		private const string 	kPrecompiledHeaderExtensionPattern		= "*.pch";
		
		// File modification
		private const string	kPrecompiledHeaderRegexPattern			= @"#ifdef __OBJC__(\n?\t?[#|//](.)*)*";
		private const string	kPrecompiledHeaderEndIfTag				= "#endif";
		private const string	kPrecompiledHeaderInsertText			= "#import \"Defines.h\"";

		// Framework decompress
		private const string	kRelativePathToZippedFrameworks			= "Assets/VoxelBusters/NativePlugins/Plugins/iOSPlatform/External/Framework";
		private const string	kRelativePathToFrameworks				= "NativePlugins/Framework";
		private const string	kRelativePathToFrameworkModFile			= "NativePlugins/NPFramework.xcodemods";
		#endregion

		#region Methods

		[PostProcessBuild(0)]
		public static void OnPostprocessBuild (BuildTarget _target, string _buildPath) 
		{			

			#if UNITY_5 || UNITY_6 || UNITY_7
			if (_target == BuildTarget.iOS)
			#else
			if (_target == BuildTarget.iPhone)
			#endif
			{
				// Decompress
				DecompressZippedFrameworks();

				// Append fabric details
				AppendFabricKitToInfoPlist(_buildPath);

				// Append code
				AppendCode(_buildPath);
			}
		}

		private static void DecompressZippedFrameworks ()
		{
			string _zippedFrameworksFolder				= AssetsUtility.AssetPathToAbsolutePath(kRelativePathToZippedFrameworks);
			string[] _zippedFiles 						= Directory.GetFiles(_zippedFrameworksFolder, "*.gz", SearchOption.AllDirectories);
			List<string> _frameworkFileReferenceList	= new List<string>();

			// Create director for unzipped frameworks
			string _projectPath							= AssetsUtility.GetProjectPath();
			string _unzippedFrameworksFolder			= Path.Combine(_projectPath, kRelativePathToFrameworks);
			string _frameworkFolderRelativeToModFile	= new DirectoryInfo(_unzippedFrameworksFolder).Name;

			if (!Directory.Exists(_unzippedFrameworksFolder))
				Directory.CreateDirectory(_unzippedFrameworksFolder);

			// Iterate through each zip files
			foreach (string _zippedFilePath in _zippedFiles) 
			{
				string _zippedFileName					= Path.GetFileNameWithoutExtension(_zippedFilePath);
				string _unzippedFileAbsolutePath		= Path.Combine(_unzippedFrameworksFolder, _zippedFileName);

				// Decompress files
				Zip.DecompressToDirectory(_zippedFilePath, _unzippedFileAbsolutePath);

				// Add name of the file which was unzipped
				_frameworkFileReferenceList.Add(_frameworkFolderRelativeToModFile + "/" + _zippedFileName);
			}

			// Generate xmod file for unzipped frameworks
			Dictionary<string, object> _xcodeModDict	= new Dictionary<string, object>();
			_xcodeModDict["group"]						= "NativePlugins";
			_xcodeModDict["files"]						= _frameworkFileReferenceList;

			string _xcodeModJsonString					= _xcodeModDict.ToJSON();
			File.WriteAllText(Path.Combine(_projectPath, kRelativePathToFrameworkModFile), _xcodeModJsonString);
		}

//		{
//			"Fabric": {
//				"APIKey": "{0}",
//				"Kits": [
//				    {
//					"KitInfo": {
//						"consumerKey": "",
//						"consumerSecret": ""
//					},
//					"KitName": "Twitter"
//				    }
//				    ]
//			}
//		}

		private static void AppendFabricKitToInfoPlist (string _buildPath)
		{
			TwitterSettings _twitterSettings	= NPSettings.Twitter;
			string _fabricJsonStr				= string.Format(kFabricKitJsonStringFormat, _twitterSettings.ConsumerKey);
			IDictionary _fabricJsonDictionary	= JSONUtility.FromJSON(_fabricJsonStr) as IDictionary;
			string _path2InfoPlist				= Path.Combine(_buildPath, kInfoPlistFileRelativePath);
			string _path2InfoPlistBackupFile	= Path.Combine(_buildPath, kInfoPlistBackupFileRelativePath);
			Plist _infoPlist					= Plist.LoadPlistAtPath(_path2InfoPlist);

			// Create backup
			_infoPlist.Save(_path2InfoPlistBackupFile);

			// Add fabric
			_infoPlist.AddValue(kFabricKitRootKey, _fabricJsonDictionary[kFabricKitRootKey] as IDictionary);

			// Save
			_infoPlist.Save(_path2InfoPlist);
		}

		private static void AppendCode (string _buildPath)
		{
			string _pchFileDirectory	= Path.Combine(_buildPath, kPrecompiledFileRelativeDirectoryPath);

			string[] _pchFiles = Directory.GetFiles(_pchFileDirectory, kPrecompiledHeaderExtensionPattern);

			string _pchFilePath = null;

			if(_pchFiles.Length > 0)
			{
				_pchFilePath =  _pchFiles[0];//There will be only one file per project if it exists.
			}

			if (File.Exists(_pchFilePath))
			{
				string _fileContents 	= File.ReadAllText(_pchFilePath);

				// Make sure content doesnt exist
				if (_fileContents.Contains(kPrecompiledHeaderInsertText))
					return;

				Regex _regex			= new Regex(kPrecompiledHeaderRegexPattern);
				Match _match 			= _regex.Match(_fileContents);
				int _endOfPatternIndex	= _match.Groups[0].Index + _match.Groups[0].Length;

				// We should append text within end tag
				if (_match.Value.Contains(kPrecompiledHeaderEndIfTag))
				{
					_endOfPatternIndex	-= kPrecompiledHeaderEndIfTag.Length;
				}

				string _updatedContents	= _fileContents.Insert(_endOfPatternIndex, "\t" + kPrecompiledHeaderInsertText + "\n");

				// Write updated text
				File.WriteAllText(_pchFilePath, _updatedContents);
			}
		}
		
		#endregion
	}
}