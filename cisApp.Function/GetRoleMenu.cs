using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using cisApp.Core;

namespace cisApp.Function
{
    public static class GetRoleMenu
    {
        public class Get
        {
            public static List<RoleMenu> GetAll()
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.RoleMenu.ToList();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public static RoleMenu GetById(Guid id)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.RoleMenu.Where(o => o.RoleMenuId == id).FirstOrDefault();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static List<RoleMenu> GetByMenuId(Guid id)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.RoleMenu.Where(o => o.MenuId == id).ToList();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public static RoleMenu GetByRoleIdAndMenuId(Guid roleId, Guid menuId)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.RoleMenu.Where(o => o.MenuId == menuId && o.RoleId == roleId).FirstOrDefault();

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

        }

    }
}
