using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cisApp.Core;

namespace cisApp.Models
{
    public class UserModel : Users
    { 
        public UserDesigner designer { get; set; }
        public UsersPassword password { get; set; }
        public string UserTypeDesc { get; set; }
        public string ProvinceDesc { get; set; }
        public string DistrictDesc { get; set; }
        public string SubDistrictDesc { get; set; }
        public string BankName { get; set; }
        public string AccountTypeDesc { get; set; }
    }
}
