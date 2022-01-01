using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using cisApp.Core;

namespace cisApp.Function
{
    public static class GetChatGroup
    {
        public class Get
        {
            public static ChatGroup GetById(Guid id)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[]
                    {
                        new SqlParameter("@id", id)
                    };

                    var data = StoreProcedure.GetAllStored<ChatGroup>("GetChatGroupById", parameter);

                    if (data.Count > 0)
                    {
                        return data.FirstOrDefault();
                    }

                    return null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static List<ChatGroupUser> GetUserByGroupId(Guid id)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[]
                    {
                        new SqlParameter("@id", id)
                    };

                    var data = StoreProcedure.GetAllStored<ChatGroupUser>("GetChatGroupUserByGroupId", parameter);

                    if (data.Count > 0)
                    {
                        return data;
                    }

                    return null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public class Update
        {

        }
    }
}
