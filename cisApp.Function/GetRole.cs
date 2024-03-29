﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using cisApp.Core;

namespace cisApp.Function
{
    public static class GetRole
    {
        public class Get
        {
            public static List<Role> GetAll()
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.Role.ToList();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public static Role GetById(Guid id)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.Role.Where(o => o.RoleId == id).FirstOrDefault();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static List<Role> GetDropdown()
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.Role.Where(o => o.IsActive == true && o.IsDeleted == false).ToList();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static List<Role> GetRole(SearchModel model)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@stext", !String.IsNullOrEmpty(model.text) ? model.text.Trim() : (object)DBNull.Value),
                       new SqlParameter("@roleid", model.gId != null && model.gId != Guid.Empty ? model?.gId : (object)DBNull.Value), 
                       new SqlParameter("@skip", model.currentPage.HasValue ? (model.currentPage-1)*model.pageSize : (object)DBNull.Value),
                       new SqlParameter("@take", model.pageSize.HasValue ? model.pageSize.Value : (object)DBNull.Value)
                    };

                    return StoreProcedure.GetAllStored<Role>("GetRole", parameter);
                }
                catch (Exception ex)
                {
                    return new List<Role>();
                }
            }
            public static int GetRoleTotal(SearchModel model)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@stext", !String.IsNullOrEmpty(model.text) ? model.text.Trim() : (object)DBNull.Value),
                       new SqlParameter("@roleid", model.gId != null && model.gId != Guid.Empty ? model?.gId : (object)DBNull.Value), 
                    };

                    var dt = StoreProcedure.GetAllStoredDataTable("GetRoleTotal", parameter);
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
            public static Role Update(Role data, List<RoleManageModel> menulist)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            Role obj = new Role();
                            
                            if (data.RoleId != null && data.RoleId != Guid.Empty)
                            {
                                obj = context.Role.Find(data.RoleId);
                            }
                            else
                            {
                                obj.IsActive = true;
                                obj.CreatedDate = DateTime.Now;
                                obj.CreatedBy = data.CreatedBy;
                            }

                            obj.RoleName = data.RoleName;
                            obj.IsDeleted = data.IsDeleted;
                            obj.UpdatedDate = DateTime.Now;
                            obj.UpdatedBy = data.UpdatedBy;

                            context.Role.Update(obj);
                            context.SaveChanges();
                             
                            if (menulist.Count > 0)
                            {
                                List<RoleMenu> resultListRoleMenuDelete = new List<RoleMenu>();
                                resultListRoleMenuDelete = context.RoleMenu.Where(o => o.RoleId == data.RoleId).ToList();

                                if(resultListRoleMenuDelete.Count > 0)
                                {
                                    context.RoleMenu.RemoveRange(resultListRoleMenuDelete);
                                    context.SaveChanges();
                                }

                                List<RoleMenu> objMap = new List<RoleMenu>();
                                foreach (var item in menulist)
                                {
                                    RoleMenu objAdd = new RoleMenu(); 
                                    objAdd.RoleId = obj.RoleId;
                                    objAdd.MenuId = item.MenuId;
                                    objAdd.Type = item.Type;
                                    objAdd.CreatedBy = obj.UpdatedBy;
                                    objAdd.CreatedDate = DateTime.Now;
                                    objMap.Add(objAdd);
                                }

                                context.RoleMenu.AddRange(objMap);
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

            public static Role Active(Guid id, bool active, Guid userId)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            Role obj = context.Role.Find(id); 

                            obj.IsActive = active;

                            obj.UpdatedDate = DateTime.Now;
                            obj.UpdatedBy = userId;

                            context.Role.Update(obj);
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

            public static Role Delete(Guid id, Guid userId)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            Role obj = context.Role.Find(id);

                            obj.IsDeleted = true;

                            obj.DeletedDate = DateTime.Now;
                            obj.DeletedBy = userId;

                            context.Role.Update(obj);
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
