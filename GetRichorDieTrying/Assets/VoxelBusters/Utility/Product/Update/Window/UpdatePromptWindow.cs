using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;

#if UNITY_EDITOR
using UnityEditor;

namespace VoxelBusters.Product.Internal
{
	public class UpdatePromptWindow : PromptWindow
	{
		#region Propeties

		public Texture2D	LogoTexture
		{
			get;
			set;
		}

		private bool		m_initialised;

		#endregion

		#region Unity Methods

		protected override void OnGUI () 
		{
			if (!m_initialised && LogoTexture != null)
			{
				// Set scrollview size
				Vector2 _newScrollSize 	= new Vector2(WindowSize.x - LogoTexture.width - 10f, WindowSize.y);
				ScrollSize				= _newScrollSize;

				// Mark as initialised
				m_initialised			= true;
			}

			GUILayout.BeginHorizontal();
			{
				GUILayout.Label(LogoTexture);

				GUILayout.BeginVertical();
				{
					base.OnGUI();
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndHorizontal();
		}

		#endregion
	}
}
#endif