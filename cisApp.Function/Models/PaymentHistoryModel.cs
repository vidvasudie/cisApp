using cisApp.Core;
using cisApp.library;
using System;
using System.Collections.Generic;
using System.Text;

namespace cisApp.Function
{
    public class PaymentHistoryModel : PaymentHistory
    {
        public string FullName { get; set; }
        public string PaymentDateStr
        {
            get
            {
                return PaymentDate.ToStringFormat();
            }
            set
            {
                PaymentDate = value.ToDateTimeFormat();
            }
        }
    }
}
