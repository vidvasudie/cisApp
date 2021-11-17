using System;
using System.Collections.Generic;
using System.Linq;
using cisApp.Core;

namespace cisApp.Function
{
    public static class GetTmDistrict
    {
        public class Get
        {
            public static List<TmDistrict> GetAll()
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.TmDistrict.ToList();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static TmDistrict GetById(int id)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.TmDistrict.Where(o => o.Id == id).FirstOrDefault();

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
