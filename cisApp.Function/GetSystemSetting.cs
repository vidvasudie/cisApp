using cisApp.Core;
using cisApp.library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cisApp.Function
{
    public static class GetSystemSetting
    {
        public class Get
        {
            
            public static SystemSetting GetByKeyword(string keyword)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.SystemSetting.Where(o => o.Keyword.ToLower() == keyword.ToLower()).FirstOrDefault();
                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static EmailSettingModel GetEmailSettingModel()
            {
                try
                {
                    var data = GetByKeyword("Email");

                    EmailSettingModel model = new EmailSettingModel()
                    {
                        Host = data.Value1,
                        FromEmail = data.Value2,
                        Password = data.Value3,
                        Port = data.Value4,
                        EnableSsl = data.Value5
                    };

                    return model;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public class Manage
        {
            public static SystemSetting Update(SystemSetting data, Guid userId)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        // only update content
                        var obj = context.SystemSetting.Where(o => o.Keyword == data.Keyword).FirstOrDefault();

                        if (obj == null )
                        {
                            throw new Exception("Object not found exception");
                        }

                        obj.Value1 = data.Value1;
                        obj.Value2 = data.Value2;
                        obj.Value3 = data.Value3;
                        obj.Value4 = data.Value4;
                        obj.Value5 = data.Value5;
                        obj.Value6 = data.Value6;
                        obj.Value7 = data.Value7;
                        obj.Value8 = data.Value8;
                        obj.Value9 = data.Value9;
                        obj.Value10 = data.Value10;
                        obj.UpdatedBy = userId;
                        obj.UpdatedDate = DateTime.Now;

                        context.SystemSetting.Update(obj);

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
