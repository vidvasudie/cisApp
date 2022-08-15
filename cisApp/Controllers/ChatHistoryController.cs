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
using cisApp.Common;

namespace cisApp.Controllers
{
    [CustomActionExecute("33A0E193-2812-4783-9A7C-480762AC5A54")]
    public class ChatHistoryController : BaseController
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

                LogActivityEvent(LogCommon.LogMode.CHAT_HIST);
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

            LogActivityEvent(LogCommon.LogMode.GROUP_CHAT_HIST);
            return View(model);
        }

        [HttpGet]
        public PartialViewResult GetChatList(string text, string type = "1") // 1 is private , 2 is group
        {
            try
            {
                if (type == "1")
                {
                    var chatList = GetChatHistory.Get.GetChatList(text);

                    return PartialView("PT/_ChatList", chatList);
                }
                else
                {
                    var chatList = GetChatHistory.Get.GetChatGroupList(text);

                    return PartialView("PT/_ChatList", chatList);
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        public PartialViewResult GetChatCard(Guid? id1, Guid? id2)
        {
            try
            {
                if (id1 == null || id2 == null)
                {
                    throw new Exception("id null exception");
                }

                SearchModel model = new SearchModel()
                {
                    SenderId = id1.Value,
                    RecieverId = id2.Value
                };

                var chatMessages = GetChatMessage.Get.GetChatMessageModels(model);

                var chatDetailModel = GetChatHistory.Get.GetChatDetail(id1.Value, id2.Value);

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
        public PartialViewResult ItemList(SearchModel model)
        {
            List<UserModel> _model = GetUser.Get.GetUserModels(model);
            int count = GetUser.Get.GetUserModelsTotal(model);

            ViewData["GroupId"] = model.GroupId;

            return PartialView("PT/_itemlist", new PaginatedList<UserModel>(_model, count, model.currentPage.Value, model.pageSize.Value));
        }

    }
}
