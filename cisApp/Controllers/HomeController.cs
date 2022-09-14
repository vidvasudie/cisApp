using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using cisApp.Models;
using cisApp.Function;
using cisApp.Core;

namespace cisApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public PartialViewResult GetWaitProcess()
        {
            DashboardModel model = new DashboardModel();
            try
            {
               
                model.PaymentCount = GetJobPayment.Get.GetWaitProcessCount();
                model.DesignerRequestCount = GetUserDesignerRequest.Get.GetDesignerRequestCount();

                return PartialView("~/Views/Home/PT/_WaitProcessItem.cshtml", model);
            }
            catch (Exception ex)
            {
                return PartialView("~/Views/Home/PT/_WaitProcessItem.cshtml", model);
            }
        } 

        public PartialViewResult GetProblemList()
        {
            List<UserHelp> data = GetUserHelp.Get.GetByStatus(1);//แจ้งปัญหา
            DashboardModel model = new DashboardModel();
            model.UserHelps = data != null && data.Count() > 0 ? data.OrderByDescending(o => o.CreatedDate).Take(5).ToList() : new List<UserHelp>();
            model.Total = data != null && data.Count() > 0 ? data.Count() : 0;

            return PartialView("~/Views/Home/PT/_ProblemItem.cshtml", model);
        }

        public PartialViewResult GetJobStatus()
        {
            return PartialView("~/Views/Home/PT/_JobItem.cshtml");
        }
        public JsonResult GetJobStatusData()
        {
            try
            {
                List<Jobs> jobs = GetJobs.Get.GetAll();
                int draftCount = jobs.Where(o => o.JobStatus == 1).Count();
                int waitCount = jobs.Where(o => o.JobStatus == 2).Count();
                int contestCount = jobs.Where(o => o.JobStatus == 3).Count();
                int attachCount = jobs.Where(o => o.JobStatus == 5).Count();
                int editCount = jobs.Where(o => o.JobStatus == 9).Count();
                int fileCount = jobs.Where(o => o.JobStatus == 7).Count();
                int finishCount = jobs.Where(o => o.JobStatus == 8).Count();
                int cancelCount = jobs.Where(o => o.JobStatus == 6).Count();

                return Json(new { data = new List<int> { draftCount, waitCount, contestCount, attachCount, editCount, fileCount, finishCount, cancelCount } });
            }
            catch (Exception ex)
            {
                return Json(new { data=new List<int>() });
            }
        }

        public PartialViewResult GetUser()
        {
            return PartialView("~/Views/Home/PT/_UserItem.cshtml");
        }
        public JsonResult GetUserData()
        {
            try
            {
                List<Users> users = cisApp.Function.GetUser.Get.GetAll();
                int customerCount = users.Where(o => o.IsDeleted == false && o.IsActive == true && o.UserType == 1).Count();
                int designerCount = users.Where(o => o.IsDeleted == false && o.IsActive == true && o.UserType == 2).Count();
                int officerCount = users.Where(o => o.IsDeleted == false && o.IsActive == true && o.UserType == 3).Count();

                return Json(new { data = new List<int> { customerCount, designerCount, officerCount } });
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<int>() });
            }
            
        }

        public PartialViewResult GetUserAccess()
        {
            DashboardModel model = new DashboardModel();
            try
            {
                model.DailyAccess = GetLogActivity.Get.GetDailyAccessCount();
                model.WeeklyAccess = GetLogActivity.Get.GetWeeklyAccessCount();
                model.MonthlyAccess  = GetLogActivity.Get.GetMonthlyAccessCount();

                return PartialView("~/Views/Home/PT/_UserAccessItem.cshtml", model);
            }
            catch (Exception ex)
            {
                return PartialView("~/Views/Home/PT/_UserAccessItem.cshtml", model);
            }
             
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
