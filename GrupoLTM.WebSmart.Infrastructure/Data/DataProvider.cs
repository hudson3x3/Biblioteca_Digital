using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace GrupoLTM.WebSmart.Infrastructure.Data
{
    public class DataProvider
    {
        public static string connectionString { get; set; }
        //public static string connectionString = ConfigurationManager.ConnectionStrings["DB_CatalogoVejaEntitiesProc"].ConnectionString;

        public static DataTable Query(string query)
        {
            DataTable data = new DataTable();
            SqlConnection cn = new SqlConnection(connectionString);
            cn.Open();
            SqlCommand cmd = new SqlCommand(query, cn);

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(data);
            return data;

        }

        public static DataSet NonSelectStoreProcedure(string ProcName)
        {
            DataSet data = new DataSet();
            SqlConnection cn = new SqlConnection(connectionString);
            cn.Open();
            SqlCommand cmd = new SqlCommand(ProcName, cn);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(data);
            cn.Close();
            return data;

        }

        public static void NonQueryCommand(string query)
        {
            SqlConnection cn = new SqlConnection(connectionString);
            try
            {
                cn.Open();
                using (var cmd = new SqlCommand(query, cn))
                {
                    cmd.CommandTimeout = 3600;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cn.Close();
            }
        }

        public static DataTable SelectStoreProcedure(string ProcName, List<SqlParameter> ParaArr)
        {
            DataTable data = new DataTable();
            SqlConnection cn = new SqlConnection(connectionString);
            cn.Open();
            SqlCommand cmd = new SqlCommand(ProcName, cn);
            foreach (SqlParameter para in ParaArr)
            {
                cmd.Parameters.Add(para);
            }
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(data);
            cn.Close();
            return data;
        }

        public static DataTable SelectStoreProcedureWithOutPut(string ProcName, List<SqlParameter> ParaArr, SqlParameter output)
        {
            DataTable data = new DataTable();
            SqlConnection cn = new SqlConnection(connectionString);
            cn.Open();
            SqlCommand cmd = new SqlCommand(ProcName, cn);
            foreach (SqlParameter para in ParaArr)
            {
                cmd.Parameters.Add(para);
            }
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(output).Direction = ParameterDirection.Output;

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(data);
            cn.Close();
            return data;
        }

        public static DataTable SelectStoreProcedure(string ProcName)
        {
            DataTable data = new DataTable();
            SqlConnection cn = new SqlConnection(connectionString);
            cn.Open();
            SqlCommand cmd = new SqlCommand(ProcName, cn);

            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(data);
            cn.Close();
            return data;
        }

        public static SqlCommand OutPutProc(string procname, List<SqlParameter> ParaArr, SqlParameter output)
        {
            SqlConnection cn = new SqlConnection(connectionString);
            cn.Open();
            SqlCommand cmd = new SqlCommand(procname, cn);
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter para in ParaArr)
            {
                cmd.Parameters.Add(para);
            }
            cmd.Parameters.Add(output).Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();
            return cmd;

        }

        public static SqlCommand OutPutProc(string procname, SqlParameter output)
        {
            SqlConnection cn = new SqlConnection(connectionString);
            cn.Open();
            SqlCommand cmd = new SqlCommand(procname, cn);
            cmd.CommandType = CommandType.StoredProcedure;


            cmd.Parameters.Add(output).Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();
            return cmd;

        }

        public static void NonqueryProc(string ProcName, List<SqlParameter> ParaArr)
        {
            SqlConnection cn = new SqlConnection(connectionString);
            cn.Open();
            SqlCommand cmd = new SqlCommand(ProcName, cn);
            foreach (SqlParameter para in ParaArr)
            {
                cmd.Parameters.Add(para);
            }
            cmd.CommandTimeout = 2400; //40 minutos
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
            cn.Close();
        }
    }
}

