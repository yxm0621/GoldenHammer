//----------------------------------------------
//    Google2u: Google Doc Unity integration
//         Copyright © 2015 Litteratus
//----------------------------------------------

using System;
using UnityEngine;

namespace Google2u
{
    [Serializable]
    public class Google2uExportOptions
    {
        private readonly string _Prefix;

        #region Legacy
        public bool LowercaseHeader = false;
        #endregion

        #region Whitespace
        public bool TrimStrings = true;
        public bool TrimStringArrays = true;
        #endregion

        #region Delimiters

        public string[] DelimiterOptionStrings = {", - Comma", "| - Pipe", "  - Space"};
        public string[] DelimiterOptions = {",", "|", " "};

        public int ArrayDelimiters = 0;
        public int StringArrayDelimiters = 1;
        public int ComplexTypeDelimiters = 0;
        public int ComplexArrayDelimiters = 1;
        #endregion

        #region ObjectDatabase Options
        [SerializeField]
        private GameObject _ExportDatabaseGameObject;
        public string ExportDatabaseGameObjectName;
        public bool ObjectDBCullColumns;
        public bool ObjectDBCullRows;

        public GameObject ExportDatabaseGameObject
        {
            get
            {
                if (string.IsNullOrEmpty(ExportDatabaseGameObjectName))
                    return null;

                var go = GameObject.Find(ExportDatabaseGameObjectName);
                if (go != null)
                {
                    _ExportDatabaseGameObject = go;
                }
                return _ExportDatabaseGameObject;
            }
            set
            {
                if (value == null)
                {
                    ExportDatabaseGameObjectName = string.Empty;
                    Google2uGUIUtil.SetString(_Prefix + "GameObjectDatabaseName", string.Empty);
                    _ExportDatabaseGameObject = null;
                }
                else
                {
                    ExportDatabaseGameObjectName = value.name;
                    Google2uGUIUtil.SetString(_Prefix + "GameObjectDatabaseName", value.name);
                    _ExportDatabaseGameObject = value;
                }
            }
        }

        public bool GeneratePlaymakerActions;
        public bool UseDoNotDestroy;
        #endregion

        #region StaticDatabase Options
        public bool StaticDBCullColumns;
        public bool StaticDBCullRows;
        #endregion

        #region JSON Options

        public enum ExportType
        {
            ExportObject,
            ExportArray
        }

        public bool EscapeUnicode;
        public bool JSONCellArrayToString;
        public bool JSONExportClass;
        public ExportType JSONExportType = ExportType.ExportObject;
        public bool JSONCullColumns;
        public bool JSONCullRows;
        #endregion

        #region XML Options
        public bool XMLCellArrayToString;
        public bool XMLCullColumns;
        public bool XMLCullRows;
        #endregion

        #region CSV Options
        public bool EscapeCSVStrings;
        public bool CSVCullColumns;
        public bool CSVCullRows;
        public bool CSVConvertLineBreaks;
        #endregion

        #region CSV Options
        public bool EscapeNGUIStrings;
        public bool NGUICullColumns;
        public bool NGUICullRows;
        public bool NGUIConvertLineBreaks;
        #endregion

