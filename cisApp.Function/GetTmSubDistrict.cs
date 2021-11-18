using System;
using System.Collections.Generic;
using System.Linq;
using cisApp.Core;

namespace cisApp.Function
{
    public static class GetTmSubDistrict
    {
        public class Get
        {
            public static List<TmSubdistrict> GetAll()
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.TmSubdistrict.ToList();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public static List<TmSubdistrict> GetByActive()
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.TmSubdistrict.Where(o => o.IsActive == true).ToList();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public static TmSubdistrict GetById(int id)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.TmSubdistrict.Where(o => o.Id == id).FirstOrDefault();

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
