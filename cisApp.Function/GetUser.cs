using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using cisApp.Core;
using cisApp.library;

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
                        // get payment_img id
                        var userImg = Get.GetUserImgs(data.FirstOrDefault().UserId.Value);

                        if (userImg.Count > 0)
                        {
                            data.FirstOrDefault().AttachFileImage = GetAttachFile.Get.GetByRefId(userImg.FirstOrDefault().UserImgId.Value);
                        }
                        //data.FirstOrDefault().AttachFileImage = GetAttachFile.Get.GetByRefId(data.FirstOrDefault().UserId.Value);
                        return data.FirstOrDefault();
                    }

                    return new UserModel();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static UserModel GetByEmail(string email)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[]
                    {
                        new SqlParameter("@stext", email)
                    };

                    var data = StoreProcedure.GetAllStored<UserModel>("GetUserByEmail", parameter);

                    return data.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static List<Users> GetCutomerActive()
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.Users.Where(o => o.IsActive == true && o.IsDeleted == false && o.UserType == 1).ToList();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static bool IsEmailAlreadyUseInsert(string email)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[]
                    {
                        new SqlParameter("@stext", email)
                    };

                    var data = StoreProcedure.GetAllStored<UserModel>("GetUserByEmail", parameter);

                    if (data.Count > 0)
                    {
                        return false;
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static bool IsEmailAlreadyUseUpdate(string email)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[]
                    {
                        new SqlParameter("@stext", email)
                    };

                    var data = StoreProcedure.GetAllStored<UserModel>("GetUserByEmail", parameter);

                    if (data.Count > 1)
                    {
                        return false;
                    }

                    return true;
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
                       new SqlParameter("@stext", !String.IsNullOrEmpty(model.text) ? model.text.Trim() : (object)DBNull.Value),
                       new SqlParameter("@type", model.type != null ? model.type : (object)DBNull.Value),
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
                       new SqlParameter("@stext", !String.IsNullOrEmpty(model.text) ? model.text.Trim() : (object)DBNull.Value),
                       new SqlParameter("@type", model.type != null ? model.type : (object)DBNull.Value)
                    };
                    var dt = StoreProcedure.GetAllStoredDataTable("GetUserModelsTotal", parameter);
                    return (int)dt.Rows[0]["TotalCount"];
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }

            public static List<UserModel> GetUserLogin(LoginModel model)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@username", !String.IsNullOrEmpty(model.username) ? model.username.Trim() : (object)DBNull.Value),
                       new SqlParameter("@password", !String.IsNullOrEmpty(model.password) ? Encryption.Encrypt(model.password.Trim()) : (object)DBNull.Value),
                       new SqlParameter("@usertype", model.userType == null ? 1 : model.userType)
                    };

                    return StoreProcedure.GetAllStored<UserModel>("GetUserLogin", parameter);
                }
                catch (Exception ex)
                {
                    return new List<UserModel>();
                }
            }

            public static List<UserImg> GetUserImgs(Guid userId)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.UserImg.Where(o => o.UserId == userId).ToList();

                        data = (from p in data
                                join a in context.AttachFile on p.UserImgId equals a.RefId
                                where a.IsActive == true
                                select p).ToList();

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
            public static Users Update(UserModel data, Guid? userId = null, string newPassword = null)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            //using var transaction = context.Database.BeginTransaction();
                            Users obj = new Users();
                            UsersPassword usersPassword = null;

                            if (data.UserId != null)
                            {
                                obj = context.Users.Find(data.UserId.Value);
                                if (!Get.IsEmailAlreadyUseUpdate(data.Email))
                                {
                                    throw new Exception("Email ดังกล่าวถูกใช้งานไปแล้ว");
                                }
                            }
                            else
                            {
                                obj.IsActive = true;
                                obj.CreatedDate = DateTime.Now;
                                obj.CreatedBy = userId;

                                if (!Get.IsEmailAlreadyUseInsert(data.Email))
                                {
                                    throw new Exception("Email ดังกล่าวถูกใช้งานไปแล้ว");
                                }

                                // in case insert we need insert new password

                                if (string.IsNullOrEmpty(newPassword))
                                {
                                    newPassword = Utility.RandomPassword();
                                }


                                newPassword = Encryption.Encrypt(newPassword);
                                usersPassword = new UsersPassword()
                                {
                                    Password = newPassword
                                };



                                context.UsersPassword.Update(usersPassword);

                                context.SaveChanges();

                                obj.PasswordId = usersPassword.PasswordId.Value;

                            }

                            obj.Fname = data.Fname;
                            obj.Lname = data.Lname;
                            obj.UserType = data.UserType;
                            obj.Tel = data.Tel;
                            obj.Email = data.Email;
                            obj.RoleId = data.RoleId;
                            //obj.IsActive = data.IsActive;

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
                                else if (data.UserId != null && data.UserId != Guid.Empty)
                                {
                                    objSub = context.UserDesigner.Where(o => o.UserId == data.UserId).FirstOrDefault();
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

                            // save profile
                            if (!String.IsNullOrEmpty(data.FileBase64)) // ถ้ามีไฟล์อัพมาใหม่ fileBase64 จะมีค่า
                            {
                                // remove previous img
                                var activeUserImg = Get.GetUserImgs(obj.UserId.Value);

                                if (activeUserImg.Count > 0)
                                {
                                    foreach (var item in activeUserImg)
                                    {
                                        GetAttachFile.Manage.UpdateStatusByRefId(item.UserImgId.Value, false, userId.Value);
                                    }
                                }

                                // insert new UserImg
                                UserImg userImg = new UserImg()
                                {
                                    UserId = obj.UserId.Value
                                };

                                context.UserImg.Update(userImg);

                                context.SaveChanges();

                                GetAttachFile.Manage.UploadFile(data.FileBase64, data.FileName, Convert.ToInt32(data.FileSize), userImg.UserImgId.Value, userId.Value);

                                obj.UserImgId = userImg.UserImgId;

                                context.Users.Update(obj);

                                context.SaveChanges();
                            }
                            else if (data.FileRemove) // ถ้าลบไฟล์ออก แล้วไม่ได้อัพไฟล์ใหม่ขึ้นมาจะเข้า เงื่อนไขนี้
                            {
                                var userImg = context.UserImg.Where(o => o.UserId == obj.UserId.Value).FirstOrDefault();

                                GetAttachFile.Manage.UpdateStatusByRefId(userImg.UserImgId.Value, false, userId.Value);
                            }

                            if (usersPassword != null)
                            {
                                usersPassword.UserId = obj.UserId;

                                context.UsersPassword.Update(usersPassword);

                                context.SaveChanges();
                            }

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

            public static Users UpdateProfile(AttachFile data, Guid id, Guid? userId = null)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            //using var transaction = context.Database.BeginTransaction();
                            Users obj = new Users();

                            obj = context.Users.Find(id);

                            // remove previous img
                            var activeUserImg = Get.GetUserImgs(obj.UserId.Value);

                            if (activeUserImg.Count > 0)
                            {
                                foreach (var item in activeUserImg)
                                {
                                    GetAttachFile.Manage.UpdateStatusByRefId(item.UserImgId.Value, false, userId.Value);
                                }
                            }

                            // insert new UserImg
                            UserImg userImg = new UserImg()
                            {
                                UserId = obj.UserId.Value
                            };

                            context.UserImg.Update(userImg);

                            context.SaveChanges();

                            data.RefId = userImg.UserImgId;

                            context.AttachFile.Update(data);

                            obj.UserImgId = userImg.UserImgId;

                            context.Users.Update(obj);

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

            public static Users ResetPassWord(Guid id, string newPassword)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        Users obj = context.Users.Find(id);

                        UsersPassword usersPassword = new UsersPassword()
                        {
                            Password = newPassword,
                            UserId = obj.UserId
                        };

                        context.UsersPassword.Update(usersPassword);

                        context.SaveChanges();

                        obj.PasswordId = usersPassword.PasswordId.Value;

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

            public static Users LoginStamp(Guid id)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        Users obj = context.Users.Find(id);

                        obj.LastLogin = DateTime.Now;

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

            public static Users Downgrade(Guid id, Guid userId)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        Users obj = context.Users.Find(id);

                        obj.UserType = 1;

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
        }
    }
}
