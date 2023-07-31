using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DomainModel;

namespace Web.Models
{
    public class ProjectPaginationResult : BasePaginationResult
    {
        public List<Project> Data { get; set; }
    }
}