using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cisApp.API.Models
{
    public class LoginModels
    {
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
        public string ClientId { get; set; }
    }
    public class LoginResult
    {
        public Guid uSID { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public bool isDesigner { get; set; }


       
    }



}
