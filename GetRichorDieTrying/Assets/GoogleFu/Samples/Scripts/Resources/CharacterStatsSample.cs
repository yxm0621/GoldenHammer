//----------------------------------------------
//    GoogleFu: Google Doc Unity integration
//         Copyright Â© 2013 Litteratus
//
//        This file has been auto-generated
//              Do not manually edit
//----------------------------------------------

using UnityEngine;

namespace GoogleFuSample
{
	[System.Serializable]
	public class CharacterStatsSampleRow 
	{
		public string _NAME;
		public int _LEVEL;
		public float _BASEMODIFIER;
		public string _ARCHETYPE;
		public int _STRENGTH;
		public int _ENDURANCE;
		public int _INTELLIGENCE;
		public int _DEXTERITY;
		public int _HEALTH;
		public int _MANA;
		public int _SPEED;
		public bool _UNLOCKED;
		public char _STARS;
		public CharacterStatsSampleRow(string __NAME, string __LEVEL, string __BASEMODIFIER, string __ARCHETYPE, string __STRENGTH, string __ENDURANCE, string __INTELLIGENCE, string __DEXTERITY, string __HEALTH, string __MANA, string __SPEED, string __UNLOCKED, string __STARS) 
		{
			_NAME = __NAME.Trim();
			{
			int res;
				if(int.TryParse(__LEVEL, out res))
					_LEVEL = res;
				else
					Debug.LogError("Failed To Convert LEVEL string: "+ __LEVEL +" to int");
			}
			{
			float res;
				if(float.TryParse(__BASEMODIFIER, out res))
					_BASEMODIFIER = res;
				else
					Debug.LogError("Failed To Convert BASEMODIFIER string: "+ __BASEMODIFIER +" to float");
			}
			_ARCHETYPE = __ARCHETYPE.Trim();
			{
			int res;
				if(int.TryParse(__STRENGTH, out res))
					_STRENGTH = res;
				else
					Debug.LogError("Failed To Convert STRENGTH string: "+ __STRENGTH +" to int");
			}
			{
			int res;
				if(int.TryParse(__ENDURANCE, out res))
					_ENDURANCE = res;
				else
					Debug.LogError("Failed To Convert ENDURANCE string: "+ __ENDURANCE +" to int");
			}
			{
			int res;
				if(int.TryParse(__INTELLIGENCE, out res))
					_INTELLIGENCE = res;
				else
					Debug.LogError("Failed To Convert INTELLIGENCE string: "+ __INTELLIGENCE +" to int");
			}
			{
			int res;
				if(int.TryParse(__DEXTERITY, out res))
					_DEXTERITY = res;
				else
					Debug.LogError("Failed To Convert DEXTERITY string: "+ __DEXTERITY +" to int");
			}
			{
			int res;
				if(int.TryParse(__HEALTH, out res))
					_HEALTH = res;
				else
					Debug.LogError("Failed To Convert HEALTH string: "+ __HEALTH +" to int");
			}
			{
			int res;
				if(int.TryParse(__MANA, out res))
					_MANA = res;
				else
					Debug.LogError("Failed To Convert MANA string: "+ __MANA +" to int");
			}
			{
			int res;
				if(int.TryParse(__SPEED, out res))
					_SPEED = res;
				else
					Debug.LogError("Failed To Convert SPEED string: "+ __SPEED +" to int");
			}
			{
			bool res;
				if(bool.TryParse(__UNLOCKED, out res))
					_UNLOCKED = res;
				else
					Debug.LogError("Failed To Convert UNLOCKED string: "+ __UNLOCKED +" to bool");
			}
			{
			char res;
				if(char.TryParse(__STARS, out res))
					_STARS = res;
				else
					Debug.LogError("Failed To Convert STARS string: "+ __STARS +" to char");
			}
		}

		public int Length { get { return 13; } }

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
					ret = _NAME.ToString();
					break;
				case 1:
					ret = _LEVEL.ToString();
					break;
				case 2:
					ret = _BASEMODIFIER.ToString();
					break;
				case 3:
					ret = _ARCHETYPE.ToString();
					break;
				case 4:
					ret = _STRENGTH.ToString();
					break;
				case 5:
					ret = _ENDURANCE.ToString();
					break;
				case 6:
					ret = _INTELLIGENCE.ToString();
					break;
				case 7:
					ret = _DEXTERITY.ToString();
					break;
				case 8:
					ret = _HEALTH.ToString();
					break;
				case 9:
					ret = _MANA.ToString();
					break;
				case 10:
					ret = _SPEED.ToString();
					break;
				case 11:
					ret = _UNLOCKED.ToString();
					break;
				case 12:
					ret = _STARS.ToString();
					break;
			}

