using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrupoLTM.WebSmart.Infrastructure.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using GrupoLTM.WebSmart.DTO;
using GrupoLTM.WebSmart.Services.Importacao;
using GrupoLTM.WebSmart.Infrastructure.CSV;

namespace GrupoLTM.WebSmart.Services
{
    public class EstruturaService : BaseService<Estrutura>
    {
        public List<WebSmart.DTO.EstruturaModel> ListarEstruturasAtivos()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repEstrutrura = context.CreateRepository<Estrutura>();
                List<WebSmart.DTO.EstruturaModel> model = new List<WebSmart.DTO.EstruturaModel>();

                var estruturas = repEstrutrura.Filter<Estrutura>(x => x.Ativo);

                foreach (var estrutura in estruturas)
                {
                    model.Add(new WebSmart.DTO.EstruturaModel
                    {
                        Ativo = estrutura.Ativo,
                        DataAlteracao = estrutura.DataAlteracao.HasValue ? estrutura.DataAlteracao.Value : DateTime.MinValue,
                        DataInclusao = estrutura.DataInclusao,
                        Id = estrutura.Id,
                        Nome = estrutura.Nome,
                        PaiId = estrutura.PaiId.HasValue ? estrutura.PaiId.Value : 0,
                        PeriodoId = estrutura.PeriodoId,
                        Tipo = estrutura.Tipo
                    });
                }

