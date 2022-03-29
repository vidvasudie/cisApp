using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cisApp.Function;
using cisApp.library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace cisApp.Designer.Controllers
{
    [CustomActionExecute]
    public class IncomeListController : BaseController
    {
        private readonly ILogger<IncomeListController> _logger;

        public IncomeListController(ILogger<IncomeListController> logger)
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
            model.UserId = _UserId().Value;
            List<PaymentHistoryModel> _model = GetPaymentHistory.Get.GetBySearch(model);
            int count = GetPaymentHistory.Get.GetBySearchTotal(model);

            return PartialView("PT/_itemlist", new PaginatedList<PaymentHistoryModel>(_model, count, model.currentPage.Value, model.pageSize.Value));
        }
    }
}