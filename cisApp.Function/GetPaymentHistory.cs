using System;
using System.Collections.Generic;
using System.Data;
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
            public static DataTable GetExportPaymentHistory(SearchModel model)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                        new SqlParameter("@userId", model.UserId != null ? model.UserId : (object)DBNull.Value),
                        new SqlParameter("@isPaid", model.IsPaid != null ? model.IsPaid : (object)DBNull.Value),
                        new SqlParameter("@startDate", model.StartDate != null ? model.StartDate.Value : (object)DBNull.Value),
                        new SqlParameter("@endDate", model.EndDate != null ? model.EndDate.Value : (object)DBNull.Value),
                       new SqlParameter("@skip", model.currentPage.HasValue ? (model.currentPage-1)*model.pageSize : (object)DBNull.Value),
                       new SqlParameter("@take", model.pageSize.HasValue ? model.pageSize.Value : (object)DBNull.Value)
                    };

                    return StoreProcedure.GetAllStoredDataTable("GetExportPaymentHistory", parameter);
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
            }
            public static List<PaymentHistoryModel> GetBySearch(SearchModel model)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                        new SqlParameter("@userId", model.UserId != null ? model.UserId : (object)DBNull.Value),
                        new SqlParameter("@isPaid", model.IsPaid != null ? model.IsPaid : (object)DBNull.Value),
                        new SqlParameter("@startDate", model.StartDate != null ? model.StartDate.Value : (object)DBNull.Value),
                        new SqlParameter("@endDate", model.EndDate != null ? model.EndDate.Value : (object)DBNull.Value),
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
                        new SqlParameter("@userId", model.UserId != null ? model.UserId : (object)DBNull.Value),
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

            public static List<PaymentHistoryModel> GetByMonthYear(Guid userId, int? month, int? year)
            {
                try
                {
                    SearchModel model = new SearchModel()
                    {
                        UserId = userId,
                        pageSize = 99999,
                        currentPage = 1
                    };

                    if (month != null && year != null)
                    {
                        DateTime dtStart = new DateTime(year.Value, month.Value, 1);
                        DateTime dtEnd = new DateTime(year.Value, month.Value, DateTime.DaysInMonth(year.Value, month.Value));

                        model.StartDate = dtStart;
                        model.EndDate = dtEnd;
                    }

                    var data = GetBySearch(model);

                    return data;
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
                            obj.IsPaid = data.IsPaid;
                            obj.RefCode = data.RefCode;
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
