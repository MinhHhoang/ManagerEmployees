using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    /// <summary>
    /// 
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// 
        /// </summary>
        public int EmployeeID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime BirthDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Notes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Photo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Email { get; set; }


        public int CountProject { get; set; }

        public string LevelCoding { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int CodePhongBan { get; set; }

         public string NamePhongBan { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string LanguageProgram { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int RoleAccount { get; set; }

        public string checkRole { get; set; }
    }
}
