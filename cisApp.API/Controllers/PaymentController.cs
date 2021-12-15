﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cisApp.API.Models;
using cisApp.Function;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using cisApp.Core;


namespace cisApp.API.Controllers
{ 
    [ApiController]
    public class PaymentController : BaseController
    {        
        [Route("api/Payment/Create")]
        [HttpPost]
        public IActionResult PaymentCreate([FromBody] PaymentModel value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    JobPayment data = new JobPayment()
                    {
                        JobId = value.JobId,
                        PayDate = DateTime.Now,
                        PayStatus = 1, // รอดำเนินการ
                    };

                    var payment =  GetJobPayment.Manage.Update(data, value.UserId.Value, value.Ip);

                    return Ok(resultJson.success("บันทึกข้อมูลสำเร็จ", "success", new { JobPayId = payment.JobPayId
                        , JobId = payment.JobId
                        , UserId = value.UserId
                        , PayDate = payment.PayDate
                        , PayExpire = payment.PayDate.Value.AddMinutes(15) }));
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
    }
}