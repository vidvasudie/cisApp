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

            public static Album GetUserExampleAlbum(Guid userId)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.Album.Where(o => o.UserId == userId && o.AlbumType == "0" && o.IsDeleted == false).FirstOrDefault();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static Album GetByJobIdWithStatus(Guid id, Guid userId, string albumType)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.Album.Where(o => o.JobId == id && o.UserId == userId && o.AlbumType == albumType
                        && o.IsActive == true && o.IsDeleted == false).FirstOrDefault();

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

            public static List<AlbumImageModel> GetAlbum(SearchModel model, string domainUrl = "")
            {
                try
                {
                    List<string> imgString = new List<string>();

                    if (model.Imgs != null)
                    {
                        imgString = model.Imgs.Select(o => "'" + o.ToString() + "'").ToList();
                    }

                    int skip = (model.currentPage.Value - 1) * model.pageSize.Value;
                    SqlParameter[] parameter = new SqlParameter[] {
                        new SqlParameter("@stext", ""),
                        new SqlParameter("@orderBy", model.Orderby),
                        new SqlParameter("@tags", model.Tags),
                        new SqlParameter("@categories", model.Categories),
                        new SqlParameter("@skip", skip),
                        new SqlParameter("@take", model.pageSize.Value),
                        new SqlParameter("@imgs", model.Imgs != null ? String.Join(",", imgString) : (object)DBNull.Value),
                        new SqlParameter("@designer", model.Designer != null ? model.Designer : (object)DBNull.Value)
                    };

                    var data = StoreProcedure.GetAllStored<AlbumImageModel>("GetAlbum", parameter);

                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            var albumImgs = GetAlbumImageByAlbumId(new SearchModel()
                            {
                                AlbumId = item.AlbumId,
                                pageSize = 5
                            }, domainUrl);

                            item.Thumbnails = new List<AlbumThumbnail>();

                            if (albumImgs != null)
                            {
                                foreach (var img in albumImgs)
                                {
                                    item.Thumbnails.Add(new AlbumThumbnail()
                                    {
                                        AttachId = img.AttachFileId,
                                        FileName = img.FileName,
                                        UrlPath = img.UrlPath,
                                        FullUrlPath = img.FullUrlPath
                                    });
                                }
                            }
                        }
                    }

                    return data;
                }
                catch (Exception ex)
                {
                    return new List<AlbumImageModel>();
                }
            }

            public static int GetAlbumTotal(SearchModel model)
            {
                try
                {

                    List<string> imgString = new List<string>();

                    if (model.Imgs != null)
                    {
                        imgString = model.Imgs.Select(o => "'" + o.ToString() + "'").ToList();
                    }
                    SqlParameter[] parameter = new SqlParameter[] {
                        new SqlParameter("@stext", ""),
                        new SqlParameter("@orderBy", model.Orderby),
                        new SqlParameter("@tags", model.Tags),
                        new SqlParameter("@categories", model.Categories),
                        new SqlParameter("@imgs", model.Imgs != null ? String.Join(",", imgString) : (object)DBNull.Value),
                        new SqlParameter("@designer", model.Designer != null ? model.Designer : (object)DBNull.Value)
                    };

                    var dt = StoreProcedure.GetAllStoredDataTable("GetAlbumTotal", parameter);
                    return (int)dt.Rows[0]["TotalCount"];
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }

            public static List<AlbumImageModel> GetAlbumImage(SearchModel model, string domainUrl = "")
            {
                try
                {
                    List<string> imgString = new List<string>();

                    if (model.Imgs != null)
                    {
                        imgString = model.Imgs.Select(o => "'" + o.ToString() + "'").ToList();                        
                    }

                    int skip = (model.currentPage.Value - 1) * model.pageSize.Value;
                    SqlParameter[] parameter = new SqlParameter[] {
                        new SqlParameter("@stext", ""),
                        new SqlParameter("@orderBy", model.Orderby),
                        new SqlParameter("@tags", model.Tags),
                        new SqlParameter("@categories", model.Categories),
                        new SqlParameter("@skip", skip),
                        new SqlParameter("@take", model.pageSize.Value),
                        new SqlParameter("@imgs", model.Imgs != null ? String.Join(",", imgString) : (object)DBNull.Value),
                        new SqlParameter("@designer", model.Designer != null ? model.Designer : (object)DBNull.Value)
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

            public static int GetAlbumImageTotal(SearchModel model)
            {
                try
                {

                    List<string> imgString = new List<string>();

                    if (model.Imgs != null)
                    {
                        imgString = model.Imgs.Select(o => "'" + o.ToString() + "'").ToList();
                    }
                    SqlParameter[] parameter = new SqlParameter[] {
                        new SqlParameter("@stext", ""),
                        new SqlParameter("@orderBy", model.Orderby),
                        new SqlParameter("@tags", model.Tags),
                        new SqlParameter("@categories", model.Categories),
                        new SqlParameter("@imgs", model.Imgs != null ? String.Join(",", imgString) : (object)DBNull.Value),
                        new SqlParameter("@designer", model.Designer != null ? model.Designer : (object)DBNull.Value)
                    };

                    var dt = StoreProcedure.GetAllStoredDataTable("GetAlbumImageTotal", parameter);
                    return (int)dt.Rows[0]["TotalCount"];
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }

            public static List<AlbumImageModel> GetAlbumImageByAlbumId(SearchModel model, string domainUrl = "")
            {
                try
                {
                    List<string> imgString = new List<string>();

                    if (model.Imgs != null)
                    {
                        imgString = model.Imgs.Select(o => "'" + o.ToString() + "'").ToList();
                    }

                    int skip = (model.currentPage.Value - 1) * model.pageSize.Value;
                    SqlParameter[] parameter = new SqlParameter[] {
                        new SqlParameter("@albumId", model.AlbumId.ToString()),
                        new SqlParameter("@skip", skip),
                        new SqlParameter("@take", model.pageSize.Value),
                        new SqlParameter("@imgs", model.Imgs != null ? String.Join(",", imgString) : (object)DBNull.Value)
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

            public static int GetAlbumImageByAlbumIdTotal(SearchModel model)
            {
                try
                {

                    List<string> imgString = new List<string>();

                    if (model.Imgs != null)
                    {
                        imgString = model.Imgs.Select(o => "'" + o.ToString() + "'").ToList();
                    }
                    SqlParameter[] parameter = new SqlParameter[] {
                        new SqlParameter("@albumId", model.AlbumId.ToString()),
                        new SqlParameter("@imgs", model.Imgs != null ? String.Join(",", imgString) : (object)DBNull.Value)
                    };

                    var dt = StoreProcedure.GetAllStoredDataTable("GetAlbumImageTotal", parameter);
                    return (int)dt.Rows[0]["TotalCount"];
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }

            public static AlbumImageModel GetAlbumImageByAttachId(string domainUrl, Guid id)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@id", id)
                    };

                    var data = StoreProcedure.GetAllStored<AlbumImageModel>("GetAlbumImageByAttachId", parameter);

                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            item.FullUrlPath = domainUrl + item.UrlPath;
                        }
                    }

                    return data.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

            

            public static List<AttachFile> GetAttachFileByAlbumId(int id, string domain)
            {
                try
                {
                    List<AttachFile> attachFiles = new List<AttachFile>();

                    var albumImages = GetAlbumImageByAlbumId(id);

                    foreach (var item in albumImages)
                    {
                        AttachFile attach = GetAttachFile.Get.GetByRefId(item.ImgId.Value);

                        attach.Path = domain + attach.UrlPathAPI;

                        attachFiles.Add(attach);
                    }

                    return attachFiles;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static List<AlbumImage> GetAlbumImageByAlbumId(int id)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var albumImages = context.AlbumImage.Where(o => o.AlbumId == id).ToList();

                        return albumImages;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static Album GetById(int id)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.Album.Find(id);

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static List<CountValueModel> GetAlbumCategories()
            {
                try
                {

                    var data = StoreProcedure.GetAllStoredNonparam<CountValueModel>("GetAlbumCategories");

                    return data;
                }
                catch (Exception ex)
                {
                    return new List<CountValueModel>();
                }
            }

            public static List<CountValueModel> GetAlbumTags()
            {
                try
                {

                    var data = StoreProcedure.GetAllStoredNonparam<CountValueModel>("GetAlbumTags");

                    return data;
                }
                catch (Exception ex)
                {
                    return new List<CountValueModel>();
                }
            }

        }

        public class Manage
        {
            public static Album Update(AlbumModel data, Guid userId, bool isFormAPI = true)
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
                                obj = context.Album.Find(data.AlbumId.Value);
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
                            if (isFormAPI)
                            {
                                ManageImageApi(context, data.apiFiles, obj, userId);
                            }
                            else
                            {
                                ManageImages(context, data.files, obj);
                            }
                            

                            

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

                var albumImages = Get.GetAlbumImageByAlbumId(obj.AlbumId.Value);

                context.AlbumImage.RemoveRange(albumImages);

                context.SaveChanges();

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

                    attachFile.CreatedBy = obj.UpdatedBy.Value;
                    attachFile.CreatedDate = DateTime.Now;
                    attachFile.UpdatedBy = obj.UpdatedBy.Value;
                    attachFile.UpdatedDate = DateTime.Now;

                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);

                    File.WriteAllBytes(Path.Combine(uploadPath, file.FileName), Convert.FromBase64String(file.FileBase64.Split(",")[1]));

                    context.AttachFile.Add(attachFile);
                    context.SaveChanges();
                }

                foreach (var file in imgs.Where(o => o != null && String.IsNullOrEmpty(o.FileBase64)))
                {
                    AlbumImage map = new AlbumImage()
                    {
                        AlbumId = obj.AlbumId.Value,
                        UserId = obj.UserId
                    };

                    context.AlbumImage.Add(map);
                    context.SaveChanges();

                    GetAttachFile.Manage.ChangeRefId(file.AttachFileId, map.ImgId.Value, obj.UpdatedBy.Value);
                }

                return 1;
            }

            public static int ManageImageApi(CAppContext context, List<Guid> imgs, Album obj, Guid userId)
            {
                try
                {
                    // need to delete previous img ?

                    var albumImages = Get.GetAlbumImageByAlbumId(obj.AlbumId.Value);

                    context.AlbumImage.RemoveRange(albumImages);

                    context.SaveChanges();

                    if (imgs == null)
                    {
                        return 1;
                    }
                                       

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

            public static void DeleteAttachFileImage(Guid id, Guid userId)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var attachFile = context.AttachFile.Find(id);
                        if(attachFile != null)
                        {
                            var albumImage = context.AlbumImage.Where(o => o.ImgId == attachFile.RefId).FirstOrDefault();

                            if (albumImage != null)
                            {
                                attachFile.IsActive = false;
                                attachFile.DeletedBy = userId;
                                attachFile.DeletedDate = DateTime.Now;


                                context.AttachFile.Update(attachFile);
                                context.AlbumImage.Remove(albumImage);

                                context.SaveChanges();
                            }
                            
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static void DeleteAlbumExample(Guid userId)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var album = context.Album.Where(o => o.UserId == userId && o.IsDeleted != true && o.AlbumType == "0").FirstOrDefault();
                        if (album != null)
                        {
                            album.IsDeleted = true;
                            album.DeletedDate = DateTime.Now;
                            album.DeletedBy = userId;

                            context.Album.Update(album);

                            context.SaveChanges();
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public class Utils
        {
            public static AlbumModel ToAlbumModel(Album data)
            {
                AlbumModel model = new AlbumModel()
                {
                    AlbumId = data.AlbumId,
                    JobId = data.JobId,
                    UserId = data.UserId,
                    Category = data.Category,
                    Tags = data.Tags,
                    AlbumName = data.AlbumName,
                    Url = data.Url,
                    AlbumType = data.AlbumType,
                    CreatedDate = data.CreatedDate,
                    CreatedBy = data.CreatedBy,
                    UpdatedDate = data.UpdatedDate,
                    UpdatedBy = data.UpdatedBy,
                    DeletedDate = data.DeletedDate,
                    DeletedBy = data.DeletedBy,
                    IsDeleted = data.IsDeleted,
                    IsActive = data.IsActive
                };

                return model;
            }
        }
    }
}
