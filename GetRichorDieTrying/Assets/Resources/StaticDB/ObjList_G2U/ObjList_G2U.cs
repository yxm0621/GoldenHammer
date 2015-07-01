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
		public string _Category;
		public string _Location;
		public string _Behaviors;
		public string _Trigger;
		public int _Gold;
		public int _HitPoints;
		public float _Occurance;
		public int _Size;
		public string _Encyclopedia;
		public string _Lane;
		public ObjList_G2URow(string __G2U_ID, string __Name, string __Category, string __Location, string __Behaviors, string __Trigger, string __Gold, string __HitPoints, string __Occurance, string __Size, string __Encyclopedia, string __Lane) 
		{
			_Name = __Name.Trim();
			_Category = __Category.Trim();
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
			_Lane = __Lane.Trim();
		}

		public int Length { get { return 11; } }

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
					ret = _Category.ToString();
					break;
				case 2:
					ret = _Location.ToString();
					break;
				case 3:
					ret = _Behaviors.ToString();
					break;
				case 4:
					ret = _Trigger.ToString();
					break;
				case 5:
					ret = _Gold.ToString();
					break;
				case 6:
					ret = _HitPoints.ToString();
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
				case 10:
					ret = _Lane.ToString();
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
				case "Category":
					ret = _Category.ToString();
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
				case "Occurance":
					ret = _Occurance.ToString();
					break;
				case "Size":
					ret = _Size.ToString();
					break;
				case "Encyclopedia":
					ret = _Encyclopedia.ToString();
					break;
				case "Lane":
					ret = _Lane.ToString();
					break;
			}

			return ret;
		}
		public override string ToString()
		{
			string ret = System.String.Empty;
			ret += "{" + "Name" + " : " + _Name.ToString() + "} ";
			ret += "{" + "Category" + " : " + _Category.ToString() + "} ";
			ret += "{" + "Location" + " : " + _Location.ToString() + "} ";
			ret += "{" + "Behaviors" + " : " + _Behaviors.ToString() + "} ";
			ret += "{" + "Trigger" + " : " + _Trigger.ToString() + "} ";
			ret += "{" + "Gold" + " : " + _Gold.ToString() + "} ";
			ret += "{" + "HitPoints" + " : " + _HitPoints.ToString() + "} ";
			ret += "{" + "Occurance" + " : " + _Occurance.ToString() + "} ";
			ret += "{" + "Size" + " : " + _Size.ToString() + "} ";
			ret += "{" + "Encyclopedia" + " : " + _Encyclopedia.ToString() + "} ";
			ret += "{" + "Lane" + " : " + _Lane.ToString() + "} ";
			return ret;
		}
	}
	public sealed class ObjList_G2U : IGoogle2uDB
	{
		public enum rowIds {
			Boss_Bishamonten, Static_City_Road, Static_City_Tree_1, Static_City_Tree_2, Static_Streetlight, Static_FireHydrant, Static_Building_1, Static_Building_2, Static_Building_3, Static_Long_Building_1, Static_Long_Building_2, Static_Long_Building_3, Static_Large_Building_1, Static_Large_Building_2, Static_Large_Building_3, Static_SkyScraper, Traffic_SafetyCones, Traffic_City_Man
			, Traffic_City_Woman, Traffic_Police_Officer, Traffic_Police_Car, Static_Flowerpot, Static_Seeds, Traffic_Soldier, Traffic_Tank, Global_Airplane, Traffic_Banana_Peel, Static_Soda_Machine, Static_Arcade, Global_Balloon, Static_Telephone_Pole, Boss_Fujin, Global_RainClouds, Traffic_Cat, Traffic_White_Cat, Traffic_Fat_Cat, Traffic_Fat_Dog, Traffic_Dog
			, Traffic_Squirrell, Global_Bird, Global_Cloud, Global_Sun, Global_Moon, Static_Checkpoint, Traffic_Sheep, Traffic_Shepherd, Traffic_Cow, Traffic_Chicken, Traffic_Tractor, Static_Silo, Static_Farmhouse, Traffic_Farmer_Woman, Traffic_Farmer_Man, Traffic_Tsuchinoko, Static_Fence, Static_Windmill, Static_Haystack, Static_Fruit_Tree
			, Static_Corn, Traffic_Wolf, Static_Mushroom, Global_Crow, Static_Seaweed, Static_Clam, Static_Oyster, Static_Coral, Traffic_Whale, Traffic_Electronic_Eel, Traffic_Diver, Traffic_Kappa, Traffic_FishHook, Static_Wrecked_Ship, Static_Treasure_Chest, Static_Giant_Anchor, Traffic_Octopus, Traffic_PufferFish, Traffic_Dolphin, Traffic_Shark
			, Boss_Ebisu, Boss_Ebisu_Hook, Traffic_Flying_Fish, Traffic_Crab, Traffic_Nemo, Boss_Raijin, Boss_Lightning, Traffic_Asterorid, Global_Mercury, Global_Venus, Global_Mars, Global_Saturn, Global_Jupiter, Global_Neptune, Global_Uranus, Global_Pluto, Global_UFO, Global_Space_Baby, Global_Stars, Traffic_Fox
			, Traffic_Deer, Static_Wild_Tree, Static_Boulder, Traffic_Tengu, Static_Cave, Traffic_Monkey, Traffic_Titan
		};
		public string [] rowNames = {
			"Boss_Bishamonten", "Static_City_Road", "Static_City_Tree_1", "Static_City_Tree_2", "Static_Streetlight", "Static_FireHydrant", "Static_Building_1", "Static_Building_2", "Static_Building_3", "Static_Long_Building_1", "Static_Long_Building_2", "Static_Long_Building_3", "Static_Large_Building_1", "Static_Large_Building_2", "Static_Large_Building_3", "Static_SkyScraper", "Traffic_SafetyCones", "Traffic_City_Man"
			, "Traffic_City_Woman", "Traffic_Police_Officer", "Traffic_Police_Car", "Static_Flowerpot", "Static_Seeds", "Traffic_Soldier", "Traffic_Tank", "Global_Airplane", "Traffic_Banana_Peel", "Static_Soda_Machine", "Static_Arcade", "Global_Balloon", "Static_Telephone_Pole", "Boss_Fujin", "Global_RainClouds", "Traffic_Cat", "Traffic_White_Cat", "Traffic_Fat_Cat", "Traffic_Fat_Dog", "Traffic_Dog"
			, "Traffic_Squirrell", "Global_Bird", "Global_Cloud", "Global_Sun", "Global_Moon", "Static_Checkpoint", "Traffic_Sheep", "Traffic_Shepherd", "Traffic_Cow", "Traffic_Chicken", "Traffic_Tractor", "Static_Silo", "Static_Farmhouse", "Traffic_Farmer_Woman", "Traffic_Farmer_Man", "Traffic_Tsuchinoko", "Static_Fence", "Static_Windmill", "Static_Haystack", "Static_Fruit_Tree"
			, "Static_Corn", "Traffic_Wolf", "Static_Mushroom", "Global_Crow", "Static_Seaweed", "Static_Clam", "Static_Oyster", "Static_Coral", "Traffic_Whale", "Traffic_Electronic_Eel", "Traffic_Diver", "Traffic_Kappa", "Traffic_FishHook", "Static_Wrecked_Ship", "Static_Treasure_Chest", "Static_Giant_Anchor", "Traffic_Octopus", "Traffic_PufferFish", "Traffic_Dolphin", "Traffic_Shark"
			, "Boss_Ebisu", "Boss_Ebisu_Hook", "Traffic_Flying_Fish", "Traffic_Crab", "Traffic_Nemo", "Boss_Raijin", "Boss_Lightning", "Traffic_Asterorid", "Global_Mercury", "Global_Venus", "Global_Mars", "Global_Saturn", "Global_Jupiter", "Global_Neptune", "Global_Uranus", "Global_Pluto", "Global_UFO", "Global_Space_Baby", "Global_Stars", "Traffic_Fox"
			, "Traffic_Deer", "Static_Wild_Tree", "Static_Boulder", "Traffic_Tengu", "Static_Cave", "Traffic_Monkey", "Traffic_Titan"
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
			Rows.Add( new ObjList_G2URow("Boss_Bishamonten", "Bishamonten", "Boss", "Battlefield", "", "Destroy Tank", "5000", "100", "0.00", "0", "", "Special"));
			Rows.Add( new ObjList_G2URow("Static_City_Road", "City_Road", "Static", "City", "N/A", "N/A", "10", "5", "1.00", "1", "Piece of Road.", "Road"));
			Rows.Add( new ObjList_G2URow("Static_City_Tree_1", "City_Tree_1", "Static", "City", "N/A", "N/A", "5", "1", "0.95", "1", "Never gets too big.", "Sidewalk"));
			Rows.Add( new ObjList_G2URow("Static_City_Tree_2", "City_Tree_2", "Static", "City", "N/A", "N/A", "5", "1", "0.95", "1", "It grows fruit.", "Sidewalk"));
			Rows.Add( new ObjList_G2URow("Static_Streetlight", "Streetlight", "Static", "City", "N/A", "N/A", "3", "1", "0.90", "0", "Lights for safety.", "Sidewalk"));
			Rows.Add( new ObjList_G2URow("Static_FireHydrant", "FireHydrant", "Static", "City", "N/A", "N/A", "1", "1", "0.90", "1", "Don't park in front of them.", "Sidewalk"));
			Rows.Add( new ObjList_G2URow("Static_Building_1", "Building_1", "Static", "City", "N/A", "N/A", "20", "2", "0.95", "1", "Average brownstone.", "Ground"));
			Rows.Add( new ObjList_G2URow("Static_Building_2", "Building_2", "Static", "City", "N/A", "N/A", "20", "2", "0.95", "1", "", "Ground"));
			Rows.Add( new ObjList_G2URow("Static_Building_3", "Building_3", "Static", "City", "N/A", "N/A", "20", "2", "0.95", "1", "", "Ground"));
			Rows.Add( new ObjList_G2URow("Static_Long_Building_1", "Long_Building_1", "Static", "City", "N/A", "N/A", "25", "3", "0.90", "3", "", "Ground"));
			Rows.Add( new ObjList_G2URow("Static_Long_Building_2", "Long_Building_2", "Static", "City", "N/A", "N/A", "25", "3", "0.90", "2", "", "Ground"));
			Rows.Add( new ObjList_G2URow("Static_Long_Building_3", "Long_Building_3", "Static", "City", "N/A", "N/A", "25", "3", "0.90", "0", "", "Ground"));
			Rows.Add( new ObjList_G2URow("Static_Large_Building_1", "Large_Building_1", "Static", "City", "N/A", "N/A", "50", "4", "0.80", "0", "", "Ground"));
			Rows.Add( new ObjList_G2URow("Static_Large_Building_2", "Large_Building_2", "Static", "City", "N/A", "N/A", "50", "4", "0.80", "0", "", "Ground"));
			Rows.Add( new ObjList_G2URow("Static_Large_Building_3", "Large_Building_3", "Static", "City", "N/A", "N/A", "50", "4", "0.80", "0", "", "Ground"));
			Rows.Add( new ObjList_G2URow("Static_SkyScraper", "SkyScraper", "Static", "City", "N/A", "N/A", "75", "10", "0.40", "0", "", "Ground"));
			Rows.Add( new ObjList_G2URow("Traffic_SafetyCones", "SafetyCones", "Traffic", "City", "N/A", "N/A", "1", "1", "0.95", "0", "", "Sidewalk"));
			Rows.Add( new ObjList_G2URow("Traffic_City_Man", "City_Man", "Traffic", "City", "Walk down Sidewalk toward screen", "N/A", "15", "2", "0.85", "0", "", "Sidewalk"));
			Rows.Add( new ObjList_G2URow("Traffic_City_Woman", "City_Woman", "Traffic", "City", "Walk down Sidewalk toward screen", "N/A", "15", "2", "0.85", "0", "", "Sidewalk"));
			Rows.Add( new ObjList_G2URow("Traffic_Police_Officer", "Police_Officer", "Traffic", "City", "Walk toward Player to arrest", "N/A", "35", "3", "0.35", "0", "", "Sidewalk"));
			Rows.Add( new ObjList_G2URow("Traffic_Police_Car", "Police_Car", "Traffic", "City", "Block Street in front of Player", "Smash 3 people or one police officer", "25", "5", "0.00", "0", "", "Road"));
			Rows.Add( new ObjList_G2URow("Static_Flowerpot", "Flowerpot", "Static", "City", "", "N/A", "45", "1", "0.20", "0", "", "Ground"));
			Rows.Add( new ObjList_G2URow("Static_Seeds", "Seeds", "Static", "City", "", "N/A", "35", "1", "0.20", "0", "", "Ground"));
			Rows.Add( new ObjList_G2URow("Traffic_Soldier", "Soldier", "Traffic", "City", "Cover each walkable block and move toward screen", "Smash Tank or summon Bishamonten", "50", "5", "0.00", "0", "", "Sidewalk"));
			Rows.Add( new ObjList_G2URow("Traffic_Tank", "Tank", "Traffic", "City", "", "Smash Police Car or summon Bishamonten", "500", "20", "0.00", "0", "", "Road"));
			Rows.Add( new ObjList_G2URow("Global_Airplane", "Airplane", "Global", "City", "", "N/A", "50", "3", "0.45", "0", "", "Special"));
			Rows.Add( new ObjList_G2URow("Traffic_Banana_Peel", "Banana_Peel", "Traffic", "City", "", "N/A", "2", "1", "0.75", "0", "", "Sidewalk"));
			Rows.Add( new ObjList_G2URow("Static_Soda_Machine", "Soda_Machine", "Static", "City", "", "N/A", "3", "1", "0.95", "0", "", "Sidewalk"));
			Rows.Add( new ObjList_G2URow("Static_Arcade", "Arcade", "Static", "City", "", "N/A", "80", "1", "0.35", "0", "", "Sidewalk"));
			Rows.Add( new ObjList_G2URow("Global_Balloon", "Balloon", "Global", "City/Countryside", "", "N/A", "1", "1", "0.95", "0", "", "Special"));
			Rows.Add( new ObjList_G2URow("Static_Telephone_Pole", "Telephone_Pole", "Static", "City/Countryside", "", "N/A", "3", "1", "0.95", "0", "", "Sidewalk"));
			Rows.Add( new ObjList_G2URow("Boss_Fujin", "Fujin", "Boss", "City/Countryside/Wilderness", "", "Levels 5, 10, 15, 20...", "5000", "100", "0.35", "0", "God of Wind.", "Special"));
			Rows.Add( new ObjList_G2URow("Global_RainClouds", "RainClouds", "Global", "City/Countryside/Wilderness", "", "Clouds Destroyed", "25", "3", "0.00", "0", "When you smash it you make the others cry.", "Special"));
			Rows.Add( new ObjList_G2URow("Traffic_Cat", "Cat", "Traffic", "City/Countryside/Wilderness", "", "N/A", "2", "1", "0.95", "0", "", "Sidewalk"));
			Rows.Add( new ObjList_G2URow("Traffic_White_Cat", "White_Cat", "Traffic", "City/Countryside/Wilderness", "", "N/A", "2", "1", "0.95", "0", "", "Sidewalk"));
			Rows.Add( new ObjList_G2URow("Traffic_Fat_Cat", "Fat_Cat", "Traffic", "City/Countryside/Wilderness", "", "N/A", "4", "2", "0.95", "0", "", "Sidewalk"));
			Rows.Add( new ObjList_G2URow("Traffic_Fat_Dog", "Fat_Dog", "Traffic", "City/Countryside/Wilderness", "", "N/A", "4", "2", "0.95", "0", "", "Sidewalk"));
			Rows.Add( new ObjList_G2URow("Traffic_Dog", "Dog", "Traffic", "City/Countryside/Wilderness", "", "N/A", "2", "1", "0.95", "0", "", "Sidewalk"));
			Rows.Add( new ObjList_G2URow("Traffic_Squirrell", "Squirrell", "Traffic", "City/Countryside/Wilderness", "", "N/A", "2", "1", "0.95", "0", "", "Sidewalk"));
			Rows.Add( new ObjList_G2URow("Global_Bird", "Bird", "Global", "City/Countryside/Wilderness", "", "N/A", "2", "1", "0.95", "0", "", "Special"));
			Rows.Add( new ObjList_G2URow("Global_Cloud", "Cloud", "Global", "City/Countryside/Wilderness/Battlefield", "", "Always Present", "50", "5", "0.00", "0", "Living the free life.", "Special"));
			Rows.Add( new ObjList_G2URow("Global_Sun", "Sun", "Global", "City/Countryside/Wilderness/Battlefield", "", "Always Present", "200", "50", "0.00", "0", "Giving life.", "Special"));
			Rows.Add( new ObjList_G2URow("Global_Moon", "Moon", "Global", "City/Countryside/Wilderness/Battlefield", "", "Always Present", "200", "50", "0.00", "0", "", "Special"));
			Rows.Add( new ObjList_G2URow("Static_Checkpoint", "Checkpoint", "Static", "City/Countryside/Wilderness/Ocean", "", "Special Power", "3", "500", "0.00", "0", "", "Sidewalk"));
			Rows.Add( new ObjList_G2URow("Traffic_Sheep", "Sheep", "Traffic", "Countryside", "", "N/A", "3", "1", "0.95", "0", "", "Sidewalk"));
			Rows.Add( new ObjList_G2URow("Traffic_Shepherd", "Shepherd", "Traffic", "Countryside", "", "N/A", "5", "1", "0.95", "0", "", "Sidewalk"));
			Rows.Add( new ObjList_G2URow("Traffic_Cow", "Cow", "Traffic", "Countryside", "", "N/A", "3", "1", "0.95", "0", "", "Road"));
			Rows.Add( new ObjList_G2URow("Traffic_Chicken", "Chicken", "Traffic", "Countryside", "", "N/A", "2", "1", "0.95", "0", "", "Sidewalk"));
			Rows.Add( new ObjList_G2URow("Traffic_Tractor", "Tractor", "Traffic", "Countryside", "", "N/A", "5", "1", "0.95", "0", "", "Road"));
			Rows.Add( new ObjList_G2URow("Static_Silo", "Silo", "Static", "Countryside", "", "N/A", "20", "2", "0.95", "0", "", "Ground"));
			Rows.Add( new ObjList_G2URow("Static_Farmhouse", "Farmhouse", "Static", "Countryside", "", "N/A", "20", "2", "0.95", "0", "", "Ground"));
			Rows.Add( new ObjList_G2URow("Traffic_Farmer_Woman", "Farmer_Woman", "Traffic", "Countryside", "", "N/A", "15", "2", "0.95", "0", "", "Sidewalk"));
			Rows.Add( new ObjList_G2URow("Traffic_Farmer_Man", "Farmer_Man", "Traffic", "Countryside", "", "N/A", "15", "2", "0.95", "0", "", "Sidewalk"));
			Rows.Add( new ObjList_G2URow("Traffic_Tsuchinoko", "Tsuchinoko", "Traffic", "Countryside", "", "N/A", "200", "1", "0.20", "0", "", "Ground"));
			Rows.Add( new ObjList_G2URow("Static_Fence", "Fence", "Static", "Countryside", "", "N/A", "5", "1", "0.95", "0", "", "Ground"));
			Rows.Add( new ObjList_G2URow("Static_Windmill", "Windmill", "Static", "Countryside", "", "N/A", "20", "2", "0.95", "0", "", "Ground"));
			Rows.Add( new ObjList_G2URow("Static_Haystack", "Haystack", "Static", "Countryside", "", "N/A", "1", "2", "0.95", "0", "", "Ground"));
			Rows.Add( new ObjList_G2URow("Static_Fruit_Tree", "Fruit_Tree", "Static", "Countryside", "", "N/A", "3", "1", "0.95", "0", "", "Ground"));
			Rows.Add( new ObjList_G2URow("Static_Corn", "Corn", "Static", "Countryside", "", "N/A", "2", "1", "0.95", "0", "", "Ground"));
			Rows.Add( new ObjList_G2URow("Traffic_Wolf", "Wolf", "Traffic", "Countryside/Wilderness", "", "N/A", "2", "1", "0.95", "0", "", "Sidewalk"));
			Rows.Add( new ObjList_G2URow("Static_Mushroom", "Mushroom", "Static", "Countryside/Wilderness", "", "N/A", "2", "1", "0.35", "0", "", "Ground"));
			Rows.Add( new ObjList_G2URow("Global_Crow", "Crow", "Global", "Countryside/Wilderness", "", "N/A", "2", "1", "0.95", "0", "", "Special"));
			Rows.Add( new ObjList_G2URow("Static_Seaweed", "Seaweed", "Static", "Ocean", "", "N/A", "1", "1", "0.95", "0", "", "Sidewalk"));
			Rows.Add( new ObjList_G2URow("Static_Clam", "Clam", "Static", "Ocean", "", "N/A", "2", "1", "0.95", "0", "", "Ground"));
			Rows.Add( new ObjList_G2URow("Static_Oyster", "Oyster", "Static", "Ocean", "", "N/A", "900", "1", "0.20", "0", "", "Sidewalk"));
			Rows.Add( new ObjList_G2URow("Static_Coral", "Coral", "Static", "Ocean", "", "N/A", "2", "1", "0.95", "0", "", "Ground"));
			Rows.Add( new ObjList_G2URow("Traffic_Whale", "Whale", "Traffic", "Ocean", "", "N/A", "250", "5", "0.45", "0", "", "Ground"));
			Rows.Add( new ObjList_G2URow("Traffic_Electronic_Eel", "Electronic_Eel", "Traffic", "Ocean", "", "N/A", "2", "1", "0.95", "0", "", "Road"));
			Rows.Add( new ObjList_G2URow("Traffic_Diver", "Diver", "Traffic", "Ocean", "", "N/A", "5", "2", "0.95", "0", "", "Road"));
			Rows.Add( new ObjList_G2URow("Traffic_Kappa", "Kappa", "Traffic", "Ocean", "", "N/A", "500", "2", "0.20", "0", "", "Sidewalk"));
			Rows.Add( new ObjList_G2URow("Traffic_FishHook", "FishHook", "Traffic", "Ocean", "", "N/A", "1", "1", "0.95", "0", "", "Special"));
			Rows.Add( new ObjList_G2URow("Static_Wrecked_Ship", "Wrecked_Ship", "Static", "Ocean", "", "N/A", "350", "7", "0.25", "0", "", "Ground"));
			Rows.Add( new ObjList_G2URow("Static_Treasure_Chest", "Treasure_Chest", "Static", "Ocean", "", "N/A", "9000", "1", "0.15", "0", "", "Ground"));
			Rows.Add( new ObjList_G2URow("Static_Giant_Anchor", "Giant_Anchor", "Static", "Ocean", "", "N/A", "100", "5", "0.95", "0", "", "Ground"));
			Rows.Add( new ObjList_G2URow("Traffic_Octopus", "Octopus", "Traffic", "Ocean", "", "N/A", "0", "1", "0.95", "0", "", "Sidewalk"));
			Rows.Add( new ObjList_G2URow("Traffic_PufferFish", "PufferFish", "Traffic", "Ocean", "", "N/A", "0", "1", "0.95", "0", "", "Sidewalk"));
			Rows.Add( new ObjList_G2URow("Traffic_Dolphin", "Dolphin", "Traffic", "Ocean", "", "N/A", "0", "1", "0.95", "0", "", "Road"));
			Rows.Add( new ObjList_G2URow("Traffic_Shark", "Shark", "Traffic", "Ocean", "", "N/A", "0", "1", "0.95", "0", "", "Road"));
			Rows.Add( new ObjList_G2URow("Boss_Ebisu", "Ebisu", "Boss", "Ocean", "", "In Ocean", "0", "50", "0.75", "0", "", "Special"));
			Rows.Add( new ObjList_G2URow("Boss_Ebisu_Hook", "Ebisu_Hook", "Boss", "Ocean", "", "In Ocean", "0", "1", "0.75", "0", "", "Special"));
			Rows.Add( new ObjList_G2URow("Traffic_Flying_Fish", "Flying_Fish", "Traffic", "Ocean", "", "N/A", "0", "1", "0.95", "0", "", "Special"));
			Rows.Add( new ObjList_G2URow("Traffic_Crab", "Crab", "Traffic", "Ocean", "", "N/A", "0", "2", "0.95", "0", "", "Sidewalk"));
			Rows.Add( new ObjList_G2URow("Traffic_Nemo", "Nemo", "Traffic", "Ocean", "", "N/A", "0", "1", "0.20", "0", "", "Sidewalk"));
			Rows.Add( new ObjList_G2URow("Boss_Raijin", "Raijin", "Boss", "Rain", "Appears every 5 seconds during rain and throws lighting bolts", "Appears During Rain", "5000", "100", "0.00", "0", "God of Thunder.", "Special"));
			Rows.Add( new ObjList_G2URow("Boss_Lightning", "Lightning", "Boss", "Rain", "Can destroy any obstacle hit", "During Rain", "3", "1", "0.45", "0", "", "Special"));
			Rows.Add( new ObjList_G2URow("Traffic_Asterorid", "Asterorid", "Traffic", "Space", "", "In Space", "0", "4", "0.90", "0", "", "Special"));
			Rows.Add( new ObjList_G2URow("Global_Mercury", "Mercury", "Global", "Space", "", "In Space", "0", "3", "0.90", "0", "", "Special"));
			Rows.Add( new ObjList_G2URow("Global_Venus", "Venus", "Global", "Space", "", "In Space", "0", "4", "0.90", "0", "", "Special"));
			Rows.Add( new ObjList_G2URow("Global_Mars", "Mars", "Global", "Space", "", "In Space", "0", "4", "0.90", "0", "", "Special"));
			Rows.Add( new ObjList_G2URow("Global_Saturn", "Saturn", "Global", "Space", "", "In Space", "0", "5", "0.90", "0", "", "Special"));
			Rows.Add( new ObjList_G2URow("Global_Jupiter", "Jupiter", "Global", "Space", "", "In Space", "0", "8", "0.90", "0", "", "Special"));
			Rows.Add( new ObjList_G2URow("Global_Neptune", "Neptune", "Global", "Space", "", "In Space", "0", "4", "0.90", "0", "", "Special"));
			Rows.Add( new ObjList_G2URow("Global_Uranus", "Uranus", "Global", "Space", "", "In Space", "0", "4", "0.90", "0", "", "Special"));
			Rows.Add( new ObjList_G2URow("Global_Pluto", "Pluto", "Global", "Space", "", "In Space", "0", "2", "0.90", "0", "", "Special"));
			Rows.Add( new ObjList_G2URow("Global_UFO", "UFO", "Global", "Space", "", "In Space", "0", "3", "0.20", "0", "", "Special"));
			Rows.Add( new ObjList_G2URow("Global_Space_Baby", "Space_Baby", "Global", "Space", "", "In Space", "0", "1", "0.15", "0", "", "Special"));
			Rows.Add( new ObjList_G2URow("Global_Stars", "Stars", "Global", "Space/Night", "", "In Space", "0", "1", "0.95", "0", "", "Special"));
			Rows.Add( new ObjList_G2URow("Traffic_Fox", "Fox", "Traffic", "Wilderness", "", "N/A", "0", "1", "0.95", "0", "", "Sidewalk"));
			Rows.Add( new ObjList_G2URow("Traffic_Deer", "Deer", "Traffic", "Wilderness", "", "N/A", "0", "1", "0.95", "0", "", "Sidewalk"));
			Rows.Add( new ObjList_G2URow("Static_Wild_Tree", "Wild_Tree", "Static", "Wilderness", "", "N/A", "0", "1", "0.95", "0", "", "Ground"));
			Rows.Add( new ObjList_G2URow("Static_Boulder", "Boulder", "Static", "Wilderness", "", "N/A", "0", "3", "0.95", "0", "", "Ground"));
			Rows.Add( new ObjList_G2URow("Traffic_Tengu", "Tengu", "Traffic", "Wilderness", "", "N/A", "0", "1", "0.20", "0", "", "Sidewalk"));
			Rows.Add( new ObjList_G2URow("Static_Cave", "Cave", "Static", "Wilderness", "", "N/A", "0", "2", "0.95", "0", "", "Ground"));
			Rows.Add( new ObjList_G2URow("Traffic_Monkey", "Monkey", "Traffic", "Wilderness", "", "N/A", "0", "1", "0.95", "0", "", "Sidewalk"));
			Rows.Add( new ObjList_G2URow("Traffic_Titan", "Titan", "Traffic", "Wilderness", "", "N/A", "0", "5", "0.25", "0", "", "Road"));
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
