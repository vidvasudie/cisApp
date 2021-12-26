using System;
using System.Collections.Generic;
using System.Linq;
using cisApp.Core;

namespace cisApp.Function
{
    public static class GetTmCauseCancel
    {
        public class Get
        {
            public static List<TmCauseCancel> GetAll()
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.TmCauseCancel.ToList();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public static List<TmCauseCancel> GetByActive()
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.TmCauseCancel.Where(o => o.IsActive == true);
                        if(data.Any())
                            return data.ToList();
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public static TmCauseCancel GetById(int id)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.TmCauseCancel.Where(o => o.Id == id).FirstOrDefault();

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
