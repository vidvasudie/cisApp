using cisApp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cisApp.Function
{
    public class GetJobCadidateLock
    {
        public class Get
        {

            public static List<JobCadidateLock> GetByJobId(Guid id)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.JobCadidateLock.Where(o => o.JobId == id).ToList();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    //throw ex;
                    return null;
                }
            }

            public static JobCadidateLock GetLockByCaId(int id)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = (from ca in context.JobsCandidate.Where(o => o.JobCaId == id)
                                   join lck in context.JobCadidateLock on ca.JobId equals lck.JobId
                                   where DateTime.Compare(DateTime.Now, lck.ExpireDate) < 0
                                   select lck).FirstOrDefault();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    //throw ex;
                    return null;
                }
            }

            public static JobCadidateLock GetLockByJobId(Guid id)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.JobCadidateLock.Where(o => o.JobId == id && DateTime.Compare(DateTime.Now, o.ExpireDate) < 0).FirstOrDefault(); 

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    //throw ex;
                    return null;
                }
            }

        }

        public class Manage
        {
            public static JobCadidateLock Add(JobCadidateLock obj)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            var joblock = context.JobCadidateLock.Where(o => o.JobId == obj.JobId).ToList();
                            if(joblock != null && joblock.Count > 0)
                            {
                                //clear all before job lock to IsActive = 0 if exist
                                foreach (var jl in joblock)
                                {
                                    jl.IsActive = false;
                                }

                                context.JobCadidateLock.UpdateRange(joblock);
                                context.SaveChanges();
                            }

                            // add new job candidate lock into table
                            context.JobCadidateLock.Add(obj);
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
