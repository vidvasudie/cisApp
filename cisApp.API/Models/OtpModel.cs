using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cisApp.API
{
    public class OtpModel
    {
        public Guid? ActivityID { get; set; } // รหัสกิจกรรม
        public string msgType { get; set; }// text
        public bool resend { get; set; } 
        public string mobile { get; set; }// เบอร์โทร
        public string mobileCode { get; set; } // TH or EN
        public string bizScene { get; set; }// หน้าจอ
        public bool isNewMobile { get; set; }  // เบอร์ใหม่

        // Exam {"msgType":"TEXT","resend":false,"mobile":"084
    }

}
