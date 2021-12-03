using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using cisApp.Core;

namespace cisApp.Function
{
    public static class GetTmVatratio
    {
        public class Get
        {
            public static TmVatratio GetFirst()
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.TmVatratio.FirstOrDefault();

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
