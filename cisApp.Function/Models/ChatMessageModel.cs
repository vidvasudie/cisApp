﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using cisApp.Core;
using Newtonsoft.Json;

namespace cisApp.Function
{
    public class ChatMessageModel : ChatMessage
    {
        public string SenderName { get; set; }

        public string CreatedDateStr { 
            get
            {
                try
                {
                    CultureInfo cultureInfo = new CultureInfo("th-TH");
                    return (CreatedDate != null) ? CreatedDate.ToString("dd/MM/yyyy HH:mm", cultureInfo) : "";
                }
                catch (Exception ex)
                {
                    return "";
                }
            } 
        }

        public AttachFileAPIModel Profile { get; set; }
        
        public List<AttachFileAPIModel> Files { get; set; }

        public List<AttachFile> AttachFiles { get; set; }

        public string ChatGroupReadJson { get; set; }

        public List<ChatGroupRead> ChatGroupReads
        {
            get
            {
                try
                {
                    return JsonConvert.DeserializeObject<List<ChatGroupRead>>(ChatGroupReadJson);
                }
                catch (Exception ex)
                {
                    return new List<ChatGroupRead>();
                }
            }
        }

        public class ChatGroupRead
        {
            public Guid RecieverId { get; set; }
        }

    }
}
