using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cisApp.Controllers
{
    public class ChatsController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Private()
        {
            return View();
        }

        public IActionResult Group()
        {
            return View();
        }

        [HttpGet]
        public PartialViewResult GetChatList(int type = 0) // 0 is private , 1 is group
        {
            try
            {


                return PartialView("PT/_ChatList");
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


                return PartialView("PT/_ChatCard");
            }
            catch (Exception ex)
            {
                throw ex;
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
