//----------------------------------------------
//    Google2u: Google Doc Unity integration
//         Copyright © 2015 Litteratus
//----------------------------------------------

using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace Google2u
{
    public partial class Google2u : EditorWindow
    {

        public static void ExportObjectDb(Google2uWorksheet in_sheet, string in_resourcesPath, string in_editorPath, string in_playmakerPath, Google2uExportOptions in_options)
        {

            if (!Directory.Exists(in_resourcesPath))
                Directory.CreateDirectory(in_resourcesPath);

            if (!Directory.Exists(in_editorPath))
                Directory.CreateDirectory(in_editorPath);


            var arrayDelimiter = in_options.DelimiterOptions[in_options.ArrayDelimiters];
            var stringArrayDelimiter = in_options.DelimiterOptions[in_options.StringArrayDelimiters];
            var complexTypeDelimiter = in_options.DelimiterOptions[in_options.ComplexTypeDelimiters];
            var complexTypeArrayDelimiters = in_options.DelimiterOptions[in_options.ComplexArrayDelimiters];
            
            var typesInFirstRow = in_sheet.UseTypeRow;

            ///////////////////////////////////////////////
            // open the file 
            var className = Path.GetInvalidFileNameChars().Aggregate(in_sheet.WorksheetName, (in_current, in_c) => in_current.Replace(in_c, '_'));

            var exportInfo = new Google2uObjDbExport();
            exportInfo.Data = in_sheet;
            exportInfo.CullEmptyRows = in_options.ObjectDBCullRows;

            exportInfo.ObjectName = string.IsNullOrEmpty(in_options.ExportDatabaseGameObjectName)
                ? "Google2uDatabase"
                : in_options.ExportDatabaseGameObjectName;

            exportInfo.ScriptName = className;

            

            using ( var fs = File.Open(Path.Combine(in_resourcesPath, className + ".cs"), FileMode.Create, FileAccess.Write) )
            {
                using (var sw = new StreamWriter(fs))
                {

                    var fileString = String.Empty;

                    fileString += FormatLine("//----------------------------------------------");
                    fileString += FormatLine("//    Google2u: Google Doc Unity integration");
                    fileString += FormatLine("//         Copyright © 2015 Litteratus");
                    fileString += FormatLine("//");
                    fileString += FormatLine("//        This file has been auto-generated");
                    fileString += FormatLine("//              Do not manually edit");
                    fileString += FormatLine("//----------------------------------------------");
                    fileString += FormatLine(System.String.Empty);
                    fileString += FormatLine("using UnityEngine;");
                    fileString += FormatLine(System.String.Empty);
                    fileString += FormatLine("namespace Google2u");
                    fileString += FormatLine("{");
                    fileString += FormatLine("	[System.Serializable]");
                    fileString += FormatLine("	public class " + className + "Row : IGoogle2uRow");
                    fileString += FormatLine("	{");


                    // variable declarations
                    for (int i = 1; i < in_sheet.Rows[0].Count; i++)
                    {
                        if (in_sheet.Rows[0][i].MyType == SupportedType.Void ||
                            string.IsNullOrEmpty(MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader)) ||
                            (in_options.ObjectDBCullColumns && i > in_sheet.FirstBlankCol))
                        {
                        }
                        else if (IsSupportedArrayType(in_sheet.Rows[0][i].MyType))
                        {
                            fileString +=
                                FormatLine("		public System.Collections.Generic.List<" +
                                           StringSupportedType(in_sheet.Rows[0][i].MyType) + "> " +
                                           MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           " = new System.Collections.Generic.List<" +
                                           StringSupportedType(in_sheet.Rows[0][i].MyType) + ">();");
                        }
                        else
                            fileString +=
                                FormatLine("		public " + StringSupportedType(in_sheet.Rows[0][i].MyType) + " " +
                                           MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + ";");
                    }

                    // constructor parameter list
                    fileString += ("		public " + className + "Row(");
                    {
                        var firstItem = true;
                        for (var i = 0; i < in_sheet.Rows[0].Count; i++)
                        {
                            if (in_sheet.Rows[0][i].MyType == SupportedType.Void ||
                                string.IsNullOrEmpty(MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader)) ||
                                (in_options.ObjectDBCullColumns && i > in_sheet.FirstBlankCol))
                                continue;

                            if (!firstItem)
                                fileString += (", ");
                            firstItem = false;
                            fileString += ("string _" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader));
                        }
                    }
                    fileString += FormatLine(") " + System.Environment.NewLine + "		{");

                    // processing each of the input parameters and copying it into the members
                    for (int i = 1; i < in_sheet.Rows[0].Count; i++)
                    {
                        //nightmare time
                        if (in_sheet.Rows[0][i].MyType == SupportedType.Void ||
                            string.IsNullOrEmpty(MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader)) ||
                            (in_options.ObjectDBCullColumns && i > in_sheet.FirstBlankCol))
                        {
                        }
                        else if (in_sheet.Rows[0][i].MyType == SupportedType.GameObject)
                        {
                            fileString +=
                                FormatLine("			" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           " = GameObject.Find(\"" +
                                           MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + "\");");
                        }
                        else if (in_sheet.Rows[0][i].MyType == SupportedType.Bool)
                        {
                            fileString += FormatLine("			{");
                            fileString += FormatLine("			" + StringSupportedType(in_sheet.Rows[0][i].MyType) + " res;");
                            fileString +=
                                FormatLine("				if(" + StringSupportedType(in_sheet.Rows[0][i].MyType) + ".TryParse(_" +
                                           MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + ", out res))");
                            fileString +=
                                FormatLine("					" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           " = res;");
                            fileString += FormatLine("				else");
                            fileString +=
                                FormatLine("					Debug.LogError(\"Failed To Convert " +
                                           MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           " string: \"+ _" +
                                           MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           " +\" to bool\");");
                            fileString += FormatLine("			}");
                        }
                        else if (in_sheet.Rows[0][i].MyType == SupportedType.Byte)
                        {
                            fileString += FormatLine("			{");
                            fileString += FormatLine("			" + StringSupportedType(in_sheet.Rows[0][i].MyType) + " res;");
                            fileString +=
                                FormatLine("				if(" + StringSupportedType(in_sheet.Rows[0][i].MyType) + ".TryParse(_" +
                                           MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + ", out res))");
                            fileString +=
                                FormatLine("					" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           " = res;");
                            fileString += FormatLine("				else");
                            fileString +=
                                FormatLine("					Debug.LogError(\"Failed To Convert " +
                                           MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           " string: \"+ _" +
                                           MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           " +\" to byte\");");
                            fileString += FormatLine("			}");
                        }
                        else if (in_sheet.Rows[0][i].MyType == SupportedType.Float)
                        {
                            fileString += FormatLine("			{");
                            fileString += FormatLine("			" + StringSupportedType(in_sheet.Rows[0][i].MyType) + " res;");
                            fileString +=
                                FormatLine("				if(" + StringSupportedType(in_sheet.Rows[0][i].MyType) + ".TryParse(_" +
                                           MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + ", out res))");
                            fileString +=
                                FormatLine("					" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           " = res;");
                            fileString += FormatLine("				else");
                            fileString +=
                                FormatLine("					Debug.LogError(\"Failed To Convert " +
                                           MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           " string: \"+ _" +
                                           MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + " +\" to " +
                                           StringSupportedType(in_sheet.Rows[0][i].MyType) + "\");");
                            fileString += FormatLine("			}");
                        }
                        else if ((in_sheet.Rows[0][i].MyType == SupportedType.ByteArray)
                                 || (in_sheet.Rows[0][i].MyType == SupportedType.BoolArray)
                                 || (in_sheet.Rows[0][i].MyType == SupportedType.FloatArray))
                        {
                            fileString += FormatLine("			{");
                            fileString += FormatLine("				" + StringSupportedType(in_sheet.Rows[0][i].MyType) + " res;");
                            fileString +=
                                FormatLine("				string []result = _" +
                                           MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + ".Split(\"" +
                                           arrayDelimiter +
                                           "\".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);");
                            fileString += FormatLine("				for(int i = 0; i < result.Length; i++)");
                            fileString += FormatLine("				{");
                            fileString +=
                                FormatLine("					if(" + StringSupportedType(in_sheet.Rows[0][i].MyType) +
                                           ".TryParse(result[i], out res))");
                            fileString +=
                                FormatLine("						" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           ".Add(res);");
                            fileString += FormatLine("					else");
                            fileString += FormatLine("					{");
                            if (in_sheet.Rows[0][i].MyType == SupportedType.ByteArray)
                                fileString +=
                                    FormatLine("						" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                               ".Add( 0 );");
                            else if (in_sheet.Rows[0][i].MyType == SupportedType.BoolArray)
                                fileString +=
                                    FormatLine("						" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                               ".Add( false );");
                            else if (in_sheet.Rows[0][i].MyType == SupportedType.FloatArray)
                                fileString +=
                                    FormatLine("						" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                               ".Add( float.NaN );");
                            fileString +=
                                FormatLine("						Debug.LogError(\"Failed To Convert " +
                                           MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           " string: \"+ result[i] +\" to " +
                                           (StringSupportedType(in_sheet.Rows[0][i].MyType)) + "\");");
                            fileString += FormatLine("					}");
                            fileString += FormatLine("				}");
                            fileString += FormatLine("			}");
                        }
                        else if (in_sheet.Rows[0][i].MyType == SupportedType.Int)
                        {
                            fileString += FormatLine("			{");
                            fileString += FormatLine("			" + StringSupportedType(in_sheet.Rows[0][i].MyType) + " res;");
                            fileString +=
                                FormatLine("				if(int.TryParse(_" +
                                           MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + ", out res))");
                            fileString +=
                                FormatLine("					" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           " = res;");
                            fileString += FormatLine("				else");
                            fileString +=
                                FormatLine("					Debug.LogError(\"Failed To Convert " +
                                           MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           " string: \"+ _" +
                                           MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           " +\" to int\");");
                            fileString += FormatLine("			}");
                        }
                        else if (in_sheet.Rows[0][i].MyType == SupportedType.IntArray)
                        {
                            fileString += FormatLine("			{");
                            fileString +=
                                FormatLine("				" + (StringSupportedType(in_sheet.Rows[0][i].MyType)) + " res;");
                            fileString +=
                                FormatLine("				string []result = _" +
                                           MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + ".Split(\"" +
                                           arrayDelimiter +
                                           "\".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);");
                            fileString += FormatLine("				for(int i = 0; i < result.Length; i++)");
                            fileString += FormatLine("				{");
                            fileString += FormatLine("					if(int.TryParse(result[i], out res))");
                            fileString +=
                                FormatLine("						" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           ".Add( res );");
                            fileString += FormatLine("					else");
                            fileString += FormatLine("					{");
                            fileString +=
                                FormatLine("						" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           ".Add( 0 );");
                            fileString +=
                                FormatLine("						Debug.LogError(\"Failed To Convert " +
                                           MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           " string: \"+ result[i] +\" to " +
                                           (StringSupportedType(in_sheet.Rows[0][i].MyType)) + "\");");
                            fileString += FormatLine("					}");
                            fileString += FormatLine("				}");
                            fileString += FormatLine("			}");
                        }
                        else if (in_sheet.Rows[0][i].MyType == SupportedType.String)
                        {
                            if (in_options.TrimStrings)
                                fileString +=
                                    FormatLine("			" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                               " = _" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                               ".Trim();");
                            else
                                fileString +=
                                    FormatLine("			" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                               " = _" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                               ";");
                        }
                        else if (in_sheet.Rows[0][i].MyType == SupportedType.StringArray)
                        {
                            fileString += FormatLine("			{");
                            fileString +=
                                FormatLine("				string []result = _" +
                                           MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + ".Split(\"" +
                                           stringArrayDelimiter +
                                           "\".ToCharArray(),System.StringSplitOptions.RemoveEmptyEntries);");
                            fileString += FormatLine("				for(int i = 0; i < result.Length; i++)");
                            fileString += FormatLine("				{");
                            if (in_options.TrimStringArrays)
                                fileString +=
                                    FormatLine("					" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                               ".Add( result[i].Trim() );");
                            else
                                fileString +=
                                    FormatLine("					" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                               ".Add( result[i] );");
                            fileString += FormatLine("				}");
                            fileString += FormatLine("			}");
                        }
                        else if (in_sheet.Rows[0][i].MyType == SupportedType.Vector2)
                        {
                            fileString += FormatLine("			{");
                            fileString +=
                                FormatLine("				string [] splitpath = _" +
                                           MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + ".Split(\"" +
                                           complexTypeDelimiter +
                                           "\".ToCharArray(),System.StringSplitOptions.RemoveEmptyEntries);");
                            fileString += FormatLine("				if(splitpath.Length != 2)");
                            fileString +=
                                FormatLine("					Debug.LogError(\"Incorrect number of parameters for " +
                                           StringSupportedType(in_sheet.Rows[0][i].MyType) + " in \" + _" +
                                           MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + " );");
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
                                                     + "_" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader)
                                                     +
                                                     " + \" Component: \" + splitpath[i] + \" parameter \" + i + \" of variable "
                                                     + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                                     "\");");
                            fileString += FormatLine("					}");
                            fileString += FormatLine("				}");
                            fileString +=
                                FormatLine("				" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           ".x = results[0];");
                            fileString +=
                                FormatLine("				" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           ".y = results[1];");
                            fileString += FormatLine("			}");
                        }
                        else if (in_sheet.Rows[0][i].MyType == SupportedType.Vector2Array)
                        {
                            fileString += FormatLine("			{");

                            fileString +=
                                FormatLine("				string []result = _" +
                                           MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + ".Split(\"" +
                                           complexTypeArrayDelimiters +
                                           "\".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);");
                            fileString += FormatLine("				for(int i = 0; i < result.Length; i++)");
                            fileString += FormatLine("				{");

                            fileString += FormatLine("                  {");
                            fileString +=
                                FormatLine("      				string [] splitpath = result[i].Split(\"" + complexTypeDelimiter +
                                           "\".ToCharArray(),System.StringSplitOptions.RemoveEmptyEntries);");
                            fileString += FormatLine("      				if(splitpath.Length != 2)");
                            fileString +=
                                FormatLine("      					Debug.LogError(\"Incorrect number of parameters for " +
                                           StringSupportedType(in_sheet.Rows[0][i].MyType) +
                                           " in \" + _" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           " );");
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
                                                     + "_" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader)
                                                     +
                                                     " + \" Component: \" + splitpath[i] + \" parameter \" + i + \" of variable "
                                                     + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                                     "\");");
                            fileString += FormatLine("		        			continue;");
                            fileString += FormatLine("		        		}");
                            fileString += FormatLine("		        	}");
                            fileString +=
                                FormatLine("		        		" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           ".Add( new " + (StringSupportedType(in_sheet.Rows[0][i].MyType)) +
                                           "(results[0], results[1] ));");
                            fileString += FormatLine("		        	}");

                            fileString += FormatLine("				}");
                            fileString += FormatLine("			}");
                        }
                        else if (in_sheet.Rows[0][i].MyType == SupportedType.Vector3)
                        {
                            fileString += FormatLine("			{");
                            fileString +=
                                FormatLine("				string [] splitpath = _" +
                                           MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + ".Split(\"" +
                                           complexTypeDelimiter +
                                           "\".ToCharArray(),System.StringSplitOptions.RemoveEmptyEntries);");
                            fileString += FormatLine("				if(splitpath.Length != 3)");
                            fileString +=
                                FormatLine("					Debug.LogError(\"Incorrect number of parameters for " +
                                           StringSupportedType(in_sheet.Rows[0][i].MyType) + " in \" + _" +
                                           MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + " );");
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
                                                     + "_" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader)
                                                     +
                                                     " + \" Component: \" + splitpath[i] + \" parameter \" + i + \" of variable "
                                                     + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                                     "\");");
                            fileString += FormatLine("					}");
                            fileString += FormatLine("				}");
                            fileString +=
                                FormatLine("				" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           ".x = results[0];");
                            fileString +=
                                FormatLine("				" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           ".y = results[1];");
                            fileString +=
                                FormatLine("				" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           ".z = results[2];");
                            fileString += FormatLine("			}");
                        }
                        else if (in_sheet.Rows[0][i].MyType == SupportedType.Vector3Array)
                        {
                            fileString += FormatLine("			{");
                            fileString +=
                                FormatLine("				string []result = _" +
                                           MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + ".Split(\"" +
                                           complexTypeArrayDelimiters +
                                           "\".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);");
                            fileString += FormatLine("				for(int i = 0; i < result.Length; i++)");
                            fileString += FormatLine("				{");

                            fileString += FormatLine("      			{");
                            fileString +=
                                FormatLine("      				string [] splitpath = result[i].Split(\"" + complexTypeDelimiter +
                                           "\".ToCharArray(),System.StringSplitOptions.RemoveEmptyEntries);");
                            fileString += FormatLine("      				if(splitpath.Length != 3)");
                            fileString +=
                                FormatLine("      					Debug.LogError(\"Incorrect number of parameters for " +
                                           StringSupportedType(in_sheet.Rows[0][i].MyType) +
                                           " in \" + _" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           " );");
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
                                                     + "_" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader)
                                                     +
                                                     " + \" Component: \" + splitpath[i] + \" parameter \" + i + \" of variable "
                                                     + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                                     "\");");
                            fileString += FormatLine("		        			continue;");
                            fileString += FormatLine("		        		}");
                            fileString += FormatLine("		        	}");
                            fileString +=
                                FormatLine("		        	" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           ".Add( new " + (StringSupportedType(in_sheet.Rows[0][i].MyType)) +
                                           "(results[0], results[1], results[2] ));");

                            fileString += FormatLine("		        	}");

                            fileString += FormatLine("				}");
                            fileString += FormatLine("			}");
                        }
                        else if (in_sheet.Rows[0][i].MyType == SupportedType.Color)
                        {
                            fileString += FormatLine("			{");
                            fileString +=
                                FormatLine("				string [] splitpath = _" +
                                           MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + ".Split(\"" +
                                           complexTypeDelimiter +
                                           "\".ToCharArray(),System.StringSplitOptions.RemoveEmptyEntries);");
                            fileString += FormatLine("				if(splitpath.Length != 3 && splitpath.Length != 4)");
                            fileString +=
                                FormatLine("					Debug.LogError(\"Incorrect number of parameters for " +
                                           StringSupportedType(in_sheet.Rows[0][i].MyType) + " in \" + _" +
                                           MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + " );");
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
                                                     + "_" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader)
                                                     +
                                                     " + \" Component: \" + splitpath[i] + \" parameter \" + i + \" of variable "
                                                     + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                                     "\");");
                            fileString += FormatLine("					}");
                            fileString += FormatLine("				}");
                            fileString +=
                                FormatLine("				" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           ".r = results[0];");
                            fileString +=
                                FormatLine("				" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           ".g = results[1];");
                            fileString +=
                                FormatLine("				" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           ".b = results[2];");
                            fileString += FormatLine("				if(splitpath.Length == 4)");
                            fileString +=
                                FormatLine("					" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           ".a = results[3];");
                            fileString += FormatLine("			}");
                        }
                        else if (in_sheet.Rows[0][i].MyType == SupportedType.ColorArray)
                        {
                            fileString += FormatLine("			{");
                            fileString +=
                                FormatLine("				string []result = _" +
                                           MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + ".Split(\"" +
                                           complexTypeArrayDelimiters +
                                           "\".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);");
                            fileString += FormatLine("				for(int i = 0; i < result.Length; i++)");
                            fileString += FormatLine("				{");

                            fileString += FormatLine("      			{");
                            fileString +=
                                FormatLine("      				string [] splitpath = result[i].Split(\"" + complexTypeDelimiter +
                                           "\".ToCharArray(),System.StringSplitOptions.RemoveEmptyEntries);");
                            fileString += FormatLine("      				if(splitpath.Length != 3 && splitpath.Length != 4)");
                            fileString +=
                                FormatLine("      					Debug.LogError(\"Incorrect number of parameters for " +
                                           StringSupportedType(in_sheet.Rows[0][i].MyType) +
                                           " in \" + _" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           " );");
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
                                                     + "_" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader)
                                                     +
                                                     " + \" Component: \" + splitpath[i] + \" parameter \" + i + \" of variable "
                                                     + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                                     "\");");
                            fileString += FormatLine("		        			continue;");
                            fileString += FormatLine("		        		}");
                            fileString += FormatLine("		        	}");
                            fileString += FormatLine("		        		if(splitpath.Length == 3)");
                            fileString +=
                                FormatLine("		        		" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           ".Add( new " + (StringSupportedType(in_sheet.Rows[0][i].MyType)) +
                                           "(results[0], results[1], results[2] ));");
                            fileString += FormatLine("		        		else");
                            fileString +=
                                FormatLine("		        		" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           ".Add( new " + (StringSupportedType(in_sheet.Rows[0][i].MyType)) +
                                           "(results[0], results[1], results[2], results[3] ));");

                            fileString += FormatLine("		        	}");

                            fileString += FormatLine("				}");
                            fileString += FormatLine("			}");
                        }
                        else if (in_sheet.Rows[0][i].MyType == SupportedType.Color32)
                        {
                            fileString += FormatLine("			{");
                            fileString +=
                                FormatLine("				string [] splitpath = _" +
                                           MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + ".Split(\"" +
                                           complexTypeDelimiter +
                                           "\".ToCharArray(),System.StringSplitOptions.RemoveEmptyEntries);");
                            fileString += FormatLine("				if(splitpath.Length != 3 && splitpath.Length != 4)");
                            fileString +=
                                FormatLine("					Debug.LogError(\"Incorrect number of parameters for " +
                                           StringSupportedType(in_sheet.Rows[0][i].MyType) + " in \" + _" +
                                           MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + " );");
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
                                                     + "_" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader)
                                                     +
                                                     " + \" Component: \" + splitpath[i] + \" parameter \" + i + \" of variable "
                                                     + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                                     "\");");
                            fileString += FormatLine("					}");
                            fileString += FormatLine("				}");
                            fileString +=
                                FormatLine("				" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           ".r = results[0];");
                            fileString +=
                                FormatLine("				" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           ".g = results[1];");
                            fileString +=
                                FormatLine("				" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           ".b = results[2];");
                            fileString += FormatLine("				if(splitpath.Length == 4)");
                            fileString +=
                                FormatLine("					" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           ".a = results[3];");
                            fileString += FormatLine("			}");
                        }
                        else if (in_sheet.Rows[0][i].MyType == SupportedType.Color32Array)
                        {
                            fileString += FormatLine("			{");
                            fileString +=
                                FormatLine("				string []result = _" +
                                           MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + ".Split(\"" +
                                           complexTypeArrayDelimiters +
                                           "\".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);");
                            fileString += FormatLine("				for(int i = 0; i < result.Length; i++)");
                            fileString += FormatLine("				{");

                            fileString += FormatLine("      			{");
                            fileString +=
                                FormatLine("      				string [] splitpath = result[i].Split(\"" + complexTypeDelimiter +
                                           "\".ToCharArray(),System.StringSplitOptions.RemoveEmptyEntries);");
                            fileString += FormatLine("      				if(splitpath.Length != 3 && splitpath.Length != 4)");
                            fileString +=
                                FormatLine("      					Debug.LogError(\"Incorrect number of parameters for " +
                                           StringSupportedType(in_sheet.Rows[0][i].MyType) +
                                           " in \" + _" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           " );");
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
                                                     + "_" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader)
                                                     +
                                                     " + \" Component: \" + splitpath[i] + \" parameter \" + i + \" of variable "
                                                     + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                                     "\");");
                            fileString += FormatLine("		        			continue;");
                            fileString += FormatLine("		        		}");
                            fileString += FormatLine("		        	}");
                            fileString += FormatLine("		        		if(splitpath.Length == 3)");
                            fileString +=
                                FormatLine("		        		    " +
                                           MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + ".Add( new " +
                                           (StringSupportedType(in_sheet.Rows[0][i].MyType)) +
                                           "(results[0], results[1], results[2], System.Convert.ToByte(0) ));");
                            fileString += FormatLine("		        		else");
                            fileString +=
                                FormatLine("		        		    " +
                                           MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + ".Add( new " +
                                           (StringSupportedType(in_sheet.Rows[0][i].MyType)) +
                                           "(results[0], results[1], results[2], results[3] ));");

                            fileString += FormatLine("		        	}");

                            fileString += FormatLine("				}");
                            fileString += FormatLine("			}");
                        }
                        else if (in_sheet.Rows[0][i].MyType == SupportedType.Quaternion)
                        {
                            fileString += FormatLine("			{");
                            fileString +=
                                FormatLine("				string [] splitpath = _" +
                                           MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + ".Split(\"" +
                                           complexTypeDelimiter +
                                           "\".ToCharArray(),System.StringSplitOptions.RemoveEmptyEntries);");
                            fileString += FormatLine("				if(splitpath.Length != 4)");
                            fileString +=
                                FormatLine("					Debug.LogError(\"Incorrect number of parameters for " +
                                           StringSupportedType(in_sheet.Rows[0][i].MyType) + " in \" + _" +
                                           MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + " );");
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
                                                     + "_" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader)
                                                     +
                                                     " + \" Component: \" + splitpath[i] + \" parameter \" + i + \" of variable "
                                                     + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                                     "\");");
                            fileString += FormatLine("					}");
                            fileString += FormatLine("				}");
                            fileString +=
                                FormatLine("				" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           ".x = results[0];");
                            fileString +=
                                FormatLine("				" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           ".y = results[1];");
                            fileString +=
                                FormatLine("				" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           ".z = results[2];");
                            fileString +=
                                FormatLine("				" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           ".w = results[3];");
                            fileString += FormatLine("			}");
                        }
                        else if (in_sheet.Rows[0][i].MyType == SupportedType.QuaternionArray)
                        {
                            fileString += FormatLine("			{");
                            fileString +=
                                FormatLine("				string []result = _" +
                                           MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + ".Split(\"" +
                                           complexTypeArrayDelimiters +
                                           "\".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);");
                            fileString += FormatLine("				for(int i = 0; i < result.Length; i++)");
                            fileString += FormatLine("				{");

                            fileString += FormatLine("      			{");
                            fileString +=
                                FormatLine("      				string [] splitpath = result[i].Split(\"" + complexTypeDelimiter +
                                           "\".ToCharArray(),System.StringSplitOptions.RemoveEmptyEntries);");
                            fileString += FormatLine("      				if(splitpath.Length != 3 && splitpath.Length != 4)");
                            fileString +=
                                FormatLine("      					Debug.LogError(\"Incorrect number of parameters for " +
                                           StringSupportedType(in_sheet.Rows[0][i].MyType) +
                                           " in \" + _" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           " );");
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
                                                     + "_" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader)
                                                     +
                                                     " + \" Component: \" + splitpath[i] + \" parameter \" + i + \" of variable "
                                                     + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                                     "\");");
                            fileString += FormatLine("		        			continue;");
                            fileString += FormatLine("		        		}");
                            fileString += FormatLine("		        	}");
                            fileString +=
                                FormatLine("		        		" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           ".Add( new " + (StringSupportedType(in_sheet.Rows[0][i].MyType)) +
                                           "(results[0], results[1], results[2], results[3] ));");
                            fileString += FormatLine("		        	}");

                            fileString += FormatLine("				}");
                            fileString += FormatLine("			}");
                        }
                        else
                        {
                            fileString +=
                                FormatLine("			" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + " = _" +
                                           MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + ";");
                        }
                    }
                    fileString += FormatLine("		}");

                    fileString += FormatLine(System.String.Empty);
                    {

                        int colCount = 0;

                        for (int i = 1; i < in_sheet.Rows[0].Count; i++)
                        {
                            if (in_sheet.Rows[0][i].MyType == SupportedType.Void ||
                                string.IsNullOrEmpty(MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader)) ||
                                (in_options.ObjectDBCullColumns && i > in_sheet.FirstBlankCol))
                                continue;
                            colCount++;
                        }
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
                        for (int i = 1; i < in_sheet.Rows[0].Count; i++)
                        {
                            if (in_sheet.Rows[0][i].MyType == SupportedType.Void ||
                                string.IsNullOrEmpty(MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader)) ||
                                (in_options.ObjectDBCullColumns && i > in_sheet.FirstBlankCol))
                                continue;
                            fileString += FormatLine("				case " + colNum++ + ":");
                            fileString +=
                                FormatLine("					ret = " + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           ".ToString();");
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

                    for (int i = 1; i < in_sheet.Rows[0].Count; i++)
                    {
                        if (in_sheet.Rows[0][i].MyType == SupportedType.Void ||
                            string.IsNullOrEmpty(MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader)) ||
                            (in_options.ObjectDBCullColumns && i > in_sheet.FirstBlankCol))
                            continue;
                        fileString += FormatLine("				case \"" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + "\":");
                        fileString +=
                            FormatLine("					ret = " + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                       ".ToString();");
                        fileString += FormatLine("					break;");
                    }

                    fileString += FormatLine("			}");
                    fileString += FormatLine(System.String.Empty);
                    fileString += FormatLine("			return ret;");
                    fileString += FormatLine("		}");

                    fileString += FormatLine("		public override string ToString()");
                    fileString += FormatLine("		{");
                    fileString += FormatLine("			string ret = System.String.Empty;");
                    for (int i = 1; i < in_sheet.Rows[0].Count; i++)
                    {
                        if (in_sheet.Rows[0][i].MyType == SupportedType.Void ||
                            string.IsNullOrEmpty(MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader)) ||
                            (in_options.ObjectDBCullColumns && i > in_sheet.FirstBlankCol))
                            continue;
                        fileString +=
                            FormatLine("			ret += \"{\" + \"" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                       "\" + \" : \" + " + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                       ".ToString() + \"} \";");
                    }
                    fileString += FormatLine("			return ret;");
                    fileString += FormatLine("		}");

                    fileString += FormatLine("	}");

                    fileString +=
                        FormatLine("	public class " + className + " :  Google2uComponentBase, IGoogle2uDB");
                    fileString += FormatLine("	{");

                    // this is the enums, the enum matches the name of the row
                    fileString += FormatLine("		public enum rowIds {");
                    fileString += ("			");

                    {
                        int curRow = 0;
                        // Iterate through each row, printing its cell values to be the enum names.
                        foreach (var row in in_sheet.Rows)
                        {
                            if (curRow == 0 || (typesInFirstRow && curRow == 1))
                            {
                                curRow++;
                                continue;
                            }

                            var rowType = row[0].MyType;
                            var rowHeader = row[0].CellValueString;
                            if (string.IsNullOrEmpty(rowHeader))
                            // if this header is empty
                            {
                                if (in_options.ObjectDBCullRows)
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

                            fileString += row[0].CellValueString;
                            if ((curRow + 1) % 20 == 0)
                                fileString += System.Environment.NewLine + "			";
                            fileString += (", ");
                            curRow++;
                        }
                    }
                    fileString = fileString.Remove(fileString.Length - 2); // remove the last comma

                    fileString += FormatLine(System.String.Empty);
                    fileString += FormatLine("		};");



                    fileString += FormatLine("		public string [] rowNames = {");
                    fileString += "			";
                    // Iterate through each row, printing its cell values for the row names strings.
                    {
                        int curRow = 0;
                        foreach (var row in in_sheet.Rows)
                        {
                            if (curRow == 0 || (typesInFirstRow && curRow == 1))
                            {
                                curRow++;
                                continue;
                            }

                            var rowType = row[0].MyType;
                            var rowHeader = row[0].CellValueString;
                            if (string.IsNullOrEmpty(rowHeader))
                            // if this header is empty
                            {
                                if (in_options.ObjectDBCullRows)
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

                            fileString += "\"" + row[0].CellValueString + "\"";
                            if ((curRow + 1) % 20 == 0)
                                fileString += System.Environment.NewLine + "			";
                            fileString += ", ";
                            curRow++;
                        }
                    }
                    fileString = fileString.Remove(fileString.Length - 2); // remove the last comma
                    fileString += FormatLine(System.Environment.NewLine + "		};");
                    // the declaration of the storage for the row data
                    fileString +=
                        FormatLine("		public System.Collections.Generic.List<" + className +
                                   "Row> Rows = new System.Collections.Generic.List<" + className + "Row>();");


                    {
                        // the dont destroy on awake flag
                        if (in_options.UseDoNotDestroy)
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
                        fileString +=
                            FormatLine(
                                "		public override void AddRowGeneric (System.Collections.Generic.List<string> input)");
                        fileString += FormatLine("		{");
                        fileString += ("			Rows.Add(new " + className + "Row(");
                        {
                            int typeCount = 1;
                            // variable declarations
                            for (int i = 1; i < in_sheet.Rows[0].Count; i++)
                            {
                                if (in_sheet.Rows[0][i].MyType == SupportedType.Void ||
                                    string.IsNullOrEmpty(MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader)) ||
                                    (in_options.ObjectDBCullColumns && i > in_sheet.FirstBlankCol))
                                    continue;
                                typeCount++;
                            }
                            bool firstItem = true;
                            for (int i = 0; i < typeCount; i++)
                            {
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

                    fileString += FormatLine("		public IGoogle2uRow GetGenRow(string in_RowString)");
                    fileString += FormatLine("		{");
                    fileString += FormatLine("			IGoogle2uRow ret = null;");
                    fileString += FormatLine("			try");
                    fileString += FormatLine("			{");
                    fileString += FormatLine("				ret = Rows[(int)System.Enum.Parse(typeof(rowIds), in_RowString)];");
                    fileString += FormatLine("			}");
                    fileString += FormatLine("			catch(System.ArgumentException) {");
                    fileString += FormatLine("				Debug.LogError( in_RowString + \" is not a member of the rowIds enumeration.\");");
                    fileString += FormatLine("			}");
                    fileString += FormatLine("			return ret;");
                    fileString += FormatLine("		}");

                    fileString += FormatLine("		public IGoogle2uRow GetGenRow(rowIds in_RowID)");
                    fileString += FormatLine("		{");
                    fileString += FormatLine("			IGoogle2uRow ret = null;");
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

                    fileString += FormatLine("		public " + className + "Row GetRow(rowIds in_RowID)");
                    fileString += FormatLine("		{");
                    fileString += FormatLine("			" + className + "Row ret = null;");
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


                    fileString += FormatLine("		public " + className + "Row GetRow(string in_RowString)");
                    fileString += FormatLine("		{");
                    fileString += FormatLine("			" + className + "Row ret = null;");
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
                    sw.Flush();

                } // using StreamWriter

            } // Using Filestream

            exportInfo.LastAttempt = DateTime.Now;
            Instance.ObjDbExport.Add(exportInfo);

            using (var fs = File.Open(Path.Combine(in_editorPath, className + "Editor.cs"), FileMode.Create, FileAccess.Write))
            {
                using (var sw = new StreamWriter(fs))
                {
                    var fileString = String.Empty;
                    //////////////////////////////////////////////////////////////////////////////////////////////////////
                    // Writing out the custom inspector
                    //////////////////////////////////////////////////////////////////////////////////////////////////////

                    fileString += FormatLine("using UnityEngine;");
                    fileString += FormatLine("using UnityEditor;");
                    fileString += FormatLine(System.String.Empty);
                    fileString += FormatLine("namespace Google2u");
                    fileString += FormatLine("{");
                    fileString += FormatLine("	[CustomEditor(typeof(" + className + "))]");
                    fileString += FormatLine("	public class " + className + "Editor : Editor");
                    fileString += FormatLine("	{");
                    fileString += FormatLine("		public int Index = 0;");
                    // sneaky time, count all the arrays and make an index for each of them within the inspector
                    for (int i = 1; i < in_sheet.Rows[0].Count; i++)
                    {
                        if (in_sheet.Rows[0][i].IsArrayType)
                        {
                            fileString +=
                                FormatLine("		public int " + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           "_Index = 0;");
                        }
                    }
                    fileString += FormatLine("		public override void OnInspectorGUI ()");
                    fileString += FormatLine("		{");
                    fileString += FormatLine("			" + className + " s = target as " + className + ";");
                    fileString += FormatLine("			" + className + "Row r = s.Rows[ Index ];");
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

                    for (int i = 1; i < in_sheet.Rows[0].Count; i++)
                    {
                        if (in_options.ObjectDBCullColumns && i > in_sheet.FirstBlankCol)
                            continue;

                        fileString += FormatLine("			EditorGUILayout.BeginHorizontal();");

                        if (in_sheet.Rows[0][i].IsArrayType)
                        {
                            fileString += FormatLine("			if ( r." + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + ".Count == 0 )");
                            fileString += FormatLine("			{");
                            fileString += FormatLine("			    GUILayout.Label( \"" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           "\", GUILayout.Width( 150.0f ) );");
                            fileString += FormatLine("			    {");
                            fileString += FormatLine("			    	EditorGUILayout.LabelField( \"Empty Array\" );");
                            fileString += FormatLine("			    }");
                            fileString += FormatLine("			}");
                            fileString += FormatLine("			else");
                            fileString += FormatLine("			{");
                            fileString += FormatLine("			    GUILayout.Label( \"" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           "\", GUILayout.Width( 130.0f ) );");
                            // when you switch the row you are examining, they may have different array sizes... therefore, we may actually be past the end of the list
                            fileString += FormatLine("			    if ( " + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           "_Index >= r." + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + ".Count )");
                            fileString += FormatLine("				    " + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + "_Index = 0;");
                            // back button
                            fileString += FormatLine("			    if ( GUILayout.Button(\"<\", GUILayout.Width( 18.0f )) )");
                            fileString += FormatLine("			    {");
                            fileString += FormatLine("			    	" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + "_Index -= 1;");
                            fileString += FormatLine("			    	if ( " + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           "_Index < 0 )");
                            fileString += FormatLine("			    		" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           "_Index = r." + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           ".Count - 1;");
                            fileString += FormatLine("			    }");

                            fileString += FormatLine("			    EditorGUILayout.LabelField(" +
                                MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + "_Index.ToString(), GUILayout.Width( 15.0f ));");

                            // fwd button
                            fileString += FormatLine("			    if ( GUILayout.Button(\">\", GUILayout.Width( 18.0f )) )");
                            fileString += FormatLine("			    {");
                            fileString += FormatLine("			    	" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + "_Index += 1;");
                            fileString += FormatLine("			    	if ( " + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           "_Index >= r." + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + ".Count )");
                            fileString += FormatLine("		        		" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                           "_Index = 0;");
                            fileString += FormatLine("				}");
                        }
                        if (in_sheet.Rows[0][i].MyType != SupportedType.Void)
                        {
                            if (in_sheet.Rows[0][i].MyType == SupportedType.Float)
                            {
                                fileString += FormatLine("			GUILayout.Label( \"" +
                                               MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                               "\", GUILayout.Width( 150.0f ) );");
                                fileString += FormatLine("			{");
                                fileString += FormatLine("				EditorGUILayout.FloatField( (float)r." +
                                               MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + " );");
                                fileString += FormatLine("			}");
                            }
                            else if ((in_sheet.Rows[0][i].MyType == SupportedType.Byte) ||
                                     (in_sheet.Rows[0][i].MyType == SupportedType.Int))
                            {
                                fileString += FormatLine("			GUILayout.Label( \"" +
                                               MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                               "\", GUILayout.Width( 150.0f ) );");
                                fileString += FormatLine("			{");
                                fileString += FormatLine("				EditorGUILayout.IntField( r." +
                                               MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + " );");
                                fileString += FormatLine("			}");
                            }
                            else if (in_sheet.Rows[0][i].MyType == SupportedType.BoolArray)
                            {
                                fileString += FormatLine("				EditorGUILayout.Toggle( System.Convert.ToBoolean( r." +
                                               MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                               "[" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + "_Index] ) );");
                                fileString += FormatLine("			}");
                            }
                            else if (in_sheet.Rows[0][i].MyType == SupportedType.StringArray)
                            {
                                fileString += FormatLine("				EditorGUILayout.TextField( r." +
                                               MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + "[" +
                                               MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                               "_Index] );");
                                fileString += FormatLine("			}");
                            }
                            else if (in_sheet.Rows[0][i].MyType == SupportedType.FloatArray)
                            {
                                fileString += FormatLine("				EditorGUILayout.FloatField( (float)r." +
                                               MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + "[" +
                                               MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + "_Index] );");
                                fileString += FormatLine("			}");
                            }
                            else if (IsSupportedArrayType(in_sheet.Rows[0][i].MyType) &&
                                     (in_sheet.Rows[0][i].MyType == SupportedType.ByteArray) ||
                                      (in_sheet.Rows[0][i].MyType == SupportedType.IntArray))
                            {
                                fileString += FormatLine("				EditorGUILayout.IntField( r." +
                                               MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + "[" +
                                               MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + "_Index] );");
                                fileString += FormatLine("			}");
                            }
                            else if (in_sheet.Rows[0][i].MyType == SupportedType.Byte)
                            {
                                fileString += FormatLine("			GUILayout.Label( \"" +
                                               MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                               "\", GUILayout.Width( 150.0f ) );");
                                fileString += FormatLine("			{");
                                fileString += FormatLine("				EditorGUILayout.TextField( System.Convert.ToString( r." +
                                               MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                               " ) );");
                                fileString += FormatLine("			}");
                            }
                            else if (in_sheet.Rows[0][i].MyType == SupportedType.Bool)
                            {
                                fileString +=
                                    FormatLine("			GUILayout.Label( \"" +
                                    MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + "\", GUILayout.Width( 150.0f ) );");
                                fileString += FormatLine("			{");
                                fileString += FormatLine("				EditorGUILayout.Toggle( System.Convert.ToBoolean( r." +
                                               MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + " ) );");
                                fileString += FormatLine("			}");
                            }
                            else if (in_sheet.Rows[0][i].MyType == SupportedType.String)
                            {
                                fileString += FormatLine("			GUILayout.Label( \"" +
                                               MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                               "\", GUILayout.Width( 150.0f ) );");
                                fileString += FormatLine("			{");
                                fileString +=
                                    FormatLine("				EditorGUILayout.TextField( r." +
                                               MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + " );");
                                fileString += FormatLine("			}");
                            }
                            else if (in_sheet.Rows[0][i].MyType == SupportedType.GameObject)
                            {
                                fileString += FormatLine("			GUILayout.Label( \"" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                               "\", GUILayout.Width( 150.0f ) );");
                                fileString += FormatLine("			{");
                                fileString +=
                                    FormatLine("				EditorGUILayout.ObjectField( r." +
                                               MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + " );");
                                fileString += FormatLine("			}");
                            }
                            else if (in_sheet.Rows[0][i].MyType == SupportedType.Vector2)
                            {
                                fileString +=
                                    FormatLine("			EditorGUILayout.Vector2Field( \"" +
                                               MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + "\", r." +
                                               MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + " );");
                            }
                            else if (in_sheet.Rows[0][i].MyType == SupportedType.Vector2Array)
                            {
                                fileString += FormatLine("			EditorGUILayout.EndHorizontal();");
                                fileString += FormatLine("			EditorGUILayout.BeginHorizontal();");
                                fileString += FormatLine("			EditorGUILayout.Vector2Field( \"\", r." +
                                               MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                               "[" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                               "_Index] );");
                                fileString += FormatLine("			}");
                            }
                            else if (in_sheet.Rows[0][i].MyType == SupportedType.Vector3)
                            {
                                fileString += FormatLine("			EditorGUILayout.Vector3Field( \"" +
                                               MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + "\", r." +
                                               MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + " );");
                            }
                            else if (in_sheet.Rows[0][i].MyType == SupportedType.Vector3Array)
                            {
                                fileString += FormatLine("			EditorGUILayout.EndHorizontal();");
                                fileString += FormatLine("			EditorGUILayout.BeginHorizontal();");
                                fileString += FormatLine("			EditorGUILayout.Vector3Field( \"\", r." +
                                               MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + "[" +
                                               MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + "_Index] );");
                                fileString += FormatLine("			}");
                            }
                            else if ((in_sheet.Rows[0][i].MyType == SupportedType.Color) ||
                                     (in_sheet.Rows[0][i].MyType == SupportedType.Color32))
                            {
                                fileString += FormatLine("			GUILayout.Label( \"" +
                                               MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                               "\", GUILayout.Width( 150.0f ) );");
                                fileString += FormatLine("			{");
                                fileString += FormatLine("				EditorGUILayout.ColorField( r." +
                                               MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + " );");
                                fileString += FormatLine("			}");
                            }
                            else if ((in_sheet.Rows[0][i].MyType == SupportedType.ColorArray) ||
                                 (in_sheet.Rows[0][i].MyType == SupportedType.Color32Array))
                            {
                                fileString += FormatLine("			EditorGUILayout.EndHorizontal();");
                                fileString += FormatLine("			EditorGUILayout.BeginHorizontal();");
                                fileString += FormatLine("			EditorGUILayout.ColorField( \"\", r." +
                                    MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                    "[" + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + "_Index] );");
                                fileString += FormatLine("			}");
                            }
                            else if (in_sheet.Rows[0][i].MyType == SupportedType.Quaternion)
                            {
                                fileString +=FormatLine("          Vector4 converted" +
                                        MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) +
                                        " = new Vector4( r." + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + ".x, "
                                        + "r." + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + ".y, "
                                        + "r." + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + ".z, "
                                        + "r." + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + ".w ); ");
                                fileString +=FormatLine("			EditorGUILayout.Vector4Field( \"" +
                                               MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + "\", converted" +
                                               MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + " );");
                            }
                            else if (in_sheet.Rows[0][i].MyType == SupportedType.QuaternionArray)
                            {
                                fileString += FormatLine("			EditorGUILayout.EndHorizontal();");
                                fileString += FormatLine("			EditorGUILayout.BeginHorizontal();");
                                fileString += FormatLine("          Vector4 converted" +
                                               MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + " = new Vector4( r." +
                                               MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + "[" +
                                               MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + "_Index].x, " +
                                               "r." + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + "[" +
                                               MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + "_Index].y, " +
                                               "r." + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + "[" +
                                               MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + "_Index].z, " +
                                               "r." + MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + "[" +
                                               MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + "_Index].w ); ");
                                fileString += FormatLine( "			EditorGUILayout.Vector4Field( \"\", converted" +
                                        MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader) + " );");
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
                }
            }

            ///////////////////////////////////
            // export playmaker actions (check if playmaker is installed first)
            if (in_options.GeneratePlaymakerActions)
            {
                var playmakerPath = in_playmakerPath;
                using (var fs = File.Open(Path.Combine(playmakerPath, "Get" + className + "DataByID.cs"), FileMode.Create, FileAccess.Write))
                {
                    using (var sw = new StreamWriter(fs))
                    {
                        var fileString = String.Empty;

                        /////////////////////////////
                        // Generate the Action for Get*DataByID

                        fileString = System.String.Empty;
                        fileString += FormatLine("using UnityEngine;");
                        fileString += FormatLine(System.String.Empty);

                        fileString += FormatLine("namespace HutongGames.PlayMaker.Actions");
                        fileString += FormatLine("{");
                        fileString += FormatLine("	[ActionCategory(\"Google2u\")]");
                        fileString +=
                            FormatLine("	[Tooltip(\"Gets the specified entry in the " + className +
                                       " Database.\")]");
                        fileString +=
                            FormatLine("	public class Get" + className + "DataByID : FsmStateAction");
                        fileString += FormatLine("	{");
                        fileString += FormatLine("		[RequiredField]");
                        fileString += FormatLine("		[UIHint(UIHint.Variable)]");
                        fileString +=
                            FormatLine("		[Tooltip(\"The object that contains the " + className +
                                       " database.\")]");
                        fileString += FormatLine("		public FsmGameObject databaseObj;");

                        fileString += FormatLine("		[RequiredField]");
                        fileString += FormatLine("		[UIHint(UIHint.Variable)]");
                        fileString += FormatLine("		[Tooltip(\"Row name of the entry you wish to retrieve.\")]");
                        fileString += FormatLine("		public FsmString rowName;");

                        for (int i = 1; i < in_sheet.Rows[0].Count; i++)
                        {
                            var fsmvarType = System.String.Empty;
                            var varType = StringSupportedType(in_sheet.Rows[0][i].MyType);
                            var varName = MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader);

                            if ((in_sheet.Rows[0][i].MyType == SupportedType.Void) ||
                                (IsSupportedArrayType(in_sheet.Rows[0][i].MyType)) ||
                                (in_options.ObjectDBCullColumns && i > in_sheet.FirstBlankCol))
                            {
                                continue;
                            }

                            if (in_sheet.Rows[0][i].MyType == SupportedType.Float)
                            {
                                fsmvarType = "FsmFloat";
                            }
                            else if ((in_sheet.Rows[0][i].MyType == SupportedType.Int) ||
                                     (in_sheet.Rows[0][i].MyType == SupportedType.Byte))
                            {
                                fsmvarType = "FsmInt";
                            }
                            else if (in_sheet.Rows[0][i].MyType == SupportedType.Bool)
                            {
                                fsmvarType = "FsmBool";
                            }
                            else if (in_sheet.Rows[0][i].MyType == SupportedType.String)
                            {
                                fsmvarType = "FsmString";
                            }
                            else if (in_sheet.Rows[0][i].MyType == SupportedType.GameObject)
                            {
                                fsmvarType = "FsmGameObject";
                            }
                            else if (in_sheet.Rows[0][i].MyType == SupportedType.Vector2)
                            {
                                fsmvarType = "FsmVector2";
                            }
                            else if (in_sheet.Rows[0][i].MyType == SupportedType.Vector3)
                            {
                                fsmvarType = "FsmVector3";
                            }
                            else if ((in_sheet.Rows[0][i].MyType == SupportedType.Color) ||
                                     (in_sheet.Rows[0][i].MyType == SupportedType.Color32))
                            {
                                fsmvarType = "FsmColor";
                            }
                            else if (in_sheet.Rows[0][i].MyType == SupportedType.Quaternion)
                            {
                                fsmvarType = "FsmQuaternion";
                            }

                            fileString += FormatLine("		[UIHint(UIHint.Variable)]");
                            fileString +=
                                FormatLine("		[Tooltip(\"Store the " + varName + " in a " + varType +
                                           " variable.\")]");

                            fileString += FormatLine("		public " + fsmvarType + " " + varName + ";");
                        }

                        fileString += FormatLine("		public override void Reset()");
                        fileString += FormatLine("		{");
                        fileString += FormatLine("			databaseObj = null;");
                        fileString += FormatLine("			rowName = null;");

                        for (int i = 1; i < in_sheet.Rows[0].Count; i++)
                        {
                            var varName = MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader);

                            if ((in_sheet.Rows[0][i].MyType == SupportedType.Void) ||
                                IsSupportedArrayType(in_sheet.Rows[0][i].MyType) ||
                                (in_options.ObjectDBCullColumns && i > in_sheet.FirstBlankCol))
                                continue;

                            var tmpVarName =
                                varName.Split(new[] {'_'}, System.StringSplitOptions.RemoveEmptyEntries)[0];

                            if (string.Compare(tmpVarName, "IGNORE", StringComparison.OrdinalIgnoreCase) == 0 ||
                                string.Compare(tmpVarName, "VOID", StringComparison.OrdinalIgnoreCase) == 0)
                                continue;

                            fileString += FormatLine("			" + varName + " = null;");
                        }

                        fileString += FormatLine("		}");

                        fileString += FormatLine("		public override void OnEnter()");
                        fileString += FormatLine("		{");
                        fileString +=
                            FormatLine(
                                "			if ( databaseObj != null && rowName != null && rowName.Value != System.String.Empty )");
                        fileString += FormatLine("			{");
                        fileString +=
                            FormatLine("				Google2u." + className +
                                       " db = databaseObj.Value.GetComponent<Google2u." +
                                       className + ">();");
                        fileString +=
                            FormatLine("				Google2u." + className +
                                       "Row row = db.GetRow( rowName.Value );");

                        for (int i = 1; i < in_sheet.Rows[0].Count; i++)
                        {
                            var varName = MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader);

                            if ((in_sheet.Rows[0][i].MyType == SupportedType.Void) ||
                                IsSupportedArrayType(in_sheet.Rows[0][i].MyType) ||
                                (in_options.ObjectDBCullColumns && i > in_sheet.FirstBlankCol))
                                continue;

                            var tmpVarName =
                                varName.Split(new[] {'_'}, System.StringSplitOptions.RemoveEmptyEntries)[0];


                            if (string.Compare(tmpVarName, "IGNORE", StringComparison.OrdinalIgnoreCase) == 0 ||
                                string.Compare(tmpVarName, "VOID", StringComparison.OrdinalIgnoreCase) == 0)
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
                    }
                }

                using (var fs = File.Open(Path.Combine(playmakerPath, "Get" + className + "DataByIndex.cs"), FileMode.Create, FileAccess.Write))
                {
                    using (var sw = new StreamWriter(fs))
                    {
                        var fileString = String.Empty;

                        /////////////////////////////
                        // Generate the Action for Get*DataByIndex
                        fileString = System.String.Empty;
                        fileString += FormatLine("using UnityEngine;");
                        fileString += FormatLine(System.String.Empty);

                        fileString += FormatLine("namespace HutongGames.PlayMaker.Actions");
                        fileString += FormatLine("{");
                        fileString += FormatLine("	[ActionCategory(\"Google2u\")]");
                        fileString +=
                            FormatLine("	[Tooltip(\"Gets the specified entry in the " + className +
                                       " Database By Index.\")]");
                        fileString +=
                            FormatLine("	public class Get" + className + "DataByIndex : FsmStateAction");
                        fileString += FormatLine("	{");
                        fileString += FormatLine("		[RequiredField]");
                        fileString += FormatLine("		[UIHint(UIHint.Variable)]");
                        fileString +=
                            FormatLine("		[Tooltip(\"The object that contains the " + className +
                                       " database.\")]");
                        fileString += FormatLine("		public FsmGameObject databaseObj;");

                        fileString += FormatLine("		[RequiredField]");
                        fileString += FormatLine("		[UIHint(UIHint.Variable)]");
                        fileString += FormatLine("		[Tooltip(\"Row index of the entry you wish to retrieve.\")]");
                        fileString += FormatLine("		public FsmInt rowIndex;");

                        fileString += FormatLine("		[UIHint(UIHint.Variable)]");
                        fileString += FormatLine("		[Tooltip(\"Row ID of the entry.\")]");
                        fileString += FormatLine("		public FsmString rowName;");

                        for (int i = 1; i < in_sheet.Rows[0].Count; i++)
                        {
                            var fsmvarType = System.String.Empty;
                            var varType = StringSupportedType(in_sheet.Rows[0][i].MyType);
                            var varName = MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader);

                            if ((in_sheet.Rows[0][i].MyType == SupportedType.Void) ||
                                IsSupportedArrayType(in_sheet.Rows[0][i].MyType) ||
                                (in_options.ObjectDBCullColumns && i > in_sheet.FirstBlankCol))
                            {
                                continue;
                            }

                            if (in_sheet.Rows[0][i].MyType == SupportedType.Float)
                            {
                                fsmvarType = "FsmFloat";
                            }
                            else if ((in_sheet.Rows[0][i].MyType == SupportedType.Byte) ||
                                     (in_sheet.Rows[0][i].MyType == SupportedType.Int))
                            {
                                fsmvarType = "FsmInt";
                            }
                            else if (in_sheet.Rows[0][i].MyType == SupportedType.Bool)
                            {
                                fsmvarType = "FsmBool";
                            }
                            else if (in_sheet.Rows[0][i].MyType == SupportedType.String)
                            {
                                fsmvarType = "FsmString";
                            }
                            else if (in_sheet.Rows[0][i].MyType == SupportedType.GameObject)
                            {
                                fsmvarType = "FsmGameObject";
                            }
                            else if (in_sheet.Rows[0][i].MyType == SupportedType.Vector2)
                            {
                                fsmvarType = "FsmVector2";
                            }
                            else if (in_sheet.Rows[0][i].MyType == SupportedType.Vector3)
                            {
                                fsmvarType = "FsmVector3";
                            }
                            else if ((in_sheet.Rows[0][i].MyType == SupportedType.Color) ||
                                     (in_sheet.Rows[0][i].MyType == SupportedType.Color32))
                            {
                                fsmvarType = "FsmColor";
                            }
                            else if (in_sheet.Rows[0][i].MyType == SupportedType.Quaternion)
                            {
                                fsmvarType = "FsmQuaternion";
                            }

                            fileString += FormatLine("		[UIHint(UIHint.Variable)]");
                            fileString +=
                                FormatLine("		[Tooltip(\"Store the " + varName + " in a " + varType +
                                           " variable.\")]");

                            fileString += FormatLine("		public " + fsmvarType + " " + varName + ";");
                        }

                        fileString += FormatLine("		public override void Reset()");
                        fileString += FormatLine("		{");
                        fileString += FormatLine("			databaseObj = null;");
                        fileString += FormatLine("			rowIndex = null;");

                        for (int i = 1; i < in_sheet.Rows[0].Count; i++)
                        {
                            var varName = MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader);

                            if ((in_sheet.Rows[0][i].MyType == SupportedType.Void) ||
                                IsSupportedArrayType(in_sheet.Rows[0][i].MyType) ||
                                (in_options.ObjectDBCullColumns && i > in_sheet.FirstBlankCol))
                                continue;

                            var tmpVarName =
                                varName.Split(new[] {'_'}, System.StringSplitOptions.RemoveEmptyEntries)[0];

                            if (string.Compare(tmpVarName, "IGNORE", StringComparison.OrdinalIgnoreCase) == 0 ||
                                string.Compare(tmpVarName, "VOID", StringComparison.OrdinalIgnoreCase) == 0)
                                continue;

                            fileString += FormatLine("			" + varName + " = null;");
                        }

                        fileString += FormatLine("		}");

                        fileString += FormatLine("		public override void OnEnter()");
                        fileString += FormatLine("		{");
                        fileString += FormatLine("			if ( databaseObj != null && rowIndex != null )");
                        fileString += FormatLine("			{");
                        fileString +=
                            FormatLine("				Google2u." + className +
                                       " db = databaseObj.Value.GetComponent<Google2u." +
                                       className + ">();");

                        fileString +=
                            FormatLine("				// For sanity sake, we are going to do an auto-wrap based on the input");
                        fileString += FormatLine("				// This should prevent accessing the array out of bounds");
                        fileString += FormatLine("				int i = rowIndex.Value;");
                        fileString += FormatLine("				int L = db.Rows.Count;");
                        fileString += FormatLine("				while ( i < 0 )");
                        fileString += FormatLine("					i += L;");
                        fileString += FormatLine("				while ( i > L-1 )");
                        fileString += FormatLine("					i -= L;");
                        fileString += FormatLine("				Google2u." + className + "Row row = db.Rows[i];");

                        fileString += FormatLine("				if ( rowName != null )");
                        fileString += FormatLine("					rowName.Value = db.rowNames[i];");

                        for (int i = 1; i < in_sheet.Rows[0].Count; i++)
                        {
                            var varName = MakeValidVariableName(in_sheet.Rows[0][i].CellValueString, in_options.LowercaseHeader);

                            if ((in_sheet.Rows[0][i].MyType == SupportedType.Void) ||
                                IsSupportedArrayType(in_sheet.Rows[0][i].MyType) ||
                                (in_options.ObjectDBCullColumns && i > in_sheet.FirstBlankCol))
                                continue;

                            var tmpVarName =
                                varName.Split(new[] {'_'}, System.StringSplitOptions.RemoveEmptyEntries)[0];

                            if (string.Compare(tmpVarName, "IGNORE", StringComparison.OrdinalIgnoreCase) == 0 ||
                                string.Compare(tmpVarName, "VOID", StringComparison.OrdinalIgnoreCase) == 0)
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
                    }
                }

                using (var fs = File.Open(Path.Combine(playmakerPath, "Get" + className + "Count.cs"), FileMode.Create, FileAccess.Write))
                {
                    using (var sw = new StreamWriter(fs))
                    {
                        /////////////////////////////
                        // Generate the Action for Get*DataByIndex
                        var fileString = String.Empty;


                        fileString = System.String.Empty;
                        fileString += FormatLine("using UnityEngine;");
                        fileString += FormatLine(System.String.Empty);

                        fileString += FormatLine("namespace HutongGames.PlayMaker.Actions");
                        fileString += FormatLine("{");
                        fileString += FormatLine("	[ActionCategory(\"Google2u\")]");
                        fileString +=
                            FormatLine("	[Tooltip(\"Gets the specified entry in the " + className +
                                       " Database By Index.\")]");
                        fileString += FormatLine("	public class Get" + className + "Count : FsmStateAction");
                        fileString += FormatLine("	{");
                        fileString += FormatLine("		[RequiredField]");
                        fileString += FormatLine("		[UIHint(UIHint.Variable)]");
                        fileString +=
                            FormatLine("		[Tooltip(\"The object that contains the " + className +
                                       " database.\")]");
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
                        fileString +=
                            FormatLine("				Google2u." + className +
                                       " db = databaseObj.Value.GetComponent<Google2u." +
                                       className + ">();");
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
            } // /found playmaker

            PushNotification("Saving to: " + in_editorPath);
        } // ExportObjectDb
    }
}
