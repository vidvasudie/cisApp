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
            List<UserModel> _model = new List<UserModel>();
             

            return PartialView("PT/_itemlist", new PaginatedList<UserModel>(_model, _model.Count, model.currentPage.Value, model.pageSize.Value)); 
        }

        public IActionResult Manage()
        {
            return View();
        }
    }
}