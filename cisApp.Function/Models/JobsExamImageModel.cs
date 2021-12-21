using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using cisApp.Core;
using Microsoft.Extensions.Configuration;

namespace cisApp.Function
{
    public class JobsExamImageModel : AttachFile
    {
        public IConfigurationRoot _config = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("appsettings.json")
                                    .Build();

        public int? JobsExTypeId { get; set; }
        public string JobsExTypeDesc { get; set; }
        public string UrlPathAPI
        {
            get
            {
                string Host = _config.GetSection("WebConfig:AdminWebStie").Value;
                bool removeLast = Host.Last() == '/';
                string UrlPath = "~/Uploads" + "/" + this.AttachFileId + "/" + this.FileName;
                if (removeLast)
                {
                    Host = Host.Remove(Host.Length - 1);
                }
                UrlPath = UrlPath.Replace("~", Host);
                return UrlPath;
            }
        }
    }
}
