using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using cisApp.Core;
using cisApp.library;

namespace cisApp.Function
{
    public static class GetJobDesignerReview
    {
        public class Get
        {
            public static List<JobDesignerReview> GetAll()
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.JobDesignerReview.ToList();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static List<JobDesignerReview> GetByUserDesignerId(Guid caUserId)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.JobDesignerReview.Where(o => o.DesignerUserId == caUserId).ToList();

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
            public static JobDesignerReview Add(JobDesignerReview obj)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            // only add content 
                            obj.Id = null;
                            obj.CreatedBy = obj.UserId;
                            obj.CreatedDate = DateTime.Now;
                            
                            context.JobDesignerReview.Add(obj);

                            context.SaveChanges();

                            dbContextTransaction.Commit();

                            return obj;
                        } 
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
