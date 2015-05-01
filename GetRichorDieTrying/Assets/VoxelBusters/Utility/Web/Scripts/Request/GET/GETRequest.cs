using UnityEngine;
using System.Collections;
using System.Text;

namespace VoxelBusters.Utility
{
	public class GETRequest : WebRequest 
	{
		#region Constructors
		
		public GETRequest (URL _URL, object _params, bool _isAsynchronous): base(_URL, _params, _isAsynchronous)
		{
			WWWObject	= CreateWWWObject();
		}
		
		#endregion

		#region Overriden Methods

		protected override WWW CreateWWWObject ()
		{
			IDictionary _paramDict	= null;
			string _serverURL		= URL.URLString;

			// NULL parameter handling
			if (Parameters == null)
			{
				return new WWW(_serverURL);
			}

			// Supports string as parameter
			if (Parameters.GetType() == typeof(string))
			{
				return new WWW(_serverURL + "/" + (Parameters as string));
			}
			// Supports Dictionary as parameter
			else if ((_paramDict = (Parameters as IDictionary)) != null)
			{
				int _paramCount	= _paramDict.Count;
				
				if (_paramCount == 0)
				{
					return new WWW(_serverURL);
				}
				else
				{
					// Server url, will have parameters appended for Get request
					StringBuilder _urlBuilder	= new StringBuilder(_serverURL, _serverURL.Length);
					int _paramAdded				= 0;
					
					// Append parameters to url
					AppendParameters(null, _paramDict, _urlBuilder, ref _paramAdded);
					
					return new WWW(_urlBuilder.ToString());
				}
			}
			// Unsupported parameter
			else
			{
				Debug.LogError("[GETRequest] Invalid parameter");
				return null;
			}
		}

		private void AppendParameters (string _key, object _value, StringBuilder _urlBuilder, ref int _paramAdded)
		{
			// Generic dictionary type
			if (_value as IDictionary != null)
			{
				string _rootKey		= string.IsNullOrEmpty(_key) ? string.Empty : _key;
				IDictionary _dict	= (_value as IDictionary);
				
				foreach (object _dictKey in _dict)
					AppendParameters(string.Format("{0}[{1}]", _rootKey, _dictKey), _dict[_dictKey], _urlBuilder, ref _paramAdded);
				
				return;
			}
			// List or array type
			else if (_value as IEnumerable != null)
			{
				string _newKey	= _key + "[]";
				
				foreach (object _entry in (_value as IEnumerable))
					AppendParameters(_newKey, _entry, _urlBuilder, ref _paramAdded);
				
				return;
			}
			// Should be primitives or else entries will be inconsistent
			else
			{
				if (_paramAdded == 0)
					_urlBuilder.AppendFormat("?{0}={1}", _key, _value);
				else
					_urlBuilder.AppendFormat("?{0}={1}", _key, _value);
				
				// Increment as we added new parameter
				_paramAdded++;
				
				return;
			}
		}

		#endregion

		#region Static Methods
		
		public static GETRequest CreateRequest (URL _URL, object _params)
		{
			GETRequest _request	= new GETRequest(_URL,	_params, false);

			return _request;
		}
		
		public static GETRequest CreateAsyncRequest (URL _URL, object _params)
		{
			GETRequest _request	= new GETRequest(_URL,	_params, true);
			
			return _request;
		}
		
		#endregion
	}
}
