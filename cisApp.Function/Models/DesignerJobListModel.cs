using cisApp.library;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace cisApp.Function
{
    public class DesignerJobListModel
    {
        public IConfigurationRoot _config = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("appsettings.json")
                                    .Build();
        public bool IsCusFavorite { get; set; }
        public Guid JobID { get; set; }
        public string JobNo { get; set; } 
        public string JobTypeName { get; set; }  
        public string JobDescription { get; set; } 
        public decimal JobAreaSize { get; set; } 
        public decimal JobPrice { get; set; }
        public decimal JobPriceProceed { get; set; }
        public decimal JobFinalPrice { get; set; }
        public decimal JobPricePerSqM { get; set; } 
        public int JobStatus { get; set; }
        public bool IsInvRequired { get; set; }
        public string InvAddress { get; set; }
        public string InvPersonalID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedDateStr
        {
            get
            {
                return CreatedDate.ToStringFormat();
            }
            set
            {
                CreatedDate = value.ToDateTimeFormat();
            }
        }
        public Guid UserID { get; set; }
        public string Fullname { get; set; }
        public Guid AttachFileID { get; set; }
        public string FileName { get; set; }
        public string UrlPathUserImage
        {
            get
            {
                if (AttachFileID == Guid.Empty)
                    return null;
                string Host = _config.GetSection("WebConfig:AdminWebStie").Value;
                bool removeLast = Host.Last() == '/';
                string UrlPath = "~/Uploads" + "/" + this.AttachFileID + "/" + this.FileName;
                if (removeLast)
                {
                    Host = Host.Remove(Host.Length - 1);
                }
                UrlPath = UrlPath.Replace("~", Host);
                return UrlPath;
            }
        }
        public decimal RecruitedPrice { get; set; }
        public decimal ContestPrice { get; set; }
        public List<JobCandidateModel> jobCandidates { get; set; }
        public List<JobsExamImageModel> jobsExamImages { get; set; }
    }
}
