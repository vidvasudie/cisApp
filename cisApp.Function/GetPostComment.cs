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
            //public static PostLike GetByUserIdAndRefId(Guid userId, Guid refId)
            //{
            //    try
            //    {
            //        SqlParameter[] parameter = new SqlParameter[]
            //        {
            //            new SqlParameter("@userId", userId),
            //            new SqlParameter("@refId", refId)
            //        };

            //        var data = StoreProcedure.GetAllStored<PostLike>("GetPostLikeByUserAndRefId", parameter);

            //        if (data.Count > 0)
            //        {
            //            return data.FirstOrDefault();
            //        }

            //        return null;
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}
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
                            ImgId = null
                        };

                        context.PostComment.Add(obj);

                        context.SaveChanges();

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
