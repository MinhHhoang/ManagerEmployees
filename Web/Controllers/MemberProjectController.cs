using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer;
using DomainModel;

namespace Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    [RoutePrefix("MemberProject")]
    /// <summary>
    /// 
    /// </summary>
    public class MemberProjectController : Controller
    {
        public ActionResult Index(string ProjectID, int page = 1, string searchValue = "")
        {

         
            int pageSize = 10;
            int rowCount = 0;
            string role = this.Session["ROLE"] as string;
            var data = BusinessLayer.CommonDataService.ListOfMemberProject( Convert.ToInt32(ProjectID), page, pageSize, searchValue, out rowCount);
            Models.MemberProjectPaginationResult model = new Models.MemberProjectPaginationResult()
            {
                ProjectID = Convert.ToInt32(ProjectID),
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
            MemberProject model = new MemberProject()
            {
                ProjectID = 0
            };
            ViewBag.Title = "Bổ sung thành tiền";
            return View(model);
        }

        [HttpPost]
        public ActionResult Save(MemberProject model)
        {
            if (string.IsNullOrWhiteSpace(Convert.ToString(model.EmployeeID)) || Convert.ToString(model.EmployeeID).Equals(""))
                ModelState.AddModelError("EmployeeID", "Không được bỏ trống thông tin");
            if (string.IsNullOrWhiteSpace(model.Position))
                ModelState.AddModelError("Position", "Không được bỏ trống thông tin");


            if (!ModelState.IsValid)
            {
                return View("Create", model);
            }

            int result = CommonDataService.AddMemberProject(model);

            if (result == 0)
            {
                ModelState.AddModelError("EmployeeID", "Nhân viên đã tồn tại trong dự án này rồi.");
                return View("Create", model);
            }

            return RedirectToAction("Index");
        }


        

        [Route("Delete/{EmployeeID}/{ProjectID}")]
        public ActionResult Delete(int EmployeeID,int ProjectID)
        {
            if (Request.HttpMethod == "POST")
            {
                CommonDataService.DeleteMemberProject(EmployeeID, ProjectID);
                return RedirectToAction("Index");
            }

            var model = CommonDataService.GetMemberProject(EmployeeID, ProjectID);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
    }
}