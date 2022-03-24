using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using cisApp.Core;
using cisApp.library;
using Microsoft.Extensions.Configuration;

namespace cisApp.Function
{
    public class JobCandidateModel : JobsCandidate
    {
        public IConfigurationRoot _config = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("appsettings.json")
                                    .Build();
        public string UserFullName { get; set; }
        public int UserLikes { get; set; }
        public decimal PriceRate { get; set; }
        public decimal UserRate { get; set; }
        public string Ip { get; set; }
        public int UserMessageCount { get; set; }
        public string CaStatusDesc { get; set; }
        public string PayStatusDesc { get; set; }
        public bool IsCaSubmitWork { get; set; }
        public int RegisterDays
        {
            get
            {
                return DateTime.Now.Subtract(CreatedDate.Value).Days; //return days pass after register
            } 
        }
        public string CreatedDateStr
        {
            get
            {
                return CreatedDate.ToStringFormat(DateTimeUtil.DateTimeFormat.FULL);
            }
        }
        public int UserRegAccept { get; set; }
        public int UserRegTotal { get; set; }
        public Guid AttachFileId { get; set; }
        public string AttachFileName { get; set; }
        public string UrlPath
        {
            get
            {
                if (this.AttachFileId == Guid.Empty)
                {
                    return null;
                }
                else
                {
                    return "~/Uploads" + "/" + this.AttachFileId + "/" + this.AttachFileName;
                }
            }
        }
        public string UrlPathAPI
        {
            get
            {
                if(this.AttachFileId == Guid.Empty)
                {
                    return null;
                }
                else
                {
                    string Host = _config.GetSection("WebConfig:AdminWebStie").Value;
                    bool removeLast = Host.Last() == '/';
                    string UrlPath = "~/Uploads" + "/" + this.AttachFileId + "/" + this.AttachFileName;
                    if (removeLast)
                    {
                        Host = Host.Remove(Host.Length - 1);
                    }
                    UrlPath = UrlPath.Replace("~", Host);
                    return UrlPath;
                }
                
            }
        }
        [NotMapped]
        public List<JobCandidateModel> userCandidates { get; set; }
    }
}
