using cisApp.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace cisApp.Function
{
    public static class GetAttachFile
    {
        public static string _UploadDir = "Uploads";
        public class Get
        {
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

                    string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", _UploadDir, id.ToString());

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
            public static bool UploadFile(IFormFile uploadFile, Guid refId, Guid userId)
            {
                try
                {
                    if (uploadFile == null)
                        return true;
                                      

                    Guid id = Guid.NewGuid();

                    string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", _UploadDir, id.ToString());

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
        }
    }
}
