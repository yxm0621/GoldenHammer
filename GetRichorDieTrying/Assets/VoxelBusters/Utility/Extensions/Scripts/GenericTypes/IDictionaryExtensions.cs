using UnityEngine;
using System.Collections;

namespace VoxelBusters.Utility
{
	public static class IDictionaryExtensions 
	{
		public static T GetIfAvailable<T>(this IDictionary _dictionary, string _key)
		{
			T _value = default(T);
			
			if (_dictionary.Contains(_key))
			{
				_value = (T)System.Convert.ChangeType(_dictionary[_key],typeof(T));
			}
			
			return _value;
		}
	}
}
