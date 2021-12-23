using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cisApp
{
    public class ProfileBankModel
    {
        public int BankId { get; set; }
        public string BankName { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public int AccountTypeId { get; set; }
        public string AccountTypeName { get; set; }

    }
}
