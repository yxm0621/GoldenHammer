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
	public class TrafficDesignRow : IGoogle2uRow
	{
		public int _Level;
		public int _Segment;
		public int _Distance;
		public int _Gold;
		public float _Easy_Per;
		public int _Easy_Num;
		public float _Med_Per;
		public int _Med_Num;
		public float _Hard_Per;
		public int _Hard_Num;
		public float _SuHard_Per;
		public int _SuHard_Num;
		public int _Total_Obst;
		public int _Time_Interval;
		public TrafficDesignRow(string __G2U_ID, string __Level, string __Segment, string __Distance, string __Gold, string __Easy_Per, string __Easy_Num, string __Med_Per, string __Med_Num, string __Hard_Per, string __Hard_Num, string __SuHard_Per, string __SuHard_Num, string __Total_Obst, string __Time_Interval) 
		{
			{
			int res;
				if(int.TryParse(__Level, out res))
					_Level = res;
				else
					Debug.LogError("Failed To Convert _Level string: "+ __Level +" to int");
			}
			{
			int res;
				if(int.TryParse(__Segment, out res))
					_Segment = res;
				else
					Debug.LogError("Failed To Convert _Segment string: "+ __Segment +" to int");
			}
			{
			int res;
				if(int.TryParse(__Distance, out res))
					_Distance = res;
				else
					Debug.LogError("Failed To Convert _Distance string: "+ __Distance +" to int");
			}
			{
			int res;
				if(int.TryParse(__Gold, out res))
					_Gold = res;
				else
					Debug.LogError("Failed To Convert _Gold string: "+ __Gold +" to int");
			}
			{
			float res;
				if(float.TryParse(__Easy_Per, out res))
					_Easy_Per = res;
				else
					Debug.LogError("Failed To Convert _Easy_Per string: "+ __Easy_Per +" to float");
			}
			{
			int res;
				if(int.TryParse(__Easy_Num, out res))
					_Easy_Num = res;
				else
					Debug.LogError("Failed To Convert _Easy_Num string: "+ __Easy_Num +" to int");
			}
			{
			float res;
				if(float.TryParse(__Med_Per, out res))
					_Med_Per = res;
				else
					Debug.LogError("Failed To Convert _Med_Per string: "+ __Med_Per +" to float");
			}
			{
			int res;
				if(int.TryParse(__Med_Num, out res))
					_Med_Num = res;
				else
					Debug.LogError("Failed To Convert _Med_Num string: "+ __Med_Num +" to int");
			}
			{
			float res;
				if(float.TryParse(__Hard_Per, out res))
					_Hard_Per = res;
				else
					Debug.LogError("Failed To Convert _Hard_Per string: "+ __Hard_Per +" to float");
			}
			{
			int res;
				if(int.TryParse(__Hard_Num, out res))
					_Hard_Num = res;
				else
					Debug.LogError("Failed To Convert _Hard_Num string: "+ __Hard_Num +" to int");
			}
			{
			float res;
				if(float.TryParse(__SuHard_Per, out res))
					_SuHard_Per = res;
				else
					Debug.LogError("Failed To Convert _SuHard_Per string: "+ __SuHard_Per +" to float");
			}
			{
			int res;
				if(int.TryParse(__SuHard_Num, out res))
					_SuHard_Num = res;
				else
					Debug.LogError("Failed To Convert _SuHard_Num string: "+ __SuHard_Num +" to int");
			}
			{
			int res;
				if(int.TryParse(__Total_Obst, out res))
					_Total_Obst = res;
				else
					Debug.LogError("Failed To Convert _Total_Obst string: "+ __Total_Obst +" to int");
			}
			{
			int res;
				if(int.TryParse(__Time_Interval, out res))
					_Time_Interval = res;
				else
					Debug.LogError("Failed To Convert _Time_Interval string: "+ __Time_Interval +" to int");
			}
		}

		public int Length { get { return 14; } }

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
					ret = _Level.ToString();
					break;
				case 1:
					ret = _Segment.ToString();
					break;
				case 2:
					ret = _Distance.ToString();
					break;
				case 3:
					ret = _Gold.ToString();
					break;
				case 4:
					ret = _Easy_Per.ToString();
					break;
				case 5:
					ret = _Easy_Num.ToString();
					break;
				case 6:
					ret = _Med_Per.ToString();
					break;
				case 7:
					ret = _Med_Num.ToString();
					break;
				case 8:
					ret = _Hard_Per.ToString();
					break;
				case 9:
					ret = _Hard_Num.ToString();
					break;
				case 10:
					ret = _SuHard_Per.ToString();
					break;
				case 11:
					ret = _SuHard_Num.ToString();
					break;
				case 12:
					ret = _Total_Obst.ToString();
					break;
				case 13:
					ret = _Time_Interval.ToString();
					break;
			}

			return ret;
		}

		public string GetStringData( string colID )
		{
			var ret = System.String.Empty;
			switch( colID.ToLower() )
			{
				case "Level":
					ret = _Level.ToString();
					break;
				case "Segment":
					ret = _Segment.ToString();
					break;
				case "Distance":
					ret = _Distance.ToString();
					break;
				case "Gold":
					ret = _Gold.ToString();
					break;
				case "Easy_Per":
					ret = _Easy_Per.ToString();
					break;
				case "Easy_Num":
					ret = _Easy_Num.ToString();
					break;
				case "Med_Per":
					ret = _Med_Per.ToString();
					break;
				case "Med_Num":
					ret = _Med_Num.ToString();
					break;
				case "Hard_Per":
					ret = _Hard_Per.ToString();
					break;
				case "Hard_Num":
					ret = _Hard_Num.ToString();
					break;
				case "SuHard_Per":
					ret = _SuHard_Per.ToString();
					break;
				case "SuHard_Num":
					ret = _SuHard_Num.ToString();
					break;
				case "Total_Obst":
					ret = _Total_Obst.ToString();
					break;
				case "Time_Interval":
					ret = _Time_Interval.ToString();
					break;
			}

			return ret;
		}
		public override string ToString()
		{
			string ret = System.String.Empty;
			ret += "{" + "Level" + " : " + _Level.ToString() + "} ";
			ret += "{" + "Segment" + " : " + _Segment.ToString() + "} ";
			ret += "{" + "Distance" + " : " + _Distance.ToString() + "} ";
			ret += "{" + "Gold" + " : " + _Gold.ToString() + "} ";
			ret += "{" + "Easy_Per" + " : " + _Easy_Per.ToString() + "} ";
			ret += "{" + "Easy_Num" + " : " + _Easy_Num.ToString() + "} ";
			ret += "{" + "Med_Per" + " : " + _Med_Per.ToString() + "} ";
			ret += "{" + "Med_Num" + " : " + _Med_Num.ToString() + "} ";
			ret += "{" + "Hard_Per" + " : " + _Hard_Per.ToString() + "} ";
			ret += "{" + "Hard_Num" + " : " + _Hard_Num.ToString() + "} ";
			ret += "{" + "SuHard_Per" + " : " + _SuHard_Per.ToString() + "} ";
			ret += "{" + "SuHard_Num" + " : " + _SuHard_Num.ToString() + "} ";
			ret += "{" + "Total_Obst" + " : " + _Total_Obst.ToString() + "} ";
			ret += "{" + "Time_Interval" + " : " + _Time_Interval.ToString() + "} ";
			return ret;
		}
	}
	public sealed class TrafficDesign : IGoogle2uDB
	{
		public enum rowIds {
			Level_1, Level_2, Level_3, Level_4, Level_5, Level_6, Level_7, Level_8, Level_9, Level_10, Level_11, Level_12, Level_13, Level_14, Level_15, Level_16, Level_17, Level_18
			, Level_19, Level_20, Level_21, Level_22, Level_23, Level_24, Level_25, Level_26, Level_27, Level_28, Level_29, Level_30, Level_31, Level_32, Level_33, Level_34, Level_35, Level_36, Level_37, Level_38
			, Level_39, Level_40, Level_41, Level_42, Level_43, Level_44, Level_45, Level_46, Level_47, Level_48, Level_49, Level_50
		};
		public string [] rowNames = {
			"Level_1", "Level_2", "Level_3", "Level_4", "Level_5", "Level_6", "Level_7", "Level_8", "Level_9", "Level_10", "Level_11", "Level_12", "Level_13", "Level_14", "Level_15", "Level_16", "Level_17", "Level_18"
			, "Level_19", "Level_20", "Level_21", "Level_22", "Level_23", "Level_24", "Level_25", "Level_26", "Level_27", "Level_28", "Level_29", "Level_30", "Level_31", "Level_32", "Level_33", "Level_34", "Level_35", "Level_36", "Level_37", "Level_38"
			, "Level_39", "Level_40", "Level_41", "Level_42", "Level_43", "Level_44", "Level_45", "Level_46", "Level_47", "Level_48", "Level_49", "Level_50"
		};
		public System.Collections.Generic.List<TrafficDesignRow> Rows = new System.Collections.Generic.List<TrafficDesignRow>();

		public static TrafficDesign Instance
		{
			get { return NestedTrafficDesign.instance; }
		}

		private class NestedTrafficDesign
		{
			static NestedTrafficDesign() { }
			internal static readonly TrafficDesign instance = new TrafficDesign();
		}

		private TrafficDesign()
		{
			Rows.Add( new TrafficDesignRow("Level_1", "1", "5", "30", "500", "0.20", "2", "0.60", "6", "0.20", "2", "0.00", "0", "10", "3"));
			Rows.Add( new TrafficDesignRow("Level_2", "2", "10", "60", "1000", "0.10", "1", "0.60", "6", "0.30", "3", "0.00", "0", "10", "3"));
			Rows.Add( new TrafficDesignRow("Level_3", "3", "15", "90", "2000", "0.05", "1", "0.60", "6", "0.30", "3", "0.05", "1", "10", "3"));
			Rows.Add( new TrafficDesignRow("Level_4", "4", "20", "120", "3500", "0.00", "0", "0.50", "5", "0.45", "5", "0.05", "1", "10", "3"));
			Rows.Add( new TrafficDesignRow("Level_5", "5", "25", "150", "5000", "0.40", "4", "0.60", "6", "0.00", "0", "0.00", "0", "10", "3"));
			Rows.Add( new TrafficDesignRow("Level_6", "6", "30", "180", "6500", "0.20", "3", "0.60", "9", "0.20", "3", "0.00", "0", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_7", "7", "35", "210", "8000", "0.10", "2", "0.60", "9", "0.30", "5", "0.00", "0", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_8", "8", "40", "240", "9500", "0.05", "1", "0.60", "9", "0.30", "5", "0.05", "1", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_9", "9", "45", "270", "11000", "0.00", "0", "0.50", "8", "0.45", "7", "0.05", "1", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_10", "10", "50", "300", "12500", "0.40", "6", "0.60", "9", "0.00", "0", "0.00", "0", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_11", "11", "55", "330", "14000", "0.15", "2", "0.60", "9", "0.25", "4", "0.00", "0", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_12", "12", "60", "360", "15500", "0.05", "1", "0.60", "9", "0.35", "5", "0.00", "0", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_13", "13", "65", "390", "17000", "0.00", "0", "0.45", "7", "0.50", "8", "0.05", "1", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_14", "14", "70", "420", "18500", "0.00", "0", "0.40", "6", "0.50", "8", "0.10", "2", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_15", "15", "75", "450", "20000", "0.40", "6", "0.60", "9", "0.00", "0", "0.00", "0", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_16", "16", "80", "480", "21500", "0.10", "2", "0.60", "9", "0.30", "5", "0.00", "0", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_17", "17", "85", "510", "23000", "0.05", "1", "0.55", "8", "0.35", "5", "0.05", "1", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_18", "18", "90", "540", "24500", "0.00", "0", "0.55", "8", "0.40", "6", "0.05", "1", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_19", "19", "95", "570", "26000", "0.00", "0", "0.50", "8", "0.40", "6", "0.10", "2", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_20", "20", "100", "600", "27500", "0.40", "6", "0.60", "9", "0.00", "0", "0.00", "0", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_21", "21", "105", "630", "29000", "0.05", "1", "0.60", "9", "0.35", "5", "0.00", "0", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_22", "22", "110", "660", "30500", "0.00", "0", "0.50", "8", "0.45", "7", "0.05", "1", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_23", "23", "115", "690", "32000", "0.00", "0", "0.45", "7", "0.45", "7", "0.10", "2", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_24", "24", "120", "720", "33500", "0.00", "0", "0.40", "6", "0.40", "6", "0.20", "3", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_25", "25", "125", "750", "35000", "0.40", "6", "0.60", "9", "0.00", "0", "0.00", "0", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_26", "26", "130", "780", "36500", "0.00", "0", "0.60", "9", "0.40", "6", "0.00", "0", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_27", "27", "135", "810", "38000", "0.00", "0", "0.50", "8", "0.45", "7", "0.05", "1", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_28", "28", "140", "840", "39500", "0.00", "0", "0.40", "6", "0.40", "6", "0.20", "3", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_29", "29", "145", "870", "41000", "0.00", "0", "0.30", "5", "0.40", "6", "0.30", "5", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_30", "30", "150", "900", "42500", "0.40", "6", "0.60", "9", "0.00", "0", "0.00", "0", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_31", "31", "155", "930", "44000", "0.00", "0", "0.50", "8", "0.45", "7", "0.05", "1", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_32", "32", "160", "960", "45500", "0.00", "0", "0.40", "6", "0.50", "8", "0.10", "2", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_33", "33", "165", "990", "47000", "0.00", "0", "0.25", "4", "0.50", "8", "0.25", "4", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_34", "34", "170", "1020", "48500", "0.00", "0", "0.10", "2", "0.50", "8", "0.40", "6", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_35", "35", "175", "1050", "50000", "0.40", "6", "0.60", "9", "0.00", "0", "0.00", "0", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_36", "36", "180", "1080", "51500", "0.00", "0", "0.50", "8", "0.40", "6", "0.10", "2", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_37", "37", "185", "1110", "53000", "0.00", "0", "0.40", "6", "0.40", "6", "0.20", "3", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_38", "38", "190", "1140", "54500", "0.00", "0", "0.25", "4", "0.40", "6", "0.35", "5", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_39", "39", "195", "1170", "56000", "0.00", "0", "0.10", "2", "0.40", "6", "0.50", "8", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_40", "40", "200", "1200", "57500", "0.40", "6", "0.60", "9", "0.00", "0", "0.00", "0", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_41", "41", "205", "1230", "59000", "0.00", "0", "0.20", "3", "0.50", "8", "0.30", "5", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_42", "42", "210", "1260", "60500", "0.00", "0", "0.20", "3", "0.40", "6", "0.40", "6", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_43", "43", "215", "1290", "62000", "0.00", "0", "0.15", "2", "0.35", "5", "0.50", "8", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_44", "44", "220", "1320", "63500", "0.00", "0", "0.05", "1", "0.35", "5", "0.60", "9", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_45", "45", "225", "1350", "65000", "0.40", "6", "0.60", "9", "0.00", "0", "0.00", "0", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_46", "46", "230", "1380", "66500", "0.00", "0", "0.20", "3", "0.40", "6", "0.40", "6", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_47", "47", "235", "1410", "68000", "0.00", "0", "0.15", "2", "0.35", "5", "0.50", "8", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_48", "48", "240", "1440", "69500", "0.00", "0", "0.05", "1", "0.35", "5", "0.60", "9", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_49", "49", "245", "1470", "71000", "0.00", "0", "0.00", "0", "0.30", "5", "0.70", "11", "15", "2"));
			Rows.Add( new TrafficDesignRow("Level_50", "50", "250", "1500", "72500", "0.40", "6", "0.60", "9", "0.00", "0", "0.00", "0", "15", "2"));
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
		public TrafficDesignRow GetRow(rowIds in_RowID)
		{
			TrafficDesignRow ret = null;
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
		public TrafficDesignRow GetRow(string in_RowString)
		{
			TrafficDesignRow ret = null;
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
