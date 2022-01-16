using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using cisApp.Core;
using cisApp.library;
using cisApp;

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

            public static List<JobDetailModel> GetJobDetail(Guid jobId)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@jobId", jobId != Guid.Empty ? jobId : (object)DBNull.Value)
                    };

                    return StoreProcedure.GetAllStored<JobDetailModel>("GetJobDetail", parameter);
                }
                catch (Exception ex)
                {
                    return new List<JobDetailModel>();
                }
            }
            public static List<JobModel> GetJobs(SearchModel model)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@stext", !String.IsNullOrEmpty(model.text) ? model.text.Trim() : (object)DBNull.Value),
                       new SqlParameter("@userId", model.Id != null && model.Id != Guid.Empty ? model?.Id : (object)DBNull.Value),
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
                       new SqlParameter("@userId", model.Id != null && model.Id != Guid.Empty ? model?.Id : (object)DBNull.Value),
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

            public static List<CustomerJobListModel> GetCustomerJobList(Guid userId)
            {
                try
                {
                    if (userId == Guid.Empty)
                        return null;

                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@userId", userId) 
                    };

                    return StoreProcedure.GetAllStored<CustomerJobListModel>("GetCustomerJobList", parameter);
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            public static List<DesignerSubmitWorkModel> GetWorkSubmitList(Guid jobId)
            {
                try
                {
                    if (jobId == Guid.Empty)
                        return null;

                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@jobId", jobId)
                    };

                    return StoreProcedure.GetAllStored<DesignerSubmitWorkModel>("GetWorkSubmitList", parameter);
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

            public static List<JobDesignerApproveDetailModel> GetApproveDetail(Guid jobId)
            {
                try
                {
                    if (jobId == Guid.Empty)
                        return null;

                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@jobId", jobId)
                    };

                    return StoreProcedure.GetAllStored<JobDesignerApproveDetailModel>("GetJobDesignerDetailApprove ", parameter);
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public class Manage
        {
            public static Jobs UpdateCandidate(CandidateSelectModel value)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            Jobs obj = new Jobs(); 
                            var data = context.Jobs.Where(o => o.JobId == value.JobId);
                            if (!data.Any())
                            {
                                return null;
                            }
                            obj = data.FirstOrDefault();
                            obj.JobCaUserId = value.CaUserId;
                            obj.JobStatus = 4;//ประกาศ
                            obj.UpdatedDate = DateTime.Now;
                            obj.UpdatedBy = value.UserId;
                            context.Jobs.Update(obj);
                            context.SaveChanges();

                            var jobCas = context.JobsCandidate.Where(o => o.JobId == value.JobId);
                            if (!jobCas.Any())
                            {
                                return null;
                            }
                            var jobCa = jobCas.Where(o => o.UserId == value.CaUserId).FirstOrDefault();
                            jobCa.CaStatusId = 3;//3=คัดเลือก
                            jobCa.UpdatedDate = DateTime.Now;
                            jobCa.UpdatedBy = value.UserId;
                            context.JobsCandidate.Update(jobCa);
                            context.SaveChanges();

                            //change status for other 2 candidate
                            var jobCa2 = context.JobsCandidate.Where(o => o.JobId == value.JobId && o.UserId != value.CaUserId);
                            foreach (var ca in jobCa2)
                            {
                                ca.CaStatusId = 4;//4=ไม่ได้รับคัดเลือก
                                ca.UpdatedDate = DateTime.Now;
                                ca.UpdatedBy = value.UserId;

                                context.JobsCandidate.Update(ca);
                                context.SaveChanges();
                            } 

                            //clear work slot for other 2 candidate
                            List<UserDesigner> tmp = new List<UserDesigner>();
                            var usrDesginers = context.UserDesigner.ToList();
                            foreach (var ca in context.JobsCandidate.Where(o => o.JobId == value.JobId && o.UserId != value.CaUserId))
                            {
                                var usrDesginer = usrDesginers.Where(o => o.UserId == ca.UserId).FirstOrDefault();
                                usrDesginer.AreaSqmused -= (int) obj.JobAreaSize;
                                usrDesginer.AreaSqmremain = usrDesginer.AreaSqmmax - usrDesginer.AreaSqmused;
                                tmp.Add(usrDesginer);
                            }
                            context.UserDesigner.UpdateRange(tmp);
                            context.SaveChanges();

                            //add job tracking for jobStatus 
                            JobsTracking tracking = new JobsTracking();
                            tracking.JobId = obj.JobId;
                            tracking.StatusDate = DateTime.Now;
                            tracking.JobStatus = 4; //4=ประกาศ
                            context.JobsTracking.Add(tracking);
                            context.SaveChanges();

                            //add job log for every job activity 
                            JobsLogs log = new JobsLogs();
                            log.JobId = obj.JobId;
                            log.Description = ActionCommon.JobCandidateSelected;
                            log.Ipaddress = value.ip;
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
            public static Jobs Update(JobModel data, string ip = null)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            Jobs obj = new Jobs();
                            JobsLogs log = new JobsLogs();
                            if (data.JobId != null && data.JobId != Guid.Empty)
                            {
                                obj = context.Jobs.Where(o => o.JobId == data.JobId).FirstOrDefault();
                                log.Description = ActionCommon.JobUpdate;
                            }
                            else
                            {
                                log.Description = ActionCommon.JobInsert;
                                obj.CreatedDate = DateTime.Now;
                                obj.CreatedBy = data.CreatedBy;
 
                            }
                            if (data.JobStatus == 2)
                            {
                                //create new JobNo if not Draft and new job status = 2
                                var dataList = context.Jobs.ToList();
                                if (dataList != null && dataList.Count > 0)
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
                            obj.JobPricePerSqM = data.JobPricePerSqM;
                            
                            obj.JobPrice = data.JobPrice;
                            
                            obj.JobProceedRatio = data.JobProceedRatio;
                            obj.JobPriceProceed = data.JobPriceProceed;

                            obj.JobVatratio = data.JobVatratio;
                            obj.JobFinalPrice = data.JobFinalPrice;

                            obj.IsInvRequired = data.IsInvRequired;
                            obj.InvAddress = data.InvAddress;
                            obj.InvPersonalId = data.InvPersonalId;

                            obj.JobStatus = data.JobStatus;
                            obj.JobBeginDate = data.JobBeginDate;
                            obj.JobEndDate = data.JobEndDate;
                            obj.UpdatedDate = DateTime.Now;
                            obj.UpdatedBy = data.UpdatedBy;

                            context.Jobs.Update(obj);
                            context.SaveChanges();

                            //validate insert and remove image 
                            //insert job image ex
                            if (data.IsApi)
                            {   
                                // ไฟล์อัพมาใหม่ 
                                foreach (var file in data.files.Where(o => o != null))
                                {
                                    //insert JobExImage
                                    JobsExamImage map = new JobsExamImage();
                                    map.JobsExImgId = Guid.NewGuid();
                                    map.JobsExTypeId = file.TypeId;
                                    map.JobId = obj.JobId;
                                    context.JobsExamImage.Add(map);
                                    context.SaveChanges();

                                    var athFile = context.AttachFile.Where(o => o.AttachFileId == file.AttachFileId);
                                    if (athFile.Any())
                                    {
                                        var afile = athFile.FirstOrDefault();
                                        afile.RefId = map.JobsExImgId;
                                        context.AttachFile.Update(afile);
                                        context.SaveChanges();
                                    }
                                }
                            }
                            else
                            {
                                ManageImages(context, data.files, obj);
                            }
                            

                            //add job tracking for jobStatus 
                            JobsTracking tracking = new JobsTracking();
                            tracking.JobId = obj.JobId;
                            tracking.StatusDate = DateTime.Now;
                            tracking.JobStatus = obj.JobStatus;
                            context.JobsTracking.Add(tracking);
                            context.SaveChanges();

                            //add job log for every job activity 
                            log.JobId = obj.JobId; 
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
            private static int ManageImages(CAppContext context, List<FileAttachModel> imgs, Jobs obj)
            {
                if(imgs == null || imgs.Count == 0)
                {
                    return 0; 
                }
                int count = imgs.Where(o => o != null).Count();
                if (count == 0)
                {
                    return 0;
                }
                //get old data ที่ไม่อยุ่ในรายการที่ o.FileBase64 ไม่มีค่า = ลบทิ้ง
                string listId = String.Join(",", imgs.Where(o => o != null && String.IsNullOrEmpty(o.FileBase64)).Select(o => o.gId.ToString()));
                SqlParameter[] parameter = new SqlParameter[] { 
                       new SqlParameter("@jobId", obj.JobId), //jobid
                       new SqlParameter("@imgList", listId), //list
                       new SqlParameter("@mode", "1")//not in
                    };
                var delList = StoreProcedure.GetAllStored<FileAttachModel>("GetJobsExImageFile", parameter);

                foreach (var file in delList)
                {
                    var item = context.AttachFile.Where(o => o.AttachFileId == file.AttachFileId).FirstOrDefault();
                    item.IsActive = false;
                    item.UpdatedDate = obj.UpdatedDate.Value;
                    item.UpdatedBy = obj.UpdatedBy.Value;
                    context.AttachFile.Remove(item);
                    context.SaveChanges();
                }
                

                // ถ้ามีไฟล์อัพมาใหม่ fileBase64 จะมีค่า
                foreach (var file in imgs.Where(o => o != null && !String.IsNullOrEmpty(o.FileBase64)))
                {
                    //insert JobExImage
                    JobsExamImage map = new JobsExamImage();
                    map.JobsExImgId = Guid.NewGuid();
                    map.JobsExTypeId = file.TypeId;
                    map.JobId = obj.JobId;
                    context.JobsExamImage.Add(map); 
                    context.SaveChanges();

                    //insert image base64 into AttachFile
                    Guid id = Guid.NewGuid();

                    string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", GetAttachFile._UploadDir, id.ToString());

                    string virtualPath = Path.Combine(GetAttachFile._UploadDir, id.ToString(), file.FileName);

                    AttachFile attachFile = new AttachFile();
                    attachFile.AttachFileId = id;
                    attachFile.RefId = map.JobsExImgId;
                    attachFile.IsActive = true;
                    attachFile.FileName = file.FileName;
                    attachFile.Path = virtualPath;
                    attachFile.Size = file.Size;

                    attachFile.CreatedBy = obj.CreatedBy.Value;
                    attachFile.CreatedDate = DateTime.Now;
                    attachFile.UpdatedBy = obj.UpdatedBy.Value;
                    attachFile.UpdatedDate = DateTime.Now;

                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);

                    File.WriteAllBytes(Path.Combine(uploadPath, file.FileName), Convert.FromBase64String(file.FileBase64.Split(",")[1]));

                    context.AttachFile.Add(attachFile); 
                    context.SaveChanges();
                }

                return 1; 
            } 

            public static Jobs CancelJob(Guid jobId, Guid userId, int cancelId, string cancelMsg, string ip)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            var jobPms = context.JobPayment.Where(o => o.JobId == jobId);
                            if (jobPms.Any())
                            {
                                if(jobPms.Where(o => o.PayStatus != 1 || o.PayStatus != 4).Count() > 0) //1=รอชำระเงิน, 4=ไม่อนุมัติ/คืนเงิน 
                                    return null;
                            }
                                
                            var datas = context.Jobs.Where(o => o.JobId == jobId);
                            if (!datas.Any())
                                return null;

                            var data = datas.FirstOrDefault();
                            data.CancelId = cancelId;
                            data.JobStatus = 6;//ยกเลิก
                            data.UpdatedDate = DateTime.Now;
                            data.UpdatedBy = userId;

                            context.Jobs.Update(data);
                            context.SaveChanges();

                            //get massage cancel
                            if (String.IsNullOrEmpty(cancelMsg) && cancelId > 0)
                            {
                                var cc = context.TmCauseCancel.Where(o => o.Id == cancelId).FirstOrDefault();
                                cancelMsg = cc.Description;
                            }

                            //add job log for every job activity 
                            JobsLogs log = new JobsLogs();
                            log.Description = cancelMsg;
                            log.JobId = data.JobId;
                            log.Ipaddress = ip;
                            log.CreatedDate = DateTime.Now;
                            context.JobsLogs.Add(log);
                            context.SaveChanges();

                            dbContextTransaction.Commit();

                            return data;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static Jobs UpdateEditCount(Guid jobId, int editCount)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var job = context.Jobs.Find(jobId);

                        job.EditSubmitCount = editCount;

                        context.Jobs.Update(job);

                        context.SaveChanges();

                        return job;
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
