using cisApp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cisApp.Models
{
    public class DashboardModel
    {
        public List<UserHelp> UserHelps { get; set; }
        public int Total { get; set; }
        public int PaymentCount { get; set; }
        public int DesignerRequestCount { get; set; }
        public int DailyAccess { get; set; }
        public int WeeklyAccess { get; set; }
        public int MonthlyAccess { get; set; }
    }
}
