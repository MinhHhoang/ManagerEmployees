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
    public class MemberProjectDAL : _BaseDAL, IMemberProject
    {
        public MemberProjectDAL(string connectionString) : base(connectionString)
        {

        }

      


        public int Add(MemberProject data)
        {
            int result = 0;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"insert into MemberProject(EmployeeID,ProjectID,Position)
                                            values(@EmployeeID,@ProjectID,@Position)";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                //Truyền tham số cho câu truy vấn
                cmd.Parameters.AddWithValue("@EmployeeID", data.EmployeeID);
                cmd.Parameters.AddWithValue("@ProjectID", data.ProjectID);
                cmd.Parameters.AddWithValue("@Position", data.Position);

                try
                {
                    result = Convert.ToInt32(cmd.ExecuteScalar());
                } catch (SqlException e)
                {
                    return 0;
                }

                cn.Close();
            }
            return result;
        }

       
        public int Count(int projectID)
        {
            
            int count = 0;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select count(*)
                                    from    MemberProject 
                                            inner join Projects on MemberProject.ProjectID = Projects.ProjectID 
                                            inner join Employees on Employees.EmployeeID = MemberProject.EmployeeID
                                    where  MemberProject.ProjectID = @projectID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                //Truyền tham số cho câu truy vấn
                cmd.Parameters.AddWithValue("@projectID", projectID);
                count = Convert.ToInt32(cmd.ExecuteScalar());

                cn.Close();
            }

            return count;
        }

   

        public bool Delete(int EmployeeID, int ProjectID)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"Delete 
                                      from MemberProject
                                       where ProjectID = @ProjectID and EmployeeID = @EmployeeID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                //Truyền tham số cho câu truy vấn
                cmd.Parameters.AddWithValue("@ProjectID", ProjectID);
                cmd.Parameters.AddWithValue("@EmployeeID", EmployeeID);


                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }
            return result;
        }




        public MemberProject Get(int EmployeeID, int ProjectID)
        {
            MemberProject data = null;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select MemberProject.*, Employees.FirstName,Employees.LastName
                                    from    MemberProject 
                                            inner join Projects on MemberProject.ProjectID = Projects.ProjectID 
                                            inner join Employees on Employees.EmployeeID = MemberProject.EmployeeID 
                                    where MemberProject.ProjectID = @ProjectID and MemberProject.EmployeeID = @EmployeeID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                //Truyền tham số cho câu truy vấn
                cmd.Parameters.AddWithValue("@ProjectID", ProjectID);
                cmd.Parameters.AddWithValue("@EmployeeID", EmployeeID);


                var result = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (result.Read())
                {
                    data = new MemberProject()
                    {
                        ProjectID = Convert.ToInt32(result["ProjectID"]),
                        EmployeeID = Convert.ToInt32(result["EmployeeID"]),
                        Position = Convert.ToString(result["Position"]),
                        LastName = Convert.ToString(result["LastName"]),
                        FirstName = Convert.ToString(result["FirstName"]),

                    };
                }
                result.Close();
                cn.Close();
            }
            return data;
        }


        /// <summary>
        /// Tìm kiếm và phân trang cho list Thành viên dự án
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
    
        public IList<MemberProject> List(int ProjectID, int page, int pageSize, string searchValue)
        {
            IList<MemberProject> data = new List<MemberProject>(); // khỏi tạo list thành viên của dự án

            if (searchValue != "")
                searchValue = "%" + searchValue + "%";
            //Tạo CSDL
            using (SqlConnection cn = OpenConnection()) // open connection
            {
                SqlCommand cmd = new SqlCommand();
                //Định nghĩa cậu lệnh truy vấn
                cmd.CommandText = @"select *
                                     from   (
                                            select MemberProject.*, Employees.FirstName,Employees.LastName,Projects.NameProject,
                                                    row_number() over(order by NameProject) as RowNumber
                                            from    MemberProject 
                                                    inner join Projects on MemberProject.ProjectID = Projects.ProjectID 
                                                    inner join Employees on Employees.EmployeeID = MemberProject.EmployeeID
                                            where   MemberProject.ProjectID = @ProjectID and ((@searchValue = N'')
                                                or (
                                                        (FirstName like @searchValue)
                                                        or
                                                        (LastName like @searchValue)
                                                    ))
                                        ) as t
                                    where (@pageSize = 0 ) or (t.RowNumber between (@page - 1) * @pageSize + 1 and @page * @pageSize)
                                    order by t.RowNumber;";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                //Truyền tham số cho câu truy vấn
                cmd.Parameters.AddWithValue("@ProjectID", ProjectID);
                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@searchValue", searchValue);

                SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection); // Excute câu lệnh truy vấn trên
                while (dbReader.Read())
                {
                    data.Add(new MemberProject() //tạo mới object và add vào data
                    {
                        ProjectID = Convert.ToInt32(dbReader["ProjectID"]),
                        EmployeeID = Convert.ToInt32(dbReader["EmployeeID"]),
                        LastName = Convert.ToString(dbReader["LastName"]),
                        FirstName = Convert.ToString(dbReader["FirstName"]),
                        NameProject = Convert.ToString(dbReader["NameProject"]),
                        Position = Convert.ToString(dbReader["Position"])
                    });
                }
                dbReader.Close(); // Đóng 
                cn.Close(); // close connect
            }


            return data; //  trả về list data thành viên của dự án thoe cậu lệnh truy vấn trên
        }


   
        public bool Update(MemberProject data)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"Update MemberProject
                                    Set Position = @Position
                                    where ProjectID = @ProjectID and EmployeeID = @EmployeeID";
               
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                //Truyền tham số cho câu truy vấn
                cmd.Parameters.AddWithValue("@EmployeeID", data.EmployeeID);
                cmd.Parameters.AddWithValue("@ProjectID", data.ProjectID);
                cmd.Parameters.AddWithValue("@Position", data.Position);

                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }
            return result;
        }
    }
}
