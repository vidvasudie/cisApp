using System;
using System.Collections.Generic;
using System.Linq;
using cisApp.Core;

namespace cisApp.Function
{
    public static class GetUserDesigner
    {
        public class Get
        {
            public static List<UserDesigner> GetAll()
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.UserDesigner.ToList();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public static UserDesigner GetById(Guid id)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.UserDesigner.Where(o => o.UserDesignerId == id).FirstOrDefault();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public static UserDesigner GetByUserId(Guid userid)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.UserDesigner.Where(o => o.UserId == userid).FirstOrDefault();

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
