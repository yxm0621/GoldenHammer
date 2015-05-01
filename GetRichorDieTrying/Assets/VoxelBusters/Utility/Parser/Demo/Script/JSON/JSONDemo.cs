using UnityEngine;
using System.Collections;

namespace VoxelBusters.Utility.Demo
{
	public class JSONDemo : MonoBehaviour 
	{
		#region Properties

		private string		m_JSONObject		= string.Empty;
		private string		m_result			= string.Empty;
		private ArrayList	m_GUIButtons		= new ArrayList(new string[] { "JSON --> C# Object" });

		#endregion

		#region Unity Methods

		private void OnGUI ()
		{
			// Input layout
			m_JSONObject	= GUIExtensions.TextArea(m_JSONObject, new Rect(0.05f, 0.01f, 0.9f, 0.44f));

			// Action layout
			GUIExtensions.Buttons(m_GUIButtons, OnGUIButtonPressed, new Rect(0.05f, 0.45f, 0.9f, 0.1f));

			// Result layout
			GUIExtensions.TextArea(m_result, new Rect(0.05f, 0.55f, 0.9f, 0.44f));
		}

		#endregion

		#region GUI Callback Methods

		private void OnGUIButtonPressed (string _buttonName)
		{
			object _value	= JSONUtility.FromJSON(m_JSONObject);
			
			if (_value != null)
			{
				m_result	= "Value=" + JSONUtility.ToJSON(_value) +
					"\nType=" + _value.GetType();
			}
			else
			{
				m_result	= "Something went wrong!!! Value=NULL";
			}
		}

		#endregion
	}
}