using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    public class MemberProject
    {
        public int ProjectID { get; set; }
        public int EmployeeID { get; set; }
        public string NameProject { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }

        public string LevelCoding { get; set; }
        public string Position { get; set; }

        
    }
}
