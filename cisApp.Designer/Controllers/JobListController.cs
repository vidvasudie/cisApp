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
            var dmodel = new DesignerJobListSearch() { userId = _UserId().Value, text = model.text, skip = model.currentPage.HasValue ? (model.currentPage - 1) * model.pageSize:0, take = model.pageSize.HasValue ? model.pageSize.Value:10 };
            var djobs = GetUserDesigner.Get.GetJobListSearch(dmodel, true);// 
            List<JobModel> _model = new List<JobModel>();
            if (djobs != null)
            {
                foreach (var item in djobs)
                {
                    if (_model.Where(o => o.JobId == item.JobID).Count() == 0)
                    {
                        var jobs = GetJobs.Get.GetJobs(new SearchModel { gId = item.JobID });
                        _model.Add(jobs.First());
                    } 
                }
            } 
            int count = GetUserDesigner.Get.GetJobListSearchTotal(dmodel, true);

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