			return ret;
		}

		public string GetStringData( string colID )
		{
			string ret = System.String.Empty;
			switch( colID.ToUpper() )
			{
				case "NAME":
					ret = _NAME.ToString();
					break;
				case "LEVEL":
					ret = _LEVEL.ToString();
					break;
				case "BASEMODIFIER":
					ret = _BASEMODIFIER.ToString();
					break;
				case "ARCHETYPE":
					ret = _ARCHETYPE.ToString();
					break;
				case "STRENGTH":
					ret = _STRENGTH.ToString();
					break;
				case "ENDURANCE":
					ret = _ENDURANCE.ToString();
					break;
				case "INTELLIGENCE":
					ret = _INTELLIGENCE.ToString();
					break;
				case "DEXTERITY":
					ret = _DEXTERITY.ToString();
					break;
				case "HEALTH":
					ret = _HEALTH.ToString();
					break;
				case "MANA":
					ret = _MANA.ToString();
					break;
				case "SPEED":
					ret = _SPEED.ToString();
					break;
				case "UNLOCKED":
					ret = _UNLOCKED.ToString();
					break;
				case "STARS":
					ret = _STARS.ToString();
					break;
			}

			return ret;
		}
		public override string ToString()
		{
			string ret = System.String.Empty;
			ret += "{" + "NAME" + " : " + _NAME.ToString() + "} ";
			ret += "{" + "LEVEL" + " : " + _LEVEL.ToString() + "} ";
			ret += "{" + "BASEMODIFIER" + " : " + _BASEMODIFIER.ToString() + "} ";
			ret += "{" + "ARCHETYPE" + " : " + _ARCHETYPE.ToString() + "} ";
			ret += "{" + "STRENGTH" + " : " + _STRENGTH.ToString() + "} ";
			ret += "{" + "ENDURANCE" + " : " + _ENDURANCE.ToString() + "} ";
			ret += "{" + "INTELLIGENCE" + " : " + _INTELLIGENCE.ToString() + "} ";
			ret += "{" + "DEXTERITY" + " : " + _DEXTERITY.ToString() + "} ";
			ret += "{" + "HEALTH" + " : " + _HEALTH.ToString() + "} ";
			ret += "{" + "MANA" + " : " + _MANA.ToString() + "} ";
			ret += "{" + "SPEED" + " : " + _SPEED.ToString() + "} ";
			ret += "{" + "UNLOCKED" + " : " + _UNLOCKED.ToString() + "} ";
			ret += "{" + "STARS" + " : " + _STARS.ToString() + "} ";
			return ret;
		}
	}
	public class CharacterStatsSample : GoogleFu.GoogleFuComponentBase
	{
		public enum rowIds {
			AI_GOBLIN, AI_ORC, AI_TROLL, AI_DEATH_KNIGHT
		};
		public string [] rowNames = {
			"AI_GOBLIN", "AI_ORC", "AI_TROLL", "AI_DEATH_KNIGHT"
		};
		public System.Collections.Generic.List<CharacterStatsSampleRow> Rows = new System.Collections.Generic.List<CharacterStatsSampleRow>();

		void Awake()
		{
			DontDestroyOnLoad(this);
		}
		public override void AddRowGeneric (System.Collections.Generic.List<string> input)
		{
			Rows.Add(new CharacterStatsSampleRow(input[0],input[1],input[2],input[3],input[4],input[5],input[6],input[7],input[8],input[9],input[10],input[11],input[12]));
		}
		public override void Clear ()
		{
			Rows.Clear();
		}
		public CharacterStatsSampleRow GetRow(rowIds rowID)
		{
			CharacterStatsSampleRow ret = null;
			try
			{
				ret = Rows[(int)rowID];
			}
			catch( System.Collections.Generic.KeyNotFoundException ex )
			{
				Debug.LogError( rowID + " not found: " + ex.Message );
			}
			return ret;
		}
		public CharacterStatsSampleRow GetRow(string rowString)
		{
			CharacterStatsSampleRow ret = null;
			try
			{
				ret = Rows[(int)System.Enum.Parse(typeof(rowIds), rowString)];
			}
			catch(System.ArgumentException) {
				Debug.LogError( rowString + " is not a member of the rowIds enumeration.");
			}
			return ret;
		}

	}

}
