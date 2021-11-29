using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using cisApp.Core;
using cisApp.library;

namespace cisApp.Function
{
    public static class GetJobsExamType
    {
        public class Get
        {
            public static List<JobsExamType> GetAll()
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.JobsExamType.ToList();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            //public static List<JobsExamType> GetActive()
            //{
            //    try
            //    {
            //        using (var context = new CAppContext())
            //        {
            //            var data = context.JobsExamType.Where(o => o.is == id).ToList();

            //            return data;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //} 

        }

        public class Manage
        {
            


        }

    }
}
