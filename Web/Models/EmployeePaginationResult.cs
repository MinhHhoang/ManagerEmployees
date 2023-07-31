using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DomainModel;

namespace Web.Models
{
    public class EmployeePaginationResult : BasePaginationResult
    {
        public List<Employee> Data { get; set; }
    }
}