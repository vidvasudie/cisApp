using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using cisApp.Core;
using cisApp.library;

namespace cisApp.Function
{
    public static class GetJobsLogs
    { 
        public class Get
        {
            public static List<JobsLogs> GetAll()
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.JobsLogs.ToList();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public static List<JobsLogs> GetByJobId(Guid jobId)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.JobsLogs.Where(o => o.JobId == jobId).ToList();

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
