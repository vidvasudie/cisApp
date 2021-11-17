using System;
using System.Collections.Generic;
using System.Linq;
using cisApp.Core;

namespace cisApp.Function
{
    public static class GetTmBankAccountType
    {
        public class Get
        {
            public static List<TmBankAccountType> GetAll()
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.TmBankAccountType.ToList();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static TmBankAccountType GetById(int id)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.TmBankAccountType.Where(o => o.Id == id).FirstOrDefault();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
