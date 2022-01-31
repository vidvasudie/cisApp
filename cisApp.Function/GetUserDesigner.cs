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
            public static DesignerProfileModel GetDesignerProfile(Guid userDesignerId, Guid userId)
            {
                try
                {
                    if (userDesignerId == Guid.Empty)
                    {
                        return null;
                    }
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@userDesignerId", userDesignerId),
                       new SqlParameter("@userId", userId)
                    };
                    var data = StoreProcedure.GetAllStored<DesignerProfileModel>("GetDesignerProfile", parameter);
                    if (data.Any())
                        return data.FirstOrDefault();
                    return null;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            public static List<AlbumModel> GetDesignerAlbumImage(DesignerJobListSearch model)
            {
                try
                { 
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@userId", model.userId),
                       new SqlParameter("@skip", model.skip),
                       new SqlParameter("@take", model.take)
                    };
                    var data = StoreProcedure.GetAllStored<AlbumModel>("GetDesignerAlbumImage", parameter);
                    if (data.Any())
                        return data.ToList();
                    return null;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            public static List<DesignerContestSummary> GetContestSummary(Guid userId)
            {
                try
                {
                    if (userId == Guid.Empty)
                    {
                        return null;
                    }
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@userId", userId)
                    };
                    var data = StoreProcedure.GetAllStored<DesignerContestSummary>("GetDesignerContestSummary", parameter);
                    if (data.Any())
                        return data.ToList();
                    return null;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            public static List<DesignerJobsHistoryModel> GetJobsHistory(DesignerJobListSearch model)
            {
                try
                {
                    if (model.userId == Guid.Empty)
                    {
                        return null;
                    }
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@userId", model.userId),
                       new SqlParameter("@skip", model.skip),
                       new SqlParameter("@take", model.take)
                    };
                    var data = StoreProcedure.GetAllStored<DesignerJobsHistoryModel>("GetJobDesignerHistory", parameter);
                    if (data.Any())
                        return data.ToList();
                    return null;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            public static List<DesignerListReviewModel> GetReview(DesignerJobListSearch model)
            {
                try
                {
                    if (model.userId == Guid.Empty && model.jobId == Guid.Empty)
                    {
                        return null;
                    }
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@userId", model.userId == Guid.Empty ? (object)DBNull.Value : model.userId),
                       new SqlParameter("@jobId", model.jobId == Guid.Empty ? (object)DBNull.Value : model.jobId),
                       new SqlParameter("@skip", model.skip),
                       new SqlParameter("@take", model.take)
                    };
                    var data = StoreProcedure.GetAllStored<DesignerListReviewModel>("GetDesignerReviewList", parameter);
                    if (data.Any())
                        return data.ToList();
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

            public static List<DesignerJobListModel> GetJobListSearch(DesignerJobListSearch model, bool IsSubmit = false)
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
                       new SqlParameter("@submitList", IsSubmit ? 1 : 0),
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
            public static List<DesignerJobListModel> GetJobDetailValid(Guid userId, Guid jobId)
            {
                try
                {
                    if (Guid.Empty == userId || Guid.Empty == jobId)
                    {
                        return new List<DesignerJobListModel>();
                    }
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@userId", userId),
                       new SqlParameter("@jobId", jobId)
                    };

                    return StoreProcedure.GetAllStored<DesignerJobListModel>("GetJobDetailValidate", parameter);
                }
                catch (Exception ex)
                {
                    return new List<DesignerJobListModel>();
                }
            }
            public static List<DesignerJobListModel> GetJobContestList(DesignerJobListSearch model)
            {
                try
                {
                    if (Guid.Empty == model.userId)
                    {
                        return new List<DesignerJobListModel>();
                    }
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@userId", model.userId),
                       new SqlParameter("@jobId", model.jobId != Guid.Empty ? model.jobId : (object)DBNull.Value),
                       new SqlParameter("@skip", model.skip),
                       new SqlParameter("@take", model.take)
                    };

                    return StoreProcedure.GetAllStored<DesignerJobListModel>("GetDesignerJobContestList", parameter);
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
                            var obj = context.UserDesigner.Where(o => o.UserId == model.UserId).FirstOrDefault();
                            if (obj == null)
                            {
                                return null;
                            }
                            data = obj;
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
