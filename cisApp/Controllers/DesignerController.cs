using cisApp.Function;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cisApp.library;

namespace cisApp.Controllers
{
    [CustomActionExecute("3D619832-0815-49BB-81F1-9C994E890666")]
    public class DesignerController : BaseController
    {
        private Guid _PermissionMenuId;
        private int _PermissionManage;
        public DesignerController()
        {
            _PermissionMenuId = Guid.Parse("3D619832-0815-49BB-81F1-9C994E890666");
            _PermissionManage = 2;// สิทธิ์ผู้ใช้งาน
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public PartialViewResult ItemList(SearchModel model)
        { 
            List<UserModel> _model = GetUserDesigner.Get.GetDesignerItems(model);
            int count = GetUserDesigner.Get.GetDesignerItemsTotal(model);

            return PartialView("PT/_itemlist", new PaginatedList<UserModel>(_model, count, model.currentPage.Value, model.pageSize.Value));
        }
    }
}
