using UnityEngine;
using UnityEditor;

namespace GoogleFuSample
{
	[CustomEditor(typeof(CharacterStatsSample))]
	public class CharacterStatsSampleEditor : Editor
	{
		public int Index = 0;
		public override void OnInspectorGUI ()
		{
			CharacterStatsSample s = target as CharacterStatsSample;
			CharacterStatsSampleRow r = s.Rows[ Index ];

			EditorGUILayout.BeginHorizontal();
			if ( GUILayout.Button("<<") )
			{
				Index = 0;
			}
			if ( GUILayout.Button("<") )
			{
				Index -= 1;
				if ( Index < 0 )
					Index = s.Rows.Count - 1;
			}
			if ( GUILayout.Button(">") )
			{
				Index += 1;
				if ( Index >= s.Rows.Count )
					Index = 0;
			}
			if ( GUILayout.Button(">>") )
			{
				Index = s.Rows.Count - 1;
			}

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "ID", GUILayout.Width( 150.0f ) );
			{
				EditorGUILayout.LabelField( s.rowNames[ Index ] );
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "NAME", GUILayout.Width( 150.0f ) );
			{
				EditorGUILayout.TextField( r._NAME );
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "LEVEL", GUILayout.Width( 150.0f ) );
			{
				EditorGUILayout.IntField( r._LEVEL );
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "BASEMODIFIER", GUILayout.Width( 150.0f ) );
			{
				EditorGUILayout.FloatField( (float)r._BASEMODIFIER );
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "ARCHETYPE", GUILayout.Width( 150.0f ) );
			{
				EditorGUILayout.TextField( r._ARCHETYPE );
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "STRENGTH", GUILayout.Width( 150.0f ) );
			{
				EditorGUILayout.IntField( r._STRENGTH );
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "ENDURANCE", GUILayout.Width( 150.0f ) );
			{
				EditorGUILayout.IntField( r._ENDURANCE );
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "INTELLIGENCE", GUILayout.Width( 150.0f ) );
			{
				EditorGUILayout.IntField( r._INTELLIGENCE );
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "DEXTERITY", GUILayout.Width( 150.0f ) );
			{
				EditorGUILayout.IntField( r._DEXTERITY );
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "HEALTH", GUILayout.Width( 150.0f ) );
			{
				EditorGUILayout.IntField( r._HEALTH );
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "MANA", GUILayout.Width( 150.0f ) );
			{
				EditorGUILayout.IntField( r._MANA );
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "SPEED", GUILayout.Width( 150.0f ) );
			{
				EditorGUILayout.IntField( r._SPEED );
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "UNLOCKED", GUILayout.Width( 150.0f ) );
			{
				EditorGUILayout.Toggle( System.Convert.ToBoolean( r._UNLOCKED ) );
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "STARS", GUILayout.Width( 150.0f ) );
			{
				EditorGUILayout.TextField( System.Convert.ToString( r._STARS ) );
			}
			EditorGUILayout.EndHorizontal();

		}
	}
}
