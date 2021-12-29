using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using cisApp.Core;
using Microsoft.Extensions.Configuration;

namespace cisApp.Function
{
    public static class GetChatMessage
    {
        readonly static IConfigurationRoot config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
                      .AddJsonFile("appsettings.json")
                      .Build();

        static string _DefaultProfile = "assets/media/users/100_1.jpg";

        public class Get
        {

            public static List<ChatListModel> GetChatList(Guid id)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@id", id)
                    };

                    var messages = StoreProcedure.GetAllStored<ChatListModel>("GetChatList", parameter);

                    if (messages != null)
                    {
                        string webAdmin = config.GetSection("WebConfig:AdminWebStie").Value;
                        foreach (var item in messages)
                        {
                            Guid refId = item.SenderId == id ? item.RecieverId : item.SenderId;

                            var chatGroup = GetChatGroup.Get.GetById(refId);

                            item.Profiles = new List<AttachFileAPIModel>();

                            if (chatGroup != null)
                            {
                                var chatGroupUsers = GetChatGroup.Get.GetUserByGroupId(chatGroup.ChatGroupId.Value);

                                if (chatGroupUsers != null)
                                {
                                    foreach(var user in chatGroupUsers)
                                    {
                                        // get profile
                                        var profile = GetUser.Get.GetUserProfileImg(user.UserId);

                                        if (profile != null)
                                        {
                                            item.Profiles.Add(new AttachFileAPIModel()
                                            {
                                                Name = profile.FileName,
                                                Path = webAdmin + profile.UrlPathAPI
                                            });
                                        }
                                        else
                                        {
                                            // insert default img
                                            item.Profiles.Add(new AttachFileAPIModel()
                                            {
                                                Name = "default",
                                                Path = webAdmin + _DefaultProfile
                                            });
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // get profile
                                var profile = GetUser.Get.GetUserProfileImg(item.SenderId);

                                if (profile != null)
                                {
                                    item.Profiles.Add(new AttachFileAPIModel()
                                    {
                                        Name = profile.FileName,
                                        Path = webAdmin + profile.UrlPathAPI
                                    });
                                }
                                else
                                {
                                    // insert default img
                                    item.Profiles.Add(new AttachFileAPIModel()
                                    {
                                        Name = "default",
                                        Path = webAdmin + _DefaultProfile
                                    });
                                }
                            }

                            
                        }
                    }

                    return messages;
                }
                catch (Exception ex)
                {
                    return new List<ChatListModel>();
                }
            }

            public static List<ChatMessageModel> GetChatMessageModels(SearchModel model)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@sender", model.SenderId.Value),
                       new SqlParameter("@reciever", model.RecieverId.Value),
                       new SqlParameter("@skip", model.currentPage.HasValue ? (model.currentPage-1)*model.pageSize : (object)DBNull.Value),
                       new SqlParameter("@take", model.pageSize.HasValue ? model.pageSize.Value : (object)DBNull.Value)
                    };

                    var messages =  StoreProcedure.GetAllStored<ChatMessageModel>("GetChatMessageModel", parameter);

                    if (messages != null)
                    {
                        string webAdmin = config.GetSection("WebConfig:AdminWebStie").Value;
                        foreach (var item in messages)
                        {

                            // if message has attachFile must get it
                            if (item.ImgId != null)
                            {
                                var attachFiles = GetAttachFile.Get.GetByRefIdList(item.ImgId.Value);

                                item.Files = new List<AttachFileAPIModel>();

                                foreach (var file in attachFiles)
                                {
                                    AttachFileAPIModel attachFile = new AttachFileAPIModel()
                                    {
                                        Name = file.FileName,
                                        Path = webAdmin + file.UrlPathAPI
                                    };

                                    item.Files.Add(attachFile);
                                }
                            }

                            // get profile
                            var profile = GetUser.Get.GetUserProfileImg(item.SenderId);

                            if (profile != null)
                            {
                                item.Profile = new AttachFileAPIModel()
                                {
                                    Name = profile.FileName,
                                    Path = webAdmin + profile.UrlPathAPI
                                };
                            }
                            else
                            {
                                // insert default img
                                item.Profile = new AttachFileAPIModel()
                                {
                                    Name = "default",
                                    Path = webAdmin + _DefaultProfile
                                };
                            }
                        }
                    }

                    return messages;
                }
                catch (Exception ex)
                {
                    return new List<ChatMessageModel>();
                }
            }
        }

        public class Manage
        {
            public static ChatMessage Add(ChatMessage data, List<Guid> imgs, Guid userId)
            {
                try
                {
                    using (var context = new CAppContext()) {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            var createDate = DateTime.Now;

                            Guid? chatMessageImgId = null;

                            if (imgs != null && imgs.Count > 0)
                            {
                                chatMessageImgId = AddChatMessageImg(Guid.NewGuid(), imgs, userId);
                                data.ImgId = chatMessageImgId;
                            }

                            data.CreatedDate = createDate;                            

                            context.ChatMessage.Add(data);

                            var chatGroup = GetChatGroup.Get.GetById(data.RecieverId);

                            if (chatGroup != null)
                            {
                                // if not null need to send message to other in group
                                var users = GetChatGroup.Get.GetUserByGroupId(chatGroup.ChatGroupId.Value);

                                if (users != null)
                                {                                    
                                    foreach (var user in users)
                                    {
                                        if (data.RealSenderId != user.UserId)
                                        {
                                            ChatMessage chat = new ChatMessage()
                                            {
                                                SenderId = chatGroup.ChatGroupId.Value, // in group chat change sender to group
                                                RealSenderId = data.RealSenderId,
                                                RecieverId = user.UserId,
                                                CreatedDate = createDate,
                                                ImgId = chatMessageImgId,
                                                Message = data.Message,
                                                Ip = data.Ip
                                            };

                                            context.ChatMessage.Add(chat);
                                        }
                                        
                                    }
                                }
                            }

                            context.SaveChanges();

                            dbContextTransaction.Commit();

                            return data;
                        }
                    };
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            private static Guid AddChatMessageImg(Guid imgId, List<Guid> imgs, Guid userId)
            {
                try
                {
                    foreach (var item in imgs)
                    {
                        GetAttachFile.Manage.ChangeRefId(item, imgId, userId);
                    }

                    return imgId;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
