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
using GrupoLTM.WebSmart.DTO;

namespace GrupoLTM.WebSmart.Services
{
    public class VendaService
    {
        public static bool ImportarArquivoVenda(DataTable dt, int ArquivoId, int MesId, int Ano)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString))
            {
                bulkCopy.DestinationTableName =
                    "dbo.VendaImportacao";
                try
                {
                    dt.Columns.Add("ArquivoId");
                    dt.Columns.Add("Mes");
                    dt.Columns.Add("Ano");
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["ArquivoId"] = ArquivoId;
                        dr["Mes"] = MesId;
                        dr["Ano"] = Ano;
                    }
                    bulkCopy.WriteToServer(dt);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public static bool ProcessaVendaArquivo(int ArquivoId, int Mes, int Ano, out int countErro)
        {
            countErro = 0;
            bool blnSucesso = false;

            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Imp_ImportacaoVenda";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@ArquivoId", Value = ArquivoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@Mes", Value = Mes, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@Ano", Value = Ano, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@ImportadoSucesso", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output });
            listParam.Add(new SqlParameter { ParameterName = "@CountErro", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output });

            DataProvider.NonqueryProc(proc, listParam);

            if ((Boolean)listParam[3].Value)
            {
                blnSucesso = true;
            }
            else
            {
                blnSucesso = false;
            }
            countErro = (int)listParam[4].Value;

            return blnSucesso;
        }

        public static string ExportaArquivoVendaErro(int ArquivoId)
        {
            string strDownloadArquivoErro = "";

            DataSet ds = new DataSet();
            ds.Tables.Add(ListaImportacaoVendaErro(ArquivoId));

            string strArquivoGerado = "";
            ExcelExport.WriteXLSFile(ds, "venda/erro/", out strArquivoGerado);

            strDownloadArquivoErro = "venda/erro/" + strArquivoGerado;

            return strDownloadArquivoErro;
        }

        public static DataTable ListaImportacaoVendaErro(int ArquivoId)
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Sel_ImpVendaErro";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@ArquivoId", Value = ArquivoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            DataTable table = DataProvider.SelectStoreProcedure(proc, listParam);
            return table;
        }

        public static List<VendaLogArquivoModel> ListaVendaArquivoLog()
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Sel_VendaArquivoLog";

            DataTable table = DataProvider.SelectStoreProcedure(proc);

            List<VendaLogArquivoModel> listVendaLogArquivoModel = new List<VendaLogArquivoModel>();

            foreach (DataRow item in table.Rows)
            {
                listVendaLogArquivoModel.Add(new VendaLogArquivoModel
                {
                    Ano = item.Field<int>("Ano"),
                    ArquivoId = item.Field<int>("ArquivoId"),
                    Ativo = item.Field<bool>("Ativo"),
                    DataInativacao = item.Field<DateTime?>("DataInativacao"),
                    DataInclusao = item.Field<DateTime>("DataInclusao"),
                    Id = item.Field<int>("Id"),
                    Mes = item.Field<int>("Mes"),
                    Nome = item.Field<string>("Nome"),
                    NomeGerado = item.Field<string>("NomeGerado")
                });
            }

            return listVendaLogArquivoModel;
        }


    }
}
