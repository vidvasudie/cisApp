using System;
using System.Collections.Generic;
using System.Text;
using cisApp.Core;

namespace cisApp.Function
{
    public class ChatMessageModel : ChatMessage
    {
        public string SenderName { get; set; }

        public AttachFileAPIModel Profile { get; set; }
        
        public List<AttachFileAPIModel> Files { get; set; }

    }
}
