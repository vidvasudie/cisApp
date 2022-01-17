using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using cisApp.Core;
using cisApp.library;

namespace cisApp.Function
{
    public static class GetFaq
    {
        public class Get
        {
            public static List<Faq> GetAll()
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.Faq.ToList();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public static List<Faq> GetActive()
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.Faq.Where(o => o.IsActive == true && o.IsDeleted == false).OrderBy(o => o.Qorder).ToList();

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
            public static Faq Update(Faq model)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            //Faq faq = new Faq();
                            //var data = context.UserBookmarkDesigner.Where(o => o.UserDesignerId == model.CaUserId && o.UserId == model.UserId);
                            //if(!data.Any())
                            //{
                            //    //add favorite if not exist
                            //    UserBookmarkDesigner obj = new UserBookmarkDesigner();
                            //    // obj.Id = null; 
                            //    obj.UserDesignerId = model.CaUserId;
                            //    obj.UserId = model.UserId;
                            //    obj.CreatedDate = DateTime.Now;

                            //    context.UserBookmarkDesigner.Add(obj);
                            //    outp = obj;
                            //}
                            //else
                            //{
                            //    //remove favorite if exist
                            //    context.UserBookmarkDesigner.Remove(data.FirstOrDefault());
                            //    outp = data.FirstOrDefault();
                            //}

                            //context.SaveChanges();

                            dbContextTransaction.Commit();

                            return model;
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
