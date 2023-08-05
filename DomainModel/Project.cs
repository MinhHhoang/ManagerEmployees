using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    public class Project
    {
        public int ProjectID { get; set; }
        public string NameProject { get; set; }
        public int ProjectManager { get; set; }

        public string LastName { get; set; }
        public string FirstName { get; set; }

        public string StatusProject { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public string Description { get; set; }

        public string LinkSource { get; set; }

    }
}
