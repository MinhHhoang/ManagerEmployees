using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer
{
    /// <summary>
    /// Interface các phương thức cho class Employee
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEmployeeDAL<T> where T : class
    {
        /// <summary>
        /// Tìm kiếm, phân trang
        /// </summary>
        /// <param name="page">Trang cần xem</param>
        /// <param name="pageSize">Số dòng trên mỗi trang (0 nếu không phân trang)</param>
        /// <param name="searchValue">Giá trị tìm kiềm (rỗng nếu bỏ qua)</param>
        /// <returns></returns>
        IList<T> List(string role, string code,int page = 1, int pageSize = 0, string searchValue = "");

        /// <summary>
        /// Đếm số nhân viên thoe value search
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue = "");

        /// <summary>
        /// Lấy thông tin nhân viên theo ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        T Get(int ID);

        /// <summary>
        /// Bổ sung 1 nhân viên
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(T data);

        /// <summary>
        /// Cập nhật nhân viên
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(T data);

        /// <summary>
        /// Xóa nhân viên
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        bool Delete(int ID);

        /// <summary>
        /// Kiểm tra nhân viên có tồn tại trong table nào không
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        bool InUsed(int id);

        /// <summary>
        /// Kiểm tra đăng nhập tài khoản
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        T CheckAccount(string email, string password);
        /// <summary>
        /// Kiểm tra email đăng ký có tồn tại trong table Employee không
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        T CheckExistEmail(string email);

        /// <summary>
        /// Change password employee by email
        /// </summary>
        /// <param name="passold"></param>
        /// <param name="passnew"></param>
        /// <returns></returns>
        bool ChangePassWord(string passold, string passnew, string email);
    }
}
