using cisApp.Core;
using cisApp.library;
using Microsoft.EntityFrameworkCore;
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
                            return 0;

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

            public static UserDesignerRequest Active(UserModel data)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        UserDesignerRequest obj = context.UserDesignerRequest.Where( o => o.Code == data.Code).FirstOrDefault();

                        obj.Status = data.Status;
                        obj.Remark = data.Remark;

                        obj.UpdatedDate = DateTime.Now;
                        obj.UpdatedBy = data.UpdatedBy;

                        context.UserDesignerRequest.Update(obj);

                        context.SaveChanges();

                        return obj;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static int UpdateRequestStatus(UserModel data)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            //update UserType 
                            Users obj = new Users();
                            obj = context.Users.Find(data.UserId.Value);

                            obj.UserType = data.UserType;
                            obj.UpdatedDate = DateTime.Now;
                            obj.UpdatedBy= data.UpdatedBy;

                            context.Users.Update(obj);
                            context.SaveChanges();

                            //add new Designer data
                            UserDesigner objSub = new UserDesigner();
                            //objSub.UserDesignerId = Guid.NewGuid();
                            objSub.UserId = obj.UserId;
                            objSub.PersonalId = data.PersonalId;
                            objSub.BankId = data.BankId;
                            objSub.AccountNumber = data.AccountNumber;
                            objSub.AccountType = data.AccountType;
                            objSub.Address = data.Address;
                            objSub.SubDistrictId = data.SubDistrictId;
                            objSub.DistrictId = data.DistrictId;
                            objSub.ProvinceId = data.ProvinceId;
                            objSub.PostCode = data.PostCode;

                            context.UserDesigner.Update(objSub);
                            context.SaveChanges();

                            //update request status 
                            UserDesignerRequest dsObj = context.UserDesignerRequest.Where(o => o.Code == data.Code).FirstOrDefault();

                            dsObj.Status = data.Status;
                            dsObj.Remark = data.Remark;

                            dsObj.UpdatedDate = DateTime.Now;
                            dsObj.UpdatedBy = data.UpdatedBy;

                            context.UserDesignerRequest.Update(dsObj);

                            var result = context.SaveChanges();

                            dbContextTransaction.Commit();

                            return result;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static int AddNewRequest(UserModel data)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            //add or update User 
                            Users obj = new Users();
                            //var userId = Guid.NewGuid();
                            //obj.UserId = userId;
                            obj.Fname = data.Fname;
                            obj.Lname = data.Lname;
                            obj.UserType = data.UserType;
                            obj.Tel = data.Tel;
                            obj.Email = data.Email;
                            //obj.IsActive = data.IsActive;
                            obj.IsActive = true;
                            obj.CreatedDate = DateTime.Now;
                            obj.CreatedBy = data.CreatedBy;
                            obj.UpdatedDate = DateTime.Now;
                            obj.UpdatedBy = data.UpdatedBy;
                            obj.IsDeleted = false;

                            context.Users.Update(obj);
                            context.SaveChanges();

                            //add new Request Designer data 
                            var dataList = context.UserDesignerRequest.ToList().OrderBy(o => o.Id).LastOrDefault();
                            string code = Utility.GenerateRequestCode(Int32.Parse(dataList.Code.Substring(7, 4))+1);
                            UserDesignerRequest objSub = new UserDesignerRequest();
                            objSub.Code = code;
                            objSub.UserId = obj.UserId;
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

                            objSub.CreatedDate = DateTime.Now;
                            objSub.CreatedBy = data.CreatedBy;
                            objSub.UpdatedDate = DateTime.Now;
                            objSub.UpdatedBy = data.UpdatedBy;

                            context.UserDesignerRequest.Update(objSub);
                              
                            var result = context.SaveChanges();

                            dbContextTransaction.Commit();

                            return result;
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
