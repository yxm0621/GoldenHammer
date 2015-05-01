//----------------------------------------------
//    GoogleFu: Google Doc Unity integration
//         Copyright Â© 2013 Litteratus
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System.Linq;

namespace GoogleFu
{
    public partial class GoogleFuEditor : EditorWindow
    {
        public static string UpdateMessage = string.Empty;

        public static void CheckForUpdate()
        {
            try
            {
                var ma = 1;
                var mi = 0;
                var bu = 15;

                var request =
                    System.Net.WebRequest.Create("http://www.litteratus.net/CheckUpdate.php?ma=" + ma + "&mi=" + mi + "&bu=" + bu);
                var response = request.GetResponse();
                var data = response.GetResponseStream();
                string html;
                if (data == null) return;
                using (var sr = new System.IO.StreamReader(data))
                {
                    html = sr.ReadToEnd();
                }

                UpdateMessage = html;
            }
            catch (System.Exception)
            {
                UpdateMessage = "Unable to check for updates. Try again later.";
            }

        }

        // Add menu item named "GoogleFu" to the Window menu
        [MenuItem("Window/GoogleFu")]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            EditorWindow googleFuWindow = GetWindow(typeof(GoogleFuEditor));
            googleFuWindow.title = "GoogleFu";

            var lastChecked = System.Convert.ToDateTime(EditorPrefs.GetString("GoogleFuLastCheckedForUpdate", System.Convert.ToString(System.DateTime.MinValue)));
            var diff = (System.DateTime.Now - lastChecked).Days;
            if (diff <= 1) return;
            EditorPrefs.SetString("GoogleFuLastCheckedForUpdate", System.Convert.ToString(System.DateTime.Now));

            var t = new System.Threading.Thread(CheckForUpdate);
            t.Start();
        }

        void OnEnable()
        {
            EditorApplication.update += Update;
        }

