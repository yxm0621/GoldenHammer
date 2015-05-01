using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace VoxelBusters.DesignPatterns
{
	public class PollingPattern : SingletonPattern <PollingPattern>
	{
		#region Properties

		private List<IPoller>							m_listOfPollers			= new List<IPoller>();		
		private List<IPoller>							m_listOfActivePollers	= new List<IPoller>();
		private Dictionary<string, System.DateTime>		m_lastPollingData		= new Dictionary<string, System.DateTime>();
		 
		// Related to state
		private bool									m_isInitialised			= false;

		#endregion

		#region Unity Methods

		protected override void Awake ()
		{
			base.Awake();

			if (instance != this)
				return;

			m_isInitialised	= true;

			// Load data to previously pollings
			Deserialise();
		}

		protected override void Start ()
		{
			base.Start();

			// Start coroutine which monitors behaviour
			StartCoroutine(Monitor());
		}

		protected override void OnEnable ()
		{
			base.OnEnable();

			// If reenabled already initialised object, then restart coroutine
			if (m_isInitialised)
				StartCoroutine(Monitor());
		}

		protected override void OnDisable ()
		{
			base.OnDisable();

			// Stop all coroutines
			StopAllCoroutines();

			// Serialise
			Serialise();
		}

		#endregion

		#region Exposed Methods

		public void Register (IPoller _newPoller)
		{
			if (!m_listOfPollers.Contains(_newPoller))
				m_listOfPollers.Add(_newPoller);
		}

		#endregion

		#region Poller Handling

		private IEnumerator Monitor ()
		{
			float _dt	= 1f;

			while (true)
			{
				System.DateTime _now	= System.DateTime.UtcNow;

				// Based on eligiblity, pollers are pushed to active list and their task is fired
				FilterOutEligiblePollers(_now);

				// Completed pollers are removed from active list
				CheckActivePollersTaskStatus(_now);

				// Wait
				yield return new WaitForSeconds(_dt);
			}
		}

		private void FilterOutEligiblePollers (System.DateTime _now)
		{
			int _totalPollers	= m_listOfPollers.Count;

			for (int _pIter = 0; _pIter < _totalPollers; _pIter++)
			{
				IPoller _poller		= m_listOfPollers[_pIter];
				string _uniqueID	= _poller.UniqueIdentifier;

				// Check if this poller is already listed in active pollers
				if (m_listOfActivePollers.Contains(_poller))
					continue;

				bool _addToActiveList	= false;

				// Check if this poller ever did poll
				if (!m_lastPollingData.ContainsKey(_uniqueID))
				{
					_addToActiveList	= true;
				}
				// Check if time has crossed interval
				else
				{
					float _elapsedHours	= (float)(_now - m_lastPollingData[_uniqueID]).TotalHours;
					if (_elapsedHours >= ((float)_poller.Interval))
					{
						_addToActiveList	= true;
					}
				}

				// Marked to be added to active list
				if (_addToActiveList)
				{
					FireEvent(_poller, _now);

					// Adding to list
					m_listOfActivePollers.Add(_poller);
				}
			}
		}

		private void FireEvent (IPoller _poller, System.DateTime _now)
		{
			// Poller task is fired and marked task progress as incomplete
			_poller.IsDone	= false;
			_poller.FiredAt	= _now;
			_poller.Fire();
		}

		private void CheckActivePollersTaskStatus (System.DateTime _now)
		{
			int _totalActivePollers	= m_listOfActivePollers.Count;

			for (int _pIter = 0; _pIter < _totalActivePollers; _pIter++)
			{
				IPoller _activePoller	= m_listOfActivePollers[_pIter];

				// Check if task is completed
				if (!_activePoller.IsDone)
				{
					// Allowed to retry
					if (_activePoller.RetryAfter > 0f)
					{
						float _secElapsed	= (float)(_now - _activePoller.FiredAt).TotalSeconds;

						// Re-triggering event "Fire"
						if (_secElapsed > _activePoller.RetryAfter)
						{
							_activePoller.Abort();
							FireEvent(_activePoller, _now);
						}
					}

					continue;
				}

				// Keep track of time, so that next polling will be after 'x' intervals
				m_lastPollingData[_activePoller.UniqueIdentifier]	= System.DateTime.UtcNow;

				// Unset task completed flag
				_activePoller.IsDone	= false;

				// Remove from active list
				m_listOfActivePollers.Remove(_activePoller);
				_pIter--;
				_totalActivePollers--;
			}
		}

		#endregion

		#region Serialisation Methods

		private const string kPlayerPref	= "dp-polling-pattern";

		private void Deserialise ()
		{
			string _serialisedJsonData	= PlayerPrefs.GetString(kPlayerPref, string.Empty);
			
			if (!string.IsNullOrEmpty(_serialisedJsonData))
			{
				// Clear container
				m_lastPollingData.Clear();

				// Splist stored data to get key value pair
				string[] _keyValuePairList	= _serialisedJsonData.Split(',');

				foreach (string _keyValuePair in _keyValuePairList)
				{
					int _colonIndex	= _keyValuePair.IndexOf(':');

					if (_colonIndex != -1)
					{
						int _colonIndexPlus1				= _colonIndex + 1;
						string _key							= _keyValuePair.Substring(0, _colonIndex);
						string _value						= _keyValuePair.Substring(_colonIndexPlus1, _keyValuePair.Length - _colonIndexPlus1);
						System.DateTime _lastPollDateTime	= System.DateTime.SpecifyKind(System.DateTime.Parse(_value), System.DateTimeKind.Utc);

						// Add this value
						m_lastPollingData.Add(_key, _lastPollDateTime);
					}
				}
			}
		}

		private void Serialise ()
		{
			StringBuilder _stringBuilder	= new StringBuilder();
			IList _keyValuePairList			= m_lastPollingData.ToList<KeyValuePair<string, System.DateTime>>();
			int _keyCount					= _keyValuePairList.Count;
			int _keyCountMinus1				= _keyCount - 1;
			int _kIter						= 0;

			foreach (KeyValuePair<string, System.DateTime> _kvPair in _keyValuePairList)
			{
				_stringBuilder.AppendFormat("{0}:{1}", _kvPair.Key, _kvPair.Value);

				if (_kIter < _keyCountMinus1)
					_stringBuilder.Append(',');

				_kIter++;
			}

			// Save
			PlayerPrefs.SetString(kPlayerPref, _stringBuilder.ToString());
			PlayerPrefs.Save();
		}

		#endregion
	}
}