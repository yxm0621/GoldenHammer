using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{
	/// <summary>
	/// Notification properties specifically used only in Android platform.
	/// </summary>
	public partial class CrossPlatformNotification 
	{
		public class AndroidSpecificProperties
		{
			#region Properties

			/// <summary>
			/// Gets or sets the content title value. This is the text that is displayed in the notification bar on Android as title of the notification.
			/// </summary>
			/// <value>The content title used in notification bar.</value>
			public string			ContentTitle
			{
				get; 
				set;
			}

			/// <summary>
			/// Gets or sets the ticker text. The text that will be visible  in a scrolling fashion on status bar. \note Pre-Lollipop devices has this feature.
			/// </summary>
			/// <value>The ticker text which scrolls in status bar.</value>
			public string			TickerText
			{
				get; 
				set;
			}

			/// <summary>
			/// Gets or sets the tag. If the tag is set with diff value than previous notification, it won't override the previous one in notification bar. Else it will.
			/// </summary>
			/// <value>Tag which can define this notification uniquely or can be empty which overwrites previous notification.</value>
			public string			Tag
			{
				get; 
				set;
			}
			
			#endregion

			#region Constants

			private const string 	kContentTitleKey	= "content-title";
			private const string 	kTickerTextKey		= "ticker-text";
			private const string 	kTagKey				= "tag";

			#endregion

			#region Constructors

			public AndroidSpecificProperties ()
			{
				ContentTitle	= null;
				TickerText		= null;
				Tag				= null;
			}

			internal AndroidSpecificProperties (IDictionary _jsonDict)
			{
				ContentTitle	= _jsonDict.GetIfAvailable<string>(kContentTitleKey);
				TickerText		= _jsonDict.GetIfAvailable<string>(kTickerTextKey);
				Tag				= _jsonDict.GetIfAvailable<string>(kTagKey);
			}

			#endregion

			#region Methods

			internal IDictionary JSONObject ()
			{
				Dictionary<string, string> _jsonDict	= new Dictionary<string, string>();
				_jsonDict[kContentTitleKey]				= ContentTitle;
				_jsonDict[kTickerTextKey]				= TickerText;
				_jsonDict[kTagKey]						= Tag;

				return _jsonDict;
			}

			/// <summary>
			/// String representation of <see cref="CrossPlatformNotification+AndroidSpecificProperties"/>.
			/// </summary>
			public override string ToString ()
			{
				return string.Format ("[AndroidSpecificProperties: ContentTitle={0}, TickerText={1}, Tag = {2}]", 
				                      ContentTitle, TickerText, Tag);
			}

			#endregion
		}
	}
}
