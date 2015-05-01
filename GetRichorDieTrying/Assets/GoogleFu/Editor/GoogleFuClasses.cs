//----------------------------------------------
//    GoogleFu: Google Doc Unity integration
//         Copyright © 2013 Litteratus
//----------------------------------------------

using System;
using System.Linq;
using UnityEngine;

namespace GoogleFu
{
    public partial class GoogleFuEditor
    {
        private enum GfPage
        {
            Settings,
            Workbooks,
            Toolbox,
            Help
        }

        [Serializable]
        public class WorkBookInfo
        {
            public Google.GData.Spreadsheets.SpreadsheetEntry SpreadsheetEntry = null;
            public Google.GData.Client.AtomEntryCollection WorksheetEntries = null;
            public System.Collections.Generic.List<Google.GData.Spreadsheets.WorksheetEntry> ManualEntries = new System.Collections.Generic.List<Google.GData.Spreadsheets.WorksheetEntry>();
            public string Url = "";
            public string Title = "";

            public WorkBookInfo()
            {
            }

            public WorkBookInfo(Google.GData.Spreadsheets.SpreadsheetEntry in_entry)
            {
                SetupFromSpreadsheetEntry(in_entry);
            }

            public void SetupFromSpreadsheetEntry(Google.GData.Spreadsheets.SpreadsheetEntry in_entry)
            {
                SpreadsheetEntry = in_entry;
                WorksheetEntries = SpreadsheetEntry.Worksheets.Entries;
                Title = in_entry.Title.Text;
                foreach (var link in in_entry.Links)
                {
                    if (link.Rel.Equals("alternate", StringComparison.OrdinalIgnoreCase))
                    {
                        Url = link.HRef.ToString();
                        break;
                    }
                }
            }

            public void AddWorksheetEntry(Google.GData.Spreadsheets.WorksheetEntry in_entry, string in_url)
            {
                if (SpreadsheetEntry == null)
                    SpreadsheetEntry = new Google.GData.Spreadsheets.SpreadsheetEntry();
                Google.GData.Client.AtomEntry.ImportFromFeed(in_entry);
                Title = in_entry.Feed.Title.Text;
                Url = in_url;
                ManualEntries.Add(in_entry);
            }

            public Google.GData.Spreadsheets.SpreadsheetFeed GetSpreadsheetFeed()
            {
                return SpreadsheetEntry.Feed as Google.GData.Spreadsheets.SpreadsheetFeed;
            }

            public override string ToString()
            {
                return string.Format(Url + "." + Title);
            }
        }
        

        [Serializable]
        public class AdvancedDatabaseInfo
        {
            public GameObject DatabaseAttachObject;
            public string ComponentName;
            public Google.GData.Spreadsheets.WorksheetEntry Entry;
            public System.Collections.Generic.List<string> EntryStrings = new System.Collections.Generic.List<string>();
            public int EntryStride = 0;
            public bool GeneratePlaymaker;
            public AdvancedDatabaseInfo(string in_componentName, Google.GData.Spreadsheets.WorksheetEntry in_entry, Google.GData.Spreadsheets.SpreadsheetsService in_service, GameObject in_databaseAttachObject, bool in_bGeneratePlaymaker, bool in_bFirstRowValueTypes)
            {
                ParseWorksheetEntry(in_entry, in_service, in_bFirstRowValueTypes);
                ComponentName = in_componentName;
                if (in_databaseAttachObject != null)
                    DatabaseAttachObject = in_databaseAttachObject;
                else
                {
                    DatabaseAttachObject = GameObject.Find("databaseObj") ?? new GameObject("databaseObj");
                }

                GeneratePlaymaker = in_bGeneratePlaymaker;
            }

            private void ParseWorksheetEntry(Google.GData.Spreadsheets.WorksheetEntry in_entry, Google.GData.Spreadsheets.SpreadsheetsService in_service, bool in_bFirstRowValueTypes)
            {
                if (in_entry == null)
                {
                    Debug.LogError("Could not read WorksheetEntry - retry count:  ");
                    return;
                }

                // Define the URL to request the list feed of the worksheet.
                Google.GData.Client.AtomLink listFeedLink = in_entry.Links.FindService(Google.GData.Spreadsheets.GDataSpreadsheetsNameTable.ListRel, null);

                // Fetch the list feed of the worksheet.
                var listQuery = new Google.GData.Spreadsheets.ListQuery(listFeedLink.HRef.ToString());
                Google.GData.Spreadsheets.ListFeed listFeed = in_service.Query(listQuery);

                //int rowCt = listFeed.Entries.Count;
                //int colCt = ((ListEntry)listFeed.Entries[0]).Elements.Count;

                if (listFeed.Entries.Count > 0)
                {

                    int curRow = 0;
                    // Iterate through each row, printing its cell values.
                    foreach (var atomEntry in listFeed.Entries)
                    {
                        var row = (Google.GData.Spreadsheets.ListEntry)atomEntry;

                        // skip the first row if this is a value type row
                        if (curRow == 0 && in_bFirstRowValueTypes)
                        {
                            curRow++;
                            continue;
                        }

                        if (row.Title.Text.Equals("VOID", StringComparison.OrdinalIgnoreCase))
                        {
                            curRow++;
                            continue;
                        }

                        int curCol = 0;
                        // Iterate over the remaining columns, and print each cell value
                        foreach (Google.GData.Spreadsheets.ListEntry.Custom element in row.Elements)
                        {
                            // this will be the list of all the values in the row excluding the first 'name' column
                            if (curCol > 0)
                                EntryStrings.Add(element.Value);
                            curCol++;
                        }
                        EntryStride = curCol - 1;

                        curRow++;
                    }
                }
            }
        }
    }
}
