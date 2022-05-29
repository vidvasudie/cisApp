using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using cisApp.Core;
using cisApp.library;

namespace cisApp.Function
{
    public static class GetPaymentHistoryDate
    { 
        public class Get
        {
            
            

            public static PaymentHistoryDate GetDefault()
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.PaymentHistoryDate.FirstOrDefault();
                                                
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
            public static PaymentHistoryDate Update(int day, Guid userId)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            //using var transaction = context.Database.BeginTransaction();
                            PaymentHistoryDate obj = new PaymentHistoryDate();

                            obj = context.PaymentHistoryDate.FirstOrDefault();

                            if (obj == null) obj = new PaymentHistoryDate();

                            obj.Day = day;
                            obj.UpdatedBy = userId;
                            obj.UpdatedDate = DateTime.Now;

                            context.PaymentHistoryDate.Update(obj);

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
