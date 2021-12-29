using System;
using System.Collections.Generic;
using System.Text;
using cisApp.Core;

namespace cisApp.Function
{
    public class ChatListModel : ChatMessage
    {
        public string ChatName { get; set; }
        
        public int UnRead { get; set; }
        public Guid UserId { get; set; }

        public List<AttachFileAPIModel> Profiles { get; set; }

    }
}
