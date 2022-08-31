using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using cisApp.Common;
using cisApp.Core;
using cisApp.Function;
using cisApp.library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; 
using static cisApp.Controllers.BaseController;

namespace cisApp.Controllers
{
    [Authorize]
    [CustomActionExecute("932C5CEE-4F5F-4E7B-971D-F3B78920F683")]
    public class SystemProblemsController : BaseController
    {
        private List<SystemProblemModel> _model = new List<SystemProblemModel>();
        private Guid _PermissionMenuId;
        private int _PermissionManage;
        private readonly IWebHostEnvironment _HostingEnvironment;

        public SystemProblemsController(IWebHostEnvironment environment)
        {
            _PermissionMenuId = Guid.Parse("932C5CEE-4F5F-4E7B-971D-F3B78920F683");
            _PermissionManage = 2;// สิทธิ์ผู้ใช้งาน
            _HostingEnvironment = environment;
        }

        // GET: SystemProblems
        public ActionResult Index(int pageIndex = 1)
        {
            LogActivityEvent(LogCommon.LogMode.PLOBLEM);
            return View(new PaginatedList<SystemProblemModel>(_model, _model.Count, pageIndex, 2));
        }

        [HttpPost]
        public ActionResult ItemList(SearchModel model)
        {
            //model.OrderBy = "CreatedDate";
            List<SystemProblemModel> _model = GetUserHelp.Get.GetUserHelpModels(model);
            int count = GetUserHelp.Get.GetUserHelpModelsTotal(model);

            LogActivityEvent(LogCommon.LogMode.SEARCH);
            return PartialView("PT/_itemlist", new PaginatedList<SystemProblemModel>(_model, count, model.currentPage.Value, model.pageSize.Value));
        }

        public PartialViewResult Manage(int id)
        {
            try
            {
                LogActivityEvent(LogCommon.LogMode.MANAGE);
                var uHelp = GetUserHelp.Get.GetUserHelpModelByID(id);
                if (uHelp == null || uHelp.Count == 0)
                {
                    return PartialView("~/Views/SystemProblems/PT/_Detail.cshtml", new SystemProblemModel());
                }
                return PartialView("~/Views/SystemProblems/PT/_Detail.cshtml", uHelp.FirstOrDefault());//, uHelp.FirstOrDefault()
            }
            catch (Exception ex)
            {
                LogActivityEvent(LogCommon.LogMode.MANAGE, MessageCommon.TXT_OPERATE_ERROR, ex.ToString());
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult Manage(SystemProblemModel data)
        {
            try
            {
                var uHelp = GetUserHelp.Manage.Update(data.Id, data.Remark, _UserId().Value);

                SendMail.SendMailProblemReply(uHelp.Email, uHelp.Email, uHelp.Message, uHelp.Remark, _HostingEnvironment.WebRootPath);

                LogActivityEvent(LogCommon.LogMode.UPDATE, MessageCommon.SaveSuccess);
                return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Index", "SystemProblems")));
            }
            catch (Exception ex)
            {
                LogActivityEvent(LogCommon.LogMode.UPDATE, MessageCommon.TXT_OPERATE_ERROR, ex.ToString());
                return Json(new ResponseModel().ResponseError());
            }
        }

        [HttpPost]
        public PartialViewResult Export(SearchModel model)
        {
            var dt = GetUserHelp.Get.GetExportUserHelp(model);

            return PartialView("~/Views/Shared/Export/_TableDetail.cshtml", dt);
        }
        

    }
}