using UnityEngine;
using System.Collections;

#if UNITY_IOS
namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class AddressBookIOS : AddressBook 
	{
		private enum iOSABAuthorizationStatus
		{
			kABAuthorizationStatusNotDetermined = 0,
			kABAuthorizationStatusRestricted,
			kABAuthorizationStatusDenied,
			kABAuthorizationStatusAuthorized
		};

		#region Parsing Methods

		protected override void ParseContactData (IDictionary _contactInfoDict, out AddressBookContact _contact)
		{
			_contact	= new iOSAddressBookContact(_contactInfoDict);
		}

		protected override void ParseAuthorizationStatusData (string _statusStr, out eABAuthorizationStatus _authStatus)
		{
			iOSABAuthorizationStatus _iOSAuthStatus	= ((iOSABAuthorizationStatus)int.Parse(_statusStr));

			if (_iOSAuthStatus == iOSABAuthorizationStatus.kABAuthorizationStatusAuthorized)
			{
				_authStatus	= eABAuthorizationStatus.AUTHORIZED;
			}
			else if (_iOSAuthStatus == iOSABAuthorizationStatus.kABAuthorizationStatusDenied)
			{
				_authStatus	= eABAuthorizationStatus.DENIED;
			}
			else
			{
				_authStatus	= eABAuthorizationStatus.RESTRICTED;
			}
		}

		#endregion
	}
}
#endif