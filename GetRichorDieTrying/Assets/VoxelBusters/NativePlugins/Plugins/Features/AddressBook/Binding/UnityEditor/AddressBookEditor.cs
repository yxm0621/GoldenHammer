using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
namespace VoxelBusters.NativePlugins
{	
	using Internal;

	public partial class AddressBookEditor : AddressBook 
	{		
		#region Overriden API's

		public override void ReadContacts (ReadContactsCompletion _onCompletion)
		{
			base.ReadContacts(_onCompletion);

			// Requesting for contacts info
			EditorAddressBook.ReadContacts();
		}

		#endregion
	}
}
#endif