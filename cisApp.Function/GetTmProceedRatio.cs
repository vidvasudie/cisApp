using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using cisApp.Core;

namespace cisApp.Function
{
    public static class GetTmProceedRatio
    {
        public class Get
        {
            public static TmProceedRatio GetFirst()
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.TmProceedRatio.FirstOrDefault();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            } 

        }

        public class Manage
        {
            

        }

    }
}
