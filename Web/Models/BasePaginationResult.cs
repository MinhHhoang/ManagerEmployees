using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    /// <summary>
    /// Lớp cơ sở (Lớp cha) của các lớp dùng để lưu trữ giữ các kết quả liên quan đến tìm kiếm, phân trang
    /// </summary>
    public abstract class BasePaginationResult
    {
        /// <summary>
        /// Số trang
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// Kích thước của trang
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Từ khóa tìm kiếm 
        /// </summary>
        public string SearchValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int RowCount { get; set; }

        public string RoleAccount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int PageCount
        {
            get
            {
                if (PageSize == 0)
                    return 1;
                int p = RowCount / PageSize;
                if (RowCount % PageSize > 0)
                {
                    p += 1;
                }
                return p;
            }
        }

    }



}
