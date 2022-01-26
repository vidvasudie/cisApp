using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using cisApp.Core;
using cisApp.library;

namespace cisApp.Function
{
    public static class GetJobsCandidate
    {
        public class Get
        {
            public static List<JobsCandidate> GetAll()
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.JobsCandidate.ToList();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public static List<JobCandidateModel> GetByJobId(SearchModel model)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] { 
                       new SqlParameter("@jobId", model.gId != null && model.gId != Guid.Empty ? model?.gId.ToString() : (object)DBNull.Value), 
                       new SqlParameter("@status", !String.IsNullOrEmpty(model.statusStr) ? model.statusStr : (object)DBNull.Value),
                       new SqlParameter("@statusOpt", String.IsNullOrEmpty(model.statusOpt) ? "equal" : model.statusOpt)//equal, more, less, in
                    };

                    return StoreProcedure.GetAllStored<JobCandidateModel>("GetJobsCandidate", parameter);
                }
                catch (Exception ex)
                {
                    return new List<JobCandidateModel>();
                }
            }

            public static List<JobCandidateModel> GetUserCandidateModels(SearchModel model)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@jobId", model.gId != null && model.gId != Guid.Empty ? model?.gId.ToString() : (object)DBNull.Value) 
                    };

                    return StoreProcedure.GetAllStored<JobCandidateModel>("GetUserCandidateModels", parameter);
                }
                catch (Exception ex)
                {
                    return new List<JobCandidateModel>();
                }
            }
            public static List<JobCandidateModel> GetDesignerJobSubmitList(SearchModel model)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@jobId", model.gId != null && model.gId != Guid.Empty ? model?.gId.ToString() : (object)DBNull.Value)
                    };

                    return StoreProcedure.GetAllStored<JobCandidateModel>("GetDesignerJobSubmitList", parameter);
                }
                catch (Exception ex)
                {
                    return new List<JobCandidateModel>();
                }
            }


        }

        public class Manage
        {
            public static JobsCandidate Add(Guid jobId, Guid userId, string ip)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            int dbCount = context.JobsCandidate.Where(o => o.JobId == jobId && o.UserId == userId).Count();
                            if(dbCount >= 2)
                            {
                                return null;
                            }
                            JobsCandidate obj = new JobsCandidate();
                            obj.JobId = jobId;
                            obj.UserId = userId;
                            obj.CaStatusId = 1;//รอประกวด

                            obj.UpdatedDate = DateTime.Now;
                            obj.UpdatedBy = userId;
                            obj.CreatedDate = DateTime.Now;
                            obj.CreatedBy = userId;

                            context.JobsCandidate.Add(obj);

                            context.SaveChanges();

                            //add job log for every job activity 
                            JobsLogs log = new JobsLogs();
                            log.Description = ActionCommon.JobUpdate;
                            log.JobId = jobId;
                            log.Ipaddress = ip;
                            log.CreatedDate = DateTime.Now;
                            context.JobsLogs.Add(log);
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
            public static int UpdateNewCandidate(List<JobCandidateModel> list, Guid _user, string ip)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            JobsLogs log = new JobsLogs();
                            if (list != null && list.Count > 0)
                            {
                                foreach (var item in list)
                                {
                                    log.JobId = item.JobId.Value;
                                    JobsCandidate obj = new JobsCandidate();
                                    obj.CaStatusId = 1;//รอประกวด
                                    obj.JobId = item.JobId;
                                    obj.UserId = item.UserId;
                                    obj.CreatedDate = DateTime.Now;
                                    obj.CreatedBy = _user;

                                    context.JobsCandidate.Update(obj); 
                                }
                            }

                            int result = context.SaveChanges();

                            //add job log for every job activity  
                            log.Description = ActionCommon.JobCandidateAdd;  
                            log.Ipaddress = ip;
                            log.CreatedDate = DateTime.Now;
                            context.JobsLogs.Add(log);
                            context.SaveChanges();

                            dbContextTransaction.Commit();

                            return result;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static JobsCandidate Delete(int id, Guid userId, string ip)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            JobsCandidate obj = context.JobsCandidate.Find(id);

                            obj.IsDeleted = true;

                            obj.DeletedDate = DateTime.Now;
                            obj.DeletedBy = userId;

                            context.JobsCandidate.Update(obj);

                            context.SaveChanges();

                            //add job log for every job activity  
                            JobsLogs log = new JobsLogs();
                            log.JobId = obj.JobId.Value;
                            log.Description = ActionCommon.JobCandidateAdd;
                            log.Ipaddress = ip;
                            log.CreatedDate = DateTime.Now;
                            context.JobsLogs.Add(log);
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

            public static JobsCandidate Reject(Guid jobId, Guid userId, Guid caUserId, string ip)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            var objs = context.JobsCandidate.Where(o => o.JobId == jobId && o.UserId == caUserId);
                            if(objs == null || objs.Count() == 0)
                            {
                                return null;
                            }
                            JobsCandidate obj = objs.FirstOrDefault();

                            obj.CaStatusId = 5;//ปฎิเสธ

                            obj.DeletedDate = DateTime.Now;
                            obj.DeletedBy = userId;

                            context.JobsCandidate.Update(obj);

                            context.SaveChanges();

                            //add job log for every job activity 
                            JobsLogs log = new JobsLogs();
                            log.Description = ActionCommon.JobUpdate;
                            log.JobId = jobId;
                            log.Ipaddress = ip;
                            log.CreatedDate = DateTime.Now;
                            context.JobsLogs.Add(log);
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
