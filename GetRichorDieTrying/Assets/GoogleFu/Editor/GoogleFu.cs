//----------------------------------------------
//    GoogleFu: Google Doc Unity integration
//         Copyright © 2015 Litteratus
//----------------------------------------------

using System.Linq;
using Google.GData.Client;
using UnityEngine;
using UnityEditor;

namespace GoogleFu
{

    public partial class GoogleFuEditor : EditorWindow
    {
        public static string EditorException = System.String.Empty;
        public static bool DoRefreshWorkbooks = false;
        public static bool CreatingWorkbook = false;
        public static bool ShowUploadedNotification = false;
        private static int _Anoncolname;

        private Google.GData.Spreadsheets.SpreadsheetsService _Service;
        private readonly Google.GData.Spreadsheets.SpreadsheetQuery _Query = new Google.GData.Spreadsheets.SpreadsheetQuery();
        private WorkBookInfo _ActiveWorkbook;
        private System.Collections.Generic.List<WorkBookInfo> _Workbooks = new System.Collections.Generic.List<WorkBookInfo>();
        private System.Collections.Generic.List<WorkBookInfo> _Manualworkbooks = new System.Collections.Generic.List<WorkBookInfo>();
        private System.Collections.Generic.List<string> _Manualworkbookurls = new System.Collections.Generic.List<string>();
        private System.Collections.Generic.List<AdvancedDatabaseInfo> _AdvancedDatabaseObjectInfo = new System.Collections.Generic.List<AdvancedDatabaseInfo>();
        private System.Collections.Generic.Dictionary<string, GameObject> _AdvancedDatabaseObjects = new System.Collections.Generic.Dictionary<string, GameObject>();
        private System.Collections.Generic.Dictionary<string, bool> _ShowHelp = new System.Collections.Generic.Dictionary<string, bool>();
        private GfPage _CurrentPage = GfPage.Settings;
        private bool _Authorized;
        private Vector2 _ScrollPos = new Vector2(0.0f, 0.0f);
        private Color _DefaultBgColor;
        private Color _DefaultFgColor;
        private Color _LabelHeaderColor;
        private Color _SelectedTabColor;
        private Color _UnselectedTabColor;
        private Color _PathLabelBgColor;
        private Color _PathLabelFgColor;
        private GUIStyle _LargeLabelStyle;
        private GUIStyle _PathLabelStyle;
        private Texture2D _PathLabelBg;
        private Texture _UnityLogo;
        private Texture _LitteratusLogo;
        private Texture _Separator;
        private Texture _HelpButton;
        private Texture _BrowseButton;
        private string _Username;
        private string _Password;
        private string _EditorInfo;
        private string _EditorWarning;
        private string _EditorWorking;
        private string _EditorPathInfo;
        private string _Manualurl;
        private string _CurrentHelpDoc;
        private string _EditorLanguage = "en";
        private string _ObjDbResourcesDirectory;
        private string _ObjDbEditorDirectory;
        private string _StaticDbResourcesDirectory;
        private string _NguiDirectory;
        private string _DaikonforgeDirectory;
        private string _XmlDirectory;
        private string _JsonDirectory;
        private string _CsvDirectory;
        private string _PlaymakerDirectory;
        private string _ActiveWorkbookname;
        private string _WorkbookUploadPath;
        private string _ProjectPath;
        private int _InitDraw;
        private int _AuthenticateTick = -1;
        private int _LanguagesIndex = 8;
        private int _CurIndent;
        private bool _SaveCredentials;
        private bool _AutoLogin;
        private bool _TemporaryAutoLogin;
        private bool _UseObjDb;
        private bool _UseStaticDb;
        private bool _UseNgui;
        private bool _UseNguiLegacy;
        private bool _UseDaikonForge;
        private bool _UseXml;
        private bool _UseJson;
        private bool _UseCsv;
        private bool _UsePlaymaker;
        private bool _IsProSkin;

        private bool _FoundPlaymaker;
        private bool _FoundNgui;
        private bool _FoundDaikonForge;

        // Settings GUI
        private bool _BShowAuthentication = true;
        private bool _BShowLanguage;
        private bool _BShowPaths;

        // Workbooks GUI
        private bool _BShowAccountWorkbooks = true;
        private bool _BShowManualWorkbooks;
        private bool _BShowCreateWorkbook;

        // Tools GUI
        private string _ArrayDelimiters = ", ";
        private string _StringArrayDelimiters = "|";
        private string _ComplexTypeDelimiters = ", ";
        private string _ComplexTypeArrayDelimiters = "|";
        private bool _TrimStrings;
        private bool _TrimStringArrays;

        // Help GUI
        private bool _BShowMain = true;
        private bool _BShowDocs;

        public static bool Validator(object in_sender, System.Security.Cryptography.X509Certificates.X509Certificate in_certificate, System.Security.Cryptography.X509Certificates.X509Chain in_chain,
                                      System.Net.Security.SslPolicyErrors in_sslPolicyErrors)
        {
            return true;
        }

        void SetActiveWorkbook(WorkBookInfo in_workBook)
        {
            _ActiveWorkbook = in_workBook;

            if(_ActiveWorkbook == null)
                return;
            if (_Service == null)
                return;

            var feed = _Service.Query(new Google.GData.Spreadsheets.SpreadsheetQuery());
            foreach (var entry in feed.Entries.Cast<Google.GData.Spreadsheets.SpreadsheetEntry>().Where(in_entry => in_entry.Title.Text.Equals(in_workBook.Title)))
            {
                _ActiveWorkbook.SetupFromSpreadsheetEntry(entry);
                _CurrentPage = GfPage.Toolbox;
            }

        }

