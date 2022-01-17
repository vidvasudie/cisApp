using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using cisApp.Core;
using cisApp.library;

namespace cisApp.Function
{
    public static class GetUserFavoriteDesigner
    {
        public class Get
        {
            public static List<UserFavoriteDesigner> GetAll()
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.UserFavoriteDesigner.ToList();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static List<JobCandidateModel> GetFavoriteList(Guid userId, int page=1, int limit = 10)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@userId", userId != Guid.Empty ? userId : (object)DBNull.Value),
                       new SqlParameter("@page", page-1),
                       new SqlParameter("@limit", limit)
                    };

                    return StoreProcedure.GetAllStored<JobCandidateModel>("GetFavoriteList", parameter);
                }
                catch (Exception ex)
                {
                    return new List<JobCandidateModel>();
                }
            }


        }

        public class Manage
        {
            public static UserFavoriteDesigner Update(FavoriteModel model)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            UserFavoriteDesigner outp = new UserFavoriteDesigner();
                            var data = context.UserFavoriteDesigner.Where(o => o.UserDesignerId == model.CaUserId && o.UserId == model.UserId);
                            if(!data.Any())
                            {
                                //add favorite if not exist
                                UserFavoriteDesigner obj = new UserFavoriteDesigner();
                               // obj.Id = null; 
                               obj.UserDesignerId = model.CaUserId;
                                obj.UserId = model.UserId;
                                obj.CreatedDate = DateTime.Now;

                                context.UserFavoriteDesigner.Add(obj);
                                outp = obj;
                            }
                            else
                            {
                                //remove favorite if exist
                                context.UserFavoriteDesigner.Remove(data.FirstOrDefault());
                                outp = data.FirstOrDefault();
                            }

                            context.SaveChanges();

                            dbContextTransaction.Commit();

                            return outp;
                        } 
                    }
                }
                catch (Exception ex)
                {
                    //throw ex;
                    return null;
                }
            }


        }

    }
}
