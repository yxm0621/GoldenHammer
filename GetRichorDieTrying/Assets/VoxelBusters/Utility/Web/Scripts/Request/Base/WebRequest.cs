using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace VoxelBusters.Utility
{
	public abstract class WebRequest : Request
	{
		#region Delegates

		public delegate void JSONResponse (WebResponse _response);

		#endregion

		#region Properties
		
		public object 			Parameters  		 	
		{ 
			get; 
			set; 
		}

		public JSONResponse 	OnSuccess  	 	
		{ 
			get; 
			set; 
		}

		public JSONResponse		OnFailure  	 	
		{ 
			get; 
			set; 
		}

		#endregion

		#region Constructors
		
		public WebRequest (URL _URL, object _params, bool _isAsynchronous) : base(_URL, _isAsynchronous)
		{
			this.Parameters			= _params;
		}

		#endregion

		#region Handling Requests
		
		protected abstract WWW CreateWWWObject ();

		#endregion

		#region Handling Response 

		protected override void OnFetchingResponse ()
		{
			WebResponse _response;

			// Create respone based on completion data
			if (string.IsNullOrEmpty(WWWObject.error))
			{
				_response = new WebResponse(JSONUtility.FromJSON(WWWObject.text) as IDictionary);
			}
			else
			{
				_response = new WebResponse(WWWObject.error);
			}

			if (_response.Status == 200 || _response.Status == 400)
			{
				if (OnSuccess != null)
				{
					OnSuccess(_response);
				}
			}
			else
			{
				if (OnFailure != null)
				{
					OnFailure(_response);
				}
			}
		}

		#endregion
	}
}