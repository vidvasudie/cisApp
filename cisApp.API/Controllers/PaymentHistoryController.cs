﻿using System;
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

namespace cisApp.API.Controllers
{ 
    [ApiController]
    public class PaymentHistoryController : BaseController
    {

        public static IConfigurationRoot _config = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("appsettings.json")
                                    .Build();

        
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

            public List<PaymentHistoryModel> PaidPayments { get; set; }
            public List<PaymentHistoryModel> AwaitPaidPayments { get; set; }
        }
    }
}