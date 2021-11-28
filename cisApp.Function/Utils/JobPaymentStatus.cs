using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace cisApp.Function
{
    public static class JobPaymentStatus
    {
        public static List<KeyValueModel> GetAll()
        {
            return new List<KeyValueModel>()
            {
                new KeyValueModel() {key = 1, value="รอชำระเงิน"},
                new KeyValueModel() {key = 2, value="อยู่ระหว่างตรวจสอบ"},
                new KeyValueModel() {key = 3, value="สำเร็จ"},
                new KeyValueModel() {key = 4, value="ไม่อนุมัติ/คืนเงิน"},
            };
        }

        public static string GetLabel(int key)
        {
            try
            {
                return GetAll().Where(o => o.key == key).FirstOrDefault().value;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
