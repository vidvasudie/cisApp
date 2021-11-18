using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using cisApp.library;
using cisApp.Core;
using cisApp.Function;
using cisApp.Models;

namespace cisApp.Controllers
{
    public class userApproveController : Controller
    {
        private List<UserModel> _model = new List<UserModel>(){
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
                new UserModel(),
            };
        public IActionResult Index(int pageIndex=1)
        { 

            return View(new PaginatedList<UserModel>(_model, _model.Count, pageIndex, 2));
        }
        [HttpPost]
        public PartialViewResult ItemList(int? currentPage = 1, int? pageSize = 10)
        {
            var users = GetUser.Get.GetAll();  
            _model.Clear();
            foreach (var u in users)
            {
                _model.Add(new UserModel() { 
                    UserId = u.UserId,
                    Fname = u.Fname,
                    Lname = u.Lname,
                    UserType = u.UserType,
                    Tel = u.Tel,
                    Email = u.Email,
                    designer = GetUserDesigner.Get.GetByUserId(u.UserId.Value)
                });;
            }
            foreach (var um in _model)
            {
                um.UserTypeDesc = GetTmUserType.Get.GetById(um.UserType.Value).Name;
                if (um.UserType == 2 && um.designer != null)
                {
                    um.ProvinceDesc = GetTmProvince.Get.GetById(um.designer.ProvinceId.Value).NameTh;
                    um.DistrictDesc = GetTmDistrict.Get.GetById(um.designer.DistrictId.Value).NameTh;
                    um.SubDistrictDesc = GetTmSubDistrict.Get.GetById(um.designer.SubDistrictId.Value).NameTh;
                    um.BankName = GetTmBank.Get.GetById(um.designer.BankId.Value).Name;
                    um.AccountTypeDesc = GetTmBankAccountType.Get.GetById(um.designer.AccountType.Value).Name;
                }
            }

            // ทดสอบข้อมูลจำลอง 

            return PartialView("PT/_itemlist", new PaginatedList<UserModel>(_model, _model.Count, currentPage.Value, pageSize.Value)); 
        }

        public IActionResult Manage()
        {
            return View();
        }
    }
}