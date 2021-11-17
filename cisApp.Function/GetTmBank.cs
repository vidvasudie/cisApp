using System;
using System.Collections.Generic;
using System.Linq;
using cisApp.Core;

namespace cisApp.Function
{
    public static class GetTmBank
    {
        public class Get
        {
            public static List<TmBank> GetAll()
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.TmBank.ToList();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public static TmBank GetById(int id)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.TmBank.Where(o => o.Id == id).FirstOrDefault();

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
