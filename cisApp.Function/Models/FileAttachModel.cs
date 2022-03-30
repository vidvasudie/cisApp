using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using cisApp.Core;
using Microsoft.Extensions.Configuration;

namespace cisApp.Function
{ 
    public class FileAttachModel : AttachFile
    {
        public IConfigurationRoot _config = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("appsettings.json")
                                    .Build();
        public Guid gId { get; set; }
        public int Id { get; set; }
        public int TypeId { get; set; }
        public string Description { get; set; }
        public bool IsView { get; set; } = false;
        public string FileBase64 { get; set; }
        public int NextImg { get; set; }
        public int NextImgSelected { get; set; }
        public int col { get; set; }
        public string AdminUrlBase 
        { 
            get { return _config.GetSection("WebConfig:AdminWebStie").Value; } 
        }
        public string UrlPath2
        {
            get; set;
        }
    }
}
