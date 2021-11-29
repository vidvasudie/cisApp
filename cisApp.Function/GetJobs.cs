using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using cisApp.Core;
using cisApp.library;

namespace cisApp.Function
{
    public static class GetJobs
    {
        public class Get
        {
            public static List<Jobs> GetAll()
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.Jobs.ToList();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public static Jobs GetById(Guid id)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.Jobs.Where(o => o.JobId == id).FirstOrDefault();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
             

            public static List<JobModel> GetJobs(SearchModel model)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@stext", !String.IsNullOrEmpty(model.text) ? model.text.Trim() : (object)DBNull.Value),
                       new SqlParameter("@jobId", model.gId != null && model.gId != Guid.Empty ? model?.gId : (object)DBNull.Value),
                       new SqlParameter("@jobType", model.type.HasValue ? model.type : (object)DBNull.Value),
                       new SqlParameter("@jobStatus", model.status != 0 ? model.status : (object)DBNull.Value),
                       new SqlParameter("@skip", model.currentPage.HasValue ? (model.currentPage-1)*model.pageSize : (object)DBNull.Value),
                       new SqlParameter("@take", model.pageSize.HasValue ? model.pageSize.Value : (object)DBNull.Value)
                    };

                    return StoreProcedure.GetAllStored<JobModel>("GetJobs", parameter);
                }
                catch (Exception ex)
                {
                    return new List<JobModel>();
                }
            }
            public static int GetJobsTotal(SearchModel model)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@stext", !String.IsNullOrEmpty(model.text) ? model.text.Trim() : (object)DBNull.Value),
                       new SqlParameter("@jobId", model.gId != null && model.gId != Guid.Empty ? model?.gId : (object)DBNull.Value),
                       new SqlParameter("@jobType", model.type.HasValue ? model.type : (object)DBNull.Value),
                       new SqlParameter("@jobStatus", model.status != 0 ? model.status : (object)DBNull.Value)
                    };

                    var dt = StoreProcedure.GetAllStoredDataTable("GetJobsTotal", parameter);
                    return (int)dt.Rows[0]["TotalCount"];
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }


        }

        public class Manage
        {
            public static Jobs Update(JobModel data)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            Jobs obj = new Jobs();
                            
                            if (data.JobId != null && data.JobId != Guid.Empty)
                            {
                                obj = context.Jobs.Find(data.JobId);
                            }
                            else
                            {
                                //obj.CreatedDate = DateTime.Now;
                                //obj.CreatedBy = data.CreatedBy;

                                //create new JobNo
                                var dataList = context.Jobs.ToList();
                                if(dataList != null && dataList.Count > 0)
                                {
                                    var dl = dataList.OrderBy(o => o.JobNo).LastOrDefault();
                                    obj.JobNo = Utility.GenerateRequestCode("ID{0}-{1}{2}", Int32.Parse(dl.JobNo.Substring(7, 5)) + 1, dl.JobNo.Substring(5, 2) != DateTime.Now.Month.ToString("00"));
                                }
                                else
                                {
                                    obj.JobNo = Utility.GenerateRequestCode("ID{0}-{1}{2}", 0, true);
                                }
                            } 
                            obj.UserId = data.UserId; 
                            obj.JobCaUserId = data.JobCaUserId;
                            obj.JobTypeId = data.JobTypeId;
                            obj.JobDescription = data.JobDescription;
                            obj.JobAreaSize = data.JobAreaSize;
                            obj.JobPrice = data.JobPrice;
                            obj.JobPricePerSqM = data.JobPricePerSqM;
                            obj.JobStatus = data.JobStatus;
                            obj.JobBeginDate = data.JobBeginDate;
                            obj.JobEndDate = data.JobEndDate;
                            //obj.UpdatedDate = DateTime.Now;
                            //obj.UpdatedBy = data.UpdatedBy;

                            context.Jobs.Update(obj);
                            context.SaveChanges();
                              
                            //add job tracking for jobStatus 

                            //add job log for every job activity

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
