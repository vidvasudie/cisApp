using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using cisApp.Designer.Models;
using cisApp.Function;
using cisApp.library;

namespace cisApp.Designer.Controllers
{
    [CustomActionExecute]
    public class JobListController : BaseController
    {
        private readonly ILogger<JobListController> _logger;

        public JobListController(ILogger<JobListController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(SearchModel model)
        {
            return View(model);
        }

        [HttpPost]
        public PartialViewResult ItemList(SearchModel model)
        {
            List<JobModel> _model = GetJobs.Get.GetJobs(model);
            int count = GetJobs.Get.GetJobsTotal(model);

            return PartialView("PT/_itemlist", new PaginatedList<JobModel>(_model, count, model.currentPage.Value, model.pageSize.Value));
        }

        public IActionResult Detail(SearchModel model)
        {
            List<JobModel> _model = GetJobs.Get.GetJobs(model);
            if (_model != null && _model.Count > 0)
                return View(_model.FirstOrDefault());

            return View(new JobModel());
        }

        #region Candidate 
         
        [HttpPost]
        public PartialViewResult CandidateUserList(JobCandidateModel model)
        {

            return PartialView("~/Views/Shared/Jobs/_listUserDesigner.cshtml", model);
        }

        [HttpPost]
        public PartialViewResult CardCandidateList(JobModel model)
        {
            return PartialView("~/Views/Jobs/PT/_CardJobCandidate.cshtml", model);
        }

        #endregion


    }
}
