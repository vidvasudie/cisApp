using System;
using System.Collections.Generic;
using System.Text;
using cisApp.Common;
namespace cisApp
{
    public class resultJson
    {
        public static  common success(string Message , string MessageEN , Object Data, int? rows = null, int? Totalsrows = null, int? page= null, int? nextpage = null)
        {
            return new common() { success =true, Message =  Message , MessageEN = MessageEN, Data =  Data , page = page, nextpage = nextpage, rows = rows, Totalsrows = Totalsrows };
        }
        public static common errors(string Message, string MessageEN, Object Data )
        {
            return new common() { success = false, Message = Message, MessageEN = Message, Data = Data  }; 
        }

     }
}
