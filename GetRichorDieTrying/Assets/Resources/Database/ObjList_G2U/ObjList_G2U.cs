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
	public class ObjList_G2URow : IGoogle2uRow
	{
		public string _Name;
		public string _Location;
		public string _Behaviors;
		public string _Trigger;
		public int _Gold;
		public int _HitPoints;
		public string _Category;
		public float _Occurance;
		public int _Size;
		public string _Encyclopedia;
		public ObjList_G2URow(string __G2U_ID, string __Name, string __Location, string __Behaviors, string __Trigger, string __Gold, string __HitPoints, string __Category, string __Occurance, string __Size, string __Encyclopedia) 
		{
			_Name = __Name.Trim();
			_Location = __Location.Trim();
			_Behaviors = __Behaviors.Trim();
			_Trigger = __Trigger.Trim();
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
			{
			int res;
				if(int.TryParse(__Size, out res))
					_Size = res;
				else
					Debug.LogError("Failed To Convert _Size string: "+ __Size +" to int");
			}
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
					ret = _Location.ToString();
					break;
				case 2:
					ret = _Behaviors.ToString();
					break;
				case 3:
					ret = _Trigger.ToString();
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
				case "Location":
					ret = _Location.ToString();
					break;
				case "Behaviors":
					ret = _Behaviors.ToString();
					break;
				case "Trigger":
					ret = _Trigger.ToString();
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
			ret += "{" + "Location" + " : " + _Location.ToString() + "} ";
			ret += "{" + "Behaviors" + " : " + _Behaviors.ToString() + "} ";
			ret += "{" + "Trigger" + " : " + _Trigger.ToString() + "} ";
			ret += "{" + "Gold" + " : " + _Gold.ToString() + "} ";
			ret += "{" + "HitPoints" + " : " + _HitPoints.ToString() + "} ";
			ret += "{" + "Category" + " : " + _Category.ToString() + "} ";
			ret += "{" + "Occurance" + " : " + _Occurance.ToString() + "} ";
			ret += "{" + "Size" + " : " + _Size.ToString() + "} ";
			ret += "{" + "Encyclopedia" + " : " + _Encyclopedia.ToString() + "} ";
			return ret;
		}
	}
	public sealed class ObjList_G2U : IGoogle2uDB
	{
		public enum rowIds {
			Boss_Raijin, Obstacle_Lightning, Boss_Fujin, Static_Cloud, Static_RainClouds, Static_Sun, Static_Moon, Boss_Bishamonten, Common_City_Road, Common_City_Tree_1, Common_City_Tree_2, Common_Streetlight, Common_FireHydrant, Common_Building_1, Common_Building_2, Common_Building_3, Common_Long_Building_1, Common_Long_Building_2
			, Common_Long_Building_3, Common_Large_Building_1, Common_Large_Building_2, Common_Large_Building_3, Rare_SkyScraper, Obstacle_SafetyCones, Obstacle_City_Man, Obstacle_City_Woman, Special_Police_Officer, Special_Police_Car, Rare_Flowerpot, Rare_Seeds, Special_Soldier, Special_Tank, Rare_Airplane, Obstacle_Banana_Peel, Common_Soda_Machine, UnCommon_Arcade, Common_Balloon, Common_Telephone_Pole
			, Common_Checkpoint, Obstacle_Cat, Obstacle_White_Cat, Obstacle_Fat_Cat, Obstacle_Fat_Dog, Obstacle_Dog, Obstacle_Squirrell, Obstacle_Bird, Obstacle_Sheep, Common_Shepherd, Obstacle_Cow, Common_Chicken, Common_Tractor, Common_Silo, Common_Farmhouse, Common_Farmer_Woman, Common_Farmer_Man, Rare_Tsuchinoko, Common_Fence, Common_Windmill
			, Common_Haystack, Common_Fruit_Tree, Common_Corn, Obstacle_Wolf, UnCommon_Mushroom, Common_Crow, Common_Seaweed, Common_Clam, Rare_Oyster, Common_Coral, Rare_Whale, Obstacle_Electronic_Eel, Common_Diver, Rare_Kappa, Common_FishHook, Rare_Wrecked_Ship, Rare_Treasure_Chest, Common_Giant_Anchor, Obstacle_Octopus, Common_PufferFish
			, Common_Dolphin, Obstacle_Shark, Boss_Ebisu, Boss_Ebisu_Hook, Obstacle_Flying_Fish, Obstacle_Crab, Rare_Nemo, Obstacle_Asterorid, Special_Mercury, Special_Venus, Special_Mars, Special_Saturn, Special_Jupiter, Special_Neptune, Special_Uranus, Special_Pluto, Rare_UFO, Rare_Space_Baby, Common_Stars, Obstacle_Fox
			, Obstacle_Deer, Common_Wild_Tree, Common_Boulder, Rare_Tengu, Common_Cave, Common_Monkey, Obstacle_Titan
		};
		public string [] rowNames = {
			"Boss_Raijin", "Obstacle_Lightning", "Boss_Fujin", "Static_Cloud", "Static_RainClouds", "Static_Sun", "Static_Moon", "Boss_Bishamonten", "Common_City_Road", "Common_City_Tree_1", "Common_City_Tree_2", "Common_Streetlight", "Common_FireHydrant", "Common_Building_1", "Common_Building_2", "Common_Building_3", "Common_Long_Building_1", "Common_Long_Building_2"
			, "Common_Long_Building_3", "Common_Large_Building_1", "Common_Large_Building_2", "Common_Large_Building_3", "Rare_SkyScraper", "Obstacle_SafetyCones", "Obstacle_City_Man", "Obstacle_City_Woman", "Special_Police_Officer", "Special_Police_Car", "Rare_Flowerpot", "Rare_Seeds", "Special_Soldier", "Special_Tank", "Rare_Airplane", "Obstacle_Banana_Peel", "Common_Soda_Machine", "UnCommon_Arcade", "Common_Balloon", "Common_Telephone_Pole"
			, "Common_Checkpoint", "Obstacle_Cat", "Obstacle_White_Cat", "Obstacle_Fat_Cat", "Obstacle_Fat_Dog", "Obstacle_Dog", "Obstacle_Squirrell", "Obstacle_Bird", "Obstacle_Sheep", "Common_Shepherd", "Obstacle_Cow", "Common_Chicken", "Common_Tractor", "Common_Silo", "Common_Farmhouse", "Common_Farmer_Woman", "Common_Farmer_Man", "Rare_Tsuchinoko", "Common_Fence", "Common_Windmill"
			, "Common_Haystack", "Common_Fruit_Tree", "Common_Corn", "Obstacle_Wolf", "UnCommon_Mushroom", "Common_Crow", "Common_Seaweed", "Common_Clam", "Rare_Oyster", "Common_Coral", "Rare_Whale", "Obstacle_Electronic_Eel", "Common_Diver", "Rare_Kappa", "Common_FishHook", "Rare_Wrecked_Ship", "Rare_Treasure_Chest", "Common_Giant_Anchor", "Obstacle_Octopus", "Common_PufferFish"
			, "Common_Dolphin", "Obstacle_Shark", "Boss_Ebisu", "Boss_Ebisu_Hook", "Obstacle_Flying_Fish", "Obstacle_Crab", "Rare_Nemo", "Obstacle_Asterorid", "Special_Mercury", "Special_Venus", "Special_Mars", "Special_Saturn", "Special_Jupiter", "Special_Neptune", "Special_Uranus", "Special_Pluto", "Rare_UFO", "Rare_Space_Baby", "Common_Stars", "Obstacle_Fox"
			, "Obstacle_Deer", "Common_Wild_Tree", "Common_Boulder", "Rare_Tengu", "Common_Cave", "Common_Monkey", "Obstacle_Titan"
		};
		public System.Collections.Generic.List<ObjList_G2URow> Rows = new System.Collections.Generic.List<ObjList_G2URow>();

		public static ObjList_G2U Instance
		{
			get { return NestedObjList_G2U.instance; }
		}

		private class NestedObjList_G2U
		{
			static NestedObjList_G2U() { }
			internal static readonly ObjList_G2U instance = new ObjList_G2U();
		}

		private ObjList_G2U()
		{
			Rows.Add( new ObjList_G2URow("Boss_Raijin", "Raijin", "Rain", "Appears every 5 seconds during rain and throws lighting bolts", "Appears During Rain", "5000", "100", "Boss", "0.00", "0", "Appears every 5 seconds during rain and throws lighting bolts"));
			Rows.Add( new ObjList_G2URow("Obstacle_Lightning", "Lightning", "Rain", "Can destroy any obstacle hit", "During Rain", "3", "1", "Obstacle", "0.45", "0", ""));
			Rows.Add( new ObjList_G2URow("Boss_Fujin", "Fujin", "Any", "", "Levels 5, 10, 15, 20...", "5000", "100", "Boss", "0.35", "0", "God of Wind."));
			Rows.Add( new ObjList_G2URow("Static_Cloud", "Cloud", "Any", "", "Always Present", "50", "5", "Static", "0.00", "0", "Living the free life."));
			Rows.Add( new ObjList_G2URow("Static_RainClouds", "RainClouds", "Any", "", "Clouds Destroyed", "25", "3", "Static", "0.00", "0", "When you smash it you make the others cry."));
			Rows.Add( new ObjList_G2URow("Static_Sun", "Sun", "Any", "", "Always Present", "200", "50", "Static", "0.00", "0", "Giving life."));
			Rows.Add( new ObjList_G2URow("Static_Moon", "Moon", "Any", "", "Always Present", "200", "50", "Static", "0.00", "0", ""));
			Rows.Add( new ObjList_G2URow("Boss_Bishamonten", "Bishamonten", "Battlefield", "", "Destroy Tank", "5000", "100", "Boss", "0.00", "0", ""));
			Rows.Add( new ObjList_G2URow("Common_City_Road", "City_Road", "City", "N/A", "N/A", "10", "5", "Common", "1.00", "1", "Piece of Road."));
			Rows.Add( new ObjList_G2URow("Common_City_Tree_1", "City_Tree_1", "City", "N/A", "N/A", "5", "1", "Common", "0.95", "1", "Never gets too big."));
			Rows.Add( new ObjList_G2URow("Common_City_Tree_2", "City_Tree_2", "City", "N/A", "N/A", "5", "1", "Common", "0.95", "1", "It grows fruit."));
			Rows.Add( new ObjList_G2URow("Common_Streetlight", "Streetlight", "City", "N/A", "N/A", "3", "1", "Common", "0.90", "0", "Lights for safety."));
			Rows.Add( new ObjList_G2URow("Common_FireHydrant", "FireHydrant", "City", "N/A", "N/A", "1", "1", "Common", "0.90", "1", "Don't park in front of them."));
			Rows.Add( new ObjList_G2URow("Common_Building_1", "Building_1", "City", "N/A", "N/A", "20", "2", "Common", "0.95", "1", "Average brownstone."));
			Rows.Add( new ObjList_G2URow("Common_Building_2", "Building_2", "City", "N/A", "N/A", "20", "2", "Common", "0.95", "1", ""));
			Rows.Add( new ObjList_G2URow("Common_Building_3", "Building_3", "City", "N/A", "N/A", "20", "2", "Common", "0.95", "1", ""));
			Rows.Add( new ObjList_G2URow("Common_Long_Building_1", "Long_Building_1", "City", "N/A", "N/A", "25", "3", "Common", "0.90", "3", ""));
			Rows.Add( new ObjList_G2URow("Common_Long_Building_2", "Long_Building_2", "City", "N/A", "N/A", "25", "3", "Common", "0.90", "2", ""));
			Rows.Add( new ObjList_G2URow("Common_Long_Building_3", "Long_Building_3", "City", "N/A", "N/A", "25", "3", "Common", "0.90", "0", ""));
			Rows.Add( new ObjList_G2URow("Common_Large_Building_1", "Large_Building_1", "City", "N/A", "N/A", "50", "4", "Common", "0.80", "0", ""));
			Rows.Add( new ObjList_G2URow("Common_Large_Building_2", "Large_Building_2", "City", "N/A", "N/A", "50", "4", "Common", "0.80", "0", ""));
			Rows.Add( new ObjList_G2URow("Common_Large_Building_3", "Large_Building_3", "City", "N/A", "N/A", "50", "4", "Common", "0.80", "0", ""));
			Rows.Add( new ObjList_G2URow("Rare_SkyScraper", "SkyScraper", "City", "N/A", "N/A", "75", "10", "Rare", "0.40", "0", ""));
			Rows.Add( new ObjList_G2URow("Obstacle_SafetyCones", "SafetyCones", "City", "N/A", "N/A", "1", "1", "Obstacle", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Obstacle_City_Man", "City_Man", "City", "Walk down Sidewalk toward screen", "N/A", "15", "2", "Obstacle", "0.85", "0", ""));
			Rows.Add( new ObjList_G2URow("Obstacle_City_Woman", "City_Woman", "City", "Walk down Sidewalk toward screen", "N/A", "15", "2", "Obstacle", "0.85", "0", ""));
			Rows.Add( new ObjList_G2URow("Special_Police_Officer", "Police_Officer", "City", "Walk toward Player to arrest", "N/A", "35", "3", "Special", "0.35", "0", ""));
			Rows.Add( new ObjList_G2URow("Special_Police_Car", "Police_Car", "City", "Block Street in front of Player", "Smash 3 people or one police officer", "25", "5", "Special", "0.00", "0", ""));
			Rows.Add( new ObjList_G2URow("Rare_Flowerpot", "Flowerpot", "City", "", "N/A", "45", "1", "Rare", "0.20", "0", ""));
			Rows.Add( new ObjList_G2URow("Rare_Seeds", "Seeds", "City", "", "N/A", "35", "1", "Rare", "0.20", "0", ""));
			Rows.Add( new ObjList_G2URow("Special_Soldier", "Soldier", "City", "Cover each walkable block and move toward screen", "Smash Tank or summon Bishamonten", "50", "5", "Special", "0.00", "0", ""));
			Rows.Add( new ObjList_G2URow("Special_Tank", "Tank", "City", "", "Smash Police Car or summon Bishamonten", "500", "20", "Special", "0.00", "0", ""));
			Rows.Add( new ObjList_G2URow("Rare_Airplane", "Airplane", "City", "", "N/A", "50", "3", "Rare", "0.45", "0", ""));
			Rows.Add( new ObjList_G2URow("Obstacle_Banana_Peel", "Banana_Peel", "City", "", "N/A", "2", "1", "Obstacle", "0.75", "0", ""));
			Rows.Add( new ObjList_G2URow("Common_Soda_Machine", "Soda_Machine", "City", "", "N/A", "3", "1", "Common", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("UnCommon_Arcade", "Arcade", "City", "", "N/A", "80", "1", "UnCommon", "0.35", "0", ""));
			Rows.Add( new ObjList_G2URow("Common_Balloon", "Balloon", "City/Countryside", "", "N/A", "1", "1", "Common", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Common_Telephone_Pole", "Telephone_Pole", "City/Countryside", "", "N/A", "3", "1", "Common", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Common_Checkpoint", "Checkpoint", "All", "", "Level End", "3", "500", "Common", "0.00", "0", ""));
			Rows.Add( new ObjList_G2URow("Obstacle_Cat", "Cat", "City/Countryside/Wilderness", "", "N/A", "2", "1", "Obstacle", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Obstacle_White_Cat", "White_Cat", "City/Countryside/Wilderness", "", "N/A", "2", "1", "Obstacle", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Obstacle_Fat_Cat", "Fat_Cat", "City/Countryside/Wilderness", "", "N/A", "4", "2", "Obstacle", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Obstacle_Fat_Dog", "Fat_Dog", "City/Countryside/Wilderness", "", "N/A", "4", "2", "Obstacle", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Obstacle_Dog", "Dog", "City/Countryside/Wilderness", "", "N/A", "2", "1", "Obstacle", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Obstacle_Squirrell", "Squirrell", "City/Countryside/Wilderness", "", "N/A", "2", "1", "Obstacle", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Obstacle_Bird", "Bird", "City/Countryside/Wilderness", "", "N/A", "2", "1", "Obstacle", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Obstacle_Sheep", "Sheep", "Countryside", "", "N/A", "3", "1", "Obstacle", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Common_Shepherd", "Shepherd", "Countryside", "", "N/A", "5", "1", "Common", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Obstacle_Cow", "Cow", "Countryside", "", "N/A", "3", "1", "Obstacle", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Common_Chicken", "Chicken", "Countryside", "", "N/A", "2", "1", "Common", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Common_Tractor", "Tractor", "Countryside", "", "N/A", "5", "1", "Common", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Common_Silo", "Silo", "Countryside", "", "N/A", "20", "2", "Common", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Common_Farmhouse", "Farmhouse", "Countryside", "", "N/A", "20", "2", "Common", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Common_Farmer_Woman", "Farmer_Woman", "Countryside", "", "N/A", "15", "2", "Common", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Common_Farmer_Man", "Farmer_Man", "Countryside", "", "N/A", "15", "2", "Common", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Rare_Tsuchinoko", "Tsuchinoko", "Countryside", "", "N/A", "200", "1", "Rare", "0.20", "0", ""));
			Rows.Add( new ObjList_G2URow("Common_Fence", "Fence", "Countryside", "", "N/A", "5", "1", "Common", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Common_Windmill", "Windmill", "Countryside", "", "N/A", "20", "2", "Common", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Common_Haystack", "Haystack", "Countryside", "", "N/A", "1", "2", "Common", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Common_Fruit_Tree", "Fruit_Tree", "Countryside", "", "N/A", "3", "1", "Common", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Common_Corn", "Corn", "Countryside", "", "N/A", "2", "1", "Common", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Obstacle_Wolf", "Wolf", "Countryside/Wilderness", "", "N/A", "2", "1", "Obstacle", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("UnCommon_Mushroom", "Mushroom", "Countryside/Wilderness", "", "N/A", "2", "1", "UnCommon", "0.35", "0", ""));
			Rows.Add( new ObjList_G2URow("Common_Crow", "Crow", "Countryside/Wilderness", "", "N/A", "2", "1", "Common", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Common_Seaweed", "Seaweed", "Ocean", "", "N/A", "1", "1", "Common", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Common_Clam", "Clam", "Ocean", "", "N/A", "2", "1", "Common", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Rare_Oyster", "Oyster", "Ocean", "", "N/A", "900", "1", "Rare", "0.20", "0", ""));
			Rows.Add( new ObjList_G2URow("Common_Coral", "Coral", "Ocean", "", "N/A", "2", "1", "Common", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Rare_Whale", "Whale", "Ocean", "", "N/A", "250", "5", "Rare", "0.45", "0", ""));
			Rows.Add( new ObjList_G2URow("Obstacle_Electronic_Eel", "Electronic_Eel", "Ocean", "", "N/A", "2", "1", "Obstacle", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Common_Diver", "Diver", "Ocean", "", "N/A", "5", "2", "Common", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Rare_Kappa", "Kappa", "Ocean", "", "N/A", "500", "2", "Rare", "0.20", "0", ""));
			Rows.Add( new ObjList_G2URow("Common_FishHook", "FishHook", "Ocean", "", "N/A", "1", "1", "Common", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Rare_Wrecked_Ship", "Wrecked_Ship", "Ocean", "", "N/A", "350", "7", "Rare", "0.25", "0", ""));
			Rows.Add( new ObjList_G2URow("Rare_Treasure_Chest", "Treasure_Chest", "Ocean", "", "N/A", "9000", "1", "Rare", "0.15", "0", ""));
			Rows.Add( new ObjList_G2URow("Common_Giant_Anchor", "Giant_Anchor", "Ocean", "", "N/A", "100", "5", "Common", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Obstacle_Octopus", "Octopus", "Ocean", "", "N/A", "0", "1", "Obstacle", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Common_PufferFish", "PufferFish", "Ocean", "", "N/A", "0", "1", "Common", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Common_Dolphin", "Dolphin", "Ocean", "", "N/A", "0", "1", "Common", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Obstacle_Shark", "Shark", "Ocean", "", "N/A", "0", "1", "Obstacle", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Boss_Ebisu", "Ebisu", "Ocean", "", "In Ocean", "0", "50", "Boss", "0.75", "0", ""));
			Rows.Add( new ObjList_G2URow("Boss_Ebisu_Hook", "Ebisu_Hook", "Ocean", "", "In Ocean", "0", "1", "Boss", "0.75", "0", ""));
			Rows.Add( new ObjList_G2URow("Obstacle_Flying_Fish", "Flying_Fish", "Ocean", "", "N/A", "0", "1", "Obstacle", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Obstacle_Crab", "Crab", "Ocean", "", "N/A", "0", "2", "Obstacle", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Rare_Nemo", "Nemo", "Ocean", "", "N/A", "0", "1", "Rare", "0.20", "0", ""));
			Rows.Add( new ObjList_G2URow("Obstacle_Asterorid", "Asterorid", "Space", "", "In Space", "0", "4", "Obstacle", "0.90", "0", ""));
			Rows.Add( new ObjList_G2URow("Special_Mercury", "Mercury", "Space", "", "In Space", "0", "3", "Special", "0.90", "0", ""));
			Rows.Add( new ObjList_G2URow("Special_Venus", "Venus", "Space", "", "In Space", "0", "4", "Special", "0.90", "0", ""));
			Rows.Add( new ObjList_G2URow("Special_Mars", "Mars", "Space", "", "In Space", "0", "4", "Special", "0.90", "0", ""));
			Rows.Add( new ObjList_G2URow("Special_Saturn", "Saturn", "Space", "", "In Space", "0", "5", "Special", "0.90", "0", ""));
			Rows.Add( new ObjList_G2URow("Special_Jupiter", "Jupiter", "Space", "", "In Space", "0", "8", "Special", "0.90", "0", ""));
			Rows.Add( new ObjList_G2URow("Special_Neptune", "Neptune", "Space", "", "In Space", "0", "4", "Special", "0.90", "0", ""));
			Rows.Add( new ObjList_G2URow("Special_Uranus", "Uranus", "Space", "", "In Space", "0", "4", "Special", "0.90", "0", ""));
			Rows.Add( new ObjList_G2URow("Special_Pluto", "Pluto", "Space", "", "In Space", "0", "2", "Special", "0.90", "0", ""));
			Rows.Add( new ObjList_G2URow("Rare_UFO", "UFO", "Space", "", "In Space", "0", "3", "Rare", "0.20", "0", ""));
			Rows.Add( new ObjList_G2URow("Rare_Space_Baby", "Space_Baby", "Space", "", "In Space", "0", "1", "Rare", "0.15", "0", ""));
			Rows.Add( new ObjList_G2URow("Common_Stars", "Stars", "Space/Night", "", "In Space", "0", "1", "Common", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Obstacle_Fox", "Fox", "Wilderness", "", "N/A", "0", "1", "Obstacle", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Obstacle_Deer", "Deer", "Wilderness", "", "N/A", "0", "1", "Obstacle", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Common_Wild_Tree", "Wild_Tree", "Wilderness", "", "N/A", "0", "1", "Common", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Common_Boulder", "Boulder", "Wilderness", "", "N/A", "0", "3", "Common", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Rare_Tengu", "Tengu", "Wilderness", "", "N/A", "0", "1", "Rare", "0.20", "0", ""));
			Rows.Add( new ObjList_G2URow("Common_Cave", "Cave", "Wilderness", "", "N/A", "0", "2", "Common", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Common_Monkey", "Monkey", "Wilderness", "", "N/A", "0", "1", "Common", "0.95", "0", ""));
			Rows.Add( new ObjList_G2URow("Obstacle_Titan", "Titan", "Wilderness", "", "N/A", "0", "5", "Obstacle", "0.25", "0", ""));
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
		public ObjList_G2URow GetRow(rowIds in_RowID)
		{
			ObjList_G2URow ret = null;
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
		public ObjList_G2URow GetRow(string in_RowString)
		{
			ObjList_G2URow ret = null;
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
