using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cisApp.Function;
using cisApp.Models;
using cisApp.library;
using cisApp.Core;

namespace cisApp.Controllers
{
    public class userManagementController : Controller
    {
        private List<UserModel> _model = new List<UserModel>();
        public IActionResult Index(int pageIndex = 1)
        {
            return View(new PaginatedList<UserModel>(_model, _model.Count, pageIndex, 2));
        }

        [HttpPost]
        public PartialViewResult ItemList(int? currentPage = 1, int? pageSize = 10)
        { 
            return PartialView("PT/_itemlist", new PaginatedList<UserModel>(_model, _model.Count, currentPage.Value, pageSize.Value));
        }

        public IActionResult Manage(Guid? id)
        {
            Users data = new Users();
            if (id != null) {
                data = GetUser.Get.GetById(id.Value);
            }

            return View(data);
        }
    }


}
