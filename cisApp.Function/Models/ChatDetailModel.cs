using cisApp.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace cisApp.Function
{
    public class ChatDetailModel
    {
        public Guid? RecieverId { get; set; }
        public bool IsGroup { get; set; }
        public string ChatName { get; set; }
        public List<ChatProfileModel> chatProfiles { get; set; }
        public List<ChatMessageModel> chatMessages { get; set; }
        public Guid? SenderId { get; set; }

    }

    public class ChatProfileModel
    {
        public string Name { get; set; }
        public string ImgUrl { get; set; }
    }
}
