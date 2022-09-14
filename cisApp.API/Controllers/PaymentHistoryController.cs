using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cisApp.API.Models;
using cisApp.Function;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using cisApp.Core;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Globalization;

namespace cisApp.API.Controllers
{ 
    [ApiController]
    public class PaymentHistoryController : BaseController
    { 
        
        [Route("api/PaymentHistory/Sum")]
        [HttpGet]
        public object Get(Guid? userId, int? month, int? year)
        {
            
            try
            {

                var data = GetPaymentHistory.Get.GetByMonthYear(userId.Value, month, year);

                PaymentHistorySummaryModel model = new PaymentHistorySummaryModel()
                {
                    PaidPayments = data.Where(o => o.IsPaid == true).ToList(),
                    AwaitPaidPayments = data.Where(o => o.IsPaid == false).ToList()
                };
                return Ok(resultJson.success(null, null, model, null, data.Count, null, null));

            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        public class PaymentHistorySummaryModel
        {
            public decimal PaidSum { 
                get
                {
                    try
                    {
                        decimal sum = PaidPayments.Select(o => o.Amount).Sum();

                        return sum;
                    }
                    catch (Exception ex)
                    {
                        return 0m;
                    }
                } 
            }


            public decimal AwaitPaidSum
            {
                get
                {
                    try
                    {
                        decimal sum = AwaitPaidPayments.Select(o => o.Amount).Sum();

                        return sum;
                    }
                    catch (Exception ex)
                    {
                        return 0m;
                    }
                }
            }

            public string PaidSumTurn {
                get
                {
                    try
                    {
                        var paymentHistoryDate = GetPaymentHistoryDate.Get.GetDefault();

                        DateTime dateNow = DateTime.Now;
                        DateTime dateStart = new DateTime(dateNow.Year, dateNow.Month, paymentHistoryDate.Day.Value);

                        CultureInfo cultureInfo = new CultureInfo("th-TH");
                        string dateString = dateStart.ToString("dd/MM/yyyy", cultureInfo);

                        return dateString;

                        //DateTime dateNow = DateTime.Now;
                        //DateTime dateStart = new DateTime(dateNow.Year, dateNow.Month, paymentHistoryDate.Day.Value);
                        //DateTime dateEnd = new DateTime(dateNow.Year, dateNow.Month, paymentHistoryDate.Day.Value);

                        //if (dateNow.Day > paymentHistoryDate.Day.Value)
                        //{
                        //    dateEnd.AddMonths(1);
                        //}
                        //else
                        //{
                        //    dateStart.AddMonths(-1);
                        //}

                        //decimal sum = AwaitPaidPayments.Select(o => o.Amount).Sum();
                        //sum += PaidPayments.Select(o => o.Amount).Sum();

                        //return sum;
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                }
            }

            public List<PaymentHistoryModel> PaidPayments { get; set; }
            public List<PaymentHistoryModel> AwaitPaidPayments { get; set; }
        }
    }
}