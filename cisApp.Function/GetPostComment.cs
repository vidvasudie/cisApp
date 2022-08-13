using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using cisApp.Core;

namespace cisApp.Function
{
    public static class GetPostComment
    {
        public class Get
        {
            public static List<PostComment> GetByRefId(Guid refId, int page = 1, int limit = 10)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[]
                    {                        
                        new SqlParameter("@refId", refId),
                        new SqlParameter("@skip", (page-1)*limit),
                        new SqlParameter("@take", limit)
                    };

                    var data = StoreProcedure.GetAllStored<PostComment>("GetPostCommentByRefID", parameter);
                                        

                    return data;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static int GetByRefIdTotal(Guid refId)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[]
                    {
                        new SqlParameter("@refId", refId)
                    };

                    var dt = StoreProcedure.GetAllStoredDataTable("GetPostCommentByRefIDTotal", parameter);
                    return (int)dt.Rows[0]["TotalCount"];
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public class Manage
        {
            public static PostComment Add(Guid userId, Guid refId, string comment)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        PostComment obj = new PostComment()
                        {
                            RefId = refId,
                            UserId = userId,
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            UpdatedBy = userId,
                            Comment = comment,
                            ImgId = null,
                            IsActive = true
                        };

                        context.PostComment.Add(obj);

                        context.SaveChanges();

                        try
                        {
                            // notify user
                            var attachFile = context.AttachFile.Find(refId);
                            if (attachFile != null)
                            {
                                var albumImage = context.AlbumImage.Where(o => o.ImgId == attachFile.AttachFileId).FirstOrDefault();

                                var album = context.Album.Find(albumImage.AlbumId);

                                var user = GetUser.Get.GetById(userId);

                                new MobileNotfication().Fordesigner(MobileNotfication.ModeDesigner.comment, album.UserId.Value, null, user.FullName);
                            }
                        }
                        catch (Exception ex)
                        {

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
