using cisApp.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace cisApp.Function
{
    public static class GetAttachFile
    {
        public static string _UploadDir = "Uploads";
        static IConfigurationRoot _config = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("appsettings.json")
                                    .Build();
        public class Get
        {
            public static AttachFile GetById(Guid id)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.AttachFile.Find(id);

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static AttachFile GetByRefId(Guid refId)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.AttachFile.Where(o => o.RefId == refId && o.IsActive == true).FirstOrDefault();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }                
            }

            public static AttachFile GetByUserId(Guid userId)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = from user in context.Users.Where(o => o.UserId == userId && o.IsActive == true) 
                                   join img in context.AttachFile on user.UserImgId equals img.RefId
                                   select img;
                        if (!data.Any())
                            return null;

                        return data.FirstOrDefault();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static List<AttachFile> GetByRefIdList(Guid id)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@id", id)
                    };

                    return StoreProcedure.GetAllStored<AttachFile>("GetAttachFileByRefId", parameter);
                }
                catch (Exception ex)
                {
                    return new List<AttachFile>();
                }
            }
        }

        public class Manage
        {

            public static AttachFile UploadFile(string base64File, string fileName, int fileSize, Guid? refId, Guid userId)
            {
                try
                {
                    if (string.IsNullOrEmpty(base64File))
                        return null;
                                       

                    Guid id = Guid.NewGuid();

                    //string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", _UploadDir, id.ToString());
                    string uploadPath = Path.Combine(_config.GetSection("Upload:Path").Value, _UploadDir, id.ToString());

                    string virtualPath = Path.Combine(_UploadDir, id.ToString(), fileName);

                    AttachFile attachFile = new AttachFile();
                    attachFile.AttachFileId = id;
                    attachFile.RefId = refId;
                    attachFile.IsActive = true;
                    attachFile.FileName = fileName;
                    attachFile.Path = virtualPath;
                    attachFile.Size = (int)fileSize;

                    attachFile.CreatedBy = userId;
                    attachFile.CreatedDate = DateTime.Now;
                    attachFile.UpdatedBy = userId;
                    attachFile.UpdatedDate = DateTime.Now;

                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);

                    File.WriteAllBytes(Path.Combine(uploadPath, fileName), Convert.FromBase64String(base64File.Split(",")[1]));

                    using (var context = new CAppContext())
                    {
                        context.AttachFile.Add(attachFile);

                        context.SaveChanges();
                    }

                    return attachFile;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static AttachFile EditFile(string base64File, string fileName, int fileSize, Guid? attachFileId, Guid userId)
            {
                try
                {
                    if (string.IsNullOrEmpty(base64File))
                        return null;

                    using (var context = new CAppContext())
                    {

                        //string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", _UploadDir, id.ToString());
                        string uploadPath = Path.Combine(_config.GetSection("Upload:Path").Value, _UploadDir, attachFileId.ToString());

                        string virtualPath = Path.Combine(_UploadDir, attachFileId.ToString(), fileName);

                        AttachFile attachFile = context.AttachFile.Find(attachFileId);
                        
                        attachFile.IsActive = true;
                        attachFile.FileName = fileName;
                        attachFile.Path = virtualPath;
                        attachFile.Size = (int)fileSize;

                        //attachFile.CreatedBy = userId;
                        //attachFile.CreatedDate = DateTime.Now;
                        attachFile.UpdatedBy = userId;
                        attachFile.UpdatedDate = DateTime.Now;

                        if (!Directory.Exists(uploadPath))
                            Directory.CreateDirectory(uploadPath);

                        File.WriteAllBytes(Path.Combine(uploadPath, fileName), Convert.FromBase64String(base64File.Split(",")[1]));

                        context.AttachFile.Update(attachFile);

                        context.SaveChanges();

                        return attachFile;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public static bool UploadFile(IFormFile uploadFile, Guid refId, Guid userId)
            {
                try
                {
                    if (uploadFile == null)
                        return true;
                                      

                    Guid id = Guid.NewGuid();

                    //string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", _UploadDir, id.ToString());
                    string uploadPath = Path.Combine(_config.GetSection("Upload:Path").Value, _UploadDir, id.ToString());

                    string virtualPath = Path.Combine(_UploadDir, id.ToString(), uploadFile.FileName);

                    AttachFile attachFile = new AttachFile();
                    attachFile.AttachFileId = id;
                    attachFile.RefId = refId;
                    attachFile.IsActive = true;
                    attachFile.FileName = uploadFile.FileName;
                    attachFile.Path = virtualPath;
                    attachFile.Size = (int)uploadFile.Length;

                    attachFile.CreatedBy = userId;
                    attachFile.CreatedDate = DateTime.Now;
                    attachFile.UpdatedBy = userId;
                    attachFile.UpdatedDate = DateTime.Now;

                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);

                    using (var fileStream = new FileStream(Path.Combine(uploadPath, uploadFile.FileName), FileMode.Create))
                    {
                        uploadFile.CopyToAsync(fileStream).Wait();

                        using (var context = new CAppContext())
                        {
                            context.AttachFile.Update(attachFile);

                            context.SaveChanges();
                        }
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static List<AttachFile> UpdateStatusByRefId(Guid id, bool active, Guid userId)
            {                
                try
                {
                    using (var context = new CAppContext())
                    {
                        var attchFiles = context.AttachFile.Where(o => o.RefId == id).ToList();

                        foreach (var item in attchFiles)
                        {
                            item.IsActive = active;
                            item.UpdatedBy = userId;
                            item.UpdatedDate = DateTime.Now;
                        }

                        context.AttachFile.UpdateRange(attchFiles);

                        context.SaveChanges();

                        return attchFiles;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static AttachFile ChangeRefId(Guid id, Guid RefId, Guid userId)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var attchFile = context.AttachFile.Find(id);

                        attchFile.RefId = RefId;
                        attchFile.UpdatedBy = userId;
                        attchFile.UpdatedDate = DateTime.Now;

                        context.AttachFile.Update(attchFile);

                        context.SaveChanges();

                        return attchFile;
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
            public static FileAttachModel ToFileAttachModel(AttachFile data)
            {
                FileAttachModel model = new FileAttachModel()
                {
                    AttachFileId = data.AttachFileId,
                    RefId = data.RefId,
                    FileName = data.FileName,
                    Path = data.Path,
                    Size = data.Size,
                    IsActive = data.IsActive,
                    CreatedDate = data.CreatedDate,
                    UpdatedDate = data.UpdatedDate,
                    CreatedBy = data.CreatedBy,
                    UpdatedBy = data.UpdatedBy,
                    DeletedDate = data.DeletedDate,
                    DeletedBy = data.DeletedBy
                };

                return model;
            }
        }
    }
}
