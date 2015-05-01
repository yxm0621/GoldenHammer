using UnityEngine;
using System.Collections;

namespace VoxelBusters.Utility
{
	public static class GameObjectExtensions  
	{
		public static GameObject AddChild (this GameObject _parentGO, string _childName)
		{
			GameObject _childGO				= new GameObject(_childName);

			return _parentGO.AddChild(_childGO, Vector3.zero, Quaternion.identity, Vector3.zero);
		}

		public static GameObject AddChild (this GameObject _parentGO, string _childName, Vector3 _localPosition, 
		                                   Quaternion _localRotation, Vector3 _localScale)
		{
			GameObject _childGO				= new GameObject(_childName);

			return _parentGO.AddChild(_childGO, _localPosition, _localRotation, _localScale);
		}

		public static GameObject AddChild (this GameObject _parentGO, GameObject _childGO)
		{
			return _parentGO.AddChild(_childGO, Vector3.zero, Quaternion.identity, Vector3.zero);
		}

		public static GameObject AddChild (this GameObject _parentGO, GameObject _childGO, Vector3 _localPosition, 
		                                   Quaternion _localRotation, Vector3 _localScale)
		{
			// Parent and child transform
			Transform _parentTransform		= _parentGO.transform;
			Transform _childTransform		= _childGO.transform;

			// Set as parent
			_childTransform.parent			= _parentTransform;

			// Set local position, rotation, scale
			_childTransform.localPosition	= _localPosition;
			_childTransform.localRotation	= _localRotation;
			_childTransform.localScale		= _localScale;

			return _childGO;
		}
	}
}
