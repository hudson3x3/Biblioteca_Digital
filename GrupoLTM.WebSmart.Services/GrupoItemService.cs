using GrupoLTM.WebSmart.Infrastructure.Configuration;
using GrupoLTM.WebSmart.Infrastructure.Data;
using GrupoLTM.WebSmart.Infrastructure.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace GrupoLTM.WebSmart.Services
{
    public class GrupoItemService
    {
        public static bool ImportarArquivoGrupoItem(DataTable dtParticipante, int ArquivoId)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString))
            {
                bulkCopy.DestinationTableName =
                    "dbo.GrupoItemImportacao";
                try
                {
                    dtParticipante.Columns.Add("ArquivoId");
                    foreach (DataRow dr in dtParticipante.Rows)
                    {
                        dr["ArquivoId"] = ArquivoId;
                    }
                    bulkCopy.WriteToServer(dtParticipante);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public static bool ProcessaGrupoItemArquivo(int ArquivoId, out int countErro)
        {
            countErro = 0;
            bool blnSucesso = false;

            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Imp_GrupoItem";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@ArquivoId", Value = ArquivoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@ImportadoSucesso", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output });
            listParam.Add(new SqlParameter { ParameterName = "@CountErro", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output });

            DataProvider.NonqueryProc(proc, listParam);

            if ((Boolean)listParam[1].Value)
            {
                blnSucesso = true;
            }
            else
            {
                blnSucesso = false;
            }
            countErro = (int)listParam[2].Value;

            return blnSucesso;
        }

        public static string ExportaArquivoGrupoItemErro(int ArquivoId)
        {
            string strDownloadArquivoErro = "";

            DataSet ds = new DataSet();
            ds.Tables.Add(ListaImportacaoGrupoItemErro(ArquivoId));

            string strArquivoGerado = "";
            ExcelExport.WriteXLSFile(
                ds,
                "grupoItem/erro/",
                out strArquivoGerado);

            strDownloadArquivoErro = "grupoItem/erro/" + strArquivoGerado;

            return strDownloadArquivoErro;
        }

        public static DataTable ListaImportacaoGrupoItemErro(int ArquivoId)
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Sel_ImpGrupoItemErro";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@ArquivoId", Value = ArquivoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            DataTable table = DataProvider.SelectStoreProcedure(proc, listParam);
            return table;
        }
    }
}
