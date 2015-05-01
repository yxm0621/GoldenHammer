using UnityEngine;
using System.Collections;

namespace VoxelBusters.DesignPatterns
{
	public interface IPoller
	{
		#region Properties

		/// <summary>
		/// Value is used to uniquely identify poller.
		/// </summary>
		/// <value>The unique identifier.</value>
		string					UniqueIdentifier
		{
			get;
			set;
		}

		/// <summary>
		/// Poller task gets fired at equal interval of time (value is in hours)
		/// </summary>
		/// <value>The interval.</value>
		int						Interval
		{
			get;
			set;
		}

		/// <summary>
		/// If value is non-negative then "Fire" is retriggered after x seconds
		/// </summary>
		/// <value>The retry after.</value>
		float					RetryAfter
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets when "Fire" event was called
		/// </summary>
		/// <value>The fired at.</value>
		System.DateTime			FiredAt
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a value indicating whether task is completed or not.
		/// </summary>
		/// <value><c>true</c> if task is completed; otherwise, <c>false</c>.</value>
		bool					IsDone
		{
			get;
			set;
		}

		#endregion

		#region Task Handling Methods

		/// <summary>
		/// Define the task to be done. Gets triggered at equal interval of time.
		/// </summary>
		void Fire ();

		/// <summary>
		/// Task can be declared void
		/// </summary>
		void Abort ();

		#endregion
	}
}