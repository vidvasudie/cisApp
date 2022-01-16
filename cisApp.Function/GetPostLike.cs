using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using cisApp.Core;

namespace cisApp.Function
{
    public static class GetPostLike
    {
        public class Get
        {
            public static PostLike GetByUserIdAndRefId(Guid userId, Guid refId)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[]
                    {
                        new SqlParameter("@userId", userId),
                        new SqlParameter("@refId", refId)
                    };

                    var data = StoreProcedure.GetAllStored<PostLike>("GetPostLikeByUserAndRefId", parameter);

                    if (data.Count > 0)
                    {
                        return data.FirstOrDefault();
                    }

                    return null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public class Manage
        {
            public static PostLike Update(Guid userId, Guid refId, bool IsActive)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        if (IsActive)
                        {
                            // เช็คซ้ำ
                            var like = Get.GetByUserIdAndRefId(userId, refId);
                            if (like != null)
                            {
                                // มีแล้วไม่ต้องทำอะไร
                                return like;
                            }

                            // ยังไม่มี insert ใหม่
                            PostLike obj = new PostLike()
                            {
                                RefId = refId,
                                UserId = userId,
                                CreatedDate = DateTime.Now
                            };

                            context.PostLike.Add(obj);

                            context.SaveChanges();

                            return obj;
                        }
                        else
                        {
                            // เช็คซ้ำ
                            var like = Get.GetByUserIdAndRefId(userId, refId);
                            if (like == null)
                            {
                                // ไม่มีไม่ต้องลบอะไร
                                return like;
                            }

                            context.PostLike.Remove(like);

                            context.SaveChanges();

                            return like;
                        }
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