        public Google2uExportOptions(string in_prefix)
        {
            _Prefix = in_prefix;

            LowercaseHeader = Google2uGUIUtil.GetBool(in_prefix + "LowercaseHeader", LowercaseHeader);

            TrimStrings = Google2uGUIUtil.GetBool(in_prefix + "TrimStrings", TrimStrings);
            TrimStringArrays = Google2uGUIUtil.GetBool(in_prefix + "TrimStringArrays", TrimStringArrays);

            ArrayDelimiters = Google2uGUIUtil.GetInt(in_prefix + "ArrayDelimiters", ArrayDelimiters);
            StringArrayDelimiters = Google2uGUIUtil.GetInt(in_prefix + "StringArrayDelimiters", StringArrayDelimiters);
            ComplexTypeDelimiters = Google2uGUIUtil.GetInt(in_prefix + "ComplexTypeDelimiters", ComplexTypeDelimiters);
            ComplexArrayDelimiters = Google2uGUIUtil.GetInt(in_prefix + "ComplexArrayDelimiters", ComplexArrayDelimiters);

            #region ObjectDatabase Options
            var dbObjName = Google2uGUIUtil.GetString(_Prefix + "GameObjectDatabaseName", string.Empty);
            if (string.IsNullOrEmpty(dbObjName) == false)
            {
                var go = GameObject.Find(dbObjName);
                if (go)
                {
                    ExportDatabaseGameObjectName = dbObjName;
                    _ExportDatabaseGameObject = go;
                }
            }
            GeneratePlaymakerActions = Google2uGUIUtil.GetBool(in_prefix + "GeneratePlaymakerActions", GeneratePlaymakerActions);
            UseDoNotDestroy = Google2uGUIUtil.GetBool(in_prefix + "UseDoNotDestroy", UseDoNotDestroy);
            ObjectDBCullColumns = Google2uGUIUtil.GetBool(in_prefix + "ObjectDBCullColumns", ObjectDBCullColumns);
            ObjectDBCullRows = Google2uGUIUtil.GetBool(in_prefix + "ObjectDBCullRows", ObjectDBCullRows);
            #endregion

            #region Static DB Options
            StaticDBCullColumns = Google2uGUIUtil.GetBool(in_prefix + "StaticDBCullColumns", StaticDBCullColumns);
            StaticDBCullRows = Google2uGUIUtil.GetBool(in_prefix + "StaticDBCullRows", StaticDBCullRows);
            #endregion

            #region JSON Options
            EscapeUnicode = Google2uGUIUtil.GetBool(in_prefix + "EscapeUnicode", EscapeUnicode);
            JSONCellArrayToString = Google2uGUIUtil.GetBool(in_prefix + "JSONCellArrayToString", JSONCellArrayToString);
            JSONExportClass = Google2uGUIUtil.GetBool(in_prefix + "JSONExportClass", JSONExportClass);
            JSONExportType = Google2uGUIUtil.GetEnum(in_prefix + "JSONExportType", JSONExportType);
            JSONCullColumns = Google2uGUIUtil.GetBool(in_prefix + "JSONCullColumns", JSONCullColumns);
            JSONCullRows = Google2uGUIUtil.GetBool(in_prefix + "JSONCullRows", JSONCullRows);
            #endregion

            #region XML Options
            XMLCellArrayToString = Google2uGUIUtil.GetBool(in_prefix + "XMLCellArrayToString", XMLCellArrayToString);
            XMLCullColumns = Google2uGUIUtil.GetBool(in_prefix + "XMLCullColumns", XMLCullColumns);
            XMLCullRows = Google2uGUIUtil.GetBool(in_prefix + "XMLCullRows", XMLCullRows);
            #endregion

            #region CSV Options
            EscapeCSVStrings = Google2uGUIUtil.GetBool(in_prefix + "EscapeCSVStrings", EscapeCSVStrings);
            CSVCullColumns = Google2uGUIUtil.GetBool(in_prefix + "CSVCullColumns", CSVCullColumns);
            CSVCullRows = Google2uGUIUtil.GetBool(in_prefix + "CSVCullRows", CSVCullRows);
            CSVConvertLineBreaks = Google2uGUIUtil.GetBool(in_prefix + "CSVConvertLineBreaks", CSVConvertLineBreaks);
            #endregion

            #region NGUI Options
            EscapeNGUIStrings = Google2uGUIUtil.GetBool(in_prefix + "EscapeNGUIStrings", EscapeNGUIStrings);
            NGUICullColumns = Google2uGUIUtil.GetBool(in_prefix + "NGUICullColumns", NGUICullColumns);
            NGUICullRows = Google2uGUIUtil.GetBool(in_prefix + "NGUICullRows", NGUICullRows);
            NGUIConvertLineBreaks = Google2uGUIUtil.GetBool(in_prefix + "NGUIConvertLineBreaks", NGUIConvertLineBreaks);
            #endregion
        }
    }
}
