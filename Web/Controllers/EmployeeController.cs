using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer;
using DomainModel;
using Excel = Microsoft.Office.Interop.Excel;

namespace Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    [RoutePrefix("Employee")]
    /// <summary>
    /// 
    /// </summary>
    public class EmployeeController : Controller
    {
        public ActionResult Index(int page = 1, string searchValue = "")
        {

            int pageSize = 10;
            int rowCount = 0;
            string role = this.Session["ROLE"] as string;
            string codePhongBan = this.Session["CODEPHONGBAN"] as string;
            var data = BusinessLayer.CommonDataService.ListOfEmployee(role,codePhongBan, page, pageSize, searchValue, out rowCount);
            Models.EmployeePaginationResult model = new Models.EmployeePaginationResult()
            {
                Page = page,
                PageSize = pageSize,
                SearchValue = searchValue,
                RowCount = rowCount-1,
                Data = data,
                RoleAccount = role
            };
            return View(model);
        }
        public ActionResult Create()
        {
            string role = this.Session["ROLE"] as string;
            Employee model = new Employee()
            {
                EmployeeID = 0,
                checkRole = role
            };

            ViewBag.Title = "Bổ sung nhân viên";
            return View(model);
        }


        public ActionResult CreateMutiEmp()
        {
         
            return View();
        }

        [HttpPost]
        public ActionResult Save(Employee model, string birthDateString, HttpPostedFileBase uploadPhoto)
        {

            string role = this.Session["ROLE"] as string;
            string codePhongBan = this.Session["CODEPHONGBAN"] as string;


            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(model.LastName))
                ModelState.AddModelError("LastName", "Không được bỏ trống thông tin");
            if (string.IsNullOrWhiteSpace(model.FirstName))
                ModelState.AddModelError("FirstName", "Không được bỏ trống thông tin");
            if (string.IsNullOrWhiteSpace(model.LastName))
                ModelState.AddModelError("Email", "Không được bỏ trống thông tin");
            if (string.IsNullOrWhiteSpace(model.Email))
                ModelState.AddModelError("Email", "Không được bỏ trống thông tin");
            if (string.IsNullOrWhiteSpace(model.LevelCoding) || model.LevelCoding.Equals(""))
                ModelState.AddModelError("LevelCoding", "Vui lòng chọn level trình độ");
            if(role.Equals("1"))
            {
                if (string.IsNullOrWhiteSpace(Convert.ToString(model.CodePhongBan)) || Convert.ToString(model.CodePhongBan).Equals(""))
                    ModelState.AddModelError("CodePhongBan", "Không được bỏ trống thông tin");
            } else
            {
                model.CodePhongBan = Int32.Parse(codePhongBan);
                model.RoleAccount = 3;
            }


            // Xử lý ngày sinh
            DateTime birthday;
            try
            {
                birthday = DateTime.ParseExact(birthDateString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime mindate = DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", CultureInfo.InvariantCulture);
                if (birthday < mindate)
                    ModelState.AddModelError("BirthDate", "Ngày tháng không hợp lệ, vui lòng chọn lại");
                else
                    model.BirthDate = birthday;
            }
            catch
            {
                ModelState.AddModelError("BirthDate", "Ngày tháng không hợp lệ");
            }

            // Xử lý ảnh

            if (uploadPhoto != null)
            {
                string path = Server.MapPath("~/images/employees");
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                string uploadFilePath = System.IO.Path.Combine(path, fileName);
                uploadPhoto.SaveAs(uploadFilePath);
                model.Photo = $"/images/employees/{fileName}";

            }

            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.EmployeeID == 0 ? "Bổ sung nhân viên" : "Cập nhật nhân viên";
                return View("Create", model);
            }



            if (model.EmployeeID > 0)
                CommonDataService.UpdateEmployee(model);
            else
                CommonDataService.AddEmployee(model);
            return RedirectToAction("Index");
        }

        

        [HttpPost]
            public ActionResult ImportFile(HttpPostedFileBase fileExcel){

                if(fileExcel== null ||fileExcel.ContentLength == 0)
                {
                    ViewBag.Error = " Làm ơn chọn file để import";
                    return View("CreateMutiEmp");
                } else{
                    string path = Server.MapPath("~/Content/" + $"{DateTime.Now.Ticks}_{fileExcel.FileName}");
                    if(System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    fileExcel.SaveAs(path);
                    Excel.Application xlApp = new Excel.Application();
                    Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(path);
                    Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                    Excel.Range xlRange = xlWorksheet.UsedRange;
                    string rowerror = "";
                    for (int row = 2; row < xlRange.Rows.Count; row++)
                    {
                   
                        string email = ((Excel.Range)xlRange.Cells[row, 5]).Text;
                        Employee epm = CommonDataService.CheckExsitAccount(email);
                        if(epm == null )
                        {
                            try
                            {
                                epm = new Employee()
                                {
                                    LastName = ((Excel.Range)xlRange.Cells[row, 1]).Text,
                                    FirstName = ((Excel.Range)xlRange.Cells[row, 2]).Text,
                                    BirthDate = Convert.ToDateTime(((Excel.Range)xlRange.Cells[row, 3]).Text),
                                    Photo = ((Excel.Range)xlRange.Cells[row, 4]).Text,
                                    Email = ((Excel.Range)xlRange.Cells[row, 5]).Text,
                                    CountProject = Convert.ToInt32(((Excel.Range)xlRange.Cells[row, 6]).Text),
                                    LevelCoding = ((Excel.Range)xlRange.Cells[row, 7]).Text,
                                    CodePhongBan = Convert.ToInt32(((Excel.Range)xlRange.Cells[row, 8]).Text),
                                    LanguageProgram = ((Excel.Range)xlRange.Cells[row, 9]).Text,
                                    Notes = ((Excel.Range)xlRange.Cells[row, 10]).Text,
                                };
                            }
                            catch
                            {
                                rowerror = rowerror + "row " + row + "-";
                                continue;
                            }

                            if (CommonDataService.AddEmployee(epm) == 0)
                            {
                                rowerror = rowerror + "row " + row + "-";
                            }
                        } else
                        {
                            rowerror = rowerror + "row " + row + "-";
                        }
                    }
                    if (!rowerror.Equals(""))
                    {
                        ViewBag.Error = rowerror + " Vui lòng check lại data, lỗi phòng ban không tồn tại hoặc datebirt hoặc email đã trùng";
                        return View("CreateMutiEmp");
                    }
                    else
                    {
                    return RedirectToAction("Index");
                }

                }

            
            }

 

        [Route("edit/{EmployeeID}")]
        public ActionResult Edit(int EmployeeID)
        {
            string role = this.Session["ROLE"] as string;
            string codePhongBan = this.Session["CODEPHONGBAN"] as string;


            Employee model = CommonDataService.GetEmployee(EmployeeID);
            if (model == null)
                return RedirectToAction("Index");
            if (role.Equals("1"))
            {
                model.checkRole = "1";
            }
            else model.checkRole = "0";
            ViewBag.Title = "Cập nhật nhân viên";
            return View("Create", model);
        }

        [Route("delete/{EmployeeID}")]
        public ActionResult Delete(int EmployeeID)
        {
            if (Request.HttpMethod == "POST")
            {
                CommonDataService.DeleteEmployee(EmployeeID);
                return RedirectToAction("Index");
            }

            var model = CommonDataService.GetEmployee(EmployeeID);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
    }
}