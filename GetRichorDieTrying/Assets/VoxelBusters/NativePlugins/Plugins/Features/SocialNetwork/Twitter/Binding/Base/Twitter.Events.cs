using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;
using VoxelBusters.DebugPRO;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class Twitter : MonoBehaviour 
	{
		#region Delegates

		/// <summary>
		/// Use this delegate type to get callback when user login succeeds or fails.
		/// </summary>
		/// <param name="_session">Contains the OAuth tokens and minimal information associated with the logged in user or nil.</param>
		/// <param name="_error">Error that will be non nil if the authentication request failed.</param>
		public delegate void TWTRLoginCompletion (TwitterSession _session, string _error);

		/// <summary>
		/// Use this delegate type to get callback when the user finishes composing a Tweet.
		/// </summary>
		public delegate void TWTRTweetCompletion (eTwitterComposerResult _result);

		/// <summary>
		/// Use this delegate type to get callback when load user details request succeeds or fails.
		/// </summary>
		/// <param name="_user">The Twitter User.</param>
		/// <param name="_error">Error that will be set if the API request failed.</param>
		public delegate void TWTRAccountDetailsCompletion (TwitterUser _user, string _error);

		/// <summary>
		/// Use this delegate type to get callback when the user accepts or denies access to their email address.
		/// </summary>
		/// <param name="_email">The user's email address. This will be nil if the user does not grant access to their email address or your application is not allowed to request email addresses.</param>
		/// <param name="_error">An error that details why a user's email address could not be provided.</param>
		public delegate void TWTREmailAccessCompletion (string _email, string _error);

		/// <summary>
		/// Use this delegate type to get callback when the network request succeeds or fails.
		/// </summary>
		/// <param name="_response">Content data of the response.</param>
		/// <param name="_error">Describes the network error that occurred.</param>
		public delegate void TWTRResonse (IDictionary _response, string _error);

		#endregion

		#region Events
		
		protected TWTRLoginCompletion				OnLoginFinished;
		protected TWTRTweetCompletion				OnTweetComposerClosed;
		protected TWTRAccountDetailsCompletion		OnRequestAccountDetailsFinished;
		protected TWTREmailAccessCompletion			OnRequestEmailAccessFinished;
		protected TWTRResonse						OnTwitterURLRequestFinished;
		
		#endregion

		#region Native Callback Methods

		protected void TwitterLoginSuccess (string _sessionJsonStr)
		{
			// Resume unity player
			this.ResumeUnity();

			// Get session json object
			IDictionary _sessionJsonDict	= JSONUtility.FromJSON(_sessionJsonStr) as IDictionary;
			TwitterSession _session;

			// Parse received data
			ParseSessionData(_sessionJsonDict, out _session);
			Console.Log(Constants.kDebugTag, "[Twitter] Login success, Session=" + _session);

			// Trigger event
			if (OnLoginFinished != null)
				OnLoginFinished(_session, null);
		}
		
		protected void TwitterLoginFailed (string _error)
		{
			Console.Log(Constants.kDebugTag, "[Twitter] Login failed");

			// Resume unity player
			this.ResumeUnity();

			if (OnLoginFinished != null)
				OnLoginFinished(null, _error);
		}
		
		protected void TweetComposerDismissed (string _resultStr)
		{
			// Resume unity player
			this.ResumeUnity();

			eTwitterComposerResult _result;

			// Parse received data
			ParseTweetComposerDismissedData(_resultStr, out _result);
			Console.Log(Constants.kDebugTag, "[Twitter] Tweet composer was dismissed, Result=" + _result);

			if (OnTweetComposerClosed != null)
				OnTweetComposerClosed(_result);
		}
		
		protected void RequestAccountDetailsSuccess (string _userJsonStr)
		{
			// Get twitter user json object
			IDictionary _userJsonDict	= JSONUtility.FromJSON(_userJsonStr) as IDictionary;
			TwitterUser _user;

			// Parse received data
			ParseUserData(_userJsonDict, out _user);
			Console.Log(Constants.kDebugTag, "[Twitter] Accessed account details, User=" + _user);

			// Trigger event
			if (OnRequestAccountDetailsFinished != null)
				OnRequestAccountDetailsFinished(_user, null);
		}
		
		protected void RequestAccountDetailsFailed (string _error)
		{
			Console.Log(Constants.kDebugTag, "[Twitter] Accessing account details failed, Error=" + _error);

			if (OnRequestAccountDetailsFinished != null)
				OnRequestAccountDetailsFinished(null, _error);
		}
		
		protected void RequestEmailAccessSuccess (string _email)
		{
			Console.Log(Constants.kDebugTag, "[Twitter] Access to Email is permitted, Email=" + _email);

			// Resume unity player
			this.ResumeUnity();

			if (OnRequestEmailAccessFinished != null)
				OnRequestEmailAccessFinished(_email, null);
		}
		
		protected void RequestEmailAccessFailed (string _error)
		{
			Console.Log(Constants.kDebugTag, "[Twitter] Access to Email is not permitted, Error=" + _error);

			// Resume unity player
			this.ResumeUnity();

			if (OnRequestEmailAccessFinished != null)
				OnRequestEmailAccessFinished(null, _error);
		}
		
		protected void TwitterURLRequestSuccess (string _responseJsonStr)
		{
			Console.Log(Constants.kDebugTag, "[Twitter] URL request success, Response=" + _responseJsonStr);

			if (OnTwitterURLRequestFinished != null)
			{
				// Get response data
				IDictionary _responseJsonDict	= JSONUtility.FromJSON(_responseJsonStr) as IDictionary;

				// Trigger event
				OnTwitterURLRequestFinished(_responseJsonDict, null);
			}
		}
		
		protected void TwitterURLRequestFailed (string _error)
		{
			Console.Log(Constants.kDebugTag, "[Twitter] URL request failed, Error=" + _error);

			if (OnTwitterURLRequestFinished != null)
				OnTwitterURLRequestFinished(null, _error);
		}

		#endregion

		#region Parse Methods

		protected virtual void ParseSessionData (IDictionary _sessionDict, out TwitterSession _session)
		{
			_session	= null;
		}
		
		protected virtual void ParseTweetComposerDismissedData (string _resultStr, out eTwitterComposerResult _result)
		{
			_result		= (eTwitterComposerResult)int.Parse(_resultStr);
		}
		
		protected virtual void ParseUserData (IDictionary _userDict, out TwitterUser _user)
		{
			_user		= null;
		}

		#endregion
	}
}