        void RefreshWorkbooks()
        {
            if (!string.IsNullOrEmpty(_Username) && !string.IsNullOrEmpty(_Password))
            {
                Google.GData.Spreadsheets.SpreadsheetFeed feed = null;
                var doWorkbookQuery = false;
                try
                {
                    _Service.setUserCredentials(_Username, _Password);

                    var savedToken = GetString("AuthToken_" + _Username, string.Empty);

                    if (savedToken != string.Empty)
                    {
                        _Service.SetAuthenticationToken(savedToken);
                    }
                    else
                    {
                        var token = _Service.QueryClientLoginToken();
                        SetString("AuthToken_" + _Username, token);
                        _Service.SetAuthenticationToken(token);
                    }

                    //_Workbooks.Clear();

                    feed = _Service.Query(_Query);

                    if (feed.Entries.Count == 0)
                    {
                        _EditorInfo = Localize("ID_NO_DB_ERROR");
                    }
                    else if(_Workbooks.Count == 0)
                    {
                        doWorkbookQuery = true;
                    }
                    else
                    {
                        foreach (var info in _Workbooks.Where(in_info => GfuStrCmp(in_info.Title, _ActiveWorkbookname)))
                        {
                            SetActiveWorkbook(info);
                        }
                        _Authorized = true;
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.Log(ex.Message);
                    _EditorInfo = Localize("ID_AUTH_ERROR");
                    SetString("AuthToken", string.Empty);
                }

                if (doWorkbookQuery)
                {
                    foreach (var entry in feed.Entries.Cast<Google.GData.Spreadsheets.SpreadsheetEntry>())
                    {
                        try
                        {
                            var info = _Workbooks.Find(
                                in_i => GfuStrCmp(in_i.Title, entry.Feed.Title.Text)
                                );
                            if (info == null)
                            {
                                info = new WorkBookInfo(entry);
                                _Workbooks.Add(info);
                            }
                            if (GfuStrCmp(info.Title, _ActiveWorkbookname))
                                SetActiveWorkbook(info);
                        }
                        catch (System.Exception ex)
                        {
                            Debug.Log(Localize("ID_INACCESSIBLE_ERROR"));
                            Debug.Log(ex.Message);
                        }
                    }

                    _Authorized = true;
                }
            }

            _Manualworkbookurls.Clear();
            var tmpManualWorkbooks = GetString("manualworkbookurls", System.String.Empty);
            var splitManualWorkbooks = tmpManualWorkbooks.Split(new[] { '|' }, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (var info in splitManualWorkbooks.Select(in_s => AddManualWorkbookByUrl(in_s)).Where(in_info => in_info != null && GfuStrCmp(in_info.Title, _ActiveWorkbookname)))
            {
                SetActiveWorkbook(info);
            }
        }

        void DrawAuthenticationGUI()
        {

            if (_Authorized == false)
            {
                EditorGUILayout.BeginHorizontal();
                DrawHelpButtonGUI("ID_HELP_AUTHENTICATION");
                GUI.color = Color.yellow;
                DrawLabelHeader("ID_AUTH_OPTIONAL");
                GUI.color = _DefaultFgColor;
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                DrawHelpButtonGUI("ID_HELP_GOOGLE_USER_NAME");
                string oldUsername = _Username;
                _Username = EditorGUILayout.TextField(Localize("ID_GOOGLE_USERNAME"), _Username);
                if (_Username != oldUsername && _SaveCredentials)
                {
                    SetString("username", _Username);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                DrawHelpButtonGUI("ID_HELP_GOOGLE_PASSWORD");
                string oldPassword = _Password;
                _Password = EditorGUILayout.PasswordField(Localize("ID_GOOGLE_PASSWORD"), _Password);
                if (_Password != oldPassword && _SaveCredentials)
                {
                    SetString("password", _Password);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                DrawHelpButtonGUI("ID_HELP_SAVE_CREDENTIALS");
                GUI.SetNextControlName("SaveCredentials");
                bool oldSaveCredentials = _SaveCredentials;
                _SaveCredentials = DrawToggle("ID_SAVE_CREDENTIALS", _SaveCredentials);
                if (_SaveCredentials != oldSaveCredentials)
                {
                    SetBool("saveCredientials", _SaveCredentials);
                    if (_SaveCredentials == false)
                    {
                        _Username = System.String.Empty;
                        _Password = System.String.Empty;
                        SetString("username", System.String.Empty);
                        SetString("password", System.String.Empty);
                        GUI.FocusControl("SaveCredentials");
                        Repaint();
                    }
                    else
                    {
                        SetString("username", _Username);
                        SetString("password", _Password);
                    }
                }
                if (_SaveCredentials)
                {
                    DrawHelpButtonGUI("ID_HELP_AUTO_LOG_IN");
                    bool oldAutoLogin = _AutoLogin;
                    _AutoLogin = DrawToggle("ID_AUTO_LOGIN", _AutoLogin);
                    if (_AutoLogin != oldAutoLogin)
                    {
                        SetBool("autoLogin", _AutoLogin);
                    }
                }
                else
                {
                    _AutoLogin = false;
                    SetBool("autoLogin", _AutoLogin);
                }
                EditorGUILayout.EndHorizontal();

                if (DoRefreshWorkbooks)
                    GUI.enabled = false;
                if (GUILayout.Button(Localize("ID_AUTHORIZE")))
                {
                    ClearMessages();
                    DoRefreshWorkbooks = true;
                }
                GUI.enabled = true;
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                DrawHelpButtonGUI("ID_HELP_ACTIVE_ACCOUNT");
                DrawLabelHeader("ID_ACTIVE_ACCOUNT");
                GUILayout.Label(_Username, _LargeLabelStyle);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Separator();

                if (GUILayout.Button(Localize("ID_LOGOUT")))
                {
                    ClearMessages();

                    _Authorized = false;
                    _Workbooks.Clear();
                    SetActiveWorkbook(null);
                    _ActiveWorkbookname = System.String.Empty;

                    if (GetBool("saveCredientials", _SaveCredentials) != true)
                    {
                        _Username = System.String.Empty;
                        _Password = System.String.Empty;
                        SetString("username", _Username);
                        SetString("password", _Password);
                    }
                }

            }
        }

        void DrawActiveWorkbookGUI()
        {
            EditorGUILayout.BeginHorizontal();
            DrawHelpButtonGUI("ID_HELP_ACTIVE_WORKBOOK");
            DrawLabelHeader("ID_ACTIVE_WORKBOOK");
            GUILayout.Label(_ActiveWorkbook.Title, _LargeLabelStyle);
            if (GUILayout.Button(Localize("ID_OPEN_URL"), EditorStyles.miniButton, GUILayout.Width(70)))
            {
                ClearMessages();
                Application.OpenURL(_ActiveWorkbook.Url);
            }
            if (GUILayout.Button(Localize("ID_DEACTIVATE"), EditorStyles.miniButton, GUILayout.Width(70)))
            {
                ClearMessages();
                SetActiveWorkbook(null);
                _ActiveWorkbookname = System.String.Empty;
                SetString("activeworkbookname", System.String.Empty);
                _CurrentPage = GfPage.Workbooks;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Separator();
        }

        WorkBookInfo AddManualWorkbookByUrl(string in_manualUrl)
        {
            WorkBookInfo info = null;
            if (string.IsNullOrEmpty(in_manualUrl))
            {
                _EditorInfo = Localize("ID_NO_URL_ERROR");
                return null;
            }

            try
            {
                var key = in_manualUrl.Substring(in_manualUrl.IndexOf("key=", System.StringComparison.InvariantCultureIgnoreCase) + 4);
                key = key.Split(new[] { '&' })[0];

                var singleQuery = new Google.GData.Spreadsheets.WorksheetQuery(key, "public", "values");
                Google.GData.Spreadsheets.WorksheetFeed feed = _Service.Query(singleQuery);
                var finalUrl = in_manualUrl.Split(new[] { '&' })[0];
                if (feed != null)
                {
                    foreach (var atomEntry in feed.Entries)
                    {
                        var entry = (Google.GData.Spreadsheets.WorksheetEntry)atomEntry;
                        info = _Manualworkbooks.Find(
                            in_i => GfuStrCmp(in_i.Url, finalUrl)
                            ) ?? _Workbooks.Find(
                                in_i => GfuStrCmp(in_i.Url, finalUrl)
                                );
                        if (info == null)
                        {
                            info = new WorkBookInfo();

                            _Manualworkbooks.Add(info);
                            if (_Manualworkbookurls.Contains(in_manualUrl) == false)
                                _Manualworkbookurls.Add(in_manualUrl);
                            var newManualWorkbookUrls = _Manualworkbookurls.Aggregate(System.String.Empty, (in_current, in_s) => in_current + (in_s + "|"));
                            SetString("manualworkbookurls", newManualWorkbookUrls);
                        }

                        info.AddWorksheetEntry(entry, finalUrl);
                    }
                }
            }
            catch
            {
                try
                {
                    var key = in_manualUrl.Substring(in_manualUrl.IndexOf("spreadsheets/d/", System.StringComparison.InvariantCultureIgnoreCase) + 15);
                    key = key.Split(new[] { '/' })[0];

                    var singleQuery = new Google.GData.Spreadsheets.WorksheetQuery(key, "public", "values");
                    var feed = _Service.Query(singleQuery);
                    var urlParts = in_manualUrl.Split(new[] { '/' });

                    var finalUrl = "";
                    var urlBuild = 0;
                    string urlPart;
                    do
                    {
                        urlPart = urlParts[urlBuild];
                        finalUrl += urlPart + '/';
                        urlBuild++;
                    } while (urlPart != key);
                    if (feed != null)
                    {
                        foreach (var atomEntry in feed.Entries)
                        {
                            var entry = (Google.GData.Spreadsheets.WorksheetEntry)atomEntry;
                            info = _Manualworkbooks.Find(
                                in_i => GfuStrCmp(in_i.Url, finalUrl)
                                ) ?? _Workbooks.Find(
                                    in_i => GfuStrCmp(in_i.Url, finalUrl)
                                    );
                            if (info == null)
                            {
                                info = new WorkBookInfo();

                                _Manualworkbooks.Add(info);
                                if (_Manualworkbookurls.Contains(in_manualUrl) == false)
                                    _Manualworkbookurls.Add(in_manualUrl);
                                string newManualWorkbookUrls = _Manualworkbookurls.Aggregate(System.String.Empty, (in_current, in_s) => in_current + (in_s + "|"));
                                SetString("manualworkbookurls", newManualWorkbookUrls);
                            }

                            info.AddWorksheetEntry(entry, finalUrl);
                        }
                    }
                }
                catch
                {
                    _EditorInfo = Localize("ID_INVALID_URL_ERROR");
                }
            }

            return info;
        }

        void DrawAccountWorkbooksGUI()
        {

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("", EditorStyles.miniLabel);
            if (GUILayout.Button(Localize("ID_MANUAL_REFRESH"), EditorStyles.toolbarButton, GUILayout.Width(135)))
            {
                var feed = _Service.Query(_Query);

                if (feed.Entries.Count == 0)
                {
                    _EditorInfo = Localize("ID_NO_DB_ERROR");
                }
                else
                {
                    Debug.Log("Manually Refreshing Workbooks.. This could take a while");
                    _Workbooks.Clear();
                    foreach (var entry in feed.Entries.Cast<Google.GData.Spreadsheets.SpreadsheetEntry>())
                    {
                        try
                        {
                            var info = _Workbooks.Find(
                                in_i => GfuStrCmp(in_i.Title, entry.Feed.Title.Text)
                                );
                            if (info == null)
                            {
                                info = new WorkBookInfo(entry);
                                _Workbooks.Add(info);
                            }
                            if (GfuStrCmp(info.Title, _ActiveWorkbookname))
                                SetActiveWorkbook(info);
                        }
                        catch (System.Exception ex)
                        {
                            Debug.Log(Localize("ID_INACCESSIBLE_ERROR"));
                            Debug.Log(ex.Message);
                        }
                    }
                    Debug.Log("Manual Refresh Complete");
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            DrawHelpButtonGUI("ID_HELP_ACCOUNT_WORKBOOKS");
            DrawLabelHeader("ID_ACCOUNT_WORKBOOK");
            EditorGUILayout.EndHorizontal();

            foreach (WorkBookInfo wbInfo in _Workbooks)
            {
                if (wbInfo == _ActiveWorkbook)
                    GUI.enabled = false;
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(wbInfo.Title, EditorStyles.miniLabel);

                if (GUILayout.Button(Localize("ID_OPEN_URL"), EditorStyles.miniButton, GUILayout.Width(65)))
                {
                    ClearMessages();
                    Application.OpenURL(wbInfo.Url);
                }

                if (GUILayout.Button(Localize("ID_ACTIVATE"), EditorStyles.miniButton, GUILayout.Width(65)))
                {
                    ClearMessages();
                    SetActiveWorkbook(wbInfo);
                    SetString("activeworkbookname", wbInfo.Title);
                    _CurrentPage = GfPage.Toolbox;
                }
                EditorGUILayout.EndHorizontal();
                GUI.enabled = true;
            }
        }

        void DrawPathLabel(string in_path)
        {
            if (_PathLabelBg == null)
                _PathLabelBg = new Texture2D(1, 1);
            if (_PathLabelStyle == null)
                _PathLabelStyle = new GUIStyle();

            _PathLabelBg.SetPixel(0, 0, _PathLabelBgColor);
            _PathLabelStyle.normal.textColor = _PathLabelFgColor;
            _PathLabelStyle.normal.background = _PathLabelBg;
            GUILayout.Label(in_path, _PathLabelStyle);
        }

        void DrawCreateWorkbookGUI()
        {
            EditorGUILayout.BeginHorizontal();
            DrawHelpButtonGUI("ID_HELP_UPLOAD_WORKBOOK");
            DrawLabelHeader("ID_CREATE_WORKBOOK");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();
            Indent(1);
            GUI.backgroundColor = Color.clear;
            GUI.color = Color.yellow;
            bool bDoSave = false;
            GUI.SetNextControlName("Clear");
            if (GUILayout.Button(_BrowseButton, EditorStyles.toolbarButton, GUILayout.Width(24)))
            {
                ClearMessages();
                string workbookpath = EditorUtility.OpenFilePanel(
                    Localize("ID_SELECT_UPLOAD_WORKBOOK_PATH"), EditorApplication.applicationPath, "*.xls;*.xlsx;*.ods;*.csv;*.txt;*.tsv");

                if (!string.IsNullOrEmpty(workbookpath))
                {
                    if ((workbookpath.IndexOf(".xls", System.StringComparison.InvariantCultureIgnoreCase) != -1) ||
                        (workbookpath.IndexOf(".xlsx", System.StringComparison.InvariantCultureIgnoreCase) != -1) ||
                        (workbookpath.IndexOf(".ods", System.StringComparison.InvariantCultureIgnoreCase) != -1) ||
                        (workbookpath.IndexOf(".csv", System.StringComparison.InvariantCultureIgnoreCase) != -1) ||
                        (workbookpath.IndexOf(".txt", System.StringComparison.InvariantCultureIgnoreCase) != -1) ||
                        (workbookpath.IndexOf(".tsv", System.StringComparison.InvariantCultureIgnoreCase) != -1))
                    {
                        bDoSave = true;
                    }

                    if (bDoSave)
                    {
                        _WorkbookUploadPath = workbookpath;
                        GUI.FocusControl("Clear");
                        Repaint();
                    }
                    else
                    {
                        _EditorWarning = Localize("ID_ERROR_UPLOAD_WORKBOOK_PATH");
                        Debug.LogWarning(_EditorWarning);
                    }
                }
            }
            GUI.backgroundColor = _DefaultBgColor;
            GUI.color = _DefaultFgColor;
            EditorGUILayout.TextField(_WorkbookUploadPath);
            GUILayout.Label(System.String.Empty, GUILayout.Width(5));

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(System.String.Empty);

            if (CreatingWorkbook)
                GUI.enabled = false;

            if (GUILayout.Button(Localize("ID_CREATE"), EditorStyles.miniButton, GUILayout.Width(65)))
            {
                ClearMessages();

                // We need a DocumentService
                var service = new Google.GData.Documents.DocumentsService("GoogleFuUploader");
                string mimeType = GoogleFuMimeType.GetMimeType(_WorkbookUploadPath);

                var authenticator = new Google.GData.Client.ClientLoginAuthenticator("UnityGoogleFu", Google.GData.Client.ServiceNames.Documents, _Username, _Password);
                //service.setUserCredentials(_username, _password);

                // Instantiate a DocumentEntry object to be inserted.
                var entry = new Google.GData.Documents.DocumentEntry();

                CreatingWorkbook = true;
                entry.MediaSource = new Google.GData.Client.MediaFileSource(_WorkbookUploadPath, mimeType);

                // Define the resumable upload link
                var createUploadUrl = new System.Uri("https://docs.google.com/feeds/upload/create-session/default/private/full");
                var link = new Google.GData.Client.AtomLink(createUploadUrl.AbsoluteUri)
                {
                    Rel = Google.GData.Client.ResumableUpload.ResumableUploader.CreateMediaRelation
                };

                entry.Links.Add(link);

                // Set the service to be used to parse the returned entry
                entry.Service = service;


                // Instantiate the ResumableUploader component.
                var uploader = new Google.GData.Client.ResumableUpload.ResumableUploader();

                // Set the handlers for the completion and progress events
                uploader.AsyncOperationCompleted += OnSpreadsheetUploadDone;
                uploader.AsyncOperationProgress += OnSpreadsheetUploadProgress;

                // Start the upload process
                uploader.InsertAsync(authenticator, entry, new object());
                _EditorInfo = Localize("ID_CREATING_DATABASE_MESSAGE");

                //Repaint();
            }
            GUI.enabled = true;
            GUILayout.Label(System.String.Empty, GUILayout.Width(5));
            EditorGUILayout.EndHorizontal();
        }

        static void OnSpreadsheetUploadDone(object in_sender, Google.GData.Client.AsyncOperationCompletedEventArgs in_e)
        {
            var entry = in_e.Entry as Google.GData.Documents.DocumentEntry;

            if (entry == null || entry.IsSpreadsheet != true)
                EditorException = in_e.Error.Message;
            else
                ShowUploadedNotification = true;

            DoRefreshWorkbooks = true;
            CreatingWorkbook = false;


        }

        static void OnSpreadsheetUploadProgress(object in_sender, Google.GData.Client.AsyncOperationProgressEventArgs in_e)
        {
            //int percentage = e.ProgressPercentage;
        }

        void DrawManualWorkbooksGUI()
        {
            if (_Manualworkbooks.Count > 0)
            {
                EditorGUILayout.BeginHorizontal();
                DrawHelpButtonGUI("ID_HELP_MANUAL_WORKBOOKS");
                DrawLabelHeader("ID_MANUAL_WORKBOOK");
                EditorGUILayout.EndHorizontal();

                foreach (WorkBookInfo wbInfo in _Manualworkbooks)
                {
                    if (wbInfo == _ActiveWorkbook)
                        GUI.enabled = false;

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label(wbInfo.Title, EditorStyles.miniLabel);

                    if (GUILayout.Button(Localize("ID_OPEN_URL"), EditorStyles.miniButton, GUILayout.Width(65)))
                    {
                        ClearMessages();
                        Application.OpenURL(wbInfo.Url);
                    }

                    if (GUILayout.Button(Localize("ID_ACTIVATE"), EditorStyles.miniButton, GUILayout.Width(65)))
                    {
                        ClearMessages();
                        SetActiveWorkbook(wbInfo);
                        SetString("activeworkbookname", wbInfo.Title);
                        _CurrentPage = GfPage.Toolbox;
                    }
                    EditorGUILayout.EndHorizontal();
                    GUI.enabled = true;
                }
            }

            EditorGUILayout.BeginHorizontal();
            DrawHelpButtonGUI("ID_HELP_ADD_MANUAL_WORKBOOK");
            DrawLabelHeader("ID_ADD_WORKBOOK_MANUALLY");
            EditorGUILayout.EndHorizontal();



            _Manualurl = EditorGUILayout.TextField(_Manualurl);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(System.String.Empty);

            if (GUILayout.Button(Localize("ID_OPEN_URL"), EditorStyles.miniButton, GUILayout.Width(65)))
            {
                ClearMessages();
                Application.OpenURL(_Manualurl);
            }
            if (GUILayout.Button(Localize("ID_ADD_WORKBOOK"), EditorStyles.miniButton, GUILayout.Width(65)))
            {
                ClearMessages();
                AddManualWorkbookByUrl(_Manualurl);
            }
            EditorGUILayout.EndHorizontal();
        }


        void DrawHelpButtonGUI(string in_hdString)
        {
            bool bShow = false;
            if (_ShowHelp.ContainsKey(in_hdString))
                bShow = _ShowHelp[in_hdString];
            else
                _ShowHelp.Add(in_hdString, false);

            if (bShow == false)
            {
                GUI.backgroundColor = Color.clear;
                bShow = GUILayout.Button(_HelpButton, EditorStyles.toolbarButton, GUILayout.Width(24));
                GUI.backgroundColor = _DefaultBgColor;
                _ShowHelp[in_hdString] = bShow;
            }
            else
            {
                GUI.backgroundColor = Color.clear;
                bShow = !GUILayout.Button(_HelpButton, EditorStyles.toolbarButton, GUILayout.Width(24));
                GUI.backgroundColor = _DefaultBgColor;
                {
                    EditorGUILayout.HelpBox(Localize(in_hdString), MessageType.Info);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    Indent(_CurIndent);
                    GUILayout.Label("", GUILayout.Width(24));
                }
                _ShowHelp[in_hdString] = bShow;
            }
        }

        void DrawLabelHeader(string in_labelID)
        {
            GUI.color = _LabelHeaderColor;
            GUILayout.Label(Localize(in_labelID), _LargeLabelStyle);
            GUI.color = _DefaultFgColor;
        }

        void DrawChooseLanguageGUI()
        {

            var lsOptions = new string[Language.languageStrings.GetUpperBound(0)];
            var lsCode = new string[Language.languageStrings.GetUpperBound(0)];
            var lsName = new string[Language.languageStrings.GetUpperBound(0)];
            for (var i = 0; i < Language.languageStrings.GetUpperBound(0); ++i)
            {
                lsOptions[i] = Language.languageStrings[i, 0];
                lsCode[i] = Language.languageStrings[i, 1];
                lsName[i] = Language.languageStrings[i, 2];
            }

            var oldIdx = _LanguagesIndex;

            EditorGUILayout.BeginHorizontal();
            DrawHelpButtonGUI("ID_HELP_SELECT_LANGUAGE");
            DrawLabelHeader("ID_SELECT_LANGUAGE");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            GUI.SetNextControlName("Clear");
            _LanguagesIndex = EditorGUILayout.Popup(_LanguagesIndex, lsOptions);

            if (oldIdx != _LanguagesIndex)
            {
                _EditorLanguage = Language.languageStrings[_LanguagesIndex, 1];
                SetString("editorLanguage", _EditorLanguage);
                SetInt("languagesIndex", _LanguagesIndex);
                GUI.FocusControl("Clear");
                Repaint();
            }
        }

        string GoogleFuGenPath(string in_pathType)
        {
            var retPath = "";

            if (GfuStrCmp(in_pathType, "GOOGLEFUGEN"))
            {
                {
                    retPath = System.IO.Path.Combine(_ProjectPath, "GoogleFuGen");
                    if (!System.IO.Directory.Exists(retPath))
                        System.IO.Directory.CreateDirectory(retPath);
                }
            }
            else if (GfuStrCmp(in_pathType, "OBJDB"))
            {
                {
                    var googlefugenPath = GoogleFuGenPath("GOOGLEFUGEN");

                    retPath = System.IO.Path.Combine(googlefugenPath, "ObjDB");
                    if (!System.IO.Directory.Exists(retPath))
                        System.IO.Directory.CreateDirectory(retPath);
                }
            }
            else if (GfuStrCmp(in_pathType, "OBJDBRESOURCES"))
            {
                {
                    retPath = GetString("objDBResourcesDirectory", retPath);
                    if (!System.IO.Directory.Exists(retPath))
                    {
                        string objdbPath = GoogleFuGenPath("OBJDB");

                        retPath = System.IO.Path.Combine(objdbPath, "Resources");
                        if (!System.IO.Directory.Exists(retPath))
                            System.IO.Directory.CreateDirectory(retPath);

                        SetString("objDBResourcesDirectory", retPath);
                        _ObjDbResourcesDirectory = retPath;
                    }
                }
            }
            else if (GfuStrCmp(in_pathType, "OBJDBEDITOR"))
            {
                {
                    retPath = GetString("objDBEditorDirectory", retPath);
                    if (!System.IO.Directory.Exists(retPath))
                    {
                        string objdbPath = GoogleFuGenPath("OBJDB");

                        retPath = System.IO.Path.Combine(objdbPath, "Editor");
                        if (!System.IO.Directory.Exists(retPath))
                            System.IO.Directory.CreateDirectory(retPath);

                        SetString("objDBEditorDirectory", retPath);
                        _ObjDbEditorDirectory = retPath;
                    }
                }
            }
            else if (GfuStrCmp(in_pathType, "STATICDB"))
            {
                {
                    var googlefugenPath = GoogleFuGenPath("GOOGLEFUGEN");

                    retPath = System.IO.Path.Combine(googlefugenPath, "StaticDB");
                    if (!System.IO.Directory.Exists(retPath))
                        System.IO.Directory.CreateDirectory(retPath);
                }
            }
            else if (GfuStrCmp(in_pathType, "STATICDBRESOURCES"))
            {
                {
                    retPath = GetString("staticDBResourcesDirectory", retPath);
                    if (System.IO.Directory.Exists(retPath))
                        return retPath;
                    var staticdbPath = GoogleFuGenPath("STATICDB");

                    retPath = System.IO.Path.Combine(staticdbPath, "Resources");
                    if (!System.IO.Directory.Exists(retPath))
                        System.IO.Directory.CreateDirectory(retPath);

                    SetString("staticDBResourcesDirectory", retPath);
                    _StaticDbResourcesDirectory = retPath;
                }
            }
            else if (GfuStrCmp(in_pathType, "JSON"))
            {
                {
                    retPath = GetString("jsonDirectory", retPath);
                    if (System.IO.Directory.Exists(retPath))
                        return retPath;
                    var googlefugenPath = GoogleFuGenPath("GOOGLEFUGEN");

                    retPath = System.IO.Path.Combine(googlefugenPath, "JSON");
                    if (!System.IO.Directory.Exists(retPath))
                        System.IO.Directory.CreateDirectory(retPath);

                    SetString("jsonDirectory", retPath);
                    _JsonDirectory = retPath;
                }
            }
            else if (GfuStrCmp(in_pathType, "CSV"))
            {
                {
                    retPath = GetString("csvDirectory", retPath);
                    if (System.IO.Directory.Exists(retPath))
                        return retPath;
                    var googlefugenPath = GoogleFuGenPath("GOOGLEFUGEN");

                    retPath = System.IO.Path.Combine(googlefugenPath, "CSV");
                    if (!System.IO.Directory.Exists(retPath))
                        System.IO.Directory.CreateDirectory(retPath);

                    SetString("csvDirectory", retPath);
                    _CsvDirectory = retPath;
                }
            }
            else if (GfuStrCmp(in_pathType, "XML"))
            {
                {
                    retPath = GetString("xmlDirectory", retPath);
                    if (System.IO.Directory.Exists(retPath))
                        return retPath;
                    var googlefugenPath = GoogleFuGenPath("GOOGLEFUGEN");

                    retPath = System.IO.Path.Combine(googlefugenPath, "XML");
                    if (!System.IO.Directory.Exists(retPath))
                        System.IO.Directory.CreateDirectory(retPath);

                    SetString("xmlDirectory", retPath);
                    _XmlDirectory = retPath;
                }
            }
            else if (GfuStrCmp(in_pathType, "NGUI"))
            {
                {
                    retPath = GetString("nguiDirectory", retPath);
                    if (System.IO.Directory.Exists(retPath))
                        return retPath;
                    var googlefugenPath = GoogleFuGenPath("GOOGLEFUGEN");

                    retPath = System.IO.Path.Combine(googlefugenPath, "NGUI");
                    if (!System.IO.Directory.Exists(retPath))
                        System.IO.Directory.CreateDirectory(retPath);

                    SetString("nguiDirectory", retPath);
                    _NguiDirectory = retPath;
                }
            }
            else if (GfuStrCmp(in_pathType, "DAIKONFORGE"))
            {
                {
                    retPath = GetString("daikonforgeDirectory", retPath);
                    if (System.IO.Directory.Exists(retPath))
                        return retPath;
                    var googlefugenPath = GoogleFuGenPath("GOOGLEFUGEN");

                    retPath = System.IO.Path.Combine(googlefugenPath, "DAIKONFORGE");
                    if (!System.IO.Directory.Exists(retPath))
                        System.IO.Directory.CreateDirectory(retPath);

                    SetString("daikonforgeDirectory", retPath);
                    _DaikonforgeDirectory = retPath;
                }
            }
            else if (GfuStrCmp(in_pathType, "PLAYMAKER"))
            {
                {
                    retPath = GetString("playmakerDirectory", retPath);
                    if (System.IO.Directory.Exists(retPath))
                        return retPath;
                    // attempt to find the playmaker actions directory
                    // We already know that the playmaker dll exists, but we need to find the actual path
                    var playmakerPaths = System.IO.Directory.GetFiles(Application.dataPath, "PlayMaker.dll",
                        System.IO.SearchOption.AllDirectories);
                    var playmakerPath = System.String.Empty;
                    if (playmakerPaths.Length > 0)
                    {
                        // We are just going to use the first entry. If there is more than 1 entry, there are bigger issues
                        var fileName = playmakerPaths[0];
                        var fileInfo = new System.IO.FileInfo(fileName);
                        playmakerPath = fileInfo.DirectoryName;
                    }

                    if (playmakerPath != System.String.Empty)
                    {
                        if (playmakerPath != null) retPath = System.IO.Path.Combine(playmakerPath, "Actions");
                        if (System.IO.Directory.Exists(retPath))
                        {
                            // We have found the Playmaker Actions dir!
                            SetString("playmakerDirectory", retPath);
                            _PlaymakerDirectory = retPath;
                        }
                        else
                        {
                            // The actions subdirectory doesn't exist? Rather than making it in the playmaker directory,
                            // We will just use our GoogleFuGen path instead and let the user figure it out
                            var googlefugenPath = GoogleFuGenPath("GOOGLEFUGEN");

                            retPath = System.IO.Path.Combine(googlefugenPath, "PlayMaker");
                            if (!System.IO.Directory.Exists(retPath))
                                System.IO.Directory.CreateDirectory(retPath);

                            SetString("playmakerDirectory", retPath);
                            _PlaymakerDirectory = retPath;
                        }
                    }
                }
            }

            return retPath;
        }

        void DrawCreatePathsGUI()
        {
            EditorGUILayout.BeginHorizontal();
            DrawHelpButtonGUI("ID_HELP_CREATE_PATHS");

            GUI.SetNextControlName("PathRefocus");
            if (GUILayout.Button(Localize("ID_GENERATE_PATHS"), EditorStyles.toolbarButton))
            {
                GoogleFuGenPath("GoogleFuGen");
                GoogleFuGenPath("ObjDBResources");
                GoogleFuGenPath("ObjDBEditor");
                GoogleFuGenPath("StaticDBResources");
                GoogleFuGenPath("JSON");
                GoogleFuGenPath("CSV");
                GoogleFuGenPath("XML");
                if (_FoundNgui)
                    GoogleFuGenPath("NGUI");
                if (_FoundDaikonForge)
                    GoogleFuGenPath("DAIKONFORGE");
                if (_FoundPlaymaker)
                    GoogleFuGenPath("PLAYMAKER");
                GetPathErrors();
                GUI.FocusControl("PathRefocus");
                Repaint();
            }
            EditorGUILayout.EndHorizontal();
        }

        void DrawEnableDbObjGUI()
        {
            EditorGUILayout.BeginHorizontal();
            DrawHelpButtonGUI("ID_HELP_GAMEOBJECT_DB_ENABLE");
            var oldUseObjDb = _UseObjDb;
            _UseObjDb = DrawToggle("ID_ENABLE_DB_OBJ", oldUseObjDb);
            EditorGUILayout.EndHorizontal();

            if (oldUseObjDb != _UseObjDb)
            {
                SetBool("useObjDB", _UseObjDb);
            }

            if (_UseObjDb)
            {
                EditorGUILayout.Separator();
                var bDoSave = true;
                var oldObjDbResourcesDirectory = _ObjDbResourcesDirectory;
                var oldObjDbEditorDirectory = _ObjDbEditorDirectory;

                EditorGUILayout.BeginHorizontal();
                Indent(1);
                DrawHelpButtonGUI("ID_HELP_GAMEOBJECT_RESOURCES_PATH");
                DrawLabelHeader("ID_SELECT_RESOURCES_DIRECTORY");
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                Indent(1);
                GUI.backgroundColor = Color.clear;
                GUI.color = Color.yellow;
                if (GUILayout.Button(_BrowseButton, EditorStyles.toolbarButton, GUILayout.Width(24)))
                {
                    ClearMessages();
                    var objDbResourcesDirectory = EditorUtility.SaveFolderPanel(
                        oldObjDbResourcesDirectory, EditorApplication.applicationPath, System.String.Empty);

                    if ((objDbResourcesDirectory.Length == 0) ||
                        (objDbResourcesDirectory.IndexOf(_ProjectPath, System.StringComparison.InvariantCultureIgnoreCase) != 0) ||
                        (objDbResourcesDirectory.IndexOf("/RESOURCES", System.StringComparison.InvariantCultureIgnoreCase) == -1))
                    {
                        _EditorPathInfo = Localize("ID_ERROR_RESOURCES_DIRECTORY");
                        Debug.LogWarning(_EditorPathInfo);
                        bDoSave = false;
                    }

                    if ((bDoSave) && (oldObjDbResourcesDirectory != objDbResourcesDirectory))
                    {
                        SetString("objDBResourcesDirectory", objDbResourcesDirectory);
                        _ObjDbResourcesDirectory = objDbResourcesDirectory;
                    }
                }
                GUI.color = _DefaultFgColor;
                GUI.backgroundColor = _DefaultBgColor;
                DrawPathLabel(_ObjDbResourcesDirectory);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                Indent(1);
                DrawHelpButtonGUI("ID_HELP_GAMEOBJECT_EDITOR_PATH");
                DrawLabelHeader("ID_SELECT_EDITOR_DIRECTORY");
                EditorGUILayout.EndHorizontal();

                bDoSave = true;

                EditorGUILayout.BeginHorizontal();
                Indent(1);
                GUI.backgroundColor = Color.clear;
                GUI.color = Color.yellow;
                if (GUILayout.Button(_BrowseButton, EditorStyles.toolbarButton, GUILayout.Width(24)))
                {
                    ClearMessages();
                    var objDbEditorDirectory = EditorUtility.SaveFolderPanel(
                        oldObjDbEditorDirectory, EditorApplication.applicationPath, System.String.Empty);

                    if ((objDbEditorDirectory.Length == 0) ||
                        (objDbEditorDirectory.IndexOf(_ProjectPath, System.StringComparison.InvariantCultureIgnoreCase) != 0) ||
                        (objDbEditorDirectory.IndexOf("/EDITOR", System.StringComparison.InvariantCultureIgnoreCase) == -1))
                    {
                        _EditorPathInfo = Localize("ID_ERROR_EDITOR_DIRECTORY");
                        Debug.LogWarning(_EditorPathInfo);
                        bDoSave = false;
                    }

                    if ((bDoSave) && (oldObjDbEditorDirectory != objDbEditorDirectory))
                    {
                        SetString("objDBEditorDirectory", objDbEditorDirectory);
                        _ObjDbEditorDirectory = objDbEditorDirectory;
                    }
                }
                GUI.color = _DefaultFgColor;
                GUI.backgroundColor = _DefaultBgColor;
                DrawPathLabel(_ObjDbEditorDirectory);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
        }

        void DrawEnableStaticObjGUI()
        {
            EditorGUILayout.BeginHorizontal();
            DrawHelpButtonGUI("ID_HELP_STATIC_DB_ENABLE");
            var oldUseStaticDb = _UseStaticDb;
            _UseStaticDb = DrawToggle("ID_ENABLE_DB_STATIC", oldUseStaticDb);
            EditorGUILayout.EndHorizontal();

            if (oldUseStaticDb != _UseStaticDb)
            {
                SetBool("useStaticDB", _UseStaticDb);
            }

            if (_UseStaticDb)
            {
                EditorGUILayout.Separator();
                var bDoSave = true;
                var oldStaticDbResourcesDirectory = _StaticDbResourcesDirectory;

                EditorGUILayout.BeginHorizontal();
                Indent(1);
                DrawHelpButtonGUI("ID_HELP_STATIC_RESOURCES_PATH");
                DrawLabelHeader("ID_SELECT_RESOURCES_DIRECTORY");
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                Indent(1);
                GUI.backgroundColor = Color.clear;
                GUI.color = Color.yellow;
                if (GUILayout.Button(_BrowseButton, EditorStyles.toolbarButton, GUILayout.Width(24)))
                {
                    ClearMessages();
                    var staticDbResourcesDirectory = EditorUtility.SaveFolderPanel(
                        oldStaticDbResourcesDirectory, EditorApplication.applicationPath, System.String.Empty);

                    if ((staticDbResourcesDirectory.Length == 0) ||
                        (staticDbResourcesDirectory.IndexOf(_ProjectPath, System.StringComparison.InvariantCultureIgnoreCase) != 0) ||
                        (staticDbResourcesDirectory.IndexOf("/RESOURCES", System.StringComparison.InvariantCultureIgnoreCase) == -1))
                    {
                        _EditorPathInfo = Localize("ID_ERROR_RESOURCES_DIRECTORY");
                        Debug.LogWarning(_EditorPathInfo);
                        bDoSave = false;
                    }

                    if ((bDoSave) && (oldStaticDbResourcesDirectory != staticDbResourcesDirectory))
                    {
                        SetString("staticDBResourcesDirectory", staticDbResourcesDirectory);
                        _StaticDbResourcesDirectory = staticDbResourcesDirectory;
                    }
                }
                GUI.color = _DefaultFgColor;
                GUI.backgroundColor = _DefaultBgColor;
                DrawPathLabel(_StaticDbResourcesDirectory);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
        }

        void DrawChooseNguiPathGUI()
        {
            if (_FoundNgui == false)
                return;

            EditorGUILayout.BeginHorizontal();
            DrawHelpButtonGUI("ID_HELP_NGUI_ENABLE");
            var oldUseNgui = _UseNgui;
            _UseNgui = DrawToggle("ID_ENABLE_NGUI", _UseNgui);

            EditorGUILayout.EndHorizontal();

            if (oldUseNgui != _UseNgui)
            {
                SetBool("useNGUI", _UseNgui);
            }

            if (_UseNgui)
            {
                EditorGUILayout.Separator();

                EditorGUILayout.BeginHorizontal();
                Indent(1);
                DrawHelpButtonGUI("ID_HELP_NGUI_PATH");
                DrawLabelHeader("ID_SELECT_NGUI_DIRECTORY");
                EditorGUILayout.EndHorizontal();

                bool bDoSave = true;
                string oldNguiPath = _NguiDirectory;

                EditorGUILayout.BeginHorizontal();
                Indent(1);
                GUI.backgroundColor = Color.clear;
                GUI.color = Color.yellow;
                if (GUILayout.Button(_BrowseButton, EditorStyles.toolbarButton, GUILayout.Width(24)))
                {
                    ClearMessages();
                    string nguipath = EditorUtility.SaveFolderPanel(
                        oldNguiPath, EditorApplication.applicationPath, System.String.Empty);

                    if ((nguipath.Length == 0) ||
                        (nguipath.IndexOf(_ProjectPath, System.StringComparison.InvariantCultureIgnoreCase) != 0))
                    {
                        _EditorPathInfo = Localize("ID_ERROR_NGUI_DIRECTORY");
                        Debug.LogWarning(_EditorPathInfo);
                        bDoSave = false;
                    }

                    if ((bDoSave) && (oldNguiPath != nguipath))
                    {
                        SetString("nguiDirectory", nguipath);
                        _NguiDirectory = nguipath;
                    }
                }
                GUI.color = _DefaultFgColor;
                GUI.backgroundColor = _DefaultBgColor;
                DrawPathLabel(_NguiDirectory);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                Indent(1);
                DrawHelpButtonGUI("ID_HELP_NGUI_LEGACY");
                var oldUseNguiLegacy = _UseNguiLegacy;

                _UseNguiLegacy = DrawToggle("ID_ENABLE_NGUI_LEGACY", _UseNguiLegacy);

                if (oldUseNguiLegacy != _UseNguiLegacy)
                {
                    SetBool("useNGUILegacy", _UseNguiLegacy);
                }
                EditorGUILayout.EndHorizontal();



            }
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
        }

        void DrawChooseDaikonForgePathGUI()
        {
            if (_FoundDaikonForge == false)
                return;

            EditorGUILayout.BeginHorizontal();
            DrawHelpButtonGUI("ID_HELP_DAIKONFORGE_ENABLE");
            bool oldUseDaikonForge = _UseDaikonForge;
            _UseDaikonForge = DrawToggle("ID_ENABLE_DAIKONFORGE", _UseDaikonForge);
            EditorGUILayout.EndHorizontal();

            if (oldUseDaikonForge != _UseDaikonForge)
            {
                SetBool("useDaikonForge", _UseDaikonForge);
            }

            if (_UseDaikonForge)
            {
                EditorGUILayout.Separator();

                EditorGUILayout.BeginHorizontal();
                Indent(1);
                DrawHelpButtonGUI("ID_HELP_DAIKONFORGE_PATH");
                DrawLabelHeader("ID_SELECT_DAIKONFORGE_DIRECTORY");
                EditorGUILayout.EndHorizontal();

                bool bDoSave = true;
                string oldDaikonForgePath = _DaikonforgeDirectory;

                EditorGUILayout.BeginHorizontal();
                Indent(1);
                GUI.backgroundColor = Color.clear;
                GUI.color = Color.yellow;
                if (GUILayout.Button(_BrowseButton, EditorStyles.toolbarButton, GUILayout.Width(24)))
                {
                    ClearMessages();
                    string daikonforgepath = EditorUtility.SaveFolderPanel(
                        oldDaikonForgePath, EditorApplication.applicationPath, System.String.Empty);

                    if ((daikonforgepath.Length == 0) ||
                        (daikonforgepath.IndexOf(_ProjectPath, System.StringComparison.InvariantCultureIgnoreCase) != 0))
                    {
                        _EditorPathInfo = Localize("ID_ERROR_DAIKONFORGE_DIRECTORY");
                        Debug.LogWarning(_EditorPathInfo);
                        bDoSave = false;
                    }

                    if ((bDoSave) && (oldDaikonForgePath != daikonforgepath))
                    {
                        SetString("daikonforgeDirectory", daikonforgepath);
                        _DaikonforgeDirectory = daikonforgepath;
                    }
                }
                GUI.color = _DefaultFgColor;
                GUI.backgroundColor = _DefaultBgColor;
                DrawPathLabel(_DaikonforgeDirectory);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
        }

        void DrawChooseXmlPathGUI()
        {
            EditorGUILayout.BeginHorizontal();
            DrawHelpButtonGUI("ID_HELP_XML_ENABLE");
            var oldUseXml = _UseXml;
            _UseXml = DrawToggle("ID_ENABLE_XML", _UseXml);
            EditorGUILayout.EndHorizontal();

            if (oldUseXml != _UseXml)
            {
                SetBool("useXML", _UseXml);
            }

            if (_UseXml)
            {
                EditorGUILayout.Separator();

                EditorGUILayout.BeginHorizontal();
                Indent(1);
                DrawHelpButtonGUI("ID_HELP_XML_PATH");
                DrawLabelHeader("ID_SELECT_XML_DIRECTORY");
                EditorGUILayout.EndHorizontal();

                var bDoSave = true;
                var oldXmlPath = _XmlDirectory;

                EditorGUILayout.BeginHorizontal();
                Indent(1);
                GUI.backgroundColor = Color.clear;
                GUI.color = Color.yellow;
                if (GUILayout.Button(_BrowseButton, EditorStyles.toolbarButton, GUILayout.Width(24)))
                {
                    ClearMessages();
                    string xmlpath = EditorUtility.SaveFolderPanel(
                        oldXmlPath, EditorApplication.applicationPath, System.String.Empty);

                    if ((xmlpath.Length == 0) ||
                        (xmlpath.IndexOf(_ProjectPath, System.StringComparison.InvariantCultureIgnoreCase) != 0))
                    {
                        _EditorPathInfo = Localize("ID_ERROR_XML_DIRECTORY");
                        Debug.LogWarning(_EditorPathInfo);
                        bDoSave = false;
                    }

                    if ((bDoSave) && (oldXmlPath != xmlpath))
                    {
                        SetString("xmlDirectory", xmlpath);
                        _XmlDirectory = xmlpath;
                    }
                }
                GUI.color = _DefaultFgColor;
                GUI.backgroundColor = _DefaultBgColor;
                DrawPathLabel(_XmlDirectory);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
        }

        void DrawChooseCsvPathGUI()
        {
            EditorGUILayout.BeginHorizontal();
            DrawHelpButtonGUI("ID_HELP_CSV_ENABLE");
            var oldUseCsv = _UseCsv;
            _UseCsv = DrawToggle("ID_ENABLE_CSV", _UseCsv);
            EditorGUILayout.EndHorizontal();

            if (oldUseCsv != _UseCsv)
            {
                SetBool("useCSV", _UseCsv);
            }

            if (_UseCsv)
            {
                EditorGUILayout.Separator();

                EditorGUILayout.BeginHorizontal();
                Indent(1);
                DrawHelpButtonGUI("ID_HELP_CSV_PATH");
                DrawLabelHeader("ID_SELECT_CSV_DIRECTORY");
                EditorGUILayout.EndHorizontal();

                bool bDoSave = true;
                var oldCsvPath = _CsvDirectory;
                EditorGUILayout.BeginHorizontal();
                Indent(1);
                GUI.backgroundColor = Color.clear;
                GUI.color = Color.yellow;
                if (GUILayout.Button(_BrowseButton, EditorStyles.toolbarButton, GUILayout.Width(24)))
                {
                    ClearMessages();
                    string csvpath = EditorUtility.SaveFolderPanel(
                        oldCsvPath, EditorApplication.applicationPath, System.String.Empty);

                    if ((csvpath.Length == 0) ||
                        (csvpath.IndexOf(_ProjectPath, System.StringComparison.InvariantCultureIgnoreCase) != 0))
                    {
                        _EditorPathInfo = Localize("ID_ERROR_CSV_DIRECTORY");
                        Debug.LogWarning(_EditorPathInfo);
                        bDoSave = false;
                    }

                    if ((bDoSave) && (oldCsvPath != csvpath))
                    {
                        SetString("csvDirectory", csvpath);
                        _CsvDirectory = csvpath;
                    }
                }
                GUI.color = _DefaultFgColor;
                GUI.backgroundColor = _DefaultBgColor;
                DrawPathLabel(_CsvDirectory);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
        }

        void DrawChooseJsonPathGUI()
        {
            EditorGUILayout.BeginHorizontal();
            DrawHelpButtonGUI("ID_HELP_JSON_ENABLE");
            var oldUseJson = _UseJson;
            _UseJson = DrawToggle("ID_ENABLE_JSON", _UseJson);
            EditorGUILayout.EndHorizontal();

            if (oldUseJson != _UseJson)
            {
                SetBool("useJSON", _UseJson);
            }

            if (_UseJson)
            {
                EditorGUILayout.Separator();

                EditorGUILayout.BeginHorizontal();
                Indent(1);
                DrawHelpButtonGUI("ID_HELP_JSON_PATH");
                DrawLabelHeader("ID_SELECT_JSON_DIRECTORY");
                EditorGUILayout.EndHorizontal();

                var bDoSave = true;
                var oldJsonPath = _JsonDirectory;
                EditorGUILayout.BeginHorizontal();
                Indent(1);
                GUI.backgroundColor = Color.clear;
                GUI.color = Color.yellow;
                if (GUILayout.Button(_BrowseButton, EditorStyles.toolbarButton, GUILayout.Width(24)))
                {
                    ClearMessages();
                    string jsonpath = EditorUtility.SaveFolderPanel(
                        oldJsonPath, EditorApplication.applicationPath, System.String.Empty);

                    if ((jsonpath.Length == 0) ||
                        (jsonpath.IndexOf(_ProjectPath, System.StringComparison.InvariantCultureIgnoreCase) != 0))
                    {
                        _EditorPathInfo = Localize("ID_ERROR_JSON_DIRECTORY");
                        Debug.LogWarning(_EditorPathInfo);
                        bDoSave = false;
                    }

                    if ((bDoSave) && (oldJsonPath != jsonpath))
                    {
                        SetString("jsonDirectory", jsonpath);
                        _JsonDirectory = jsonpath;
                    }
                }
                GUI.color = _DefaultFgColor;
                GUI.backgroundColor = _DefaultBgColor;
                DrawPathLabel(_JsonDirectory);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
        }

        void DrawChoosePlaymakerPathGUI()
        {
            if (_FoundPlaymaker == false)
                return;

            EditorGUILayout.BeginHorizontal();
            DrawHelpButtonGUI("ID_HELP_PLAYMAKER_ENABLE");
            bool oldUsePlaymaker = _UsePlaymaker;
            _UsePlaymaker = DrawToggle("ID_ENABLE_PLAYMAKER", _UsePlaymaker);
            EditorGUILayout.EndHorizontal();

            if (oldUsePlaymaker != _UsePlaymaker)
            {
                SetBool("usePlaymaker", _UsePlaymaker);
            }

            if (_UsePlaymaker)
            {
                EditorGUILayout.Separator();

                EditorGUILayout.BeginHorizontal();
                Indent(1);
                DrawHelpButtonGUI("ID_HELP_PLAYMAKER_PATH");
                DrawLabelHeader("ID_SELECT_PLAYMAKER_DIRECTORY");
                EditorGUILayout.EndHorizontal();

                bool bDoSave = true;
                string oldPlaymakerPath = _PlaymakerDirectory;
                EditorGUILayout.BeginHorizontal();
                Indent(1);
                GUI.backgroundColor = Color.clear;
                GUI.color = Color.yellow;
                if (GUILayout.Button(_BrowseButton, EditorStyles.toolbarButton, GUILayout.Width(24)))
                {
                    ClearMessages();
                    string playmakerpath = EditorUtility.SaveFolderPanel(
                        oldPlaymakerPath, EditorApplication.applicationPath, System.String.Empty);

                    if ((playmakerpath.Length == 0) ||
                        (playmakerpath.IndexOf(_ProjectPath, System.StringComparison.InvariantCultureIgnoreCase) != 0))
                    {
                        _EditorPathInfo = Localize("ID_ERROR_PLAYMAKER_DIRECTORY");
                        Debug.LogWarning(_EditorPathInfo);
                        bDoSave = false;
                    }

                    if ((bDoSave) && (oldPlaymakerPath != playmakerpath))
                    {
                        SetString("playmakerDirectory", playmakerpath);
                        _PlaymakerDirectory = playmakerpath;
                    }
                }
                GUI.color = _DefaultFgColor;
                GUI.backgroundColor = _DefaultBgColor;
                DrawPathLabel(_PlaymakerDirectory);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
        }

        void Export(string in_workbookName, System.Collections.Generic.IEnumerable<Google.GData.Spreadsheets.WorksheetEntry> in_activeEntries)
        {
            _Anoncolname = 0;

            var objDbEntries = new System.Collections.Generic.List<Google.GData.Spreadsheets.WorksheetEntry>();
            var staticDbEntries = new System.Collections.Generic.List<Google.GData.Spreadsheets.WorksheetEntry>();
            var nguiEntries = new System.Collections.Generic.List<Google.GData.Spreadsheets.WorksheetEntry>();
            var xmlEntries = new System.Collections.Generic.List<Google.GData.Spreadsheets.WorksheetEntry>();
            var jsonEntries = new System.Collections.Generic.List<Google.GData.Spreadsheets.WorksheetEntry>();
            var daikonforgeEntries = new System.Collections.Generic.List<Google.GData.Spreadsheets.WorksheetEntry>();
            var csvEntries = new System.Collections.Generic.List<Google.GData.Spreadsheets.WorksheetEntry>();

            string sanitizedWorkbookName = MakeValidFileName(in_workbookName);

            foreach (Google.GData.Spreadsheets.WorksheetEntry entry in in_activeEntries)
            {
                if (GetBool(in_workbookName + "." + entry.Title.Text + ".EXPORTOBJDB", false))
                    objDbEntries.Add(entry);
                else if (GetBool(in_workbookName + "." + entry.Title.Text + ".EXPORTSTATICDB", false))
                    staticDbEntries.Add(entry);
                else if (GetBool(in_workbookName + "." + entry.Title.Text + ".EXPORTNGUI", false))
                    nguiEntries.Add(entry);
                else if (GetBool(in_workbookName + "." + entry.Title.Text + ".EXPORTXML", false))
                    xmlEntries.Add(entry);
                else if (GetBool(in_workbookName + "." + entry.Title.Text + ".EXPORTJSON", false))
                    jsonEntries.Add(entry);
                else if (GetBool(in_workbookName + "." + entry.Title.Text + ".EXPORTDAIKONFORGE", false))
                    daikonforgeEntries.Add(entry);
                else if (GetBool(in_workbookName + "." + entry.Title.Text + ".EXPORTCSV", false))
                    csvEntries.Add(entry);
            }

            if (objDbEntries.Count > 0)
            {
                var path = GoogleFuGenPath("ObjDBResources");

                foreach (var entry in objDbEntries)
                {

                    var filename = MakeValidFileName(entry.Title.Text);

                    if (!ExportDatabase(path, filename, entry, false)) continue;

                    var dbattachName = GetString(_ActiveWorkbook.Title + "." + entry.Title.Text + ".OBJDB", System.String.Empty);
                    var dbattach = GameObject.Find(dbattachName);

                    var info = dbattach == null ?
                        new AdvancedDatabaseInfo(filename, entry, _Service, null, GetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".OBJDB" + ".PM", true), GetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".OBJDB" + ".UFR", true)) :
                        new AdvancedDatabaseInfo(filename, entry, _Service, dbattach, GetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".OBJDB" + ".PM", true), GetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".OBJDB" + ".UFR", true));
                    _AdvancedDatabaseObjectInfo.Add(info);


                    AssetDatabase.ImportAsset(filename, ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);
                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                }
            }

            if (staticDbEntries.Count > 0)
            {
                string path = GoogleFuGenPath("StaticDBResources");

                foreach (Google.GData.Spreadsheets.WorksheetEntry entry in staticDbEntries)
                {
                    var filename = MakeValidFileName(entry.Title.Text);
                    ExportDatabase(path, filename, entry, true);
                }
            }

            if (nguiEntries.Count > 0)
            {
                // output this book as localized
                ExportNgui(GoogleFuGenPath("NGUI"), nguiEntries);
            }

            if (xmlEntries.Count > 0)
            {

                string path = System.IO.Path.Combine(GoogleFuGenPath("XML"), sanitizedWorkbookName + ".xml");
                ExportXml(path, xmlEntries);
            }

            if (daikonforgeEntries.Count > 0)
            {
                string path = System.IO.Path.Combine(GoogleFuGenPath("DAIKONFORGE"), sanitizedWorkbookName + ".csv");
                ExportDaikonForge(path, daikonforgeEntries);
            }

            if (csvEntries.Count > 0)
            {
                string path = System.IO.Path.Combine(GoogleFuGenPath("CSV"), sanitizedWorkbookName + ".csv");
                ExportCsv(path, csvEntries, false);
            }

            if (jsonEntries.Count > 0)
            {
                var path = System.IO.Path.Combine(GoogleFuGenPath("JSON"), sanitizedWorkbookName);
                if (path.Length != 0)
                {
                    ExportJson(path, jsonEntries);
                }
            }

        }

        private static string MakeValidFileName(string in_name)
        {
            char[] invalidChars = System.IO.Path.GetInvalidFileNameChars();
            string tmp = "";
            foreach (char c in in_name)
            {
                bool foundInvalid = false;
                foreach (char ic in invalidChars)
                    if (c == ic)
                        foundInvalid = true;
                if (!foundInvalid)
                    tmp += c;
            }
            tmp = tmp.Replace(" ", System.String.Empty);
            return tmp;
        }

        void Indent(int in_tabs)
        {
            _CurIndent = in_tabs;
            GUILayout.Label("", GUILayout.Width(24 * in_tabs));
        }

        void DrawSelectAndExportGUI()
        {
            if (_ActiveWorkbook == null)
                return;

            EditorGUILayout.BeginHorizontal();
            DrawHelpButtonGUI("ID_HELP_EXPORT_OPTIONS");
            DrawLabelHeader("ID_EXPORT_OPTIONS");
            EditorGUILayout.EndHorizontal();

            // Trim Strings Checkbox
            EditorGUILayout.BeginHorizontal();
            Indent(1);
            DrawHelpButtonGUI("ID_HELP_TRIM_STRINGS");
            if (GUILayout.Toggle(_TrimStrings, Localize("ID_TRIM_STRINGS")))
            {
                SetBool("trimStrings", true);
                _TrimStrings = true;
            }
            else if (_TrimStrings)
            {
                SetBool("trimStrings", false);
                _TrimStrings = false;
            }
            EditorGUILayout.EndHorizontal();

            // Trim String Arrays Checkbox
            EditorGUILayout.BeginHorizontal();
            Indent(1);
            DrawHelpButtonGUI("ID_HELP_TRIM_STRING_ARRAYS");
            if (GUILayout.Toggle(_TrimStringArrays, Localize("ID_TRIM_STRING_ARRAYS")))
            {
                SetBool("trimStringArrays", true);
                _TrimStringArrays = true;
            }
            else if (_TrimStringArrays)
            {
                SetBool("trimStringArrays", false);
                _TrimStringArrays = false;
            }
            EditorGUILayout.EndHorizontal();

            // Array Delimiter Selection
            EditorGUILayout.BeginHorizontal();
            Indent(1);
            DrawHelpButtonGUI("ID_HELP_ARRAY_DELIMITERS"); 
            var oldArrayDelimiters = _ArrayDelimiters;
            _ArrayDelimiters = EditorGUILayout.TextField(Localize("ID_ARRAY_DELIMITERS"), oldArrayDelimiters);

            if (oldArrayDelimiters != _ArrayDelimiters)
            {
                SetString("arrayDelimiters", _ArrayDelimiters);
            }

            GUILayout.Label(System.String.Empty, GUILayout.Width(5));
            EditorGUILayout.EndHorizontal();

            // String Array Delimiter Section
            EditorGUILayout.BeginHorizontal();
            Indent(1);
            DrawHelpButtonGUI("ID_HELP_STRING_ARRAY_DELIMITERS");
            string oldStringArrayDelimiters = _StringArrayDelimiters;
            _StringArrayDelimiters = EditorGUILayout.TextField(Localize("ID_STRING_ARRAY_DELIMITERS"), oldStringArrayDelimiters);

            if (oldStringArrayDelimiters != _StringArrayDelimiters)
            {
                SetString("stringArrayDelimiters", _StringArrayDelimiters);
            }

            GUILayout.Label(System.String.Empty, GUILayout.Width(5));
            EditorGUILayout.EndHorizontal();

            // Complex Type Delimiter Selection
            EditorGUILayout.BeginHorizontal();
            Indent(1);
            DrawHelpButtonGUI("ID_HELP_COMPLEX_TYPE_DELIMITERS");
            string oldComplexTypeDelimiters = _ComplexTypeDelimiters;
            _ComplexTypeDelimiters = EditorGUILayout.TextField(Localize("ID_COMPLEX_TYPE_DELIMITERS"), oldComplexTypeDelimiters);

            if (oldComplexTypeDelimiters != _ComplexTypeDelimiters)
            {
                SetString("complexTypeDelimiters", _ComplexTypeDelimiters);
            }

            GUILayout.Label(System.String.Empty, GUILayout.Width(5));
            EditorGUILayout.EndHorizontal();

            // Complex Type Delimiter Selection
            EditorGUILayout.BeginHorizontal();
            Indent(1);
            DrawHelpButtonGUI("ID_HELP_COMPLEX_TYPE_ARRAY_DELIMITERS");
            string oldComplexTypeArrayDelimiters = _ComplexTypeArrayDelimiters;
            _ComplexTypeArrayDelimiters = EditorGUILayout.TextField(Localize("ID_COMPLEX_TYPE_ARRAY_DELIMITERS"), oldComplexTypeArrayDelimiters);

            if (oldComplexTypeArrayDelimiters != _ComplexTypeArrayDelimiters)
            {
                SetString("complexTypeArrayDelimiters", _ComplexTypeArrayDelimiters);
            }

            GUILayout.Label(System.String.Empty, GUILayout.Width(5));
            EditorGUILayout.EndHorizontal();


            var activeEntries = new System.Collections.Generic.List<Google.GData.Spreadsheets.WorksheetEntry>();

            EditorGUILayout.BeginHorizontal();
            DrawHelpButtonGUI("ID_HELP_EXPORT_WORKSHEETS");
            DrawLabelHeader("ID_SELECT_WORKSHEETS");
            EditorGUILayout.EndHorizontal();

            var allEntries = new System.Collections.Generic.List<Google.GData.Spreadsheets.WorksheetEntry>();
            if (_ActiveWorkbook.WorksheetEntries != null)
                allEntries.AddRange(_ActiveWorkbook.WorksheetEntries.Cast<Google.GData.Spreadsheets.WorksheetEntry>());
            if (_ActiveWorkbook.ManualEntries.Count > 0)
                allEntries.AddRange(_ActiveWorkbook.ManualEntries);

            if (allEntries.Count > 0)
            {
                foreach (var entry in allEntries)
                {

                    bool useEntry = GetBool(_ActiveWorkbook.Title + "." + entry.Title.Text, false);
                    EditorGUILayout.BeginHorizontal();
                    Indent(1);
                    DrawHelpButtonGUI("ID_HELP_SELECT_WORKSHEET");
                    if (GUILayout.Toggle(useEntry, entry.Title.Text))
                    {
                        SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text, true);
                        useEntry = true;
                    }
                    else if (useEntry)
                    {
                        SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text, false);
                        useEntry = false;
                    }
                    EditorGUILayout.EndHorizontal();


                    if (useEntry)
                    {
                        var useExportObjDb = GetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTOBJDB", false);
                        var useExportStaticDb = GetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTSTATICDB", false);
                        var useExportNgui = GetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTNGUI", false);
                        var useExportXml = GetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTXML", false);
                        var useExportJson = GetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTJSON", false);
                        var useExportDaikonForge = GetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTDAIKONFORGE", false);
                        var useExportCsv = GetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTCSV", false);

                        if (_UseObjDb)
                        {
                            EditorGUILayout.BeginHorizontal();
                            Indent(2);
                            DrawHelpButtonGUI("ID_HELP_EXPORT_AS_GAMEOBJECT");
                            if (DrawToggle("ID_EXPORT_OBJ_DB", useExportObjDb))
                            {
                                useExportObjDb = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTOBJDB", true);
                                useExportStaticDb = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTSTATICDB", false);
                                useExportNgui = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTNGUI", false);
                                useExportXml = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTXML", false);
                                useExportJson = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTJSON", false);
                                useExportDaikonForge = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTDAIKONFORGE", false);
                                useExportCsv = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTCSV", false);
                            }
                            else if (useExportObjDb)
                            {
                                SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTOBJDB", false);
                            }
                            EditorGUILayout.EndHorizontal();

                            if (useExportObjDb)
                            {
                                EditorGUILayout.BeginHorizontal();
                                Indent(3);
                                var oldUfr = GetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".OBJDB" + ".UFR", false);
                                DrawHelpButtonGUI("ID_HELP_EXPORT_FIRST_ROW_AS_VALUES");
                                if (DrawToggle("ID_EXPORT_UFR", oldUfr))
                                {
                                    SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".OBJDB" + ".UFR", true);
                                }
                                else if (oldUfr)
                                {
                                    SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".OBJDB" + ".UFR", false);
                                }
                                EditorGUILayout.EndHorizontal();

                                EditorGUILayout.BeginHorizontal();
                                Indent(3);
                                DrawHelpButtonGUI("ID_HELP_EXPORT_DO_NOT_DESTROY");
                                var oldDnd = GetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".OBJDB" + ".DND", false);
                                if (DrawToggle("ID_EXPORT_DND", oldDnd))
                                {
                                    SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".OBJDB" + ".DND", true);
                                }
                                else if (oldDnd)
                                {
                                    SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".OBJDB" + ".DND", false);
                                }
                                EditorGUILayout.EndHorizontal();

                                if (_FoundPlaymaker)
                                {
                                    EditorGUILayout.BeginHorizontal();
                                    Indent(3);
                                    var oldPm = GetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".OBJDB" + ".PM", false);
                                    DrawHelpButtonGUI("ID_HELP_EXPORT_PLAYMAKER_ACTIONS");
                                    if (DrawToggle("ID_EXPORT_PM", oldPm))
                                    {
                                        SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".OBJDB" + ".PM", true);
                                    }
                                    else if (oldPm)
                                    {
                                        SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".OBJDB" + ".PM", false);
                                    }
                                    EditorGUILayout.EndHorizontal();
                                }


                                GameObject oldObjDb = null;
                                var objDbString = GetString(_ActiveWorkbook.Title + "." + entry.Title.Text + ".OBJDB", System.String.Empty);
                                // Attempt the faster lookup first
                                if (objDbString != System.String.Empty && _AdvancedDatabaseObjects.ContainsKey(objDbString))
                                    oldObjDb = _AdvancedDatabaseObjects[objDbString];

                                // If that doesn't work, try the slow method
                                if (objDbString != System.String.Empty && oldObjDb == null)
                                    oldObjDb = GameObject.Find(objDbString);


                                EditorGUILayout.BeginHorizontal();
                                Indent(3);
                                DrawHelpButtonGUI("ID_HELP_EXPORT_TO_SPECIFIED_GAMEOBJECT");
                                GUILayout.Label(Localize("ID_EXPORT_SELECT_OBJECT"));
                                EditorGUILayout.EndHorizontal();

                                EditorGUILayout.BeginHorizontal();
                                Indent(3);
                                var newObjDb = (GameObject)EditorGUILayout.ObjectField(oldObjDb, typeof(GameObject), true, GUILayout.Width(140));

                                if (oldObjDb != newObjDb)
                                {
                                    if (newObjDb == null)
                                    {
                                        SetString(_ActiveWorkbook.Title + "." + entry.Title.Text + ".OBJDB", System.String.Empty);
                                    }
                                    else
                                    {
                                        SetString(_ActiveWorkbook.Title + "." + entry.Title.Text + ".OBJDB", newObjDb.name);
                                        if (_AdvancedDatabaseObjects.ContainsKey(newObjDb.name) == false)
                                            _AdvancedDatabaseObjects.Add(newObjDb.name, newObjDb);
                                    }
                                }
                                EditorGUILayout.EndHorizontal();
                            }
                        }
                        if (_UseStaticDb)
                        {
                            EditorGUILayout.BeginHorizontal();
                            Indent(2);
                            DrawHelpButtonGUI("ID_HELP_EXPORT_AS_STATIC");
                            if (DrawToggle("ID_EXPORT_STATIC_DB", useExportStaticDb))
                            {
                                useExportObjDb = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTOBJDB", false);
                                useExportStaticDb = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTSTATICDB", true);
                                useExportNgui = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTNGUI", false);
                                useExportXml = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTXML", false);
                                useExportJson = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTJSON", false);
                                useExportDaikonForge = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTDAIKONFORGE", false);
                                useExportCsv = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTCSV", false);
                            }
                            else if (useExportStaticDb)
                            {
                                SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTSTATICDB", false);
                            }

                            EditorGUILayout.EndHorizontal();

                            if (useExportStaticDb)
                            {
                                EditorGUILayout.BeginHorizontal();
                                Indent(3);
                                var oldUfr = GetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".STATICDB" + ".UFR", false);
                                DrawHelpButtonGUI("ID_HELP_EXPORT_FIRST_ROW_AS_VALUES");
                                if (DrawToggle("ID_EXPORT_UFR", oldUfr))
                                {
                                    SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".STATICDB" + ".UFR", true);
                                }
                                else if (oldUfr)
                                {
                                    SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".STATICDB" + ".UFR", false);
                                }
                                EditorGUILayout.EndHorizontal();
                            }
                        }
                        if (_FoundNgui && _UseNgui)
                        {
                            EditorGUILayout.BeginHorizontal();
                            Indent(2);
                            DrawHelpButtonGUI("ID_HELP_EXPORT_AS_NGUI");
                            if (DrawToggle("ID_EXPORT_NGUI", useExportNgui))
                            {
                                useExportObjDb = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTOBJDB", false);
                                useExportStaticDb = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTSTATICDB", false);
                                useExportNgui = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTNGUI", true);
                                useExportXml = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTXML", false);
                                useExportJson = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTJSON", false);
                                useExportDaikonForge = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTDAIKONFORGE", false);
                                useExportCsv = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTCSV", false);
                            }
                            else if (useExportNgui)
                            {
                                SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTNGUI", false);
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                        if (_FoundDaikonForge && _UseDaikonForge)
                        {
                            EditorGUILayout.BeginHorizontal();
                            Indent(2);
                            DrawHelpButtonGUI("ID_HELP_EXPORT_AS_DAIKONFORGE");
                            if (DrawToggle("ID_EXPORT_DAIKONFORGE", useExportDaikonForge))
                            {
                                useExportObjDb = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTOBJDB", false);
                                useExportStaticDb = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTSTATICDB", false);
                                useExportNgui = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTNGUI", false);
                                useExportXml = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTXML", false);
                                useExportJson = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTJSON", false);
                                useExportDaikonForge = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTDAIKONFORGE", true);
                                useExportCsv = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTCSV", false);
                            }
                            else if (useExportDaikonForge)
                            {
                                SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTDAIKONFORGE", false);
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                        if (_UseCsv)
                        {
                            EditorGUILayout.BeginHorizontal();
                            Indent(2);
                            DrawHelpButtonGUI("ID_HELP_EXPORT_AS_CSV");
                            if (DrawToggle("ID_EXPORT_CSV", useExportCsv))
                            {
                                useExportObjDb = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTOBJDB", false);
                                useExportStaticDb = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTSTATICDB", false);
                                useExportNgui = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTNGUI", false);
                                useExportXml = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTXML", false);
                                useExportJson = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTJSON", false);
                                useExportDaikonForge = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTDAIKONFORGE", false);
                                useExportCsv = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTCSV", true);
                            }
                            else if (useExportCsv)
                            {
                                SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTCSV", false);
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                        if (_UseXml)
                        {
                            EditorGUILayout.BeginHorizontal();
                            Indent(2);
                            DrawHelpButtonGUI("ID_HELP_EXPORT_AS_XML");
                            if (DrawToggle("ID_EXPORT_XML", useExportXml))
                            {
                                useExportObjDb = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTOBJDB", false);
                                useExportStaticDb = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTSTATICDB", false);
                                useExportNgui = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTNGUI", false);
                                useExportXml = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTXML", true);
                                useExportJson = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTJSON", false);
                                useExportDaikonForge = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTDAIKONFORGE", false);
                                useExportCsv = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTCSV", false);
                            }
                            else if (useExportXml)
                            {
                                SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTXML", false);
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                        if (_UseJson)
                        {
                            EditorGUILayout.BeginHorizontal();
                            Indent(2);
                            DrawHelpButtonGUI("ID_HELP_EXPORT_AS_JSON");
                            if (DrawToggle("ID_EXPORT_JSON", useExportJson))
                            {
                                useExportObjDb = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTOBJDB", false);
                                useExportStaticDb = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTSTATICDB", false);
                                useExportNgui = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTNGUI", false);
                                useExportXml = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTXML", false);
                                useExportJson = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTJSON", true);
                                useExportDaikonForge = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTDAIKONFORGE", false);
                                useExportCsv = SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTCSV", false);
                            }
                            else if (useExportJson)
                            {
                                SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".EXPORTJSON", false);
                            }

                            EditorGUILayout.EndHorizontal();

                            if (useExportJson)
                            {
                                EditorGUILayout.BeginHorizontal();
                                Indent(3);
                                bool oldEscapeUni = GetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".JSON" + ".ESCAPEUNICODE", true);
                                DrawHelpButtonGUI("ID_HELP_ESCAPE_UNICODE");
                                if (DrawToggle("ID_ESCAPE_UNICODE", oldEscapeUni))
                                {
                                    SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".JSON" + ".ESCAPEUNICODE", true);
                                }
                                else if (oldEscapeUni)
                                {
                                    SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".JSON" + ".ESCAPEUNICODE", false);
                                }
                                EditorGUILayout.EndHorizontal();

                                EditorGUILayout.BeginHorizontal();
                                Indent(3);
                                bool oldIncludeTypes = GetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".JSON" + ".INCLUDETYPEROW", true);
                                DrawHelpButtonGUI("ID_HELP_INCLUDE_TYPE_ROW");
                                if (DrawToggle("ID_INCLUDE_TYPE_ROW", oldIncludeTypes))
                                {
                                    SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".JSON" + ".INCLUDETYPEROW", true);
                                }
                                else if (oldIncludeTypes)
                                {
                                    SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".JSON" + ".INCLUDETYPEROW", false);
                                }
                                EditorGUILayout.EndHorizontal();

                                EditorGUILayout.BeginHorizontal();
                                Indent(3);
                                var oldCombineSheets = GetBool(_ActiveWorkbook.Title + "." + ".JSON" + ".COMBINESHEETS", true);
                                DrawHelpButtonGUI("ID_HELP_COMBINE_SHEETS");
                                if (DrawToggle("ID_COMBINE_SHEETS", oldCombineSheets))
                                {
                                    SetBool(_ActiveWorkbook.Title + "." + ".JSON" + ".COMBINESHEETS", true);
                                }
                                else if (oldCombineSheets)
                                {
                                    SetBool(_ActiveWorkbook.Title + "." + ".JSON" + ".COMBINESHEETS", false);
                                }
                                EditorGUILayout.EndHorizontal();

                                EditorGUILayout.BeginHorizontal();
                                Indent(3);
                                var oldConvertArrays = GetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".JSON" + ".CONVERTARRAYS", true);
                                DrawHelpButtonGUI("ID_HELP_CONVERT_ARRAYS");
                                if (DrawToggle("ID_CONVERT_ARRAYS", oldConvertArrays))
                                {
                                    SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".JSON" + ".CONVERTARRAYS", true);
                                }
                                else if (oldConvertArrays)
                                {
                                    SetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".JSON" + ".CONVERTARRAYS", false);
                                }
                                EditorGUILayout.EndHorizontal();

                            }
                        }

                        if ((_UseObjDb && useExportObjDb) ||
                            (_UseStaticDb && useExportStaticDb) ||
                            (_UseNgui && useExportNgui) ||
                            (_UseJson && useExportJson) ||
                            (_UseXml && useExportXml) ||
                            (_UseCsv && useExportCsv) ||
                            (_UseDaikonForge && useExportDaikonForge))
                            activeEntries.Add(entry);
                    }

                    EditorGUILayout.Space();
                }
            }

            if (activeEntries.Count <= 0)
                GUI.enabled = false;
            if (GUILayout.Button(Localize("ID_EXPORT")))
            {
                ClearMessages();
                Export(_ActiveWorkbook.Title, activeEntries);
            }
            GUI.enabled = true;

        }

        void SettingsGUI()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical(GUILayout.Width(120));
            GUI.backgroundColor = _BShowAuthentication ? _SelectedTabColor : _UnselectedTabColor;
            if (GUILayout.Button(Localize("ID_CREDENTIALS"), EditorStyles.toolbarButton))
            {
                ClearMessages();
                _BShowAuthentication = true;
                _BShowLanguage = false;
                _BShowPaths = false;
            }
            GUI.backgroundColor = _DefaultBgColor;

            GUI.backgroundColor = _BShowLanguage ? _SelectedTabColor : _UnselectedTabColor;
            if (GUILayout.Button(Localize("ID_LANGUAGE"), EditorStyles.toolbarButton))
            {
                ClearMessages();
                _BShowAuthentication = false;
                _BShowLanguage = true;
                _BShowPaths = false;
            }
            GUI.backgroundColor = _DefaultBgColor;

            GUI.backgroundColor = _BShowPaths ? _SelectedTabColor : _UnselectedTabColor;
            if (GUILayout.Button(Localize("ID_PATHS"), EditorStyles.toolbarButton))
            {
                ClearMessages();
                _BShowAuthentication = false;
                _BShowLanguage = false;
                _BShowPaths = true;
            }
            GUI.backgroundColor = _DefaultBgColor;
            EditorGUILayout.EndVertical();

            EditorGUILayout.Separator();

            EditorGUILayout.BeginVertical();
            if (_BShowAuthentication)
            {
                DrawAuthenticationGUI();
            }
            else if (_BShowLanguage)
            {
                DrawChooseLanguageGUI();
            }
            else if (_BShowPaths)
            {
                DrawCreatePathsGUI();
                DrawEnableDbObjGUI();
                DrawEnableStaticObjGUI();
                DrawChooseNguiPathGUI();
                DrawChooseXmlPathGUI();
                DrawChooseJsonPathGUI();
                DrawChoosePlaymakerPathGUI();
                DrawChooseDaikonForgePathGUI();
                DrawChooseCsvPathGUI();
            }
            EditorGUILayout.EndVertical();
            GUILayout.Label(System.String.Empty);
            EditorGUILayout.EndHorizontal();
        }

        void WorkbooksGUI()
        {
            if (_Authorized == false)
            {
                _BShowAccountWorkbooks = false;
                _BShowManualWorkbooks = true;
                _BShowCreateWorkbook = false;
            }


            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical(GUILayout.Width(120));
            GUI.backgroundColor = _BShowAccountWorkbooks ? _SelectedTabColor : _UnselectedTabColor;
            if (_Authorized == false)
                GUI.enabled = false;
            if (GUILayout.Button(Localize("ID_ACCOUNT_WORKBOOK"), EditorStyles.toolbarButton))
            {
                ClearMessages();
                _BShowAccountWorkbooks = true;
                _BShowManualWorkbooks = false;
                _BShowCreateWorkbook = false;
            }
            GUI.enabled = true;
            GUI.backgroundColor = _DefaultBgColor;

            GUI.backgroundColor = _BShowManualWorkbooks ? _SelectedTabColor : _UnselectedTabColor;
            if (GUILayout.Button(Localize("ID_MANUAL_WORKBOOK"), EditorStyles.toolbarButton))
            {
                ClearMessages();
                _BShowAccountWorkbooks = false;
                _BShowManualWorkbooks = true;
                _BShowCreateWorkbook = false;
            }
            GUI.backgroundColor = _DefaultBgColor;

            GUI.backgroundColor = _BShowCreateWorkbook ? _SelectedTabColor : _UnselectedTabColor;
            if (_Authorized == false)
                GUI.enabled = false;
            if (GUILayout.Button(Localize("ID_CREATE_WORKBOOK"), EditorStyles.toolbarButton))
            {
                ClearMessages();
                _BShowAccountWorkbooks = false;
                _BShowManualWorkbooks = false;
                _BShowCreateWorkbook = true;
            }
            GUI.enabled = true;
            GUI.backgroundColor = _DefaultBgColor;
            EditorGUILayout.EndVertical();

            EditorGUILayout.Separator();

            EditorGUILayout.BeginVertical();

            if (_BShowAccountWorkbooks)
            {
                DrawAccountWorkbooksGUI();
            }
            else if (_BShowManualWorkbooks)
            {
                DrawManualWorkbooksGUI();
            }
            else if (_BShowCreateWorkbook)
            {
                DrawCreateWorkbookGUI();
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        void ToolboxGUI()
        {
            DrawSelectAndExportGUI();
        }

        void DrawHelpMainGUI()
        {
            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();
            GUI.backgroundColor = Color.black;
            if (GUILayout.Button(_LitteratusLogo))
            {
                ClearMessages();
                Application.OpenURL("http://www.litteratus.net");
            }
            GUI.backgroundColor = _DefaultBgColor;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUI.backgroundColor = Color.black;
            if (GUILayout.Button(_UnityLogo))
            {
                ClearMessages();
                Application.OpenURL("http://www.unity3d.com");
            }
            GUI.backgroundColor = _DefaultBgColor;
            EditorGUILayout.EndHorizontal();
            GUILayout.Label(Localize("ID_CREATED_WITH_UNITY") + " \u00a9 " + Localize("ID_COPYRIGHT_UNITY"));
        }

        void DrawHelpDocEntry(string in_entryID, string in_title, string in_helpText, ref bool in_bPreviousActive)
        {
            if (EditorGUILayout.BeginToggleGroup(in_title, _CurrentHelpDoc == in_entryID && !in_bPreviousActive))
            {
                EditorGUILayout.HelpBox(in_helpText, MessageType.None);
                _CurrentHelpDoc = in_entryID;
                in_bPreviousActive = true;
            }
            EditorGUILayout.EndToggleGroup();

        }

        void DrawHelpDocsGUI()
        {
            bool bPreviousActive = false;

            DrawHelpDocEntry("HELP_LOGGING_IN", Localize("ID_HELP_LOGGING_IN_TITLE"), Localize("ID_HELP_LOGGING_IN_TEXT"), ref bPreviousActive);
            DrawHelpDocEntry("HELP_SAVE_CREDENTIALS", Localize("ID_HELP_SAVE_CREDENTIALS_TITLE"), Localize("ID_HELP_SAVE_CREDENTIALS_TEXT"), ref bPreviousActive);
            DrawHelpDocEntry("HELP_ACTIVE_WORKBOOK", Localize("ID_HELP_ACTIVE_WORKBOOK_TITLE"), Localize("ID_HELP_ACTIVE_WORKBOOK_TEXT"), ref bPreviousActive);
            DrawHelpDocEntry("HELP_NGUI_EXPORT", Localize("ID_HELP_NGUI_EXPORT_TITLE"), Localize("ID_HELP_NGUI_EXPORT_TEXT"), ref bPreviousActive);
            DrawHelpDocEntry("HELP_PLAYMAKER", Localize("ID_HELP_PLAYMAKER_TITLE"), Localize("ID_HELP_PLAYMAKER_TEXT"), ref bPreviousActive);
        }

        void HelpGUI()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical(GUILayout.Width(120));
            GUI.backgroundColor = _BShowMain ? _SelectedTabColor : _UnselectedTabColor;

            if (GUILayout.Button(Localize("ID_HELP_MAIN"), EditorStyles.toolbarButton))
            {
                ClearMessages();
                _BShowMain = true;
                _BShowDocs = false;
            }
            GUI.backgroundColor = _DefaultBgColor;

            GUI.backgroundColor = _BShowDocs ? _SelectedTabColor : _UnselectedTabColor;
            if (GUILayout.Button(Localize("ID_DOCUMENTATION"), EditorStyles.toolbarButton))
            {
                ClearMessages();
                _BShowMain = false;
                _BShowDocs = true;
            }
            GUI.backgroundColor = _DefaultBgColor;
            EditorGUILayout.EndVertical();

            EditorGUILayout.Separator();

            EditorGUILayout.BeginVertical();
            if (_BShowMain)
            {
                DrawHelpMainGUI();
            }
            else if (_BShowDocs)
            {
                DrawHelpDocsGUI();
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        bool CheckEnabledPaths()
        {

            // If we have NOTHING selected, that's a problem too
            if ((_UseObjDb == false) && (_UseStaticDb == false) &&
                (_FoundNgui && (_UseNgui == false)) && (_UseXml == false) && (_UseCsv == false) &&
                (_UseJson == false) && (_FoundPlaymaker && (_UsePlaymaker == false)) &&
                (_FoundDaikonForge && (_UseDaikonForge == false)))
                return false;

            if (_EditorPathInfo != System.String.Empty)
                return false;

            if ((_UseObjDb == false) || (_UseObjDb && (_ObjDbResourcesDirectory != System.String.Empty && _ObjDbEditorDirectory != System.String.Empty)))
                return true;
            if ((_UseStaticDb == false) || (_UseStaticDb && (_StaticDbResourcesDirectory != System.String.Empty)))
                return true;
            if (_FoundNgui && ((_UseNgui == false) || (_UseNgui && (_NguiDirectory != System.String.Empty))))
                return true;
            if ((_UseXml == false) || (_UseXml && (_XmlDirectory != System.String.Empty)))
                return true;
            if ((_UseJson == false) || (_UseJson && (_JsonDirectory == System.String.Empty)))
                return true;
            if ((_UseCsv == false) || (_UseCsv && (_CsvDirectory == System.String.Empty)))
                return true;
            if (_FoundPlaymaker && ((_UsePlaymaker == false) || (_UsePlaymaker && (_PlaymakerDirectory == System.String.Empty))))
                return true;
            if (_FoundDaikonForge && ((_UseDaikonForge == false) || (_UseDaikonForge && (_DaikonforgeDirectory == System.String.Empty))))
                return true;

            return false;

        }

        void GetPathErrors()
        {
            if (_UseObjDb && _ObjDbResourcesDirectory == System.String.Empty)
            {
                _EditorPathInfo = Localize("ID_ERROR_RESOURCES_DIRECTORY");
            }
            else if (_UseObjDb && _ObjDbEditorDirectory == System.String.Empty)
            {
                _EditorPathInfo = Localize("ID_ERROR_EDITOR_DIRECTORY");
            }
            else if (_UseStaticDb && _StaticDbResourcesDirectory == System.String.Empty)
            {
                _EditorPathInfo = Localize("ID_ERROR_RESOURCES_DIRECTORY");
            }
            else if (_FoundNgui && (_UseNgui && _NguiDirectory == System.String.Empty))
            {
                _EditorPathInfo = Localize("ID_ERROR_NGUI_DIRECTORY");
            }
            else if (_UseXml && _XmlDirectory == System.String.Empty)
            {
                _EditorPathInfo = Localize("ID_ERROR_XML_DIRECTORY");
            }
            else if (_UseJson && _JsonDirectory == System.String.Empty)
            {
                _EditorPathInfo = Localize("ID_ERROR_JSON_DIRECTORY");
            }
            else if (_UseCsv && _CsvDirectory == System.String.Empty)
            {
                _EditorPathInfo = Localize("ID_ERROR_CSV_DIRECTORY");
            }
            else if (_FoundPlaymaker && (_UsePlaymaker && _PlaymakerDirectory == System.String.Empty))
            {
                _EditorPathInfo = Localize("ID_ERROR_PLAYMAKER_DIRECTORY");
            }
            else if (_FoundDaikonForge && (_UseDaikonForge && _DaikonforgeDirectory == System.String.Empty))
            {
                _EditorPathInfo = Localize("ID_ERROR_DAIKONFORGE_DIRECTORY");
            }
            else
            {
                _EditorPathInfo = System.String.Empty;
            }
        }

        void OnGUI()
        {

            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.StandaloneWindows &&
                EditorUserBuildSettings.activeBuildTarget != BuildTarget.StandaloneWindows64 &&
                EditorUserBuildSettings.activeBuildTarget != BuildTarget.StandaloneOSXIntel &&
                EditorUserBuildSettings.activeBuildTarget != BuildTarget.StandaloneOSXIntel64 &&
                EditorUserBuildSettings.activeBuildTarget != BuildTarget.StandaloneOSXUniversal &&
                EditorUserBuildSettings.activeBuildTarget != BuildTarget.StandaloneLinux &&
                EditorUserBuildSettings.activeBuildTarget != BuildTarget.StandaloneLinux64 &&
                EditorUserBuildSettings.activeBuildTarget != BuildTarget.StandaloneLinuxUniversal &&
                EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android &&
				#if UNITY_5
                EditorUserBuildSettings.activeBuildTarget != BuildTarget.iOS &&
				#else
				EditorUserBuildSettings.activeBuildTarget != BuildTarget.iPhone &&
				#endif
                EditorUserBuildSettings.activeBuildTarget != BuildTarget.WP8Player)
            {
                EditorGUILayout.HelpBox(Localize("ID_ERROR_BUILD_TARGET"), MessageType.Error);
                return;
            }

            if (ShowUploadedNotification)
            {
                ShowNotification(new GUIContent("Successfully uploaded to Google"));
                Debug.Log("Successfully uploaded to Google");
                ShowUploadedNotification = false;
            }

            GetPathErrors();
            if (_EditorPathInfo != System.String.Empty)
            {
                if (_CurrentPage != GfPage.Help)
                {
                    _CurrentPage = GfPage.Settings;
                    _BShowAuthentication = false;
                    _BShowLanguage = false;
                    _BShowPaths = true;
                }
            }

            _IsProSkin = EditorGUIUtility.isProSkin;
            _DefaultBgColor = GUI.backgroundColor;

            _DefaultFgColor = GUI.color;
            if (_IsProSkin)
            {
                _LabelHeaderColor = Color.gray;
                _UnselectedTabColor = Color.gray;
                _SelectedTabColor = Color.green;
                _PathLabelBgColor = Color.gray;
                _PathLabelFgColor = Color.black;
                _LargeLabelStyle = EditorStyles.whiteLargeLabel;
            }
            else
            {
                _LabelHeaderColor = Color.black;
                _UnselectedTabColor = Color.grey;
                _SelectedTabColor = Color.green;
                _PathLabelBgColor = Color.gray;
                _PathLabelFgColor = Color.black;
                _LargeLabelStyle = EditorStyles.largeLabel;
            }



            if (_InitDraw < 2)
            {
                DrawLabelHeader("ID_INITIALIZING");
                _InitDraw++;
                return;
            }

            if (_Service == null)
            {
                Init();
            }

            if (_Authorized == false && _ActiveWorkbook == null && _CurrentPage == GfPage.Toolbox)
            {
                if (_Manualworkbooks.Count > 0)
                {
                    _EditorWarning = Localize("ID_ERROR_ACTIVATE_WORKBOOK");
                    _CurrentPage = GfPage.Workbooks;
                    _BShowAccountWorkbooks = false;
                    _BShowManualWorkbooks = true;
                    _BShowCreateWorkbook = false;
                }
                else
                {
                    _EditorWarning = Localize("ID_ERROR_ACTIVATE_WORKBOOK");
                    _CurrentPage = GfPage.Settings;
                    _BShowAuthentication = true;
                    _BShowLanguage = false;
                    _BShowPaths = false;
                }
            }
            else
                if (_UseObjDb == false &&
                    _UseStaticDb == false &&
                    _UseJson == false &&
                    _UseXml == false &&
                    _UseCsv == false &&
                    (_FoundNgui == false || (_FoundNgui && _UseNgui == false)) &&
                    (_FoundDaikonForge == false || (_FoundDaikonForge && _UseDaikonForge == false)) &&
                    _CurrentPage == GfPage.Toolbox)
                {
                    // Well you have to enable SOMETHING..
                    _EditorWarning = Localize("ID_NO_EXPORT_TYPE_ERROR");
                    _CurrentPage = GfPage.Settings;
                    _BShowAuthentication = false;
                    _BShowLanguage = false;
                    _BShowPaths = true;
                }

            EditorGUILayout.BeginVertical();
            if (!string.IsNullOrEmpty(_EditorWarning))
            {
                EditorGUILayout.HelpBox(_EditorWarning, MessageType.Warning);
            }
            if (!string.IsNullOrEmpty(_EditorInfo))
            {
                EditorGUILayout.HelpBox(_EditorInfo, MessageType.Error);
            }
            if (!string.IsNullOrEmpty(_EditorWorking))
            {
                EditorGUILayout.HelpBox(_EditorWorking, MessageType.Info);
            }
            if (!string.IsNullOrEmpty(_EditorPathInfo))
            {
                EditorGUILayout.HelpBox(_EditorPathInfo, MessageType.Error);
            }
            if (CreatingWorkbook == false && !string.IsNullOrEmpty(EditorException))
            {
                EditorGUILayout.HelpBox(EditorException, MessageType.Error);
            }
            EditorGUILayout.EndVertical();

            if (_ActiveWorkbook != null)
            {
                DrawActiveWorkbookGUI();
            }

            EditorGUILayout.Separator();

            bool checkpaths = CheckEnabledPaths();

            EditorGUILayout.BeginHorizontal();

            GUI.backgroundColor = _DefaultBgColor;
            GUI.enabled = true;
            GUI.backgroundColor = _CurrentPage != GfPage.Settings ? _UnselectedTabColor : _SelectedTabColor;
            if (GUILayout.Button(Localize("ID_SETTINGS"), EditorStyles.toolbarButton))
            {
                ClearMessages();
                _CurrentPage = GfPage.Settings;
            }

            GUI.enabled = true;
            GUI.backgroundColor = _DefaultBgColor;
            if (checkpaths == false)
            {
                GUI.enabled = false;
            }
            else if (_CurrentPage != GfPage.Workbooks)
            {
                GUI.backgroundColor = _UnselectedTabColor;
            }
            else
            {
                GUI.backgroundColor = _SelectedTabColor;
            }

            if (GUILayout.Button(Localize("ID_WORKBOOKS"), EditorStyles.toolbarButton))
            {
                ClearMessages();
                _CurrentPage = GfPage.Workbooks;
            }

            GUI.enabled = true;
            GUI.backgroundColor = _DefaultBgColor;
            if (checkpaths == false)
            {
                GUI.enabled = false;
            }
            GUI.backgroundColor = _CurrentPage != GfPage.Toolbox ? _UnselectedTabColor : _SelectedTabColor;
            if (GUILayout.Button(Localize("ID_TOOLS"), EditorStyles.toolbarButton))
            {
                ClearMessages();
                _CurrentPage = GfPage.Toolbox;
            }

            GUI.backgroundColor = _DefaultBgColor;
            GUI.enabled = true;
            GUI.backgroundColor = _CurrentPage != GfPage.Help ? _UnselectedTabColor : _SelectedTabColor;
            if (GUILayout.Button(Localize("ID_HELP"), EditorStyles.toolbarButton))
            {
                ClearMessages();
                _CurrentPage = GfPage.Help;
            }

            GUI.enabled = true;
            GUI.backgroundColor = _DefaultBgColor;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            _ScrollPos = EditorGUILayout.BeginScrollView(_ScrollPos);

            switch (_CurrentPage)
            {
                case GfPage.Settings:
                    SettingsGUI();
                    break;
                case GfPage.Workbooks:
                    WorkbooksGUI();
                    break;
                case GfPage.Toolbox:
                    ToolboxGUI();
                    break;
                case GfPage.Help:
                    HelpGUI();
                    break;
            }

            EditorGUILayout.LabelField("\n\n\n\n\n\n", EditorStyles.wordWrappedLabel);

            EditorGUILayout.EndScrollView();


        }

        void ExportNgui(string in_path, System.Collections.Generic.IEnumerable<Google.GData.Spreadsheets.WorksheetEntry> in_entries)
        {
            if (_FoundNgui == false)
                return;

            if (_UseNguiLegacy)
            {
                ExportNguiLegacy(in_path, in_entries);
                return;
            }

            ExportCsv(in_path + "/Localization.csv", in_entries, true);
        }

        void ExportNguiLegacy(string in_path, System.Collections.Generic.IEnumerable<Google.GData.Spreadsheets.WorksheetEntry> in_entries)
        {
            if (_FoundNgui == false)
                return;

            ShowNotification(new GUIContent("Saving to: " + in_path));
            Debug.Log("Saving to: " + in_path);

            var languages = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, string>>();

            // for each page
            foreach (Google.GData.Spreadsheets.WorksheetEntry entry in in_entries)
            {
                // Define the URL to request the list feed of the worksheet.
                var listFeedLink = entry.Links.FindService(Google.GData.Spreadsheets.GDataSpreadsheetsNameTable.ListRel, null);

                // Fetch the list feed of the worksheet.
                var listQuery = new Google.GData.Spreadsheets.ListQuery(listFeedLink.HRef.ToString());
                var listFeed = _Service.Query(listQuery);

                var curCol = 0;
                if (listFeed.Entries.Count <= 0) continue;

                foreach (var atomEntry in listFeed.Entries)
                {
                    var row = (Google.GData.Spreadsheets.ListEntry)atomEntry;

                    foreach (Google.GData.Spreadsheets.ListEntry.Custom element in row.Elements)
                    {
                        if (curCol > 0)
                        {
                            if (!languages.ContainsKey(element.LocalName))
                                languages.Add(element.LocalName, new System.Collections.Generic.Dictionary<string, string>());
                            languages[element.LocalName].Add(row.Title.Text, element.Value);
                        }
                        curCol++;
                    }

                    curCol = 0;
                }
            }

            foreach (var lang in languages)
            {
                var filepath = in_path + "/" + lang.Key + ".txt";
                var fs = System.IO.File.Open(filepath, System.IO.File.Exists(filepath) ?
                    System.IO.FileMode.Truncate :
                    System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);

                var sw = new System.IO.StreamWriter(fs);
                var fileString = System.String.Empty;

                fileString += FormatLine("Flag = Flag-" + lang.Key);
                fileString = lang.Value.Aggregate(fileString, (in_current, in_word) => in_current + FormatLine(in_word.Key + " = " + in_word.Value));
                sw.Write(fileString);
                sw.Close();
                fs.Close();
            }
        }

        //////////////////////////////////////
        // format from: http://www.daikonforge.com/docs/df-gui/classdf_language_manager.html
        //Data File Format
        //
        //All data files must conform to the following format in order to be used by the dfLanguageManager class.
        //Localization data is stored as comma-seperated values (CSV) in a text file which must follow these rules:
        //
        //Each record is located on a separate line, delimited by a newline (LF) character.
        //	Note that CRLF-style line breaks will be converted internally during processing to single-LF characters.
        //The last record in the file may or may not have an ending newline.
        //The first line of the file must contain a header record in the same format as normal record lines, containing names
        //	corresponding to the fields in the file and should contain the same number of fields as the records in the rest of the file.
        //	The name of the first field is not used, but is KEY by default. The following fields must be an uppercase two-letter ISO 639-1
        //  country code that indicates the language for that column.
        //Within the header and each record, there may be one or more fields, separated by commas.
        //  Each line should contain the same number of fields throughout the file.
        //Fields containing newline characters, double-quote characters, or comma characters must be enclosed in double-quotes.
        //If double-quotes are used to enclose fields, then a double-quote appearing inside a field must
        //  be escaped by preceding it with another double quote.
        //Example:
        //
        //  KEY,EN,ES,FR,DE
        //  GREET,"Greetings, citizen!","Saludos, ciudadano!","Salutations, citoyens!","Grüße, Bürger!"
        //  ENTER,Enter the door,Entra por la puerta,Entrez dans la porte,Geben Sie die Tür
        //  QUOTE,"""Quickly now!"", he said","""¡Rápido!"", Dijo","""Vite!"", At-il dit","""Schnell jetzt!"", Sagte er"
        //
        //
        //	The goal here is to take all sheets selected as Daikon Forge output and compine them into one CSV
        //  So if the user splits the localization into multiple sheets, there will still only be 1 CSV file at the end
        ////////////////////////////////////

        void ExportDaikonForge(string in_path, System.Collections.Generic.IEnumerable<Google.GData.Spreadsheets.WorksheetEntry> in_entries)
        {
            if (_FoundDaikonForge == false)
                return;

            ExportCsv(in_path, in_entries, true);
        }

        void ExportCsv(string in_path, System.Collections.Generic.IEnumerable<Google.GData.Spreadsheets.WorksheetEntry> in_entries, bool in_bSanitize)
        {
            ShowNotification(new GUIContent("Saving to: " + in_path));
            Debug.Log("Saving to: " + in_path);

            // System.Collections.Generic.Dictionary< String Key, System.Collections.Generic.Dictionary< Language, String Value > >
            var allRows = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, string>>();
            var colHeaders = new System.Collections.Generic.List<string>();
            // for each page


            foreach (var listFeed in in_entries.Select(in_entry => in_entry.Links.FindService(Google.GData.Spreadsheets.GDataSpreadsheetsNameTable.ListRel, null)).Select(in_listFeedLink => new Google.GData.Spreadsheets.ListQuery(in_listFeedLink.HRef.ToString())).Select(in_listQuery => _Service.Query(in_listQuery)).Where(in_listFeed => in_listFeed.Entries.Count > 0))
            {
                var curCol = 0;
                // Iterate through each row, printing its cell values.
                foreach (var atomEntry in listFeed.Entries)
                {
                    var row = (Google.GData.Spreadsheets.ListEntry)atomEntry;
                    // Don't process rows marked for _Ignore
                    if (GfuStrCmp(row.Title.Text, "VOID"))
                    {
                        continue;
                    }

                    // Iterate over the columns, and print each cell value
                    foreach (Google.GData.Spreadsheets.ListEntry.Custom element in row.Elements)
                    {
                        // Don't process columns marked for _Ignore
                        if (GfuStrCmp(element.LocalName, "VOID"))
                        {
                            curCol++;
                            continue;
                        }

                        if (curCol > 0)
                        {
                            if (!allRows.ContainsKey(row.Title.Text))
                                allRows.Add(row.Title.Text, new System.Collections.Generic.Dictionary<string, string>());
                            allRows[row.Title.Text].Add(element.LocalName, element.Value);

                            // Maintain a single list of available column headers, we will use this to
                            // iterate the columns later
                            if (!colHeaders.Contains(element.LocalName))
                                colHeaders.Add(element.LocalName);
                        }
                        curCol++;
                    }

                    curCol = 0;
                }
            }

            var fs = System.IO.File.Open(in_path, System.IO.File.Exists(in_path) ? System.IO.FileMode.Truncate : System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);

            var sw = new System.IO.StreamWriter(fs);
            string fileString = "KEY,";

            foreach (var colHeader in colHeaders)
            {
                fileString += colHeader;
                if (colHeader != colHeaders[colHeaders.Count - 1])
                    fileString += ",";
            }
            fileString += System.Environment.NewLine;

            foreach (var curRow in allRows)
            {

                fileString += curRow.Key + ",";
                System.Collections.Generic.Dictionary<string, string> rowValue = curRow.Value;

                foreach (string colHeader in colHeaders)
                {
                    if (rowValue.ContainsKey(colHeader))
                    {
                        if (in_bSanitize)
                            fileString += SanitizeDf(rowValue[colHeader]);
                        else
                            fileString += rowValue[colHeader];
                    }
                    if (colHeader != colHeaders[colHeaders.Count - 1])
                        fileString += ",";
                }
                fileString += System.Environment.NewLine;
            }

            sw.Write(fileString);
            sw.Close();

            fs.Close();
        }

        static string SanitizeDf(string in_inString)
        {
            in_inString = in_inString.Replace("\"", "\"\"");
            return "\"" + in_inString + "\"";
        }

        void ExportXml(string in_path, System.Collections.Generic.IEnumerable<Google.GData.Spreadsheets.WorksheetEntry> in_entries)
        {

            ShowNotification(new GUIContent("Saving to: " + in_path));
            Debug.Log("Saving to: " + in_path);

            // Create the System.Xml.XmlDocument.
            var xmlDoc = new System.Xml.XmlDocument();
            var rootNode = xmlDoc.CreateElement("Sheets");
            xmlDoc.AppendChild(rootNode);

            foreach (Google.GData.Spreadsheets.WorksheetEntry entry in in_entries)
            {
                // Define the URL to request the list feed of the worksheet.
                Google.GData.Client.AtomLink listFeedLink = entry.Links.FindService(Google.GData.Spreadsheets.GDataSpreadsheetsNameTable.ListRel, null);

                // Fetch the list feed of the worksheet.
                var listQuery = new Google.GData.Spreadsheets.ListQuery(listFeedLink.HRef.ToString());
                var listFeed = _Service.Query(listQuery);

                //int rowCt = listFeed.Entries.Count;
                //int colCt = ((Google.GData.Spreadsheets.ListEntry)listFeed.Entries[0]).Elements.Count;

                System.Xml.XmlNode sheetNode = xmlDoc.CreateElement("sheet");
                System.Xml.XmlAttribute sheetName = xmlDoc.CreateAttribute("name");
                sheetName.Value = entry.Title.Text;
                if (sheetNode.Attributes != null)
                {
                    sheetNode.Attributes.Append(sheetName);
                    rootNode.AppendChild(sheetNode);

                    if (listFeed.Entries.Count <= 0) continue;
                    // Iterate through each row, printing its cell values.
                    foreach (var atomEntry in listFeed.Entries)
                    {
                        var row = (Google.GData.Spreadsheets.ListEntry)atomEntry;
                        // Don't process rows or columns marked for _Ignore
                        if (GfuStrCmp(row.Title.Text, "VOID"))
                        {
                            continue;
                        }

                        System.Xml.XmlNode rowNode = xmlDoc.CreateElement("row");
                        System.Xml.XmlAttribute rowName = xmlDoc.CreateAttribute("name");
                        rowName.Value = row.Title.Text;
                        if (rowNode.Attributes == null) continue;
                        rowNode.Attributes.Append(rowName);
                        sheetNode.AppendChild(rowNode);

                        // Iterate over the remaining columns, and print each cell value
                        foreach (Google.GData.Spreadsheets.ListEntry.Custom element in row.Elements)
                        {
                            // Don't process rows or columns marked for _Ignore
                            if (GfuStrCmp(element.LocalName, "VOID"))
                            {
                                continue;
                            }

                            System.Xml.XmlNode colNode = xmlDoc.CreateElement("col");
                            System.Xml.XmlAttribute colName = xmlDoc.CreateAttribute("name");
                            colName.Value = element.LocalName;
                            if (colNode.Attributes != null) colNode.Attributes.Append(colName);
                            colNode.InnerText = element.Value;
                            rowNode.AppendChild(colNode);
                        }
                    }
                }
            }

            // Save the document to a file and auto-indent the output.
            var writer = new System.Xml.XmlTextWriter(in_path, null) { Formatting = System.Xml.Formatting.Indented };
            xmlDoc.Save(writer);
            writer.Close();
            ShowNotification(new GUIContent("Saving to: " + in_path));
            Debug.Log("Saving to: " + in_path);
        }

        private static bool GfuStrCmp(string in_1, string in_2)
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

        private static bool GfuStartsWith(string in_1, string in_2)
        {
            return (in_1.StartsWith(in_2, System.StringComparison.InvariantCultureIgnoreCase));
        }

        //////////////////////////////////////
        // format
        //{
        //    "sheetName": 
        //    [
        //        {
        //            "colName": "value",
        //            "lastName": "value2"
        //        },
        //        {
        //            "colName": "value",
        //            "colName2": "value2"
        //        }
        //    ]
        //}
        ////////////////////////////////////
        void ExportJson(string in_path, System.Collections.Generic.List<Google.GData.Spreadsheets.WorksheetEntry> in_entries)
        {
            var bCombineSheets = GetBool(_ActiveWorkbook.Title + "." + ".JSON" + ".COMBINESHEETS", true);

            if (bCombineSheets)
            {
                var jsonPath = in_path + ".json";

                Debug.Log("Saving to: " + jsonPath);

                var fs = System.IO.File.Open(jsonPath, System.IO.File.Exists(jsonPath) ?
                    System.IO.FileMode.Truncate :
                    System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);

                var sw = new System.IO.StreamWriter(fs);

                ExportJsonWorkbook(sw, in_entries);

                sw.Close();
                fs.Close();

                ShowNotification(new GUIContent("Saving to: " + jsonPath));
            }
            else
            {


                if (!System.IO.Directory.Exists(in_path))
                    System.IO.Directory.CreateDirectory(in_path);

                foreach (var entry in in_entries)
                {
                    var jsonPath = in_path + "\\" + entry.Title.Text + ".json";
                    Debug.Log("Saving to: " + jsonPath);

                    var fs = System.IO.File.Open(jsonPath, System.IO.File.Exists(jsonPath) ?
                        System.IO.FileMode.Truncate :
                        System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);

                    var sw = new System.IO.StreamWriter(fs);

                    ExportJsonWorksheet(sw, entry);

                    sw.Close();
                    fs.Close();
                }

                ShowNotification(new GUIContent("Saving to: " + in_path));

            }
        }

        private void ExportJsonWorksheet(System.IO.StreamWriter in_sw, Google.GData.Spreadsheets.WorksheetEntry in_entry)
        {
            var fileString = System.String.Empty;

            // Define the URL to request the list feed of the worksheet.
            var listFeedLink = in_entry.Links.FindService(Google.GData.Spreadsheets.GDataSpreadsheetsNameTable.ListRel, null);

            // Fetch the list feed of the worksheet.
            var listQuery = new Google.GData.Spreadsheets.ListQuery(listFeedLink.HRef.ToString());
            var listFeed = _Service.Query(listQuery);

            var escapeUnicode = GetBool(_ActiveWorkbook.Title + "." + in_entry.Title.Text + ".JSON" + ".ESCAPEUNICODE", true);
            var includeTypeRow = GetBool(_ActiveWorkbook.Title + "." + in_entry.Title.Text + ".JSON" + ".INCLUDETYPEROW", false);
            var bConvertArrays = GetBool(_ActiveWorkbook.Title + "." + in_entry.Title.Text + ".JSON" + ".CONVERTARRAYS", true);

            // This is always row 0. This may or may not be valid types
            var typesList = new System.Collections.Generic.List<string>();

            fileString += ("[");

            var rowCt = listFeed.Entries.Count;
            var voidRows = 0;

            if (rowCt > 0)
            {

                var colCt = ((Google.GData.Spreadsheets.ListEntry)listFeed.Entries[0]).Elements.Count;

                // We need to make sure we don't process the last column if it's flagged as _Ignore.
                while ((colCt > 0) && GfuStrCmp(((Google.GData.Spreadsheets.ListEntry)listFeed.Entries[0]).Elements[colCt - 1].LocalName, "VOID"))
                    colCt -= 1;

                var curRow = 0;
                var curCol = 0;

                // Iterate through each row, printing its cell values.
                foreach (var row in listFeed.Entries.Cast<Google.GData.Spreadsheets.ListEntry>())
                {
                    // if we are skipping the type row, record the types and increment curRow now
                    if (curRow == 0)
                    {
                        typesList.AddRange(from Google.GData.Spreadsheets.ListEntry.Custom element in row.Elements select element.Value);

                        if (includeTypeRow == false)
                        {
                            curRow++;
                            continue;
                        }
                    }

                    if (GfuStrCmp(row.Title.Text, "void"))
                    {
                        curRow++;
                        voidRows++;
                        continue;
                    }


                    fileString += ("{");
                    // Iterate over the remaining columns, and print each cell value
                    foreach (Google.GData.Spreadsheets.ListEntry.Custom element in row.Elements)
                    {
                        // Don't process rows or columns marked for ignore
                        if (GfuStrCmp(element.LocalName, "void"))
                        {
                            curCol++;
                            continue;
                        }

                        if (bConvertArrays && IsSupportedArrayType(typesList[curCol]))
                        {

                            var delim = GetString("arrayDelimiters", _ArrayDelimiters).ToCharArray();

                            fileString += "\"" + SanitizeJson(element.LocalName, escapeUnicode) + "\":[";

                            bool isString = false;
                            if (GfuStrCmp(typesList[curCol], "string array") ||
                                GfuStrCmp(typesList[curCol], "string []") ||
                                GfuStrCmp(typesList[curCol], "string[]"))
                            {
                                delim = GetString("stringArrayDelimiters", _StringArrayDelimiters).ToCharArray();
                                isString = true;
                            }
                            if (curCol == 0)
                                isString = true;

                            var value = element.Value.Split(delim, System.StringSplitOptions.RemoveEmptyEntries);
                            var ct = 0;
                            foreach (var s in value)
                            {
                                if (isString)
                                    fileString += "\"" + SanitizeJson(s, escapeUnicode) + "\"";
                                else if (GfuStrCmp(typesList[curCol], "bool"))
                                {
                                    string val = s.ToLower();
                                    if (val == "1")
                                        val = "true";
                                    if (val == "0")
                                        val = "false";
                                    fileString += SanitizeJson(val, escapeUnicode);
                                }
                                else
                                    fileString += SanitizeJson(s, escapeUnicode);
                                if (ct < value.Length - 1)
                                    fileString += ",";
                                ct++;
                            }

                            fileString += "]";
                        }
                        else
                        {
                            if (GfuStrCmp(typesList[curCol], "string") || (curCol == 0))
                                fileString += ("\"" + SanitizeJson(element.LocalName, escapeUnicode) + "\":\"" + SanitizeJson(element.Value, escapeUnicode) + "\"");
                            else if (GfuStrCmp(typesList[curCol], "bool"))
                            {
                                string val = element.Value.ToLower();
                                if (val == "1")
                                    val = "true";
                                if (val == "0")
                                    val = "false";
                                fileString += ("\"" + SanitizeJson(element.LocalName, escapeUnicode) + "\":" +
                                               SanitizeJson(val, escapeUnicode));
                            }
                            else
                                fileString += ("\"" + SanitizeJson(element.LocalName, escapeUnicode) + "\":" +
                                               SanitizeJson(element.Value, escapeUnicode) + "");
                        }

                        if (curCol < colCt - 1)
                            fileString += ",";

                        curCol++;

                    }

                    fileString += ("}");
                    if (curRow < rowCt - voidRows - 1)
                        fileString += ",";

                    curCol = 0;
                    curRow++;
                }

            }

            fileString += ("]");
            in_sw.Write(fileString);
        }

        private void ExportJsonWorkbook(System.IO.StreamWriter in_sw, System.Collections.Generic.List<Google.GData.Spreadsheets.WorksheetEntry> in_entries)
        {
            string fileString = System.String.Empty;
            fileString += ("{");

            var sheetCount = in_entries.Count;
            var curSheet = 0;
            // for each page
            foreach (Google.GData.Spreadsheets.WorksheetEntry entry in in_entries)
            {
                // Define the URL to request the list feed of the worksheet.
                var listFeedLink = entry.Links.FindService(Google.GData.Spreadsheets.GDataSpreadsheetsNameTable.ListRel, null);

                // Fetch the list feed of the worksheet.
                var listQuery = new Google.GData.Spreadsheets.ListQuery(listFeedLink.HRef.ToString());
                var listFeed = _Service.Query(listQuery);

                var escapeUnicode = GetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".JSON" + ".ESCAPEUNICODE", true);
                var includeTypeRow = GetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".JSON" + ".INCLUDETYPEROW", false);
                var bConvertArrays = GetBool(_ActiveWorkbook.Title + "." + entry.Title.Text + ".JSON" + ".CONVERTARRAYS", true);

                // This is always row 0. This may or may not be valid types
                var typesList = new System.Collections.Generic.List<string>();

                fileString += ("\"" + SanitizeJson(listFeed.Title.Text, escapeUnicode) + "\":["); // "sheetName":[

                var rowCt = listFeed.Entries.Count;
                var voidRows = 0;
                if (rowCt > 0)
                {

                    var colCt = ((Google.GData.Spreadsheets.ListEntry)listFeed.Entries[0]).Elements.Count;

                    // We need to make sure we don't process the last column if it's flagged as _Ignore.
                    while ((colCt > 0) && GfuStrCmp(((Google.GData.Spreadsheets.ListEntry)listFeed.Entries[0]).Elements[colCt - 1].LocalName, "VOID"))
                        colCt -= 1;

                    var curRow = 0;
                    var curCol = 0;

                    // Iterate through each row, printing its cell values.
                    foreach (var row in listFeed.Entries.Cast<Google.GData.Spreadsheets.ListEntry>())
                    {
                        // if we are skipping the type row, record the types and increment curRow now
                        if (curRow == 0)
                        {
                            typesList.AddRange(from Google.GData.Spreadsheets.ListEntry.Custom element in row.Elements select element.Value);

                            if (includeTypeRow == false)
                            {
                                curRow++;
                                continue;
                            }
                        }

                        if (GfuStrCmp(row.Title.Text, "void"))
                        {
                            curRow++;
                            voidRows++;
                            continue;
                        }


                        fileString += ("{");
                        // Iterate over the remaining columns, and print each cell value
                        foreach (Google.GData.Spreadsheets.ListEntry.Custom element in row.Elements)
                        {
                            // Don't process rows or columns marked for ignore
                            if (GfuStrCmp(element.LocalName, "void"))
                            {
                                curCol++;
                                continue;
                            }

                            if (bConvertArrays && IsSupportedArrayType(typesList[curCol]))
                            {

                                var delim = GetString("arrayDelimiters", _ArrayDelimiters).ToCharArray();

                                fileString += "\"" + SanitizeJson(element.LocalName, escapeUnicode) + "\":[";
                                bool isString = false;

                                if (GfuStrCmp(typesList[curCol], "string array") ||
                                    GfuStrCmp(typesList[curCol], "string []") ||
                                    GfuStrCmp(typesList[curCol], "string[]"))
                                {
                                    delim = GetString("stringArrayDelimiters", _StringArrayDelimiters).ToCharArray();
                                    isString = true;
                                }
                                if (curCol == 0)
                                    isString = true;

                                var value = element.Value.Split(delim, System.StringSplitOptions.RemoveEmptyEntries);
                                var ct = 0;
                                foreach (var s in value)
                                {
                                    if (isString)
                                        fileString += "\"" + SanitizeJson(s, escapeUnicode) + "\"";
                                    else if (GfuStrCmp(typesList[curCol], "bool"))
                                    {
                                        string val = s.ToLower();
                                        if (val == "1")
                                            val = "true";
                                        if (val == "0")
                                            val = "false";
                                        fileString += SanitizeJson(val, escapeUnicode);
                                    }
                                    else
                                        fileString += SanitizeJson(s, escapeUnicode);
                                    if (ct < value.Length - 1)
                                        fileString += ",";
                                    ct++;
                                }

                                fileString += "]";
                            }
                            else
                            {
                                if (GfuStrCmp(typesList[curCol], "string") || (curCol == 0))
                                    fileString += ("\"" + SanitizeJson(element.LocalName, escapeUnicode) + "\":\"" + SanitizeJson(element.Value, escapeUnicode) + "\"");
                                else if (GfuStrCmp(typesList[curCol], "bool"))
                                {
                                    string val = element.Value.ToLower();
                                    if (val == "1")
                                        val = "true";
                                    if (val == "0")
                                        val = "false";
                                    fileString += ("\"" + SanitizeJson(element.LocalName, escapeUnicode) + "\":" +
                                                   SanitizeJson(val, escapeUnicode));
                                }
                                else
                                    fileString += ("\"" + SanitizeJson(element.LocalName, escapeUnicode) + "\":" + SanitizeJson(element.Value, escapeUnicode) + "");
                            }

                            if (curCol < colCt - 1)
                                fileString += ",";

                            curCol++;

                        }

                        fileString += ("}");
                        if (curRow < rowCt - voidRows - 1)
                            fileString += ",";

                        curCol = 0;
                        curRow++;
                    }

                }

                fileString += ("]");
                if (curSheet < sheetCount - 1)
                    fileString += ",";

                curSheet++;

            }
            fileString += ("}");
            in_sw.Write(fileString);
        }


        static string SanitizeJson(string in_value, bool in_escapeUnicode)
        {
            var sb = new System.Text.StringBuilder();
            foreach (var c in in_value)
            {
                if ((c > 127) && (in_escapeUnicode))
                {
                    // change this character into a unicode escape
                    string encodedValue = "\\u" + ((int)c).ToString("x4");
                    sb.Append(encodedValue);
                }
                else
                {
                    switch (c)
                    {
                        case '\n':
                            sb.Append("\\n");
                            break;
                        case '\r':
                            sb.Append("\\r");
                            break;
                        case '\t':
                            sb.Append("\\t");
                            break;
                        case '\b':
                            sb.Append("\\b");
                            break;
                        case '\a':
                            sb.Append("\\a");
                            break;
                        case '\f':
                            sb.Append("\\f");
                            break;
                        case '\\':
                            sb.Append("\\\\");
                            break;
                        case '\"':
                            sb.Append("\\\"");
                            break;
                        default:
                            sb.Append(c);
                            break;
                    }
                }
            }
            return sb.ToString();
        }

        static string StripArray(string in_string)
        {
            var strippedVarName = in_string;
            var lastIndex = strippedVarName.LastIndexOf("array", System.StringComparison.InvariantCultureIgnoreCase);
            if (lastIndex > -1)
                strippedVarName = strippedVarName.Substring(0, lastIndex).Trim();

            lastIndex = strippedVarName.LastIndexOf("[", System.StringComparison.InvariantCultureIgnoreCase);
            if (lastIndex > -1)
                strippedVarName = strippedVarName.Substring(0, lastIndex).Trim();

            return strippedVarName;
        }

        bool ExportDatabase(string in_path, string in_fileName, Google.GData.Spreadsheets.WorksheetEntry in_entry, bool in_staticClass)
        {

            ////////////////////////////////////////////
            // gathering the data
            var types = new System.Collections.Generic.List<string>();
            var colNames = new System.Collections.Generic.List<string>();
            var varNames = new System.Collections.Generic.List<string>();
            var rowNames = new System.Collections.Generic.List<string>();

            bool typesInFirstRow = in_staticClass ?
                GetBool(_ActiveWorkbook.Title + "." + in_entry.Title.Text + ".STATICDB" + ".UFR", false) :
                GetBool(_ActiveWorkbook.Title + "." + in_entry.Title.Text + ".OBJDB" + ".UFR", false);

            if(in_staticClass)
                SetBool("temporaryAutoLogin", true);

            // Define the URL to request the list feed of the worksheet.
            var listFeedLink = in_entry.Links.FindService(Google.GData.Spreadsheets.GDataSpreadsheetsNameTable.ListRel, null);

            // Fetch the list feed of the worksheet.
            var listQuery = new Google.GData.Spreadsheets.ListQuery(listFeedLink.HRef.ToString());
            var listFeed = _Service.Query(listQuery);

            if (listFeed.Entries.Count > 0)
            {
                var rowCt = 0;
                // Iterate through each row, printing its cell values.
                foreach (var row in listFeed.Entries.Cast<Google.GData.Spreadsheets.ListEntry>())
                {
                    // skip the first row. This is the title row, and we can get the values later
                    if (rowCt == 0)
                    {
                        int colCt = 0;
                        // Iterate over the remaining columns, and print each cell value
                        foreach (Google.GData.Spreadsheets.ListEntry.Custom element in row.Elements)
                        {
                            if (colCt > 0)
                            {
                                var vartype = element.Value;
                                var fixedColName = element.LocalName;
                                if (GfuStrCmp(fixedColName, "VOID"))
                                {
                                    // at this point, we know that Google has mangled our void column into a void_2 or something, fix it.
                                    fixedColName = "void";
                                }
                                if (GfuStrCmp(fixedColName, "VOID"))
                                    vartype = "Ignore";
                                else
                                {
                                    if (!typesInFirstRow)
                                    {
                                        vartype = "string";
                                    }
                                    else FixVarType(ref vartype);
                                        
                                }
                                types.Add(vartype);

                                colNames.Add(fixedColName);
                                varNames.Add(MakeValidVariableName(element.LocalName));
                            }
                            colCt++;
                        }

                        if (typesInFirstRow == false)
                        {
                            rowNames.Add(row.Elements[0].Value);
                        }
                    }
                    else
                    {
                        // store the row names to write out into the enum
                        rowNames.Add(row.Elements[0].Value);

                    }
                    rowCt++;
                }
            }

            if (typesInFirstRow)
            {
                if (!IsDataValid(types, colNames, rowNames))
                {
                    Debug.LogError("Cannot output data for " + in_fileName + " until all errors with the data are fixed");

                    // dont nuke their data if the new data is bad
                    return false;
                }
            }
            else
            {
                if (!IsDataValid(colNames, rowNames))
                {
                    Debug.LogError("Cannot output data for " + in_fileName + " until all errors with the data are fixed");
                    // dont nuke their data if the new data is bad
                    return false;
                }
            }

            ///////////////////////////////////////////////
            // open the file 

            var fs = System.IO.File.Open(in_path + "/" + in_fileName + ".cs", System.IO.File.Exists(in_path + "/" + in_fileName + ".cs") ?
                System.IO.FileMode.Truncate :
                System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);

            if (fs == null)
            {
                Debug.LogError("Cannot open " + in_fileName + ".cs for writing");
                return false;
            }

            var sw = new System.IO.StreamWriter(fs);
            if (sw == null)
            {
                Debug.LogError("Cannot make a stream writer.. dude are you out of memory?");
                return false;
            }

            ////////////////////////////////////////
            // writing out the class
            var fileString = System.String.Empty;

            fileString += FormatLine("//----------------------------------------------");
            fileString += FormatLine("//    GoogleFu: Google Doc Unity integration");
            fileString += FormatLine("//         Copyright © 2013 Litteratus");
            fileString += FormatLine("//");
            fileString += FormatLine("//        This file has been auto-generated");
            fileString += FormatLine("//              Do not manually edit");
            fileString += FormatLine("//----------------------------------------------");
            fileString += FormatLine(System.String.Empty);
            fileString += FormatLine("using UnityEngine;");
            fileString += FormatLine(System.String.Empty);
            fileString += FormatLine("namespace GoogleFu");
            fileString += FormatLine("{");
            fileString += FormatLine("	[System.Serializable]");
            fileString += FormatLine("	public class " + in_fileName + "Row : IGoogleFuRow");
            fileString += FormatLine("	{");

            // variable declarations
            for (int i = 0; i < types.Count; i++)
            {
                if (IsSupportedArrayType(types[i]))
                {
                    fileString += FormatLine("		public System.Collections.Generic.List<" + StripArray(types[i]) + "> " + varNames[i] + " = new System.Collections.Generic.List<" + StripArray(types[i]) + ">();");
                }
                else if (GfuStrCmp(types[i], "Ignore")) { }
                else
                    fileString += FormatLine("		public " + types[i] + " " + varNames[i] + ";");
            }
            // constructor parameter list
            fileString += ("		public " + in_fileName + "Row(");
            {
                var firstItem = true;
                for (var i = 0; i < types.Count; i++)
                {
                    if (GfuStrCmp(types[i], "Ignore"))
                        continue;

                    if (!firstItem)
                        fileString += (", ");
                    firstItem = false;
                    fileString += ("string _" + varNames[i]);
                }
            }
            fileString += FormatLine(") " + System.Environment.NewLine + "		{");

            string customArrayDelimiters = _ArrayDelimiters;
            if (customArrayDelimiters.Length == 0)
            {
                Debug.LogWarning("Array Delimiters not found. Using \", \" as default delimiters");
                customArrayDelimiters = ", ";
            }

            string customStringArrayDelimiters = _StringArrayDelimiters;
            if (customStringArrayDelimiters.Length == 0)
            {
                Debug.LogWarning("String Array Delimiters not found. Using \"|\" as default delimiter");
                customStringArrayDelimiters = "|";
            }

            string customComplexTypeDelimiters = _ComplexTypeDelimiters;
            if (customComplexTypeDelimiters.Length == 0)
            {
                customComplexTypeDelimiters = ", ";
                Debug.LogWarning("Complex Type Delimiters not found. Using \", \" as default delimiter");
            }


            string customComplexTypeArrayDelimiters = _ComplexTypeArrayDelimiters;
            if (customComplexTypeArrayDelimiters.Length == 0)
            {
                customComplexTypeArrayDelimiters = "|";
                Debug.LogWarning("Complex Type Array Delimiters not found. Using \"|\" as default delimiter");
            }
            else
            {
                var bContainsInvalid = customComplexTypeArrayDelimiters.ToCharArray().Any(in_c => customComplexTypeDelimiters.Contains(System.Convert.ToString(in_c)));
                if (bContainsInvalid)
                {
                    customComplexTypeDelimiters = ",";
                    Debug.LogWarning("Complex Type Delimiters uses the same Delimiter as Complex Type Array. Using \",\" as default Complex Type delimiter");

                    customComplexTypeArrayDelimiters = "|";
                    Debug.LogWarning("Complex Type Array Delimiters uses the same Delimiter as Complex Type. Using \"|\" as default Complex Type Array delimiter");
                }
            }
            // processing each of the input parameters and copying it into the members
            for (var i = 0; i < types.Count; i++)
            {
                //nightmare time
                if (GfuStrCmp(types[i], "IGNORE"))
                {
                }
                else if (GfuStrCmp(types[i], "GAMEOBJECT"))
                {
                    fileString += FormatLine("			" + varNames[i] + " = GameObject.Find(\"" + colNames[i] + "\");");
                }
                else if (GfuStrCmp(types[i], "BOOL"))
                {
                    fileString += FormatLine("			{");
                    fileString += FormatLine("			" + types[i] + " res;");
                    fileString += FormatLine("				if(" + types[i] + ".TryParse(_" + varNames[i] + ", out res))");
                    fileString += FormatLine("					" + varNames[i] + " = res;");
                    fileString += FormatLine("				else");
                    fileString +=
                        FormatLine("					Debug.LogError(\"Failed To Convert " + colNames[i] + " string: \"+ _" +
                                   varNames[i] + " +\" to bool\");");
                    fileString += FormatLine("			}");
                }
                else if (GfuStrCmp(types[i], "BYTE"))
                {
                    fileString += FormatLine("			{");
                    fileString += FormatLine("			" + types[i] + " res;");
                    fileString += FormatLine("				if(" + types[i] + ".TryParse(_" + varNames[i] + ", out res))");
                    fileString += FormatLine("					" + varNames[i] + " = res;");
                    fileString += FormatLine("				else");
                    fileString +=
                        FormatLine("					Debug.LogError(\"Failed To Convert " + colNames[i] + " string: \"+ _" +
                                   varNames[i] + " +\" to byte\");");
                    fileString += FormatLine("			}");
                }
                else if (GfuStrCmp(types[i], "CHAR"))
                {
                    fileString += FormatLine("			{");
                    fileString += FormatLine("			" + types[i] + " res;");
                    fileString += FormatLine("				if(" + types[i] + ".TryParse(_" + varNames[i] + ", out res))");
                    fileString += FormatLine("					" + varNames[i] + " = res;");
                    fileString += FormatLine("				else");
                    fileString +=
                        FormatLine("					Debug.LogError(\"Failed To Convert " + colNames[i] + " string: \"+ _" +
                                   varNames[i] + " +\" to char\");");
                    fileString += FormatLine("			}");
                }
                else if (GfuStrCmp(types[i], "FLOAT"))
                {
                    fileString += FormatLine("			{");
                    fileString += FormatLine("			" + types[i] + " res;");
                    fileString += FormatLine("				if(" + types[i] + ".TryParse(_" + varNames[i] + ", out res))");
                    fileString += FormatLine("					" + varNames[i] + " = res;");
                    fileString += FormatLine("				else");
                    fileString +=
                        FormatLine("					Debug.LogError(\"Failed To Convert " + colNames[i] + " string: \"+ _" +
                                   varNames[i] + " +\" to " + types[i] + "\");");
                    fileString += FormatLine("			}");
                }
                else if (IsSupportedArrayType(types[i]) && (GfuStrCmp(StripArray(types[i]), "BYTE") || GfuStrCmp(StripArray(types[i]), "BOOL") || GfuStrCmp(StripArray(types[i]), "FLOAT")))
                {
                    fileString += FormatLine("			{");
                    fileString += FormatLine("				" + StripArray(types[i]) + " res;");
                    fileString +=
                        FormatLine("				string []result = _" + varNames[i] + ".Split(\"" + customArrayDelimiters +
                                   "\".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);");
                    fileString += FormatLine("				for(int i = 0; i < result.Length; i++)");
                    fileString += FormatLine("				{");
                    fileString += FormatLine("					if(" + StripArray(types[i]) + ".TryParse(result[i], out res))");
                    fileString += FormatLine("						" + varNames[i] + ".Add(res);");
                    fileString += FormatLine("					else");
                    fileString += FormatLine("					{");
                    if (GfuStrCmp(StripArray(types[i]), "BYTE"))
                        fileString += FormatLine("						" + varNames[i] + ".Add( 0 );");
                    else if (GfuStrCmp(StripArray(types[i]), "BOOL"))
                        fileString += FormatLine("						" + varNames[i] + ".Add( false );");
                    else if (GfuStrCmp(StripArray(types[i]), "FLOAT"))
                        fileString += FormatLine("						" + varNames[i] + ".Add( float.NaN );");
                    fileString +=
                        FormatLine("						Debug.LogError(\"Failed To Convert " + colNames[i] +
                                   " string: \"+ result[i] +\" to " + StripArray(types[i]) + "\");");
                    fileString += FormatLine("					}");
                    fileString += FormatLine("				}");
                    fileString += FormatLine("			}");
                }
                else if (GfuStrCmp(types[i], "INT"))
                {
                    fileString += FormatLine("			{");
                    fileString += FormatLine("			" + types[i] + " res;");
                    fileString += FormatLine("				if(int.TryParse(_" + varNames[i] + ", out res))");
                    fileString += FormatLine("					" + varNames[i] + " = res;");
                    fileString += FormatLine("				else");
                    fileString +=
                        FormatLine("					Debug.LogError(\"Failed To Convert " + colNames[i] + " string: \"+ _" +
                                   varNames[i] + " +\" to int\");");
                    fileString += FormatLine("			}");
                }
                else if (IsSupportedArrayType(types[i]) && GfuStrCmp(StripArray(types[i]), "INT"))
                {
                    fileString += FormatLine("			{");
                    fileString += FormatLine("				" + StripArray(types[i]) + " res;");
                    fileString +=
                        FormatLine("				string []result = _" + varNames[i] + ".Split(\"" + customArrayDelimiters +
                                   "\".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);");
                    fileString += FormatLine("				for(int i = 0; i < result.Length; i++)");
                    fileString += FormatLine("				{");
                    fileString += FormatLine("					if(int.TryParse(result[i], out res))");
                    fileString += FormatLine("						" + varNames[i] + ".Add( res );");
                    fileString += FormatLine("					else");
                    fileString += FormatLine("					{");
                    fileString += FormatLine("						" + varNames[i] + ".Add( 0 );");
                    fileString +=
                        FormatLine("						Debug.LogError(\"Failed To Convert " + colNames[i] +
                                   " string: \"+ result[i] +\" to " + StripArray(types[i]) + "\");");
                    fileString += FormatLine("					}");
                    fileString += FormatLine("				}");
                    fileString += FormatLine("			}");
                }
                else if (GfuStrCmp(types[i], "STRING"))
                {
                    if (_TrimStrings)
                        fileString += FormatLine("			" + varNames[i] + " = _" + varNames[i] + ".Trim();");
                    else
                        fileString += FormatLine("			" + varNames[i] + " = _" + varNames[i] + ";");
                }
                else if (IsSupportedArrayType(types[i]) && GfuStrCmp(StripArray(types[i]), "STRING"))
                {
                    fileString += FormatLine("			{");
                    fileString +=
                        FormatLine("				string []result = _" + varNames[i] + ".Split(\"" + customStringArrayDelimiters +
                                   "\".ToCharArray(),System.StringSplitOptions.RemoveEmptyEntries);");
                    fileString += FormatLine("				for(int i = 0; i < result.Length; i++)");
                    fileString += FormatLine("				{");
                    if (_TrimStringArrays)
                        fileString += FormatLine("					" + varNames[i] + ".Add( result[i].Trim() );");
                    else
                        fileString += FormatLine("					" + varNames[i] + ".Add( result[i] );");
                    fileString += FormatLine("				}");
                    fileString += FormatLine("			}");
                }
                else if (GfuStrCmp(types[i], "VECTOR2"))
                {
                    fileString += FormatLine("			{");
                    fileString +=
                        FormatLine("				string [] splitpath = _" + varNames[i] + ".Split(\"" +
                                   customComplexTypeDelimiters +
                                   "\".ToCharArray(),System.StringSplitOptions.RemoveEmptyEntries);");
                    fileString += FormatLine("				if(splitpath.Length != 2)");
                    fileString +=
                        FormatLine("					Debug.LogError(\"Incorrect number of parameters for " + types[i] + " in \" + _" +
                                   varNames[i] + " );");
                    fileString += FormatLine("				float []results = new float[splitpath.Length];");
                    fileString += FormatLine("				for(int i = 0; i < 2; i++)");
                    fileString += FormatLine("				{");
                    fileString += FormatLine("					float res;");
                    fileString += FormatLine("					if(float.TryParse(splitpath[i], out res))");
                    fileString += FormatLine("					{");
                    fileString += FormatLine("						results[i] = res;");
                    fileString += FormatLine("					}");
                    fileString += FormatLine("					else ");
                    fileString += FormatLine("					{");
                    fileString += FormatLine("						Debug.LogError(\"Error parsing \" + "
                                             + "_" + varNames[i]
                                             +
                                             " + \" Component: \" + splitpath[i] + \" parameter \" + i + \" of variable "
                                             + colNames[i] + "\");");
                    fileString += FormatLine("					}");
                    fileString += FormatLine("				}");
                    fileString += FormatLine("				" + varNames[i] + ".x = results[0];");
                    fileString += FormatLine("				" + varNames[i] + ".y = results[1];");
                    fileString += FormatLine("			}");
                }
                else if (IsSupportedArrayType(types[i]) && GfuStrCmp(StripArray(types[i]), "VECTOR2"))
                {
                    fileString += FormatLine("			{");

                    fileString +=
                        FormatLine("				string []result = _" + varNames[i] + ".Split(\"" +
                                   customComplexTypeArrayDelimiters +
                                   "\".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);");
                    fileString += FormatLine("				for(int i = 0; i < result.Length; i++)");
                    fileString += FormatLine("				{");

                    fileString += FormatLine("                  {");
                    fileString +=
                        FormatLine("      				string [] splitpath = result[i].Split(\"" + customComplexTypeDelimiters +
                                   "\".ToCharArray(),System.StringSplitOptions.RemoveEmptyEntries);");
                    fileString += FormatLine("      				if(splitpath.Length != 2)");
                    fileString +=
                        FormatLine("      					Debug.LogError(\"Incorrect number of parameters for " + types[i] +
                                   " in \" + _" + varNames[i] + " );");
                    fileString += FormatLine("      				float []results = new float[splitpath.Length];");
                    fileString += FormatLine("      				for(int j = 0; j < splitpath.Length; j++)");
                    fileString += FormatLine("      				{");
                    fileString += FormatLine("				            float [] temp = new float[splitpath.Length];");
                    fileString += FormatLine("      					if(float.TryParse(splitpath[j], out temp[j]))");
                    fileString += FormatLine("      					{");
                    fileString += FormatLine("      						results[j] = temp[j];");
                    fileString += FormatLine("      					}");
                    fileString += FormatLine("      					else ");
                    fileString += FormatLine("      					{");
                    fileString += FormatLine("	        					Debug.LogError(\"Error parsing \" + "
                                             + "_" + varNames[i]
                                             +
                                             " + \" Component: \" + splitpath[i] + \" parameter \" + i + \" of variable "
                                             + colNames[i] + "\");");
                    fileString += FormatLine("		        			continue;");
                    fileString += FormatLine("		        		}");
                    fileString += FormatLine("		        	}");
                    fileString +=
                        FormatLine("		        		" + varNames[i] + ".Add( new " + StripArray(types[i]) +
                                   "(results[0], results[1] ));");
                    fileString += FormatLine("		        	}");

                    fileString += FormatLine("				}");
                    fileString += FormatLine("			}");
                }
                else if (GfuStrCmp(types[i], "VECTOR3"))
                {
                    fileString += FormatLine("			{");
                    fileString +=
                        FormatLine("				string [] splitpath = _" + varNames[i] + ".Split(\"" +
                                   customComplexTypeDelimiters +
                                   "\".ToCharArray(),System.StringSplitOptions.RemoveEmptyEntries);");
                    fileString += FormatLine("				if(splitpath.Length != 3)");
                    fileString +=
                        FormatLine("					Debug.LogError(\"Incorrect number of parameters for " + types[i] + " in \" + _" +
                                   varNames[i] + " );");
                    fileString += FormatLine("				float []results = new float[splitpath.Length];");
                    fileString += FormatLine("				for(int i = 0; i < 3; i++)");
                    fileString += FormatLine("				{");
                    fileString += FormatLine("					float res;");
                    fileString += FormatLine("					if(float.TryParse(splitpath[i], out res))");
                    fileString += FormatLine("					{");
                    fileString += FormatLine("						results[i] = res;");
                    fileString += FormatLine("					}");
                    fileString += FormatLine("					else ");
                    fileString += FormatLine("					{");
                    fileString += FormatLine("						Debug.LogError(\"Error parsing \" + "
                                             + "_" + varNames[i]
                                             +
                                             " + \" Component: \" + splitpath[i] + \" parameter \" + i + \" of variable "
                                             + colNames[i] + "\");");
                    fileString += FormatLine("					}");
                    fileString += FormatLine("				}");
                    fileString += FormatLine("				" + varNames[i] + ".x = results[0];");
                    fileString += FormatLine("				" + varNames[i] + ".y = results[1];");
                    fileString += FormatLine("				" + varNames[i] + ".z = results[2];");
                    fileString += FormatLine("			}");
                }
                else if (IsSupportedArrayType(types[i]) && GfuStrCmp(StripArray(types[i]), "VECTOR3"))
                {
                    fileString += FormatLine("			{");
                    fileString +=
                        FormatLine("				string []result = _" + varNames[i] + ".Split(\"" +
                                   customComplexTypeArrayDelimiters +
                                   "\".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);");
                    fileString += FormatLine("				for(int i = 0; i < result.Length; i++)");
                    fileString += FormatLine("				{");

                    fileString += FormatLine("      			{");
                    fileString +=
                        FormatLine("      				string [] splitpath = result[i].Split(\"" + customComplexTypeDelimiters +
                                   "\".ToCharArray(),System.StringSplitOptions.RemoveEmptyEntries);");
                    fileString += FormatLine("      				if(splitpath.Length != 3)");
                    fileString +=
                        FormatLine("      					Debug.LogError(\"Incorrect number of parameters for " + types[i] +
                                   " in \" + _" + varNames[i] + " );");
                    fileString += FormatLine("      				float []results = new float[splitpath.Length];");
                    fileString += FormatLine("      				for(int j = 0; j < splitpath.Length; j++)");
                    fileString += FormatLine("      				{");
                    fileString += FormatLine("				            float [] temp = new float[splitpath.Length];");
                    fileString += FormatLine("      					if(float.TryParse(splitpath[j], out temp[j]))");
                    fileString += FormatLine("      					{");
                    fileString += FormatLine("      						results[j] = temp[j];");
                    fileString += FormatLine("      					}");
                    fileString += FormatLine("      					else ");
                    fileString += FormatLine("      					{");
                    fileString += FormatLine("	        					Debug.LogError(\"Error parsing \" + "
                                             + "_" + varNames[i]
                                             +
                                             " + \" Component: \" + splitpath[i] + \" parameter \" + i + \" of variable "
                                             + colNames[i] + "\");");
                    fileString += FormatLine("		        			continue;");
                    fileString += FormatLine("		        		}");
                    fileString += FormatLine("		        	}");
                    fileString +=
                        FormatLine("		        	" + varNames[i] + ".Add( new " + StripArray(types[i]) +
                                   "(results[0], results[1], results[2] ));");

                    fileString += FormatLine("		        	}");

                    fileString += FormatLine("				}");
                    fileString += FormatLine("			}");
                }
                else if (GfuStrCmp(types[i], "COLOR"))
                {
                    fileString += FormatLine("			{");
                    fileString +=
                        FormatLine("				string [] splitpath = _" + varNames[i] + ".Split(\"" +
                                   customComplexTypeDelimiters +
                                   "\".ToCharArray(),System.StringSplitOptions.RemoveEmptyEntries);");
                    fileString += FormatLine("				if(splitpath.Length != 3 && splitpath.Length != 4)");
                    fileString +=
                        FormatLine("					Debug.LogError(\"Incorrect number of parameters for " + types[i] + " in \" + _" +
                                   varNames[i] + " );");
                    fileString += FormatLine("				float []results = new float[splitpath.Length];");
                    fileString += FormatLine("				for(int i = 0; i < splitpath.Length; i++)");
                    fileString += FormatLine("				{");
                    fileString += FormatLine("					float res;");
                    fileString += FormatLine("					if(float.TryParse(splitpath[i], out res))");
                    fileString += FormatLine("					{");
                    fileString += FormatLine("						results[i] = res;");
                    fileString += FormatLine("					}");
                    fileString += FormatLine("					else ");
                    fileString += FormatLine("					{");
                    fileString += FormatLine("						Debug.LogError(\"Error parsing \" + "
                                             + "_" + varNames[i]
                                             +
                                             " + \" Component: \" + splitpath[i] + \" parameter \" + i + \" of variable "
                                             + colNames[i] + "\");");
                    fileString += FormatLine("					}");
                    fileString += FormatLine("				}");
                    fileString += FormatLine("				" + varNames[i] + ".r = results[0];");
                    fileString += FormatLine("				" + varNames[i] + ".g = results[1];");
                    fileString += FormatLine("				" + varNames[i] + ".b = results[2];");
                    fileString += FormatLine("				if(splitpath.Length == 4)");
                    fileString += FormatLine("					" + varNames[i] + ".a = results[3];");
                    fileString += FormatLine("			}");
                }
                else if (IsSupportedArrayType(types[i]) && GfuStrCmp(StripArray(types[i]), "COLOR"))
                {
                    fileString += FormatLine("			{");
                    fileString +=
                        FormatLine("				string []result = _" + varNames[i] + ".Split(\"" +
                                   customComplexTypeArrayDelimiters +
                                   "\".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);");
                    fileString += FormatLine("				for(int i = 0; i < result.Length; i++)");
                    fileString += FormatLine("				{");

                    fileString += FormatLine("      			{");
                    fileString +=
                        FormatLine("      				string [] splitpath = result[i].Split(\"" + customComplexTypeDelimiters +
                                   "\".ToCharArray(),System.StringSplitOptions.RemoveEmptyEntries);");
                    fileString += FormatLine("      				if(splitpath.Length != 3 && splitpath.Length != 4)");
                    fileString +=
                        FormatLine("      					Debug.LogError(\"Incorrect number of parameters for " + types[i] +
                                   " in \" + _" + varNames[i] + " );");
                    fileString += FormatLine("      				float []results = new float[splitpath.Length];");
                    fileString += FormatLine("      				for(int j = 0; j < splitpath.Length; j++)");
                    fileString += FormatLine("      				{");
                    fileString += FormatLine("				            float [] temp = new float[splitpath.Length];");
                    fileString += FormatLine("      					if(float.TryParse(splitpath[j], out temp[j]))");
                    fileString += FormatLine("      					{");
                    fileString += FormatLine("      						results[j] = temp[j];");
                    fileString += FormatLine("      					}");
                    fileString += FormatLine("      					else ");
                    fileString += FormatLine("      					{");
                    fileString += FormatLine("	        					Debug.LogError(\"Error parsing \" + "
                                             + "_" + varNames[i]
                                             +
                                             " + \" Component: \" + splitpath[i] + \" parameter \" + i + \" of variable "
                                             + colNames[i] + "\");");
                    fileString += FormatLine("		        			continue;");
                    fileString += FormatLine("		        		}");
                    fileString += FormatLine("		        	}");
                    fileString += FormatLine("		        		if(splitpath.Length == 3)");
                    fileString +=
                        FormatLine("		        		" + varNames[i] + ".Add( new " + StripArray(types[i]) +
                                   "(results[0], results[1], results[2] ));");
                    fileString += FormatLine("		        		else");
                    fileString +=
                        FormatLine("		        		" + varNames[i] + ".Add( new " + StripArray(types[i]) +
                                   "(results[0], results[1], results[2], results[3] ));");

                    fileString += FormatLine("		        	}");

                    fileString += FormatLine("				}");
                    fileString += FormatLine("			}");
                }
                else if (GfuStrCmp(types[i], "COLOR32"))
                {
                    fileString += FormatLine("			{");
                    fileString +=
                        FormatLine("				string [] splitpath = _" + varNames[i] + ".Split(\"" +
                                   customComplexTypeDelimiters +
                                   "\".ToCharArray(),System.StringSplitOptions.RemoveEmptyEntries);");
                    fileString += FormatLine("				if(splitpath.Length != 3 && splitpath.Length != 4)");
                    fileString +=
                        FormatLine("					Debug.LogError(\"Incorrect number of parameters for " + types[i] + " in \" + _" +
                                   varNames[i] + " );");
                    fileString += FormatLine("				byte []results = new byte[splitpath.Length];");
                    fileString += FormatLine("				for(int i = 0; i < splitpath.Length; i++)");
                    fileString += FormatLine("				{");
                    fileString += FormatLine("					byte res;");
                    fileString += FormatLine("					if(byte.TryParse(splitpath[i], out res))");
                    fileString += FormatLine("					{");
                    fileString += FormatLine("						results[i] = res;");
                    fileString += FormatLine("					}");
                    fileString += FormatLine("					else ");
                    fileString += FormatLine("					{");
                    fileString += FormatLine("						Debug.LogError(\"Error parsing \" + "
                                             + "_" + varNames[i]
                                             +
                                             " + \" Component: \" + splitpath[i] + \" parameter \" + i + \" of variable "
                                             + colNames[i] + "\");");
                    fileString += FormatLine("					}");
                    fileString += FormatLine("				}");
                    fileString += FormatLine("				" + varNames[i] + ".r = results[0];");
                    fileString += FormatLine("				" + varNames[i] + ".g = results[1];");
                    fileString += FormatLine("				" + varNames[i] + ".b = results[2];");
                    fileString += FormatLine("				if(splitpath.Length == 4)");
                    fileString += FormatLine("					" + varNames[i] + ".a = results[3];");
                    fileString += FormatLine("			}");
                }
                else if (IsSupportedArrayType(types[i]) && GfuStrCmp(StripArray(types[i]), "COLOR32"))
                {
                    fileString += FormatLine("			{");
                    fileString +=
                        FormatLine("				string []result = _" + varNames[i] + ".Split(\"" +
                                   customComplexTypeArrayDelimiters +
                                   "\".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);");
                    fileString += FormatLine("				for(int i = 0; i < result.Length; i++)");
                    fileString += FormatLine("				{");

                    fileString += FormatLine("      			{");
                    fileString +=
                        FormatLine("      				string [] splitpath = result[i].Split(\"" + customComplexTypeDelimiters +
                                   "\".ToCharArray(),System.StringSplitOptions.RemoveEmptyEntries);");
                    fileString += FormatLine("      				if(splitpath.Length != 3 && splitpath.Length != 4)");
                    fileString +=
                        FormatLine("      					Debug.LogError(\"Incorrect number of parameters for " + types[i] +
                                   " in \" + _" + varNames[i] + " );");
                    fileString += FormatLine("      				byte []results = new byte[splitpath.Length];");
                    fileString += FormatLine("      				for(int j = 0; j < splitpath.Length; j++)");
                    fileString += FormatLine("      				{");
                    fileString += FormatLine("				            byte [] temp = new byte[splitpath.Length];");
                    fileString += FormatLine("      					if(byte.TryParse(splitpath[j], out temp[j]))");
                    fileString += FormatLine("      					{");
                    fileString += FormatLine("      						results[j] = temp[j];");
                    fileString += FormatLine("      					}");
                    fileString += FormatLine("      					else ");
                    fileString += FormatLine("      					{");
                    fileString += FormatLine("	        					Debug.LogError(\"Error parsing \" + "
                                             + "_" + varNames[i]
                                             +
                                             " + \" Component: \" + splitpath[i] + \" parameter \" + i + \" of variable "
                                             + colNames[i] + "\");");
                    fileString += FormatLine("		        			continue;");
                    fileString += FormatLine("		        		}");
                    fileString += FormatLine("		        	}");
                    fileString += FormatLine("		        		if(splitpath.Length == 3)");
                    fileString +=
                        FormatLine("		        		    " + varNames[i] + ".Add( new " + StripArray(types[i]) +
                                   "(results[0], results[1], results[2], System.Convert.ToByte(0) ));");
                    fileString += FormatLine("		        		else");
                    fileString +=
                        FormatLine("		        		    " + varNames[i] + ".Add( new " + StripArray(types[i]) +
                                   "(results[0], results[1], results[2], results[3] ));");

                    fileString += FormatLine("		        	}");

                    fileString += FormatLine("				}");
                    fileString += FormatLine("			}");
                }
                else if (GfuStrCmp(types[i], "QUATERNION"))
                {
                    fileString += FormatLine("			{");
                    fileString +=
                        FormatLine("				string [] splitpath = _" + varNames[i] + ".Split(\"" +
                                   customComplexTypeDelimiters +
                                   "\".ToCharArray(),System.StringSplitOptions.RemoveEmptyEntries);");
                    fileString += FormatLine("				if(splitpath.Length != 4)");
                    fileString +=
                        FormatLine("					Debug.LogError(\"Incorrect number of parameters for " + types[i] + " in \" + _" +
                                   varNames[i] + " );");
                    fileString += FormatLine("				float []results = new float[splitpath.Length];");
                    fileString += FormatLine("				for(int i = 0; i < 4; i++)");
                    fileString += FormatLine("				{");
                    fileString += FormatLine("					float res;");
                    fileString += FormatLine("					if(float.TryParse(splitpath[i], out res))");
                    fileString += FormatLine("					{");
                    fileString += FormatLine("						results[i] = res;");
                    fileString += FormatLine("					}");
                    fileString += FormatLine("					else ");
                    fileString += FormatLine("					{");
                    fileString += FormatLine("						Debug.LogError(\"Error parsing \" + "
                                             + "_" + varNames[i]
                                             +
                                             " + \" Component: \" + splitpath[i] + \" parameter \" + i + \" of variable "
                                             + colNames[i] + "\");");
                    fileString += FormatLine("					}");
                    fileString += FormatLine("				}");
                    fileString += FormatLine("				" + varNames[i] + ".x = results[0];");
                    fileString += FormatLine("				" + varNames[i] + ".y = results[1];");
                    fileString += FormatLine("				" + varNames[i] + ".z = results[2];");
                    fileString += FormatLine("				" + varNames[i] + ".w = results[3];");
                    fileString += FormatLine("			}");
                }
                else if (IsSupportedArrayType(types[i]) && GfuStrCmp(StripArray(types[i]), "QUATERNION"))
                {
                    fileString += FormatLine("			{");
                    fileString +=
                        FormatLine("				string []result = _" + varNames[i] + ".Split(\"" +
                                   customComplexTypeArrayDelimiters +
                                   "\".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);");
                    fileString += FormatLine("				for(int i = 0; i < result.Length; i++)");
                    fileString += FormatLine("				{");

                    fileString += FormatLine("      			{");
                    fileString +=
                        FormatLine("      				string [] splitpath = result[i].Split(\"" + customComplexTypeDelimiters +
                                   "\".ToCharArray(),System.StringSplitOptions.RemoveEmptyEntries);");
                    fileString += FormatLine("      				if(splitpath.Length != 3 && splitpath.Length != 4)");
                    fileString +=
                        FormatLine("      					Debug.LogError(\"Incorrect number of parameters for " + types[i] +
                                   " in \" + _" + varNames[i] + " );");
                    fileString += FormatLine("      				float []results = new float[splitpath.Length];");
                    fileString += FormatLine("      				for(int j = 0; j < splitpath.Length; j++)");
                    fileString += FormatLine("      				{");
                    fileString += FormatLine("				            float [] temp = new float[splitpath.Length];");
                    fileString += FormatLine("      					if(float.TryParse(splitpath[j], out temp[j]))");
                    fileString += FormatLine("      					{");
                    fileString += FormatLine("      						results[j] = temp[j];");
                    fileString += FormatLine("      					}");
                    fileString += FormatLine("      					else ");
                    fileString += FormatLine("      					{");
                    fileString += FormatLine("	        					Debug.LogError(\"Error parsing \" + "
                                             + "_" + varNames[i]
                                             +
                                             " + \" Component: \" + splitpath[i] + \" parameter \" + i + \" of variable "
                                             + colNames[i] + "\");");
                    fileString += FormatLine("		        			continue;");
                    fileString += FormatLine("		        		}");
                    fileString += FormatLine("		        	}");
                    fileString +=
                        FormatLine("		        		" + varNames[i] + ".Add( new " + StripArray(types[i]) +
                                   "(results[0], results[1], results[2], results[3] ));");
                    fileString += FormatLine("		        	}");

                    fileString += FormatLine("				}");
                    fileString += FormatLine("			}");
                }
                else
                {
                    fileString += FormatLine("			" + varNames[i] + " = _" + varNames[i] + ";");
                }
            }
            fileString += FormatLine("		}");

            fileString += FormatLine(System.String.Empty);
            {
                var colCount = colNames.Where((in_t, in_i) => !GfuStrCmp(in_t, "VOID") && !GfuStrCmp(types[in_i], "Ignore")).Count();
                fileString += FormatLine("		public int Length { get { return " + colCount + "; } }");
            }
            fileString += FormatLine(System.String.Empty);

            // allow indexing by []
            fileString += FormatLine("		public string this[int i]");
            fileString += FormatLine("		{");
            fileString += FormatLine("		    get");
            fileString += FormatLine("		    {");
            fileString += FormatLine("		        return GetStringDataByIndex(i);");
            fileString += FormatLine("		    }");
            fileString += FormatLine("		}");
            fileString += FormatLine(System.String.Empty);
            // get string data by index lets the user use an int field rather than the name to retrieve the data
            fileString += FormatLine("		public string GetStringDataByIndex( int index )");
            fileString += FormatLine("		{");
            fileString += FormatLine("			string ret = System.String.Empty;");
            fileString += FormatLine("			switch( index )");
            fileString += FormatLine("			{");

            {
                var colNum = 0;
                for (var i = 0; i < colNames.Count; i++)
                {
                    if (GfuStrCmp(types[i], "Ignore"))
                        continue;
                    if (GfuStrCmp(colNames[i], "VOID"))
                        continue;
                    fileString += FormatLine("				case " + colNum++ + ":");
                    fileString += FormatLine("					ret = " + varNames[i] + ".ToString();");
                    fileString += FormatLine("					break;");
                }
            }
            fileString += FormatLine("			}");
            fileString += FormatLine(System.String.Empty);
            fileString += FormatLine("			return ret;");
            fileString += FormatLine("		}");
            fileString += FormatLine(System.String.Empty);

            // get the data by column name rather than index
            fileString += FormatLine("		public string GetStringData( string colID )");
            fileString += FormatLine("		{");
            fileString += FormatLine("			var ret = System.String.Empty;");
            fileString += FormatLine("			switch( colID.ToLower() )");
            fileString += FormatLine("			{");

            for (var i = 0; i < colNames.Count; i++)
            {
                if (GfuStrCmp(types[i], "Ignore"))
                    continue;
                if (GfuStrCmp(colNames[i], "VOID"))
                    continue;
                fileString += FormatLine("				case \"" + colNames[i] + "\":");
                fileString += FormatLine("					ret = " + varNames[i] + ".ToString();");
                fileString += FormatLine("					break;");
            }

            fileString += FormatLine("			}");
            fileString += FormatLine(System.String.Empty);
            fileString += FormatLine("			return ret;");
            fileString += FormatLine("		}");

            fileString += FormatLine("		public override string ToString()");
            fileString += FormatLine("		{");
            fileString += FormatLine("			string ret = System.String.Empty;");
            for (int i = 0; i < colNames.Count; i++)
            {
                if (GfuStrCmp(types[i], "Ignore"))
                    continue;
                if (GfuStrCmp(colNames[i], "VOID"))
                    continue;
                fileString += FormatLine("			ret += \"{\" + \"" + colNames[i] + "\" + \" : \" + " + varNames[i] + ".ToString() + \"} \";");
            }
            fileString += FormatLine("			return ret;");
            fileString += FormatLine("		}");

            fileString += FormatLine("	}");



            ///////////////////////////////////////////////////////////////////////////////
            // the database class itself, this contains the rows defined above
            if (in_staticClass)
                fileString += FormatLine("	public sealed class " + in_fileName + " : IGoogleFuDB");
            else
                fileString += FormatLine("	public class " + in_fileName + " :  GoogleFuComponentBase, IGoogleFuDB");
            fileString += FormatLine("	{");


            // this is the enums, the enum matches the name of the row
            fileString += FormatLine("		public enum rowIds {");
            fileString += ("			");
            for (var i = 0; i < rowNames.Count; i++)
            {
                if (GfuStrCmp(rowNames[i], "VOID"))
                    continue;

                fileString += (rowNames[i]);
                if (i != rowNames.Count - 1)
                    fileString += (", ");
                if ((i + 1) % 20 == 0)
                    fileString += System.Environment.NewLine + "			";
            }
            fileString += FormatLine(System.String.Empty);
            fileString += FormatLine("		};");



            fileString += FormatLine("		public string [] rowNames = {");
            fileString += "			";
            for (int i = 0; i < rowNames.Count; i++)
            {
                if (GfuStrCmp(rowNames[i], "VOID"))
                    continue;

                fileString += "\"" + rowNames[i] + "\"";
                if (i != rowNames.Count - 1)
                    fileString += ", ";
                if ((i + 1) % 20 == 0)
                    fileString += System.Environment.NewLine + "			";
            }
            fileString += FormatLine(System.Environment.NewLine + "		};");
            // the declaration of the storage for the row data
            fileString += FormatLine("		public System.Collections.Generic.List<" + in_fileName + "Row> Rows = new System.Collections.Generic.List<" + in_fileName + "Row>();");

            // declare the instance as well as the get functionality, if this is going to be a static class
            if (in_staticClass)
            {
                fileString += FormatLine(System.String.Empty);
                fileString += FormatLine("		public static " + in_fileName + " Instance");
                fileString += FormatLine("		{");
                fileString += FormatLine("			get { return Nested" + in_fileName + ".instance; }");
                fileString += FormatLine("		}");
                fileString += FormatLine(System.String.Empty);
                fileString += FormatLine("		private class Nested" + in_fileName + "");
                fileString += FormatLine("		{");
                fileString += FormatLine("			static Nested" + in_fileName + "() { }");
                fileString += FormatLine("			internal static readonly " + in_fileName + " instance = new " + in_fileName + "();");
                fileString += FormatLine("		}");
                fileString += FormatLine(System.String.Empty);
                fileString += FormatLine("		private " + in_fileName + "()");
                fileString += FormatLine("		{");

                var rowCt = 0;
                // Iterate through each row, printing its cell values.
                foreach (var row in listFeed.Entries.Cast<Google.GData.Spreadsheets.ListEntry>())
                {
                    if (typesInFirstRow)
                    {
                        // skip the first row. This is the title row, and we can get the values later
                        if (rowCt == 0)
                        {
                            rowCt++;
                            continue;
                        }
                    }

                    if (GfuStrCmp(row.Title.Text, "VOID"))
                    {
                        rowCt++;
                        continue;
                    }

                    var thisRow = (from Google.GData.Spreadsheets.ListEntry.Custom element in row.Elements select SanitizeJson(element.Value, true)).ToList();
                    // Iterate over the remaining columns, and print each cell value

                    // Prevent empty / void row entries
                    if (string.IsNullOrEmpty(thisRow[0]))
                    {
                        rowCt++;
                        continue;
                    }

                    fileString += "			Rows.Add( new " + in_fileName + "Row(";
                    {
                        bool firstItem = true;
                        for (var i = 1; i < thisRow.Count; i++)
                        {
                            if (GfuStrCmp(types[i - 1], "Ignore"))
                                continue;
                            if (!firstItem)
                                fileString += "," + System.Environment.NewLine + "														";
                            firstItem = false;
                            fileString += "\"" + thisRow[i] + "\"";
                        }
                    }
                    fileString += FormatLine("));");

                    rowCt++;
                }
                fileString += FormatLine("		}");
            }
            else
            {
                // the dont destroy on awake flag
                if (GetBool(_ActiveWorkbook.Title + "." + in_entry.Title.Text + ".OBJDB" + ".DND", false))
                {
                    fileString += FormatLine(System.String.Empty);
                    fileString += FormatLine("		void Awake()");
                    fileString += FormatLine("		{");
                    fileString += FormatLine("			DontDestroyOnLoad(this);");
                    fileString += FormatLine("		}");
                }

                // this is the processing that actually gets the data into the object itself later on, 
                // this loops through the generic input and seperates it into strings for the above
                // row class to handle and parse into its members
                fileString += FormatLine("		public override void AddRowGeneric (System.Collections.Generic.List<string> input)");
                fileString += FormatLine("		{");
                fileString += ("			Rows.Add(new " + in_fileName + "Row(");
                {
                    bool firstItem = true;
                    for (int i = 0; i < types.Count; i++)
                    {
                        if (GfuStrCmp(types[i], "Ignore"))
                            continue;

                        if (!firstItem)
                            fileString += (",");
                        firstItem = false;
                        fileString += ("input[" + i + "]");
                    }
                }
                fileString += FormatLine("));");
                fileString += FormatLine("		}");

                fileString += FormatLine("		public override void Clear ()");
                fileString += FormatLine("		{");
                fileString += FormatLine("			Rows.Clear();");
                fileString += FormatLine("		}");
            }

            fileString += FormatLine("		public IGoogleFuRow GetGenRow(string in_RowString)");
            fileString += FormatLine("		{");
            fileString += FormatLine("			IGoogleFuRow ret = null;");
            fileString += FormatLine("			try");
            fileString += FormatLine("			{");
            fileString += FormatLine("				ret = Rows[(int)System.Enum.Parse(typeof(rowIds), in_RowString)];");
            fileString += FormatLine("			}");
            fileString += FormatLine("			catch(System.ArgumentException) {");
            fileString += FormatLine("				Debug.LogError( in_RowString + \" is not a member of the rowIds enumeration.\");");
            fileString += FormatLine("			}");
            fileString += FormatLine("			return ret;");
            fileString += FormatLine("		}");

            fileString += FormatLine("		public IGoogleFuRow GetGenRow(rowIds in_RowID)");
            fileString += FormatLine("		{");
            fileString += FormatLine("			IGoogleFuRow ret = null;");
            fileString += FormatLine("			try");
            fileString += FormatLine("			{");
            fileString += FormatLine("				ret = Rows[(int)in_RowID];");
            fileString += FormatLine("			}");
            fileString += FormatLine("			catch( System.Collections.Generic.KeyNotFoundException ex )");
            fileString += FormatLine("			{");
            fileString += FormatLine("				Debug.LogError( in_RowID + \" not found: \" + ex.Message );");
            fileString += FormatLine("			}");
            fileString += FormatLine("			return ret;");
            fileString += FormatLine("		}");

            fileString += FormatLine("		public " + in_fileName + "Row GetRow(rowIds in_RowID)");
            fileString += FormatLine("		{");
            fileString += FormatLine("			" + in_fileName + "Row ret = null;");
            fileString += FormatLine("			try");
            fileString += FormatLine("			{");
            fileString += FormatLine("				ret = Rows[(int)in_RowID];");
            fileString += FormatLine("			}");
            fileString += FormatLine("			catch( System.Collections.Generic.KeyNotFoundException ex )");
            fileString += FormatLine("			{");
            fileString += FormatLine("				Debug.LogError( in_RowID + \" not found: \" + ex.Message );");
            fileString += FormatLine("			}");
            fileString += FormatLine("			return ret;");
            fileString += FormatLine("		}");


            fileString += FormatLine("		public " + in_fileName + "Row GetRow(string in_RowString)");
            fileString += FormatLine("		{");
            fileString += FormatLine("			" + in_fileName + "Row ret = null;");
            fileString += FormatLine("			try");
            fileString += FormatLine("			{");
            fileString += FormatLine("				ret = Rows[(int)System.Enum.Parse(typeof(rowIds), in_RowString)];");
            fileString += FormatLine("			}");
            fileString += FormatLine("			catch(System.ArgumentException) {");
            fileString += FormatLine("				Debug.LogError( in_RowString + \" is not a member of the rowIds enumeration.\");");
            fileString += FormatLine("			}");
            fileString += FormatLine("			return ret;");
            fileString += FormatLine("		}");
            fileString += FormatLine(System.String.Empty);
            fileString += FormatLine("	}");
            fileString += FormatLine(System.String.Empty);
            fileString += FormatLine("}");

            sw.Write(fileString);

            ///////////////////////////////////
            // done writing, clean up
            sw.Flush();
            sw.Close();
            fs.Close();

            if (in_staticClass == false)
            {
                // Writing out the custom inspector
                ///////////////////////////////////////////////
                // open the file 

                Debug.Log("Saving to: " + GoogleFuGenPath("ObjDBEditor") + "/" + in_fileName + "Editor.cs");
                fs = System.IO.File.Open(GoogleFuGenPath("ObjDBEditor") + "/" + in_fileName + "Editor.cs", System.IO.File.Exists(GoogleFuGenPath("ObjDBEditor") + "/" + in_fileName + "Editor.cs") ?
                    System.IO.FileMode.Truncate :
                    System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);

                if (fs == null)
                {
                    Debug.LogError("Cannot open " + in_fileName + "Editor.cs for writing");
                    return false;
                }

                sw = new System.IO.StreamWriter(fs);
                if (sw == null)
                {
                    Debug.LogError("Cannot create a streamwriter, dude are you out of memory?");
                    return false;
                }

                fileString = System.String.Empty;
                fileString += FormatLine("using UnityEngine;");
                fileString += FormatLine("using UnityEditor;");
                fileString += FormatLine(System.String.Empty);
                fileString += FormatLine("namespace GoogleFu");
                fileString += FormatLine("{");
                fileString += FormatLine("	[CustomEditor(typeof(" + in_fileName + "))]");
                fileString += FormatLine("	public class " + in_fileName + "Editor : Editor");
                fileString += FormatLine("	{");
                fileString += FormatLine("		public int Index = 0;");
                // sneaky time, count all the arrays and make an index for each of them within the inspector
                for (int i = 0; i < types.Count; i++)
                {
                    if (types[i].Contains("array"))
                    {
                        fileString += FormatLine("		public int " + varNames[i] + "_Index = 0;");
                    }
                }
                fileString += FormatLine("		public override void OnInspectorGUI ()");
                fileString += FormatLine("		{");
                fileString += FormatLine("			" + in_fileName + " s = target as " + in_fileName + ";");
                fileString += FormatLine("			" + in_fileName + "Row r = s.Rows[ Index ];");
                fileString += FormatLine(System.String.Empty);
                fileString += FormatLine("			EditorGUILayout.BeginHorizontal();");
                fileString += FormatLine("			if ( GUILayout.Button(\"<<\") )");
                fileString += FormatLine("			{");
                fileString += FormatLine("				Index = 0;");
                fileString += FormatLine("			}");
                fileString += FormatLine("			if ( GUILayout.Button(\"<\") )");
                fileString += FormatLine("			{");
                fileString += FormatLine("				Index -= 1;");
                fileString += FormatLine("				if ( Index < 0 )");
                fileString += FormatLine("					Index = s.Rows.Count - 1;");
                fileString += FormatLine("			}");
                fileString += FormatLine("			if ( GUILayout.Button(\">\") )");
                fileString += FormatLine("			{");
                fileString += FormatLine("				Index += 1;");
                fileString += FormatLine("				if ( Index >= s.Rows.Count )");
                fileString += FormatLine("					Index = 0;");
                fileString += FormatLine("			}");
                fileString += FormatLine("			if ( GUILayout.Button(\">>\") )");
                fileString += FormatLine("			{");
                fileString += FormatLine("				Index = s.Rows.Count - 1;");
                fileString += FormatLine("			}");
                fileString += FormatLine(System.String.Empty);
                fileString += FormatLine("			EditorGUILayout.EndHorizontal();");
                fileString += FormatLine(System.String.Empty);
                fileString += FormatLine("			EditorGUILayout.BeginHorizontal();");
                fileString += FormatLine("			GUILayout.Label( \"ID\", GUILayout.Width( 150.0f ) );");
                fileString += FormatLine("			{");
                fileString += FormatLine("				EditorGUILayout.LabelField( s.rowNames[ Index ] );");
                fileString += FormatLine("			}");
                fileString += FormatLine("			EditorGUILayout.EndHorizontal();");
                fileString += FormatLine(System.String.Empty);

                for (int i = 0; i < types.Count; i++)
                {
                    fileString += FormatLine("			EditorGUILayout.BeginHorizontal();");

                    if (types[i].ToUpper().Contains("ARRAY"))
                    {
                        fileString += FormatLine("			if ( r." + varNames[i] + ".Count == 0 )");
                        fileString += FormatLine("			{");
                        fileString += FormatLine("			    GUILayout.Label( \"" + colNames[i] + "\", GUILayout.Width( 150.0f ) );");
                        fileString += FormatLine("			    {");
                        fileString += FormatLine("			    	EditorGUILayout.LabelField( \"Empty Array\" );");
                        fileString += FormatLine("			    }");
                        fileString += FormatLine("			}");
                        fileString += FormatLine("			else");
                        fileString += FormatLine("			{");
                        fileString += FormatLine("			    GUILayout.Label( \"" + colNames[i] + "\", GUILayout.Width( 130.0f ) );");
                        // when you switch the row you are examining, they may have different array sizes... therefore, we may actually be past the end of the list
                        fileString += FormatLine("			    if ( " + varNames[i] + "_Index >= r." + varNames[i] + ".Count )");
                        fileString += FormatLine("				    " + varNames[i] + "_Index = 0;");
                        // back button
                        fileString += FormatLine("			    if ( GUILayout.Button(\"<\", GUILayout.Width( 18.0f )) )");
                        fileString += FormatLine("			    {");
                        fileString += FormatLine("			    	" + varNames[i] + "_Index -= 1;");
                        fileString += FormatLine("			    	if ( " + varNames[i] + "_Index < 0 )");
                        fileString += FormatLine("			    		" + varNames[i] + "_Index = r." + varNames[i] + ".Count - 1;");
                        fileString += FormatLine("			    }");

                        fileString += FormatLine("			    EditorGUILayout.LabelField(" + varNames[i] + "_Index.ToString(), GUILayout.Width( 15.0f ));");

                        // fwd button
                        fileString += FormatLine("			    if ( GUILayout.Button(\">\", GUILayout.Width( 18.0f )) )");
                        fileString += FormatLine("			    {");
                        fileString += FormatLine("			    	" + varNames[i] + "_Index += 1;");
                        fileString += FormatLine("			    	if ( " + varNames[i] + "_Index >= r." + varNames[i] + ".Count )");
                        fileString += FormatLine("		        		" + varNames[i] + "_Index = 0;");
                        fileString += FormatLine("				}");
                    }
                    if (!GfuStrCmp(types[i], "IGNORE"))
                    {
                        if (GfuStrCmp(types[i], "FLOAT"))
                        {
                            fileString +=
                                FormatLine("			GUILayout.Label( \"" + colNames[i] + "\", GUILayout.Width( 150.0f ) );");
                            fileString += FormatLine("			{");
                            fileString += FormatLine("				EditorGUILayout.FloatField( (float)r." + varNames[i] + " );");
                            fileString += FormatLine("			}");
                        }
                        else if (GfuStrCmp(types[i], "BYTE") || GfuStrCmp(types[i], "INT"))
                        {
                            fileString +=
                                FormatLine("			GUILayout.Label( \"" + colNames[i] + "\", GUILayout.Width( 150.0f ) );");
                            fileString += FormatLine("			{");
                            fileString += FormatLine("				EditorGUILayout.IntField( r." + varNames[i] + " );");
                            fileString += FormatLine("			}");
                        }
                        else if (GfuStrCmp(types[i], "BOOL ARRAY"))
                        {
                            fileString +=
                                FormatLine("				EditorGUILayout.Toggle( System.Convert.ToBoolean( r." + varNames[i] +
                                           "[" +
                                           varNames[i] + "_Index] ) );");
                            fileString += FormatLine("			}");
                        }
                        else if (GfuStrCmp(types[i], "STRING ARRAY"))
                        {
                            fileString +=
                                FormatLine("				EditorGUILayout.TextField( r." + varNames[i] + "[" + varNames[i] +
                                           "_Index] );");
                            fileString += FormatLine("			}");
                        }
                        else if (GfuStrCmp(types[i], "FLOAT ARRAY"))
                        {
                            fileString +=
                                FormatLine("				EditorGUILayout.FloatField( (float)r." + varNames[i] + "[" + varNames[i] +
                                           "_Index] );");
                            fileString += FormatLine("			}");
                        }
                        else if (IsSupportedArrayType(types[i]) && (GfuStrCmp(StripArray(types[i]), "BYTE") || GfuStrCmp(StripArray(types[i]), "INT")))
                        {
                            fileString +=
                                FormatLine("				EditorGUILayout.IntField( r." + varNames[i] + "[" + varNames[i] +
                                           "_Index] );");
                            fileString += FormatLine("			}");
                        }
                        else if (GfuStrCmp(types[i], "CHAR"))
                        {
                            fileString +=
                                FormatLine("			GUILayout.Label( \"" + colNames[i] + "\", GUILayout.Width( 150.0f ) );");
                            fileString += FormatLine("			{");
                            fileString +=
                                FormatLine("				EditorGUILayout.TextField( System.Convert.ToString( r." + varNames[i] +
                                           " ) );");
                            fileString += FormatLine("			}");
                        }
                        else if (GfuStrCmp(types[i], "BOOL"))
                        {
                            fileString +=
                                FormatLine("			GUILayout.Label( \"" + colNames[i] + "\", GUILayout.Width( 150.0f ) );");
                            fileString += FormatLine("			{");
                            fileString +=
                                FormatLine("				EditorGUILayout.Toggle( System.Convert.ToBoolean( r." + varNames[i] +
                                           " ) );");
                            fileString += FormatLine("			}");
                        }
                        else if (GfuStrCmp(types[i], "STRING"))
                        {
                            fileString +=
                                FormatLine("			GUILayout.Label( \"" + colNames[i] + "\", GUILayout.Width( 150.0f ) );");
                            fileString += FormatLine("			{");
                            fileString += FormatLine("				EditorGUILayout.TextField( r." + varNames[i] + " );");
                            fileString += FormatLine("			}");
                        }
                        else if (GfuStrCmp(types[i], "GAMEOBJECT"))
                        {
                            fileString +=
                                FormatLine("			GUILayout.Label( \"" + colNames[i] + "\", GUILayout.Width( 150.0f ) );");
                            fileString += FormatLine("			{");
                            fileString += FormatLine("				EditorGUILayout.ObjectField( r." + varNames[i] + " );");
                            fileString += FormatLine("			}");
                        }
                        else if (GfuStrCmp(types[i], "VECTOR2"))
                        {
                            fileString +=
                                FormatLine("			EditorGUILayout.Vector2Field( \"" + colNames[i] + "\", r." + varNames[i] +
                                           " );");
                        }
                        else if (IsSupportedArrayType(types[i]) && GfuStrCmp(StripArray(types[i]), "VECTOR2"))
                        {
                            fileString += FormatLine("			EditorGUILayout.EndHorizontal();");
                            fileString += FormatLine("			EditorGUILayout.BeginHorizontal();");
                            fileString +=
                                FormatLine("			EditorGUILayout.Vector2Field( \"\", r." + varNames[i] + "[" + varNames[i] +
                                           "_Index] );");
                            fileString += FormatLine("			}");
                        }
                        else if (GfuStrCmp(types[i], "VECTOR3"))
                        {
                            fileString +=
                                FormatLine("			EditorGUILayout.Vector3Field( \"" + colNames[i] + "\", r." + varNames[i] +
                                           " );");
                        }
                        else if (IsSupportedArrayType(types[i]) && GfuStrCmp(StripArray(types[i]), "VECTOR3"))
                        {
                            fileString += FormatLine("			EditorGUILayout.EndHorizontal();");
                            fileString += FormatLine("			EditorGUILayout.BeginHorizontal();");
                            fileString +=
                                FormatLine("			EditorGUILayout.Vector3Field( \"\", r." + varNames[i] + "[" + varNames[i] +
                                           "_Index] );");
                            fileString += FormatLine("			}");
                        }
                        else if (GfuStrCmp(types[i], "COLOR") || GfuStrCmp(types[i], "COLOR32"))
                        {
                            fileString +=
                                FormatLine("			GUILayout.Label( \"" + colNames[i] + "\", GUILayout.Width( 150.0f ) );");
                            fileString += FormatLine("			{");
                            fileString += FormatLine("				EditorGUILayout.ColorField( r." + varNames[i] + " );");
                            fileString += FormatLine("			}");
                        }
                        else if (IsSupportedArrayType(types[i]) && (GfuStrCmp(StripArray(types[i]), "COLOR") || GfuStrCmp(StripArray(types[i]), "COLOR32")))
                        {
                            fileString += FormatLine("			EditorGUILayout.EndHorizontal();");
                            fileString += FormatLine("			EditorGUILayout.BeginHorizontal();");
                            fileString +=
                                FormatLine("			EditorGUILayout.ColorField( \"\", r." + varNames[i] + "[" + varNames[i] +
                                           "_Index] );");
                            fileString += FormatLine("			}");
                        }
                        else if (GfuStrCmp(types[i], "QUATERNION"))
                        {
                            fileString +=
                                FormatLine("          Vector4 converted" + colNames[i] + " = new Vector4( r." +
                                           varNames[i] +
                                           ".x, " +
                                           "r." + varNames[i] + ".y, " +
                                           "r." + varNames[i] + ".z, " +
                                           "r." + varNames[i] + ".w ); ");
                            fileString +=
                                FormatLine("			EditorGUILayout.Vector4Field( \"" + colNames[i] + "\", converted" +
                                           colNames[i] + " );");
                        }
                        else if (IsSupportedArrayType(types[i]) && GfuStrCmp(StripArray(types[i]), "QUATERNION"))
                        {
                            fileString += FormatLine("			EditorGUILayout.EndHorizontal();");
                            fileString += FormatLine("			EditorGUILayout.BeginHorizontal();");
                            fileString +=
                                FormatLine("          Vector4 converted" + colNames[i] + " = new Vector4( r." +
                                           varNames[i] +
                                           "[" + varNames[i] + "_Index].x, " +
                                           "r." + varNames[i] + "[" + varNames[i] + "_Index].y, " +
                                           "r." + varNames[i] + "[" + varNames[i] + "_Index].z, " +
                                           "r." + varNames[i] + "[" + varNames[i] + "_Index].w ); ");
                            fileString +=
                                FormatLine("			EditorGUILayout.Vector4Field( \"\", converted" + colNames[i] + " );");
                            fileString += FormatLine("			}");
                        }
                    }

                    fileString += FormatLine("			EditorGUILayout.EndHorizontal();");
                    fileString += FormatLine(System.String.Empty);
                }

                fileString += FormatLine("		}");
                fileString += FormatLine("	}");
                fileString += FormatLine("}");


                sw.Write(fileString);

                ///////////////////////////////////
                // done writing, clean up
                sw.Flush();
                sw.Close();
                fs.Close();

                ///////////////////////////////////
                // export playmaker actions (check if playmaker is installed first)
                if (_FoundPlaymaker && (GetBool(_ActiveWorkbook.Title + "." + in_entry.Title.Text + ".OBJDB" + ".PM", false)))
                {
                    /////////////////////////////
                    // Generate the Action for Get*DataByID
                    Debug.Log("Saving to: " + GoogleFuGenPath("PLAYMAKER") + "/GoogleFu/Get" + in_fileName + "DataByID.cs");

                    if (System.IO.Directory.Exists(GoogleFuGenPath("PLAYMAKER") + "/GoogleFu") == false)
                        System.IO.Directory.CreateDirectory(GoogleFuGenPath("PLAYMAKER") + "/GoogleFu");

                    fs = System.IO.File.Open(GoogleFuGenPath("PLAYMAKER") + "/GoogleFu/Get" + in_fileName + "DataByID.cs", System.IO.File.Exists(GoogleFuGenPath("PLAYMAKER") + "/GoogleFu/Get" + in_fileName + "DataByID.cs") ?
                        System.IO.FileMode.Truncate :
                        System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);

                    if (fs == null)
                    {
                        Debug.LogError("Cannot open " + GoogleFuGenPath("PLAYMAKER") + "/GoogleFu/Get" + in_fileName + "DataByID.cs for writing");
                        return false;
                    }

                    sw = new System.IO.StreamWriter(fs);
                    if (sw == null)
                    {
                        Debug.LogError("Cannot create a streamwriter, dude are you out of memory?");
                        return false;
                    }

                    fileString = System.String.Empty;
                    fileString += FormatLine("using UnityEngine;");
                    fileString += FormatLine(System.String.Empty);

                    fileString += FormatLine("namespace HutongGames.PlayMaker.Actions");
                    fileString += FormatLine("{");
                    fileString += FormatLine("	[ActionCategory(\"GoogleFu\")]");
                    fileString += FormatLine("	[Tooltip(\"Gets the specified entry in the " + in_fileName + " Database.\")]");
                    fileString += FormatLine("	public class Get" + in_fileName + "DataByID : FsmStateAction");
                    fileString += FormatLine("	{");
                    fileString += FormatLine("		[RequiredField]");
                    fileString += FormatLine("		[UIHint(UIHint.Variable)]");
                    fileString += FormatLine("		[Tooltip(\"The object that contains the " + in_fileName + " database.\")]");
                    fileString += FormatLine("		public FsmGameObject databaseObj;");

                    fileString += FormatLine("		[RequiredField]");
                    fileString += FormatLine("		[UIHint(UIHint.Variable)]");
                    fileString += FormatLine("		[Tooltip(\"Row name of the entry you wish to retrieve.\")]");
                    fileString += FormatLine("		public FsmString rowName;");

                    for (var i = 0; i < types.Count; i++)
                    {
                        var fsmvarType = System.String.Empty;
                        var varType = types[i];
                        var varName = varNames[i];

                        if (GfuStrCmp(types[i], "IGNORE") || GfuStrCmp(types[i], "VOID") || IsSupportedArrayType(types[i]))
                        {
                            continue;
                        }

                        if (GfuStrCmp(types[i], "FLOAT"))
                        {
                            fsmvarType = "FsmFloat";
                        }
                        else if (GfuStrCmp(types[i], "UNISGNED INT") || GfuStrCmp(types[i], "INT") || GfuStrCmp(types[i], "CHAR"))
                        {
                            fsmvarType = "FsmInt";
                        }
                        else if (GfuStrCmp(types[i], "BOOLEAN") || GfuStrCmp(types[i], "BOOL"))
                        {
                            fsmvarType = "FsmBool";
                        }
                        else if (GfuStrCmp(types[i], "STRING"))
                        {
                            fsmvarType = "FsmString";
                        }
                        else if (GfuStrCmp(types[i], "GAMEOBJECT"))
                        {
                            fsmvarType = "FsmGameObject";
                        }
                        else if (GfuStrCmp(types[i], "VECTOR2"))
                        {
                            fsmvarType = "FsmVector2";
                        }
                        else if (GfuStrCmp(types[i], "VECTOR3"))
                        {
                            fsmvarType = "FsmVector3";
                        }
                        else if (GfuStrCmp(types[i], "COLOR") || GfuStrCmp(types[i], "COLOR32"))
                        {
                            fsmvarType = "FsmColor";
                        }
                        else if (GfuStrCmp(types[i], "QUATERNION"))
                        {
                            fsmvarType = "FsmQuaternion";
                        }

                        fileString += FormatLine("		[UIHint(UIHint.Variable)]");
                        fileString += FormatLine("		[Tooltip(\"Store the " + varName + " in a " + varType + " variable.\")]");

                        fileString += FormatLine("		public " + fsmvarType + " " + varName + ";");
                    }

                    fileString += FormatLine("		public override void Reset()");
                    fileString += FormatLine("		{");
                    fileString += FormatLine("			databaseObj = null;");
                    fileString += FormatLine("			rowName = null;");

                    for (int index = 0; index < varNames.Count; index++)
                    {
                        var varName = varNames[index];

                        if (GfuStrCmp(types[index], "IGNORE") || GfuStrCmp(types[index], "VOID") || IsSupportedArrayType(types[index]))
                            continue;

                        var tmpVarName = varName.Split(new[] { '_' }, System.StringSplitOptions.RemoveEmptyEntries)[0];

                        if (GfuStrCmp(tmpVarName, "IGNORE") || GfuStrCmp(tmpVarName, "VOID"))
                            continue;

                        fileString += FormatLine("			" + varName + " = null;");
                    }

                    fileString += FormatLine("		}");

                    fileString += FormatLine("		public override void OnEnter()");
                    fileString += FormatLine("		{");
                    fileString += FormatLine("			if ( databaseObj != null && rowName != null && rowName.Value != System.String.Empty )");
                    fileString += FormatLine("			{");
                    fileString += FormatLine("				GoogleFu." + in_fileName + " db = databaseObj.Value.GetComponent<GoogleFu." + in_fileName + ">();");
                    fileString += FormatLine("				GoogleFu." + in_fileName + "Row row = db.GetRow( rowName.Value );");

                    for (int index = 0; index < varNames.Count; index++)
                    {
                        var varName = varNames[index];

                        if (GfuStrCmp(types[index], "IGNORE") || GfuStrCmp(types[index], "VOID") || IsSupportedArrayType(types[index]))
                            continue;

                        var tmpVarName = varName.Split(new[] { '_' }, System.StringSplitOptions.RemoveEmptyEntries)[0];

                        if (GfuStrCmp(tmpVarName, "IGNORE") || GfuStrCmp(tmpVarName, "VOID"))
                            continue;

                        fileString += FormatLine("				if ( " + varName + " != null )");
                        fileString += FormatLine("				" + varName + ".Value = row." + varName + ";");
                    }

                    fileString += FormatLine("			}");
                    fileString += FormatLine("			Finish();");
                    fileString += FormatLine("		}");
                    fileString += FormatLine("	}");
                    fileString += FormatLine("}");

                    sw.Write(fileString);

                    ///////////////////////////////////
                    // done writing, clean up
                    sw.Flush();
                    sw.Close();
                    fs.Close();

                    /////////////////////////////
                    // Generate the Action for Get*DataByIndex
                    Debug.Log("Saving to: " + GoogleFuGenPath("PLAYMAKER") + "/GoogleFu/Get" + in_fileName + "DataByIndex.cs");

                    if (System.IO.Directory.Exists(GoogleFuGenPath("PLAYMAKER") + "/GoogleFu") == false)
                        System.IO.Directory.CreateDirectory(GoogleFuGenPath("PLAYMAKER") + "/GoogleFu");

                    fs = System.IO.File.Open(GoogleFuGenPath("PLAYMAKER") + "/GoogleFu/Get" + in_fileName + "DataByIndex.cs", System.IO.File.Exists(GoogleFuGenPath("PLAYMAKER") + "/GoogleFu/Get" + in_fileName + "DataByIndex.cs") ? System.IO.FileMode.Truncate : System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);

                    if (fs == null)
                    {
                        Debug.LogError("Cannot open " + GoogleFuGenPath("PLAYMAKER") + "/GoogleFu/Get" + in_fileName + "DataByIndex.cs for writing");
                        return false;
                    }

                    sw = new System.IO.StreamWriter(fs);
                    if (sw == null)
                    {
                        Debug.LogError("Cannot create a streamwriter, dude are you out of memory?");
                        return false;
                    }

                    fileString = System.String.Empty;
                    fileString += FormatLine("using UnityEngine;");
                    fileString += FormatLine(System.String.Empty);

                    fileString += FormatLine("namespace HutongGames.PlayMaker.Actions");
                    fileString += FormatLine("{");
                    fileString += FormatLine("	[ActionCategory(\"GoogleFu\")]");
                    fileString += FormatLine("	[Tooltip(\"Gets the specified entry in the " + in_fileName + " Database By Index.\")]");
                    fileString += FormatLine("	public class Get" + in_fileName + "DataByIndex : FsmStateAction");
                    fileString += FormatLine("	{");
                    fileString += FormatLine("		[RequiredField]");
                    fileString += FormatLine("		[UIHint(UIHint.Variable)]");
                    fileString += FormatLine("		[Tooltip(\"The object that contains the " + in_fileName + " database.\")]");
                    fileString += FormatLine("		public FsmGameObject databaseObj;");

                    fileString += FormatLine("		[RequiredField]");
                    fileString += FormatLine("		[UIHint(UIHint.Variable)]");
                    fileString += FormatLine("		[Tooltip(\"Row index of the entry you wish to retrieve.\")]");
                    fileString += FormatLine("		public FsmInt rowIndex;");

                    fileString += FormatLine("		[UIHint(UIHint.Variable)]");
                    fileString += FormatLine("		[Tooltip(\"Row ID of the entry.\")]");
                    fileString += FormatLine("		public FsmString rowName;");

                    for (var i = 0; i < types.Count; i++)
                    {
                        var fsmvarType = System.String.Empty;
                        var varType = types[i];
                        var varName = varNames[i];

                        if (GfuStrCmp(types[i], "IGNORE") || GfuStrCmp(types[i], "VOID") || IsSupportedArrayType(types[i]))
                        {
                            continue;
                        }

                        if (GfuStrCmp(types[i], "FLOAT"))
                        {
                            fsmvarType = "FsmFloat";
                        }
                        else if (GfuStrCmp(types[i], "BYTE") || GfuStrCmp(types[i], "INT") || GfuStrCmp(types[i], "CHAR"))
                        {
                            fsmvarType = "FsmInt";
                        }
                        else if (GfuStrCmp(types[i], "BOOLEAN") || GfuStrCmp(types[i], "BOOL"))
                        {
                            fsmvarType = "FsmBool";
                        }
                        else if (GfuStrCmp(types[i], "STRING"))
                        {
                            fsmvarType = "FsmString";
                        }
                        else if (GfuStrCmp(types[i], "GAMEOBJECT"))
                        {
                            fsmvarType = "FsmGameObject";
                        }
                        else if (GfuStrCmp(types[i], "VECTOR2"))
                        {
                            fsmvarType = "FsmVector2";
                        }
                        else if (GfuStrCmp(types[i], "VECTOR3"))
                        {
                            fsmvarType = "FsmVector3";
                        }
                        else if (GfuStrCmp(types[i], "COLOR") || GfuStrCmp(types[i], "COLOR32"))
                        {
                            fsmvarType = "FsmColor";
                        }
                        else if (GfuStrCmp(types[i], "QUATERNION"))
                        {
                            fsmvarType = "FsmQuaternion";
                        }

                        fileString += FormatLine("		[UIHint(UIHint.Variable)]");
                        fileString += FormatLine("		[Tooltip(\"Store the " + varName + " in a " + varType + " variable.\")]");

                        fileString += FormatLine("		public " + fsmvarType + " " + varName + ";");
                    }

                    fileString += FormatLine("		public override void Reset()");
                    fileString += FormatLine("		{");
                    fileString += FormatLine("			databaseObj = null;");
                    fileString += FormatLine("			rowIndex = null;");

                    for (int index = 0; index < varNames.Count; index++)
                    {
                        var varName = varNames[index];

                        if (GfuStrCmp(types[index], "IGNORE") || GfuStrCmp(types[index], "VOID") || IsSupportedArrayType(types[index]))
                            continue;

                        var tmpVarName = varName.Split(new[] { '_' }, System.StringSplitOptions.RemoveEmptyEntries)[0];

                        if (GfuStrCmp(tmpVarName, "IGNORE") || GfuStrCmp(tmpVarName, "VOID"))
                            continue;

                        fileString += FormatLine("			" + varName + " = null;");
                    }

                    fileString += FormatLine("		}");

                    fileString += FormatLine("		public override void OnEnter()");
                    fileString += FormatLine("		{");
                    fileString += FormatLine("			if ( databaseObj != null && rowIndex != null )");
                    fileString += FormatLine("			{");
                    fileString += FormatLine("				GoogleFu." + in_fileName + " db = databaseObj.Value.GetComponent<GoogleFu." + in_fileName + ">();");

                    fileString += FormatLine("				// For sanity sake, we are going to do an auto-wrap based on the input");
                    fileString += FormatLine("				// This should prevent accessing the array out of bounds");
                    fileString += FormatLine("				int i = rowIndex.Value;");
                    fileString += FormatLine("				int L = db.Rows.Count;");
                    fileString += FormatLine("				while ( i < 0 )");
                    fileString += FormatLine("					i += L;");
                    fileString += FormatLine("				while ( i > L-1 )");
                    fileString += FormatLine("					i -= L;");
                    fileString += FormatLine("				GoogleFu." + in_fileName + "Row row = db.Rows[i];");

                    fileString += FormatLine("				if ( rowName != null )");
                    fileString += FormatLine("					rowName.Value = db.rowNames[i];");

                    for (int index = 0; index < varNames.Count; index++)
                    {
                        var varName = varNames[index];

                        if (GfuStrCmp(types[index], "IGNORE") || GfuStrCmp(types[index], "VOID") || IsSupportedArrayType(types[index]))
                            continue;

                        var tmpVarName = varName.Split(new[] { '_' }, System.StringSplitOptions.RemoveEmptyEntries)[0];

                        if (GfuStrCmp(tmpVarName, "IGNORE") || GfuStrCmp(tmpVarName, "VOID"))
                            continue;

                        fileString += FormatLine("				if ( " + varName + " != null )");
                        fileString += FormatLine("				" + varName + ".Value = row." + varName + ";");
                    }

                    fileString += FormatLine("			}");
                    fileString += FormatLine("			Finish();");
                    fileString += FormatLine("		}");
                    fileString += FormatLine("	}");
                    fileString += FormatLine("}");

                    sw.Write(fileString);

                    ///////////////////////////////////
                    // done writing, clean up
                    sw.Flush();
                    sw.Close();
                    fs.Close();


                    /////////////////////////////
                    // Generate the Action for Get*DataByIndex
                    Debug.Log("Saving to: " + GoogleFuGenPath("PLAYMAKER") + "/GoogleFu/Get" + in_fileName + "Count.cs");

                    if (System.IO.Directory.Exists(GoogleFuGenPath("PLAYMAKER") + "/GoogleFu") == false)
                        System.IO.Directory.CreateDirectory(GoogleFuGenPath("PLAYMAKER") + "/GoogleFu");

                    fs = System.IO.File.Open(GoogleFuGenPath("PLAYMAKER") + "/GoogleFu/Get" + in_fileName + "Count.cs", System.IO.File.Exists(GoogleFuGenPath("PLAYMAKER") + "/GoogleFu/Get" + in_fileName + "Count.cs") ?
                        System.IO.FileMode.Truncate :
                        System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);

                    if (fs == null)
                    {
                        Debug.LogError("Cannot open " + GoogleFuGenPath("PLAYMAKER") + "/GoogleFu/Get" + in_fileName + "Count.cs for writing");
                        return false;
                    }

                    sw = new System.IO.StreamWriter(fs);
                    if (sw == null)
                    {
                        Debug.LogError("Cannot create a streamwriter, dude are you out of memory?");
                        return false;
                    }

                    fileString = System.String.Empty;
                    fileString += FormatLine("using UnityEngine;");
                    fileString += FormatLine(System.String.Empty);

                    fileString += FormatLine("namespace HutongGames.PlayMaker.Actions");
                    fileString += FormatLine("{");
                    fileString += FormatLine("	[ActionCategory(\"GoogleFu\")]");
                    fileString += FormatLine("	[Tooltip(\"Gets the specified entry in the " + in_fileName + " Database By Index.\")]");
                    fileString += FormatLine("	public class Get" + in_fileName + "Count : FsmStateAction");
                    fileString += FormatLine("	{");
                    fileString += FormatLine("		[RequiredField]");
                    fileString += FormatLine("		[UIHint(UIHint.Variable)]");
                    fileString += FormatLine("		[Tooltip(\"The object that contains the " + in_fileName + " database.\")]");
                    fileString += FormatLine("		public FsmGameObject databaseObj;");

                    fileString += FormatLine("		[UIHint(UIHint.Variable)]");
                    fileString += FormatLine("		[Tooltip(\"Row Count of the database.\")]");
                    fileString += FormatLine("		public FsmInt rowCount;");

                    fileString += FormatLine("		public override void Reset()");
                    fileString += FormatLine("		{");
                    fileString += FormatLine("			databaseObj = null;");
                    fileString += FormatLine("			rowCount = null;");
                    fileString += FormatLine("		}");

                    fileString += FormatLine("		public override void OnEnter()");
                    fileString += FormatLine("		{");
                    fileString += FormatLine("			if ( databaseObj != null && rowCount != null )");
                    fileString += FormatLine("			{");
                    fileString += FormatLine("				GoogleFu." + in_fileName + " db = databaseObj.Value.GetComponent<GoogleFu." + in_fileName + ">();");
                    fileString += FormatLine("				rowCount.Value = db.Rows.Count;");
                    fileString += FormatLine("			}");
                    fileString += FormatLine("			Finish();");
                    fileString += FormatLine("		}");
                    fileString += FormatLine("	}");
                    fileString += FormatLine("}");

                    sw.Write(fileString);

                    ///////////////////////////////////
                    // done writing, clean up
                    sw.Flush();
                    sw.Close();
                    fs.Close();
                }
            }

            ShowNotification(new GUIContent("Saving to: " + in_path + "/" + in_fileName + ".cs"));
            Debug.Log("Saving to: " + in_path);
            return true;
        }

        private static void FixVarType(ref System.String vartype)
        {
            if (string.Compare(vartype, "float", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = vartype.ToLower();
            else if (string.Compare(vartype, "float array", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = vartype.ToLower();
            else if (string.Compare(vartype, "float []", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "float array";
            else if (string.Compare(vartype, "float[]", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "float array";
            else if (string.Compare(vartype, "int", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = vartype.ToLower();
            else if (string.Compare(vartype, "int array", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = vartype.ToLower();
            else if (string.Compare(vartype, "int []", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "int array";
            else if (string.Compare(vartype, "int[]", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "int array";
            else if (string.Compare(vartype, "boolean", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "bool";
            else if (string.Compare(vartype, "boolean array", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "bool array";
            else if (string.Compare(vartype, "boolean []", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "bool array";
            else if (string.Compare(vartype, "boolean[]", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "bool array";
            else if (string.Compare(vartype, "bool", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = vartype.ToLower();
            else if (string.Compare(vartype, "bool array", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = vartype.ToLower();
            else if (string.Compare(vartype, "bool []", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "bool array";
            else if (string.Compare(vartype, "bool[]", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "bool array";
            else if (string.Compare(vartype, "char", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = vartype.ToLower();
            else if (string.Compare(vartype, "byte", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = vartype.ToLower();
            else if (string.Compare(vartype, "byte array", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = vartype.ToLower();
            else if (string.Compare(vartype, "byte []", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "byte array";
            else if (string.Compare(vartype, "byte[]", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "byte array";
            else if (string.Compare(vartype, "string", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = vartype.ToLower();
            else if (string.Compare(vartype, "string array", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = vartype.ToLower();
            else if (string.Compare(vartype, "string []", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "string array";
            else if (string.Compare(vartype, "string[]", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "string array";
            else if (string.Compare(vartype, "vector2", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "Vector2";
            else if (string.Compare(vartype, "vector2 array", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "Vector2 array";
            else if (string.Compare(vartype, "vector2 []", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "Vector2 array";
            else if (string.Compare(vartype, "vector2[]", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "Vector2 array";
            else if (string.Compare(vartype, "vector3", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "Vector3";
            else if (string.Compare(vartype, "vector3 array", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "Vector3 array";
            else if (string.Compare(vartype, "vector3 []", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "Vector3 array";
            else if (string.Compare(vartype, "vector3[]", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "Vector3 array";
            else if (string.Compare(vartype, "vector", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "Vector3";
            else if (string.Compare(vartype, "vector array", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "Vector3 array";
            else if (string.Compare(vartype, "vector []", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "Vector3 array";
            else if (string.Compare(vartype, "vector[]", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "Vector3 array";
            else if (string.Compare(vartype, "color", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "Color";
            else if (string.Compare(vartype, "color array", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "Color array";
            else if (string.Compare(vartype, "color []", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "Color array";
            else if (string.Compare(vartype, "color[]", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "Color array";
            else if (string.Compare(vartype, "color32", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "Color32";
            else if (string.Compare(vartype, "color32 array", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "Color32 array";
            else if (string.Compare(vartype, "color32 []", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "Color32 array";
            else if (string.Compare(vartype, "color32[]", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "Color32 array";
            else if (string.Compare(vartype, "quaternion", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "Quaternion";
            else if (string.Compare(vartype, "quaternion array", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "Quaternion array";
            else if (string.Compare(vartype, "quaternion []", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "Quaternion array";
            else if (string.Compare(vartype, "quaternion[]", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "Quaternion array";
            else if (string.Compare(vartype, "quat", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "Quaternion";
            else if (string.Compare(vartype, "quat array", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "Quaternion array";
            else if (string.Compare(vartype, "quat []", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "Quaternion array";
            else if (string.Compare(vartype, "quat[]", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "Quaternion array";
            else if (string.Compare(vartype, "ignore", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "Ignore";
            else if (string.Compare(vartype, "void", System.StringComparison.InvariantCultureIgnoreCase) == 0)
                vartype = "Ignore";
        }
        static string FormatLine(string in_line)
        {
            return in_line + System.Environment.NewLine;
        }

        bool IsDataValid(System.Collections.Generic.IEnumerable<string> in_colNames, System.Collections.Generic.IEnumerable<string> in_rowNames)
        {
            var ret = true;
            var hashset = new System.Collections.Generic.HashSet<string>();

            foreach (var item in in_colNames)
            {
                if (hashset.Add(item))
                {
                    if (!ContainsKeyword(item)) continue;
                    Debug.LogError("Unsupported column name (" + item +
                                   ") please check you name is not a reserved word or keyword and change this value");
                    ret = false;
                    break;
                }

                Debug.LogError("Duplicate column name (" + item + ") please check your column names for duplicate names");
                ret = false;
                break;
            }

            // rowNames must be unique, valid enumerations
            if (!ret) return false;

            hashset.Clear();
            foreach (var item in in_rowNames)
            {
                if (hashset.Add(item))
                {
                    if (IsValidEnumerationName(item)) continue;
                    Debug.LogError("Unsupported row name (" + item +
                                   ") please check you name is not a reserved word or keyword and change this value");
                    ret = false;
                    break;
                }

                Debug.LogError("Duplicate row name (" + item + ") please check your row names for duplicate names");
                ret = false;
                break;
            }

            return ret;
        }

        bool IsDataValid(System.Collections.Generic.IEnumerable<string> in_types, System.Collections.Generic.IEnumerable<string> in_colNames, System.Collections.Generic.IEnumerable<string> in_rowNames)
        {
            bool ret = true;
            var hashset = new System.Collections.Generic.HashSet<string>();
            // types must be a type we support
            foreach (var type in in_types.Where(in_type => !IsSupportedType(in_type)))
            {
                Debug.LogError("Unsupported type " + type + " please check your database column types and change this value to a supported type");
                ret = false;
                break;
            }

            // colNames cannot contain language keywords, and must also be unique
            if (ret)
            {
                foreach (var item in in_colNames.Where(in_item => !GfuStrCmp(in_item, "void")))
                {
                    if (hashset.Add(item))
                    {
                        if (!ContainsKeyword(item)) continue;
                        Debug.LogError("Unsupported column name (" + item +
                                       ") please check you name is not a reserved word or keyword and change this value");
                        ret = false;
                        break;
                    }

                    Debug.LogError("Duplicate column name (" + item + ") please check your column names for duplicate names");
                    ret = false;
                    break;
                }
            }

            // rowNames must be unique, valid enumerations
            if (!ret) return false;

            hashset.Clear();
            foreach (var item in in_rowNames.Where(in_item => !GfuStrCmp(in_item, "void")))
            {
                if (hashset.Add(item))
                {
                    if (IsValidEnumerationName(item)) continue;
                    Debug.LogError("Unsupported row name (" + item +
                                   ") please check you name is not a reserved word or keyword and change this value");
                    ret = false;
                    break;
                }

                Debug.LogError("Duplicate row name (" + item + ") please check your row names for duplicate names");
                ret = false;
                break;
            }

            return ret;
        }

        private bool IsSupportedArrayType(string in_type)
        {
            string[] supportedTypes = {
				"FLOAT ARRAY", "FLOAT[]", "FLOAT []",
				"INT ARRAY", "INT[]", "INT []",
				"BYTE ARRAY", "BYTE[]", "BYTE []",
				"BOOL ARRAY", "BOOL[]", "BOOL []",
				"STRING ARRAY", "STRING[]", "STRING []",
				"VECTOR2 ARRAY", "VECTOR2[]", "VECTOR2 []",
				"VECTOR3 ARRAY", "VECTOR3[]", "VECTOR3 []",
				"COLOR ARRAY", "COLOR[]", "COLOR []",
				"COLOR32 ARRAY", "COLOR32[]", "COLOR32 []",
				"QUATERNION ARRAY", "QUATERNION[]", "QUATERNION []" };

            return supportedTypes.Any(in_x => GfuStrCmp(in_type, in_x));
        }

        private bool IsSupportedType(string in_type)
        {
            string[] supportedTypes = {
				"FLOAT",
				"INT",
				"BYTE",
				"BOOL",
				"STRING",
				"CHAR",
				"VECTOR2",
				"VECTOR3",
				"COLOR",
				"COLOR32",
				"QUATERNION",
                "IGNORE" };

            return supportedTypes.Any(in_x => GfuStrCmp(in_type, in_x)) || IsSupportedArrayType(in_type);
        }

        static string MakeValidVariableName(string in_string)
        {
            var ret = in_string;
            if (string.IsNullOrEmpty(ret))
            {
                ret = "ANON_" + _Anoncolname;
                _Anoncolname++;
            }

            string[] invalidCharacters = { " ", ",", ".", "?", "\"", ";", ":", 
				"\'", "[", "]", "{", "}", "!", "@", "#", 
				"$", "%", "^", "&", "*", "(", ")", "-", 
				"/", "\\" };

            ret = invalidCharacters.Aggregate(ret, (in_current, in_x) => in_current.Replace(in_x, "_"));

            ret = "_" + ret;

            return ret;
        }

        private static bool IsValidEnumerationName(System.String in_string)
        {
            if (string.IsNullOrEmpty(in_string))
                return false;

            var ret = true;

            string[] invalidStarts = { " ", "0", "1", "2", "3", "4", "5", 
				"6", "7", "8", "9", "\t", "\n", "\r" };
            foreach (var x in invalidStarts.Where(in_string.StartsWith))
            {
                Debug.LogError("Found invalid starting character: ( " + x + " ) in word " + in_string);
                ret = false;
                break;
            }
            return ret;
        }

        private static bool ContainsKeyword(System.String in_string)
        {
            var ret = false;

            string[] stringArray = { "abstract", "event", "new", "struct",
				"as", "explicit", "null", "switch",
				"base", "extern", "object", "this",
				"bool", "false", "operator", "throw",
				"break", "finally", "out", "true",
				"byte", "fixed", "override", "try",
				"case", "float", "params", "typeof",
				"catch", "for", "private", "uint",
				"char", "foreach", "protected", "ulong",
				"checked", "goto", "public", "unchecked",
				"class", "if", "readonly", "unsafe",
				"const", "implicit", "ref", "ushort",
				"continue", "in", "return", "using",
				"decimal", "int", "sbyte", "virtual",
				"default", "interface", "sealed", "volatile",
				"delegate", "internal", "short",
				"do", "sizeof", "while",
				"double", "lock", "stackalloc",
				"else", "long", "static",
				"enum", "namespace", "string" };

            foreach (var x in stringArray.Where(in_x => in_x.Equals(in_string)))
            {
                Debug.LogError("Found the keyword: ( " + x + " ) this is not allowed");
                ret = true;
                break;
            }

            return ret;
        }
    }
}