        void Init()
        {
            _Authorized = false;

            _UnityLogo = (Texture2D)Resources.Load("Pwrd_By_Unity_Pri_In_sm", typeof(Texture2D));
            _LitteratusLogo = (Texture2D)Resources.Load("Litteratus_Logo_sm", typeof(Texture2D));
            _Separator = (Texture2D)Resources.Load("separator", typeof(Texture2D));
            _HelpButton = (Texture2D)Resources.Load("help", typeof(Texture2D));
            _BrowseButton = (Texture2D)Resources.Load("folder", typeof(Texture2D));

            _Username = GetString("username", _Username);
            _Password = GetString("password", _Password);
            _ActiveWorkbookname = GetString("activeworkbookname", _ActiveWorkbookname);
            _ObjDbResourcesDirectory = GetString("objDBResourcesDirectory", _ObjDbResourcesDirectory);
            _ObjDbEditorDirectory = GetString("objDBEditorDirectory", _ObjDbEditorDirectory);
            _StaticDbResourcesDirectory = GetString("staticDBResourcesDirectory", _StaticDbResourcesDirectory);
            _NguiDirectory = GetString("nguiDirectory", _NguiDirectory);
            _XmlDirectory = GetString("xmlDirectory", _XmlDirectory);
            _JsonDirectory = GetString("jsonDirectory", _JsonDirectory);
            _CsvDirectory = GetString("csvDirectory", _CsvDirectory);
            _PlaymakerDirectory = GetString("playmakerDirectory", _PlaymakerDirectory);
            _DaikonforgeDirectory = GetString("daikonforgeDirectory", _DaikonforgeDirectory);
            _EditorLanguage = GetString("editorLanguage", _EditorLanguage);
            if (Language.GetLanguageCode(_EditorLanguage) == Language.Code.INVALID)
            {
                _EditorLanguage = "en";
                SetString("editorLanguage", _EditorLanguage);
            }
            _SaveCredentials = GetBool("saveCredientials", _SaveCredentials);
            _AutoLogin = GetBool("autoLogin", _AutoLogin);
            _TemporaryAutoLogin = GetBool("temporaryAutoLogin", _TemporaryAutoLogin);
            _UseObjDb = GetBool("useObjDB", _UseObjDb);
            _UseStaticDb = GetBool("useStaticDB", _UseStaticDb);
            _UseNgui = GetBool("useNGUI", _UseNgui);
            _UseNguiLegacy = GetBool("useNGUILegacy", _UseNguiLegacy);
            _UseXml = GetBool("useXML", _UseXml);
            _UseJson = GetBool("useJSON", _UseJson);
            _UseCsv = GetBool("useCSV", _UseCsv);
            _UseDaikonForge = GetBool("useDaikonForge", _UseDaikonForge);
            _UsePlaymaker = GetBool("usePlaymaker", _UsePlaymaker);
            _LanguagesIndex = GetInt("languagesIndex", _LanguagesIndex);
            _ProjectPath = Application.dataPath;
            _CurrentHelpDoc = string.Empty;

            _ArrayDelimiters = GetString("arrayDelimiters", _ArrayDelimiters);
            _StringArrayDelimiters = GetString("stringArrayDelimiters", _StringArrayDelimiters);
            _ComplexTypeDelimiters = GetString("complexTypeDelimiters", _ComplexTypeDelimiters);
            _ComplexTypeArrayDelimiters = GetString("complexTypeArrayDelimiters", _ComplexTypeArrayDelimiters);

            _TrimStrings = GetBool("trimStrings", _TrimStrings);
            _TrimStringArrays = GetBool("trimStringArrays", _TrimStringArrays);

            System.Net.ServicePointManager.ServerCertificateValidationCallback = Validator;
            _Service = new Google.GData.Spreadsheets.SpreadsheetsService("UnityGoogleFu");
            _AuthenticateTick = -1;

            if (System.IO.Directory.GetFiles(Application.dataPath, "NGUITools.cs", System.IO.SearchOption.AllDirectories).Length > 0)
                _FoundNgui = true;

            if (System.IO.Directory.GetFiles(Application.dataPath, "PlayMaker.dll", System.IO.SearchOption.AllDirectories).Length > 0)
                _FoundPlaymaker = true;

            if (System.IO.Directory.GetFiles(Application.dataPath, "dfScriptLite.dll", System.IO.SearchOption.AllDirectories).Length > 0)
                _FoundDaikonForge = true;

            if (_AutoLogin || _TemporaryAutoLogin)
            {
                SetBool("temporaryAutoLogin", false);
                DoRefreshWorkbooks = true;
            }
            else
            {
                _Workbooks.Clear();
                var tmpManualWorkbooks = GetString("manualworkbookurls", System.String.Empty);
                var splitManualWorkbooks = tmpManualWorkbooks.Split(new[] { '|' }, System.StringSplitOptions.RemoveEmptyEntries);
                foreach (var info in splitManualWorkbooks.Select(in_s => AddManualWorkbookByUrl(in_s)).Where(in_info => in_info != null && GfuStrCmp(in_info.Title, _ActiveWorkbookname)))
                {
                    SetActiveWorkbook(info);
                }
            }
        }

        private int _Count;

