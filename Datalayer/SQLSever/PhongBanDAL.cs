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
    public class PhongBanDAL : _BaseDAL, ICommonDAL<PhongBan>
    {

        public PhongBanDAL(string connectionString) : base(connectionString)
        {

        }

     
        public int Add(PhongBan data)
        {
            int result = 0;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"insert into PhongBan(namePhongBan)
                                            values(@namePhongBan)
                                             Select @@Identity;";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                //Truyền tham số cho câu truy vấn
                cmd.Parameters.AddWithValue("@namePhongBan", data.namePhongBan);

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
                                    from    PhongBan
                                    where  (@searchValue = N'')
                                        or (
                                                (namePhongBan like @searchValue)
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

        public bool Delete(int PhongBanID)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"Delete 
                                    from PhongBan
                                     where codePhongBan = @PhongBanID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                //Truyền tham số cho câu truy vấn
                cmd.Parameters.AddWithValue("@PhongBanID", PhongBanID);

                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }


            return result;
        }

        public PhongBan Get(int PhongBanID)
        {
            PhongBan data = null;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select *
                                    from PhongBan
                                     where codePhongBan = @PhongBanID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                //Truyền tham số cho câu truy vấn
                cmd.Parameters.AddWithValue("@PhongBanID", PhongBanID);


                var result = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (result.Read())
                {
                    data = new PhongBan()
                    {
                        codePhongBan = Convert.ToInt32(result["codePhongBan"]),
                        namePhongBan = Convert.ToString(result["namePhongBan"]),
                    };
                }
                result.Close();
                cn.Close();
            }
            return data;
        }





        public bool InUsed(int PhongBanID)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select case when exists(select* from Employees where codePhongBan = @PhongBanID) then 1 else 0 end";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@PhongBanID", PhongBanID);

                result = Convert.ToBoolean(cmd.ExecuteScalar());
                cn.Close();
            }
            return result;
        }


        public IList<PhongBan> List(int page, int pageSize, string searchValue)
        {
            IList<PhongBan> data = new List<PhongBan>();

            if (searchValue != "")
                searchValue = "%" + searchValue + "%";
            //Tạo CSDL
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select *
                                    from
                                        (
                                            select    *,
                                                    row_number() over(order by namePhongBan) as RowNumber
                                            from    PhongBan
                                            where    (@searchValue = N'')
                                                or (
                                                        (namePhongBan like @searchValue)
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
                    data.Add(new PhongBan()
                    {
                        codePhongBan = Convert.ToInt32(dbReader["codePhongBan"]),
                        namePhongBan = Convert.ToString(dbReader["namePhongBan"]),
                    });
                }
                dbReader.Close();
                cn.Close();
            }


            return data;
        }

        /// <summary>
        /// Cập nhật thông tin một loại hàng
        /// </summary>
        /// <param name="data">Thông tin loại hàng cần cập nhật</param>
        /// <returns>Trả về kết quả true or false </returns>
        public bool Update(PhongBan data)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"Update PhongBan
                                    Set namePhongBan = @PhongBanName,
                                    WHERE codePhongBan = @PhongBanID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                //Truyền tham số cho câu truy vấn
                cmd.Parameters.AddWithValue("@PhongBanName", data.namePhongBan);
                cmd.Parameters.AddWithValue("@PhongBanID", data.codePhongBan);
                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }
            return result;
        }
    }
}
