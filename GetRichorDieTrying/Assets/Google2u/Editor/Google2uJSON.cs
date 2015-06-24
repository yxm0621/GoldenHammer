//----------------------------------------------
//    Google2u: Google Doc Unity integration
//         Copyright © 2015 Litteratus
//----------------------------------------------

using System;
using System.IO;
using UnityEditor;

namespace Google2u
{
    public partial class Google2u : EditorWindow
    {
        public static bool IsSupportedArrayType(SupportedType in_type)
        {
            switch(in_type)
            {
                case SupportedType.FloatArray:
                case SupportedType.IntArray:
                case SupportedType.BoolArray:
                case SupportedType.ByteArray:
                case SupportedType.StringArray:
                case SupportedType.Vector2Array:
                case SupportedType.Vector3Array:
                case SupportedType.ColorArray:
                case SupportedType.Color32Array:
                case SupportedType.QuaternionArray:
                    return true;
            }
            return false;
        }

        private static string SanitizeJson(string in_value, bool in_escapeUnicode)
        {
            var sb = new System.Text.StringBuilder();
            foreach (var c in in_value)
            {
                if ((c > 127) && (in_escapeUnicode))
                {
                    // change this character into a unicode escape
                    string encodedValue = "\\u" + ((int) c).ToString("x4");
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

        public static string ExportJsonObjectClassString(Google2uWorksheet in_sheet, Google2uExportOptions in_options)
        {
            return ExportJsonObjectClassString(in_sheet, in_options, 0);
        }
        public static string ExportJsonObjectClassString(Google2uWorksheet in_sheet, Google2uExportOptions in_options, int in_indent)
        {
            var retString =  string.Empty;

            if (in_sheet.Rows.Count <= 0)
                return retString;

            var headerRow = in_sheet.Rows[0];

            var indent = in_indent;

            retString += FormatLine("//----------------------------------------------");
            retString += FormatLine("//    Google2u: Google Doc Unity integration");
            retString += FormatLine("//         Copyright © 2015 Litteratus");
            retString += FormatLine("//");
            retString += FormatLine("//        This file has been auto-generated");
            retString += FormatLine("//              Do not manually edit");
            retString += FormatLine("//----------------------------------------------");
            retString += FormatLine(System.String.Empty);
            retString += FormatLine("using System.Collections.Generic;"); 
            retString += FormatLine(System.String.Empty);
            retString += FormatLine("namespace Google2u");
            retString += FormatLine("{");

            indent++;

            retString += Indent(indent, "public class " + in_sheet.WorksheetName + "Row" + Environment.NewLine);
            retString += Indent(indent, "{" + Environment.NewLine);

            indent++;

            foreach (var cell in headerRow.Cells)
            {
                if(cell.MyType == SupportedType.Void || cell.MyType == SupportedType.Unrecognized)
                // Don't process rows or columns marked for ignore
                {
                    if (string.IsNullOrEmpty(cell.CellValueString) && in_options.JSONCullColumns)
                        break;
                    continue;
                }

                

                retString += Indent(indent, "public ");

                // check the type
                switch (cell.MyType)
                {
                        case SupportedType.GameObject:
                        case SupportedType.String:
                        retString += "string";
                        break;
                        case SupportedType.Int:
                        retString += "int";
                        break;
                        case SupportedType.Float:
                        retString += "float";
                        break;
                        case SupportedType.Bool:
                        retString += "bool";
                        break;
                        case SupportedType.Byte:
                        retString += "byte";
                        break;
                        case SupportedType.Vector2:
                        retString += "Vector2";
                        break;
                        case SupportedType.Vector3:
                        retString += "Vector3";
                        break;
                        case SupportedType.Color:
                        retString += "Color";
                        break;
                        case SupportedType.Color32:
                        retString += "Color32";
                        break;
                        case SupportedType.Quaternion:
                        retString += "Quaternion";
                        break;
                        case SupportedType.FloatArray:
                        retString += "List< float >";
                        break;
                        case SupportedType.IntArray:
                        retString += "List< int >";
                        break;
                        case SupportedType.BoolArray:
                        retString += "List< bool >";
                        break;
                        case SupportedType.ByteArray:
                        retString += "List< byte >";
                        break;
                        case SupportedType.StringArray:
                        retString += "List< string >";
                        break;
                        case SupportedType.Vector2Array:
                        retString += "List< Vector2 >";
                        break;
                        case SupportedType.Vector3Array:
                        retString += "List< Vector3 >";
                        break;
                        case SupportedType.ColorArray:
                        retString += "List< Color >";
                        break;
                        case SupportedType.Color32Array:
                        retString += "List< Color32 >";
                        break;
                        case SupportedType.QuaternionArray:
                        retString += "List< Quaternion >";
                        break;
                }


                retString += " " + cell.CellValueString + " { get; set; }" + Environment.NewLine;

            }

            indent--;

            retString += Indent(indent, "}" + Environment.NewLine);

            if (in_options.JSONExportType == Google2uExportOptions.ExportType.ExportObject)
            {
                retString += Environment.NewLine;
                retString += Indent(indent, "public class " + in_sheet.WorksheetName + "Database" + Environment.NewLine);
                retString += Indent(indent, "{" + Environment.NewLine);

                indent++;
                retString += Indent(indent,
                    "public List< " + in_sheet.WorksheetName + "Row > Database { get; set; }" + Environment.NewLine);
                indent--;

                retString += Indent(indent, "}" + Environment.NewLine);
            }

            retString += FormatLine("}");

            return retString;
        }

        public static string ExportJsonObjectString(Google2uWorksheet in_sheet, Google2uExportOptions in_options, bool in_newlines)
        {
            var indent = 0;
            var retString = String.Empty;
            var escapeUnicode = in_options.EscapeUnicode;
            var bConvertArrays = !in_options.JSONCellArrayToString;

            if (in_options.JSONExportType == Google2uExportOptions.ExportType.ExportObject)
            {
                retString += Indent(indent, "{");

                if (in_newlines)
                {
                    retString += Environment.NewLine;
                    indent++;
                }

                retString += Indent(indent, ("\"" + SanitizeJson(in_sheet.WorksheetName, escapeUnicode) + "Row\":"));
                    // "sheetName":

                if (in_newlines)
                {
                    retString += Environment.NewLine;
                }

            }

            retString += Indent(indent, "["); // [

            if (in_newlines)
            {
                retString += Environment.NewLine;
                indent++;
            }

            var rowCt = in_sheet.Rows.Count;
            if (rowCt > 0)
            {

                var curRow = 0;
                var validRow = false;

                // Iterate through each row, printing its cell values.
                foreach (var row in in_sheet.Rows)
                {
                    // if we are skipping the type row, record the types and increment curRow now
                    if (curRow == 0 || (curRow == 1 && in_sheet.UseTypeRow))
                    {
                        curRow++;
                        continue;
                    }

                    var rowType = row[0].GetTypeFromValue();
                    var rowHeader = row[0].CellValueString;
                    if (string.IsNullOrEmpty(rowHeader))
                    // if this header is empty
                    {
                        if (in_options.JSONCullRows)
                            break;
                        curRow++;
                        continue;
                    }

                    if (rowType == SupportedType.Void ||
                    rowHeader.Equals("void", StringComparison.InvariantCultureIgnoreCase))
                    // if this cell is void, then skip the row completely
                    {
                        curRow++;
                        continue;
                    }

                    if (validRow)
                    {
                        retString += ",";
                        if (in_newlines)
                        {
                            retString += Environment.NewLine;
                        }
                    }

                    validRow = true;

                    retString += Indent(indent, "{");
                    if (in_newlines)
                    {
                        retString += Environment.NewLine;
                        indent++;
                    }

                    bool firstCell = true;
                    // Iterate over the remaining columns, and print each cell value
                    for (int i = 0; i < in_sheet.Rows[0].Count; i++)
                    {
                        // Don't process rows or columns marked for ignore
                        if ((row[i].MyType == SupportedType.Void ||
                             string.IsNullOrEmpty(row[0].CellValueString) ||
                             (in_options.JSONCullColumns && i > in_sheet.FirstBlankCol)))
                        {
                            continue;
                        }

                        if (firstCell)
                            firstCell = false;
                        else
                        {
                            retString += ", ";
                            if (in_newlines)
                                retString += Environment.NewLine;
                        }

                        if (bConvertArrays && IsSupportedArrayType(in_sheet.Rows[0].Cells[i].MyType))
                        {

                            var delim = in_options.DelimiterOptions[in_options.ArrayDelimiters].ToCharArray();

                            retString += Indent(indent,
                                "\"" + SanitizeJson(in_sheet.Rows[0][i].CellValueString, escapeUnicode) + "\":");

                            if (in_newlines)
                            {
                                retString += Environment.NewLine;
                                indent++;
                            }

                            retString += Indent(indent, "[");

                            if (in_newlines)
                            {
                                retString += Environment.NewLine;
                                indent++;
                            }

                            bool isString = false;

                            if (row[i].MyType == SupportedType.StringArray)
                            {
                                delim = in_options.DelimiterOptions[in_options.StringArrayDelimiters].ToCharArray();
                                isString = true;
                            }
                            if (i == 0)
                                isString = true;

                            var value = row[i].CellValueString.Split(delim, System.StringSplitOptions.RemoveEmptyEntries);
                            var ct = 0;
                            foreach (var s in value)
                            {
                                if (isString)
                                {
                                    retString += Indent(indent, "\"" + SanitizeJson(s, escapeUnicode) + "\"");
                                }
                                else if (in_sheet.Rows[0].Cells[i].MyType == SupportedType.BoolArray)
                                {
                                    string val = s.ToLower();
                                    if (val == "1")
                                        val = "true";
                                    if (val == "0")
                                        val = "false";
                                    retString += Indent(indent, SanitizeJson(val, escapeUnicode));
                                }
                                else
                                    retString += Indent(indent, SanitizeJson(s, escapeUnicode));

                                if (ct < value.Length - 1)
                                {
                                    retString += ",";
                                    if (in_newlines)
                                    {
                                        retString += Environment.NewLine;
                                    }
                                }
                                ct++;
                            }
                            if (in_newlines)
                            {
                                retString += Environment.NewLine;
                                indent--;
                            }
                            retString += Indent(indent, "]");

                            if (in_newlines)
                            {
                                indent--;
                            }
                        }
                        else
                        {
                            if (in_sheet.UseTypeRow == false ||
                                in_sheet.Rows[0].Cells[i].MyType == SupportedType.String || (i == 0))
                            {
                                retString += Indent(indent, "\"" +
                                              SanitizeJson(in_sheet.Rows[0][i].CellValueString, escapeUnicode) +
                                              "\":\"" +
                                              SanitizeJson(row[i].CellValueString, escapeUnicode) + "\"");
                            }
                            else if (in_sheet.Rows[0].Cells[i].MyType == SupportedType.Bool)
                            {
                                string val = row[i].CellValueString.ToLower();
                                if (val == "1")
                                    val = "true";
                                if (val == "0")
                                    val = "false";
                                retString += Indent(indent, "\"" +
                                                            SanitizeJson(in_sheet.Rows[0][i].CellValueString,
                                                                escapeUnicode) +
                                                            "\":" +
                                                            SanitizeJson(val, escapeUnicode));
                            }

                            else if (in_sheet.Rows[0].Cells[i].MyType == SupportedType.BoolArray)
                            {
                                string val = row[i].CellValueString.ToLower();
                                if (val == "1")
                                    val = "true";
                                if (val == "0")
                                    val = "false";
                                retString += Indent(indent, "\"" +
                                                            SanitizeJson(in_sheet.Rows[0][i].CellValueString,
                                                                escapeUnicode) +
                                                            "\":" +
                                                            SanitizeJson(val, escapeUnicode));
                            }
                            else
                                retString += Indent(indent, "\"" +
                                                            SanitizeJson(in_sheet.Rows[0][i].CellValueString,
                                                                escapeUnicode) +
                                                            "\":" +
                                                            SanitizeJson(row[i].CellValueString, escapeUnicode) + "");
                        }

                        if (in_newlines)
                        {
                            //retString += Environment.NewLine;
                        }


                    }

                    if (in_newlines)
                    {
                        retString += Environment.NewLine;
                        indent--;
                    }

                    retString += Indent(indent, "}");

                    curRow++;
                }

            }

            if (in_newlines)
            {
                retString += Environment.NewLine;
                indent--;
            }
            retString += Indent(indent, "]");

            if (in_options.JSONExportType == Google2uExportOptions.ExportType.ExportObject)
            {
                if (in_newlines)
                {
                    retString += Environment.NewLine;
                    indent--;
                }
                retString += ("}");
            }
            return retString;
        }

        public static void ExportJson(Google2uWorksheet in_sheet, string in_path, Google2uExportOptions in_options)
        {
            if (!Directory.Exists(in_path))
                Directory.CreateDirectory(in_path);

            in_path = Path.Combine(in_path, in_sheet.WorksheetName);

            if (!Directory.Exists(in_path))
                Directory.CreateDirectory(in_path);

            var jsonPath = Path.Combine(in_path, in_sheet.WorksheetName + ".json").Replace('\\', '/'); ;

            using (
                var fs = File.Open(jsonPath,
                    File.Exists(in_path) ? FileMode.Truncate : FileMode.OpenOrCreate,
                    FileAccess.Write))
            {
                using (var sw = new StreamWriter(fs))
                {
                    var fileString = ExportJsonObjectString(in_sheet, in_options, false);
                    sw.Write(fileString);
                    sw.Flush();
                }
            }

            if (in_options.JSONExportClass)
            {
                var jsonClassDir = in_path + "\\Resources";
                if (!Directory.Exists(jsonClassDir))
                    Directory.CreateDirectory(jsonClassDir);

                var jsonClassPath = Path.Combine(jsonClassDir, in_sheet.WorksheetName + ".cs").Replace('\\', '/'); ;

                using (
                var fs = File.Open(jsonClassPath,
                    File.Exists(in_path) ? FileMode.Truncate : FileMode.OpenOrCreate,
                    FileAccess.Write))
                {
                    using (var sw = new StreamWriter(fs))
                    {
                        var fileString = ExportJsonObjectClassString(in_sheet, in_options);
                        sw.Write(fileString);
                        sw.Flush();
                    }
                }
            }

            PushNotification("Saving to: " + in_path);
        }
    }
}
