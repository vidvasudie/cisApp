using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using cisApp.Core;
using cisApp.library;

namespace cisApp.Function
{
    public static class GetPaymentHistory
    { 
        public class Get
        {
            
            public static List<PaymentHistoryModel> GetBySearch(SearchModel model)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                        new SqlParameter("@userId", model.UserId != null ? model.UserId : (object)DBNull.Value),
                       new SqlParameter("@skip", model.currentPage.HasValue ? (model.currentPage-1)*model.pageSize : (object)DBNull.Value),
                       new SqlParameter("@take", model.pageSize.HasValue ? model.pageSize.Value : (object)DBNull.Value)
                    };

                    return StoreProcedure.GetAllStored<PaymentHistoryModel>("GetPaymentHistory", parameter);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static int GetBySearchTotal(SearchModel model)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                        new SqlParameter("@userId", model.UserId != null ? model.UserId : (object)DBNull.Value)
                    };
                    var dt = StoreProcedure.GetAllStoredDataTable("GetPaymentHistoryTotal", parameter);
                    return (int)dt.Rows[0]["TotalCount"];
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }

            public static PaymentHistory GetById(Guid id)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.PaymentHistory.Where(o => o.PaymentHistoryId == id).FirstOrDefault();
                                                
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
            public static PaymentHistory Update(PaymentHistory data, Guid userId)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            //using var transaction = context.Database.BeginTransaction();
                            PaymentHistory obj = new PaymentHistory();

                            if (data.PaymentHistoryId != null)
                            {
                                obj = context.PaymentHistory.Find(data.PaymentHistoryId);
                            }
                            else
                            {
                                obj.CreatedBy = userId;
                                obj.CreatedDate = DateTime.Now;
                                obj.IsActive = true;
                            }

                            obj.UserId = data.UserId;
                            obj.PaymentDate = data.PaymentDate;
                            obj.Amount = data.Amount;
                            obj.UpdatedBy = userId;
                            obj.UpdatedDate = DateTime.Now;

                            context.PaymentHistory.Update(obj);

                            context.SaveChanges();

                            dbContextTransaction.Commit();

                            return obj;
                        }
                            
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static PaymentHistory Delete(Guid id, Guid userId)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            var obj = context.PaymentHistory.Find(id);

                            obj.DeletedBy = userId;
                            obj.DeletedDate = DateTime.Now;
                            obj.IsDeleted = true;

                            context.PaymentHistory.Update(obj);

                            context.SaveChanges();

                            dbContextTransaction.Commit();

                            return obj;
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
