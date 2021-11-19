using cisApp.Core;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace cisApp.Function
{
    public class GetUserDesignerRequest
    {
        public class Get
        {
            public static int GetLastNumber()
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.UserDesignerRequest.ToList().OrderBy(o => o.Id).LastOrDefault();
                        if (data == null)
                            return 1;

                        return Int32.Parse(data.Code.Substring(7, 4));
                    }
                    
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static List<UserModel> GetUserDesignerRequestModel(SearchModel model)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@stext", !String.IsNullOrEmpty(model.text) ? model.text : (object)DBNull.Value),
                       new SqlParameter("@status", model.status),
                       new SqlParameter("@skip", model.currentPage.HasValue ? (model.currentPage-1)*model.pageSize : (object)DBNull.Value),
                       new SqlParameter("@take", model.pageSize.HasValue ? model.pageSize.Value : (object)DBNull.Value)
                    };

                    return StoreProcedure.GetAllStored<UserModel>("GetUserDesignerRequest", parameter);
                }
                catch (Exception ex)
                {
                    return new List<UserModel>();
                }
            }

            public static int GetUserDesignerRequestModelTotal(SearchModel model)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@stext", !String.IsNullOrEmpty(model.text) ? model.text : (object)DBNull.Value),
                       new SqlParameter("@status", model.status)
                    };
                    var dt = StoreProcedure.GetAllStoredDataTable("GetUserDesignerRequestTotal", parameter);
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
            public static UserDesignerRequest Update(UserModel data)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        //using var transaction = context.Database.BeginTransaction();
                       
                        UserDesignerRequest objSub = new UserDesignerRequest();
                        var obj = context.UserDesignerRequest.Where(o => o.Code == data.Code).FirstOrDefault();
                        if (obj != null)
                            objSub = obj;

                        objSub.Code = data.Code;
                        objSub.UserId = data.UserId;
                        objSub.PersonalId = data.PersonalId;
                        objSub.BankId = data.BankId;
                        objSub.AccountNumber = data.AccountNumber;
                        objSub.AccountType = data.AccountType;
                        objSub.Address = data.Address;
                        objSub.SubDistrictId = data.SubDistrictId;
                        objSub.DistrictId = data.DistrictId;
                        objSub.ProvinceId = data.ProvinceId;
                        objSub.PostCode = data.PostCode;
                        objSub.Status = data.Status;
                        objSub.Remark = data.Remark;

                        if (obj != null)
                            context.UserDesignerRequest.Update(objSub);
                        else
                            context.UserDesignerRequest.Add(objSub);

                        context.SaveChanges();
                         
                        return objSub;
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
