using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VoxelBusters.Utility
{
	public struct WebResponse
	{
		#region Properties

		public int			Status
		{
			get;
			private set;
		}

		public string		Message
		{
			get;
			private set;
		}
		
		public IDictionary	Data
		{
			get;
			private set;
		}
		
		public List<string>	Errors
		{
			get;
			private set;
		}

		#endregion

		#region Constructors

		public WebResponse (IDictionary _jsonResponse)
		{
			Status	= _jsonResponse.GetIfAvailable<int>("status");

			if (_jsonResponse.Contains("response"))
			{
				IDictionary _responseDict	= _jsonResponse["response"] as IDictionary;

				if (_responseDict.Contains("data"))
				{
					Data	= _responseDict["data"] as IDictionary;
					Message	= _responseDict.GetIfAvailable<string>("message");
					Errors	= _responseDict.GetIfAvailable<List<string>>("errors");
				}
			}
		}

		public WebResponse (string _errorText)
		{
			Status	= 0;
			Message	= null;
			Data	= null;

			// Errors
			Errors	= new List<string>();
			Errors.Add(_errorText);
		}

		#endregion

		#region

		#endregion
	}
}
