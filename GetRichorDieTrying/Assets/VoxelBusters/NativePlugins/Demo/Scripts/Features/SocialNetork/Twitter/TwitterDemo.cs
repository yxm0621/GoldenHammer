using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Demo
{
	public class TwitterDemo : NPDemoSubMenu 
	{
		#region Properties

		[SerializeField]
		private string 			m_shareMessage		= "this is what I wanted to share";

		[SerializeField]
		private string 			m_shareURL			= "http://www.voxelbusters.com";

		[SerializeField]
		private Texture2D		m_shareImage;

		#endregion

		#region API Calls

		private void Login ()
		{
			NPBinding.Twitter.Login(LoginFinished);
		}

		private void Logout ()
		{
			NPBinding.Twitter.Logout();
			AddNewResult("Logged out successfully");
		}

		private void ShowTweetComposerWithMessage ()
		{
			NPBinding.Twitter.ShowTweetComposerWithMessage(m_shareMessage, DismissedTweetComposer);
		}

		private void ShowTweetComposerWithLink ()
		{
			NPBinding.Twitter.ShowTweetComposerWithLink(m_shareMessage, m_shareURL, DismissedTweetComposer);
		}
		
		private void ShowTweetComposerWithScreenshot ()
		{
			NPBinding.Twitter.ShowTweetComposerWithScreenshot(m_shareMessage, (eTwitterComposerResult _result)=>{
				AddNewResult("Closed tweet composer");
				AppendResult("Result=" + _result);
			});
		}

		private void ShowTweetComposerWithImage ()
		{
			NPBinding.Twitter.ShowTweetComposerWithImage(m_shareMessage, m_shareImage, DismissedTweetComposer);
		}

		private void RequestAccountDetails ()
		{
			NPBinding.Twitter.RequestAccountDetails(AccountDetailsRequestFinished);
		}

		private void RequestEmailAccess ()
		{
			NPBinding.Twitter.RequestEmailAccess(EmailAccessRequestFinished);
		}

		private void MakeURLRequest ()
		{
			string _URL 		= "https://api.twitter.com/1.1/statuses/show.json";
			IDictionary _params = new Dictionary<string, string>(){
				{"id", "20"}
			};
			
			NPBinding.Twitter.GetURLRequest(_URL, _params, URLRequestFinished);
		}
	
		#endregion

		#region API Callbacks

		private void LoginFinished (TwitterSession _session, string _error)
		{
			if (string.IsNullOrEmpty(_error))
			{
				AddNewResult("Successfully logged-in");
				AppendResult("Twitter Session=" + _session);
			}
			else
			{
				AddNewResult("Failed to login");
				AppendResult("Error=" + _error);
			}
		}

		private void AccountDetailsRequestFinished (TwitterUser _user, string _error)
		{
			if (string.IsNullOrEmpty(_error))
			{
				AddNewResult("Received account details");
				AppendResult("Twitter User=" + _user);
			}
			else
			{
				AddNewResult("Failed to receive account details");
				AppendResult("Error=" + _error);
			}
		}

		private void DismissedTweetComposer (eTwitterComposerResult _result)
		{
			AddNewResult("Closed tweet composer");
			AppendResult("Result=" + _result);
		}

		private void EmailAccessRequestFinished (string _email, string _error)
		{
			if (string.IsNullOrEmpty(_error))
			{
				AddNewResult("Received access to user's emailID");
				AppendResult("EmailID=" + _email);
			}
			else
			{
				AddNewResult("Failed to access user's emailID information");
				AppendResult("Error=" + _error);
			}
		}

		private void URLRequestFinished (IDictionary _response, string _error)
		{
			if (string.IsNullOrEmpty(_error))
			{
				AddNewResult("Received response for URL request");
				AppendResult("Response Data=" + _response.ToJSON());
			}
			else
			{
				AddNewResult("URL request failed with errors");
				AppendResult("Error=" + _error);
			}

			AppendResult("Dont forget to check PostURLRequest, PutURLRequest, DeleteURLRequest");
		}

		#endregion

		#region UI
		
		protected override void OnGUIWindow()
		{		
			base.OnGUIWindow();

			m_rootScrollView.BeginScrollView();
			{
				// Start vertical column
				GUILayout.BeginVertical(UISkin.scrollView);
				{
					GUILayout.Label("Authentication", kSubTitleStyle);

					if (GUILayout.Button("Login"))
					{
						Login();
					}
					
					if (GUILayout.Button("Logout"))
					{
						Logout();
					}
					
					if (GUILayout.Button("IsLoggedIn"))
					{
						bool _isLoggedIn 	= NPBinding.Twitter.IsLoggedIn();
						AddNewResult("Is Loggedin=" + _isLoggedIn);
					}
				}
				GUILayout.EndVertical();
				
				// Start vertical column
				GUILayout.BeginVertical(UISkin.scrollView);
				{		
					GUILayout.Label("Session Details", kSubTitleStyle);
					
					if (GUILayout.Button("GetAuthToken"))
					{
						string _authToken		= NPBinding.Twitter.GetAuthToken();
						AddNewResult("Auth Token=" + _authToken);
					}
					
					if (GUILayout.Button("GetAuthTokenSecret"))
					{
						string _authTokenSecret	= NPBinding.Twitter.GetAuthTokenSecret();
						AddNewResult("Auth Token Secret=" + _authTokenSecret);
					}
					
					if (GUILayout.Button("GetUserID"))
					{
						string _userID			= NPBinding.Twitter.GetUserID();
						AddNewResult("User ID=" + _userID);
					}
					
					if (GUILayout.Button("GetUserName"))
					{
						string _userName		= NPBinding.Twitter.GetUserName();
						AddNewResult("Username=" + _userName);
					}
				}
				GUILayout.EndVertical();
				
				// Start vertical column
				GUILayout.BeginVertical(UISkin.scrollView);
				{
					GUILayout.Label("Tweet Composer", kSubTitleStyle);

					if (GUILayout.Button("ShowTweetComposerWithMessage"))
					{
						ShowTweetComposerWithMessage();
					}
					
					if (GUILayout.Button("ShowTweetComposerWithLink"))
					{
						ShowTweetComposerWithLink();
					}
					
					if (GUILayout.Button("ShowTweetComposerWithScreenshot"))
					{
						ShowTweetComposerWithScreenshot();
					}
					
					if (GUILayout.Button("ShowTweetComposerWithImage"))
					{
						ShowTweetComposerWithImage();
					}
				}
				GUILayout.EndVertical();
				
				// Start vertical column
				GUILayout.BeginVertical(UISkin.scrollView);
				{
					GUILayout.Label("Account Details", kSubTitleStyle);

					if (GUILayout.Button("RequestAccountDetails"))
					{
						RequestAccountDetails();
					}
					
					if (GUILayout.Button("RequestEmailAccess"))
					{
						RequestEmailAccess();
					}
				}
				GUILayout.EndVertical();
				
				// Start vertical column
				GUILayout.BeginVertical(UISkin.scrollView);
				{
					GUILayout.Label("API Request Access", kSubTitleStyle);
					if (GUILayout.Button("URLRequest"))
					{
						MakeURLRequest();
					}
				}
				GUILayout.EndVertical();
			}
			m_rootScrollView.EndScrollView();
			
			// Draw results
			DrawResults();
			
			// Back button
			DrawPopButton();
		}

		#endregion
	}
}
