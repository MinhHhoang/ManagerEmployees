using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DomainModel;

namespace Web.Models
{
    public class PhongBanPaginationResult : BasePaginationResult
    {
        public List<PhongBan> Data { get; set; }
    }
}