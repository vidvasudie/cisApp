using cisApp.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace cisApp.Function
{
    public static class GetUserClientId
    {
        public class Get
        {

        }

        public class Manage
        {
            public static UsersClientId Add(Guid userId, string clientId)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        UsersClientId obj = new UsersClientId()
                        {
                            ClientId = clientId,
                            UserId = userId,
                            CreatedDate = DateTime.Now
                        };

                        context.UsersClientId.Add(obj);
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
