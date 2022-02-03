using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using cisApp.Core;
using cisApp.library;

namespace cisApp.Function
{
    public static class GetJobPayment
    { 
        public class Get
        {
            public static List<JobPayment> GetAll()
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.JobPayment.ToList();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public static List<JobPayment> GetBySearch(SearchModel model)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                        new SqlParameter("@stext", !String.IsNullOrEmpty(model.text) ? model.text.Trim() : (object)DBNull.Value),
                       new SqlParameter("@jobId", model.JobId != null ? model.JobId : (object)DBNull.Value),
                       new SqlParameter("@jobStatus", model.JobStatus != null ? model.JobStatus : (object)DBNull.Value),
                       new SqlParameter("@jobType", model.jobType != null ? model.jobType : (object)DBNull.Value),
                       new SqlParameter("@userId", model.UserId != null ? model.UserId : (object)DBNull.Value),
                       new SqlParameter("@payMonth", model.Month != null  ? model.Month : (object)DBNull.Value),
                       new SqlParameter("@payYear", model.Year != null  ? model.Year : (object)DBNull.Value),
                       new SqlParameter("@skip", model.currentPage.HasValue ? (model.currentPage-1)*model.pageSize : (object)DBNull.Value),
                       new SqlParameter("@take", model.pageSize.HasValue ? model.pageSize.Value : (object)DBNull.Value)
                    };

