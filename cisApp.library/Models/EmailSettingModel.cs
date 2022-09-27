using System;
using System.Collections.Generic;
using System.Text;

namespace cisApp.library
{
    public class EmailSettingModel
    {
        public string Host { get; set; }
        public string FromEmail { get; set; }
        public string Password { get; set; }
        public string Port { get; set; }
        public string EnableSsl { get; set; }
    }
}
