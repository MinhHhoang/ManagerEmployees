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
    [RoutePrefix("Project")]
    /// <summary>
    /// 
    /// </summary>
    public class ProjectController : Controller
    {
        public ActionResult Index(int page = 1, string searchValue = "")
        {

            int pageSize = 10;
            int rowCount = 0;
            string role = this.Session["ROLE"] as string;
            var data = BusinessLayer.CommonDataService.ListOfProject(page, pageSize, searchValue, out rowCount);
            Models.ProjectPaginationResult model = new Models.ProjectPaginationResult()
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
            Project model = new Project()
            {
                ProjectID = 0
            };
            ViewBag.Title = "Bổ sung Project";
            return View(model);
        }

        [Route("HistoryGit/{LinkSource}")]
        public ActionResult HistoryGit(string LinkSource)
        {
           
            ViewBag.linksource = LinkSource;
            return View();
        }

        [HttpPost]
        public ActionResult Save(Project model, string StartDateString, string EndDateString)
        {
            //TODO : Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(model.NameProject))
                ModelState.AddModelError("NameProject", "Không được bỏ trống thông tin");
            if (string.IsNullOrWhiteSpace(model.StatusProject))
                ModelState.AddModelError("StatusProject", "Không được bỏ trống thông tin");
            if (string.IsNullOrWhiteSpace(model.Description))
                ModelState.AddModelError("Email", "Không được bỏ trống thông tin");
            if (string.IsNullOrWhiteSpace(model.StatusProject) || model.StatusProject.Equals(""))
                ModelState.AddModelError("StatusProject", "Vui lòng chọn trạng thái project");
            if (string.IsNullOrWhiteSpace(Convert.ToString(model.ProjectManager)) || Convert.ToString(model.ProjectManager).Equals(""))
                ModelState.AddModelError("ProjectManager", "Vui lòng chọn người quản lý");


            DateTime mindate = DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture);

            // Xử lý ngày sinh
            DateTime startDate = new DateTime();
            try
            {
                startDate = DateTime.ParseExact(StartDateString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                if (startDate < mindate)
                    ModelState.AddModelError("StartDate", "Ngày tháng không hợp lệ, vui lòng chọn lại");
                else 
                    model.StartDate = startDate;
            }
            catch
            {
                ModelState.AddModelError("StartDate", "Ngày tháng không hợp lệ, vui lòng chọn lại");
            }


            if(model.StatusProject.Equals("CLOSED"))
            {
                DateTime endDate;
                try
                {
                    endDate = DateTime.ParseExact(EndDateString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    if (endDate < mindate  || endDate < startDate)
                        ModelState.AddModelError("FinishDate", "Ngày tháng không hợp lệ, vui lòng chọn lại");
                    else
                        model.FinishDate = endDate;
                }
                catch
                {
                    ModelState.AddModelError("FinishDate", "Ngày tháng không hợp lệ, vui lòng chọn lại");
                }
            }
           
           

            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.ProjectID == 0 ? "Bổ sung project" : "Cập nhật project";
                return View("Create", model);
            }



            if (model.ProjectID > 0)
                CommonDataService.UpdateProject(model);
            else
                CommonDataService.AddProject(model);
            return RedirectToAction("Index");
        }


        [Route("edit/{ProjectID}")]
        public ActionResult Edit(int ProjectID)
        {
            Project model = CommonDataService.GetProject(ProjectID);
            if (model == null)
                return RedirectToAction("Index");

            ViewBag.Title = "Cập nhật project";
            return View("Create", model);
        }

        [Route("delete/{ProjectID}")]
        public ActionResult Delete(int ProjectID)
        {
            if (Request.HttpMethod == "POST")
            {
                CommonDataService.DeleteProject(ProjectID);
                return RedirectToAction("Index");
            }

            var model = CommonDataService.GetProject(ProjectID);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
    }
}