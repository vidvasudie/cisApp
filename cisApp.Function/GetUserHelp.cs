using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using cisApp.Core;
using cisApp.library;

namespace cisApp.Function
{
    public static class GetUserHelp
    {
        public class Get
        {
            public static List<UserHelp> GetAll()
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.UserHelp.ToList();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public static List<UserHelp> GetByStatus(int status)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.UserHelp.Where(o => o.Status == status).ToList();

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
            public static UserHelp Add(string tel, string email, string message)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            UserHelp model = new UserHelp();
                            model.Message = message;
                            model.Email = email;
                            model.Tel = tel;
                            model.CreatedDate = DateTime.Now;
                            model.Status = 1;
                            
                            context.UserHelp.Add(model);
                            context.SaveChanges();

                            dbContextTransaction.Commit();

                            return model;
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
