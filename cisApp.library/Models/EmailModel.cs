using System;
using System.Collections.Generic;
using System.Text;

namespace cisApp.library
{
    public class EmailModel
    {
        public string ToMail { get; set; }
        public string Subject { get; set; }
        public string TemplateFileName { get; set; }
        public List<string> Body { get; set; }
    }
}
