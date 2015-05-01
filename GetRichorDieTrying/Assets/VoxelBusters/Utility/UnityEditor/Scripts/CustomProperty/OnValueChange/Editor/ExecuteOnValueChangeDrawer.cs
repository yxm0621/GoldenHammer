﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Reflection;

namespace VoxelBusters.Utility
{
	[CustomPropertyDrawer(typeof(ExecuteOnValueChangeAttribute))]
	public class ExecuteOnValueChangeDrawer : PropertyDrawer 
	{
		#region Properties

		private ExecuteOnValueChangeAttribute ExecuteOnValueChange 
		{ 
			get { return ((ExecuteOnValueChangeAttribute)attribute); } 
		}

		#endregion

		#region Drawer Methods

		public override float GetPropertyHeight (SerializedProperty _property, GUIContent _label) 
		{
			return base.GetPropertyHeight(_property, _label);
		}

		public override void OnGUI (Rect _position, SerializedProperty _property, GUIContent _label)
		{
			EditorGUI.BeginProperty(_position, _label, _property);

			// Start checking if property was changed
			EditorGUI.BeginChangeCheck();

			// Call base class to draw property
			EditorGUI.PropertyField(_position, _property, _label, true);

			// Finish checking and invoke method if value is changed
			if (EditorGUI.EndChangeCheck())
			{
				object _instance		= _property.serializedObject.targetObject;
				MethodInfo _methodInfo	= _instance.GetType().GetMethod(ExecuteOnValueChange.Function);

				if (_methodInfo != null)
				{
					ParameterInfo[] _paramaters = _methodInfo.GetParameters();
					int _parametersCount		= _paramaters.Length;

					if (_parametersCount == 0)
						_methodInfo.Invoke(_instance, null);
				}
			}

			EditorGUI.EndProperty();
		}

		#endregion
	}
}
