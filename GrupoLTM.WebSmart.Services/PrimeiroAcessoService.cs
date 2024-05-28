using GrupoLTM.WebSmart.Domain.Enums;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.DTO;
using GrupoLTM.WebSmart.DTO.MarketPlace;
using GrupoLTM.WebSmart.Infrastructure.Data;
using GrupoLTM.WebSmart.Services.LiveServiceMkt;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace GrupoLTM.WebSmart.Services
{
    public class PrimeiroAcessoService
    {
        public PrimeiroAcessoService() { }

        private CatalogoService _catalogoService = new CatalogoService();
        private ParticipanteCatalogoService _participanteCatalogoService = new ParticipanteCatalogoService();
        private static readonly string FILE_PATH_BASEACESSO = "forcarprimeiroacesso/baseRA/";
        public async Task IncluirProdutoResgateOffLine(int ArquivoId, long CatalogId, string PageName, int IdAdmin, int? ProjectId, string ProductSku, string ProductId, string Product, decimal Value, decimal ValueReal, string OriginalSku, decimal ConversionRate, string UrlImage)
        {
            using (SqlConnection cn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
            {
                await cn.OpenAsync();
                var transaction = cn.BeginTransaction();
                var executeList = new List<Task>();

                var dataTable = new DataTable("ResgateOffLine");

                dataTable.Columns.Add("Id", typeof(int));
                dataTable.Columns.Add("ArquivoUploadId", typeof(int));
                dataTable.Columns.Add("CatalogId", typeof(long));
                dataTable.Columns.Add("PageName", typeof(string));
                dataTable.Columns.Add("IdAdmin", typeof(int));
                dataTable.Columns.Add("ProjectId", typeof(int));
                dataTable.Columns.Add("ProductSku", typeof(string));
                dataTable.Columns.Add("ProductId", typeof(string));
                dataTable.Columns.Add("Product", typeof(string));
                dataTable.Columns.Add("Value", typeof(decimal));
                dataTable.Columns.Add("ValueReal", typeof(decimal));
                dataTable.Columns.Add("OriginalSku", typeof(string));
                dataTable.Columns.Add("ConversionRate", typeof(decimal));
                dataTable.Columns.Add("DataInclusao", typeof(DateTime));
                dataTable.Columns.Add("DataAlteracao", typeof(DateTime));
                dataTable.Columns.Add("ArquivoEnvioId", typeof(int));
                dataTable.Columns.Add("UrlImage", typeof(string));

                dataTable.Rows.Add(0, ArquivoId, CatalogId,
                    PageName, IdAdmin, ProjectId,
                    ProductSku, ProductId, Product,
                    Value, ValueReal, OriginalSku,
                    ConversionRate, DateTime.Now,
                    DateTime.Now, 0,
                    UrlImage);

                SqlBulkCopy copy = new SqlBulkCopy(cn, SqlBulkCopyOptions.Default, transaction);
                copy.BulkCopyTimeout = 900;
                copy.DestinationTableName = "ResgateOffLine";
                executeList.Add(copy.WriteToServerAsync(dataTable));

                transaction.Commit();
            }
        }
        public async Task IncluirForcarPrimeiroAcessoCatalogo(int ArquivoId, string PageName, int IdAdmin)
        {
            using (SqlConnection cn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
            {
                await cn.OpenAsync();
                var transaction = cn.BeginTransaction();
                var executeList = new List<Task>();

                var dataTable = new DataTable("ForcarPrimeiroAcessoCatalogo");

                dataTable.Columns.Add("Id", typeof(int));
                dataTable.Columns.Add("ArquivoUploadId", typeof(int));
                dataTable.Columns.Add("PageName", typeof(string));
                dataTable.Columns.Add("IdAdmin", typeof(int));
                dataTable.Columns.Add("DataInclusao", typeof(DateTime));

                dataTable.Rows.Add(0, ArquivoId, PageName, IdAdmin, DateTime.Now);

                SqlBulkCopy copy = new SqlBulkCopy(cn, SqlBulkCopyOptions.Default, transaction);
                copy.BulkCopyTimeout = 900;
                copy.DestinationTableName = "ForcarPrimeiroAcessoCatalogo";
                executeList.Add(copy.WriteToServerAsync(dataTable));
                transaction.Commit();
            }
        }

        public List<AuthorizeOrder> MapToAuthorizeOrder(int arquivoId)
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_SEL_ResgateOffLinePorArquivo";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@arquivoId", Value = arquivoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });

            DataTable table = DataProvider.SelectStoreProcedure(proc, listParam);
            List<AuthorizeOrder> list = new List<AuthorizeOrder>();

            //de para
            foreach (DataRow row in table.Rows)
            {
                List<AuthorizeConversionRate> convertionRate = new List<AuthorizeConversionRate>();
                convertionRate.Add(new AuthorizeConversionRate
                {
                    VendorId = Convert.ToInt64(row["VendorId"]),
                    ConversionFactor = Convert.ToInt32(row["conversionRate"])
                });

                List<AuthorizeItem> Items = new List<AuthorizeItem>();
                Items.Add(new AuthorizeItem
                {
                    ProductId = (string)row["productId"],
                    VendorSku = (string)row["originalSku"],
                    Sku = (string)row["productSku"],
                    Name = (string)row["product"],
                    Quantity = Convert.ToInt32(row["quantity"]),
                    VendorId = Convert.ToInt64(row["VendorId"]),
                    CostPrice = (decimal)row["valueReal"],
                    SellingPrice = (decimal)row["valueReal"],
                    Parameters = new JObject { { "ImpersonatorUser", row["participantId"].ToString() } },
                    ImageUrl = (string)row["UrlImage"]
                });

                List<AuthorizePhone> phones = new List<AuthorizePhone>();
                phones.Add(new AuthorizePhone
                {
                    DDD = (string)row["DDDPhone"],
                    Number = (string)row["NumberPhone"],
                    PhoneType = Convert.ToInt32(row["TypePhone"]),
                });

                var model = new AuthorizeOrder
                {
                    CampaignId = row["projectid"].ToString(),
                    Channel = new AuthorizeChannel { Id = 5 },
                    CreateDate = DateTime.Now,
                    CatalogId = Convert.ToInt64(row["CatalogId"]),
                    ConversionRate = convertionRate,
                    Items = Items,
                    SendEmail = false,
                    SendSMS = false,
                    Shipping = new AuthorizeShipping
                    {
                        AuthorizeAddress = new AuthorizeAddress
                        {
                            AddressName = (string)row["AddressName"],
                            AddressText = (string)row["AddressText"],
                            City = (string)row["City"],
                            Complement = (string)row["Complement"],
                            District = (string)row["District"],
                            Number = (string)row["Number"],
                            Reference = (string)row["Reference"],
                            State = (string)row["State"],
                            ZipCode = (string)row["ZipCode"]
                        },
                        AuthorizeReceiver = new AuthorizeReceiver
                        {
                            BirthDate = Convert.ToDateTime(row["birthDateParticipant"]),
                            CpfCnpj = (string)row["DocumentNumberParticipant"],
                            Email = (string)row["EmailParticipant"],
                            GenderType = 1,
                            PersonType = 1,
                            ReceiverName = (string)row["Name"],
                            Rg = new AuthorizeRg { Number = (string)row["RG"] },
                            StateRegistration = (string)row["State"],
                            Phones = phones
                        }
                    },
                    Participant = new AuthorizeParticipant
                    {
                        Id = Convert.ToInt64(row["participantId"]),
                        Name = (string)row["Name"],
                        UserName = (string)row["username"],
                        StateRegistration = (string)row["State"],
                        ClientId = Convert.ToInt64(row["projectid"]),
                        PersonType = 1,
                        Gender = 1,
                        CpfCnpj = (string)row["DocumentNumberParticipant"],
                        BirthDate = Convert.ToDateTime(row["birthDateParticipant"]),
                        Address = new AuthorizeAddress
                        {
                            AddressName = (string)row["AddressName"],
                            AddressText = (string)row["AddressText"],
                            Number = (string)row["Number"],
                            Complement = (string)row["Complement"],
                            District = (string)row["District"],
                            City = (string)row["City"],
                            State = (string)row["State"],
                            ZipCode = (string)row["ZipCode"],
                            Reference = (string)row["Reference"]
                        },
                        Email = (string)row["EmailParticipant"],
                        Phones = phones,
                        BirthPlace = new AuthorizeForeignProvince
                        {
                            Country = (string)row["Country"],
                            Province = (string)row["Province"]
                        }
                    }
                };
                list.Add(model);
            };

            return list;
        }
        public List<CSVResgateOffLineModel> ObterCSVResgateOffLine(int ArquivoId)
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_SEL_ResgateOffLineCSVPorArquivo";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@arquivoId", Value = ArquivoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });

            DataTable table = DataProvider.SelectStoreProcedure(proc, listParam);
            List<CSVResgateOffLineModel> list = new List<CSVResgateOffLineModel>();

            //de para
            foreach (DataRow row in table.Rows)
            {
                list.Add(new CSVResgateOffLineModel
                {
                    Username = (string)row["username"],
                    ProductSku = (string)row["productSku"],
                    ProductId = (string)row["productId"],
                    Product = (string)row["product"],
                    Value = (decimal)row["value"],
                    ValueReal = (decimal)row["valueReal"],
                    OriginalSku = (string)row["originalSku"],
                    ConversionRate = Convert.ToInt32(row["conversionRate"]),
                    Name = (string)row["Name"],
                    Quantity = Convert.ToInt32(row["quantity"]),
                    PedidoId = Convert.ToInt64(row["IdPedido"]),
                    Erro = (string)row["Erro"],
                    DataEnvio = (row["DataEnvio"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["DataEnvio"]))
                });
            }

            return list;
        }
        private List<ResgateOffLine> ObterResgateOffLine(int arquivoId)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repResgateOffLine = context.CreateRepository<ResgateOffLine>();

                    List<ResgateOffLine> resgates = new List<ResgateOffLine>();
                    resgates = repResgateOffLine.Filter<ResgateOffLine>(
                        x => x.ArquivoUploadId == arquivoId).ToList();

                    return resgates;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private ForcarPrimeiroAcessoCatalogo ObterForcarPrimeiroAcesso(int arquivoId)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repForcarPrimeiroAcessoCatalogo = context.CreateRepository<ForcarPrimeiroAcessoCatalogo>();

                    var listaForcar = repForcarPrimeiroAcessoCatalogo.Filter<ForcarPrimeiroAcessoCatalogo>(
                        x => x.ArquivoUploadId == arquivoId).FirstOrDefault();

                    return listaForcar;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async void FirstAccessParticipant(int arquivoId, int tipoArquivoId)
        {

            List<ResgateOffLine> resgateoffLine = new List<ResgateOffLine>();
            ForcarPrimeiroAcessoCatalogo forcarPrimeiroAcessoCatalogo = new ForcarPrimeiroAcessoCatalogo();
            try
            {
                List<string> ListAccountNumber = null;

                if (tipoArquivoId == (int)EnumDomain.TipoArquivo.ForcarPrimeiroAcesso)
                {

                    forcarPrimeiroAcessoCatalogo = ObterForcarPrimeiroAcesso(arquivoId);
                    if (forcarPrimeiroAcessoCatalogo != null)
                    {
                        ListAccountNumber = ObterRAsForcarPrimeiroAcessoLote(arquivoId);
                        gravaLogProcessamento("[Services.FirstAccessParticipant] Forçando 1º acesso de " + ListAccountNumber.Count().ToString() + " Logins.", "GrupoLTM.Avon.MMA.Pontuacao.GeracaoArquivoRegstaeOffLine", "Main", "jobCatalog");
                        //só atualiza os participantes que não tem marketplace Id
                        ForcarPrimeiroAcessoListaRA(ListAccountNumber, forcarPrimeiroAcessoCatalogo.IdAdmin);

                        AtualizaStatusArquivoForcarPrimeiroAcesso(arquivoId, "", EnumDomain.StatusArquivo.Processado);
                    }
                    else
                    {
                        gravaLogProcessamento(String.Format("[Services.FirstAccessParticipant] ARQUIVO NÃO ENCONTRADO. ArquivoId{0}", arquivoId.ToString()), "GrupoLTM.Avon.MMA.Pontuacao.GeracaoArquivoRegstaeOffLine", "Main", "jobCatalog");
                    }
                }
                else if (tipoArquivoId == (int)EnumDomain.TipoArquivo.ResgateOffline)
                {
                    resgateoffLine = ObterResgateOffLine(arquivoId);
                    if (resgateoffLine.Count() > 0)
                    {
                        ListAccountNumber = ObterRAsResgateOffline(arquivoId);
                        gravaLogProcessamento("[Services.FirstAccessParticipant] RESGATE OFFLINE - Forçando 1º acesso de " + ListAccountNumber.Count().ToString() + " Logins.", "GrupoLTM.Avon.MMA.Pontuacao.GeracaoArquivoRegstaeOffLine", "Main", "jobCatalog");
                        //só atualiza os participantes que não tem marketplace Id
                        ForcarPrimeiroAcessoListaRA(ListAccountNumber, resgateoffLine[0].IdAdmin);

                        //EFETIVA RESGATE OFFLINE NO LIVE
                        bool disparo = await DispararAutorizacaoPedidoOffLine(arquivoId);
                        AtualizaStatusArquivoResgateOffLine(arquivoId, disparo ? "" : "Erro no disparo das autorizações de pedidos.", disparo ? EnumDomain.StatusArquivo.Processado : EnumDomain.StatusArquivo.ErroNaoImportado);
                    }
                    else
                    {
                        gravaLogProcessamento(String.Format("[Services.FirstAccessParticipant] RESGATES OFFLINE NÃO ENCONTRADOS. ArquivoId{0}", arquivoId.ToString()), "GrupoLTM.Avon.MMA.Pontuacao.GeracaoArquivoRegstaeOffLine", "Main", "jobCatalog");
                    }
                }

            }
            catch (Exception ex)
            {
                if (tipoArquivoId == (int)EnumDomain.TipoArquivo.ResgateOffline)
                {
                    AtualizaStatusArquivoResgateOffLine(arquivoId, ex.Message, EnumDomain.StatusArquivo.ErroNaoImportado);
                    gravaLogErro(String.Format("Erro ao realizar o resgate offline dos participantes. ArquivoId {}", arquivoId.ToString()), ex.Message, "GrupoLTM.WebSmart.Admin", "FirstAccessParticipant", "jobCatalog");

                }
                else if (tipoArquivoId == (int)EnumDomain.TipoArquivo.ForcarPrimeiroAcesso)
                {
                    AtualizaStatusArquivoForcarPrimeiroAcesso(arquivoId, ex.Message, EnumDomain.StatusArquivo.ErroNaoImportado);
                    gravaLogErro(String.Format("Erro ao forçar o primeiro acesso dos participantes. ArquivoId {0}", arquivoId.ToString()), ex.Message, "GrupoLTM.WebSmart.Admin", "FirstAccessParticipant", "jobCatalog");
                }

                throw ex;
            }
        }

        private string ForcarPrimeiroAcessoListaRA(List<string> ListAccountNumber, int? UsuarioAdminId)
        {
            string retorno = "";
            foreach (string AccountNumber in ListAccountNumber)
            {
                PrimeiroAcessoService primeiroAcesso = new PrimeiroAcessoService();
                retorno = primeiroAcesso.ForcarPrimeiroAcessoMktPlace(AccountNumber, "extrato", UsuarioAdminId);
            }
            return retorno;
        }

        public ParticipanteCatalogo ObterParticipanteCatalogo(long accountNumber, int? idAdmin, out int catalogId)
        {
            var userAvon = AvonService.GetNewUserInfo(accountNumber, "idAdmin-GetAccountAvon-" + idAdmin);

            var catalogo = userAvon != null ? AvonService.GetCatalogoUserInfo(userAvon) : null;

            catalogId = catalogo != null ? (int)catalogo.Codigo : ConfiguracaoService.MktPlaceCatalogoLogarComo();

            var participanteCatalogo = _participanteCatalogoService.GetByLoginCatalogo(accountNumber.ToString(), catalogId);

            return participanteCatalogo;
        }

        public string ForcarPrimeiroAcessoMktPlace(string accountNumber, string pageName, int? idAdmin)
        {
            try
            {
                var participanteCatalogo = ObterParticipanteCatalogo(Convert.ToInt64(accountNumber), idAdmin, out var catalogId);

                if (participanteCatalogo == null)
                {
                    var avonAuthentication = new AvonAuthentication
                    {
                        AccountNumber = accountNumber,
                        CatalogId = catalogId,
                        PageName = pageName,
                        i = idAdmin
                    };

                    var hasInvalidEmail = false;

                    var token = OAuthService.GetToken(avonAuthentication);

                    var mktPlaceCatalogoId = _catalogoService.ObterCatalogoContext(catalogId).Codigo;

                    var result = OAuthService.CheckParticipant(accountNumber, idAdmin, mktPlaceCatalogoId, token, out hasInvalidEmail);

                    return result == "" ? "Primeiro Acesso realizado com sucesso!" : result;
                }
                else
                {
                    return "Participante já realizou o primeiro acesso no Catálogo selecionado.";
                }
            }
            catch (Exception ex)
            {
                gravaLogProcessamento("26.[Services.ForcarPrimeiroAcessoMktPlace] OAuthService.CheckParticipant: resultERRO =" + ex.Message, "GrupoLTM.Avon.MMA.Pontuacao.GeracaoArquivoRegstaeOffLine", "Main", "jobCatalog");
                throw ex;
            }
        }

        private async Task<bool> DispararAutorizacaoPedidoOffLine(int arquivoId)
        {
            try
            {
                gravaLogProcessamento("DispararAutorizacaoPedidoOffLine Id: " + arquivoId.ToString(), "Pontuacao.GeracaoArquivoResgateOffLine", "Main", "jobCatalog");
                List<AuthorizeOrder> listAuthorizeOrders = new List<AuthorizeOrder>();
                listAuthorizeOrders = MapToAuthorizeOrder(arquivoId);

                foreach (AuthorizeOrder authorizeOrder in listAuthorizeOrders)
                {
                    var result = await OrderService.AutorizacaoPedido(authorizeOrder);
                    AtualizaIdPedido(arquivoId, result.OrderId, authorizeOrder.Participant.UserName, JsonConvert.SerializeObject(result.Errors));
                }

                return true;
            }
            catch (Exception ex)
            {
                gravaLogErro("Erro Disparar Autorização de Pedido Offline.", ex.Message, "GrupoLTM.WebSmart.Services", "DispararAutorizacaoPedidoOffLine", "jobCatalog");

                return false;
            }
        }
        private void AtualizaErroResgateOffLine(string AccountNumber, int arquivoId, string errorMessage)
        {

            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repForcarPrimeiroAcesso = context.CreateRepository<ForcarPrimeiroAcesso>();
                List<ForcarPrimeiroAcesso> listParticipante = repForcarPrimeiroAcesso.Filter<ForcarPrimeiroAcesso>(x => x.Login == AccountNumber && x.ArquivoId == arquivoId && x.DataEnvio == null).ToList();

                foreach (var item in listParticipante)
                {
                    item.Erro = errorMessage;
                }

                using (TransactionScope scope = new TransactionScope())
                {
                    repForcarPrimeiroAcesso.SaveChanges();
                    scope.Complete();
                }

            }
        }
        private void AtualizaIdPedido(int arquivoId, long? orderId, string loginName, string erroS)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repForcarPrimeiroAcesso = context.CreateRepository<ForcarPrimeiroAcesso>();
                ForcarPrimeiroAcesso Participante = repForcarPrimeiroAcesso.Filter<ForcarPrimeiroAcesso>(x => x.ArquivoId == arquivoId && x.Login == loginName).FirstOrDefault();
                if (Participante != null)
                {
                    Participante.IdPedido = orderId;
                    Participante.Erro = erroS;
                    Participante.DataEnvio = DateTime.Now;

                    using (TransactionScope scope = new TransactionScope())
                    {
                        repForcarPrimeiroAcesso.SaveChanges();
                        scope.Complete();
                    }
                }
                repForcarPrimeiroAcesso.Dispose();
            }
        }
        private void AtualizaStatusArquivoResgateOffLine(int? arquivoId, string errorMessage, EnumDomain.StatusArquivo eStatus)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                gravaLogProcessamento("AtualizaStatusArquivoResgateOffLine arquivoId:" + arquivoId.ToString() + ". errorMessage:" + errorMessage.ToString() + ". StatusArquivo:" + eStatus.ToString(), "GrupoLTM.Avon.MMA.Pontuacao.GeracaoArquivoRegstaeOffLine", "Main", "jobCatalog");
                IRepository repArquivo = context.CreateRepository<Arquivo>();
                var arquivo = repArquivo.Filter<Arquivo>(x => x.Id == arquivoId).FirstOrDefault();
                arquivo.StatusArquivoId = Convert.ToInt32(eStatus);
                arquivo.DataAlteracao = DateTime.Now;

                //Grava
                using (TransactionScope scope = new TransactionScope())
                {
                    repArquivo.SaveChanges();
                    scope.Complete();
                }
                if (eStatus == EnumDomain.StatusArquivo.ErroNaoImportado)
                {
                    gravaLogErro("Erro ao Gerar Arquivo Resgate Off Line.", errorMessage, "GrupoLTM.WebSmart.GerarArquivoResgateOffLine", "AtualizaErroSucessoResgateOffLine", "jobCatalog");

                }
                if (eStatus == EnumDomain.StatusArquivo.Inconsistente)
                {
                    gravaLogErro("Erro ao Enviar Arquivo Resgate Off Line para o Blob.", errorMessage, "GrupoLTM.WebSmart.GerarArquivoResgateOffLine", "AtualizaErroSucessoResgateOffLine", "jobCatalog");

                }
            }
        }
        private void AtualizaStatusArquivoForcarPrimeiroAcesso(int? arquivoId, string errorMessage, EnumDomain.StatusArquivo eStatus)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                gravaLogProcessamento("AtualizaStatusArquivoForcarPrimeiroAcesso arquivoId:" + arquivoId.ToString() + ". errorMessage:" + errorMessage.ToString() + ". StatusArquivo:" + eStatus.ToString(), "GrupoLTM.Avon.MMA.Pontuacao.GeracaoArquivoRegstaeOffLine", "Main", "jobCatalog");
                IRepository repArquivo = context.CreateRepository<Arquivo>();
                var arquivo = repArquivo.Filter<Arquivo>(x => x.Id == arquivoId).FirstOrDefault();
                arquivo.StatusArquivoId = Convert.ToInt32(eStatus);
                arquivo.DataAlteracao = DateTime.Now;

                //Grava
                using (TransactionScope scope = new TransactionScope())
                {
                    repArquivo.SaveChanges();
                    scope.Complete();
                }
                if (eStatus == EnumDomain.StatusArquivo.ErroNaoImportado)
                {
                    gravaLogErro("Erro ao Gerar Arquivo Forçar Primeiro Acesso.", errorMessage, "GrupoLTM.WebSmart.GerarArquivoResgateOffLine", "AtualizaErroSucessoForcarPrimeiroAcesso", "jobCatalog");

                }
            }
        }
        public List<string> ObterRAsForcarPrimeiroAcessoLote(int ArquivoId)
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_SEL_RaForcarPrimeiroAcessoLotePorArquivo";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@arquivoId", Value = ArquivoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });

            DataTable table = DataProvider.SelectStoreProcedure(proc, listParam);
            List<string> list = new List<string>();

            //de para
            foreach (DataRow row in table.Rows)
            {
                list.Add((string)row["AccountNumber"]);
            }

            return list;
        }
        public List<string> ObterRAsResgateOffline(int ArquivoId)
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_SEL_RaPrimeiroAcessoPorArquivo";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@arquivoId", Value = ArquivoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });

            DataTable table = DataProvider.SelectStoreProcedure(proc, listParam);
            List<string> list = new List<string>();

            //de para
            foreach (DataRow row in table.Rows)
            {
                list.Add((string)row["AccountNumber"]);
            }

            return list;
        }
        public async Task AtualizarRAsPrimeiroAcesso(int arquivoId, Dictionary<string, int> ras)
        {
            using (SqlConnection cn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
            {
                await cn.OpenAsync();
                var transaction = cn.BeginTransaction();
                var executeList = new List<Task>();

                var dataTable = new DataTable("ForcarPrimeiroAcesso");

                dataTable.Columns.Add("Id", typeof(int));
                dataTable.Columns.Add("Login", typeof(string));
                dataTable.Columns.Add("DataInclusao", typeof(DateTime));
                dataTable.Columns.Add("DataEnvio", typeof(DateTime));
                dataTable.Columns.Add("ArquivoId", typeof(int));
                dataTable.Columns.Add("Erro", typeof(string));
                dataTable.Columns.Add("Quantity", typeof(int));
                dataTable.Columns.Add("IdPedido", typeof(long));


                foreach (var a in ras)
                {
                    dataTable.Rows.Add(0, a.Key, DateTime.Now, null, arquivoId, null, a.Value, null);
                }

                SqlBulkCopy copy = new SqlBulkCopy(cn, SqlBulkCopyOptions.Default, transaction);
                copy.BulkCopyTimeout = 900;
                copy.DestinationTableName = "ForcarPrimeiroAcesso";
                executeList.Add(copy.WriteToServerAsync(dataTable));


                await Task.WhenAll(executeList);

                transaction.Commit();
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
                Controller = "ArquivoService",
                Pagina = string.Empty,
                Codigo = Codigo
            };

            var logErroService = new LogErroService();
            logErroService.SalvarLogErro(logErro);
        }
        static void gravaLogProcessamento(string Mensagem, string Source, string Metodo, string codigo)
        {
            Console.WriteLine(DateTime.Now.ToString() + " - " + Mensagem);
            var logProcessamento = new LogProcessamentoDb()
            {
                Metodo = Metodo,
                Controller = "GeracaoArquivoResgateOffiLine - Primeiro Acesso",
                Codigo = codigo,
                Mensagem = Mensagem,
                Source = Source,
                DataInclusao = DateTime.Now
            };

            var logService = new LogService();
            logService.GravarLogProcessamento(logProcessamento);
        }
    }

}
