using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cisApp.Core;

namespace cisApp.Models
{
    public class UserModel
    {
        Users user { get; set; }
        UserDesigner designer { get; set; }
        UsersPassword password { get; set; }
    }
}
