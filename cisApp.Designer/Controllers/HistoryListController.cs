﻿using System;
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
    public class HistoryListController : BaseController
    {
        private readonly ILogger<HistoryListController> _logger;

        public HistoryListController(ILogger<HistoryListController> logger)
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
            var dmodel = new DesignerJobListSearch() { userId = _UserId().Value, text = model.text, skip = model.currentPage.HasValue ? (model.currentPage - 1) * model.pageSize : 0, take = model.pageSize.HasValue ? model.pageSize.Value : 10 };
            var histList = GetUserDesigner.Get.GetJobsHistory(dmodel);
            List<JobModel> _model = new List<JobModel>();
            if (histList != null)
            {
                foreach (var item in histList)
                {
                    if (_model.Where(o => o.JobId == item.JobId).Count() == 0)
                    {
                        var jobs = GetJobs.Get.GetJobs(new SearchModel { gId = item.JobId });
                        _model.Add(jobs.First());
                    }
                }
            }
            int count = GetUserDesigner.Get.GetJobsHistoryTotal(dmodel);

            return PartialView("PT/_itemlist", new PaginatedList<JobModel>(_model, count, model.currentPage.Value, model.pageSize.Value));
        }

    }
}
