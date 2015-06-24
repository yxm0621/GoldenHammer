//----------------------------------------------
//    Google2u: Google Doc Unity integration
//         Copyright © 2015 Litteratus
//
//        This file has been auto-generated
//              Do not manually edit
//----------------------------------------------

using UnityEngine;

namespace Google2u
{
	[System.Serializable]
	public class ObstacleDesignRow : IGoogle2uRow
	{
		public string _Name;
		public string _Behaviors;
		public string _Trigger;
		public System.Collections.Generic.List<int> _Lane = new System.Collections.Generic.List<int>();
		public int _Gold;
		public int _HitPoints;
		public string _Category;
		public float _Occurance;
		public string _Size;
		public string _Encyclopedia;
		public ObstacleDesignRow(string __G2U_ID, string __Name, string __Behaviors, string __Trigger, string __Lane, string __Gold, string __HitPoints, string __Category, string __Occurance, string __Size, string __Encyclopedia) 
		{
			_Name = __Name.Trim();
			_Behaviors = __Behaviors.Trim();
			_Trigger = __Trigger.Trim();
			{
				int res;
				string []result = __Lane.Split(",".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
				for(int i = 0; i < result.Length; i++)
				{
					if(int.TryParse(result[i], out res))
						_Lane.Add( res );
					else
					{
						_Lane.Add( 0 );
						Debug.LogError("Failed To Convert _Lane string: "+ result[i] +" to int");
					}
				}
			}
			{
			int res;
				if(int.TryParse(__Gold, out res))
					_Gold = res;
				else
					Debug.LogError("Failed To Convert _Gold string: "+ __Gold +" to int");
			}
			{
			int res;
				if(int.TryParse(__HitPoints, out res))
					_HitPoints = res;
				else
					Debug.LogError("Failed To Convert _HitPoints string: "+ __HitPoints +" to int");
			}
			_Category = __Category.Trim();
			{
			float res;
				if(float.TryParse(__Occurance, out res))
					_Occurance = res;
				else
					Debug.LogError("Failed To Convert _Occurance string: "+ __Occurance +" to float");
			}
			_Size = __Size.Trim();
			_Encyclopedia = __Encyclopedia.Trim();
		}

		public int Length { get { return 10; } }

		public string this[int i]
		{
		    get
		    {
		        return GetStringDataByIndex(i);
		    }
		}

		public string GetStringDataByIndex( int index )
		{
			string ret = System.String.Empty;
			switch( index )
			{
				case 0:
					ret = _Name.ToString();
					break;
				case 1:
					ret = _Behaviors.ToString();
					break;
				case 2:
					ret = _Trigger.ToString();
					break;
				case 3:
					ret = _Lane.ToString();
					break;
				case 4:
					ret = _Gold.ToString();
					break;
				case 5:
					ret = _HitPoints.ToString();
					break;
				case 6:
					ret = _Category.ToString();
					break;
				case 7:
					ret = _Occurance.ToString();
					break;
				case 8:
					ret = _Size.ToString();
					break;
				case 9:
					ret = _Encyclopedia.ToString();
					break;
			}

			return ret;
		}

		public string GetStringData( string colID )
		{
			var ret = System.String.Empty;
			switch( colID.ToLower() )
			{
				case "Name":
					ret = _Name.ToString();
					break;
				case "Behaviors":
					ret = _Behaviors.ToString();
					break;
				case "Trigger":
					ret = _Trigger.ToString();
					break;
				case "Lane":
					ret = _Lane.ToString();
					break;
				case "Gold":
					ret = _Gold.ToString();
					break;
				case "HitPoints":
					ret = _HitPoints.ToString();
					break;
				case "Category":
					ret = _Category.ToString();
					break;
				case "Occurance":
					ret = _Occurance.ToString();
					break;
				case "Size":
					ret = _Size.ToString();
					break;
				case "Encyclopedia":
					ret = _Encyclopedia.ToString();
					break;
			}

			return ret;
		}
		public override string ToString()
		{
			string ret = System.String.Empty;
			ret += "{" + "Name" + " : " + _Name.ToString() + "} ";
			ret += "{" + "Behaviors" + " : " + _Behaviors.ToString() + "} ";
			ret += "{" + "Trigger" + " : " + _Trigger.ToString() + "} ";
			ret += "{" + "Lane" + " : " + _Lane.ToString() + "} ";
			ret += "{" + "Gold" + " : " + _Gold.ToString() + "} ";
			ret += "{" + "HitPoints" + " : " + _HitPoints.ToString() + "} ";
			ret += "{" + "Category" + " : " + _Category.ToString() + "} ";
			ret += "{" + "Occurance" + " : " + _Occurance.ToString() + "} ";
			ret += "{" + "Size" + " : " + _Size.ToString() + "} ";
			ret += "{" + "Encyclopedia" + " : " + _Encyclopedia.ToString() + "} ";
			return ret;
		}
	}
	public sealed class ObstacleDesign : IGoogle2uDB
	{
		public enum rowIds {
			Obstacle_Lightning, Obstacle_SafetyCones, Obstacle_City_Man, Obstacle_City_Woman, Special_Police_Officer, Special_Police_Car, Rare_Flowerpot, Rare_Seeds, Special_Soldier, Special_Tank, Rare_Airplane, Obstacle_Banana_Peel, Common_Soda_Machine, UnCommon_Arcade, Common_Balloon, Common_Telephone_Pole, Common_Checkpoint, Obstacle_Cat
			, Obstacle_Dog, Obstacle_Squirrell, Obstacle_Bird, Obstacle_Sheep, Obstacle_Cow, Common_Chicken, Common_Tractor, Common_Silo, Common_Farmhouse, Common_Farmer_Woman, Common_Farmer_Man, Rare_Tsuchinoko, Common_Fence, Common_Windmill, Common_Haystack, Common_Fruit_Tree, Common_Corn, Obstacle_Wolf, UnCommon_Mushroom, Common_Crow
			, Common_Seaweed, Common_Clam, Rare_Oyster, Common_Coral, Rare_Whale, Obstacle_Electronic_Eel, Common_Diver, Rare_Kappa, Common_FishHook, Rare_Wrecked_Ship, Rare_Treasure_Chest, Common_Giant_Anchor, Obstacle_Octopus, Common_PufferFish, Common_Dolphin, Obstacle_Shark, Boss_Ebisu, Boss_Ebisu_Hook, Obstacle_Flying_Fish, Obstacle_Crab
			, Rare_Nemo, Obstacle_Asterorid, Special_Mercury, Special_Venus, Special_Mars, Special_Saturn, Special_Jupiter, Special_Neptune, Special_Uranus, Special_Pluto, Rare_UFO, Rare_Space_Baby, Common_Stars, Obstacle_Fox, Obstacle_Deer, Common_Wild_Tree, Common_Boulder, Rare_Tengu, Common_Cave, Common_Monkey
			, Obstacle_Titan
		};
		public string [] rowNames = {
			"Obstacle_Lightning", "Obstacle_SafetyCones", "Obstacle_City_Man", "Obstacle_City_Woman", "Special_Police_Officer", "Special_Police_Car", "Rare_Flowerpot", "Rare_Seeds", "Special_Soldier", "Special_Tank", "Rare_Airplane", "Obstacle_Banana_Peel", "Common_Soda_Machine", "UnCommon_Arcade", "Common_Balloon", "Common_Telephone_Pole", "Common_Checkpoint", "Obstacle_Cat"
			, "Obstacle_Dog", "Obstacle_Squirrell", "Obstacle_Bird", "Obstacle_Sheep", "Obstacle_Cow", "Common_Chicken", "Common_Tractor", "Common_Silo", "Common_Farmhouse", "Common_Farmer_Woman", "Common_Farmer_Man", "Rare_Tsuchinoko", "Common_Fence", "Common_Windmill", "Common_Haystack", "Common_Fruit_Tree", "Common_Corn", "Obstacle_Wolf", "UnCommon_Mushroom", "Common_Crow"
			, "Common_Seaweed", "Common_Clam", "Rare_Oyster", "Common_Coral", "Rare_Whale", "Obstacle_Electronic_Eel", "Common_Diver", "Rare_Kappa", "Common_FishHook", "Rare_Wrecked_Ship", "Rare_Treasure_Chest", "Common_Giant_Anchor", "Obstacle_Octopus", "Common_PufferFish", "Common_Dolphin", "Obstacle_Shark", "Boss_Ebisu", "Boss_Ebisu_Hook", "Obstacle_Flying_Fish", "Obstacle_Crab"
			, "Rare_Nemo", "Obstacle_Asterorid", "Special_Mercury", "Special_Venus", "Special_Mars", "Special_Saturn", "Special_Jupiter", "Special_Neptune", "Special_Uranus", "Special_Pluto", "Rare_UFO", "Rare_Space_Baby", "Common_Stars", "Obstacle_Fox", "Obstacle_Deer", "Common_Wild_Tree", "Common_Boulder", "Rare_Tengu", "Common_Cave", "Common_Monkey"
			, "Obstacle_Titan"
		};
		public System.Collections.Generic.List<ObstacleDesignRow> Rows = new System.Collections.Generic.List<ObstacleDesignRow>();

