using UnityEngine;
using System.Collections;

namespace VoxelBusters.Product
{
	public interface IProductDashboard 
	{
		#region Properties

		/// <summary>
		/// Gets the name of the product.
		/// </summary>
		/// <value>The name of the product.</value>
		string 			ProductName
		{
			get;
		}

		/// <summary>
		/// Gets the product version.
		/// </summary>
		/// <value>The product version.</value>
		string 			ProductVersion
		{
			get;
		}

		Texture2D		LogoTexture
		{
			get;
		}

		#endregion

		#region Methods

#if UNITY_EDITOR

		/// <summary>
		/// User requested to check for updates.
		/// </summary>
		void CheckForUpdates (bool isAutoCheck);

		/// <summary>
		/// Automatic check for updates.
		/// </summary>
		void AutoCheckForUpdates ();

#endif
		#endregion
	}
}
