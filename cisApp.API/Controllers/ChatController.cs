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

                return Ok(resultJson.success("ดึงข้อมูลสำเร็จ", "success", chatModels));
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
