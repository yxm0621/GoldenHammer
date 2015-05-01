using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Text;

namespace VoxelBusters.Utility
{
	using JSON.Internal;

	public static partial class JSONUtility 
	{
		#region Encode Methods
		
		public static string ToJSON (object _object)
		{
			StringBuilder _stringBuilder	= new StringBuilder();

			// Convert object to json string, using string builder
			ObjectToJSON(_object, _stringBuilder);

			return _stringBuilder.ToString();
		}

		public static bool IsNull (string _jsonStr)
		{
			return _jsonStr.Equals(Constants.kNull);
		}

		private static void ObjectToJSON (object _entry, StringBuilder _stringBuilder)
		{	
			// Null value
			if (_entry	== null)
			{
				_stringBuilder.Append(Constants.kNull);
				return;
			}

			Type _entryType	= _entry.GetType();

			if (_entryType.IsPrimitive)
			{
				// Boolean type
				if (_entry is bool)
				{
					if ((bool)_entry)
						_stringBuilder.Append(Constants.kBoolTrue);
					else
						_stringBuilder.Append(Constants.kBoolFalse);
					
					return;
				}
				// Char type
				else if (_entry is char)
				{
					_stringBuilder.Append('"').Append((char)_entry).Append('"');
					return;
				}
				// Enum type
				else if (_entryType.IsEnum)
				{
					_stringBuilder.Append(((int)_entry).ToString());
					return;
				}
				// Numeric type
				else
				{
					_stringBuilder.Append(_entry);
					return;
				}
			}
			// Array type
			else if (_entryType.IsArray)
			{
				ArrayToJSON(_entry as System.Array, _stringBuilder);
				return;
			}
			// Generic list type
			else if (_entry as IList != null)
			{
				IListToJSON(_entry as IList, _stringBuilder);
				return;
			}
			// Generic dictionary type
			else if (_entry as IDictionary != null)
			{
				IDictionaryToJSON(_entry as IDictionary, _stringBuilder);
				return;
			}

			// Other types
			StringToJSON(_entry.ToString(), _stringBuilder);
			return;
		}
		
		private static void IDictionaryToJSON (IDictionary _dict, StringBuilder _stringBuilder)
		{
			bool _firstEntry						= true;
			IDictionaryEnumerator _dictEnumerator	= _dict.GetEnumerator();

			// Initialise with symbol to indicate start of hash
			_stringBuilder.Append('{');
			
			// Iterate through all keys
			while (_dictEnumerator.MoveNext())
			{
				if (!_firstEntry)
					_stringBuilder.Append(',');

				// Key
				StringToJSON(_dictEnumerator.Key.ToString(), _stringBuilder);	

				// Seperator
				_stringBuilder.Append(':');				

				// Value
				ObjectToJSON(_dictEnumerator.Value, _stringBuilder);
				
				// Not first entry anymore
				_firstEntry	= false;
			}
			
			// Append symbol to indicate end of json string representation of dictionary
			_stringBuilder.Append('}');
			return;
		}
		
		private static void ArrayToJSON (System.Array _array, StringBuilder _stringBuilder)
		{
			// Initialise with symbol to indicate start of array
			_stringBuilder.Append('[');

			switch (_array.Rank)
			{
			case 1:
				int _1DArrayLength				= _array.Length;
				int _1DArrayLengthMinus1Index	= _1DArrayLength - 1;

				for (int _iter = 0; _iter < _1DArrayLength; _iter++) 
				{
					ObjectToJSON(_array.GetValue(_iter), _stringBuilder);

					if (_iter < _1DArrayLengthMinus1Index)
						_stringBuilder.Append(',');
				}
				
				break;
				
			case 2:
				int _outerArrayLength				= _array.GetLength(0);
				int _innerArrayLength				= _array.GetLength(1);
				int _outerArrayLengthMinus1Index	= _outerArrayLength - 1;
				int _innerArrayLengthMinus1Index	= _innerArrayLength - 1;
				
				for (int _iter1 = 0; _iter1 < _outerArrayLength; _iter1++)
				{
					// Append symbol to indicate start of json string representation of inner array
					_stringBuilder.Append('[');
					
					for (int _iter2 = 0; _iter2 < _innerArrayLength; _iter2++)
					{
						ObjectToJSON(_array.GetValue(_iter1, _iter2), _stringBuilder);
						
						if (_iter2 < _innerArrayLengthMinus1Index)
							_stringBuilder.Append(',');
					}

					// Append symbol to indicate end of json string representation of inner array
					_stringBuilder.Append(']');
					
					if (_iter1 < _outerArrayLengthMinus1Index)
						_stringBuilder.Append(',');
				}
				
				break;
			}
			
			// Append symbol to indicate end of json string representation of array
			_stringBuilder.Append(']');
			return;
		}
		
		private static void IListToJSON (IList _list, StringBuilder _stringBuilder)
		{
			// Initialise with symbol to indicate start of list
			_stringBuilder.Append('[');

			int _totalCount					= _list.Count;
			int _totalCountMinus1Index		= _totalCount - 1;

			for (int _iter = 0; _iter < _totalCount; _iter++) 
			{
				ObjectToJSON(_list[_iter], _stringBuilder);
				
				if (_iter < _totalCountMinus1Index)
					_stringBuilder.Append(',');
			}
			
			// Append symbol to indicate end of json string representation of array
			_stringBuilder.Append(']');
			return;
		}

		private static void StringToJSON (string _string, StringBuilder _stringBuilder)
		{
			int _cCount			= _string.Length;
			int _cIter			= 0;
		
			// Append quotes to indicate start of string
			_stringBuilder.Append('"');

			while (_cIter < _cCount)
			{
				char _char	= _string[_cIter++];

				if (_char == '"') 
				{
					_stringBuilder.Append('\\').Append('"');
				}
				else if (_char == '\\') 
				{
					_stringBuilder.Append('\\').Append('\\');
				}
				else if (_char == '/') 
				{
					_stringBuilder.Append('\\').Append('/');
				}
				else if (_char == '\b') 
				{
					_stringBuilder.Append('\\').Append('b');
				}
				else if (_char == '\f') 
				{
					_stringBuilder.Append('\\').Append('f');
				}
				else if (_char == '\n')
				{
					_stringBuilder.Append('\\').Append('n');
				}
				else if (_char == '\r')
				{
					_stringBuilder.Append('\\').Append('r');
				}
				else if (_char == '\t')
				{
					_stringBuilder.Append('\\').Append('t');
				}
				else if (_char > 127) 
				{	
					string _unicode = "\\u" + ((int)_char).ToString("x4");
					_stringBuilder.Append(_unicode);
				}
				else
				{
					_stringBuilder.Append(_char);
				}
			}

			// Append quotes to indicate end of string
			_stringBuilder.Append('"');

			return;
		}

		#endregion
	}
}