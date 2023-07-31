using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datalayer;
using DomainModel;
using System.Configuration;

namespace BusinessLayer
{
    /// <summary>
    /// Cung cấp các chức năng xử lý dữ liệu chung
    /// </summary>
    public static class CommonDataService
    {
       
        private static readonly IEmployeeDAL<Employee> employeeDB;
        private static readonly ICommonDAL<PhongBan> phongBanDB;
        private static readonly ICommonDAL<Project> projectDB;
        private static readonly IMemberProject memberProjectDB;

        /// <summary>
        /// 
        /// </summary>
        static CommonDataService()
        {
            string provider = ConfigurationManager.ConnectionStrings["DB"].ProviderName;
            string connectionString = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
            switch (provider)
            {
                case "SQLServer":
                    employeeDB = new Datalayer.SQLSever.EmployeeDAL(connectionString);
                    phongBanDB = new Datalayer.SQLSever.PhongBanDAL(connectionString);
                    projectDB = new Datalayer.SQLSever.ProjectDAL(connectionString);
                    memberProjectDB = new Datalayer.SQLSever.MemberProjectDAL(connectionString);
                    break;
                default:
                    break;
            }
        }


        #region các chức năng liên quan đến nhân viên

        public static List<Employee> ListOfEmployees()
        {
            return employeeDB.List("1","").ToList();
        }

        public static List<Employee> ListOfEmployee(string role,string code,int page
                                            , int pageSize
                                            , string searchValue
                                            , out int rowCount)
        {
            rowCount = employeeDB.Count(searchValue);
            return employeeDB.List(role,code,page, pageSize, searchValue).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public static Employee GetEmployee(int EmployeeID)
        {
            return employeeDB.Get(EmployeeID);
        }


        public static bool ChangePassWord(string passOld, string passNew, string email)
        {
            return employeeDB.ChangePassWord(passOld, passNew, email);
        }

        public static Employee CheckAccount(string email, string password)
        {
            return employeeDB.CheckAccount(email, password);
        }


        public static Employee CheckExsitAccount(string email)
        {
            return employeeDB.CheckExistEmail(email);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public static bool DeleteEmployee(int EmployeeID)
        {
            return employeeDB.Delete(EmployeeID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddEmployee(Employee data)
        {
            return employeeDB.Add(data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateEmployee(Employee data)
        {
            return employeeDB.Update(data);
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public static bool InUsedEmployee(int EmployeeID)
        {
            return employeeDB.InUsed(EmployeeID);
        }
        #endregion

        #region Các chức năng liên quan đến phòng ban
        
        public static List<PhongBan> ListOfPhongBans()
        {
            return phongBanDB.List().ToList();
        }

      

        public static List<PhongBan> ListOfPhongBan(int page
                                                    , int pageSize
                                                    , string searchValue
                                                    , out int rowCount)
        {
            rowCount = phongBanDB.Count(searchValue);
            return phongBanDB.List(page, pageSize, searchValue).ToList();
        }

       
        public static PhongBan GetPhongBan(int phongBanID)
        {
            return phongBanDB.Get(phongBanID);
        }

      
        public static bool DeletePhongBan(int phongBanID)
        {
            return phongBanDB.Delete(phongBanID);
        }

       
        public static int AddPhongBan(PhongBan data)
        {
            return phongBanDB.Add(data);
        }

  
        public static bool UpdatePhongBan(PhongBan data)
        {
            return phongBanDB.Update(data);
        }

       
        public static bool InUsedPhongBan(int phongBanID)
        {
            return phongBanDB.InUsed(phongBanID);
        }

        #endregion

        #region các chức năng liên quan đến member project

 

        public static List<MemberProject> ListOfMemberProject(int ProjectID
                                            , int page
                                            , int pageSize
                                            , string searchValue
                                            , out int rowCount)
        {
            rowCount = memberProjectDB.Count(ProjectID);
            return memberProjectDB.List(ProjectID, page, pageSize, searchValue).ToList();
        }

     
        public static MemberProject GetMemberProject(int EmployeeID,int ProjectID)
        {
            return memberProjectDB.Get(EmployeeID,ProjectID);
        }

      
        public static bool DeleteMemberProject(int EmployeeID, int ProjectID)
        {
            return memberProjectDB.Delete(EmployeeID, ProjectID);
        }

   
        public static int AddMemberProject(MemberProject data)
        {
            return memberProjectDB.Add(data);
        }

  
        public static bool UpdateMemberProject(MemberProject data)
        {
            return memberProjectDB.Update(data);
        }

        #endregion

        #region các chức năng liên quan đến project

        public static List<Project> ListOfProjects()
        {
            return projectDB.List().ToList();
        }

        public static List<Project> ListOfProject(int page
                                            , int pageSize
                                            , string searchValue
                                            , out int rowCount)
        {
            rowCount = projectDB.Count(searchValue);
            return projectDB.List(page, pageSize, searchValue).ToList();
        }


        public static Project GetProject(int ProjectID)
        {
            return projectDB.Get(ProjectID);
        }


        public static bool DeleteProject(int ProjectID)
        {
            return projectDB.Delete(ProjectID);
        }


        public static int AddProject(Project data)
        {
            return projectDB.Add(data);
        }


        public static bool UpdateProject(Project data)
        {
            return projectDB.Update(data);
        }


        public static bool InUsedProject(int ProjectID)
        {
            return projectDB.InUsed(ProjectID);
        }
        #endregion
    }
}
