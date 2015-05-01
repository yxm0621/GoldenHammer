//----------------------------------------------
//    GoogleFu: Google doc unity sample
//         Copyright © 2013 Litteratus
//----------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GoogleFuSample
{
	public class MonsterFactorySample : MonoBehaviour
	{
		public MonsterSample _monsterPrefab;	// Prefab that represents a Monster
		public ItemSample _itemPrefab;		// Prefab that represents an Item
		CharacterStatsSample _statsDb;		// Example of an Object based Database.
		ItemsSample _itemsDb;					// Example of a Static Database.
		
		// Sample internals
		int _spawnedCount = 0;
		
		// Grab references to both the Object and Static databases
		void Start ()
		{
			// Since our database exists within an object in the scene, we'll need to find it.
			// Alternatively you could expose the database as a public member, and use the
			// inspector to set it.
			GameObject statsdbobj = GameObject.Find("databaseObj");	
			if ( statsdbobj != null )
			{
				// Get the CharacterStats component out of the GameObject.
				// CharacterStats is the name of the worksheet in the google spreadsheet
				//  that we are getting the monster information from
				_statsDb = statsdbobj.GetComponent<CharacterStatsSample>();
			}
			
			// The Items database is a Static class. Use it by grabbing the .Instance, this will
			// ensure the database is correctly initialized. Larger databases may take a while
			// to initialize, so grabbing an instance before the game is updating is recommended.
			_itemsDb = ItemsSample.Instance;
		}

		void OnGUI ()
		{
			// Make a background box
			GUI.Box(new Rect(10,10,110,120), "Spawn Menu");
	
			// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
			if(GUI.Button(new Rect(20,40,90,20), "Goblin")) 
			{
				Spawn((int)CharacterStatsSample.rowIds.AI_GOBLIN);
			}
			
			if(GUI.Button(new Rect(20,70,90,20), "Troll")) 
			{
				Spawn((int)CharacterStatsSample.rowIds.AI_TROLL);
			}
			
			if(GUI.Button(new Rect(20,100,90,20), "Random")) 
			{
				// Grab a random Row from the database
				int randomRow = Random.Range(0, _statsDb.Rows.Count);
				Spawn(randomRow);
			}
			
		}
		
		// Instantiate a random Monster from the database
		// Instantiate a random Item from the database, and attach it to the Monster
		void Spawn (int indexToSpawn)
		{
			if ( _monsterPrefab != null && _statsDb != null )
			{
				// Instantiate the Monster prefab
				MonsterSample myMonster = (MonsterSample)Instantiate( _monsterPrefab );
				
				// Assign the stats from the index to the Monster
				myMonster.MonsterStats = _statsDb.Rows[indexToSpawn];
				
				// Change the name from Monster(clone) to whatever the Monster name is
				myMonster.name = myMonster.MonsterStats._NAME + " " + _spawnedCount.ToString();
				
				// Attach an Item to the monster
				if ( _itemPrefab != null && _itemsDb != null )
				{
					// Instantiate the Item prefab
					ItemSample myItem = (ItemSample)Instantiate( _itemPrefab );
					
					// Grab a random Row from the database
					int randomRow = Random.Range(0, _itemsDb.Rows.Count);
					
					// Assign the stats from the random row to the Item
					myItem.CarriedItem = _itemsDb.Rows[ randomRow ];
					
					// Set the parent to be our new Monster
					myItem.transform.parent = myMonster.transform;
					
					// Change the name from Item(clone) to whatever the Item name is
					myItem.name = myItem.CarriedItem._NAME + " " + _spawnedCount.ToString();
				}
				
				_spawnedCount++;
			}
		}

	}
}
