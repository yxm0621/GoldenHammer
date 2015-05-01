using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins.Internal
{
	public class Constants : MonoBehaviour
	{
		#region Errors

		public const string kDebugTag			= "Native Plugins";
		public const string kErrorMessage		= "Not supported in Editor";
		public const string kiOSFeature			= "iOS feature";

		// Assets path
		public const string kEditorAssetsPath	= "Assets/VoxelBusters/NativePlugins/EditorResources";
		public const string kLogoPath			= kEditorAssetsPath + "/Logo/NativePlugins.png";

		#endregion
	}
}