        void Update()
        {
            if (!string.IsNullOrEmpty(UpdateMessage))
            {
                Debug.Log(UpdateMessage);
                UpdateMessage = string.Empty;
            }

            if (DoRefreshWorkbooks)
            {
                _EditorWorking = Localize("ID_AUTHENTICATING");
                if (_AuthenticateTick == -1)
                    _AuthenticateTick = 4;

                if (_AuthenticateTick == 0)
                {
                    DoRefreshWorkbooks = false;
                    RefreshWorkbooks();
                    _EditorWorking = System.String.Empty;
                }
                _AuthenticateTick--;
                Repaint();
            }

            if (EditorApplication.isCompiling)
            {
                return;
            }
            if (_AdvancedDatabaseObjectInfo.Count <= 0)
            {
                return;
            }

            foreach (var info in _AdvancedDatabaseObjectInfo)
            {
                if (info == null)
                    continue;

                if (string.IsNullOrEmpty(info.ComponentName))
                    continue;

                var toDestroy = info.DatabaseAttachObject.GetComponent(info.ComponentName);
                if (toDestroy != null)
                    DestroyImmediate(toDestroy);

#if UNITY_5
                var myType = GetAssemblyType("GoogleFu." + info.ComponentName);
                var comp = (GoogleFuComponentBase)info.DatabaseAttachObject.AddComponent(myType);
#else
                var comp = (GoogleFuComponentBase)info.DatabaseAttachObject.AddComponent(info.ComponentName);
#endif
                

                if (comp == null)
                {
                    AssetDatabase.ImportAsset(info.ComponentName + ".cs", ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);

                    if (_Count < 50)
                    {
                        _Count++;
                        return;
                    }
                    Debug.LogError("Could not add GoogleFu component base " + info.ComponentName + ".cs");
                    continue;
                }

                var rowInputs = new System.Collections.Generic.List<string>();

                _Count = 0;

                foreach (var entryVal in info.EntryStrings)
                {

                    rowInputs.Add(entryVal);
                    _Count++;

                    if (_Count % info.EntryStride != 0) continue;
                    comp.AddRowGeneric(rowInputs);
                    rowInputs.Clear();
                }

            }

            _AdvancedDatabaseObjectInfo.Clear();
        }

        public static System.Type GetAssemblyType(string typeName)
        {
            var type = System.Type.GetType(typeName);
            if (type != null) return type;
            foreach (var a in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                type = a.GetType(typeName);
                if (type != null) return type;
            }
            return null;
        }

        void ClearMessages()
        {
            _EditorInfo = System.String.Empty;
            _EditorWarning = System.String.Empty;
            _EditorWorking = System.String.Empty;
            _EditorPathInfo = System.String.Empty;
            EditorException = System.String.Empty;
        }

        void SetString(string stringID, string input)
        {
            EditorPrefs.SetString(System.IO.Path.Combine(Application.dataPath, stringID), input);
        }

        string GetString(string stringID, string defaultString)
        {
            return EditorPrefs.GetString(System.IO.Path.Combine(Application.dataPath, stringID), defaultString);
        }

        void SetInt(string intID, int input)
        {
            EditorPrefs.SetInt(System.IO.Path.Combine(Application.dataPath, intID), input);
        }

        int GetInt(string intID, int defaultInt)
        {
            return EditorPrefs.GetInt(System.IO.Path.Combine(Application.dataPath, intID), defaultInt);
        }

        void SetFloat(string floatID, float input)
        {
            EditorPrefs.SetFloat(System.IO.Path.Combine(Application.dataPath, floatID), input);
        }

        float GetFloat(string floatID, float defaultFloat)
        {
            return EditorPrefs.GetFloat(System.IO.Path.Combine(Application.dataPath, floatID), defaultFloat);
        }

        bool SetBool(string boolID, bool input)
        {
            EditorPrefs.SetBool(System.IO.Path.Combine(Application.dataPath, boolID), input);
            return input;
        }

        bool GetBool(string boolID, bool defaultBool)
        {
            return EditorPrefs.GetBool(System.IO.Path.Combine(Application.dataPath, boolID), defaultBool);
        }

        string Localize(string textID)
        {
            LocalizationRow row = Localization.Instance.GetRow(textID);
            if (row != null)
            {
                return row.GetStringData(_EditorLanguage);
            }
            return "Unable to find string ID: " + textID;

        }

        bool DrawToggle(string textID, bool defaultValue)
        {
            return GUILayout.Toggle(defaultValue, " " + Localize(textID));
        }

        void DrawSeparator()
        {
            GUILayout.Label(_Separator);
        }
    }
}
