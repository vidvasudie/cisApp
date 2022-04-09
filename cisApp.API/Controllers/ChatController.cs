using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cisApp.Function;
using cisApp.Core;
using cisApp.API.Models;

namespace cisApp.API.Controllers
{
    public class ChatController : Controller
    {
        [Route("api/Chat/GetChatList")]
        [HttpPost]
        public IActionResult GetChatList(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return Ok(resultJson.errors("ModelState not valid", "fail", null));
                }


                var chatModels = GetChatMessage.Get.GetChatList(id.Value);

                int unReadChat = 0;

                if (chatModels != null)
                {
                   unReadChat = chatModels.Where(o => o.UnRead > 0).ToList().Count;
                }

                return Ok(resultJson.success("ดึงข้อมูลสำเร็จ", "success",new { chatModels, unReadChat}));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("ดึงข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        [Route("api/Chat/GetChatGroupList")]
        [HttpPost]
        public IActionResult GetChatGroupList(Guid? id, Guid? chatGroupId)
        {
            try
            {
                if (id == null || chatGroupId == null)
                {
                    return Ok(resultJson.errors("ModelState not valid", "fail", null));
                }


                var chatModels = GetChatMessage.Get.GetChatList(id.Value);

                // id = userId

                List<ChatListModel> groupChatListModels = new List<ChatListModel>();

                // first add chatgroup it aways not null
                groupChatListModels.Add(chatModels.FirstOrDefault(o => o.UserId == chatGroupId.Value));

                bool isUserOwnJob = true;

                var chatGroup = GetChatGroup.Get.GetById(chatGroupId.Value);

                Guid? jobOwnerUserId = null;

                if (chatGroup != null)
                {
                    if (chatGroup.JobId != null)
                    {
                        var job = GetJobs.Get.GetById(chatGroup.JobId.Value);

                        if (job != null)
                        {
                            if (job.UserId != id)
                            {
                                isUserOwnJob = false;
                                jobOwnerUserId = job.UserId;
                            }
                        }
                    }
                    else
                    {
                        var job = GetJobs.Get.GetByJobNo(chatGroup.ChatGroupName);

                        if (job != null)
                        {
                            if (job.UserId != id)
                            {
                                isUserOwnJob = false;
                                jobOwnerUserId = job.UserId;
                            }
                        }
                    }
                }


                if (isUserOwnJob)
                {
                    // then fill users chat it sometime null
                    var chatGroupUsers = GetChatGroup.Get.GetUserByGroupId(chatGroupId.Value).Where(o => o.UserId != id.Value);

                    foreach (var item in chatGroupUsers)
                    {
                        var chat = chatModels.FirstOrDefault(o => o.UserId == item.UserId);

                        if (chat != null)
                        {
                            groupChatListModels.Add(chat);
                        }
                        else
                        {
                            var mockChatModel = GetChatMessage.Get.MockChatModel(item.UserId);

                            groupChatListModels.Add(mockChatModel);
                        }
                    }
                }
                else
                {
                    // then fill users chat it sometime null
                    var chatGroupUsers = GetChatGroup.Get.GetUserByGroupId(chatGroupId.Value).Where(o => o.UserId != id.Value && o.UserId == jobOwnerUserId);

                    foreach (var item in chatGroupUsers)
                    {
                        var chat = chatModels.FirstOrDefault(o => o.UserId == item.UserId);

                        if (chat != null)
                        {
                            groupChatListModels.Add(chat);
                        }
                        else
                        {
                            var mockChatModel = GetChatMessage.Get.MockChatModel(item.UserId);

                            groupChatListModels.Add(mockChatModel);
                        }
                    }
                }

                


                return Ok(resultJson.success("ดึงข้อมูลสำเร็จ", "success", groupChatListModels));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("ดึงข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        [Route("api/Chat/GetMessage")]
        [HttpPost]
        public IActionResult GetMessage(Guid? senderId, Guid? recieverId, int page = 1, int limit = 10)
        {
            try
            {
                if (senderId == null || recieverId == null)
                {
                    return Ok(resultJson.errors("ModelState not valid", "fail", null));
                }

                SearchModel model = new SearchModel
                {
                    SenderId = senderId
                    ,
                    RecieverId = recieverId
                    ,
                    currentPage = page
                    ,
                    pageSize = limit
                };

                var chatModels = GetChatMessage.Get.GetChatMessageModels(model);

                return Ok(resultJson.success("ดึงข้อมูลสำเร็จ", "success", chatModels ));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("ดึงข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        [Route("api/Chat/SendMessage")]
        [HttpPost]
        public IActionResult SendMessage([FromBody] ChatMessageAPIModel value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ChatMessage model = new ChatMessage()
                    {
                        SenderId = value.SenderId.Value,
                        RealSenderId = value.SenderId.Value,
                        RecieverId = value.RecieverId.Value,
                        Message = value.Message,
                        Ip = value.Ip
                    };

                    var resutl = GetChatMessage.Manage.Add(model, value.Imgs, value.SenderId.Value);

                    return Ok(resultJson.success("บันทึกข้อมูลสำเร็จ", "success", null));
                }
                else
                {
                    return Ok(resultJson.errors("ModelState not valid", "fail", null));
                }
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }
    }
}
