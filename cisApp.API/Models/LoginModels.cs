using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cisApp.API.Models
{
    public class LoginModels
    {
        public string email { get; set; }
        public string password { get; set; }
    }
    public class LoginResult
    {
        public Guid uSID { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public bool isDesigner { get; set; }


       
    }



}
