using UnityEngine;
using System.Collections;
using System.Text;

namespace VoxelBusters.Utility
{
	public static class StringExtensions
	{
		#region String Operations

		//This will compare ignoring the case in both strings
		public static bool Contains (this string _string, string _stringToCheck, bool _ignoreCase)
		{
			if(!_ignoreCase)
			{
				return _string.Contains(_stringToCheck);
			}
			else
			{
				return _string.ToLower().Contains(_stringToCheck.ToLower());
			}
		}


		public static string ToBase64(this string _string)
		{
			byte[] _bytesToEncode 	= Encoding.UTF8.GetBytes (_string);
			string _encoded			= System.Convert.ToBase64String (_bytesToEncode);
			return _encoded;
		}

		public static string FromBase64(this string _string)
		{
			byte[] _bytesEncodedInBase64 = System.Convert.FromBase64String(_string);
			string _decoded	 = System.Text.Encoding.UTF8.GetString(_bytesEncodedInBase64);
			return _decoded;
		}

		#endregion
	}
}