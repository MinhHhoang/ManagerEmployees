using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer
{
    /// <summary>
    /// Là interface , là bản mẫu để cho các class khác impliment có cùng phương thức xử lý
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICommonDAL<T> where T : class
    {
        /// <summary>
        /// Tìm kiếm, phân trang
        /// </summary>
        /// <param name="page">Trang cần xem</param>
        /// <param name="pageSize">Số dòng trên mỗi trang (0 nếu không phân trang)</param>
        /// <param name="searchValue">Giá trị tìm kiềm (rỗng nếu bỏ qua)</param>
        /// <returns></returns>
        IList<T> List(int page = 1, int pageSize = 0, string searchValue = "");

        /// <summary>
        /// Đếm số item theo từ khóa searchValue
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue = "");

        /// <summary>
        /// Lấy thông tin của đối tượng đó
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        T Get(int ID);

        /// <summary>
        /// Bổ sung 1 dối tượng vào table tương ứng với table đó
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(T data);

        /// <summary>
        /// Update 1 dối tượng vào table tương ứng với table đó
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(T data);

        /// <summary>
        /// Xóa một đối tượng trong table
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        bool Delete(int ID);

        /// <summary>
        /// Kiểm tra dối tượng có khóa ngoại ở các table khác hay không
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>bool</returns>
        bool InUsed(int id);
    }
}
