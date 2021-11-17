using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cisApp.Function;
using cisApp.Models;
using cisApp.library;

namespace cisApp.Controllers
{
    public class userManagementController : Controller
    {
        private List<UserModel> _model = new List<UserModel>(){
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
            };
        public IActionResult Index(int pageIndex = 1)
        {
            return View(new PaginatedList<UserModel>(_model, _model.Count, pageIndex, 2));
        }

        [HttpPost]
        public PartialViewResult ItemList(int currentPage, int pageSize)
        {
            return PartialView("PT/_itemlist", new PaginatedList<UserModel>(_model, _model.Count, currentPage, pageSize));
        }

        public IActionResult Manage(string Mode)
        {
            return View("Manage", Mode);
        }
    }


}
