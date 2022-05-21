using cisApp.Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace cisApp.Function
{
    public static class GetUserBookmarkImage
    {

        public static IConfigurationRoot _config = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("appsettings.json")
                                    .Build();
        public class Get
        {
            public static List<AlbumImageModel> Search(Guid userId, int currentPage = 1, int pageSize = 10)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@userId", userId),
                       new SqlParameter("@skip", (currentPage-1)*pageSize),
                       new SqlParameter("@take", pageSize)
                    };

                    var bookmarks = StoreProcedure.GetAllStored<UserBookmarkImage>("GetUserBookmarkImage", parameter);

                    string webAdmin = _config.GetSection("WebConfig:AdminWebStie").Value;
                    List<AlbumImageModel> data = new List<AlbumImageModel>();

                    foreach (var item in bookmarks)
                    {
                        var image = GetAlbum.Get.GetAlbumImageByAttachId(webAdmin, item.RefId);

                        data.Add(image);
                    }

                    return data;
                }
                catch (Exception ex)
                {
                    return new List<AlbumImageModel>();
                }
            }

            public static int Total(Guid userId)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@userId", userId)
                    };
                    var dt = StoreProcedure.GetAllStoredDataTable("GetUserBookmarkImageTotal", parameter);
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
            public static void Add(Guid userId, Guid refId)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var obj = context.UserBookmarkImage.Where(o => o.IsDeleted == false && o.UserId == userId && o.RefId == refId).FirstOrDefault();

                        if (obj != null)
                        {
                            obj.UpdatedBy = userId;
                            obj.UpdatedDate = DateTime.Now;
                            context.UserBookmarkImage.Update(obj);
                        }
                        else
                        {
                            obj = new UserBookmarkImage()
                            {
                                UserId = userId,
                                RefId = refId,
                                IsActive = true,
                                IsDeleted = false,
                                CreatedBy = userId,
                                CreatedDate = DateTime.Now,
                                UpdatedBy = userId,
                                UpdatedDate = DateTime.Now
                            };

                            context.UserBookmarkImage.Add(obj);
                        }

                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static void Delete(Guid userId, Guid refId)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var obj = context.UserBookmarkImage.Where(o => o.IsDeleted == false && o.UserId == userId && o.RefId == refId).FirstOrDefault();

                        if (obj != null)
                        {
                            obj.DeletedBy = userId;
                            obj.DeletedDate = DateTime.Now;
                            obj.IsDeleted = true;
                            context.UserBookmarkImage.Update(obj);
                        }                        

                        context.SaveChanges();
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
