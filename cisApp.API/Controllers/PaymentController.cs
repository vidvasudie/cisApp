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
    public class PaymentController : BaseController
    {

        public static IConfigurationRoot _config = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("appsettings.json")
                                    .Build();

        [Route("api/Payment/Create")]
        [HttpPost]
        public IActionResult PaymentCreate([FromBody] PaymentModel value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var job = GetJobs.Get.GetById(value.JobId.Value);

                    //if (job.JobCaUserId == null)
                    //{
                    //    return Ok(resultJson.errors("ใบงานดังกล่าวยังไม่มีผู้ผ่านการประกวด", "fail", null));
                    //}

                    //var candidate = GetUser.Get.GetById(job.JobCaUserId.Value);

                    JobPayment data = new JobPayment()
                    {
                        JobId = value.JobId,
                        PayDate = DateTime.Now,
                        PayStatus = 1, // รอดำเนินการ
                    };

                    var payment =  GetJobPayment.Manage.Update(data, value.UserId.Value, value.Ip);

                    string accountBankName = _config.GetSection("Account:AccountBankName").Value;
                    string accountName = _config.GetSection("Account:AccountName").Value;
                    string accountNumber = _config.GetSection("Account:AccountNumber").Value;

                    return Ok(resultJson.success("บันทึกข้อมูลสำเร็จ", "success", new { JobPayId = payment.JobPayId
                        , JobId = payment.JobId
                        , UserId = value.UserId
                        , PayDate = payment.PayDate
                        , PayExpire = payment.PayDate.Value.AddMinutes(15)
                    , JobFinalPrice = job.JobFinalPrice
                    , CandidateAccount = "0000000000"
                    , AccountBankName = accountBankName
                    , AccountName = accountName
                    , AccountNumber = accountNumber
                    }));
                }

                return Ok(resultJson.errors("JobId And UserId Is Ruqired", "fail", null));                
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ", "fail", ex));
            }
            
        }

        [Route("api/Payment/AddSlip")]
        [HttpPost]
        public IActionResult PaymentAddSlip([FromBody] PaymentModel value)
        {
            try
            {
                if (value.JobPaymentId == null || value.AttachId == null)
                {
                    return Ok(resultJson.errors("JobPaymentId And AttachId Is Ruqired", "fail", null));
                }

                var payment = GetJobPayment.Manage.AddSlip(value.JobPaymentId.Value, 2, value.AttachId.Value, value.UserId.Value, value.Ip);

                return Ok(resultJson.success("บันทึกข้อมูลสำเร็จ", "success", new
                {
                    JobPaymentId = payment.JobPayId
                    ,
                    JobId = payment.JobId
                    ,
                    UserId = value.UserId
                }));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ", "fail", ex));
            }

        }

        [Route("api/Payment/History")]
        [HttpGet]
        public object Get(Guid? userId, int? month, int? year, int? page = 1, int limit = 10)
        {
            
            try
            {
                SearchModel model = new SearchModel()
                {
                    UserId = userId.Value,
                    currentPage = page,
                    pageSize = limit,
                    Year = year,
                    Month = month
                };

                var Obj = GetJobPayment.Get.GetBySearch(model);
                var total = GetJobPayment.Get.GetBySearchTotal(model);

                return Ok(resultJson.success(null, null, Obj, null, total, page, page + 1));

            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }
    }
}