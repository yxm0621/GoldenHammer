﻿using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

#if UNITY_IOS
namespace VoxelBusters.NativePlugins
{	
	public partial class AddressBookIOS : AddressBook 
	{		
		#region Native Methods

		[DllImport("__Internal")]
		private static extern void readContacts ();

		#endregion
		
		#region Overriden API's

		public override void ReadContacts (ReadContactsCompletion _onCompletion)
		{
			base.ReadContacts(_onCompletion);

			// Native method is called
			readContacts();
		}

		#endregion
	}
}
#endif