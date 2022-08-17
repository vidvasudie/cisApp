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
                        var data = context.Faq.Where(o => o.IsDeleted == false).OrderBy(o => o.Qorder).ToList();

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

            public static Faq GetById(int id)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.Faq.Find(id);

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


            public static int GetLowestOrder()
            {
                using (var context = new CAppContext())
                {
                    var obj = context.Faq.Where(o => o.IsDeleted == false).OrderByDescending(o => o.Qorder).FirstOrDefault();

                    if (obj != null)
                    {
                        return obj.Qorder;
                    }

                    return 0;
                }
            }

            public static int GetHighestOrder()
            {
                using (var context = new CAppContext())
                {
                    var obj = context.Faq.Where(o => o.IsDeleted == false).OrderByDescending(o => o.Qorder).FirstOrDefault();

                    if (obj != null)
                    {
                        return obj.Qorder;
                    }

                    return 0;
                }
            }

        }

        public class Manage
        {
            public static Faq Update(Faq model, Guid userId)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {

                            Faq obj;

                            if (model.Id != null)
                            {
                                obj = context.Faq.Find(model.Id);
                                //obj.IsActive = model.IsActive;
                            }
                            else
                            {
                                obj = new Faq()
                                {
                                    IsActive = true,
                                    IsDeleted = false,
                                    CreatedBy = userId,
                                    CreatedDate = DateTime.Now,
                                    Qorder = Get.GetHighestOrder() + 1
                                };
                            }

                            obj.Question = model.Question;
                            obj.Answer = model.Answer;
                            obj.UpdatedBy = userId;
                            obj.UpdatedDate = DateTime.Now;

                            

                            context.Faq.Update(obj);

                            context.SaveChanges();

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

            public static Faq Delete(int id, Guid userId)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {

                            var obj = context.Faq.Find(id);

                            obj.IsDeleted = true;
                            obj.DeletedBy = userId;
                            obj.DeletedDate = DateTime.Now;


                            context.Faq.Update(obj);
                            context.SaveChanges();

                            dbContextTransaction.Commit();

                            return obj;
                        }
                    }
                }
                catch (Exception ex)
                {
                    //throw ex;
                    return null;
                }
            }

            public static void OrderUp(int id)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {

                            var obj = context.Faq.Find(id);

                            // min 1
                            if (obj.Qorder == 1)
                            {
                                return;
                            }

                            var previous = context.Faq.Where(o => o.IsDeleted == false && o.Qorder == (obj.Qorder - 1)).FirstOrDefault();

                            if (previous != null)
                            {
                                // need to swap
                                int tmpOrder = obj.Qorder;
                                obj.Qorder = previous.Qorder;
                                previous.Qorder = tmpOrder;

                                context.Faq.Update(obj);
                                context.Faq.Update(previous);
                            }
                            else
                            {
                                obj.Qorder = obj.Qorder - 1;
                                context.Faq.Update(obj);
                            }
                            context.SaveChanges();

                            dbContextTransaction.Commit();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static void OrderDown(int id)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {

                            var obj = context.Faq.Find(id);

                            // min 1
                            if (obj.Qorder == 1)
                            {
                                return;
                            }

                            var next = context.Faq.Where(o => o.IsDeleted == false && o.Qorder == (obj.Qorder + 1)).FirstOrDefault();

                            if (next != null)
                            {
                                // need to swap
                                int tmpOrder = obj.Qorder;
                                obj.Qorder = next.Qorder;
                                next.Qorder = tmpOrder;

                                context.Faq.Update(obj);
                                context.Faq.Update(next);
                            }
                            else
                            {
                                obj.Qorder = obj.Qorder + 1;
                                context.Faq.Update(obj);
                            }
                            context.SaveChanges();

                            dbContextTransaction.Commit();
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
