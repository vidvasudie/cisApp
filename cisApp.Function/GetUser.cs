using System;
using System.Collections.Generic;
using System.Linq;
using cisApp.Core;

namespace cisApp.Function
{
    public static class GetUser
    {
        public class Get
        {
            public static List<Users> GetAll()
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.Users.ToList();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static Users GetById(Guid id)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.Users.Find(id);

                        if (data == null) return new Users();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static List<UserDesigner> GetDesignerAll()
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.UserDesigner.ToList();

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
            public static Users Update(Users data)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        //using var transaction = context.Database.BeginTransaction();
                        Users obj = new Users();

                        if (data.UserId != null)
                        {
                            obj = context.Users.Find(data.UserId.Value);
                        }
                        else
                        {
                            obj.CreatedDate = DateTime.Now;
                        }

                        obj.Fname = data.Fname;
                        obj.Lname = data.Lname;
                        obj.UserType = data.UserType;
                        obj.Tel = data.Tel;
                        obj.Email = data.Email;
                        obj.IsActive = data.IsActive;

                        obj.UpdatedDate = DateTime.Now;
                        obj.IsDeleted = false;

                        context.Users.Update(obj);

                        context.SaveChanges();

                        return obj;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static Users Active(Guid id, bool active)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        Users obj = context.Users.Find(id);

                        obj.IsActive = active;

                        obj.UpdatedDate = DateTime.Now;

                        context.Users.Update(obj);

                        context.SaveChanges();

                        return obj;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static Users Delete(Guid id)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        Users obj = context.Users.Find(id);

                        obj.IsDeleted = true;

                        obj.DeletedDate = DateTime.Now;

                        context.Users.Update(obj);

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
