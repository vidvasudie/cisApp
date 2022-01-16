using cisApp.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace cisApp.Function
{
    public static class GetAlbum
    {
        static string _UploadDir = "Uploads";
        public class Get
        {
            public static List<Album> GetByJobId(Guid id)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.Album.Where(o => o.JobId == id && o.IsActive == true && o.IsDeleted == false).ToList();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }                
            }

            public static List<AlbumModel> GetJobCandidateAlbum(SearchModel model)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@jobId", model.gId != null && model.gId != Guid.Empty ? model?.gId : (object)DBNull.Value)
                    };

                    return StoreProcedure.GetAllStored<AlbumModel>("GetJobCandidateAlbum", parameter);
                }
                catch (Exception ex)
                {
                    return new List<AlbumModel>();
                }
            }

            public static List<AlbumModel> GetJobSubmitAlbum(SearchModel model)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@jobId", model.gId != null && model.gId != Guid.Empty ? model?.gId : (object)DBNull.Value),
                       new SqlParameter("@albumType", model.type != null ? model?.type : (object)DBNull.Value)
                    };

                    return StoreProcedure.GetAllStored<AlbumModel>("GetJobSubmitAlbum", parameter);
                }
                catch (Exception ex)
                {
                    return new List<AlbumModel>();
                }
            }

            public static List<AlbumImageModel> GetRandomAlbumImage(string domainUrl, Guid userId, int take = 10)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@userId", userId == Guid.Empty ? (object)DBNull.Value : userId),
                       new SqlParameter("@take", take)
                    };

                    var data = StoreProcedure.GetAllStored<AlbumImageModel>("GetRandomAlbumImage", parameter);

                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            item.FullUrlPath = domainUrl + item.UrlPath;
                        }
                    }

                    return data;
                }
                catch (Exception ex)
                {
                    return new List<AlbumImageModel>();
                }
            }
            public static List<AlbumImageModel> GetRandomAlbumImage(string domainUrl, int take = 10)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@userId", (object)DBNull.Value),
                       new SqlParameter("@take", take)
                    };

                    var data = StoreProcedure.GetAllStored<AlbumImageModel>("GetRandomAlbumImage", parameter);

                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            item.FullUrlPath = domainUrl + item.UrlPath;
                        }
                    }

                    return data;
                }
                catch (Exception ex)
                {
                    return new List<AlbumImageModel>();
                }
            }

            public static List<AlbumImageModel> GetRandomAlbumImage(int take = 10)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@userId", (object)DBNull.Value),
                       new SqlParameter("@take", take)
                    };

                    var data = StoreProcedure.GetAllStored<AlbumImageModel>("GetRandomAlbumImage", parameter);

                    return data;
                }
                catch (Exception ex)
                {
                    return new List<AlbumImageModel>();
                }
            }

            public static List<AlbumImageModel> GetAlbumImage(SearchModel model, string domainUrl = "")
            {
                try
                {
                    int skip = (model.currentPage.Value) * model.pageSize.Value;
                    SqlParameter[] parameter = new SqlParameter[] {
                        new SqlParameter("@stext", model.text),
                        new SqlParameter("@skip", skip),
                        new SqlParameter("@take", model.pageSize.Value)
                    };

                    var data = StoreProcedure.GetAllStored<AlbumImageModel>("GetAlbumImage", parameter);

                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            item.FullUrlPath = domainUrl + item.UrlPath;
                        }
                    }

                    return data;
                }
                catch (Exception ex)
                {
                    return new List<AlbumImageModel>();
                }
            }

        }

        public class Manage
        {
            public static Album Update(AlbumModel data, Guid userId)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            Album obj = new Album();
                            if (data.AlbumId != null)
                            {
                                
                            }
                            else
                            {
                                obj.CreatedBy = userId;
                                obj.CreatedDate = DateTime.Now;
                               
                            }

                            obj.UpdatedBy = userId;
                            obj.UpdatedDate = DateTime.Now;

                            obj.JobId = data.JobId;
                            obj.UserId = data.UserId;
                            obj.Category = data.Category;
                            obj.Tags = data.Tags;
                            obj.AlbumType = data.AlbumType;
                            obj.AlbumName = data.AlbumName;
                            obj.Url = data.Url;

                            obj.IsActive = true;
                            obj.IsDeleted = false;

                            context.Album.Update(obj);

                            context.SaveChanges();

                            //validate insert and remove image 
                            //insert job image ex
                            ManageImages(context, data.files, obj);

                            ManageImageApi(context, data.apiFiles, obj, userId);

                            //add job tracking for jobStatus 
                            //JobsTracking tracking = new JobsTracking();
                            //tracking.JobId = obj.JobId;
                            //tracking.StatusDate = DateTime.Now;
                            //tracking.JobStatus = obj.JobStatus;
                            //context.JobsTracking.Add(tracking);
                            //context.SaveChanges();

                            //add job log for every job activity 
                            JobsLogs log = new JobsLogs();
                            log.JobId = obj.JobId;
                            log.Ipaddress = "";
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

            private static int ManageImages(CAppContext context, List<FileAttachModel> imgs, Album obj)
            {
                if (imgs == null || imgs.Count == 0)
                {
                    return 0;
                }
                int count = imgs.Where(o => o != null).Count();
                if (count == 0)
                {
                    return 0;
                }
                
                // insert อย่างเดียวเลย


                // ถ้ามีไฟล์อัพมาใหม่ fileBase64 จะมีค่า
                foreach (var file in imgs.Where(o => o != null && !String.IsNullOrEmpty(o.FileBase64)))
                {
                    AlbumImage map = new AlbumImage();
                    map.AlbumId = obj.AlbumId.Value;
                    map.UserId = obj.UserId;
                    context.AlbumImage.Add(map);
                    context.SaveChanges();
                                        
                    //insert image base64 into AttachFile
                    Guid id = Guid.NewGuid();

                    string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", GetAttachFile._UploadDir, id.ToString());

                    string virtualPath = Path.Combine(GetAttachFile._UploadDir, id.ToString(), file.FileName);

                    AttachFile attachFile = new AttachFile();
                    attachFile.AttachFileId = id;
                    attachFile.RefId = map.ImgId;
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

            public static int ManageImageApi(CAppContext context, List<Guid> imgs, Album obj, Guid userId)
            {
                try
                {
                    if (imgs == null)
                    {
                        return 1;
                    }

                    // need to delete previous img ?

                    foreach(var file in imgs)
                    {
                        AlbumImage map = new AlbumImage()
                        {
                            AlbumId = obj.AlbumId.Value,
                            UserId = obj.UserId
                        };

                        context.AlbumImage.Add(map);
                        context.SaveChanges();

                        GetAttachFile.Manage.ChangeRefId(file, map.ImgId.Value, userId);
                    }

                    return 1;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
