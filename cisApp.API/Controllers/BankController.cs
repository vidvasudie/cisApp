using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cisApp.API.Models;
using cisApp.Common;
using cisApp.Function;
using cisApp.library;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace cisApp.API.Controllers
{ 
    [ApiController]
    public class BankController : BaseController
    {
        [Route("api/mail")]
        [HttpGet]
        public IActionResult Get(string mail)
        {
            //string webAdmin = config.GetSection("WebConfig:MobileLink").Value;

            EmailModel emailModel = new EmailModel()
            {
                ToMail = mail,//"vidvasudie@gmail.com",
                Subject = "คำขอรีเซ็ตรหัสผ่าน",
                TemplateFileName = "UserResetPassword.htm",
                Body = new List<string>()
                            {
                                " test mail ", "ResetPassword/ChangPassword/" 
                            }
            };
            try
            {
                SendMail.Send(emailModel, GetSystemSetting.Get.GetEmailSettingModel());
            }
            catch (Exception ex)
            {
                return Ok(new { status = true, msg=ex.ToString() });
            }

            return Ok(new { status=true });
        }
        [Route("api/bank/profile")]
        [HttpGet]
        public IActionResult GetProfileBank(Guid userId)
        {
            try
            {
                LogActivityEvent(LogCommon.LogMode.BANK_PROFILE, _UserId());

                if (userId == Guid.Empty)
                {
                    return BadRequest(resultJson.errors("parameter ไม่ถูกต้อง", "Invalid Request.", null));
                }
                var dsUser = GetUserDesigner.Get.GetBankProfile(userId);
                if(dsUser == null)
                {
                    return Ok(resultJson.success("ไม่พบข้อมูล", "Data not found.", null));
                }
                 
                return Ok(resultJson.success("ดึงข้อมูลสำเร็จ", "success", dsUser));
            }
            catch(Exception ex)
            {
                return Ok(resultJson.errors("ดึงข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        [Route("api/bank/update")]
        [HttpPut]
        public IActionResult GetUpdateBank([FromBody] BankModel model)
        {
            try
            { 
                var data = GetUserDesigner.Manage.UpdateBankProfile(model);
                if(data == null)
                {
                    return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ ไม่พบข้อมูลนักออกแบบ", "fail", null));
                }
                var dsUser = GetUserDesigner.Get.GetBankProfile(model.UserId);

                LogActivityEvent(LogCommon.LogMode.BANK_EDIT, _UserId(), MessageCommon.SaveSuccess);
                return Ok(resultJson.success("บันทึกข้อมูลสำเร็จ", "success", dsUser));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ", "fail", ex));
            }
            
        }

        [Route("api/bank/banklist")]
        [HttpGet]
        public IActionResult GetBankList()
        {
            try
            {
                //var xx = Encryption.Decrypt("s9LrP8c+HjTWUbLOve8Xhg==");
                var banks = GetTmBank.Get.GetByActive();
                if (banks == null)
                {
                    return Ok(resultJson.success("ไม่พบข้อมูล", "Data not found.", null));
                }

                return Ok(resultJson.success("ดึงข้อมูลสำเร็จ", "success", banks.Select(o => new { id=o.Id, name=o.Name })));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("ดึงข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        [Route("api/bank/accounttypelist")]
        [HttpGet]
        public IActionResult GetBankAccountTypeList()
        {
            try
            {
                var banks = GetTmBankAccountType.Get.GetByActive();
                if (banks == null)
                {
                    return Ok(resultJson.success("ไม่พบข้อมูล", "Data not found.", null));
                }

                return Ok(resultJson.success("ดึงข้อมูลสำเร็จ", "success", banks.Select(o => new { id = o.Id, name = o.Name })));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("ดึงข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }



    }
}