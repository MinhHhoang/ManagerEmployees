using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Datalayer.SQLSever
{
    /// <summary>
    /// Bổ sung lớp cơ sở (lớp cha) cho các lớp liên quan đến xữ lý dữ liệu trên SQL Server
    /// </summary>
    public abstract class _BaseDAL
    {   /// <summary>
        /// Chuỗi tham số kết nối
        /// </summary>
        protected string _connectionString;

        /// <summary>
        /// contructor
        /// </summary>
        /// <param name="connectionString"></param>
        public _BaseDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Open connect với Cở dữ liệu
        /// </summary>
        /// <returns></returns>
        protected SqlConnection OpenConnection()
        {   
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = _connectionString;
            cn.Open();
            return cn;
        }
    }
}
