using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.Infrastructure.Configuration;
using GrupoLTM.WebSmart.Infrastructure.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static GrupoLTM.WebSmart.Domain.Enums.EnumDomain;

namespace GrupoLTM.WebSmart.Services.Login
{
    public class LoginService
    {
        public LoginService() { }
        //private void ImportarListaRAAcessosBulk(List<ForcarPrimeiroAcessoModel> listaRas)
        //{
        //    try
        //    {
        //        var dtEstrutura = this.MontarDatatableEstrutura("ForcarPrimeiroAcesso");

        //        using (var bulkCopy = new SqlBulkCopy(ConfiguracaoService.GetDatabaseConnection(), SqlBulkCopyOptions.TableLock))
        //        {
        //            bulkCopy.BatchSize = 20000;
        //            bulkCopy.BulkCopyTimeout = 3600;
        //            bulkCopy.DestinationTableName = dtEstrutura.TableName;

        //            int contadorLista = 0;
        //            while (contadorLista < listaRas.Count)
        //            {
        //                for (int limite = 0; limite < 100000 && contadorLista < listaRas.Count; limite++)
        //                {
        //                    dtEstrutura.Rows.Add(AdicionarLinhasDatatableEstrutura(listaRas[contadorLista], dtEstrutura));
        //                    contadorLista++;
        //                }

        //                bulkCopy.WriteToServer(dtEstrutura);

        //                dtEstrutura.Clear();
        //            }
        //        }

        //    }
        //    catch (Exception erro)
        //    {
        //        gravaLogErro("Erro na importação do Bulk da tabela ApoioImportacao", erro.Message, "GrupoLTM.WebSmart.Services", string.Format("ApoioDadosBulk(Loteid:{0})", ), "jobCatalog");

        //        throw new Exception(string.Format("Erro na execução do Processo: {0}", erro.Message));
        //    }
        //}
        private DataRow AdicionarLinhasDatatableEstrutura(ForcarPrimeiroAcessoModel lista, DataTable dt)
        {
            try
            {
                DataRow row = dt.NewRow();

                row["Id"] = 0;        
                row["Login"] = lista.Login;
                row["DataInclusao"] = DateTime.Now;
                row["FlagEnviado"] = false;
                return row;
            }
            catch (Exception erro)
            {
                gravaLogErro(string.Format("Erro na inclusão de Linhas Forçar Primeiro Acesso:"), erro.Message, "GrupoLTM.WebSmart.Services", string.Format("AdicionarLinhasDatatableEstrutura({0})", lista.Login), "jobCatalog");

                throw new Exception(string.Format("Erro na execução do Processo: {0}", erro.Message));
            }
        }

        private static void gravaLogErro(string Erro, string Mensagem, string Source, string Metodo, string Codigo)
        {
            var logErro = new LogErro()
            {
                Erro = Erro,
                Mensagem = Mensagem,
                Source = Source,
                Metodo = Metodo,
                Controller = "LoginService",
                Pagina = string.Empty,
                Codigo = Codigo
            };

            var logErroService = new LogErroService();
            logErroService.SalvarLogErro(logErro);
        }
        private DataTable MontarDatatableEstrutura(string tabelaDestino)
        {
            var _datatable = new DataTable(tabelaDestino);

            _datatable.Columns.Add("Login");
            _datatable.Columns["Login"].MaxLength = 8;
            _datatable.Columns.Add("DataInclusao", typeof(DateTime));
            _datatable.Columns.Add("DataEnvio", typeof(DateTime));
            _datatable.Columns.Add("FlagEnviado");
            _datatable.Columns.Add("Id");
            
            return _datatable;
        }  

    }
}
