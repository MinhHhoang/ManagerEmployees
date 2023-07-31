using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DomainModel;

namespace Web.Models
{
    public class MemberProjectPaginationResult : BasePaginationResult
    {
        public int ProjectID { get; set; }
        public List<MemberProject> Data { get; set; }
    }
}