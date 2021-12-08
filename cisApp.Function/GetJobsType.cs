using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using cisApp.Core;
using cisApp.library;

namespace cisApp.Function
{
    public static class GetJobsType
    {
        public class Get
        {
            public static List<JobsType> GetAll()
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.JobsType.ToList();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public static List<JobsType> GetActive()
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.JobsType.Where(o => o.IsActive == true && o.IsDeleted == false).ToList();

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
