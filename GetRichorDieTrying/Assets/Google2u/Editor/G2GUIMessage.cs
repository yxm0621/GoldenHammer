//----------------------------------------------
//    Google2u: Google Doc Unity integration
//         Copyright © 2015 Litteratus
//----------------------------------------------

using System;

namespace Google2u
{
    [Serializable]
    public class G2GUIMessage
    {


        public GFGUIMessageType MessageType;
        public string Message;

        public G2GUIMessage(GFGUIMessageType in_type, string in_message)
        {
            MessageType = in_type;
            Message = in_message;
        }
    }
}