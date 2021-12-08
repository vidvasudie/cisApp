using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace cisApp.API.Models
{
    public class JobAPIModels
    {
        public bool IsDraft { get; set; } = false;
        public int JobTypeId { get; set; }

        public string JobDescription { get; set; }
        /// <summary>
        /// ขนาดพื้นที่ 
        /// </summary>
        public decimal JobAreaSize { get; set; }
        /// <summary>
        /// ราคารวมค่างาน
        /// </summary>
        public decimal JobPrice { get; set; }
        /// <summary>
        /// ราคา/ตรม
        /// </summary>
        public decimal JobPricePerSqM { get; set; } 
    }
}
