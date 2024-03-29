﻿using System;
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
            public static List<JobCandidateModel> GetAvailableByJobId(SearchModel model)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@jobId", model.gId != null && model.gId != Guid.Empty ? model?.gId.ToString() : (object)DBNull.Value),
                       new SqlParameter("@status", !String.IsNullOrEmpty(model.statusStr) ? model.statusStr : (object)DBNull.Value),
                       new SqlParameter("@statusOpt", String.IsNullOrEmpty(model.statusOpt) ? "equal" : model.statusOpt)//equal, more, less, in
                    };

                    return StoreProcedure.GetAllStored<JobCandidateModel>("GetJobsCandidateAvailable", parameter);
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
                            int dbCount = context.JobsCandidate.Where(o => o.JobId == jobId && o.UserId == userId && o.CaStatusId == 5).Count();
                            if(dbCount >= 2)
                            {
                                return null;
                            }
                            dbCount = context.JobsCandidate.Where(o => o.JobId == jobId && o.UserId == userId && o.CaStatusId == 7).Count();
                            if (dbCount > 5)
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

                            //lock designer work slot when sign job
                            ValidWorkSlot(context, jobId, userId);

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
                            int result = 0;
                            var job = new Jobs();
                            JobsLogs log = new JobsLogs();
                            if (list != null && list.Count > 0)
                            {
                                job = GetJobs.Get.GetById(list.First().JobId.Value);
                                var status = 1;// รอประกวด
                                if(job.JobStatus == 3)
                                {
                                    status = 2;//อยู่ระหว่างประกวด
                                } 
                                foreach (var item in list)
                                {
                                    log.JobId = item.JobId.Value;
                                    JobsCandidate obj = new JobsCandidate();
                                    obj.CaStatusId = status;
                                    obj.JobId = item.JobId;
                                    obj.UserId = item.UserId;
                                    obj.CreatedDate = DateTime.Now;
                                    obj.CreatedBy = _user;

                                    context.JobsCandidate.Update(obj);
                                    context.SaveChanges();

                                    //lock designer work slot when sign job
                                    GetJobsCandidate.Manage.ValidWorkSlot(context, job.JobId, item.UserId.Value);
                                }
                            }
                             
                            //add job log for every job activity  
                            log.Description = ActionCommon.JobCandidateAdd;  
                            log.Ipaddress = ip;
                            log.CreatedDate = DateTime.Now;
                            context.JobsLogs.Add(log);
                            context.SaveChanges();

                            dbContextTransaction.Commit();
                            result = 1;

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

                            obj.CaStatusId = 5;//ปฏิเสธ
                            obj.UpdatedDate = DateTime.Now;
                            obj.UpdatedBy = userId;

                            //obj.IsDeleted = true; 
                            //obj.DeletedDate = DateTime.Now;
                            //obj.DeletedBy = userId;

                            context.JobsCandidate.Update(obj); 
                            context.SaveChanges();

                            //unlock designer work slot when sign job
                            ValidWorkSlot(context, obj.JobId.Value, obj.UserId.Value, "unlock");

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

            public static JobsCandidate StatusUpdate(Guid jobId, Guid caUserId, Guid userId, int status, string ip=null)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            JobsCandidate obj = context.JobsCandidate.Where(o => o.JobId == jobId && o.UserId == caUserId && (o.CaStatusId == 1 || o.CaStatusId == 2 || o.CaStatusId == 3)).FirstOrDefault();

                            obj.CaStatusId = status;

                            obj.UpdatedDate = DateTime.Now;
                            obj.UpdatedBy = userId;

                            context.JobsCandidate.Update(obj); 
                            context.SaveChanges();

                            if (obj.CaStatusId == 4 || obj.CaStatusId == 5 || obj.CaStatusId == 6 || obj.CaStatusId == 7)
                            {
                                //unlock designer work slot when sign job
                                ValidWorkSlot(context, obj.JobId.Value, userId, "unlock");
                            }

                            //add job log for every job activity
                            JobsLogs log = new JobsLogs();
                            log.JobId = obj.JobId.Value;
                            log.Description = ActionCommon.JobCandidateCancel;
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
                            var objs = context.JobsCandidate.Where(o => o.JobId == jobId && o.UserId == caUserId && o.CaStatusId != 5);
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

                            //unlock designer work slot when sign job
                            ValidWorkSlot(context, jobId, caUserId, "unlock");

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

            /// <summary>
            /// 
            /// </summary>
            /// <param name="context"></param>
            /// <param name="jobId"></param>
            /// <param name="userId"></param>
            /// <param name="mode">lock, unlock</param>
            public static void ValidWorkSlot(CAppContext context, Guid jobId, Guid userId, string mode = "lock")//"unlock"
            {
                Jobs job = context.Jobs.Where(o => o.JobId == jobId).FirstOrDefault();
                UserDesigner uds = context.UserDesigner.Where(o => o.UserId == userId).FirstOrDefault();
                if (mode == "lock")
                {
                    uds.AreaSqmremain -= (int)job.JobAreaSize;
                    uds.AreaSqmused = uds.AreaSqmmax - uds.AreaSqmremain;
                }
                else
                {
                    uds.AreaSqmused -= (int)job.JobAreaSize;
                    uds.AreaSqmremain = uds.AreaSqmmax - uds.AreaSqmused;
                }

                context.UserDesigner.Update(uds);

                context.SaveChanges();
            }

        }

    }
}
