using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Datalayer.SQLSever
{
    public class EmployeeDAL : _BaseDAL, IEmployeeDAL<Employee>
    {
        public EmployeeDAL(string connectionString) : base(connectionString)
        {

        }

      

        /// <summary>
        /// Thêm một nhân viên
        /// </summary>
        /// <param name="data">Thông tin nhân viên cần thêm</param>
        /// <returns>Kết quả theo kiểu số nguyên thành công hay thất bại</returns>
        public int Add(Employee data)
        {
            int result = 0;
            using (SqlConnection cn = OpenConnection()) // Open connect với csdl
            {
                SqlCommand cmd = new SqlCommand();
                //Định nghĩa câu lệnh truy vấn
                cmd.CommandText = @"insert into Employees(LastName,FirstName,BirthDate,Photo,Email,countProject,codePhongBan,levelCoding,languageProgram,roleAccount,Password, Notes)
                                            values(@LastName,@FirstName,@BirthDate,@Photo,@Email,@countProject,@codePhongBan,@levelCoding,@languageProgram,@roleAccount,@Password,@Notes)
                                             Select @@Identity;";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                //Truyền tham số cho câu truy vấn
                cmd.Parameters.AddWithValue("@LastName", data.LastName);
                cmd.Parameters.AddWithValue("@FirstName", data.FirstName);
                cmd.Parameters.AddWithValue("@BirthDate", data.BirthDate);
                cmd.Parameters.AddWithValue("@Photo", data.Photo);
                cmd.Parameters.AddWithValue("@Email", data.Email);

                //Add property
                cmd.Parameters.AddWithValue("@countProject", data.CountProject);
                cmd.Parameters.AddWithValue("@codePhongBan", data.CodePhongBan);
                cmd.Parameters.AddWithValue("@levelCoding", data.LevelCoding);
                cmd.Parameters.AddWithValue("@languageProgram", data.LanguageProgram);
                cmd.Parameters.AddWithValue("@roleAccount", data.RoleAccount);
                cmd.Parameters.AddWithValue("@Password", "123456");
                cmd.Parameters.AddWithValue("@Notes", data.Notes);

                try
                {
                    result = Convert.ToInt32(cmd.ExecuteScalar()); // thực hiện câu lệnh query phía trên
                } catch (SqlException e) // trường hợp xảy ra lỗi kết quả trả vè bằng 0
                {
                    result = 0;
                }

                cn.Close(); // đóng connect DB
            }
            return result;
        }
        /// <summary>
        /// Change password cho nhân viên by email
        /// </summary>
        /// <param name="passold"></param>
        /// <param name="passnew"></param>
        /// <returns></returns>
        public bool ChangePassWord(string passold, string passnew, string email)
        {
            if (CheckAccount(email, passold) != null)
            {
                bool result = false;
                using (SqlConnection cn = OpenConnection())
                {
                    SqlCommand cmd = new SqlCommand();
                    //Định nghĩa câu lệnh truy vấn sql update nhân viên
                    cmd.CommandText = @"Update Employees
                                    Set Password = @Password
                                    WHERE Email = @Email";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = cn; // open connect

                    //Truyền tham số cho câu truy vấn
                    cmd.Parameters.AddWithValue("@Password", passnew);
                    cmd.Parameters.AddWithValue("@Email", email);

                    result = cmd.ExecuteNonQuery() > 0; // Thực hiện câu lệnh truy vấn SQL

                    cn.Close();// Đóng connect sql lại
                }
                return result;
            }
            else return false;
        }

        /// <summary>
        /// Kiểm tra đăng nhập
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Employee CheckAccount(string email, string password)
        {
            Employee data = null;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                //Định nghĩa câu lệnh kiểm tra đăng nhập
                cmd.CommandText = @"select *
                                    from Employees
                                     where Email = @Email and Password = @Password";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;// Open connect với csdl

                //Truyền tham số cho câu truy vấn
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);


                var result = cmd.ExecuteReader(CommandBehavior.CloseConnection); // thực hiện excute câu lệnh phía trên
                if (result.Read())
                {
                    data = new Employee() // Khởi tạo new một đối tượng Employee
                    {
                        EmployeeID = Convert.ToInt32(result["EmployeeID"]),
                        LastName = Convert.ToString(result["LastName"]),
                        FirstName = Convert.ToString(result["FirstName"]),
                        BirthDate = Convert.ToDateTime(result["BirthDate"]),
                        CodePhongBan = Convert.ToInt32(result["codePhongBan"]),
                        LevelCoding = Convert.ToString(result["levelCoding"]),
                        RoleAccount = Convert.ToInt32(result["roleAccount"]),
                        LanguageProgram = Convert.ToString(result["languageProgram"]),
                        Photo = Convert.ToString(result["Photo"]),
                        Email = Convert.ToString(result["Email"]),
                        CountProject = Convert.ToInt32(result["countProject"]),
                    };
                }
                result.Close(); // close file
                cn.Close();// Đóng connect sql
            }
            return data;
        }

        public Employee CheckExistEmail(string email)
        {
            Employee data = null;
            using (SqlConnection cn = OpenConnection())
            {
                //Định nghĩa câu lện sql kiểm tra tồn tại email trong table Employee
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select *
                                    from Employees
                                     where Email = @Email";
                cmd.CommandType = CommandType.Text; 
                cmd.Connection = cn;// Open connect với csdl

                //Truyền tham số cho câu truy vấn
                cmd.Parameters.AddWithValue("@Email", email);


                var result = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (result.Read())
                {
                    data = new Employee() // Khởi tạo new một đối tượng Employee
                    {
                        EmployeeID = Convert.ToInt32(result["EmployeeID"]),
                        LastName = Convert.ToString(result["LastName"]),
                        FirstName = Convert.ToString(result["FirstName"]),
                        BirthDate = Convert.ToDateTime(result["BirthDate"]),
                        CodePhongBan = Convert.ToInt32(result["codePhongBan"]),
                        LevelCoding = Convert.ToString(result["levelCoding"]),
                        RoleAccount = Convert.ToInt32(result["roleAccount"]),
                        LanguageProgram = Convert.ToString(result["languageProgram"]),
                        Photo = Convert.ToString(result["Photo"]),
                        Email = Convert.ToString(result["Email"]),
                        CountProject = Convert.ToInt32(result["countProject"]),
                    };
                }
                result.Close(); // close file
                cn.Close();// Đóng connect sql
            }
            return data; // trả về object Employee
        }

        /// <summary>
        /// Đếm sô lượng nhân viên thỏa mãn sau kết quả tìm kiếm
        /// </summary>
        /// <param name="searchValue">Tên cần tìm kiếm. Nếu không nhập thì đếm toàn bộ</param>
        /// <returns>Tên tìm kiếm. Nếu không nhập thì đếm toàn bộ</returns>
        public int Count(string searchValue)
        {
            if (searchValue != "")
                searchValue = "%" + searchValue + "%";

            int count = 0;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = @"select count(*)
                                    from    Employees
                                    where  (@searchValue = N'')
                                        or (
                                                (LastName like @searchValue)
                                                or
                                                (FirstName like @searchValue)
                                            )";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                //Truyền tham số cho câu truy vấn
                cmd.Parameters.AddWithValue("@searchValue", searchValue);
                count = Convert.ToInt32(cmd.ExecuteScalar());

                cn.Close();
            }

            return count;
        }

     

        
          public bool Delete(int EmployeeID)
          {
              bool result = false;
              using (SqlConnection cn = OpenConnection())
              {
                  SqlCommand cmd = new SqlCommand();
                  cmd.CommandText = @"Delete 
                                      from Employees
                                       where EmployeeID = @EmployeeID";
                  cmd.CommandType = CommandType.Text;
                  cmd.Connection = cn;

                  //Truyền tham số cho câu truy vấn
                  cmd.Parameters.AddWithValue("@EmployeeID", EmployeeID);

                  result = cmd.ExecuteNonQuery() > 0;

                  cn.Close();
              }


              return result;
          }

        /// <summary>
        /// Lấy thông tin một nhân viên
        /// </summary>
        /// <param name="EmployeeID">ID nhân viên</param>
        /// <returns>Dữ liệu nhà cung cấp theo ID nhân viên</returns>
        public Employee Get(int EmployeeID)
        {
            Employee data = null;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                //Định nghĩa câu lệnh truy vấn lấy thông tin của 1 nhân viên theo ID
                cmd.CommandText = @"select Employees.*, PhongBan.namePhongBan
                                    from Employees inner join PhongBan on Employees.codePhongBan = PhongBan.codePhongBan
                                     where EmployeeID = @EmployeeID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                //Truyền tham số cho câu truy vấn
                cmd.Parameters.AddWithValue("@EmployeeID", EmployeeID);


                var result = cmd.ExecuteReader(CommandBehavior.CloseConnection);// Thực hiện get data từ SQL
                if (result.Read())
                {
                    data = new Employee() // Khỏi tạo Nhân viên
                    {
                        EmployeeID = Convert.ToInt32(result["EmployeeID"]),
                        LastName = Convert.ToString(result["LastName"]),
                        FirstName = Convert.ToString(result["FirstName"]),
                        BirthDate = Convert.ToDateTime(result["BirthDate"]),
                        CodePhongBan = Convert.ToInt32(result["codePhongBan"]),
                        LevelCoding = Convert.ToString(result["levelCoding"]),
                        RoleAccount = Convert.ToInt32(result["roleAccount"]),
                        LanguageProgram = Convert.ToString(result["languageProgram"]),
                        Photo = Convert.ToString(result["Photo"]),
                        Email = Convert.ToString(result["Email"]),
                        CountProject = Convert.ToInt32(result["countProject"]),
                        NamePhongBan = Convert.ToString(result["namePhongBan"]),
                    };
                }
                result.Close();
                cn.Close();
            }
            return data;
        }

        public bool InUsed(int id)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select case when exists(select* from MemberProject where EmployeeID = @EmployeeID) then 1 else 0 end";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@EmployeeID", id);

                result = Convert.ToBoolean(cmd.ExecuteScalar());
                cn.Close();
            }
            return result;
        }



        /// <summary>
        /// Tìm kiếm và lấy danh sách nhân viên dưới dạng phân trang
        /// </summary>
        /// <param name="page">Trang cần tìm kiếm</param>
        /// <param name="pageSize">Số dòng trên mỗi trang</param>
        /// <param name="searchValue">Tên cần tìm kiếm. Nếu không nhập thì lấy toàn bộ</param>
        /// <returns>Danh sách  nhân viên kết quả tìm kiếm</returns>
        public IList<Employee> List(string role,string code ,int page, int pageSize, string searchValue)
        {
            IList<Employee> data = new List<Employee>(); // Khởi tạo list object nhận viên

            if (searchValue != "")
                searchValue = "%" + searchValue + "%";
            //Tạo CSDL
            using (SqlConnection cn = OpenConnection()) // Open connect sql
            {
                SqlCommand cmd = new SqlCommand();
                //Định nghĩa câu lếnh sql truy vấn danh sách nhân viên
                if(role.Equals("1"))
                {
                    cmd.CommandText = @"select *
                                     from   (
                                            select   Employees.*, PhongBan.namePhongBan,
                                                    row_number() over(order by FirstName) as RowNumber
                                            from    Employees inner join PhongBan on Employees.codePhongBan = PhongBan.codePhongBan
                                            where    (@searchValue = N'')
                                                or (
                                                        (LastName like @searchValue)
                                                        or
                                                        (FirstName like @searchValue)
                                                    )
                                        ) as t
                                    where (@pageSize = 0 ) or (t.RowNumber between (@page - 1) * @pageSize + 1 and @page * @pageSize)
                                    order by t.RowNumber;";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = cn;
                } else
                {
                    cmd.CommandText = @"select *
                                     from   (
                                            select   Employees.*, PhongBan.namePhongBan,
                                                    row_number() over(order by FirstName) as RowNumber
                                            from    Employees inner join PhongBan on Employees.codePhongBan = PhongBan.codePhongBan
                                            where   ((@searchValue = N'')
                                                or (
                                                        (LastName like @searchValue)
                                                        or
                                                        (FirstName like @searchValue)
                                                    )) and Employees.codePhongBan = @codePhongBan
                                        ) as t
                                    where (@pageSize = 0 ) or (t.RowNumber between (@page - 1) * @pageSize + 1 and @page * @pageSize)
                                    order by t.RowNumber;";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = cn;
                    cmd.Parameters.AddWithValue("@codePhongBan", code);
                }
                

                //Truyền tham số cho câu truy vấn
                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@searchValue", searchValue);

                SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection); // Thực hiện get data từ SQL
                while (dbReader.Read())
                {
                    data.Add(new Employee() //Khỏi tạo new object Employyee và add object vào data
                    {
                        EmployeeID = Convert.ToInt32(dbReader["EmployeeID"]),
                        LastName = Convert.ToString(dbReader["LastName"]),
                        FirstName = Convert.ToString(dbReader["FirstName"]),
                        BirthDate = Convert.ToDateTime(dbReader["BirthDate"]),
                        CodePhongBan = Convert.ToInt32(dbReader["codePhongBan"]),
                        LevelCoding = Convert.ToString(dbReader["levelCoding"]),
                        RoleAccount = Convert.ToInt32(dbReader["roleAccount"]),
                        LanguageProgram = Convert.ToString(dbReader["languageProgram"]),
                        Photo = Convert.ToString(dbReader["Photo"]),
                        Email = Convert.ToString(dbReader["Email"]),
                        CountProject = Convert.ToInt32(dbReader["countProject"]),
                        NamePhongBan = Convert.ToString(dbReader["namePhongBan"]),
                    });
                }
                dbReader.Close();
                cn.Close();
            }


            return data;
        }


        /// <summary>
        /// Cập nhật thông tin 1 nhân viên
        /// </summary>
        /// <param name="data">Thông tin nhân viên cần cập nhật</param>
        /// <returns>Trả về kết quả true or false </returns>
        public bool Update(Employee data)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                //Định nghĩa câu lệnh truy vấn sql update nhân viên
                cmd.CommandText = @"Update Employees
                                    Set LastName = @LastName,
                                    FirstName = @FirstName,
                                    BirthDate = @BirthDate,
                                    Photo = @Photo,
                                    Email = @Email,
                                    countProject = @countProject,
                                    codePhongBan = @codePhongBan,
                                    levelCoding = @levelCoding,
                                    languageProgram = @languageProgram
                                    WHERE EmployeeID = @EmployeeID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn; // open connect

                //Truyền tham số cho câu truy vấn
                cmd.Parameters.AddWithValue("@LastName", data.LastName);
                cmd.Parameters.AddWithValue("@FirstName", data.FirstName);
                cmd.Parameters.AddWithValue("@BirthDate", data.BirthDate);
                cmd.Parameters.AddWithValue("@Photo", data.Photo);
                cmd.Parameters.AddWithValue("@Email", data.Email);
                cmd.Parameters.AddWithValue("@EmployeeID", data.EmployeeID);


                //Add property
                cmd.Parameters.AddWithValue("@countProject", data.CountProject);
                cmd.Parameters.AddWithValue("@codePhongBan", data.CodePhongBan);
                cmd.Parameters.AddWithValue("@levelCoding", data.LevelCoding);
                cmd.Parameters.AddWithValue("@languageProgram", data.LanguageProgram);

                result = cmd.ExecuteNonQuery() > 0; // Thực hiện câu lệnh truy vấn SQL

                cn.Close();// Đóng connect sql lại
            }
            return result;
        }

    }
}
