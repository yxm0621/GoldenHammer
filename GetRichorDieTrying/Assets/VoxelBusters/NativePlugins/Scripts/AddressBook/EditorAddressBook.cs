using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	public class EditorAddressBook : AdvancedScriptableObject <EditorAddressBook>
	{
		#region Properties

		[SerializeField]
		private AddressBookContact[] 	m_contactsList					= new AddressBookContact[0];

		#endregion

		#region Constants

		// Event callbacks
		private const string			kABReadContactsFinishedEvent	= "ABReadContactsFinished";

		#endregion

		#region Static Methods

		public static void ReadContacts ()
		{
			AddressBookContact[] _contactsList		= Instance.m_contactsList;
			int _totalContacts						= _contactsList.Length;
			AddressBookContact[] _contactsListCopy	= new AddressBookContact[_totalContacts];

			for (int _iter = 0; _iter < _totalContacts; _iter++)
			{
				_contactsListCopy[_iter]	= new EditorAddressBookContact(_contactsList[_iter]);
			}

			// Callback is sent to binding event listener
			if (NPBinding.AddressBook != null)
				NPBinding.AddressBook.InvokeMethod(kABReadContactsFinishedEvent, _contactsListCopy);
		}

		#endregion
	}
}