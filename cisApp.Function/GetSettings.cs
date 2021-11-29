using cisApp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cisApp.Function
{
    public static class GetSettings
    {
        public class Get
        {
            public static List<Settings> GetAll()
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.Settings.ToList();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static Settings GetById(Guid id)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.Settings.Find(id);

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static Settings GetByKeyword(string keyword)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.Settings.Where(o => o.Keyword.ToLower() == keyword.ToLower()).FirstOrDefault();

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
            public static Settings Update(Settings data, Guid userId)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        // only update content
                        var obj = context.Settings.Find(data.SettingId.Value);

                        if (obj == null )
                        {
                            throw new Exception("Object not found exception");
                        }

                        obj.Content = data.Content;
                        obj.UpdatedBy = userId;
                        obj.UpdatedDate = DateTime.Now;

                        context.Settings.Update(obj);

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
