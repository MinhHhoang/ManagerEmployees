using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer;
using DomainModel;


namespace Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            ViewBag.Data1 = "";
            List<Employee> listEmp = CommonDataService.ListOfEmployees();
            int countIntern = 0;
            int couttFresher = 0;
            int countSenior = 0;
            foreach(Employee e in listEmp)
            {
                if (e.LevelCoding.Equals("Senior")) countSenior++;
                if (e.LevelCoding.Equals("Fresher")) couttFresher++;
                if (e.LevelCoding.Equals("Intern")) countIntern++;
            }


            List<Project> listProject = CommonDataService.ListOfProjects();
            int countCLOSED = 0;
            int couttPROCESSING = 0;
            foreach (Project e in listProject)
            {
                if (e.StatusProject.Equals("CLOSED")) countCLOSED++;
                if (e.StatusProject.Equals("PROCESSING")) couttPROCESSING++;
            }

            ViewBag.Data1 = countSenior + " " + couttFresher + " " + countIntern + " " + countCLOSED + " " + couttPROCESSING;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //public ActionResult Categories()
        //{
            //var model = BusinessLayer.CommonDataService.ListOfCategories();
           // return View(model);
        //}

    }
}