		public static ObstacleDesign Instance
		{
			get { return NestedObstacleDesign.instance; }
		}

		private class NestedObstacleDesign
		{
			static NestedObstacleDesign() { }
			internal static readonly ObstacleDesign instance = new ObstacleDesign();
		}

		private ObstacleDesign()
		{
			Rows.Add( new ObstacleDesignRow("Obstacle_Lightning", "Lightning", "Can destroy any obstacle hit", "During Rain", "", "3", "1", "Obstacle", "0.45", "", ""));
			Rows.Add( new ObstacleDesignRow("Obstacle_SafetyCones", "SafetyCones", "N/A", "N/A", "", "1", "1", "Obstacle", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Obstacle_City_Man", "City_Man", "Walk down Sidewalk toward screen", "N/A", "1,4", "15", "2", "Obstacle", "0.85", "", ""));
			Rows.Add( new ObstacleDesignRow("Obstacle_City_Woman", "City_Woman", "Walk down Sidewalk toward screen", "N/A", "", "15", "2", "Obstacle", "0.85", "", ""));
			Rows.Add( new ObstacleDesignRow("Special_Police_Officer", "Police_Officer", "Walk toward Player to arrest", "N/A", "", "35", "3", "Special", "0.35", "", ""));
			Rows.Add( new ObstacleDesignRow("Special_Police_Car", "Police_Car", "Block Street in front of Player", "Smash 3 people or one police officer", "", "25", "5", "Special", "0.00", "", ""));
			Rows.Add( new ObstacleDesignRow("Rare_Flowerpot", "Flowerpot", "", "N/A", "", "45", "1", "Rare", "0.20", "", ""));
			Rows.Add( new ObstacleDesignRow("Rare_Seeds", "Seeds", "", "N/A", "", "35", "1", "Rare", "0.20", "", ""));
			Rows.Add( new ObstacleDesignRow("Special_Soldier", "Soldier", "Cover each walkable block and move toward screen", "Smash Tank or summon Bishamonten", "", "50", "5", "Special", "0.00", "", ""));
			Rows.Add( new ObstacleDesignRow("Special_Tank", "Tank", "", "Smash Police Car or summon Bishamonten", "", "500", "20", "Special", "0.00", "", ""));
			Rows.Add( new ObstacleDesignRow("Rare_Airplane", "Airplane", "", "N/A", "", "50", "3", "Rare", "0.45", "", ""));
			Rows.Add( new ObstacleDesignRow("Obstacle_Banana_Peel", "Banana_Peel", "", "N/A", "", "2", "1", "Obstacle", "0.75", "", ""));
			Rows.Add( new ObstacleDesignRow("Common_Soda_Machine", "Soda_Machine", "", "N/A", "", "3", "1", "Common", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("UnCommon_Arcade", "Arcade", "", "N/A", "", "80", "1", "UnCommon", "0.35", "", ""));
			Rows.Add( new ObstacleDesignRow("Common_Balloon", "Balloon", "", "N/A", "", "1", "1", "Common", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Common_Telephone_Pole", "Telephone_Pole", "", "N/A", "", "3", "1", "Common", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Common_Checkpoint", "Checkpoint", "", "Level End", "", "3", "500", "Common", "0.00", "", ""));
			Rows.Add( new ObstacleDesignRow("Obstacle_Cat", "Cat", "", "N/A", "", "2", "1", "Obstacle", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Obstacle_Dog", "Dog", "", "N/A", "", "2", "1", "Obstacle", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Obstacle_Squirrell", "Squirrell", "", "N/A", "", "2", "1", "Obstacle", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Obstacle_Bird", "Bird", "", "N/A", "", "2", "1", "Obstacle", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Obstacle_Sheep", "Sheep", "", "N/A", "", "3", "1", "Obstacle", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Obstacle_Cow", "Cow", "", "N/A", "", "3", "1", "Obstacle", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Common_Chicken", "Chicken", "", "N/A", "", "2", "1", "Common", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Common_Tractor", "Tractor", "", "N/A", "", "5", "1", "Common", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Common_Silo", "Silo", "", "N/A", "", "20", "2", "Common", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Common_Farmhouse", "Farmhouse", "", "N/A", "", "20", "2", "Common", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Common_Farmer_Woman", "Farmer_Woman", "", "N/A", "", "15", "2", "Common", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Common_Farmer_Man", "Farmer_Man", "", "N/A", "", "15", "2", "Common", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Rare_Tsuchinoko", "Tsuchinoko", "", "N/A", "", "200", "1", "Rare", "0.20", "", ""));
			Rows.Add( new ObstacleDesignRow("Common_Fence", "Fence", "", "N/A", "", "5", "1", "Common", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Common_Windmill", "Windmill", "", "N/A", "", "20", "2", "Common", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Common_Haystack", "Haystack", "", "N/A", "", "1", "2", "Common", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Common_Fruit_Tree", "Fruit_Tree", "", "N/A", "", "3", "1", "Common", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Common_Corn", "Corn", "", "N/A", "", "2", "1", "Common", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Obstacle_Wolf", "Wolf", "", "N/A", "", "2", "1", "Obstacle", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("UnCommon_Mushroom", "Mushroom", "", "N/A", "", "2", "1", "UnCommon", "0.35", "", ""));
			Rows.Add( new ObstacleDesignRow("Common_Crow", "Crow", "", "N/A", "", "2", "1", "Common", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Common_Seaweed", "Seaweed", "", "N/A", "", "1", "1", "Common", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Common_Clam", "Clam", "", "N/A", "", "2", "1", "Common", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Rare_Oyster", "Oyster", "", "N/A", "", "900", "1", "Rare", "0.20", "", ""));
			Rows.Add( new ObstacleDesignRow("Common_Coral", "Coral", "", "N/A", "", "2", "1", "Common", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Rare_Whale", "Whale", "", "N/A", "", "250", "5", "Rare", "0.45", "", ""));
			Rows.Add( new ObstacleDesignRow("Obstacle_Electronic_Eel", "Electronic_Eel", "", "N/A", "", "2", "1", "Obstacle", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Common_Diver", "Diver", "", "N/A", "", "5", "2", "Common", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Rare_Kappa", "Kappa", "", "N/A", "", "500", "2", "Rare", "0.20", "", ""));
			Rows.Add( new ObstacleDesignRow("Common_FishHook", "FishHook", "", "N/A", "", "1", "1", "Common", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Rare_Wrecked_Ship", "Wrecked_Ship", "", "N/A", "", "350", "7", "Rare", "0.25", "", ""));
			Rows.Add( new ObstacleDesignRow("Rare_Treasure_Chest", "Treasure_Chest", "", "N/A", "", "9000", "1", "Rare", "0.15", "", ""));
			Rows.Add( new ObstacleDesignRow("Common_Giant_Anchor", "Giant_Anchor", "", "N/A", "", "100", "5", "Common", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Obstacle_Octopus", "Octopus", "", "N/A", "", "0", "1", "Obstacle", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Common_PufferFish", "PufferFish", "", "N/A", "", "0", "1", "Common", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Common_Dolphin", "Dolphin", "", "N/A", "", "0", "1", "Common", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Obstacle_Shark", "Shark", "", "N/A", "", "0", "1", "Obstacle", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Boss_Ebisu", "Ebisu", "", "In Ocean", "", "0", "50", "Boss", "0.75", "", ""));
			Rows.Add( new ObstacleDesignRow("Boss_Ebisu_Hook", "Ebisu_Hook", "", "In Ocean", "", "0", "1", "Boss", "0.75", "", ""));
			Rows.Add( new ObstacleDesignRow("Obstacle_Flying_Fish", "Flying_Fish", "", "N/A", "", "0", "1", "Obstacle", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Obstacle_Crab", "Crab", "", "N/A", "", "0", "2", "Obstacle", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Rare_Nemo", "Nemo", "", "N/A", "", "0", "1", "Rare", "0.20", "", ""));
			Rows.Add( new ObstacleDesignRow("Obstacle_Asterorid", "Asterorid", "", "In Space", "", "0", "4", "Obstacle", "0.90", "", ""));
			Rows.Add( new ObstacleDesignRow("Special_Mercury", "Mercury", "", "In Space", "", "0", "3", "Special", "0.90", "", ""));
			Rows.Add( new ObstacleDesignRow("Special_Venus", "Venus", "", "In Space", "", "0", "4", "Special", "0.90", "", ""));
			Rows.Add( new ObstacleDesignRow("Special_Mars", "Mars", "", "In Space", "", "0", "4", "Special", "0.90", "", ""));
			Rows.Add( new ObstacleDesignRow("Special_Saturn", "Saturn", "", "In Space", "", "0", "5", "Special", "0.90", "", ""));
			Rows.Add( new ObstacleDesignRow("Special_Jupiter", "Jupiter", "", "In Space", "", "0", "8", "Special", "0.90", "", ""));
			Rows.Add( new ObstacleDesignRow("Special_Neptune", "Neptune", "", "In Space", "", "0", "4", "Special", "0.90", "", ""));
			Rows.Add( new ObstacleDesignRow("Special_Uranus", "Uranus", "", "In Space", "", "0", "4", "Special", "0.90", "", ""));
			Rows.Add( new ObstacleDesignRow("Special_Pluto", "Pluto", "", "In Space", "", "0", "2", "Special", "0.90", "", ""));
			Rows.Add( new ObstacleDesignRow("Rare_UFO", "UFO", "", "In Space", "", "0", "3", "Rare", "0.20", "", ""));
			Rows.Add( new ObstacleDesignRow("Rare_Space_Baby", "Space_Baby", "", "In Space", "", "0", "1", "Rare", "0.15", "", ""));
			Rows.Add( new ObstacleDesignRow("Common_Stars", "Stars", "", "In Space", "", "0", "1", "Common", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Obstacle_Fox", "Fox", "", "N/A", "", "0", "1", "Obstacle", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Obstacle_Deer", "Deer", "", "N/A", "", "0", "1", "Obstacle", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Common_Wild_Tree", "Wild_Tree", "", "N/A", "", "0", "1", "Common", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Common_Boulder", "Boulder", "", "N/A", "", "0", "3", "Common", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Rare_Tengu", "Tengu", "", "N/A", "", "0", "1", "Rare", "0.20", "", ""));
			Rows.Add( new ObstacleDesignRow("Common_Cave", "Cave", "", "N/A", "", "0", "2", "Common", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Common_Monkey", "Monkey", "", "N/A", "", "0", "1", "Common", "0.95", "", ""));
			Rows.Add( new ObstacleDesignRow("Obstacle_Titan", "Titan", "", "N/A", "", "0", "5", "Obstacle", "0.25", "", ""));
		}
		public IGoogle2uRow GetGenRow(string in_RowString)
		{
			IGoogle2uRow ret = null;
			try
			{
				ret = Rows[(int)System.Enum.Parse(typeof(rowIds), in_RowString)];
			}
			catch(System.ArgumentException) {
				Debug.LogError( in_RowString + " is not a member of the rowIds enumeration.");
			}
			return ret;
		}
		public IGoogle2uRow GetGenRow(rowIds in_RowID)
		{
			IGoogle2uRow ret = null;
			try
			{
				ret = Rows[(int)in_RowID];
			}
			catch( System.Collections.Generic.KeyNotFoundException ex )
			{
				Debug.LogError( in_RowID + " not found: " + ex.Message );
			}
			return ret;
		}
		public ObstacleDesignRow GetRow(rowIds in_RowID)
		{
			ObstacleDesignRow ret = null;
			try
			{
				ret = Rows[(int)in_RowID];
			}
			catch( System.Collections.Generic.KeyNotFoundException ex )
			{
				Debug.LogError( in_RowID + " not found: " + ex.Message );
			}
			return ret;
		}
		public ObstacleDesignRow GetRow(string in_RowString)
		{
			ObstacleDesignRow ret = null;
			try
			{
				ret = Rows[(int)System.Enum.Parse(typeof(rowIds), in_RowString)];
			}
			catch(System.ArgumentException) {
				Debug.LogError( in_RowString + " is not a member of the rowIds enumeration.");
			}
			return ret;
		}

	}

}
