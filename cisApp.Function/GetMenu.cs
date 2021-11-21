using System;
using System.Collections.Generic;
using System.Linq;
using cisApp.Core;

namespace cisApp.Function
{
    public static class GetMenu
    { 
        public class Get
        {
            public static List<Menu> GetAll()
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.Menu.ToList();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public static List<Menu> GetByActive()
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.Menu.Where(o => o.IsActive == true && o.IsDeleted == false).ToList();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public static Menu GetById(Guid id)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.Menu.Where(o => o.MenuId == id).FirstOrDefault();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public static List<Menu> GetByParentId(Guid id)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.Menu.Where(o => o.Parent == id).ToList();

                        return data;
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
