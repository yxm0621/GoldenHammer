//----------------------------------------------
//    Google2u: Google Doc Unity integration
//         Copyright © 2015 Litteratus
//----------------------------------------------

using UnityEngine;
using UnityEditor;

namespace Google2u
{

    [InitializeOnLoad]
    public class Google2uHierarchyIcon
    {
        private static Texture Google2uIcon;

        static Google2uHierarchyIcon()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HierarchWindowOnGUI;

            Google2uIcon = (Texture2D)Resources.Load("Google2uSm", typeof(Texture2D));
        }

        private static void HierarchWindowOnGUI(int in_instanceID, Rect in_selectionRect)
        {

            var r = new Rect(in_selectionRect);
            r.x = r.width - 10;
            r.width = 18;

            var g = (GameObject) EditorUtility.InstanceIDToObject(in_instanceID);

            if (g != null && g.GetComponent<Google2uComponentBase>() != null)
            {
                GUI.Label(r, Google2uIcon);
            }
        }
    }
}
