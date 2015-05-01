using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Demo
{
	public class NPDemoMainMenu : NPDemoGUIWindow 
	{
		private NPDemoSubMenu[] m_subMenuList;
	
		private NPDemoSubMenu	m_currentSubMenu;
	
	
	
		// Use this for initialization
		protected override void Start () 
		{
			base.Start();
	
			//Get list of all GUIDrawer under this list
			m_subMenuList		= this.GetComponentsInChildren<NPDemoSubMenu>();
	
			foreach(NPDemoGUIWindow _each in m_subMenuList) //Setting MainMenu skin by default if UISkin not set for submenus
			{
				if(UISkin != null)
				{
					if(_each.UISkin == null)
					{
						_each.UISkin = UISkin;
					}
				}
			}
		
			//Disable all children initially
			DisableAllSubMenus();
		}
	
		void Update ()
		{
	
			if(m_currentSubMenu != null && !m_currentSubMenu.gameObject.activeSelf)
			{
				m_currentSubMenu = null;
			}
	
		}
		
		void DisableAllSubMenus()
		{
			foreach(NPDemoSubMenu each in m_subMenuList)
			{
				each.gameObject.SetActive(false);
			}
		}
	
		void EnableSubMenu(NPDemoSubMenu _enabledSubMenu)
		{
			DisableAllSubMenus();
	
			//Enable new feature window
			_enabledSubMenu.gameObject.SetActive(true);
			
			//Save the window instance
			m_currentSubMenu = _enabledSubMenu;
	
		}
	
	
		#region Drawing
	
		protected override void OnGUIWindow()
		{		
	
			if(m_currentSubMenu == null)
			{
				m_rootScrollView.BeginScrollView();
				{
					GUILayout.BeginVertical(UISkin.scrollView);
		
						GUILayout.Box(name);
					
						//If on, just list all the features
						for(int _i = 0 ; _i <  m_subMenuList.Length ; _i++)
						{
							NPDemoSubMenu  _each = 	m_subMenuList[_i];
							if(GUILayout.Button(_each.gameObject.name))
							{
								EnableSubMenu(_each);
								break;
							}
						}
		
					GUILayout.EndVertical();
				}
				m_rootScrollView.EndScrollView();
				
				GUILayout.FlexibleSpace();
				
				
			}
	
		}
	
		#endregion
	}
}