                return model;
            }
        }

        public List<WebSmart.DTO.EstruturaModel> ListarEstruturasAtivos(int idTipoEstrutura)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repEstrutrura = context.CreateRepository<Estrutura>();
                List<WebSmart.DTO.EstruturaModel> model = new List<WebSmart.DTO.EstruturaModel>();

                var estruturas = repEstrutrura.Filter<Estrutura>(x => x.Ativo && x.TipoEstruturaId == idTipoEstrutura);

                foreach (var estrutura in estruturas)
                {
                    model.Add(new WebSmart.DTO.EstruturaModel
                    {
                        Ativo = estrutura.Ativo,
                        DataAlteracao = estrutura.DataAlteracao.HasValue ? estrutura.DataAlteracao.Value : DateTime.MinValue,
                        DataInclusao = estrutura.DataInclusao,
                        Id = estrutura.Id,
                        Nome = estrutura.Nome,
                        PaiId = estrutura.PaiId.HasValue ? estrutura.PaiId.Value : 0,
                        PeriodoId = estrutura.PeriodoId,
                        Tipo = estrutura.Tipo
                    });
                }

                return model;
            }
        }

        public List<WebSmart.DTO.TipoEstruturaModel> ListaTipoEstruturaAtivos()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repEstrutrura = context.CreateRepository<TipoEstrutura>();
                List<WebSmart.DTO.TipoEstruturaModel> model = new List<WebSmart.DTO.TipoEstruturaModel>();

                var estruturas = repEstrutrura.Filter<Estrutura>(x => x.Ativo);

                foreach (var estrutura in estruturas)
                {
                    model.Add(new WebSmart.DTO.TipoEstruturaModel
                    {
                        Ativo = estrutura.Ativo,
                        DataAlteracao = estrutura.DataAlteracao.HasValue ? estrutura.DataAlteracao.Value : DateTime.MinValue,
                        DataInclusao = estrutura.DataInclusao,
                        Id = estrutura.Id,
                        Nome = estrutura.Nome,
                    });
                }

                return model;
            }
        }

        public bool InativarEstrutura(int EstruturaId)
        {
            bool blnSucesso = false;

            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_DEL_Estrutura";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@EstruturaId", Value = EstruturaId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@ProcessadoSucesso", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output });

            DataProvider.NonqueryProc(proc, listParam);

            if ((Boolean)listParam[1].Value)
            {
                blnSucesso = true;
            }
            else
            {
                blnSucesso = false;
            }

            return blnSucesso;
        }

        public Estrutura CadastrarEstrutura(EstruturaModel estruturaModel)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    //Grava o arquivo na tabela de Arquivos
                    IRepository repEstrutura = context.CreateRepository<Estrutura>();
                    Estrutura estrutura = new Estrutura();
                    estrutura.Nome = estruturaModel.Nome;
                    estrutura.Tipo = "1";
                    estrutura.Ativo = true;
                    estrutura.DataInclusao = DateTime.Now;

                    repEstrutura.Create<Estrutura>(estrutura);
                    repEstrutura.SaveChanges();

                    return estrutura;

                }
            }
            catch (Exception e)
            {
                return new Estrutura();
                throw;
            }

        }

        public void ProcessarImportacaoArquivo(Dictionary<int, string> linhasLote, string fileName)
        {
            //Arquivo arquivo
            var arquivo = ArquivoService.CadastrarArquivo(fileName, fileName, fileName, Domain.Enums.EnumDomain.TipoArquivo.SalesStructure);

            var listaApoio = this.ProcessarArquivo(linhasLote);
            this.ImportarDadosBulk(listaApoio, arquivo.Id);
        }

        private List<SalesStructureModel> ProcessarArquivo(Dictionary<int, string> linhasLote)
        {
            //Quebra arquivo por linhas
            var apoioLista = new List<SalesStructureModel>();

            //Valida Tipos campos por linha
            foreach (var linha in linhasLote)
            {
                if (!string.IsNullOrEmpty(linha.Value))
                {
                    var apoioImportacaoProcesso = new SalesStructureModel(linha.Value);
                    apoioImportacaoProcesso.NumeroLinha = linha.Key;
                    apoioLista.Add(apoioImportacaoProcesso);
                }
            }
            return apoioLista;
        }

        private void ImportarDadosBulk(List<SalesStructureModel> listaIndicacao, int arquivoId)
        {
            try
            {

                var dtEstrutura = this.MontarDatatableEstrutura("SalesStructureImportacao");

                using (var bulkCopy = new SqlBulkCopy(ConfiguracaoService.GetDatabaseConnection(), SqlBulkCopyOptions.TableLock))
                {
                    bulkCopy.BatchSize = 20000;
                    bulkCopy.BulkCopyTimeout = 3600;
                    bulkCopy.DestinationTableName = dtEstrutura.TableName;

                    int contadorLista = 0;
                    while (contadorLista < listaIndicacao.Count)
                    {
                        for (int limite = 0; limite < 100000 && contadorLista < listaIndicacao.Count; limite++)
                        {
                            dtEstrutura.Rows.Add(AdicionarLinhasDatatableEstrutura(listaIndicacao[contadorLista], dtEstrutura, arquivoId));
                            contadorLista++;
                        }

                        bulkCopy.WriteToServer(dtEstrutura);

                        dtEstrutura.Clear();
                    }
                }

            }
            catch (Exception erro)
            {
                throw new Exception(string.Format("Erro na execução do Processo: {0}", erro.Message));
            }
        }

        private DataTable MontarDatatableEstrutura(string tabelaDestino)
        {
            var _datatable = new DataTable(tabelaDestino);
            _datatable.Columns.Add("Id");

            foreach (var campo in MapearCampos())
            {
                if (_datatable.Columns[campo] == null)
                {
                    _datatable.Columns.Add(campo);
                    _datatable.Columns[campo].MaxLength = 100;
                }
            }

            _datatable.Columns.Add("DataInclusao", typeof(DateTime));
            _datatable.Columns.Add("DataAlteracao", typeof(DateTime));
            _datatable.Columns.Add("NumeroLinha");
            _datatable.Columns.Add("ArquivoId");
            _datatable.Columns["ArquivoId"].MaxLength = 100;


            return _datatable;
        }

        private new List<string> MapearCampos()
        {
            var campos = new List<string>();

            //Inicia propriedades de Detail
            campos.Add("DepartmentTypeAcronymText");
            campos.Add("DepartmentCode");
            campos.Add("DepartmentName");
            campos.Add("ParentDepartmentTypeAcronymText");
            campos.Add("ParentDepartmentCode");
            campos.Add("ParentDepartmentName");

            return campos;
        }

        private DataRow AdicionarLinhasDatatableEstrutura(SalesStructureModel lista, DataTable dt, int arquivoId)
        {
            try
            {
                DataRow row = dt.NewRow();

                row["Id"] = 0;
                row["NumeroLinha"] = lista.NumeroLinha;
                row["DataInclusao"] = DateTime.Now;
                row["DataAlteracao"] = DateTime.Now;
                row["ArquivoId"] = arquivoId;
                //Detail
                row["DepartmentTypeAcronymText"] = lista.DepartmentTypeAcronymText;
                row["DepartmentCode"] = lista.DepartmentCode;
                row["DepartmentName"] = lista.DepartmentName;
                row["ParentDepartmentTypeAcronymText"] = lista.ParentDepartmentTypeAcronymText;
                row["ParentDepartmentCode"] = lista.ParentDepartmentCode;
                row["ParentDepartmentName"] = lista.ParentDepartmentName;

                return row;

            }
            catch (Exception erro)
            {
                throw new Exception(string.Format("Erro na execução do Processo: {0}", erro.Message));
            }
        }

    }
}
