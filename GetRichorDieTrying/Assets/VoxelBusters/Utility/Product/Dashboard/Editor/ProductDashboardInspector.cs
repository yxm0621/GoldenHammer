using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Reflection;
using VoxelBusters.Utility;

namespace VoxelBusters.Product
{
	using Internal;

	public class ProductDashboardInspector : Editor
	{
		#region Properties

		// Target
		private IProductDashboard	m_dashBoard;

		// Related to GUI 
		private GUIStyle			m_guiStyle;

		#endregion

		#region Methods

		protected virtual void OnEnable ()
		{
			m_dashBoard				= target as IProductDashboard;
		}

		protected virtual void OnDisable ()
		{}

		public override void OnInspectorGUI ()
		{
			// Draw layout for this class
			DrawLayout();
		}

		public void DrawLayout ()
		{
			if (m_dashBoard.LogoTexture == null)
				return;
		
			// GUI style
			m_guiStyle				= new GUIStyle("label");
			m_guiStyle.richText		= true;

			GUILayout.BeginHorizontal();
			{
				GUILayout.BeginVertical();
				{
					GUILayout.Space(10f);

					// Logo
					GUILayout.Label(m_dashBoard.LogoTexture);
				}
				GUILayout.EndVertical();

				// Product details and copyrights
				GUILayout.BeginVertical();
				{
					string _productName		= m_dashBoard.ProductName;
					string _productVersion	= "Version " + m_dashBoard.ProductVersion;

					// Product name
					m_guiStyle.fontSize	= 32;
					GUILayout.Label(_productName, m_guiStyle, GUILayout.Height(40f));

					// Product version info
					m_guiStyle.fontSize	= 10;
					GUILayout.Label(_productVersion, m_guiStyle);

					// Copyrights info
					m_guiStyle.fontSize	= 10;
					GUILayout.Label("<i>" + Constants.kCopyrights + "</i>", m_guiStyle);
				}
				GUILayout.EndVertical();

				// To keep above GUI elements left aligned
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();

			// Extra spacing
			GUILayout.Space(10f);
		}

		#endregion
	}
}