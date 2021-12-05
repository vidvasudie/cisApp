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
                        var data = context.UserDesigner.Where(o => o.UserId == userid).FirstOrDefault();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
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
        }
    }
}
