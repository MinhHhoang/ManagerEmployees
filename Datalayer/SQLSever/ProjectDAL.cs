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
    public class ProjectDAL : _BaseDAL, ICommonDAL<Project>
    {
        public ProjectDAL(string connectionString) : base(connectionString)
        {

        }

      

        /// <summary>
        /// Thêm một nhân viên
        /// </summary>
        /// <param name="data">Thông tin nhân viên cần thêm</param>
        /// <returns>Kết quả theo kiểu số nguyên thành công hay thất bại</returns>
        public int Add(Project data)
        {
            int result = 0;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                if(data.StatusProject.Equals("CLOSED"))
                {
                    cmd.CommandText = @"insert into Projects(NameProject,ProjectManager,StatusProject,StartDate,FinishDate,Description,LinkSource)
                                            values(@NameProject,@ProjectManager,@StatusProject,@StartDate,@FinishDate,@Description,@LinkSource)
                                             Select @@Identity;";


                    cmd.Parameters.AddWithValue("@FinishDate", data.FinishDate);
                } else
                {
                    cmd.CommandText = @"insert into Projects(NameProject,ProjectManager,StatusProject,StartDate,Description,LinkSource)
                                            values(@NameProject,@ProjectManager,@StatusProject,@StartDate,@Description,@LinkSource)
                                             Select @@Identity;";
                }
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                //Truyền tham số cho câu truy vấn
                cmd.Parameters.AddWithValue("@NameProject", data.NameProject);
                cmd.Parameters.AddWithValue("@ProjectManager", data.ProjectManager);
                cmd.Parameters.AddWithValue("@StatusProject", data.StatusProject);
                cmd.Parameters.AddWithValue("@StartDate", data.StartDate);
                cmd.Parameters.AddWithValue("@Description", data.Description);
                cmd.Parameters.AddWithValue("@LinkSource", data.LinkSource);

                result = Convert.ToInt32(cmd.ExecuteScalar());

                cn.Close();
            }
            return result;
        }

       
        public int Count(string searchValue)
        {
            if (searchValue != "")
                searchValue = "%" + searchValue + "%";

            int count = 0;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select count(*)
                                    from    Projects
                                    where  (@searchValue = N'')
                                        or (
                                                (NameProject like @searchValue)
                                                or
                                                (Description like @searchValue)
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

     

        
          public bool Delete(int ProjectID)
          {
              bool result = false;
              using (SqlConnection cn = OpenConnection())
              {
                  SqlCommand cmd = new SqlCommand();
                  cmd.CommandText = @"Delete 
                                      from Projects
                                       where ProjectID = @ProjectID";
                  cmd.CommandType = CommandType.Text;
                  cmd.Connection = cn;

                  //Truyền tham số cho câu truy vấn
                  cmd.Parameters.AddWithValue("@ProjectID", ProjectID);

                  result = cmd.ExecuteNonQuery() > 0;

                  cn.Close();
              }


              return result;
          }

        /// <summary>
        /// Lấy thông tin một nhân viên
        /// </summary>
        /// <param name="ProjectID">ID nhân viên</param>
        /// <returns>Dữ liệu nhà cung cấp theo ID nhân viên</returns>
        public Project Get(int ProjectID)
        {
            Project data = null;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select Projects.*, Employees.FirstName,Employees.LastName
                                    from Projects inner join Employees on Projects.ProjectManager = Employees.EmployeeID
                                     where ProjectID = @ProjectID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                //Truyền tham số cho câu truy vấn
                cmd.Parameters.AddWithValue("@ProjectID", ProjectID);


                var result = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (result.Read())
                {
                    if(Convert.ToString(result["StatusProject"]).Equals("CLOSED"))
                    {
                        data = new Project()
                        {
                            ProjectID = Convert.ToInt32(result["ProjectID"]),
                            NameProject = Convert.ToString(result["NameProject"]),
                            LastName = Convert.ToString(result["LastName"]),
                            FirstName = Convert.ToString(result["FirstName"]),
                            ProjectManager = Convert.ToInt32(result["ProjectManager"]),
                            StatusProject = Convert.ToString(result["StatusProject"]),
                            StartDate = Convert.ToDateTime(result["StartDate"]),
                            FinishDate = Convert.ToDateTime(result["FinishDate"]),
                            Description = Convert.ToString(result["Description"]),
                            LinkSource = Convert.ToString(result["LinkSource"]),

                        };
                    } else
                    {
                        data = new Project()
                        {
                            ProjectID = Convert.ToInt32(result["ProjectID"]),
                            NameProject = Convert.ToString(result["NameProject"]),
                            LastName = Convert.ToString(result["LastName"]),
                            FirstName = Convert.ToString(result["FirstName"]),
                            ProjectManager = Convert.ToInt32(result["ProjectManager"]),
                            StatusProject = Convert.ToString(result["StatusProject"]),
                            StartDate = Convert.ToDateTime(result["StartDate"]),
                            Description = Convert.ToString(result["Description"]),
                            LinkSource = Convert.ToString(result["LinkSource"]),
                        };
                    }
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
                cmd.CommandText = @"select case when exists(select* from MemberProject where ProjectID = @ProjectID) then 1 else 0 end";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@ProjectID", id);

                result = Convert.ToBoolean(cmd.ExecuteScalar());
                cn.Close();
            }
            return result;
        }



    
        public IList<Project> List(int page, int pageSize, string searchValue)
        {
            IList<Project> data = new List<Project>();

            if (searchValue != "")
                searchValue = "%" + searchValue + "%";
            //Tạo CSDL
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select *
                                     from   (
                                            select Projects.*, Employees.FirstName,Employees.LastName,
                                                    row_number() over(order by NameProject) as RowNumber
                                            from    Projects inner join Employees on Projects.ProjectManager = Employees.EmployeeID
                                            where    (@searchValue = N'')
                                                or (
                                                        (NameProject like @searchValue)
                                                        or
                                                        (Description like @searchValue)
                                                    )
                                        ) as t
                                    where (@pageSize = 0 ) or (t.RowNumber between (@page - 1) * @pageSize + 1 and @page * @pageSize)
                                    order by t.RowNumber;";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                //Truyền tham số cho câu truy vấn
                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@searchValue", searchValue);

                SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dbReader.Read())
                {
                    if (Convert.ToString(dbReader["StatusProject"]).Equals("CLOSED"))
                    {
                        data.Add(new Project()
                        {
                            ProjectID = Convert.ToInt32(dbReader["ProjectID"]),
                            NameProject = Convert.ToString(dbReader["NameProject"]),
                            LastName = Convert.ToString(dbReader["LastName"]),
                            FirstName = Convert.ToString(dbReader["FirstName"]),
                            ProjectManager = Convert.ToInt32(dbReader["ProjectManager"]),
                            StatusProject = Convert.ToString(dbReader["StatusProject"]),
                            StartDate = Convert.ToDateTime(dbReader["StartDate"]),
                            FinishDate = Convert.ToDateTime(dbReader["FinishDate"]),
                            Description = Convert.ToString(dbReader["Description"]),
                            LinkSource = Convert.ToString(dbReader["LinkSource"]),
                        });
                    } else
                    {
                        data.Add(new Project()
                        {
                            ProjectID = Convert.ToInt32(dbReader["ProjectID"]),
                            NameProject = Convert.ToString(dbReader["NameProject"]),
                            LastName = Convert.ToString(dbReader["LastName"]),
                            FirstName = Convert.ToString(dbReader["FirstName"]),
                            ProjectManager = Convert.ToInt32(dbReader["ProjectManager"]),
                            StatusProject = Convert.ToString(dbReader["StatusProject"]),
                            StartDate = Convert.ToDateTime(dbReader["StartDate"]),
                            Description = Convert.ToString(dbReader["Description"]),
                            LinkSource = Convert.ToString(dbReader["LinkSource"]),
                        });
                    }
                }
                dbReader.Close();
                cn.Close();
            }


            return data;
        }


   
        public bool Update(Project data)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                if(data.StatusProject.Equals("CLOSED"))
                {
                    cmd.CommandText = @"Update Projects
                                    Set NameProject = @NameProject,
                                    ProjectManager = @ProjectManager,
                                    StatusProject = @StatusProject,
                                    StartDate = @StartDate,
                                    FinishDate = @FinishDate,
                                    LinkSource = @LinkSource,
                                    Description = @Description
                                    WHERE ProjectID = @ProjectID";
                    cmd.Parameters.AddWithValue("@FinishDate", data.FinishDate);
                } else
                {
                    cmd.CommandText = @"Update Projects
                                    Set NameProject = @NameProject,
                                    ProjectManager = @ProjectManager,
                                    StatusProject = @StatusProject,
                                    StartDate = @StartDate,
                                    LinkSource = @LinkSource,
                                    Description = @Description
                                    WHERE ProjectID = @ProjectID";
                }
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                //Truyền tham số cho câu truy vấn
                cmd.Parameters.AddWithValue("@NameProject", data.NameProject);
                cmd.Parameters.AddWithValue("@ProjectManager", data.ProjectManager);
                cmd.Parameters.AddWithValue("@StatusProject", data.StatusProject);
                cmd.Parameters.AddWithValue("@StartDate", data.StartDate);
                cmd.Parameters.AddWithValue("@Description", data.Description);
                cmd.Parameters.AddWithValue("@LinkSource", data.LinkSource);
                cmd.Parameters.AddWithValue("@ProjectID", data.ProjectID);
                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }
            return result;
        }
    }
}