                    return StoreProcedure.GetAllStored<JobPayment>("GetJobPayment", parameter);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static int GetBySearchTotal(SearchModel model)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                        new SqlParameter("@stext", !String.IsNullOrEmpty(model.text) ? model.text.Trim() : (object)DBNull.Value),
                       new SqlParameter("@jobId", model.JobId != null ? model.JobId : (object)DBNull.Value),
                       new SqlParameter("@jobStatus", model.JobStatus != null ? model.JobStatus : (object)DBNull.Value),
                       new SqlParameter("@jobType", model.jobType != null ? model.jobType : (object)DBNull.Value)   ,
                       new SqlParameter("@userId", model.UserId != null ? model.UserId : (object)DBNull.Value),
                       new SqlParameter("@payMonth", model.Month != null  ? model.Month : (object)DBNull.Value),
                       new SqlParameter("@payYear", model.Year != null  ? model.Year : (object)DBNull.Value)
                    };
                    var dt = StoreProcedure.GetAllStoredDataTable("GetJobPaymentTotal", parameter);
                    return (int)dt.Rows[0]["TotalCount"];
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }

            public static JobPayment GetById(int id)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.JobPayment.Where(o => o.JobPayId == id).FirstOrDefault();

                        // get payment_img id
                        var paymentImg = Get.GetJobPaymentImgs(data.JobPayId.Value);

                        if (paymentImg.Count > 0)
                        {
                            data.AttachFileImage = GetAttachFile.Get.GetByRefId(paymentImg.FirstOrDefault().JobPayimgId.Value);
                        }

                        

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public static List<JobPayment> GetByJobId(Guid id)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.JobPayment.Where(o => o.JobId == id).OrderByDescending(o => o.PayDate).ToList();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static List<JobPaymentImg> GetJobPaymentImgs(int paymentId)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.JobPaymentImg.Where(o => o.JobPayId == paymentId).ToList();

                        data = (from p in data
                                join a in context.AttachFile on p.JobPayimgId equals a.RefId
                                where a.IsActive == true
                                select p).ToList();

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
            public static JobPayment Update(JobPayment data, Guid userId, string ip = "")
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            //using var transaction = context.Database.BeginTransaction();
                            JobPayment obj = new JobPayment();

                            if (data.JobPayId != null)
                            {
                                obj = context.JobPayment.Find(data.JobPayId);
                                obj.PayDate = data.PayDate;
                            }
                            else
                            {
                                obj.PayDate = DateTime.Now;
                                obj.JobId = data.JobId;
                                var dataList = context.JobPayment.ToList();
                                if (dataList != null && dataList.Count > 0)
                                {
                                    var dl = dataList.OrderBy(o => o.JobNo).LastOrDefault();
                                    obj.OrderId = Utility.GenerateRequestCode("IDP{0}-{1}{2}", dl.JobPayId.Value + 1);
                                }
                                else
                                {
                                    obj.OrderId = Utility.GenerateRequestCode("IDP{0}-{1}{2}", 1);
                                }                           
                                obj.CreatedBy = userId;
                                obj.CreatedDate = DateTime.Now;
                            }


                            obj.PayStatus = data.PayStatus;
                            obj.Comment = data.Comment;
                            obj.UpdatedBy = userId;
                            obj.UpdatedDate = DateTime.Now;

                            context.JobPayment.Update(obj);

                            context.SaveChanges();


                            // save profile
                            if (!String.IsNullOrEmpty(data.FileBase64)) // ถ้ามีไฟล์อัพมาใหม่ fileBase64 จะมีค่า
                            {
                                // remove previous img
                                var activePaymentImg = Get.GetJobPaymentImgs(obj.JobPayId.Value);

                                if (activePaymentImg.Count > 0)
                                {
                                    foreach (var item in activePaymentImg)
                                    {
                                        GetAttachFile.Manage.UpdateStatusByRefId(item.JobPayimgId.Value, false, userId);
                                    }
                                }

                                // insert new paymentImg
                                JobPaymentImg jobPaymentImg = new JobPaymentImg()
                                {
                                    JobPayId = obj.JobPayId
                                };

                                context.JobPaymentImg.Update(jobPaymentImg);

                                context.SaveChanges();

                                GetAttachFile.Manage.UploadFile(data.FileBase64, data.FileName, Convert.ToInt32(data.FileSize), jobPaymentImg.JobPayimgId.Value, userId);
                            }
                            else if (data.FileRemove) // ถ้าลบไฟล์ออก แล้วไม่ได้อัพไฟล์ใหม่ขึ้นมาจะเข้า เงื่อนไขนี้
                            {
                                var paymentImg = context.JobPaymentImg.Where(o => o.JobPayId == obj.JobPayId).FirstOrDefault();

                                GetAttachFile.Manage.UpdateStatusByRefId(paymentImg.JobPayimgId.Value, false, userId);
                            }

                            //add job log for every job activity 
                            JobsLogs log = new JobsLogs();
                            log.JobId = obj.JobId.Value;
                            log.Ipaddress = ip;
                            log.CreatedDate = DateTime.Now;
                            context.JobsLogs.Add(log);
                            context.SaveChanges();

                            if (obj.PayStatus == 3)
                            {
                                var job = GetJobs.Get.GetById(obj.JobId.Value);

                                var previousJobStatus = job.JobStatus;

                                job.JobStatus = 3;

                                context.Jobs.Update(job);
                                context.SaveChanges();

                                var cadi = GetJobsCandidate.Get.GetByJobId(new SearchModel() { gId = job.JobId });

                                foreach (var item in cadi)
                                {
                                    item.CaStatusId = 2;
                                }

                                context.JobsCandidate.UpdateRange(cadi);

                                if (previousJobStatus == 2)
                                {
                                    // ต้องสร้างกลุ่มแชท
                                    GetChatGroup.Manage.CreateChatGroupAfterPaymentSuccess(obj.JobId.Value);
                                }

                                context.SaveChanges();
                            }

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

            public static JobPayment AddSlip(int id, int status, Guid attachId, Guid userId, string ip = "")
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            JobPayment obj = context.JobPayment.Find(id);

                            obj.PayStatus = status;
                            obj.UpdatedBy = userId;
                            obj.UpdatedDate = DateTime.Now;

                            context.JobPayment.Update(obj);

                            context.SaveChanges();


                            // remove previous img
                            var activePaymentImg = Get.GetJobPaymentImgs(obj.JobPayId.Value);

                            if (activePaymentImg.Count > 0)
                            {
                                foreach (var item in activePaymentImg)
                                {
                                    GetAttachFile.Manage.UpdateStatusByRefId(item.JobPayimgId.Value, false, userId);
                                }
                            }

                            // insert new paymentImg
                            JobPaymentImg jobPaymentImg = new JobPaymentImg()
                            {
                                JobPayId = obj.JobPayId
                            };

                            context.JobPaymentImg.Update(jobPaymentImg);

                            context.SaveChanges();

                            GetAttachFile.Manage.ChangeRefId(attachId, jobPaymentImg.JobPayimgId.Value, userId);

                            //add job log for every job activity 
                            JobsLogs log = new JobsLogs();
                            log.JobId = obj.JobId.Value;
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

            public static JobPayment Status(int id, int status, string comment, Guid userId)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        JobPayment obj = context.JobPayment.Find(id);

                        obj.PayStatus = status;
                        obj.Comment = comment;
                        obj.UpdatedBy = userId;
                        obj.UpdatedDate = DateTime.Now;                        

                        context.JobPayment.Update(obj);

                        context.SaveChanges();

                        if (obj.PayStatus == 3)
                        {
                            var job = GetJobs.Get.GetById(obj.JobId.Value);

                            var previousJobStatus = job.JobStatus;

                            job.JobStatus = 3;

                            context.Jobs.Update(job);
                            context.SaveChanges();

                            var cadi = GetJobsCandidate.Get.GetByJobId(new SearchModel() { gId = job.JobId });

                            foreach (var item in cadi)
                            {
                                item.CaStatusId = 2;
                            }

                            if (previousJobStatus == 2)
                            {
                                // ต้องสร้างกลุ่มแชท
                                GetChatGroup.Manage.CreateChatGroupAfterPaymentSuccess(obj.JobId.Value);
                            }

                            context.JobsCandidate.UpdateRange(cadi);
                            context.SaveChanges();
                        }

                        return obj;
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
