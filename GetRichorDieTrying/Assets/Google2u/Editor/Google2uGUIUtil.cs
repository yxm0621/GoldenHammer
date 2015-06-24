//----------------------------------------------
//    Google2u: Google Doc Unity integration
//         Copyright © 2015 Litteratus
//----------------------------------------------

using System;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

namespace Google2u
{
    public class Google2uGUIUtil
    {
        public static string SetString(string in_stringID, string in_input)
        {
            EditorPrefs.SetString(System.IO.Path.Combine(Application.dataPath, in_stringID), in_input);
            return in_input;
        }

        public static string GetString(string in_stringID, string in_defaultString)
        {
            return EditorPrefs.GetString(System.IO.Path.Combine(Application.dataPath, in_stringID), in_defaultString);
        }

        public static int SetInt(string in_intID, int in_input)
        {
            EditorPrefs.SetInt(System.IO.Path.Combine(Application.dataPath, in_intID), in_input);
            return in_input;
        }

        public static int GetInt(string in_intID, int in_defaultInt)
        {
            return EditorPrefs.GetInt(System.IO.Path.Combine(Application.dataPath, in_intID), in_defaultInt);
        }

        public static float SetFloat(string in_floatID, float in_input)
        {
            EditorPrefs.SetFloat(System.IO.Path.Combine(Application.dataPath, in_floatID), in_input);
            return in_input;
        }

        public static float GetFloat(string in_floatID, float in_defaultFloat)
        {
            return EditorPrefs.GetFloat(System.IO.Path.Combine(Application.dataPath, in_floatID), in_defaultFloat);
        }

        public static bool SetBool(string in_boolID, bool in_input)
        {
            EditorPrefs.SetBool(System.IO.Path.Combine(Application.dataPath, in_boolID), in_input);
            return in_input;
        }

        public static bool GetBool(string in_boolID, bool in_defaultBool)
        {
            return EditorPrefs.GetBool(System.IO.Path.Combine(Application.dataPath, in_boolID), in_defaultBool);
        }


        public static T GetEnum<T>(string in_enumID, T in_defaultEnum)
        {
            var inEnumAsString = in_defaultEnum.ToString();
            var retEnumAsString = EditorPrefs.GetString(System.IO.Path.Combine(Application.dataPath, in_enumID), inEnumAsString);
            if (string.IsNullOrEmpty(retEnumAsString))
                return in_defaultEnum;
            return (T)Enum.Parse(typeof(T), retEnumAsString);
        }

        public static T SetEnum<T>(string in_enumID, T in_input)
        {
            var enumAsString = Enum.GetName(typeof(T), in_input);
            EditorPrefs.SetString(System.IO.Path.Combine(Application.dataPath, in_enumID), enumAsString);
            return in_input;
        }

        public static bool GfuStrCmp(string in_1, string in_2)
        {
            // There is a special case for in_2 being "void"
            if (!in_2.Equals("void", System.StringComparison.InvariantCultureIgnoreCase))
                return (in_1.Equals(in_2, System.StringComparison.InvariantCultureIgnoreCase));

            if (!GfuStartsWith(in_1, "VOID_"))
                return (in_1.Equals(in_2, System.StringComparison.InvariantCultureIgnoreCase));

            var splitColName = in_1.Split(new[] { '_' }, System.StringSplitOptions.RemoveEmptyEntries);
            if (splitColName.Length != 2)
                return (in_1.Equals(in_2, System.StringComparison.InvariantCultureIgnoreCase));

            int resInt;
            return int.TryParse(splitColName[1], out resInt);
        }

        public static bool GfuStartsWith(string in_1, string in_2)
        {
            return (in_1.StartsWith(in_2, System.StringComparison.InvariantCultureIgnoreCase));
        }

        public class ComboBox
        {
            private static bool _forceToUnShow;
            private static int _useControlID = -1;
            private bool _IsClickedComboButton;
            private int _SelectedItemIndex;

