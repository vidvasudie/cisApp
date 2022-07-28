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
            public static List<SystemProblemModel> GetUserHelpModelByID(int id)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@id", id != 0 ? id : (object)DBNull.Value)
                    };

                    return StoreProcedure.GetAllStored<SystemProblemModel>("GetUserHelpModelByID", parameter);
                }
                catch (Exception ex)
                {
                    return new List<SystemProblemModel>();
                }
            }
            public static List<SystemProblemModel> GetUserHelpModels(SearchModel model)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@stext", !String.IsNullOrEmpty(model.text) ? model.text.Trim() : (object)DBNull.Value),
                       new SqlParameter("@status", model.status != 0 ? model.status : (object)DBNull.Value),
                       new SqlParameter("@orderBy", !String.IsNullOrEmpty(model.OrderBy) ? model.OrderBy.Trim() : (object)DBNull.Value),
                       new SqlParameter("@skip", model.currentPage.HasValue ? (model.currentPage-1)*model.pageSize : (object)DBNull.Value),
                       new SqlParameter("@take", model.pageSize.HasValue ? model.pageSize.Value : (object)DBNull.Value)
                    };

                    return StoreProcedure.GetAllStored<SystemProblemModel>("GetUserHelpModels", parameter);
                }
                catch (Exception ex)
                {
                    return new List<SystemProblemModel>();
                }
            }

            public static int GetUserHelpModelsTotal(SearchModel model)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@stext", !String.IsNullOrEmpty(model.text) ? model.text.Trim() : (object)DBNull.Value),
                       new SqlParameter("@type", model.type != null ? model.type : (object)DBNull.Value)
                    };
                    var dt = StoreProcedure.GetAllStoredDataTable("GetUserHelpModelsTotal", parameter);
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

            public static UserHelp Update(int id, string remark, Guid updateBy)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            var data = context.UserHelp.Where(o => o.Id == id).FirstOrDefault();

                            data.Remark = remark;
                            data.Status = 2;
                            data.UpdatedBy = updateBy;
                            data.UpdatedDate = DateTime.Now;

                            context.UserHelp.Update(data);
                            context.SaveChanges();

                            dbContextTransaction.Commit();

                            return data;
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
