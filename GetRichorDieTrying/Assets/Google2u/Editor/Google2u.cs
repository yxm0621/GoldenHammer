//----------------------------------------------
//    Google2u: Google Doc Unity integration
//         Copyright © 2015 Litteratus
//----------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using Google.GData.Client;
using Google.GData.Spreadsheets;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Google2u
{

    public class Google2uLocalization
    {
        private string _EditorLanguage = "en";
        public string EditorLanguage { get { return _EditorLanguage; } set { _EditorLanguage = value; } }

        private int _LanguagesIndex = 0;
        public int LanguagesIndex { get { return _LanguagesIndex; } set { _LanguagesIndex = value; } }

        private string[] _LanguageOptions;
        public string[] LanguageOptions { get { return _LanguageOptions; } set { _LanguageOptions = value; } }

        public string Localize(Localization.rowIds in_textID)
        {
            var row = Localization.Instance.GetRow(in_textID);
            if (row != null)
            {
                return row.GetStringData(EditorLanguage);
            }
            return "Unable to find string ID: " + in_textID;

        }

        public string Localize(Documentation.rowIds in_textID)
        {
            var row = Documentation.Instance.GetRow(in_textID);
            if (row != null)
            {
                return row.GetStringData(EditorLanguage);
            }
            return "Unable to find string ID: " + in_textID;
        }
    }

    [Serializable]
    public class Google2uData
    {
        public SpreadsheetsService Service { get; set; }


        public string ProjectPath { get; set; }
        
        public string WorkbookUploadPath { get; set; }
        public int WorkbookUploadProgress { get; set; }
        public string ManualWorkbookUrl { get; set; }
        public string ManualWorkbookCache { get; set; }



        public List<GFCommand> Commands { get; private set; }
        public List<G2GUIMessage> Messages { get; private set; }

        public List<Google2uAccountWorkbook> AccountWorkbooks { get; private set; }

        private Google2uAccountWorkbook[] _AccountWorkbooksDisplay = new Google2uAccountWorkbook[0];
        public Google2uAccountWorkbook[] AccountWorkbooksDisplay { get { return _AccountWorkbooksDisplay; } }

        public List<Google2uManualWorkbook> ManualWorkbooks { get; private set; }
        private Google2uManualWorkbook[] _ManualWorkbooksDisplay = new Google2uManualWorkbook[0];
        public Google2uManualWorkbook[] ManualWorkbooksDisplay { get { return _ManualWorkbooksDisplay; } }

        public Google2uData()
        {
            AccountWorkbooks = new List<Google2uAccountWorkbook>();
            ManualWorkbooks = new List<Google2uManualWorkbook>();
            Messages = new List<G2GUIMessage>();
            Commands = new List<GFCommand>();
        }

        public void EndOfFrame()
        {
            _AccountWorkbooksDisplay = AccountWorkbooks.ToArray();
            foreach (var wb in _AccountWorkbooksDisplay)
            {
                wb.EndOfFrame();
            }

            _ManualWorkbooksDisplay = ManualWorkbooks.ToArray();
            foreach (var wb in _ManualWorkbooksDisplay)
            {
                wb.EndOfFrame();
            }
        }
    }

    [Serializable]
    public class Google2uObjDbExport
    {
        public string ScriptName;
        public string ObjectName;
        public Google2uWorksheet Data;
        public DateTime LastAttempt = DateTime.Now;
        public bool ReloadedAssets = false;
        public bool CullEmptyRows = false;
    }

    [Serializable]
    [InitializeOnLoadAttribute]
    public partial class Google2u : EditorWindow
    {
        public bool _AutoLogin;
        public bool _SaveCredentials;
        public static string Token;
        public static string Username;
        public static string Password;
        public static bool ShowDocsAtStartup = true;
        public static bool FinishedRedraw = false;
        public static DateTime LastCheckedRSS = DateTime.MinValue;

        [SerializeField] private Google2uData _InstanceData;

        public Google2uData InstanceData
        {
            get
            {
                if(_InstanceData == null)
                    _InstanceData = new Google2uData();
                return _InstanceData;
            }
        }

        public static Google2uLocalization LocalizationInfo = new Google2uLocalization();
        public static string Ellipses = "...";
        public static DateTime LastEllipses = DateTime.Now;
        public static int EllipsesCount = 3;
        public static Google2uEditor ActiveWorkbookWindow = null;
        private Thread _LoginThread;
        public DateTime _LastLoginAttempt;
        public int _ImportTryCount = 0;

        private List<Google2uObjDbExport> _ObjDbExport;

        public List<Google2uObjDbExport> ObjDbExport
        {
            get { return _ObjDbExport; }
        }

        private static Google2u _Instance;

        public static Google2u Instance
        {
            get { return _Instance; }
        }

        private static string _NotificationString = string.Empty;

        public static void PushNotification(string in_notify)
        {
            _NotificationString = "Google2u: " + in_notify;
        }

        public static string PopNotification()
        {
            var ret = _NotificationString;
            _NotificationString = string.Empty;
            return ret;
        }

        public static string UpdateMessage = string.Empty;
        public static string GoogleRSSMessage = string.Empty;

        public static void CheckForService()
        {
            var feed = new GoogleAppStatus();
            foreach (var item in feed.RowNews.Items)
            {
                if (item.Title.Equals("Google Sheets"))
                {
                    GoogleRSSMessage = item.Description;
                    break;
                }
            }
        }

        public static void CheckForUpdate()
        {
            try
            {
                var ma = 2;
                var mi = 0;
                var bu = 3;

                var request =
                    WebRequest.Create("http://www.litteratus.net/CheckUpdate.php?ma=" + ma + "&mi=" + mi + "&bu=" + bu);
                var response = request.GetResponse();
                var data = response.GetResponseStream();
                string html;
                if (data == null) return;
                using (var sr = new StreamReader(data))
                {
                    html = sr.ReadToEnd();
                }

                UpdateMessage = html;
            }
            catch (Exception)
            {
                UpdateMessage = "Unable to check for updates. Try again later.";
            }
            
        }

        [MenuItem("Window/Google2u")]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            var Google2uWindow = GetWindow(typeof(Google2u));
            Google2uWindow.title = "Google2u";
        }


        private void Init()
        {

            var lastChecked = Convert.ToDateTime(Google2uGUIUtil.GetString("Google2uLastCheckedForUpdate", Convert.ToString(DateTime.MinValue)));
            if ((DateTime.Now - lastChecked).Days >= 1)
            {
                Google2uGUIUtil.SetString("Google2uLastCheckedForUpdate", Convert.ToString(DateTime.Now));
                var t = new Thread(CheckForUpdate);
                t.Start();
            }

            ShowDocsAtStartup = Google2uGUIUtil.GetBool("ShowDocsAtStartup", ShowDocsAtStartup);
            if (ShowDocsAtStartup)
                Google2uDocs.ShowWindow(MyGUILayout, LocalizationInfo);

            System.Net.ServicePointManager.ServerCertificateValidationCallback = Validator;
            InstanceData.Service = new SpreadsheetsService("UnityGoogle2u");

            _AutoLogin = Google2uGUIUtil.GetBool("AutoLogin", false);
            _SaveCredentials = Google2uGUIUtil.GetBool("SaveCredentials", false);

            if (_SaveCredentials)
            {
                Username = Google2uGUIUtil.GetString("SavedUsername", string.Empty);
                Password = Google2uGUIUtil.GetString("SavedPassword", string.Empty);
            }

            if (!_AutoLogin)
            {
                Token = string.Empty;
                InstanceData.Service.SetAuthenticationToken(string.Empty);
            }
            else
                if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
            {
                InstanceData.Commands.Add(GFCommand.DoLogin);
            }

            // Close lingering editor windows
            var ed = GetWindow<Google2uEditor>();
            if(ed != null)
                ed.Close();

            if (_ObjDbExport == null)
                _ObjDbExport = new List<Google2uObjDbExport>();

            _Instance = this;
        }


        public void OnDestroy()
        {
            EditorApplication.update -= Update;
            var ed = GetWindow<Google2uEditor>();
            if (ed != null)
                ed.Close();
            var help = GetWindow<Google2uDocs>();
            if(help != null)
                help.Close();

        }

        public static Type GetAssemblyType(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type != null) return type;
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = a.GetType(typeName);
                if (type != null) return type;
            }
            return null;
        }

        // Update is called once per frame
        private void Update()
        {
            if (InstanceData.Service == null)
                Init();

            var notification = PopNotification();
            if (!string.IsNullOrEmpty(notification))
            {
                Debug.Log(notification);
                ShowNotification(new GUIContent(notification));
            }

            if (!string.IsNullOrEmpty(UpdateMessage))
            {
                Debug.Log(UpdateMessage);
                UpdateMessage = string.Empty;
            }

            if ((DateTime.Now - LastEllipses).Milliseconds > 500)
            {
                LastEllipses = DateTime.Now;
                EllipsesCount += 1;
                if (EllipsesCount > 5)
                    EllipsesCount = 2;
                Ellipses = string.Empty;
                for (var i = 0; i < EllipsesCount; ++i)
                    Ellipses += ".";
                Repaint();
            }

            // Detect Skin Changes
            var oldUseDarkSkin = UseDarkSkin;
            if (EditorGUIUtility.isProSkin)
            {
                UseDarkSkin = true;
                if(oldUseDarkSkin != UseDarkSkin)
                    LoadStyles();
            }

            // Prevent Google2u from doing anything while in play mode
            if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling)
                return;

            if ((DateTime.Now - LastCheckedRSS).Hours > 0)
            {
                LastCheckedRSS = DateTime.Now;
                var t = new Thread(CheckForService);
                t.Start();
            }

            if (InstanceData.Commands.Contains(GFCommand.DoLogin))
            {
                InstanceData.Commands.Remove(GFCommand.DoLogin);
                InstanceData.Commands.Add(GFCommand.WaitForLogin);
                lock (InstanceData.Messages)
                {
                    InstanceData.Messages.RemoveAll(
                        in_message => in_message.MessageType == GFGUIMessageType.InvalidLogin);
                }

                _LoginThread = new Thread(DoLogin) {Name = "DoLogin"};
                _LoginThread.Start(InstanceData);
                _LastLoginAttempt = DateTime.Now;
            }

            // Attempt to prevent hangs
            if (InstanceData.Commands.Contains(GFCommand.WaitForLogin))
            {
                if (DateTime.Now - _LastLoginAttempt > new TimeSpan(0, 0, 0, 7))
                {
                    PushNotification("Login took to long to respond. Aborting");
                    InstanceData.Commands.Remove(GFCommand.WaitForLogin);
                    InstanceData.Commands.Add(GFCommand.DoLogout);
                }
            }

            if (InstanceData.Commands.Contains(GFCommand.DoLogout))
            {
                InstanceData.Commands.Remove(GFCommand.DoLogout);
                InstanceData.AccountWorkbooks.Clear();
                var t = new Thread(DoLogout) { Name = "DoLogout" };
                t.Start(InstanceData);
            }

            if (InstanceData.Commands.Contains(GFCommand.RetrieveWorkbooks))
            {
                InstanceData.Commands.Remove(GFCommand.RetrieveWorkbooks);
                InstanceData.Commands.Add(GFCommand.WaitForRetrievingWorkbooks);
                InstanceData.AccountWorkbooks.Clear();
                var t = new Thread(DoWorkbookQuery) { Name = "RetrieveWorkbooks" };
                t.Start(InstanceData);
            }

            if (InstanceData.Commands.Contains(GFCommand.RetrieveManualWorkbooks))
            {
                InstanceData.Commands.Remove(GFCommand.RetrieveManualWorkbooks);
                InstanceData.Commands.Add(GFCommand.WaitForRetrievingManualWorkbooks);
                var t = new Thread(DoManualWorkbookRetrieval) { Name = "ManualWorkbookRetrieval" };
                t.Start(InstanceData);
            }

            if (InstanceData.Commands.Contains(GFCommand.ManualWorkbooksRetrievalComplete))
            {
                InstanceData.Commands.Remove(GFCommand.ManualWorkbooksRetrievalComplete);
                var manualWorkbooksString = InstanceData.ManualWorkbooks.Aggregate(string.Empty, (in_current, in_wb) => in_current + (in_wb.WorkbookUrl + "|"));
                Google2uGUIUtil.SetString(InstanceData.ProjectPath + "_ManualWorkbookCache", manualWorkbooksString);
            }

            if (InstanceData.Commands.Contains(GFCommand.DoUpload))
            {
                InstanceData.Commands.Remove(GFCommand.DoUpload);
                InstanceData.Commands.Add(GFCommand.WaitingForUpload);
                var t = new Thread(DoWorkbookUpload) {Name = "WorkbookUpload"};
                t.Start(InstanceData);
            }

            if (InstanceData.Commands.Contains(GFCommand.UploadComplete))
            {
                InstanceData.Commands.Remove(GFCommand.UploadComplete);
                InstanceData.WorkbookUploadProgress = 0;
                InstanceData.AccountWorkbooks.Clear();
                InstanceData.Commands.Add(GFCommand.RetrieveWorkbooks);
            }

            foreach (var Google2uSpreadsheet in InstanceData.AccountWorkbooks)
            {
                Google2uSpreadsheet.Update();
            }

            foreach (var Google2uSpreadsheet in InstanceData.ManualWorkbooks)
            {
                Google2uSpreadsheet.Update();
            }

            if (_ObjDbExport != null && _ObjDbExport.Count > 0)
            {

                var dbInfo = _ObjDbExport[0];

                if (dbInfo == null)
                {
                    _ObjDbExport.RemoveAt(0);
                    return;
                }

                if ((DateTime.Now - dbInfo.LastAttempt).TotalSeconds < 2)
                    return;

                if (dbInfo.ReloadedAssets == false)
                {
                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                    dbInfo.ReloadedAssets = true;
                    return;
                }

                dbInfo.LastAttempt = DateTime.Now;

                Component comp = null;
                var myAssetPath = string.Empty;


                Debug.Log("Looking for Database Script: " + dbInfo.ScriptName);
                var findAssetArray = AssetDatabase.FindAssets(dbInfo.ScriptName);
                if (findAssetArray.Length > 0)
                {
                    foreach (var s in findAssetArray)
                    {
                        var mypath = AssetDatabase.GUIDToAssetPath(s);

                        if (mypath.EndsWith(".cs"))
                        {
                            myAssetPath = mypath;
                            Debug.Log("Found Database Script at: " + mypath);
                        }
                    }

                    var myType = GetAssemblyType("Google2u." + dbInfo.ScriptName);

                    Debug.Log(dbInfo.ScriptName + ": GetAssemblyType returns " + myType);
                    if (myType != null)
                    {
                var go = GameObject.Find(dbInfo.ObjectName) ?? new GameObject(dbInfo.ObjectName);


                        var toDestroy = go.GetComponent(dbInfo.ScriptName);
                if (toDestroy != null)
                    DestroyImmediate(toDestroy);

#if UNITY_5
                        comp = go.AddComponent(myType);
#else
                        comp = go.AddComponent(dbInfo.ScriptName);
#endif
                }
                }



                if (comp == null)
                {
                    if (!string.IsNullOrEmpty(myAssetPath))
                    {
                        Debug.Log("Attempting to compile: " + myAssetPath);
                        AssetDatabase.ImportAsset(myAssetPath,
                    ImportAssetOptions.ForceSynchronousImport |
                    ImportAssetOptions.ForceUpdate);
                    }

                    if (_ImportTryCount < 5)
                        {
                            _ImportTryCount++;
                            return;
                        }
                        Debug.LogError("Could not add Google2u component base " + dbInfo.ScriptName);
                        _ObjDbExport.Clear();
                        _ImportTryCount = 0;
                        return;
                    }

                _ImportTryCount = 0;
                Debug.Log("Database Script Attached!");
                _ObjDbExport.Remove(dbInfo);

                var rowInputs = new System.Collections.Generic.List<string>();

                var currow = 0;
                foreach (var row in dbInfo.Data.Rows)
                {
                    if (currow == 0)
                    {
                        currow++;
                        continue;
                    }
                    // if types in first row
                    if(dbInfo.Data.UseTypeRow && currow == 1)
                    {
                        currow++;
                        continue;
                    }

                    var rowType = row[0].GetTypeFromValue();
                    var rowHeader = row[0].CellValueString;
                    if (string.IsNullOrEmpty(rowHeader))
                    // if this header is empty
                    {
                        if (dbInfo.CullEmptyRows)
                            break;
                        currow++;
                        continue;
                    }

                    if (rowType == SupportedType.Void ||
                    rowHeader.Equals("void", StringComparison.InvariantCultureIgnoreCase))
                    // if this cell is void, then skip the row completely
                    {
                        currow++;
                        continue;
                    }

                    foreach (var cell in row.Cells)
                    {
                        if (cell.MyType == SupportedType.Void)
                            continue;
                        rowInputs.Add(cell.CellValueString);
                    }
                    (comp as Google2uComponentBase).AddRowGeneric(rowInputs);
                    rowInputs.Clear();
                    currow++;
                }
            }
        }

        public void DoManualWorkbookRetrieval(object in_instance)
        {
            var instance = in_instance as Google2uData;
            if (instance == null)
                return;

            var cacheSplit = instance.ManualWorkbookCache.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var s in cacheSplit)
            {
                AddManualWorkbookByUrl(s, InstanceData);
            }
            instance.Commands.Remove(GFCommand.WaitForRetrievingManualWorkbooks);
            instance.Commands.Add(GFCommand.ManualWorkbooksRetrievalComplete);
        }

        static void AddManualWorkbookByUrl(string in_manualUrl, Google2uData in_instance)
        {
            WorkbookBase info;
            if (string.IsNullOrEmpty(in_manualUrl))
            {
                Debug.LogError(LocalizationInfo.Localize(Localization.rowIds.ID_ERROR_EMPTY_URL));
                return;
            }

            var refreshManualWorkbookCache = false;
            try
            {
                var key = in_manualUrl.Substring(in_manualUrl.IndexOf("key=", System.StringComparison.InvariantCultureIgnoreCase) + 4);
                key = key.Split(new[] { '&' })[0];

                var singleQuery = new WorksheetQuery(key, "public", "values");
                var feed = in_instance.Service.Query(singleQuery);

                var finalUrl = in_manualUrl.Split(new[] { '&' })[0];

                if (feed != null)
                {

                    info = in_instance.ManualWorkbooks.Find( in_i => Google2uGUIUtil.GfuStrCmp(in_i.WorkbookUrl, finalUrl)) as WorkbookBase ??
                           in_instance.AccountWorkbooks.Find(in_i => Google2uGUIUtil.GfuStrCmp(in_i.WorkbookUrl, finalUrl)) as WorkbookBase;
                    if (info == null)
                    {
                        var newWorkbook = new Google2uManualWorkbook(feed, finalUrl, feed.Title.Text, in_instance.Service);
                        in_instance.ManualWorkbooks.Add(newWorkbook);

                        refreshManualWorkbookCache = true;
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
                    var feed = in_instance.Service.Query(singleQuery);
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

                        info = in_instance.ManualWorkbooks.Find(in_i => Google2uGUIUtil.GfuStrCmp(in_i.WorkbookUrl, finalUrl)) as WorkbookBase ??
                               in_instance.AccountWorkbooks.Find(in_i => Google2uGUIUtil.GfuStrCmp(in_i.WorkbookUrl, finalUrl)) as WorkbookBase;
                        if (info == null)
                        {
                            var newWorkbook = new Google2uManualWorkbook(feed, finalUrl, feed.Title.Text, in_instance.Service);
                            in_instance.ManualWorkbooks.Add(newWorkbook);

                            refreshManualWorkbookCache = true;
                        }

                    }
                }
                catch
                {
                    Debug.LogError(LocalizationInfo.Localize(Localization.rowIds.ID_ERROR_INVALID_URL));
                }
            }

            if (refreshManualWorkbookCache)
            {
                in_instance.ManualWorkbookCache = string.Empty;
                foreach (var Google2uSpreadsheet in in_instance.ManualWorkbooks)
                {
                    in_instance.ManualWorkbookCache += Google2uSpreadsheet.WorkbookUrl + "|";
                }
            }
        }


		public void DoWorkbookUpload(object in_instance)
        {
            var instance = in_instance as Google2uData;
            if (instance == null)
                return;

            if (!string.IsNullOrEmpty(instance.WorkbookUploadPath))
            {
                try
                {
                    // We need a DocumentService
                    var service = new Google.GData.Documents.DocumentsService("Google2uUploader");
                    var mimeType = Google2uMimeType.GetMimeType(instance.WorkbookUploadPath);

                    var authenticator = new ClientLoginAuthenticator("UnityGoogle2u", ServiceNames.Documents, Username, Password);

                    // Instantiate a DocumentEntry object to be inserted.
                    var entry = new Google.GData.Documents.DocumentEntry
                    {
                        MediaSource = new MediaFileSource(instance.WorkbookUploadPath, mimeType)
                    };

                    // Define the resumable upload link
                    var createUploadUrl = new Uri("https://docs.google.com/feeds/upload/create-session/default/private/full");
                    var link = new AtomLink(createUploadUrl.AbsoluteUri)
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
                    uploader.InsertAsync(authenticator, entry, instance);
                }
                catch (Exception ex)
                {
                    instance.Messages.Add(new G2GUIMessage(GFGUIMessageType.InvalidLogin, ex.Message));
                    instance.Commands.Remove(GFCommand.WaitingForUpload);
                }
            }
            
        }


        static void OnSpreadsheetUploadDone(object in_sender, Google.GData.Client.AsyncOperationCompletedEventArgs in_e)
        {
            Instance.InstanceData.Commands.Remove(GFCommand.WaitingForUpload);
            Instance.InstanceData.Commands.Add(GFCommand.UploadComplete);
        }

        static void OnSpreadsheetUploadProgress(object in_sender, Google.GData.Client.AsyncOperationProgressEventArgs in_e)
        {
            Instance.InstanceData.WorkbookUploadProgress = in_e.ProgressPercentage;
        }
        public void DoWorkbookQuery(object in_instance)
        {
            var instance = in_instance as Google2uData;
            if (instance == null)
                return;

            try
            {
                instance.AccountWorkbooks.Clear();
                var spreadsheetQuery = new SpreadsheetQuery();
                var spreadsheetFeed = InstanceData.Service.Query(spreadsheetQuery);

                foreach (var entry in spreadsheetFeed.Entries)
                {
                    var workbook = new Google2uAccountWorkbook(entry as SpreadsheetEntry, instance.Service);

                    instance.AccountWorkbooks.Add(workbook);
                }
            }
            catch (Exception ex)
            {
                if (!instance.Commands.Contains(GFCommand.DoLogout))
                {
                    instance.Commands.Add(GFCommand.DoLogout);
                }

                instance.Messages.Add(new G2GUIMessage(GFGUIMessageType.InvalidLogin, ex.Message));
            }

            instance.Commands.Remove(GFCommand.WaitForRetrievingWorkbooks);

        }

        public void DoLogout(object in_instance)
        {
            var instance = in_instance as Google2uData;
            if (instance == null)
                return;

            instance.Service.setUserCredentials(string.Empty, string.Empty);
            instance.Service.SetAuthenticationToken(string.Empty);

            if (!_SaveCredentials)
            {
                Username = string.Empty;
                Password = string.Empty;
            }

            Token = string.Empty;

        }

        public void DoLogin(object in_instance)
        {
            var instance = in_instance as Google2uData;
            if (instance == null)
                return;
            try
            {
                instance.Service.setUserCredentials(Username, Password);

                var token = instance.Service.QueryClientLoginToken();

                Token = token;

                instance.Service.SetAuthenticationToken(token);

                if (!instance.Commands.Contains(GFCommand.RetrieveWorkbooks))
                    instance.Commands.Add(GFCommand.RetrieveWorkbooks);
            }
            catch (Exception ex)
            {
                // We run our own shorter timeout, so ignore that one.
                if (!ex.Message.Contains("The request timed out"))
                {
                    if (!instance.Commands.Contains(GFCommand.DoLogout))
                        instance.Commands.Add(GFCommand.DoLogout);

                    PushNotification("Unable to Login: " + ex.Message);
                    instance.Messages.Add(new G2GUIMessage(GFGUIMessageType.InvalidLogin, ex.Message));
                }

            }

            instance.Commands.Remove(GFCommand.WaitForLogin);
        }

        public static bool Validator(object in_sender, System.Security.Cryptography.X509Certificates.X509Certificate in_certificate, System.Security.Cryptography.X509Certificates.X509Chain in_chain,
                                      System.Net.Security.SslPolicyErrors in_sslPolicyErrors)
        {
            return true;
        }

        public static string Google2uGenPath(string in_pathType)
        {
            string retPath = string.Empty;

            if (Google2uGUIUtil.GfuStrCmp(in_pathType, "Google2uGEN"))
            {
                retPath = Path.Combine(Instance.InstanceData.ProjectPath, "Google2uGen").Replace('\\','/');
                if (!Directory.Exists(retPath))
                    Directory.CreateDirectory(retPath);
            } // Standard Assets
            else if (Google2uGUIUtil.GfuStrCmp(in_pathType, "OBJDB"))
            {
                {
                    var Google2ugenPath = Google2uGenPath("Google2uGEN");

                    retPath = Path.Combine(Google2ugenPath, "ObjDB").Replace('\\', '/');
                    if (!Directory.Exists(retPath))
                        Directory.CreateDirectory(retPath);
                }
            }
            else if (Google2uGUIUtil.GfuStrCmp(in_pathType, "OBJDBRESOURCES"))
            {
                {
                    retPath = Google2uGUIUtil.GetString("g2uobjDBResourcesDirectory", retPath);
                    if (!Directory.Exists(retPath))
                    {
                        string objdbPath = Google2uGenPath("OBJDB");

                        retPath = Path.Combine(objdbPath, "Resources").Replace('\\', '/');
                        if (!Directory.Exists(retPath))
                            Directory.CreateDirectory(retPath);

                        Google2uGUIUtil.SetString("g2uobjDBResourcesDirectory", retPath);
                    }
                }
            }
            else if (Google2uGUIUtil.GfuStrCmp(in_pathType, "OBJDBEDITOR"))
            {
                {
                    retPath = Google2uGUIUtil.GetString("g2uobjDBEditorDirectory", retPath);
                    if (!Directory.Exists(retPath))
                    {
                        string objdbPath = Google2uGenPath("OBJDB");

                        retPath = Path.Combine(objdbPath, "Editor").Replace('\\', '/');
                        if (!Directory.Exists(retPath))
                            Directory.CreateDirectory(retPath);

                        Google2uGUIUtil.SetString("g2uobjDBEditorDirectory", retPath);
                    }
                }
            }
            else if (Google2uGUIUtil.GfuStrCmp(in_pathType, "STATICDB"))
            {
                {
                    var Google2ugenPath = Google2uGenPath("Google2uGEN");

                    retPath = Path.Combine(Google2ugenPath, "StaticDB").Replace('\\', '/');
                    if (!Directory.Exists(retPath))
                        Directory.CreateDirectory(retPath);
                }
            }
            else if (Google2uGUIUtil.GfuStrCmp(in_pathType, "STATICDBRESOURCES"))
            {
                {
                    retPath = Google2uGUIUtil.GetString("g2uStaticDBResourcesDirectory", retPath);
                    if (Directory.Exists(retPath))
                        return retPath;
                    var staticdbPath = Google2uGenPath("STATICDB");

                    retPath = Path.Combine(staticdbPath, "Resources").Replace('\\', '/');
                    if (!Directory.Exists(retPath))
                        Directory.CreateDirectory(retPath);

                    Google2uGUIUtil.SetString("g2uStaticDBResourcesDirectory", retPath);
                }
            }
            else if (Google2uGUIUtil.GfuStrCmp(in_pathType, "JSON"))
            {
                {
                    retPath = Google2uGUIUtil.GetString("g2ujsonDirectory", retPath);
                    if (Directory.Exists(retPath))
                        return retPath;
                    var Google2ugenPath = Google2uGenPath("Google2uGEN");

                    retPath = Path.Combine(Google2ugenPath, "JSON").Replace('\\', '/');
                    if (!Directory.Exists(retPath))
                        Directory.CreateDirectory(retPath);

                    Google2uGUIUtil.SetString("g2ujsonDirectory", retPath);
                }
            }
            else if (Google2uGUIUtil.GfuStrCmp(in_pathType, "CSV"))
            {
                {
                    retPath = Google2uGUIUtil.GetString("g2ucsvDirectory", retPath);
                    if (Directory.Exists(retPath))
                        return retPath;
                    var Google2ugenPath = Google2uGenPath("Google2uGEN");

                    retPath = Path.Combine(Google2ugenPath, "CSV").Replace('\\', '/');
                    if (!Directory.Exists(retPath))
                        Directory.CreateDirectory(retPath);

                    Google2uGUIUtil.SetString("g2ucsvDirectory", retPath);
                }
            }
            else if (Google2uGUIUtil.GfuStrCmp(in_pathType, "XML"))
            {
                {
                    retPath = Google2uGUIUtil.GetString("g2uxmlDirectory", retPath);
                    if (Directory.Exists(retPath))
                        return retPath;
                    var Google2ugenPath = Google2uGenPath("Google2uGEN");

                    retPath = Path.Combine(Google2ugenPath, "XML").Replace('\\', '/');
                    if (!Directory.Exists(retPath))
                        Directory.CreateDirectory(retPath);

                    Google2uGUIUtil.SetString("g2uxmlDirectory", retPath);
                }
            }
            else if (Google2uGUIUtil.GfuStrCmp(in_pathType, "NGUI"))
            {
                {
                    retPath = Google2uGUIUtil.GetString("g2unguiDirectory", retPath);
                    if (Directory.Exists(retPath))
                        return retPath;
                    var Google2ugenPath = Google2uGenPath("Google2uGEN");

                    retPath = Path.Combine(Google2ugenPath, "NGUI").Replace('\\', '/');
                    if (!Directory.Exists(retPath))
                        Directory.CreateDirectory(retPath);

                    Google2uGUIUtil.SetString("g2unguiDirectory", retPath);
                }
            }
            else if (Google2uGUIUtil.GfuStrCmp(in_pathType, "PLAYMAKER"))
            {
                {
                    retPath = Google2uGUIUtil.GetString("g2uplaymakerDirectory", retPath);
                    if (Directory.Exists(retPath))
                        return retPath;
                    // attempt to find the playmaker actions directory
                    // We already know that the playmaker dll exists, but we need to find the actual path
                    var playmakerPaths = Directory.GetFiles(Application.dataPath, "PlayMaker.dll",
                        SearchOption.AllDirectories);
                    var playmakerPath = System.String.Empty;
                    if (playmakerPaths.Length > 0)
                    {
                        // We are just going to use the first entry. If there is more than 1 entry, there are bigger issues
                        var fileName = playmakerPaths[0];
                        var fileInfo = new FileInfo(fileName);
                        playmakerPath = fileInfo.DirectoryName;
                    }

                    if (playmakerPath != System.String.Empty)
                    {
                        if (playmakerPath != null) retPath = Path.Combine(playmakerPath, "Actions");
                        if (Directory.Exists(retPath))
                        {
                            // We have found the Playmaker Actions dir!
                            Google2uGUIUtil.SetString("g2uplaymakerDirectory", retPath);
                        }
                        else
                        {
                            // The actions subdirectory doesn't exist? Rather than making it in the playmaker directory,
                            // We will just use our Google2uGen path instead and let the user figure it out
                            var Google2ugenPath = Google2uGenPath("Google2uGEN");

                            retPath = Path.Combine(Google2ugenPath, "PlayMaker").Replace('\\', '/');
                            if (!Directory.Exists(retPath))
                                Directory.CreateDirectory(retPath);

                            Google2uGUIUtil.SetString("g2uplaymakerDirectory", retPath);
                        }
                    }
                }
            }

            return retPath;
        }
    }
}
