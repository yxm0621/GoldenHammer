//----------------------------------------------
//    Google2u: Google Doc Unity integration
//         Copyright © 2015 Litteratus
//----------------------------------------------

using UnityEditor;

namespace Google2u
{
    public class Google2uEditor : EditorWindow
    {
        public WorkbookBase Workbook { get; set; }
        public EditorGUILayoutEx Layout { get; set; }

        public string WorkbookName
        {
            get
            {
                return Workbook == null ? string.Empty : Workbook.WorkbookName;
            }
        }

        public void OnGUI()
        {
            if (Workbook != null)
            {
                Workbook.DrawGUIFull(Layout);
                Repaint();
            }
        }
    }
}
