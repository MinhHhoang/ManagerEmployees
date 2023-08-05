using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel;

namespace Datalayer
{
    public interface IMemberProject
    {
        /// <summary>
        /// Tìm kiếm, phân trang
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        IList<MemberProject> List(int ProjectID, int page = 1, int pageSize = 0, string searchValue = "");

        
        /// <summary>
        /// Đếm số nhận viên theo id
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        int Count(int ProjectID);

        /// <summary>
        /// Get thông tin nhân viên theo dự án này
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        MemberProject Get(int EmployeeID, int ProjectID);

        /// <summary>
        /// Bổ sung nhân viên cho dự án
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(MemberProject data);

        /// <summary>
        /// Cập nhật nhân viên cho dự án
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(MemberProject data);

        /// <summary>
        /// Xóa nhận viên trong dự án
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        bool Delete(int EmployeeID, int ProjectID);

  
    }
}
