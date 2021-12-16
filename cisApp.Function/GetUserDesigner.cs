using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using cisApp.Core;

namespace cisApp.Function
{
    public static class GetUserDesigner
    {
        public class Get
        {
            public static List<UserDesigner> GetAll()
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
            public static UserDesigner GetById(Guid id)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.UserDesigner.Where(o => o.UserDesignerId == id).FirstOrDefault();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public static UserDesigner GetByUserId(Guid userid)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.UserDesigner.Where(o => o.UserId == userid);
                        if(data.Any())
                            return data.FirstOrDefault();

                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public static ProfileBankModel GetBankProfile(Guid userId)
            {
                try
                {
                    if(userId == Guid.Empty)
                    {
                        return null;
                    }
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@userId", userId)
                    };
                    var data = StoreProcedure.GetAllStored<ProfileBankModel>("GetBankProfile", parameter);
                    if (data.Any())
                        return data.FirstOrDefault();
                    return null;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

            public static List<UserModel> GetDesignerItems(SearchModel model)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@stext", !String.IsNullOrEmpty(model.text) ? model.text.Trim() : (object)DBNull.Value),
                       new SqlParameter("@skip", model.currentPage.HasValue ? (model.currentPage-1)*model.pageSize : (object)DBNull.Value),
                       new SqlParameter("@take", model.pageSize.HasValue ? model.pageSize.Value : (object)DBNull.Value)
                    };

                    return StoreProcedure.GetAllStored<UserModel>("GetDesigner", parameter);
                }
                catch (Exception ex)
                {
                    return new List<UserModel>();
                }
            }
            public static int GetDesignerItemsTotal(SearchModel model)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@stext", !String.IsNullOrEmpty(model.text) ? model.text.Trim() : (object)DBNull.Value)
                    };
                    var dt = StoreProcedure.GetAllStoredDataTable("GetDesignerTotal", parameter);
                    return (int)dt.Rows[0]["TotalCount"];
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }

            public static List<DesignerJobListModel> GetJobListSearch(DesignerJobListSearch model)
            {
                try
                {
                    if (Guid.Empty == model.userId)
                    {
                        return new List<DesignerJobListModel>();
                    }
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@userId", model.userId),
                       new SqlParameter("@jobTypeName", !String.IsNullOrEmpty(model.text) ? model.text.Trim() : (object)DBNull.Value),
                       new SqlParameter("@skip", model.skip),
                       new SqlParameter("@take", model.take)
                    };

                    return StoreProcedure.GetAllStored<DesignerJobListModel>("GetJobListByDesignerSearch", parameter);
                }
                catch (Exception ex)
                {
                    return new List<DesignerJobListModel>();
                }
            }

        }
        public class Manage
        {
            public static UserDesigner UpdateBankProfile(BankModel model)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            UserDesigner data = new UserDesigner();
                            if (model.UserId == Guid.Empty)
                            {
                                return null;
                            }
                            var objs = context.UserDesigner.Where(o => o.UserId == model.UserId);
                            if (!objs.Any())
                            {
                                return null;
                            }
                            data = objs.FirstOrDefault();
                            data.BankId = model.BankId;
                            data.AccountNumber = model.AccountNumber;
                            data.AccountType = model.AccountTypeId;
                            
                            context.UserDesigner.Update(data);
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
