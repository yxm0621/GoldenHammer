﻿using UnityEngine;
using System.Collections;

namespace VoxelBusters.Utility.JSON.Internal
{
	public struct JSONString
	{
		public string 	Value
		{
			get;
			private set;
		}
		
		public bool 	IsNullOrEmpty
		{
			get;
			private set;
		}
		
		public int		Length
		{
			get;
			private set;
		}

		public char 	this[int _index]
		{
			get
			{
				return Value[_index];
			}
		}
		
		public JSONString (string _JSONString)
		{
			Value			= _JSONString;
			IsNullOrEmpty	= string.IsNullOrEmpty(_JSONString);
			Length			= IsNullOrEmpty ? 0 : _JSONString.Length;
		}
	}
}