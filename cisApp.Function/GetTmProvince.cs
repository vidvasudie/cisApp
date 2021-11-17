using System;
using System.Collections.Generic;
using System.Linq;
using cisApp.Core;

namespace cisApp.Function
{
    public static class GetTmProvince
    {
        public class Get
        {
            public static List<TmProvince> GetAll()
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.TmProvince.ToList();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static TmProvince GetById(int id)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.TmProvince.Where(o => o.Id == id).FirstOrDefault();

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
