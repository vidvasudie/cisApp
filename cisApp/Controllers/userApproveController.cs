using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using cisApp.library;
using cisApp.Core;
using cisApp.Function; 

namespace cisApp.Controllers
{
    public class userApproveController : Controller
    { 
        public IActionResult Index()
        { 

            return View();
        }
        [HttpPost]
        public PartialViewResult ItemList(SearchModel model)
        { 
            List<UserModel> _model = GetUser.Get.GetUserModels(model);
            int count = GetUser.Get.GetUserModelsTotal(model);

            return PartialView("PT/_itemlist", new PaginatedList<UserModel>(_model, count, model.currentPage.Value, model.pageSize.Value)); 
        }

        public IActionResult Manage()
        {
            return View(new UserModel() { UserType=2 });
        }
    }
}