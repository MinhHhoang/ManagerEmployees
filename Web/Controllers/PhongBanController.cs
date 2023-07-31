using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel;
using BusinessLayer;

namespace Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    [RoutePrefix("phongban")]
    /// <summary>
    /// 
    /// </summary>
    public class PhongBanController : Controller
    {
        public ActionResult Index(int page = 1, string searchValue = "")
        {

            int pageSize = 10;
            int rowCount = 0;
            string role = this.Session["ROLE"] as string;
            var data = BusinessLayer.CommonDataService.ListOfPhongBan(page, pageSize, searchValue, out rowCount);
            Models.PhongBanPaginationResult model = new Models.PhongBanPaginationResult()
            {
                Page = page,
                PageSize = pageSize,
                SearchValue = searchValue,
                RowCount = rowCount,
                Data = data,
                RoleAccount = role
            };
            return View(model);
        }
        public ActionResult Create()
        {
            PhongBan model = new PhongBan()
            {
                codePhongBan = 0
            };
            ViewBag.Title = "Bổ sung phòng ban";
            return View(model);
        }


        [Route("edit/{phongBanID}")]
        public ActionResult Edit(int phongBanID)
        {
            PhongBan model = CommonDataService.GetPhongBan(phongBanID);
            if (model == null)
                return RedirectToAction("Index");

            ViewBag.Title = "Cập nhật phòng ban";
            return View("Create", model);
        }

        /// <summary>
        /// Lưu dữ liệu
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Save(PhongBan model)
        {
            //TODO: Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(model.namePhongBan))
                ModelState.AddModelError("namePhongBan", "Tên phòng ban không được để trống");



            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.codePhongBan == 0 ? "Bổ sung phòng ban" : "Cập nhật phòng ban";
                return View("Create", model);
            }

            if (model.codePhongBan > 0)
                CommonDataService.UpdatePhongBan(model);
            else
                CommonDataService.AddPhongBan(model);
            return RedirectToAction("Index");
        }
        [Route("delete/{phongBanID}")]
        public ActionResult Delete(int phongBanID)
        {
            if (Request.HttpMethod == "POST")
            {
                CommonDataService.DeletePhongBan(phongBanID);
                return RedirectToAction("Index");
            }

            var model = CommonDataService.GetPhongBan(phongBanID);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
    }
}