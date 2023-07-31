using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DomainModel;
using BusinessLayer;
using System.Web.Mvc;

namespace Web
{
    /// <summary>
    /// 
    /// </summary>
    public static class SelectListHelper
    {
     


        public static List<SelectListItem> Projects()
        {


            List<SelectListItem> list = new List<SelectListItem>();
          
            foreach (var c in CommonDataService.ListOfProjects())
            {
                list.Add(new SelectListItem()
                {
                    Value = Convert.ToString(c.ProjectID),
                    Text = c.NameProject
                });
            }

            return list;
        }

        public static List<SelectListItem> Positions()
        {


            List<SelectListItem> list = new List<SelectListItem>();

            list.Add(new SelectListItem()
            {
                Value = "PM",
                Text = "PM"

            });

            list.Add(new SelectListItem()
            {
                Value = "DEV",
                Text = "DEV"

            });

            list.Add(new SelectListItem()
            {
                Value = "TESTER",
                Text = "TESTER"

            });

            list.Add(new SelectListItem()
            {
                Value = "COMTOR",
                Text = "COMTOR"

            });

            list.Add(new SelectListItem()
            {
                Value = "BA",
                Text = "BA"

            });

            list.Add(new SelectListItem()
            {
                Value = "TL",
                Text = "TL"

            });

            return list;
        }


        public static List<SelectListItem> Ranks()
        {


            List<SelectListItem> list = new List<SelectListItem>();

            list.Add(new SelectListItem()
            {
                Value = "2",
                Text = "Trưởng Phòng"

            });

            list.Add(new SelectListItem()
            {
                Value = "3",
                Text = "Nhân viên"

            });

            

            return list;
        }
        public static List<SelectListItem> Employees()
        {


            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "-- Thành viên --"
            });
            foreach (var c in CommonDataService.ListOfEmployees())
            {
                list.Add(new SelectListItem()
                {
                    Value = Convert.ToString(c.EmployeeID),
                    Text = c.FirstName +" "+ c.LastName

                });
            }

            return list;
        }


        public static List<SelectListItem> StatusProject()
        {


            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "-- Trạng Thái --"
            });
          
            list.Add(new SelectListItem()
            {
                Value = "PROCESSING",
                Text = "PROCESSING"

            });

            list.Add(new SelectListItem()
            {
                Value = "CLOSED",
                Text = "CLOSED"

            });

            return list;
        }


        public static List<SelectListItem> PhongBans()
        {


            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "-- Chọn Phòng ban --"
            });
            foreach (var c in CommonDataService.ListOfPhongBans())
            {
                list.Add(new SelectListItem()
                {
                    Value = c.codePhongBan+"",
                    Text = c.namePhongBan

                });
            }

            return list;
        }


        public static List<SelectListItem> LevelCodes()
        {


            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "-- Chọn Level --"
            });
            list.Add(new SelectListItem()
            {
                Value = "Senior",
                Text = "Senior"

            });

            list.Add(new SelectListItem()
            {
                Value = "Fresher",
                Text = "Fresher"

            });

            list.Add(new SelectListItem()
            {
                Value = "Intern",
                Text = "Intern"

            });

            return list;
        }
    }
}