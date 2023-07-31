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
    /// Account controller điều khiển liên quan tới view account
    /// </summary>
    ///
    public class AccountController : Controller
    {
        /// <summary>
        /// Return về giao diện đăng nhập
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]

        public ActionResult Login()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult login(string username, string password)
        {
            Employee emp = CommonDataService.CheckAccount(username, password);
            if(emp != null) // đăng nhập thành công
            {
                System.Web.Security.FormsAuthentication.SetAuthCookie(username, false); // set cookie cho phiên đăng nhập
                this.Session["EMAIL"] = Convert.ToString(emp.Email); // set session cho account 
                this.Session["PHOTO"] = Convert.ToString(emp.Photo); // set session cho account 
                this.Session["CODEPHONGBAN"] = Convert.ToString(emp.RoleAccount); // set session cho account 
                this.Session["ROLE"] = Convert.ToString(emp.RoleAccount); // set session cho account 
                return RedirectToAction("Index", "Home"); // đăng nhập thành công di chuyển tới giao diện trang chủ
            }
            ViewBag.UserName = username;
            ViewBag.Message = "Đăng Nhập Thất bại"; // gửi về message đăng nhập thất bại
            return View(); // đăng nhập thất bại trả về view login lại
        }


       
        /// <summary>
        /// Đăng xuất và di chuyển tới page login
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login");
        }
        /// <summary>
        /// Return về giao diện đổi mật khẩu
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePass(string oldPassword, string newPassword)
        {
            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(oldPassword))
                ViewBag.errorOld = "Không được bỏ trống thông tin";

            if (string.IsNullOrWhiteSpace(newPassword))
                ViewBag.errorNew = "Không được bỏ trống thông tin";

            if (string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrWhiteSpace(newPassword))
                return View("ChangePassword");
            string email = this.Session["EMAIL"] as string;
            if (CommonDataService.ChangePassWord(oldPassword,newPassword, email))
            {
                ViewBag.success = "Change password thành công";
                return View("ChangePassword");
            }

            ViewBag.errorOld = "Password cũ không đúng vui lòng check lại";
            return View("ChangePassword");
        }
    }
}