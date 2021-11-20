using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cisApp.Function;
using cisApp.library;
using Microsoft.AspNetCore.Mvc;

namespace cisApp.Controllers
{
    public class UserRequestHistoryController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public PartialViewResult ItemList(SearchModel model)
        { 
            model.mode = 1;//ค้นหาเอาเฉพาะ สถานะ != 1
            List<UserModel> _model = GetUserDesignerRequest.Get.GetUserDesignerRequestModel(model);
            int count = GetUserDesignerRequest.Get.GetUserDesignerRequestModelTotal(model);

            return PartialView("PT/_itemlist", new PaginatedList<UserModel>(_model, count, model.currentPage.Value, model.pageSize.Value));
        }

        public IActionResult Manage(SearchModel model)
        {
            List<UserModel> _model = GetUserDesignerRequest.Get.GetUserDesignerRequestModel(model);
            if (_model != null)
                return View(_model.FirstOrDefault());
            return View(new UserModel());
        }

    }
}