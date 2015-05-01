using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Globalization;
using System.Reflection;
using VoxelBusters.Utility;
using VoxelBusters.Product;

namespace VoxelBusters.NativePlugins
{
	[CustomEditor(typeof(NPSettings))]
	public class NPSettingsInspector : ProductDashboardInspector
	{
		private enum eTabView
		{
			APPLICATION,
			BILLING,
			CONNECTVITY,
			MEDIA_LIBRARY,
			NOTIFICATION,
			TWITTER,
			UTILITY
		}

		#region Properties

		// Related to toolbar tabs
		private eTabView					m_activeView;
		private string[] 					m_toolbarItems;

		// Related to scrollview
		private Vector2						m_scrollPosition;

		#endregion

		#region Constants

		private const string				kActiveView							= "np-active-view";
		private const string				kToolBarButtonStyle					= "toolbarbutton";

		#endregion

		#region Unity Methods

		private void OnInspectorUpdate () 
		{
			// Call Repaint on OnInspectorUpdate as it repaints the windows
			// less times as if it was OnGUI/Update
			Repaint();
		}

		protected override void OnEnable ()
		{
			base.OnEnable ();

			// Toolbar items
			System.Array _viewNames	= System.Enum.GetNames(typeof(eTabView));
			m_toolbarItems			= new string[_viewNames.Length];

			for (int _iter = 0; _iter < _viewNames.Length; _iter++)
			{
				string _viewName		= _viewNames.GetValue(_iter).ToString().Replace('_', ' ').ToLower();
				string _displayName		= CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_viewName);

				m_toolbarItems[_iter]	= _displayName;
			}

			// Restoring last selection
			m_activeView		= (eTabView)EditorPrefs.GetInt(kActiveView, 0);
		}

		protected override void OnDisable ()
		{
			base.OnDisable ();

			// Save changes to settings
			EditorPrefs.SetInt(kActiveView, (int)m_activeView);
		}

	 	public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI();

			// Update object
			serializedObject.Update();

			// Settings toolbar
			GUIStyle _toolbarStyle	= new GUIStyle(kToolBarButtonStyle);
			_toolbarStyle.fontSize	= 12;

			// Make all EditorGUI look like regular controls
			EditorGUIUtility.LookLikeControls();
			
			m_scrollPosition		= EditorGUILayout.BeginScrollView(m_scrollPosition);
			{
				eTabView _selectedView	= (eTabView)GUILayout.Toolbar((int)m_activeView, m_toolbarItems, _toolbarStyle);
			
				if (_selectedView != m_activeView)
				{
					m_activeView		= _selectedView;
					
					// Remove current focus
					GUIUtility.keyboardControl 	= 0;

					// Reset scrollview position
					m_scrollPosition	= Vector2.zero;
				}

				// Drawing tabs
				EditorGUILayout.BeginVertical(UnityEditorUtility.kOuterContainerStyle);
				{	
					// Draw active view
					switch (m_activeView)
					{
					case eTabView.APPLICATION:
						ShowApplicationSettings();
						break;
						
					case eTabView.BILLING:
						ShowBillingSettings();
						break;
						
					case eTabView.CONNECTVITY:
						ShowNetworkConnectivitySettings();
						break;
						
					case eTabView.NOTIFICATION:
						ShowNotificationSettings();
						break;
						
					case eTabView.TWITTER:
						ShowTwitterSettings();
						break;
						
					case eTabView.MEDIA_LIBRARY:
						ShowMediaLibrarySettings();
						break;
						
					case eTabView.UTILITY:
						ShowUtilitySettings();
						break;
					}
				}
				EditorGUILayout.EndVertical();
			}
			EditorGUILayout.EndScrollView();

			// Apply modifications
			if (GUI.changed)
				serializedObject.ApplyModifiedProperties();
		}

		#endregion

		#region View Methods
		
		private void ShowApplicationSettings ()
		{
			DrawTabView("m_applicationSettings");
		}

		private void ShowBillingSettings ()
		{
			DrawTabView("m_billingSettings");
		}

		private void ShowNetworkConnectivitySettings ()
		{
			DrawTabView("m_networkConnectivitySettings");
		}

		private void ShowNotificationSettings ()
		{
			DrawTabView("m_notificationSettings");
		}

		private void ShowTwitterSettings ()
		{
			DrawTabView("m_twitterSettings");
		}

		private void ShowMediaLibrarySettings ()
		{
			DrawTabView("m_mediaLibrarySettings");
		}
		
		private void ShowUtilitySettings ()
		{
			DrawTabView("m_utilitySettings");
		}

		private void DrawTabView (string _propertyName)
		{
			SerializedProperty _property	= serializedObject.FindProperty(_propertyName);	

			// Draw child properties
			UnityEditorUtility.DrawChildPropertyFields(_property);
		}
		
		#endregion
	}
}
