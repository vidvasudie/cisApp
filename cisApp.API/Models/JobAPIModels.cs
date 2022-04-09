using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace cisApp.API.Models
{
    public class JobAPIModels
    {
        [Required]
        public Guid UserId { get; set; }
        public bool IsDraft { get; set; } = false;
        public int JobTypeId { get; set; }
        public string Ip { get; set; }
        public string JobDescription { get; set; }
        /// <summary>
        /// ขนาดพื้นที่ 
        /// </summary>
        public decimal JobAreaSize { get; set; }
        
        /// <summary>
        /// ราคา/ตรม
        /// </summary>
        public decimal JobPricePerSqM { get; set; } = 250;
        /// <summary>
        /// ราคารวมค่างาน
        /// </summary>
        public decimal JobPrice { get; set; }
        /// <summary>
        /// ราคารวมค่าบริการ
        /// </summary>
        public decimal? JobPriceProceed { get; set; }
        /// <summary>
        /// ราคารวมค่าบริการและภาษี
        /// </summary>
        public decimal? JobFinalPrice { get; set; }
        /// <summary>
        /// %ค่าดำเนินการในใบงาน
        /// </summary>
        public int? JobProceedRatio { get; set; } = 5;
        /// <summary>
        /// %VAT ในใบงาน
        /// </summary>
        public int? JobVatRatio { get; set; } = 7;
        public bool IsInvRequired { get; set; } = false;
        public string Invname { get; set; }
        public string InvAddress { get; set; }
        public string InvPersonalId { get; set; }
        public bool? IsAdvice { get; set; }
        public List<FileListModel> FileList { get; set; }
    }
    public class FileListModel
    {
        public string FileId { get; set; }
        public string FileName { get; set; }
        public int Type { get; set; }
    }
}
