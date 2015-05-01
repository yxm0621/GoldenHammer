using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Demo
{
	public class NPDemoGUIWindow : GUIModalWindow 
	{
		#region Constants
	
		protected const string kSubTitleStyle  				= "sub-title";
	
		#endregion
		
		#region Drawing

		protected override void OnGUIWindow()
		{
			//Draw by overriding this method
		}

		#endregion

		#region Helpers

		protected override void AdjustFontBasedOnScreen()
		{
			GUI.skin.box.fontSize 						= Mathf.Clamp((int)(Screen.width * 0.03f), 0, 36);
			GUI.skin.button.fontSize 					= Mathf.Clamp((int)(Screen.width * 0.03f), 0, 36);
			GUI.skin.label.fontSize 					= Mathf.Clamp((int)(Screen.width * 0.03f), 0, 36);
			GUI.skin.toggle.fontSize 					= Mathf.Clamp((int)(Screen.width * 0.03f), 0, 36);
			GUI.skin.GetStyle(kSubTitleStyle).fontSize	= Mathf.Clamp((int)(Screen.width * 0.04f), 0, 48);
		}

		#endregion
	}
}
