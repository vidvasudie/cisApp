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

        public class Manage
        {
            public static ChatGroup Update(ChatGroup data, Guid userId)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        // only update content
                        ChatGroup obj = new ChatGroup();

                        obj.ChatGroupName = data.ChatGroupName;

                        obj.CreatedBy = userId;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedBy = userId;
                        obj.UpdatedDate = DateTime.Now;

                        context.ChatGroup.Add(obj);

                        context.SaveChanges();

                        // เอาคนสร้างเข้ากลุ่ม

                        ChatGroupUser chatGroupUser = new ChatGroupUser();
                        chatGroupUser.UserId = userId;
                        chatGroupUser.ChatGroupId = obj.ChatGroupId.Value;

                        context.ChatGroupUser.Add(chatGroupUser);

                        context.SaveChanges();

                        // เพ่ิม message invite
                        ChatMessage chat = new ChatMessage()
                        {
                            SenderId = userId,
                            RecieverId = obj.ChatGroupId.Value,
                            CreatedDate = DateTime.Now,
                            RealSenderId = userId,
                            Message = "ได้ทำการสร้างกลุ่ม"
                        };

                        GetChatMessage.Manage.Add(chat, null, userId);

                        return obj;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static void AddUser(Guid userId, Guid groupId)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        // only update content
                        var chatGroup = context.ChatGroup.Find(groupId);

                        var user = context.Users.Find(userId);

                        // find duplicate user
                        var chatGroupUser = context.ChatGroupUser.Where(o => o.UserId == userId && o.ChatGroupId == groupId).FirstOrDefault();

                        if (chatGroupUser != null)
                        {
                            throw new Exception("ผู้ใช้ดังกล่าวอยู่ในกลุ่มดังกล่าวอยู่แล้ว");
                        }

                        // add user to group
                        ChatGroupUser groupUser = new ChatGroupUser()
                        {
                            ChatGroupId = groupId,
                            UserId = userId
                        };

                        context.ChatGroupUser.Add(groupUser);

                        context.SaveChanges();

                        // เพ่ิม message invite
                        ChatMessage chat = new ChatMessage()
                        {
                            SenderId = userId,
                            RecieverId = groupId,
                            CreatedDate = DateTime.Now,
                            RealSenderId = userId,
                            Message = "คุณ " + user.Fname + " " + user.Lname + " เข้ากลุ่ม" 
                        };

                        GetChatMessage.Manage.Add(chat, null, userId);
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
