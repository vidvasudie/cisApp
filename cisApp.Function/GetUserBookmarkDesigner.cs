using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using cisApp.Core;
using cisApp.library;

namespace cisApp.Function
{
    public static class GetUserBookmarkDesigner
    {
        public class Get
        {
            public static List<UserBookmarkDesigner> GetAll()
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.UserBookmarkDesigner.ToList();

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
            public static UserBookmarkDesigner Update(FavoriteModel model)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            UserBookmarkDesigner outp = new UserBookmarkDesigner();
                            var data = context.UserBookmarkDesigner.Where(o => o.UserDesignerId == model.CaUserId && o.UserId == model.UserId);
                            if(!data.Any())
                            {
                                //add favorite if not exist
                                UserBookmarkDesigner obj = new UserBookmarkDesigner();
                                // obj.Id = null; 
                                obj.UserDesignerId = model.CaUserId;
                                obj.UserId = model.UserId;
                                obj.CreatedDate = DateTime.Now;

                                context.UserBookmarkDesigner.Add(obj);
                                outp = obj;
                            }
                            else
                            {
                                //remove favorite if exist
                                context.UserBookmarkDesigner.Remove(data.FirstOrDefault());
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
