﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using cisApp.Core;

namespace cisApp.Function
{
    public static class GetUser
    {
        public class Get
        {
            public static List<Users> GetAll()
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.Users.ToList();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static UserModel GetById(Guid id)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[]
                    {
                        new SqlParameter("@id", id)
                    };

                    var data = StoreProcedure.GetAllStored<UserModel>("GetUserById", parameter);

                    if (data.Count > 0)
                    {
                        return data.FirstOrDefault();
                    }

                    return new UserModel();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            
            public static List<UserModel> GetUserModels(SearchModel model)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@stext", !String.IsNullOrEmpty(model.text) ? model.text : (object)DBNull.Value),
                       new SqlParameter("@skip", model.currentPage.HasValue ? (model.currentPage-1)*model.pageSize : (object)DBNull.Value),
                       new SqlParameter("@take", model.pageSize.HasValue ? model.pageSize.Value : (object)DBNull.Value)
                    };
                     
                    return StoreProcedure.GetAllStored<UserModel>("GetUserModels", parameter); 
                }
                catch (Exception ex)
                {
                    return new List<UserModel>();
                } 
            }

            public static int GetUserModelsTotal(SearchModel model)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@stext", !String.IsNullOrEmpty(model.text) ? model.text : (object)DBNull.Value)
                    };
                    var dt = StoreProcedure.GetAllStoredDataTable("GetUserModelsTotal", parameter);
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
            public static Users Update(UserModel data, Guid userId)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        //using var transaction = context.Database.BeginTransaction();
                        Users obj = new Users();

                        if (data.UserId != null)
                        {
                            obj = context.Users.Find(data.UserId.Value);
                        }
                        else
                        {
                            obj.IsActive = true;
                            obj.CreatedDate = DateTime.Now;
                            obj.CreatedBy = userId;
                        }

                        obj.Fname = data.Fname;
                        obj.Lname = data.Lname;
                        obj.UserType = data.UserType;
                        obj.Tel = data.Tel;
                        obj.Email = data.Email;
                        obj.IsActive = data.IsActive;

                        obj.UpdatedDate = DateTime.Now;
                        obj.UpdatedBy = userId;
                        obj.IsDeleted = false;

                        context.Users.Update(obj);

                        context.SaveChanges();

                        if (obj.UserType != 1)
                        {
                            // user designer 
                            UserDesigner objSub = new UserDesigner();

                            if (data.UserDesignerId != null && data.UserDesignerId != Guid.Empty)
                            {
                                objSub = context.UserDesigner.Find(data.UserDesignerId);
                            }
                            else
                            {
                                // ถ้าเพิ่ม designer จะต้องเพิ่ม log คำขอเป็น designer ที่สำเร็จแล้วด้วย
                            }

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
                        }
                        

                        return obj;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static Users Active(Guid id, bool active, Guid userId)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        Users obj = context.Users.Find(id);

                        obj.IsActive = active;

                        obj.UpdatedDate = DateTime.Now;
                        obj.UpdatedBy = userId;

                        context.Users.Update(obj);

                        context.SaveChanges();

                        return obj;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static Users Delete(Guid id, Guid userId)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        Users obj = context.Users.Find(id);

                        obj.IsDeleted = true;

                        obj.DeletedDate = DateTime.Now;
                        obj.DeletedBy = userId;

                        context.Users.Update(obj);

                        context.SaveChanges();

                        return obj;
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