            private Rect _Rect;
            private GUIContent _ButtonContent;
            private readonly GUIContent[] _ListContent;
            private readonly GUIStyle _ButtonStyle;
            private readonly GUIStyle _BoxStyle;
            private readonly GUIStyle _ListStyle;
            private Vector2 _MyScrollPos = Vector2.zero;

            public bool IsShown
            {
                get { return _IsClickedComboButton; }
            }

            public float x
            {
                get { return _Rect.x; }
                set { _Rect.x = value; }
            }

            public float y
            {
                get { return _Rect.y; }
                set { _Rect.y = value; }
            }
            public float width
            {
                get { return _Rect.width; }
                set { _Rect.width = value; }
            }

            public float height
            {
                get { return _Rect.height; }
                set { _Rect.height = value; }
            }

            public ComboBox(Rect in_rect, GUIContent in_buttonContent, GUIContent[] in_listContent, GUIStyle in_listStyle)
            {
                _Rect = in_rect;
                _ButtonContent = in_buttonContent;
                _ListContent = in_listContent;
                _ButtonStyle = "button";
                _BoxStyle = "box";
                _ListStyle = in_listStyle;

                _SelectedItemIndex = 0;
                foreach (var c in in_listContent)
                {
                    if (c == in_buttonContent)
                        break;
                    _SelectedItemIndex++;
                }
                if (_SelectedItemIndex >= in_listContent.Length)
                    _SelectedItemIndex = 0;
            }

            public ComboBox(Rect in_rect, GUIContent in_buttonContent, GUIContent[] in_listContent, GUIStyle in_buttonStyle, GUIStyle in_boxStyle, GUIStyle in_listStyle)
            {
               _Rect = in_rect;
               _ButtonContent = in_buttonContent;
               _ListContent = in_listContent;
               _ButtonStyle = in_buttonStyle;
               _BoxStyle = in_boxStyle;
               _ListStyle = in_listStyle;

                _SelectedItemIndex = 0;
                foreach (var c in in_listContent)
                {
                    if (c == in_buttonContent)
                        break;
                    _SelectedItemIndex++;
                }
                if (_SelectedItemIndex >= in_listContent.Length)
                    _SelectedItemIndex = 0;
            }

            public int Show()
            {
                if (_forceToUnShow)
                {
                    _forceToUnShow = false;
                    _IsClickedComboButton = false;
                }

                var done = false;
                var controlID = GUIUtility.GetControlID(FocusType.Passive);

                if (Event.current.GetTypeForControl(controlID) == EventType.mouseUp && _IsClickedComboButton)
                    done = true;

                if (GUI.Button(_Rect, _ButtonContent, _ButtonStyle))
                {
                    if (_useControlID == -1)
                    {
                        _useControlID = controlID;
                        _IsClickedComboButton = false;
                    }

                    if (_useControlID != controlID)
                    {
                        _forceToUnShow = true;
                        _useControlID = controlID;
                    }
                    _IsClickedComboButton = true;
                }

                if (_IsClickedComboButton)
                {
                    var boxRect = new Rect(_Rect.x, _Rect.y + _ListStyle.CalcHeight(_ListContent[0], 1.0f),
                        _Rect.width, 100.0f);

                    var listRect = new Rect(0f, 0f,
                        _Rect.width, _ListStyle.CalcHeight(_ListContent[0], 1.0f) * _ListContent.Length);

                    GUI.Box(boxRect, "", _BoxStyle);

                    _MyScrollPos = GUI.BeginScrollView(boxRect, _MyScrollPos, listRect, false, true);

                    var newSelectedItemIndex = GUI.SelectionGrid(listRect, _SelectedItemIndex, _ListContent, 1, _ListStyle);
                    if (newSelectedItemIndex != _SelectedItemIndex)
                    {
                        _SelectedItemIndex = newSelectedItemIndex;
                        _ButtonContent = _ListContent[_SelectedItemIndex];
                    }

                    GUI.EndScrollView();

                }

                if (done)
                    _IsClickedComboButton = false;

                return _SelectedItemIndex;
            }

            public int SelectedItemIndex
            {
                get
                {
                    return _SelectedItemIndex;
                }
                set
                {
                    _SelectedItemIndex = value;
                }
            }
        }
    }
}