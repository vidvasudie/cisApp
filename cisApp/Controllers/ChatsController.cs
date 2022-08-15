using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cisApp.Function;
using cisApp.Core;
using cisApp.Function.Models;
using Microsoft.Extensions.Configuration;
using System.IO;
using cisApp.library;
using Microsoft.AspNetCore.Authorization;
using cisApp.Common;

namespace cisApp.Controllers
{
    [CustomActionExecute("33A0E193-2812-4783-9A7C-480762AC5A54")]
    [Authorize]
    public class ChatsController : BaseController
    {
        readonly static IConfigurationRoot config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
                      .AddJsonFile("appsettings.json")
                      .Build();

        public IActionResult Index()
        {
            
            return View();
        }

        public IActionResult Private(SearchModel model)
        {
            try
            {
                string webSignalR = config.GetSection("WebConfig:SignalRWebSite").Value;

                ViewData["SignalRWebSite"] = webSignalR;

                LogActivityEvent(LogCommon.LogMode.CHAT);
                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public IActionResult Group(SearchModel model)
        {
            string webSignalR = config.GetSection("WebConfig:SignalRWebSite").Value;

            ViewData["SignalRWebSite"] = webSignalR;

            LogActivityEvent(LogCommon.LogMode.GROUP_CHAT);
            return View(model);
        }

        [HttpGet]
        public PartialViewResult GetChatList(string text, string type = "") // 1 is private , 2 is group
        {
            try
            {
                var chatList = GetChatMessage.Get.GetChatList(_UserId().Value, type);

                if (!string.IsNullOrEmpty(text))
                {
                    chatList = chatList.Where(o => o.ChatName.Contains(text)).ToList();
                }
                
                return PartialView("PT/_ChatList", chatList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public PartialViewResult GetChatCard(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    throw new Exception("id null exception");
                }

                SearchModel model = new SearchModel()
                {
                    SenderId = _UserId().Value,
                    RecieverId = id.Value
                };

                var chatMessages = GetChatMessage.Get.GetChatMessageModels(model);

                var chatDetailModel = GetChatMessage.Get.GetChatDetail(id.Value);

                chatDetailModel.chatMessages = chatMessages;

                return PartialView("PT/_ChatCard", chatDetailModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public PartialViewResult GetChatMessagePage(Guid? id, int page = 1)
        {
            try
            {
                if (id == null)
                {
                    throw new Exception("id null exception");
                }

                SearchModel model = new SearchModel()
                {
                    SenderId = _UserId().Value,
                    RecieverId = id.Value,
                    currentPage = page
                };

                var chatMessages = GetChatMessage.Get.GetChatMessageModels(model);

                return PartialView("PT/_ChatMessage", chatMessages);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult SendMessage(ChatMessage data)
        {
            try
            {
                data.SenderId = _UserId().Value;
                data.RealSenderId = _UserId().Value;

                var result = GetChatMessage.Manage.Add(data, null, _UserId().Value);
                if (result != null)
                {
                    return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, result));
                }
                else
                {
                    return Json(new ResponseModel().ResponseError());
                }
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel().ResponseError());
            }
        }

        [HttpPost]
        public JsonResult SendMessageFiles(ChatMessageFileModel data)
        {
            try
            {
                var chatMessage = new ChatMessage()
                {
                    SenderId = _UserId().Value,
                    RealSenderId = _UserId().Value,
                    RecieverId = data.RecieverId
                };

                List<AttachFile> uploadedFile = new List<AttachFile>();

                foreach (var file in data.FileList)
                {
                    var fileUpload = GetAttachFile.Manage.UploadFile(file.Base64Str, file.FileName, file.FileSize, null, _UserId().Value);
                    uploadedFile.Add(fileUpload);
                }

                var result = GetChatMessage.Manage.Add(chatMessage, uploadedFile.Select(o => o.AttachFileId).ToList(), _UserId().Value);

                if (result != null)
                {
                    return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, new { result, imgs = uploadedFile.Select(o => o.AttachFileId).ToList() }));
                }
                else
                {
                    return Json(new ResponseModel().ResponseError());
                }
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel().ResponseError());
            }
        }

        public IActionResult CreateGroup()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CreateGroup(ChatGroup data)
        {
            try
            {
                var user = GetChatGroup.Manage.Update(data, _UserId().Value);

                LogActivityEvent(LogCommon.LogMode.INSERT, MessageCommon.SaveSuccess);
                return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Group", "Chats", new { Id = user.ChatGroupId })));
            }
            catch (Exception ex)
            {
                LogActivityEvent(LogCommon.LogMode.INSERT, MessageCommon.TXT_OPERATE_ERROR, ex.ToString());
                return Json(new ResponseModel().ResponseError());
            }
        }

        [HttpPost]
        public PartialViewResult ItemList(SearchModel model)
        {
            List<UserModel> _model = GetUser.Get.GetUserModels(model);
            int count = GetUser.Get.GetUserModelsTotal(model);

            ViewData["GroupId"] = model.GroupId;
             
            return PartialView("PT/_itemlist", new PaginatedList<UserModel>(_model, count, model.currentPage.Value, model.pageSize.Value));
        }

        [HttpPost]
        public JsonResult AddUser(SearchModel model)
        {
            try
            {
                GetChatGroup.Manage.AddUser(model.UserId.Value, model.GroupId.Value);

                LogActivityEvent(LogCommon.LogMode.INSERT, MessageCommon.SaveSuccess);
                return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Group", "Chats", new { Id = model.GroupId.Value })));
            }
            catch (Exception ex)
            {
                LogActivityEvent(LogCommon.LogMode.INSERT, MessageCommon.TXT_OPERATE_ERROR, ex.ToString());
                return Json(new ResponseModel().ResponseError(ex.Message));
            }
        }

        public IActionResult XPrivate()
        {
            return View();
        }
        public IActionResult XGroup()
        {
            return View();
        }
    }
}
