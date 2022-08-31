using cisApp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cisApp.Function
{
    public static class GetSettings
    {
        public class Get
        {
            public static List<Settings> GetAll(string domainUrl = "")
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.Settings.ToList();

                        foreach (var item in data)
                        {
                            if (item.IsImg == true)
                            {
                                item.AttachFileImage = GetAttachFile.Get.GetByRefId(item.SettingId.Value);
                                item.FullUrlPath = domainUrl + item.UrlPath;
                            }
                        }

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static Settings GetById(Guid id, string domainUrl = "")
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.Settings.Find(id);

                        if (data.IsImg == true)
                        {
                            data.AttachFileImage = GetAttachFile.Get.GetByRefId(data.SettingId.Value);
                            data.FullUrlPath = domainUrl + data.UrlPath;
                        }

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static Settings GetByKeyword(string keyword, string domainUrl = "")
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.Settings.Where(o => o.Keyword.ToLower() == keyword.ToLower()).FirstOrDefault();

                        if (data.IsImg == true)
                        {
                            data.AttachFileImage = GetAttachFile.Get.GetByRefId(data.SettingId.Value);
                            data.FullUrlPath = domainUrl + data.UrlPath;
                        }

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
            public static Settings Update(Settings data, Guid userId)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        // only update content
                        var obj = context.Settings.Find(data.SettingId.Value);

                        if (obj == null )
                        {
                            throw new Exception("Object not found exception");
                        }

                        obj.Content = data.Content;
                        obj.UpdatedBy = userId;
                        obj.UpdatedDate = DateTime.Now;

                        context.Settings.Update(obj);

                        context.SaveChanges();

                        if (obj.IsImg == true)
                        {
                            // save profile
                            if (!String.IsNullOrEmpty(data.FileBase64)) // ถ้ามีไฟล์อัพมาใหม่ fileBase64 จะมีค่า
                            {
                                // remove previous img
                                var activeImg = GetAttachFile.Get.GetByRefId(obj.SettingId.Value);

                                if (activeImg != null)
                                {
                                    GetAttachFile.Manage.EditFile(data.FileBase64, data.FileName, Convert.ToInt32(data.FileSize), activeImg.AttachFileId, userId);
                                }
                                else
                                {
                                    GetAttachFile.Manage.UploadFile(data.FileBase64, data.FileName, Convert.ToInt32(data.FileSize), obj.SettingId, userId);
                                }
                            }
                            else if (data.FileRemove) // ถ้าลบไฟล์ออก แล้วไม่ได้อัพไฟล์ใหม่ขึ้นมาจะเข้า เงื่อนไขนี้
                            {
                                var activeImg = GetAttachFile.Get.GetByRefId(obj.SettingId.Value);

                                GetAttachFile.Manage.UpdateStatusByRefId(obj.SettingId.Value, false, userId);
                            }
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
