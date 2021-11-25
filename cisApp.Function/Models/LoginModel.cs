using System;
using System.Collections.Generic;
using System.Text;

namespace cisApp.Function
{
    public class LoginModel
    {
        public string username { get; set; }
        public string password { get; set; }
        public int? userType { get; set; } = 3;
    }
}
