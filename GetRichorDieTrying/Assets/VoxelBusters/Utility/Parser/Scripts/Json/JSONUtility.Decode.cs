using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace VoxelBusters.Utility
{
	using JSON.Internal;

	/// <summary>
	/// JSON utility provides interface to encode and decode JSON strings.
	/// </summary>
	/// <description>
	/// Here is how we mapped JSON type to C# type
	/// JSON array corresponds to type IList.
	/// JSON objects corresponds to type IDictionary.
	/// JSON intergers corresponds to type long.
	/// JSON real numbers corresponds to type double.
	/// Credits: http://techblog.procurios.nl/k/618/news/view/14605/14863/How-do-I-write-my-own-parser-for-JSON.html
	/// </description>
	public static partial class JSONUtility 
	{
		#region Decode Methods

		public static object FromJSON (string _inputJSONString)
		{
			int _errorIndex	= 0;

			return FromJSON(_inputJSONString, ref _errorIndex);
		}

		public static object FromJSON (string _inputJSONString, ref int _errorIndex)
		{
			JSONString _JSONString	= new JSONString(_inputJSONString);

			if (_JSONString.IsNullOrEmpty)
				return null;
			
			// Get json string parsed object
			int _index		= 0;
			object _value	= ParseValue(_JSONString, ref _index);

			if (_index != _JSONString.Length)
				_errorIndex	= _index;
			else
				_errorIndex	= -1;

			return _value;
		}
		
		private static object ParseValue (JSONString _JSONString, ref int _index)
		{
			// Remove white spaces
			RemoveWhiteSpace(_JSONString, ref _index);

			switch (LookAhead(_JSONString, _index))
			{
			case Token.CURLY_OPEN: 
				return ParseObject(_JSONString, ref _index);

			case Token.SQUARED_OPEN:
				return ParseArray(_JSONString, ref _index);

			case Token.STRING:
				return ParseString(_JSONString, ref _index);

			case Token.NUMBER:
				return ParseNumber(_JSONString, ref _index);

			case Token.NULL:
				_index	+= 4;
				return null;

			case Token.TRUE:
				_index += 4;
				return true;

			case Token.FALSE:
				_index += 5;
				return false;

			case Token.NONE:
				break;
			}
			
			return null;
		}
		
		private static object ParseObject (JSONString _JSONString, ref int _index)
		{
			IDictionary _dictionary 	= new Dictionary<string, object>();
			bool _done 					= false;

			// Skip curls
			_index++;
			
			while (!_done) 
			{
				Token _token = LookAhead(_JSONString, _index);

				if (_token == Token.NONE) 
				{
					return null;
				}  
				else if (_token == Token.COMMA) 
				{
					_index++;
				} 
				else if (_token == Token.CURLY_CLOSE) 
				{
					_index++;
					return _dictionary;
				}
				else 
				{
					object _key	= null;

					if (_token == Token.STRING)
					{
						_key	= ParseValue(_JSONString, ref _index);
					}
					
					// Cross check if we have key or not
					if (_key == null) 
					{
						return null;
					}
					
					// Next token should be colon
					_token 		= NextToken(_JSONString, ref _index);

					if (_token != Token.COLON) 
					{
						return null;
					}
					
					// Set key value pair
					_dictionary[_key]	= ParseValue(_JSONString, ref _index);
				}
			}
			
			return _dictionary;
		}
		
		private static object ParseArray (JSONString _JSONString, ref int _index)
		{
			IList _arraylist 	= new List<object>();
			bool _done 			= false;

			// Skip square bracket
			_index++;
			
			while (!_done) 
			{
				Token _token = LookAhead(_JSONString, _index);

				if (_token == Token.NONE) 
				{
					return null;
				} 
				else if (_token == Token.COMMA) 
				{
					_index++;
				} 
				else if (_token == Token.SQUARED_CLOSE) 
				{
					_index++;
					return _arraylist;
				} 
				else 
				{
					object _value = ParseValue(_JSONString, ref _index);

					// Add element to the json list
					_arraylist.Add(_value);
				}
			}
			
			return _arraylist;
		}
		
		private static string ParseString (JSONString _JSONString, ref int _index)
		{
			StringBuilder _strbuilder	= new StringBuilder();
			bool _done					= false;
			
			// Skip double quotes
			_index++;
			
			while (!_done)
			{
				// We are done with the json string
				if (_index == _JSONString.Length)
					break;
				
				// Get current character and increment pointer index
				char _curChar	= _JSONString[_index++];
				
				// We reached end of our string
				if (_curChar == '"')
				{
					_done	= true;
				}
				else if (_curChar == '\\')
				{
					// We are done with the json string
					if (_index == _JSONString.Length)
						break;
					
					// Get current character and increment pointer index
					_curChar	= _JSONString[_index++];
					
					if (_curChar == '"') 
						_strbuilder.Append('"');
					else if (_curChar == '\\') 
						_strbuilder.Append('\\');
					else if (_curChar == '/') 
						_strbuilder.Append('/');
					else if (_curChar == 'b') 
						_strbuilder.Append('\b');
					else if (_curChar == 'f') 
						_strbuilder.Append('\f');
					else if (_curChar == 'n')
						_strbuilder.Append('\n');
					else if (_curChar == 'r')
						_strbuilder.Append('\r');
					else if (_curChar == 't')
						_strbuilder.Append('\t');
					else if (_curChar == 'u') 
					{
						int _remLength = _JSONString.Length - _index;
						
						if (_remLength >= 4) 
						{
							string _unicodeStr 	= _JSONString.Value.Substring(_index, 4);
							
							// Append unicode char to string 
							char _unicodeChar	= (char)int.Parse(_unicodeStr, System.Globalization.NumberStyles.HexNumber);

							_strbuilder.Append(_unicodeChar);
							
							// Skip next 4 characters
							_index += 4;
						} 
						else 
						{
							break;
						}					
					}
				}
				else 
				{
					_strbuilder.Append(_curChar);
				}
			}
			
			if (!_done) 
				return null;
			
			return _strbuilder.ToString();
		}
		
		private static object ParseNumber (JSONString _JSONString, ref int _index)
		{
			int _numIndex	= _index;
			bool _done		= false;

			while (!_done)
			{
				if (Constants.kNumericLiterals.IndexOf(_JSONString[_numIndex]) != -1)
					_numIndex++;
				else
					_done	= true;
			}
			
			// Get number sequence
			int _strLength			= _numIndex - _index;
			string _numberString	= _JSONString.Value.Substring(_index, _strLength);
			
			// Update look ahead index, point it to next char after end of number string
			_index					= _numIndex;
			
			// First try to parse an number as long int
			long _longValue;

			if (long.TryParse(_numberString, out _longValue))
				return _longValue;
			
			// As long int parsing failed, try parsing it as double
			double _doubleValue;

			if (double.TryParse(_numberString, out _doubleValue))
				return _doubleValue;
			
			return null;
		}
		
		private static Token LookAhead (JSONString _JSONString, int _index)
		{
			int _indexCopy	= _index;

			// Find next token without affecting indexing
			return NextToken(_JSONString, ref _indexCopy);
		}
		
		private static Token NextToken (JSONString _JSONString, ref int _index)
		{
			// Check if exceeded json string length
			if (_index == _JSONString.Length) 
				return Token.NONE;

			// Remove spacing
			RemoveWhiteSpace(_JSONString, ref _index);

			// Cache current character
			char _char	= _JSONString[_index++];

			switch (_char)
			{
			case '{': 
				return Token.CURLY_OPEN;

			case '}': 
				return Token.CURLY_CLOSE;

			case '[': 
				return Token.SQUARED_OPEN;

			case ']': 
				return Token.SQUARED_CLOSE;

			case ':': 
				return Token.COLON;

			case ',': 
				return Token.COMMA;

			case '"': 
				return Token.STRING;

			case '0': case '1': case '2': case '3': case '4': 
			case '5': case '6': case '7': case '8': case '9':
			case '-': 
				return Token.NUMBER;
			}

			// Reverting post increment which was done after reading character
			_index--;
			
			// Can be null data
			if ((_index + 4) < _JSONString.Length)
			{
				if (('n' == _JSONString[_index]) &&
				    ('u' == _JSONString[_index + 1]) &&
				    ('l' == _JSONString[_index + 2]) &&
				    ('l' == _JSONString[_index + 3]))
				{
					_index += 4;
					return Token.NULL;
				}
			}
			
			// Can be boolean true
			if ((_index + 4) < _JSONString.Length)
			{
				if (('t' == _JSONString[_index]) &&
				    ('r' == _JSONString[_index + 1]) &&
				    ('u' == _JSONString[_index + 2]) &&
				    ('e' == _JSONString[_index + 3]))
				{
					_index += 4;
					return Token.TRUE;
				}
			}
			
			// Can be boolean false
			if ((_index + 5) < _JSONString.Length)
			{
				if (('f' == _JSONString[_index]) &&
				    ('a' == _JSONString[_index + 1]) &&
				    ('l' == _JSONString[_index + 2]) &&
				    ('s' == _JSONString[_index + 3]) &&
				 	('e' == _JSONString[_index + 4]))
				{
					_index += 5;
					return Token.FALSE;
				}
			}
			
			return Token.NONE;
		}

		private static void RemoveWhiteSpace (JSONString _JSONString, ref int _index)
		{
			int _charCount	= _JSONString.Length;

			while (_index < _charCount)
			{
				char _char	= _JSONString[_index];

				if (Constants.kWhiteSpaceLiterals.IndexOf(_char) != -1)
				{
					_index++;
				}
				else
				{
					break;
				}
			}
		}
		
		#endregion
	}
}