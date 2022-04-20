using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using cisApp.Core;
using Microsoft.Extensions.Configuration;
using System.Linq;

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

            public static List<ChatListModel> GetChatList(Guid id, string type = "")
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@id", id),
                       new SqlParameter("@type", !string.IsNullOrEmpty(type) ? type : (object)DBNull.Value)
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
                                        var userModel = GetUser.Get.GetById(user.UserId);

                                        if (profile != null)
                                        {
                                            item.Profiles.Add(new AttachFileAPIModel()
                                            {
                                                Name = profile.FileName,
                                                Path = webAdmin + profile.UrlPathAPI,
                                                Description = userModel.Fname
                                            });
                                        }
                                        else
                                        {
                                            // insert default img
                                            item.Profiles.Add(new AttachFileAPIModel()
                                            {
                                                Name = "default",
                                                Path = null,
                                                Description = userModel.Fname
                                            });
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // get profile
                                var profile = GetUser.Get.GetUserProfileImg(refId);
                                var userModel = GetUser.Get.GetById(refId);

                                if (profile != null)
                                {
                                    item.Profiles.Add(new AttachFileAPIModel()
                                    {
                                        Name = profile.FileName,
                                        Path = webAdmin + profile.UrlPathAPI,
                                        Description = userModel.Fname
                                    });
                                }
                                else
                                {
                                    // insert default img
                                    item.Profiles.Add(new AttachFileAPIModel()
                                    {
                                        Name = "default",
                                        Path = null,
                                        Description = userModel.Fname
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

                    Manage.MarkRead(model.SenderId.Value, model.RecieverId.Value);

                    if (messages != null)
                    {
                        string webAdmin = config.GetSection("WebConfig:AdminWebStie").Value;
                        foreach (var item in messages)
                        {

                            // if message has attachFile must get it
                            if (item.ImgId != null)
                            {
                                var attachFiles = GetAttachFile.Get.GetByRefIdList(item.ImgId.Value);

                                item.AttachFiles = attachFiles;
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
                            var profile = GetUser.Get.GetUserProfileImg(item.RealSenderId);

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
                                    Path = null
                                };
                            }
                        }
                    }

                    return messages.OrderBy(o => o.CreatedDate).ToList();
                }
                catch (Exception ex)
                {
                    return new List<ChatMessageModel>();
                }
            }

            public static ChatDetailModel GetChatDetail(Guid id)
            {
                try
                {
                    var chatGroup = GetChatGroup.Get.GetById(id);

                    if (chatGroup != null)
                    {
                        var chatGroupUsers = GetChatGroup.Get.GetUserByGroupId(chatGroup.ChatGroupId.Value);

                        if (chatGroupUsers != null)
                        {
                            ChatDetailModel detail = new ChatDetailModel()
                            {
                                RecieverId = id,
                                IsGroup = true,
                                ChatName = chatGroup.ChatGroupName,
                                chatProfiles = new List<ChatProfileModel>()
                            };

                            string webAdmin = config.GetSection("WebConfig:AdminWebStie").Value;

                            foreach (var item in chatGroupUsers)
                            {
                                var img = GetUser.Get.GetUserProfileImg(item.UserId);

                                var user = GetUser.Get.GetById(item.UserId);

                                if (user != null)
                                {
                                    ChatProfileModel profile = new ChatProfileModel()
                                    {
                                        Name = user?.Fname + " " + user?.Lname,
                                        ImgUrl = webAdmin + img?.UrlPathAPI
                                    };

                                    detail.chatProfiles.Add(profile);
                                }
                            }

                            return detail;
                        }
                    }
                    else
                    {
                        var user = GetUser.Get.GetById(id);

                        if (user != null)
                        {
                            ChatDetailModel detail = new ChatDetailModel()
                            {
                                RecieverId = id,
                                IsGroup = false,
                                ChatName = user.Fname + " " +user.Lname,
                                chatProfiles = new List<ChatProfileModel>()
                            };

                            string webAdmin = config.GetSection("WebConfig:AdminWebStie").Value;

                            var img = GetUser.Get.GetUserProfileImg(id);

                            ChatProfileModel profile = new ChatProfileModel()
                            {
                                Name = user?.Fname + " " + user?.Lname,
                                ImgUrl = webAdmin + img?.UrlPathAPI
                            };

                            detail.chatProfiles.Add(profile);

                            return detail;
                        }
                    }

                    return null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static List<ChatMessageModel> MockChatMessageModel(Guid senderId, Guid recieverId, string message, List<Guid> imgs)
            {
                try
                {
                    // if message has attachFile must get it
                    List<AttachFileAPIModel> attachFileAPIs = new List<AttachFileAPIModel>();

                    List<AttachFile> attachFiles = new List<AttachFile>();

                    string webAdmin = config.GetSection("WebConfig:AdminWebStie").Value;
                    var createDate = DateTime.Now;

                    if (imgs != null)
                    {
                        

                        foreach (var item in imgs)
                        {
                            var attach = GetAttachFile.Get.GetById(item);
                            attachFiles.Add(attach);
                        }

                        foreach (var file in attachFiles)
                        {
                            AttachFileAPIModel attachFile = new AttachFileAPIModel()
                            {
                                Name = file.FileName,
                                Path = webAdmin + file.UrlPathAPI
                            };

                            attachFileAPIs.Add(attachFile);
                        }
                    }

                    var chatGroup = GetChatGroup.Get.GetById(recieverId);

                    if (chatGroup != null)
                    {
                        var chatGroupUsers = GetChatGroup.Get.GetUserByGroupId(chatGroup.ChatGroupId.Value).Where(o => o.UserId != senderId).ToList();

                        if (chatGroupUsers != null)
                        {

                            List<ChatMessageModel> chatMessageModels = new List<ChatMessageModel>();
                            
                            foreach (var item in chatGroupUsers)
                            {
                                ChatMessageModel messageModel = new ChatMessageModel()
                                {
                                    SenderId = chatGroup.ChatGroupId.Value,
                                    RealSenderId = senderId,
                                    RecieverId = item.UserId,
                                    Message = message,
                                    AttachFiles = attachFiles,
                                    Files = attachFileAPIs,
                                    CreatedDate = createDate
                                };

                                var profile = GetUser.Get.GetUserProfileImg(senderId);

                                var user = GetUser.Get.GetById(senderId);

                                if (user != null)
                                {
                                    messageModel.SenderName = user.Fname + " " + user.Lname;
                                    if (profile != null)
                                    {
                                        messageModel.Profile = new AttachFileAPIModel()
                                        {
                                            Name = profile.FileName,
                                            Path = webAdmin + profile.UrlPathAPI
                                        };
                                    }
                                    else
                                    {
                                        // insert default img
                                        messageModel.Profile = new AttachFileAPIModel()
                                        {
                                            Name = "default",
                                            Path = null
                                        };
                                    }

                                    chatMessageModels.Add(messageModel);
                                }
                            }

                            return chatMessageModels;
                        }

                        return null;
                    }
                    else
                    {
                        List<ChatMessageModel> chatMessageModels = new List<ChatMessageModel>();

                        ChatMessageModel messageModel = new ChatMessageModel()
                        {
                            SenderId = senderId,
                            RealSenderId = senderId,
                            RecieverId = recieverId,
                            Message = message,
                            AttachFiles = attachFiles,
                            Files = attachFileAPIs,
                            CreatedDate = createDate
                        };

                        var profile = GetUser.Get.GetUserProfileImg(senderId);

                        var user = GetUser.Get.GetById(senderId);

                        if (user != null)
                        {
                            messageModel.SenderName = user.Fname + " " + user.Lname;

                            if (profile != null)
                            {
                                messageModel.Profile = new AttachFileAPIModel()
                                {
                                    Name = profile.FileName,
                                    Path = webAdmin + profile.UrlPathAPI
                                };
                            }
                            else
                            {
                                // insert default img
                                messageModel.Profile = new AttachFileAPIModel()
                                {
                                    Name = "default",
                                    Path = null
                                };
                            }

                            chatMessageModels.Add(messageModel);
                        }

                        return chatMessageModels;
                    }

                    return null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static ChatListModel MockChatModel(Guid userId)
            {
                try
                {

                    string webAdmin = config.GetSection("WebConfig:AdminWebStie").Value;

                    var user = GetUser.Get.GetById(userId);

                    ChatListModel chatListModel = new ChatListModel()
                    {
                        UserId = userId,
                        ChatName = user.Fname + " " + user.Lname,
                        UserType = user.UserType,
                        Profiles = new List<AttachFileAPIModel>()
                    };

                    // get profile
                    var profile = GetUser.Get.GetUserProfileImg(userId);

                    if (profile != null)
                    {
                        chatListModel.Profiles.Add(new AttachFileAPIModel()
                        {
                            Name = profile.FileName,
                            Path = webAdmin + profile.UrlPathAPI
                        });
                    }
                    else
                    {
                        // insert default img
                        chatListModel.Profiles.Add(new AttachFileAPIModel()
                        {
                            Name = "default",
                            Path = null
                        });
                    }

                    return chatListModel;

                }
                catch (Exception ex)
                {
                    return null;
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
                                data.Message = "ได้ส่งไฟล์แนบ";
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

            public static void MarkRead(Guid currentUser, Guid target)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var chatMessages = context.ChatMessage.Where(o => o.RecieverId == currentUser && o.SenderId == target).ToList();

                        foreach (var item in chatMessages)
                        {
                            item.ReadDate = DateTime.Now;
                        }

                        context.ChatMessage.UpdateRange(chatMessages);

                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
