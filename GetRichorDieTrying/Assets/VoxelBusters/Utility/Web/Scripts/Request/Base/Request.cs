using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace VoxelBusters.Utility
{
	public abstract class Request 
	{
		#region Properties

		public bool 			IsAsynchronous 		 	
		{ 
			get; 
			private set; 
		}

		public URL	 			URL  				 	
		{ 
			get; 
			private set; 
		}
		
		protected WWW			WWWObject		 	
		{ 
			get; 
			set; 
		}

		#endregion

		#region Constructors

		private Request ()
		{}

		protected Request (URL _URL, bool _isAsynchronous) 
		{
			this.URL				= _URL;
			this.IsAsynchronous		= _isAsynchronous;
		}

		#endregion

		#region Handling Requests

		public void StartRequest ()
		{
			if (WWWObject == null || string.IsNullOrEmpty(URL.URLString))
			{
				Debug.LogError("[WebRequest] Sending request is aborted");
				return;
			}

			if (IsAsynchronous)
			{
#if UNITY_EDITOR
				// Coroutine to run in editor mode
				if (!Application.isPlaying)
				{
					EditorCoroutine.StartCoroutine(StartAsynchronousRequest());
					return;
				}
#endif
				RequestManager.Instance.StartCoroutine(StartAsynchronousRequest());
			}
			else
			{
				while (!WWWObject.isDone)
				{}

				OnFetchingResponse();
			}
		}

		private IEnumerator StartAsynchronousRequest ()
		{
			while (!WWWObject.isDone)
				yield return null;

			OnFetchingResponse();
		}
		
		public void Abort ()
		{
			if (WWWObject != null && !WWWObject.isDone)
				WWWObject.Dispose();
		}
		
		#endregion

		#region Handling Response 

		protected abstract void OnFetchingResponse ();

		#endregion
	}
}