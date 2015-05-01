using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VoxelBusters.Utility
{
	public static class ComponentExtensions
	{	
		#region Methods

		public static T[] GetComponentsInChildren<T> (this Component _component, bool _includeParent, bool _includeInactive) 
													  where T : Component
		{
			T[] _components = _component.GetComponentsInChildren<T>(_includeInactive);

			if(_includeParent)
			{
				return _components;
			}
			else
			{
				List<T> _directChildren = new List<T>();

				foreach(T _each in _components)
				{
					if(_each.transform != _component.transform)
					{
						_directChildren.Add(_each);
					}
				}

				return _directChildren.ToArray();
			}
		}
		
		#endregion
	}
}

