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
    public class GetChatHistory
    {
        public class Get
        {
            readonly static IConfigurationRoot config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
                      .AddJsonFile("appsettings.json")
                      .Build();

            static string _DefaultProfile = "assets/media/users/100_1.jpg";
            public static List<ChatListModel> GetChatList(string name = "")
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@name", !string.IsNullOrEmpty(name) ? name : (object)DBNull.Value),
                       new SqlParameter("@type", (object)DBNull.Value)
                    };

                    var messages = StoreProcedure.GetAllStored<ChatListModel>("GetChatListHistory", parameter);

                    if (messages != null)
                    {
                        string webAdmin = config.GetSection("WebConfig:AdminWebStie").Value;
                        foreach (var item in messages)
                        {
                            

                            item.Profiles = new List<AttachFileAPIModel>();

                            // get profile
                            var profile = GetUser.Get.GetUserProfileImg(item.SenderId);
                            var userModel = GetUser.Get.GetById(item.SenderId);

                            var profile2 = GetUser.Get.GetUserProfileImg(item.RecieverId);
                            var userModel2 = GetUser.Get.GetById(item.RecieverId);

                            if (profile != null)
                            {
                                item.Profiles.Add(new AttachFileAPIModel()
                                {
                                    Name = profile.FileName,
                                    Path = webAdmin + profile.UrlPathAPI,
                                    Description = userModel.Fname + " " + userModel.Lname
                                });
                            }
                            else
                            {
                                // insert default img
                                item.Profiles.Add(new AttachFileAPIModel()
                                {
                                    Name = "default",
                                    Path = webAdmin + _DefaultProfile,
                                    Description = userModel.Fname + " " + userModel.Lname
                                });
                            }

                            if (profile2 != null)
                            {
                                item.Profiles.Add(new AttachFileAPIModel()
                                {
                                    Name = profile2.FileName,
                                    Path = webAdmin + profile2.UrlPathAPI,
                                    Description = userModel2.Fname + " " + userModel2.Lname
                                });
                            }
                            else
                            {
                                // insert default img
                                item.Profiles.Add(new AttachFileAPIModel()
                                {
                                    Name = "default",
                                    Path = webAdmin + _DefaultProfile,
                                    Description = userModel2.Fname + " " + userModel2.Lname
                                });
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

            public static List<ChatListModel> GetChatGroupList(string name = "")
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@name", !string.IsNullOrEmpty(name) ? name : (object)DBNull.Value)
                    };

                    var chatGroups = StoreProcedure.GetAllStored<ChatGroup>("GetChatGroupListHistory", parameter);

                    List<ChatListModel> messages = chatGroups.Select(o => new ChatListModel()
                    {
                        ChatName = o.ChatGroupName,
                        UserId = o.ChatGroupId.Value,
                        RecieverId = o.ChatGroupId.Value,
                        TmpRecieverId = o.ChatGroupId.Value,
                    }).ToList();

                    if (messages != null)
                    {
                        string webAdmin = config.GetSection("WebConfig:AdminWebStie").Value;
                        foreach (var item in messages)
                        {

                            var chatGroup = GetChatGroup.Get.GetById(item.UserId);

                            item.Profiles = new List<AttachFileAPIModel>();

                            if (chatGroup != null)
                            {
                                var chatGroupUsers = GetChatGroup.Get.GetUserByGroupId(chatGroup.ChatGroupId.Value);

                                if (chatGroupUsers != null)
                                {
                                    item.SenderId = chatGroupUsers.OrderBy(o => o.UserId).FirstOrDefault().UserId;
                                    item.TmpSenderId = chatGroupUsers.OrderBy(o => o.UserId).FirstOrDefault().UserId;
                                    foreach (var user in chatGroupUsers)
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
                                                Path = webAdmin + _DefaultProfile,
                                                Description = userModel.Fname
                                            });
                                        }
                                    }
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

            public static ChatDetailModel GetChatDetail(Guid id1, Guid id2)
            {
                try
                {
                    var userId = id2;
                    var chatGroup = GetChatGroup.Get.GetById(id1);

                    if (chatGroup == null)
                    {
                        chatGroup = GetChatGroup.Get.GetById(id2);
                        userId = id1;
                    }

                    if (chatGroup != null)
                    {
                        var chatGroupUsers = GetChatGroup.Get.GetUserByGroupId(chatGroup.ChatGroupId.Value);

                        if (chatGroupUsers != null)
                        {
                            ChatDetailModel detail = new ChatDetailModel()
                            {
                                RecieverId = userId,
                                SenderId = chatGroup.ChatGroupId,
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
                        var user = GetUser.Get.GetById(id1);

                        var user2 = GetUser.Get.GetById(id2);

                        if (user != null)
                        {
                            ChatDetailModel detail = new ChatDetailModel()
                            {
                                RecieverId = id1,
                                SenderId = id2,
                                IsGroup = false,
                                ChatName = user.Fname + " " + user.Lname + " - " + user2.Fname + " " + user2.Lname,
                                chatProfiles = new List<ChatProfileModel>()
                            };

                            string webAdmin = config.GetSection("WebConfig:AdminWebStie").Value;

                            var img = GetUser.Get.GetUserProfileImg(id1);

                            ChatProfileModel profile = new ChatProfileModel()
                            {
                                Name = user?.Fname + " " + user?.Lname,
                                ImgUrl = webAdmin + img?.UrlPathAPI
                            };

                            detail.chatProfiles.Add(profile);



                            var img2 = GetUser.Get.GetUserProfileImg(id2);

                            ChatProfileModel profile2 = new ChatProfileModel()
                            {
                                Name = user2?.Fname + " " + user2?.Lname,
                                ImgUrl = webAdmin + img2?.UrlPathAPI
                            };

                            detail.chatProfiles.Add(profile2);

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
        }
    }
}
