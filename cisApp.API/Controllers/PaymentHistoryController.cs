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
                return Ok(resultJson.success(null, null, data, null, data.Count, null, null));

            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }
    }
}