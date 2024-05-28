using GrupoLTM.WebSmart.Domain.Enums;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Models.MktPlace;
using GrupoLTM.WebSmart.DTO;
using GrupoLTM.WebSmart.Infrastructure.Cache;
using GrupoLTM.WebSmart.Infrastructure.Exceptions;
using GrupoLTM.WebSmart.Infrastructure.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.Services.JwtAuth;
using System.Threading.Tasks;
using System.Configuration;
using static GrupoLTM.WebSmart.Domain.Enums.EnumMktPlace;

namespace GrupoLTM.WebSmart.Services
{
    public class ExtratoService
    {
        private static readonly CognitoService _cognitoService = new CognitoService();

        #region Redis

        private ExtratoTotalizado ObterHeaderExtratoTotalizadoCache(int accountNumber, int mktPlaceCatalogoId, string chave)
        {
            CacheAttribute cacheAttribute = new CacheAttribute();
            return cacheAttribute.GetHashObjectExtrato<ExtratoTotalizado>(chave + "_" + accountNumber.ToString() + "_" + mktPlaceCatalogoId.ToString());
        }

        private void GravarHeaderExtratoTotalizadoCache(int accountNumber, int mktPlaceCatalogoId, object lExtrato, string chave)
        {
            CacheAttribute cacheAttribute = new CacheAttribute();
            cacheAttribute.StoreObjectExtrato(chave + "_" + accountNumber.ToString() + "_" + mktPlaceCatalogoId.ToString(), lExtrato, "CacheExtrato");
        }

        private List<HeaderExtrato> ObterHeaderExtratoCache(int accountNumber, int mktPlaceCatalogoId, string chave)
        {
            CacheAttribute cacheAttribute = new CacheAttribute();
            return cacheAttribute.GetHashObjectExtrato<List<HeaderExtrato>>(chave + "_" + accountNumber.ToString() + "_" + mktPlaceCatalogoId.ToString());
        }

        private void GravarHeaderExtratoCache(int accountNumber, int mktPlaceCatalogoId, object lExtrato, string chave)
        {
            CacheAttribute cacheAttribute = new CacheAttribute();
            cacheAttribute.StoreObjectExtrato(chave + "_" + accountNumber.ToString() + "_" + mktPlaceCatalogoId.ToString(), lExtrato, "CacheExtrato");
        }

        public void LimparExtratoCachePorLogin(List<string> ListaLogin)
        {
            CacheAttribute cacheAttribute = new CacheAttribute();
            foreach (string login in ListaLogin)
            {
                cacheAttribute.KeyDeleteAsync("AvonMMA_extratoTotalizado__" + login);
                cacheAttribute.KeyDeleteAsync("AvonMMA_linhasExtrato_N_" + login);
                cacheAttribute.KeyDeleteAsync("AvonMMA_linhasExtrato_S_" + login);
                cacheAttribute.KeyDeleteAsync("AvonMMA_linhasExtrato_C_" + login);
                cacheAttribute.KeyDeleteAsync("CacheExtratoNatal_" + login);
                cacheAttribute.KeyDeleteAsync("CacheExtratoAlianca_" + login);
            }
        }

        #endregion

        #region MktPlace

        public async Task<decimal?> ObterSaldo(string accountNumber, int mktPlaceCatalogoId, string accessTokenMktPlace, int mktPlaceParticipantId, string impersonateUser)
        {
            if (string.IsNullOrEmpty(accessTokenMktPlace))
                accessTokenMktPlace = await _cognitoService.SignIn();

            try
            {
                var mtkplaceUrl = ConfiguracaoService.MktPlaceUrl();

                var customerId = ConfigurationManager.AppSettings["mktPlace_clientId"];

                var authorization = "Bearer " + accessTokenMktPlace;

                var jsonRequest = new JsonRequest(mtkplaceUrl,
                    new KeyValuePair<string, string>("Authorization", authorization),
                    new KeyValuePair<string, string>("participantid", mktPlaceParticipantId.ToString()),
                    new KeyValuePair<string, string>("catalogId", mktPlaceCatalogoId.ToString()),
                    new KeyValuePair<string, string>("customerId", customerId),
                    new KeyValuePair<string, string>("impersonatorUser", impersonateUser),
                    new KeyValuePair<string, string>("username", accountNumber.ToString()));

                var url = "v1/participants/me/balance";

                var response = jsonRequest.Get<decimal?>(url);

                var balance = response ?? 0.0M;

                return balance;
            }
            catch (Exception ex)
            {
                throw new MarketPlaceException("Erro ao requisitar a api /v1/participants/me/balance", ex);
            }
        }

        public async Task<ReversedBalance> ObterSaldoEstornado(int accountNumber, int mktPlaceCatalogoId, string accessTokenMktPlace, int mktPlaceParticipantId, int impersonateUser)
        {
            if (string.IsNullOrEmpty(accessTokenMktPlace))
                accessTokenMktPlace = await _cognitoService.SignIn();

            string customerId = ConfigurationManager.AppSettings["mktPlace_clientId"];

            var autthorization = "Bearer " + accessTokenMktPlace;

            JsonRequest jsonRequest = new JsonRequest(ConfiguracaoService.MktPlaceUrl(),
                new KeyValuePair<string, string>("Authorization", autthorization),
                new KeyValuePair<string, string>("participantid", mktPlaceParticipantId.ToString()),
                new KeyValuePair<string, string>("catalogId", mktPlaceCatalogoId.ToString()),
                new KeyValuePair<string, string>("customerId", customerId),
                new KeyValuePair<string, string>("impersonatorUser", impersonateUser.ToString()),
                new KeyValuePair<string, string>("username", accountNumber.ToString()));

            try
            {
                //var url = "/v1/participants/me/extract";
                var url = "/v1/participants/me/accountStatement/from/2016-07-01/to/2030-08-05/1/999999999";
                List<AccountStatementModel> data = jsonRequest.Get<List<AccountStatementModel>>(url);

                return new ReversedBalance()
                {
                    DebitCurrencyBalance = data.Where(x => x.transactionTypeId == 10).Sum(x => x.valueInPoints),
                    RedeemBalance = data.Where(x => x.transactionTypeId == 9).Sum(x => x.valueInPoints) + data.Where(x => x.transactionTypeId == 17).Sum(x => x.valueInPoints)

                    //DebitCurrencyBalance = data.Where(x => x.description.Contains("Estorno")).Sum(x => x.valueInPoints),
                    //RedeemBalance = data.Where(x => x.description.Contains("Resgate")).Sum(x => x.valueInPoints)
                };
            }
            catch (Exception ex)
            {
                throw new MarketPlaceException(ex.Message, ex);
            }
        }

        public async Task<ReversePontosMaisCash> ObterPontosCash(int accountNumber, int mktPlaceCatalogoId, string accessTokenMktPlace, int mktPlaceParticipantId)
        {
            if (string.IsNullOrEmpty(accessTokenMktPlace))
                accessTokenMktPlace = await _cognitoService.SignIn();

            var autthorization = "Bearer " + accessTokenMktPlace;
            JsonRequest jsonRequest = new JsonRequest(ConfiguracaoService.MktPlaceUrl(), new KeyValuePair<string, string>("Authorization", autthorization));

            try
            {
                var url = $"/v1/participants/me/accountStatement/from/2016-07-01/to/{DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd")}/1/{accountNumber.ToString()}";
                List<AccountStatementModel> data = jsonRequest.Get<List<AccountStatementModel>>(url);

                return new ReversePontosMaisCash()
                {
                    DebitCashBalance = 0,
                    RedeemCash = data.Where(x => x.transactionTypeId == 4 &&
                                                 x.originAccountHolderId == (long)EnumMktPlace.OriginAccountHolder.OriginAccountHolderId)
                        .Sum(x => x.valueInPoints)
                };

            }
            catch (Exception ex)
            {
                throw new MarketPlaceException(ex.Message, ex);
            }

        }

        public async Task<string> ObterDataExpiracao(int accountNumber, int mktPlaceCatalogoId, string accessTokenMktPlace, int mktPlaceParticipantId)
        {
            if (string.IsNullOrEmpty(accessTokenMktPlace))
                accessTokenMktPlace = await _cognitoService.SignIn();

            var autthorization = "Bearer " + accessTokenMktPlace;
            JsonRequest jsonRequest = new JsonRequest(ConfiguracaoService.MktPlaceUrl(), new KeyValuePair<string, string>("Authorization", autthorization));

            try
            {
                var url = "/v1/participants/me/toExpire/6";
                List<dynamic> balance = jsonRequest.Get<List<dynamic>>(url);

                if (balance.Count() > 0)
                    return ((DateTime)((JContainer)((JContainer)(balance.FirstOrDefault())).Last).First).ToString("dd/MM/yyyy");
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                throw new MarketPlaceException(ex.Message, ex);
            }
        }

        public async Task<string> ObterSaldoAExpirar(string accountNumber, int mktPlaceCatalogoId, ExpirationInterval range)
        {
            var tokenCatalogo = await _cognitoService.SignInCatalogo();

            var catalogoUrlBase = ConfigurationManager.AppSettings["CatalogoUrlBase"];
            var ssoSubscriptionKey = ConfigurationManager.AppSettings["SsoSubscriptionKey"];

            var parameters = new KeyValuePair<string, string>[]
            {
                    new KeyValuePair<string, string>("Authorization", "Bearer " + tokenCatalogo),
                    new KeyValuePair<string, string>("ocp-apim-subscription-key", ssoSubscriptionKey),
                    new KeyValuePair<string, string>("username", accountNumber.ToString()),
                    new KeyValuePair<string, string>("catalog_id", mktPlaceCatalogoId.ToString()),
            };

            var jsonRequest = new JsonRequest(catalogoUrlBase, parameters);

            var url = "v1/participants/me/toExpire/" + (int)range;

            var balance = jsonRequest.Get<List<PontosExpirarExtratoItensMkt>>(url);

            if (balance.Any())
            {
                var points = balance.Sum(x => x.points);
                return Math.Abs(points).ToString();
            }

            return "0";
        }

        public string ObterNomeVitrinePorCatalogo(long MktPlaceCatalogoId)
        {
            switch (MktPlaceCatalogoId)
            {
                case 40322:
                    return "Catalogo-I";
                case 40518:
                    return "avon-diamante";
                case 40513:
                    return "avon-bronze";
                case 40514:
                    return "avon-prata";
                case 40515:
                    return "avon-ouro";
                case 40516:
                    return "avon-vip";
                case 40517:
                    return "avon-safira";
                case 40519:
                    return "avon-superestrela";
                default:
                    return "Catalogo-I";
            }
        }

        public async Task<ProductsShowCase> ObterShowcaseWithPrice(string accessTokenMktPlace, string identifier, string type, long MktPlaceCatalogoId)
        {

            if (string.IsNullOrEmpty(accessTokenMktPlace))
                accessTokenMktPlace = await _cognitoService.SignIn();

            var autthorization = "Bearer " + accessTokenMktPlace;
            JsonRequest jsonRequest = new JsonRequest(ConfiguracaoService.MktPlaceUrl(),
                new KeyValuePair<string, string>("Authorization", autthorization),
                new KeyValuePair<string, string>("identifier", identifier),
                new KeyValuePair<string, string>("type", type),
                new KeyValuePair<string, string>("catalogId", MktPlaceCatalogoId.ToString()));

            ProductsShowCase ShowcaseWithPrice = new ProductsShowCase();

            try
            {
                var url = "";

                if (identifier == "avonmma-servicos")
                {
                    url = "v1/stores/getStoresAllStatus?";
                    url += "catalogId=" + MktPlaceCatalogoId.ToString();

                    List<StoreModel> storeModels = jsonRequest.Get<List<StoreModel>>(url);
                    List<StoreModel> servicoStoreModels = new List<StoreModel>();
                    servicoStoreModels = storeModels.FindAll(x => x.SupplierTypeId == "4" || x.SupplierTypeId == "6" || x.SupplierTypeId == "14" || x.SupplierTypeId == "15").ToList();

                    ShowcaseWithPrice.Products = new List<ProductResultModel>();

                    foreach (StoreModel item in servicoStoreModels)
                    {

                        ProductResultModel product = new ProductResultModel();
                        product.Image = item.ImageUrl;
                        product.ProductSkuId = item.SupplierId.ToString();
                        product.ProductName = item.Name;
                        product.SellingPrice = 10;
                        ShowcaseWithPrice.Products.Add(product);
                    }

                }
                else
                {
                    url = "v1/showcases/withprice?";
                    url += "catalogid=" + MktPlaceCatalogoId + "&identifier=" + identifier + "&type=" + type + "&offset=0&itemsPerPage=30";
                    ShowcaseWithPrice = jsonRequest.Get<ProductsShowCase>(url);
                    //se for a landing de parceiro, devemos validar a disponibilidade
                    if (identifier == "avonmma-landingparceiro")
                    {
                        foreach (ProductResultModel produto in ShowcaseWithPrice.Products)
                        {
                            url = "/v2/products/" + produto.ProductSkuId + "/availability?catalogid=" + MktPlaceCatalogoId.ToString() + "&vendorid=" + produto.VendorId.ToString() + "&originalproductskuid=" + produto.OriginalProductId + " &clientId=306&clusterId=null";
                            var availability = jsonRequest.Get<dynamic>(url);
                            bool erro = false;

                            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(availability))
                            {
                                if (prop.Name == "errors")
                                {
                                    erro = true;
                                }
                            }

                            if (!erro)
                            {
                                foreach (var i in availability)
                                {
                                    var valor = i.sellingPrice;
                                    var isAvailable = i.isAvailable;
                                    produto.SellingPrice = (decimal)valor;
                                    produto.IsAvailable = (bool)isAvailable;
                                }
                            }
                        }
                    }
                }
                return ShowcaseWithPrice;
            }
            catch (Exception ex)
            {
                GravarLogErro("Erro ao buscar Vitrine" + identifier, ex.Message, "GrupoLTM.WebSmart.Services", "ObterShowCaseWithPrice", "WebSiteExtrato");
                return null;
                //throw new MarketPlaceException(ex.Message);  
            }
        }

        #endregion

        public ExtratoTotalizado ObterTotalizado(int accountNumber, int mktPlaceCatalogoId)
        {
            var extratoTotalizado = new ExtratoTotalizado();

            extratoTotalizado = ObterHeaderExtratoTotalizadoCache(accountNumber, mktPlaceCatalogoId, "AvonMMA_extratoTotalizado_");

            if (extratoTotalizado == null)
            {
                using (SqlConnection cn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
                {
                    SqlCommand cmd = new SqlCommand("JP_PRC_SummaryExtrato", cn);
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@AccountNumber", Value = accountNumber, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@MktPlaceCatalogoId", Value = mktPlaceCatalogoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });

                    cmd.CommandTimeout = 180; //3 minutos
                    cmd.CommandType = CommandType.StoredProcedure;
                    cn.Open();

                    extratoTotalizado = new ExtratoTotalizado();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            switch (reader["StatusPoints"].ToString())
                            {
                                case "S":
                                    extratoTotalizado.PontosDisponiveis += Convert.ToInt32(reader["Points"]);
                                    break;

                                case "N":
                                    extratoTotalizado.PontosPendentes += Convert.ToInt32(reader["Points"]);
                                    break;

                                default:
                                    extratoTotalizado.PontosCancelados += Convert.ToInt32(reader["Points"]);
                                    break;
                            }
                        }
                    }
                    GravarHeaderExtratoTotalizadoCache(accountNumber, mktPlaceCatalogoId, extratoTotalizado, "AvonMMA_extratoTotalizado_");
                }
            }
            return extratoTotalizado;
        }

        public List<HeaderExtrato> ObterHeaderPorTipo(int accountNumber, string tipoPonto, int mktPlaceCatalogoId)
        {
            var cacheKey = $"AvonMMA_linhasExtrato_{tipoPonto}_{accountNumber}";

            var linhasExtrato = ObterHeaderExtratoCache(accountNumber, mktPlaceCatalogoId, cacheKey);

            if (linhasExtrato == null)
            {
                linhasExtrato = ExecutarExtratosAsync(accountNumber, tipoPonto, mktPlaceCatalogoId);
                GravarHeaderExtratoCache(accountNumber, mktPlaceCatalogoId, linhasExtrato, cacheKey);
            }

            return linhasExtrato;
        }

        public List<HeaderExtrato> ExecutarExtratosAsync(int accountNumber, string tipoPonto, int mktPlaceCatalogoId)
        {
            List<HeaderExtrato> extratoIndicacao = null,
                extratoApoio = null,
                extratoConsecutividade = null,
                extratoClubeEstrelas = null,
                extratoMigracao = null;

            var linhasExtrato = new List<HeaderExtrato>();

            var taskIndicacao = Task.Run(() =>
            {
                extratoIndicacao = ObterHeaderPorTipoIndicacao(accountNumber, tipoPonto, mktPlaceCatalogoId);
            });

            var taskApoio = Task.Run(() =>
            {
                extratoApoio = ObterHeaderPorTipoApoio(accountNumber, tipoPonto, mktPlaceCatalogoId);
            });

            var taskConsecutividade = Task.Run(() =>
            {
                extratoConsecutividade = ObterHeaderPorTipoConsecutividade(accountNumber, tipoPonto, mktPlaceCatalogoId);
            });

            var taskClube = Task.Run(() =>
            {
                extratoClubeEstrelas = ObterHeaderPorTipoClubeDasEstrelas(accountNumber, tipoPonto);
            });

            var taskMigracao = Task.Run(() =>
            {
                extratoMigracao = ObterHeaderPorTipoMigracao(accountNumber, tipoPonto);
            });

            Task.WaitAll(taskIndicacao, taskApoio, taskConsecutividade, taskClube, taskMigracao);

            linhasExtrato.AddRange(extratoIndicacao.Union(extratoApoio).Union(extratoConsecutividade).Union(extratoClubeEstrelas).Union(extratoMigracao));

            return linhasExtrato;
        }

        public List<HeaderExtrato> ObterHeaderPorTipoConsecutividade(int accountNumber, string tipoPonto, int mktPlaceCatalogoId)
        {
            List<HeaderExtrato> linhasExtrato = new List<HeaderExtrato>();
            using (SqlConnection cn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
            {
                SqlCommand cmd = new SqlCommand("JP_PRC_ExtratoPorTipoConsecutividade", cn);
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@AccountNumber", Value = accountNumber, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@StatusPoints", Value = tipoPonto, SqlDbType = SqlDbType.Char, Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@MktPlaceCatalogoId", Value = mktPlaceCatalogoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });

                cmd.CommandTimeout = 2400; //40 minutos
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                linhasExtrato = new List<HeaderExtrato>();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        HeaderExtrato headerExtrato = new HeaderExtrato();
                        headerExtrato.Id = Convert.ToInt32(reader["Id"]);

                        headerExtrato.CampaignNumber = Convert.ToInt32(reader["CampaignNumberHeader"]);
                        headerExtrato.CampaignYearNumber = Convert.ToInt32(reader["CampaignYearNumberHeader"]);
                        headerExtrato.ReferralRepresentativeNumber = reader["ReferralRepresentativeNumberHeader"].ToString();
                        headerExtrato.ReferralName = reader["ReferralNameHeader"].ToString().Trim();
                        headerExtrato.PendingPayment = reader["PendingPaymentHeader"].ToString();
                        headerExtrato.RepresentativeStatus = reader["RepresentativeStatusHeader"].ToString();
                        headerExtrato.Points = Convert.ToInt32(reader["Points"]);
                        headerExtrato.TransactionDate = Convert.ToDateTime(reader["TransactionDateHeader"]);
                        headerExtrato.ReferredRepresentativeNumber = reader["ReferredRepresentativeNumberHeader"].ToString();
                        headerExtrato.ReferredNameDetail = reader["ReferredNameDetail"].ToString().Trim();
                        headerExtrato.StatusPoints = reader["StatusPoints"].ToString();
                        headerExtrato.TypePoints = reader["TypePoints"].ToString();
                        headerExtrato.ProgramType = Convert.ToInt32(reader["ProgramType"]);

                        headerExtrato.ProgramDescription = reader["IncentiveProgramDescriptionHeader"].ToString();

                        linhasExtrato.Add(headerExtrato);
                    }
                }

                cn.Close();
            }

            return linhasExtrato;

        }

        public List<HeaderExtrato> ObterHeaderPorTipoApoio(int accountNumber, string tipoPonto, int mktPlaceCatalogoId)
        {
            List<HeaderExtrato> linhasExtrato = new List<HeaderExtrato>();
            using (SqlConnection cn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
            {
                SqlCommand cmd = new SqlCommand("JP_PRC_ExtratoPorTipoApoio", cn);
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@AccountNumber", Value = accountNumber, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@StatusPoints", Value = tipoPonto, SqlDbType = SqlDbType.Char, Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@MktPlaceCatalogoId", Value = mktPlaceCatalogoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });

                cmd.CommandTimeout = 2400; //40 minutos
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                linhasExtrato = new List<HeaderExtrato>();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        HeaderExtrato headerExtrato = new HeaderExtrato();
                        headerExtrato.Id = Convert.ToInt32(reader["Id"]);

                        headerExtrato.CampaignNumber = Convert.ToInt32(reader["CampaignNumberHeader"]);
                        headerExtrato.CampaignYearNumber = Convert.ToInt32(reader["CampaignYearNumberHeader"]);
                        headerExtrato.ReferralRepresentativeNumber = reader["ReferralRepresentativeNumberHeader"].ToString();
                        headerExtrato.ReferralName = reader["ReferralNameHeader"].ToString().Trim();
                        headerExtrato.PendingPayment = reader["PendingPaymentHeader"].ToString();
                        headerExtrato.RepresentativeStatus = reader["RepresentativeStatusHeader"].ToString();
                        headerExtrato.Points = Convert.ToInt32(reader["Points"]);
                        headerExtrato.TransactionDate = Convert.ToDateTime(reader["TransactionDateHeader"]);
                        headerExtrato.ReferredRepresentativeNumber = reader["ReferredRepresentativeNumberHeader"].ToString();
                        headerExtrato.ReferredNameDetail = reader["ReferredNameDetail"].ToString().Trim();
                        headerExtrato.StatusPoints = reader["StatusPoints"].ToString();
                        headerExtrato.TypePoints = reader["TypePoints"].ToString();
                        headerExtrato.ProgramType = Convert.ToInt32(reader["ProgramType"]);

                        headerExtrato.ProgramDescription = reader["IncentiveProgramDescriptionHeader"].ToString();

                        linhasExtrato.Add(headerExtrato);
                    }
                }

                cn.Close();
            }

            return linhasExtrato;

        }

        public List<HeaderExtrato> ObterHeaderPorTipoIndicacao(int accountNumber, string tipoPonto, int mktPlaceCatalogoId)
        {
            List<HeaderExtrato> linhasExtrato = new List<HeaderExtrato>();
            using (SqlConnection cn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
            {
                SqlCommand cmd = new SqlCommand("JP_PRC_ExtratoPorTipoIndicacao", cn);
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@AccountNumber", Value = accountNumber, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@StatusPoints", Value = tipoPonto, SqlDbType = SqlDbType.Char, Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@MktPlaceCatalogoId", Value = mktPlaceCatalogoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });

                cmd.CommandTimeout = 2400; //40 minutos
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                linhasExtrato = new List<HeaderExtrato>();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        HeaderExtrato headerExtrato = new HeaderExtrato();
                        headerExtrato.Id = Convert.ToInt32(reader["Id"]);

                        headerExtrato.CampaignNumber = Convert.ToInt32(reader["CampaignNumberHeader"]);
                        headerExtrato.CampaignYearNumber = Convert.ToInt32(reader["CampaignYearNumberHeader"]);
                        headerExtrato.ReferralRepresentativeNumber = reader["ReferralRepresentativeNumberHeader"].ToString();
                        headerExtrato.ReferralName = reader["ReferralNameHeader"].ToString().Trim();
                        headerExtrato.PendingPayment = reader["PendingPaymentHeader"].ToString();
                        headerExtrato.RepresentativeStatus = reader["RepresentativeStatusHeader"].ToString();
                        headerExtrato.Points = Convert.ToInt32(reader["Points"]);
                        headerExtrato.TransactionDate = Convert.ToDateTime(reader["TransactionDateHeader"]);
                        headerExtrato.ReferredRepresentativeNumber = reader["ReferredRepresentativeNumberHeader"].ToString();
                        headerExtrato.ReferredNameDetail = reader["ReferredNameDetail"].ToString().Trim();
                        headerExtrato.StatusPoints = reader["StatusPoints"].ToString();
                        headerExtrato.TypePoints = reader["TypePoints"].ToString();
                        headerExtrato.ProgramType = Convert.ToInt32(reader["ProgramType"]);

                        headerExtrato.ProgramDescription = reader["IncentiveProgramDescriptionHeader"].ToString();

                        linhasExtrato.Add(headerExtrato);
                    }
                }

                cn.Close();
            }

            return linhasExtrato;

        }

        public List<HeaderExtrato> ObterHeaderPorTipoClubeDasEstrelas(int accountNumber, string tipoPonto)
        {
            using (var cn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
            {
                var cmd = new SqlCommand("JP_PRC_ExtratoPorTipoClubeEstrelas", cn);
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@AccountNumber", Value = accountNumber, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@StatusPoints", Value = tipoPonto, SqlDbType = SqlDbType.Char, Direction = ParameterDirection.Input });

                cmd.CommandTimeout = 180; //3 minutos
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                var linhasExtrato = new List<HeaderExtrato>();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime? TransctionDatetime = new DateTime();
                        if (reader["TransactionDateHeader"].ToString() != "")
                            TransctionDatetime = Convert.ToDateTime(reader["TransactionDateHeader"]);
                        //string _programDescription = GetClubeEstrelasProgramDescription(reader["Description"].ToString());

                        var headerExtrato = new HeaderExtrato
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            ProgramDescription = reader["Description"].ToString(),
                            CampaignNumber = Convert.ToInt32(reader["CampaignNumberHeader"]),
                            CampaignYearNumber = Convert.ToInt32(reader["CampaignYearNumberHeader"]),
                            Points = Convert.ToInt32(reader["Points"]),
                            TransactionDate = TransctionDatetime,
                            OrderId = reader["Description"].ToString() == "Pedido Faturado" ? reader["OrderId"].ToString() : "N/A",
                            TotalSalesDetail = Convert.ToDecimal(reader["PaymentAmount"]),
                            AtingiuTurbo = (reader["MMAGoalMeet"].ToString() == "Y"),
                            PercentualBonus = reader["MMAPercentageBonus"].ToString(),
                            StatusPoints = tipoPonto,
                            TypePoints = "ED"
                        };

                        linhasExtrato.Add(headerExtrato);
                    }

                    reader.Close();
                }

                //Agrupa para exibição no Header
                var headerExtratoGroup = linhasExtrato.GroupBy(_ => new { _.Id, _.ProgramDescription })
                                    .Select(_ => new HeaderExtrato
                                    {
                                        Id = _.Key.Id,
                                        ProgramDescription = _.Key.ProgramDescription,
                                        CampaignNumber = _.First().CampaignNumber,
                                        CampaignYearNumber = _.First().CampaignYearNumber,
                                        Points = _.Sum(h => h.Points),
                                        StatusPoints = tipoPonto,
                                        TypePoints = "EH"
                                    }).ToList();

                linhasExtrato.AddRange(headerExtratoGroup);

                return linhasExtrato;
            }
        }

        public List<HeaderExtrato> ObterHeaderPorTipoMigracao(int accountNumber, string tipoPonto)
        {
            try
            {
                using (var cn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
                {
                    var cmd = new SqlCommand("JP_PRC_ExtratoPorTipoMigracao", cn);
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@AccountNumber", Value = accountNumber, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@StatusPoints", Value = tipoPonto, SqlDbType = SqlDbType.Char, Direction = ParameterDirection.Input });

                    cmd.CommandTimeout = 180; //3 minutos
                    cmd.CommandType = CommandType.StoredProcedure;
                    cn.Open();

                    var linhasExtrato = new List<HeaderExtrato>();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime.TryParse(reader["TransactionDate"]?.ToString(), out DateTime transactionDate);

                            var headerExtrato = new HeaderExtrato
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                ProgramDescription = reader["ProgramDescription"].ToString(),
                                CampaignNumber = Convert.ToInt32(reader["CampaignNumber"]),
                                CampaignYearNumber = Convert.ToInt32(reader["CampaignYear"]),
                                EndCampaignNumber = Convert.ToInt32(reader["EndCampaignNumber"]),
                                EndCampaignYearNumber = Convert.ToInt32(reader["EndCampaignYear"]),
                                ReferralRepresentativeNumber = reader["ReferralRepresentativeNumber"].ToString(),
                                Points = Convert.ToInt32(reader["Points"]),
                                TotalSalesDetail = Convert.ToDecimal(reader["PaymentAmount"]),
                                TypePoints = "MH",
                                TransactionDate = transactionDate,
                                StatusPoints = tipoPonto,
                            };

                            linhasExtrato.Add(headerExtrato);
                        }

                        reader.Close();
                    }

                    return linhasExtrato;
                }

            }
            catch (Exception ex)
            {
                GravarLogErro(ex.Message, "Erro ao executar a proc JP_PRC_ExtratoPorTipoMigracao", "ExtratoService", "ObterHeaderPorTipoMigracao", "jobCatalog");
                throw new ApplicationException("Erro ao executar a proc JP_PRC_ExtratoPorTipoMigracao", ex);
            }
        }

        private static string GetClubeEstrelasProgramDescription(string programDescription)
        {
            using (var context = UnitOfWorkFactory.Create())
            {
                var rep = context.CreateRepository<ClubeEstrelasProgramDescription>();
                var itemDescription = rep.Filter<ClubeEstrelasProgramDescription>(_ => _.Register2ExtratoDescription == programDescription);
                var list = rep.All<ClubeEstrelasProgramDescription>();

                string description;

                if (itemDescription == null || !itemDescription.Any())
                    description = "Pedido Faturado";
                else
                    description = itemDescription.First().DescricaoExtrato;

                return description;
            }
        }

        public List<DetailExtrato> ObterDetails(int headerId, string tipoPonto)
        {
            using (SqlConnection cn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
            {
                SqlCommand cmd = new SqlCommand("JP_PRC_DetailExtrato", cn);
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@HeaderId", Value = headerId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@StatusPoints", Value = tipoPonto, SqlDbType = SqlDbType.Char, Direction = ParameterDirection.Input });
                cmd.CommandTimeout = 2400; //40 minutos
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                List<DetailExtrato> detailExtratos = new List<DetailExtrato>();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DetailExtrato detailExtrato = new DetailExtrato();

                        detailExtrato.Id = Convert.ToInt32(reader["Id"]);
                        detailExtrato.TypePoints = reader["TypePoints"].ToString();

                        switch (tipoPonto)
                        {
                            case "I":

                                //Indicação
                                detailExtrato.ReferredRepresentativeNumberDetail = reader["ReferredRepresentativeNumberDetail"].ToString();
                                detailExtrato.ReferredAppointmentCampaignNumberDetail = Convert.ToInt32(reader["ReferredAppointmentCampaignNumberDetail"]);
                                detailExtrato.ReferredAppointmentCampaignYearNumberDetail = Convert.ToInt32(reader["ReferredAppointmentCampaignYearNumberDetail"]);
                                detailExtrato.ReferralRepresentativeNumberDetail = reader["ReferralRepresentativeNumberDetail"].ToString();
                                detailExtrato.ReferenceCampaignNumberDetail = Convert.ToInt32(reader["ReferenceCampaignNumberDetail"]);
                                detailExtrato.ReferenceCampaignYearNumberDetail = Convert.ToInt32(reader["ReferenceCampaignYearNumberDetail"]);
                                detailExtrato.SentOrderCodeDetail = reader["SentOrderCodeDetail"].ToString();
                                detailExtrato.OrderPaymentCodeDetail = reader["OrderPaymentCodeDetail"].ToString();
                                detailExtrato.ReturnedOrderCodeDetail = reader["ReturnedOrderCodeDetail"].ToString();
                                detailExtrato.ReferredNameDetail = reader["ReferredNameDetail"].ToString();
                                detailExtrato.OrderCampaignNumberDetail = Convert.ToInt32(reader["OrderCampaignNumberDetail"]);
                                break;

                            case "C":

                                //Consecutividade
                                detailExtrato.ReferenceCampaignNumberDetail = Convert.ToInt32(reader["CampaignNumberDetail"]);
                                detailExtrato.ReferenceCampaignYearNumberDetail = Convert.ToInt32(reader["CampaignYearDetail"]);
                                detailExtrato.SentOrderCodeDetail = reader["OrderSentCodeDetail"].ToString();
                                detailExtrato.OrderPaymentCodeDetail = reader["OrderPaymentStatusDetail"].ToString();
                                detailExtrato.ReturnedOrderCodeDetail = reader["OrderObjectiveStatusDetail"].ToString();
                                break;
                            case "E":
                                //Clube das estrelas
                                detailExtrato.ReferenceCampaignNumberDetail = Convert.ToInt32(reader["CampaignNumber"]);
                                detailExtrato.ReferenceCampaignYearNumberDetail = Convert.ToInt32(reader["CampaignYear"]);
                                detailExtrato.MktPlaceCatalogoId = Convert.ToInt32(reader["MktPlaceCatalogoId"]);
                                detailExtrato.OrderId = reader["OrderId"].ToString();
                                detailExtrato.TotalSalesDetail = Convert.ToDecimal(reader["PaymentAmount"]);
                                detailExtrato.AtingiuTurbo = (reader["MMAGoalMeet"].ToString() == "Y");
                                detailExtrato.Descricao = reader["Description"].ToString();
                                detailExtrato.PercentualBonus = reader["MMAPercentageBonus"].ToString();
                                detailExtrato.Bonus = detailExtrato.Descricao.Contains("BONUS");
                                detailExtrato.TransactionCreationDate = Convert.ToDateTime(reader["TransactionCreationDate"]);
                                detailExtrato.CanceledPoints = Convert.ToInt32(reader["CanceledPoints"]);
                                detailExtrato.PendingPoints = Convert.ToInt32(reader["PendingPoints"]);
                                detailExtrato.AvailablePoints = Convert.ToInt32(reader["AvailablePoints"]);
                                break;

                            default:

                                //Apoio e Atividade
                                detailExtrato.ReferenceCampaignNumberDetail = Convert.ToInt32(reader["CampaignNumber"]);
                                detailExtrato.ReferenceCampaignYearNumberDetail = Convert.ToInt32(reader["CampaignYear"]);

                                detailExtrato.TotalSalesDetail = Convert.ToDecimal(reader["TotalSalesDetail"]);
                                detailExtrato.TotalReturnAmountDetail = Convert.ToDecimal(reader["TotalReturnAmountDetail"]);
                                detailExtrato.ProgramType = reader["ProgramType"].ToString();

                                break;
                        }

                        detailExtratos.Add(detailExtrato);
                    }
                    return detailExtratos;
                }
            }
        }

        public List<MapaLoja> ObterMapaLoja(string rede)
        {
            using (SqlConnection cn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
            {
                SqlCommand cmd = new SqlCommand("JP_PRC_ListarMapaLojas", cn);
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@Rede", Value = rede, SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input });

                cmd.CommandTimeout = 2400; //40 minutos
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                List<MapaLoja> lojas = new List<MapaLoja>();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MapaLoja mapa = new MapaLoja();
                        mapa.Id = Convert.ToInt32(reader["Id"]);
                        mapa.ReferenciaLoja = reader["ReferenciaLoja"].ToString();
                        mapa.Rede = reader["Rede"].ToString();
                        mapa.NomeLoja = reader["NomeLoja"].ToString();
                        mapa.Endereco = reader["Endereco"].ToString();
                        mapa.CEP = reader["CEP"].ToString();
                        mapa.Estado = reader["Estado"].ToString();
                        mapa.Municipio = reader["Municipio"].ToString();
                        mapa.Bairro = reader["Bairro"].ToString();
                        mapa.Pais = reader["Pais"].ToString();
                        mapa.Latitude = reader["Latitude"].ToString();
                        mapa.Longitude = reader["Longitude"].ToString();
                        mapa.ExibePortal = Convert.ToBoolean(reader["ExibePortal"]);
                        mapa.DataInclusao = Convert.ToDateTime(reader["DataInclusao"]);

                        lojas.Add(mapa);
                    }
                }
                return lojas;
            }
        }

        public bool ValidarBannerDirecionado(string ra, string chaveCampanha)
        {
            using (SqlConnection cn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
            {
                SqlCommand cmd = new SqlCommand("JP_PRC_ListarParticipanteCampanhaBanner", cn);
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@ra", Value = ra, SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@ChaveCampanha", Value = chaveCampanha, SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input });

                cmd.CommandTimeout = 2400; //40 minutos
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                List<MapaLoja> Participantes = new List<MapaLoja>();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var Id = Convert.ToInt32(reader["Id"]);
                        return true;
                    }
                }
                return false;
            }
        }

        public List<MapaLoja> ObterMapaLojasPorParticipante(string ra)
        {
            using (SqlConnection cn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
            {
                SqlCommand cmd = new SqlCommand("JP_PRC_ListarMapaLojasPorParticipante", cn);
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@Ra", Value = ra, SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input });

                cmd.CommandTimeout = 2400; //40 minutos
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                List<MapaLoja> Participantes = new List<MapaLoja>();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MapaLoja participanteMapa = new MapaLoja();
                        participanteMapa.Id = Convert.ToInt32(reader["Id"]);
                        participanteMapa.Rede = reader["Rede"].ToString();
                        participanteMapa.Ra = reader["Ra"].ToString();
                        participanteMapa.Estado = reader["Estado"].ToString();
                        participanteMapa.Municipio = reader["Municipio"].ToString();
                        participanteMapa.Latitude = reader["Latitude"].ToString();
                        participanteMapa.Longitude = reader["Longitude"].ToString();
                        participanteMapa.DataInclusao = Convert.ToDateTime(reader["DataInclusao"]);

                        Participantes.Add(participanteMapa);
                    }
                }
                return Participantes;
            }
        }

        #region ProcessoExtrato

        public List<MigracaoExtratoExterno> ObterExtratoMigracaoExterno(int accountNumber, string Top2ProgramName, int cmpgnNr, int cmpgnYr)
        {
            List<MigracaoExtratoExterno> extratoExterno = new List<MigracaoExtratoExterno>();
            List<MigracaoExtratoDB> extratoDB = new List<MigracaoExtratoDB>();
            extratoDB = ObterExtratoMigracao(accountNumber, Top2ProgramName, cmpgnNr, cmpgnYr);

            foreach (string Programa in extratoDB.Select(x => x.ProgramName).Distinct())
            {
                MigracaoExtratoDB itemExtratoDb = new MigracaoExtratoDB();
                itemExtratoDb = extratoDB.FirstOrDefault(x => x.ProgramName == Programa);

                MigracaoExtratoExterno itemExtratoExterno = new MigracaoExtratoExterno();
                itemExtratoExterno.Programa = itemExtratoDb.ProgramName;
                itemExtratoExterno.TotalVendasPagas = itemExtratoDb.TotalPeriodPay / 100;
                itemExtratoExterno.TotalVendasPendentes = itemExtratoDb.TotalPeriodPendingPay / 100;
                itemExtratoExterno.TotalVendas = itemExtratoDb.TotalPaymentAmount / 100;
                itemExtratoExterno.TotalPontosDisponiveis = itemExtratoDb.TotalAvailablePoints;
                itemExtratoExterno.PontosDisponiveisDoDia = itemExtratoDb.TotalAvailablePointsDay;
                itemExtratoExterno.PontosPendentes = itemExtratoDb.TotalPendingPoints;
                itemExtratoExterno.PontosTotais = itemExtratoDb.TotalPoints;

                itemExtratoExterno.campanhas = new List<MigracaoExtratoCampanhas>();
                foreach (MigracaoExtratoDB extratoDbCampanhas in extratoDB.FindAll(x => x.ProgramName == Programa).ToList())
                {
                    MigracaoExtratoCampanhas campanhaItem = new MigracaoExtratoCampanhas();
                    campanhaItem.Campanha = extratoDbCampanhas.CampaignNumber + "/" + extratoDbCampanhas.CampaignYear;
                    campanhaItem.AD = extratoDbCampanhas.OrderID;
                    campanhaItem.ValorVendasFaturadoBoleto = extratoDbCampanhas.Description == "FATURAMENTO" || extratoDbCampanhas.Description == "PAGAMENTO" ? extratoDbCampanhas.PaymentAmount / 100 : 0;
                    campanhaItem.ValorVendasConecta = extratoDbCampanhas.Description == "CONECTA" ? extratoDbCampanhas.PaymentAmount / 100 : 0;
                    campanhaItem.ValorVendasProdutosFaltantes = extratoDbCampanhas.PaymentShort / 100;
                    campanhaItem.ValorVendasDeDevolucao = extratoDbCampanhas.PaymentDevolution / 100;
                    campanhaItem.ValorTotalDeVendas = extratoDbCampanhas.TotalAmount / 100;
                    campanhaItem.VendasPagas = extratoDbCampanhas.OrderPay;

                    itemExtratoExterno.campanhas.Add(campanhaItem);
                }
                extratoExterno.Add(itemExtratoExterno);
            }
            return extratoExterno;
        }

        public List<MigracaoExtratoDB> ObterExtratoMigracao(int accountNumber, string Top2ProgramName, int cmpgnNr, int cmpgnYr)
        {
            List<MigracaoExtratoDB> linhasExtrato = new List<MigracaoExtratoDB>();
            using (SqlConnection cn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
            {
                SqlCommand cmd = new SqlCommand("JP_PRC_ExtratoMigracaoPorRA", cn);
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@AccountNumber", Value = accountNumber, SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@Top2ProgramName", Value = Top2ProgramName, SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@cmpgnNr", Value = cmpgnNr, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@cmpgnYr", Value = cmpgnYr, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });

                cmd.CommandTimeout = 2400;
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MigracaoExtratoDB extratoDb = new MigracaoExtratoDB();
                        extratoDb.Id = Convert.ToInt32(reader["Id"]);

                        extratoDb.RepresentativeAccountNumber = Convert.ToInt32(reader["RepresentativeAccountNumber"]);
                        extratoDb.ProgramName = reader["ProgramName"].ToString();
                        extratoDb.TotalPeriodPay = Convert.ToDecimal(reader["TotalPeriodPay"]);
                        extratoDb.TotalPeriodPendingPay = Convert.ToDecimal(reader["TotalPeriodPendingPay"]);
                        extratoDb.TotalPaymentAmount = Convert.ToDecimal(reader["TotalPaymentAmount"]);
                        extratoDb.TotalAvailablePoints = Convert.ToInt32(reader["TotalAvailablePoints"]);
                        extratoDb.TotalAvailablePointsDay = Convert.ToInt32(reader["TotalAvailablePointsDay"]);
                        extratoDb.TotalPendingPoints = Convert.ToInt32(reader["TotalPendingPoints"]);
                        extratoDb.TotalPoints = Convert.ToInt32(reader["TotalPoints"]);
                        extratoDb.MigracaoRegister2ExtratoId = Convert.ToInt32(reader["MigracaoRegister2ExtratoId"]);
                        extratoDb.CampaignNumber = Convert.ToInt32(reader["CampaignNumber"]);
                        extratoDb.CampaignYear = Convert.ToInt32(reader["CampaignYear"]);
                        extratoDb.OrderID = Convert.ToInt64(reader["OrderID"]);
                        extratoDb.Description = reader["Description"].ToString();
                        extratoDb.PaymentAmount = Convert.ToDecimal(reader["PaymentAmount"]);
                        extratoDb.PaymentShort = Convert.ToDecimal(reader["PaymentShort"]);
                        extratoDb.PaymentDevolution = Convert.ToDecimal(reader["PaymentDevolution"]);
                        extratoDb.TotalAmount = Convert.ToDecimal(reader["TotalAmount"]);
                        extratoDb.OrderPay = Convert.ToBoolean(reader["OrderPay"]);
                        linhasExtrato.Add(extratoDb);
                    }
                }

                cn.Close();
            }

            return linhasExtrato;
        }


        public NatalExtratoExterno ObterExtratoNatalExterno(string accountNumber)
        {
            var extratoExterno = ObterCacheExtratoNatal(accountNumber);

            if (extratoExterno != null)
                return extratoExterno;

            var extratoDB = ObterExtratoNatal(accountNumber);

            if (extratoDB is null || !extratoDB.Any())
                return new NatalExtratoExterno();

            var itemExtratoDb = extratoDB.FirstOrDefault();

            extratoExterno = new NatalExtratoExterno
            {
                NivelRepresentante = itemExtratoDb.RepresentativeLevel,
                NomeRepresentante = itemExtratoDb.RepresentativeName,
                NivelPremiacao = itemExtratoDb.AwardLevel,
                Premiacao = itemExtratoDb.Award,
                ValorTotal = itemExtratoDb.TotalSalePresents,
                campanhas = new List<NatalExtratoCampanhas>()
            };

            foreach (var extratoDbCampanhas in extratoDB)
            {
                var campanhaItem = new NatalExtratoCampanhas
                {
                    Campanha = extratoDbCampanhas.CP,
                    ValorVendasPresenteNatal = extratoDbCampanhas.TotalSaleCp,
                    PedidoPago = extratoDbCampanhas.PaymentPendingCp
                };

                extratoExterno.campanhas.Add(campanhaItem);
            }

            GravarCacheExtratoNatal(accountNumber, extratoExterno);

            return extratoExterno;
        }

        public List<NatalExtratoDB> ObterExtratoNatal(string accountNumber)
        {
            List<NatalExtratoDB> linhasExtrato = new List<NatalExtratoDB>();
            using (SqlConnection cn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
            {
                SqlCommand cmd = new SqlCommand("JP_PRC_NatalExtrato", cn);
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@AccountNumber", Value = accountNumber, SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input });

                cmd.CommandTimeout = 2400;
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        NatalExtratoDB extratoDb = new NatalExtratoDB();
                        extratoDb.Id = Convert.ToInt32(reader["Id"]);
                        extratoDb.RepresentativeLevel = reader["RepresentativeLevel"].ToString();
                        extratoDb.AwardLevel = Convert.ToInt32(reader["AwardLevel"]);
                        extratoDb.Award = reader["Award"].ToString();
                        extratoDb.CP = reader["CP"].ToString();
                        extratoDb.TotalSaleCp = Convert.ToDecimal(reader["TotalSaleCp"]);
                        extratoDb.PaymentPendingCp = Convert.ToBoolean(reader["PaymentPendingCp"]);
                        extratoDb.TotalSalePresents = Convert.ToDecimal(reader["TotalSalePresents"]);
                        extratoDb.RepresentativeName = reader["RepresentativeName"].ToString();

                        linhasExtrato.Add(extratoDb);
                    }
                }

                cn.Close();
            }

            return linhasExtrato;
        }


        private void GravarCacheExtratoNatal(string accountNumber, NatalExtratoExterno extrato)
        {
            CacheAttribute cacheAttribute = new CacheAttribute();
            cacheAttribute.StoreObjectExtrato("CacheExtratoNatal_" + accountNumber, extrato, "CacheExtrato");
        }

        private NatalExtratoExterno ObterCacheExtratoNatal(string accountNumber)
        {
            CacheAttribute cacheAttribute = new CacheAttribute();
            return cacheAttribute.GetHashObjectExtrato<NatalExtratoExterno>("CacheExtratoNatal_" + accountNumber);
        }


        public AliancaExtratoExterno ObterExtratoAliancaExterno(string accountNumber, int cmpgnNr, int cmpgnYr)
        {
            var extratoExterno = ObterCacheExtratoAlianca(accountNumber);

            if (extratoExterno != null)
                return extratoExterno;

            var extratoDB = ObterExtratoAlianca(accountNumber);

            if (extratoDB is null || !extratoDB.Any())
                return null;

            if (cmpgnNr > 0 || cmpgnYr > 0)
            {
                extratoDB = extratoDB.Where(x => x.CpPrimeiroPedido >= cmpgnNr && x.AnoCpPrimeiroPedido >= cmpgnYr).ToList();
            }

            var itemExtratoDb = extratoDB.FirstOrDefault();

            extratoExterno = new AliancaExtratoExterno
            {
                RegistroEmpresaria = itemExtratoDb.EmpresariaAccountNumber,
                Madrinhas = new List<AliancaExtratoMadrinhas>()
            };

            foreach (int DetailId in extratoDB.Select(x => x.Id).Distinct())
            {
                AliancaExtratoDB extratoDbMadrinhas = new AliancaExtratoDB();
                extratoDbMadrinhas = extratoDB.FirstOrDefault(x => x.Id == DetailId);

                AliancaExtratoMadrinhas madrinhaItem = new AliancaExtratoMadrinhas();

                madrinhaItem.RegistroMadrinha = extratoDbMadrinhas.MadrinhaAccountNumber;
                madrinhaItem.NomeMadrinha = extratoDbMadrinhas.MadrinhaNome;
                madrinhaItem.NomeNovaRepresentante = extratoDbMadrinhas.RepresentativeName;
                madrinhaItem.Campanhas = new List<AliancaExtratoCP>();

                foreach (AliancaExtratoDB extratoDbCampanhas in extratoDB.FindAll(x => x.Id == DetailId).ToList())
                {
                    var campanhaItem = new AliancaExtratoCP
                    {
                        CpPrimeiroPedido = extratoDbCampanhas.CpPrimeiroPedido + "/" + extratoDbCampanhas.AnoCpPrimeiroPedido,
                        EviouPagouPrimeiroPedido = extratoDbCampanhas.PrimeiroPedidoPago,
                        PontosConquistadosPrimeiroPedido = extratoDbCampanhas.PontosConquistadosPrimeiroPedido,
                        AtingiuVendas = extratoDbCampanhas.AtingiuVendas,
                        PontosConquistados = extratoDbCampanhas.PontosConquistados,
                        PontosConquistadosEmpresaria = extratoDbCampanhas.PontosConquistadosEmpresaria,
                        PontosConquistadosEquipe = extratoDbCampanhas.PontosConquistadosEquipe
                    };

                    madrinhaItem.Campanhas.Add(campanhaItem);

                }

                extratoExterno.Madrinhas.Add(madrinhaItem);
            }

            GravarCacheExtratoAlianca(accountNumber, extratoExterno);

            return extratoExterno;
        }


        public List<AliancaExtratoDB> ObterExtratoAlianca(string accountNumber)
        {
            List<AliancaExtratoDB> linhasExtrato = new List<AliancaExtratoDB>();
            using (SqlConnection cn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
            {
                SqlCommand cmd = new SqlCommand("JP_PRC_AliancaExtrato", cn);
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@AccountNumber", Value = accountNumber, SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input });

                cmd.CommandTimeout = 2400;
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        AliancaExtratoDB extratoDb = new AliancaExtratoDB();
                        extratoDb.Id = Convert.ToInt32(reader["Id"]);
                        extratoDb.EmpresariaAccountNumber = reader["EmpresariaAccountNumber"].ToString();
                        extratoDb.MadrinhaAccountNumber = reader["MadrinhaAccountNumber"].ToString();
                        extratoDb.MadrinhaNome = reader["MadrinhaNome"].ToString();
                        extratoDb.RepresentativeName = reader["RepresentativeName"].ToString();
                        extratoDb.CpPrimeiroPedido = Convert.ToInt32(reader["CpPrimeiroPedido"]);
                        extratoDb.AnoCpPrimeiroPedido = Convert.ToInt32(reader["AnoCpPrimeiroPedido"]);
                        extratoDb.PrimeiroPedidoPago = Convert.ToBoolean(reader["PrimeiroPedidoPago"]);
                        extratoDb.PontosConquistadosPrimeiroPedido = Convert.ToDecimal(reader["PontosConquistadosPrimeiroPedido"]);
                        extratoDb.AtingiuVendas = Convert.ToBoolean(reader["AtingiuVendas"]);
                        extratoDb.PontosConquistados = Convert.ToDecimal(reader["PontosConquistados"]);
                        extratoDb.PontosConquistadosEmpresaria = Convert.ToDecimal(reader["PontosConquistadosEmpresaria"]);
                        extratoDb.PontosConquistadosEquipe = Convert.ToDecimal(reader["PontosConquistadosEquipe"]);

                        linhasExtrato.Add(extratoDb);
                    }
                }

                cn.Close();
            }

            return linhasExtrato;
        }

        private void GravarCacheExtratoAlianca(string accountNumber, AliancaExtratoExterno extrato)
        {
            CacheAttribute cacheAttribute = new CacheAttribute();
            cacheAttribute.StoreObjectExtrato("CacheExtratoAlianca_" + accountNumber, extrato, "CacheExtrato");
        }

        private AliancaExtratoExterno ObterCacheExtratoAlianca(string accountNumber)
        {
            CacheAttribute cacheAttribute = new CacheAttribute();
            return cacheAttribute.GetHashObjectExtrato<AliancaExtratoExterno>("CacheExtratoAlianca_" + accountNumber);
        }


        public List<ConsecutividadeExtratoExterno> ObterExtratoConsecutividadeExterno(int accountNumber, string programDescription)
        {
            programDescription = string.IsNullOrEmpty(programDescription) ? "" : programDescription;
            List<ConsecutividadeExtratoExterno> extratoExterno = new List<ConsecutividadeExtratoExterno>();
            List<ConsecutividadeExtratoDB> extratoDB = new List<ConsecutividadeExtratoDB>();
            extratoDB = ObterExtratoConsecutividade(accountNumber, programDescription);

            foreach (int idHeader in extratoDB.Select(x => x.Id).Distinct())
            {
                ConsecutividadeExtratoDB itemExtratoDb = new ConsecutividadeExtratoDB();
                itemExtratoDb = extratoDB.FirstOrDefault(x => x.Id == idHeader);

                ConsecutividadeExtratoExterno itemExtratoExterno = new ConsecutividadeExtratoExterno
                {
                    programa = itemExtratoDb.ProgramDescriptionHeader,
                    pontos_consecutividade = itemExtratoDb.Points,
                    pontos_liberados = itemExtratoDb.StatusPoints == "S" ? itemExtratoDb.Points : 0,
                    pontos_cancelados = itemExtratoDb.StatusPoints == "C" ? itemExtratoDb.Points : 0,
                    pontos_pendentes = itemExtratoDb.StatusPoints == "N" ? itemExtratoDb.Points : 0,
                    campanhas = new List<ConsecutividadeExtratoCampanhas>()
                };

                foreach (ConsecutividadeExtratoDB extratoDbCampanhas in extratoDB.FindAll(x => x.Id == idHeader).ToList())
                {
                    ConsecutividadeExtratoCampanhas campanhaItem = new ConsecutividadeExtratoCampanhas
                    {
                        ano = extratoDbCampanhas.CampaignYearDetail,
                        cp = extratoDbCampanhas.CampaignNumberDetail,
                        atingiu_objetivo = extratoDbCampanhas.OrderObjectiveStatusDetail,
                        devolucao = extratoDbCampanhas.OrderDevolutionStatusDetail,
                        pedido_pago = extratoDbCampanhas.OrderPaymentStatusDetail,
                        pedido_enviado = extratoDbCampanhas.OrderSentCodeDetail
                    };

                    itemExtratoExterno.campanhas.Add(campanhaItem);
                }
                extratoExterno.Add(itemExtratoExterno);
            }
            return extratoExterno;
        }

        public List<ConsecutividadeExtratoDB> ObterExtratoConsecutividade(int accountNumber, string programDescription)
        {
            List<ConsecutividadeExtratoDB> linhasExtrato = new List<ConsecutividadeExtratoDB>();
            using (SqlConnection cn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
            {
                SqlCommand cmd = new SqlCommand("JP_PRC_ExtratoConsecutividadePorRA", cn);
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@AccountNumber", Value = accountNumber, SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@ProgramDescription", Value = programDescription, SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input });

                cmd.CommandTimeout = 2400;
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ConsecutividadeExtratoDB extratoDb = new ConsecutividadeExtratoDB();
                        extratoDb.Id = Convert.ToInt32(reader["Id"]);

                        extratoDb.ProgramDescriptionHeader = reader["ProgramDescriptionHeader"].ToString();
                        extratoDb.RepresentativeNumberHeader = reader["RepresentativeNumberHeader"].ToString();
                        extratoDb.Points = Convert.ToInt32(reader["Points"]);
                        extratoDb.StatusPoints = reader["StatusPoints"].ToString();
                        extratoDb.ConsecutividadeDetailId = Convert.ToInt32(reader["ConsecutividadeDetailId"]);
                        extratoDb.CampaignYearDetail = Convert.ToInt32(reader["CampaignYearDetail"]);
                        extratoDb.CampaignNumberDetail = Convert.ToInt32(reader["CampaignNumberDetail"]);
                        extratoDb.OrderSentCodeDetail = reader["OrderSentCodeDetail"].ToString();
                        extratoDb.OrderPaymentStatusDetail = reader["OrderPaymentStatusDetail"].ToString();
                        extratoDb.OrderObjectiveStatusDetail = reader["OrderObjectiveStatusDetail"].ToString();
                        extratoDb.OrderDevolutionStatusDetail = reader["OrderDevolutionStatusDetail"].ToString();

                        linhasExtrato.Add(extratoDb);
                    }
                }

                cn.Close();
            }

            return linhasExtrato;
        }

        public async Task<List<HomeMMAExtratoExterno>> ObterHomeMMAExtratoExterno(int accountNumber, decimal? saldo, int ExpiracaoSaldoPontos, int MktPlaceCatalogoId, string accessTokenMktPlace, int mktPlaceParticipantId)//, decimal? saldo, int ExpiracaoSaldoPontos,
        {
            var extratoExterno = new List<HomeMMAExtratoExterno>();

            var extratoPontosCNC = ObterTotalizado(accountNumber, MktPlaceCatalogoId);

            //var PontosResgatadosEstornados = await ObterSaldoEstornado(accountNumber, MktPlaceCatalogoId, accessTokenMktPlace, mktPlaceParticipantId, impersonateUser);

            //var PontosResgatados = (PontosResgatadosEstornados.DebitCurrencyBalance + PontosResgatadosEstornados.RedeemBalance) * -1;

            //var PontosExpirados = await ObterPontosExpiradoExterno(accountNumber, MktPlaceCatalogoId, accessTokenMktPlace, mktPlaceParticipantId, impersonateUser);

            var itemExtratoExterno = new HomeMMAExtratoExterno
            {
                atualizacao = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")),
                saldo = saldo,
                itens = new List<HomeMMAExtratoItens>()
            };

            var ExtratoItem = new HomeMMAExtratoItens
            {
                PontosDisponíveis = saldo,
                PontosAvencer = ExpiracaoSaldoPontos,
                PontosAreceber = extratoPontosCNC.PontosPendentes,
                //PontosVencidos = PontosExpirados.Count() > 0 ? PontosExpirados.Sum(x => x.points) * -1 : 0,
                PontosVencidos = 0,
                //PontosResgatados = PontosResgatados
                PontosResgatados = 0
            };

            itemExtratoExterno.itens.Add(ExtratoItem);

            extratoExterno.Add(itemExtratoExterno);

            return extratoExterno;
        }

        public PontosExtrato ObterPontosPExtratoCabecalhoExterno(List<PontosExtratoItens> ExtratoPontosN, int accountNumber, int mktPlaceCatalogoId, int recordSetTotal)
        {
            try
            {

                var ExtratoItem = new PontosExtrato();
                var PontosCNC = ObterTotalizado(accountNumber, mktPlaceCatalogoId);

                if (ExtratoPontosN != null)
                {
                    ExtratoItem.valueInPoints = PontosCNC.PontosPendentes;
                    ExtratoItem.name = "Pontos a Receber";
                    ExtratoItem.recordSetTotal = recordSetTotal;
                    ExtratoItem.recordSetCount = ExtratoPontosN.Count();
                }
                return ExtratoItem;
            }
            catch (Exception ex)
            {
                GravarLogErro(ex.Message, "Erro ao ObterPontosPExtratoCabecalhoExterno", "ExtratoService", "ObterPontosPExtratoCabecalhoExterno", "jobCatalog");
                return null;
            }
        }

        public List<PontosExtratoItens> ObterPontosPExtratoCorpo2Externo(List<PontosExtratoItens> ExtratoPontosN)
        {
            return ExtratoPontosN;
        }

        public PontosExtrato ObterPontosCExtratoCabecalhoExterno(List<PontosExtratoItens> ExtratoPontosS, int saldo, int recordSetTotal)
        {
            try
            {
                var ExtratoItem = new PontosExtrato();

                if (ExtratoPontosS != null)
                {
                    ExtratoItem.valueInPoints = saldo;
                    ExtratoItem.name = "Pontos disponiveis";
                    ExtratoItem.recordSetTotal = recordSetTotal;
                    ExtratoItem.recordSetCount = ExtratoPontosS.Count();
                }
                return ExtratoItem;
            }
            catch (Exception ex)
            {
                GravarLogErro(ex.Message, "Erro ao ObterPontosCExtratoCabecalhoExterno", "ExtratoService", "ObterPontosCExtratoCabecalhoExterno", "jobCatalog");
                return null;
            }
        }

        public List<PontosExtratoItens> ObterPontosExtratoCorpoExterno(int accountNumber, string tipoPonto, int mktPlaceCatalogoId)
        {
            var extratoExterno = new List<PontosExtratoItens>();

            var ExtratoPontosS = ObterHeaderPorTipo(accountNumber, tipoPonto, mktPlaceCatalogoId);

            ExtratoPontosS = ExtratoPontosS.Where(x => x.TypePoints.Contains("I") ||
                                                       x.TypePoints.Contains("T") ||
                                                       x.TypePoints.Contains("P") ||
                                                       x.TypePoints.Contains("J") ||
                                                       x.TypePoints.Contains("A") ||
                                                       x.TypePoints.Contains("C") ||
                                                       x.TypePoints.Contains("ED") ||
                                                       x.TypePoints.Contains("MH")).ToList();

            foreach (var extrato in ExtratoPontosS)
            {
                var ExtratoItem = new PontosExtratoItens
                {
                    cp = extrato.CampaignNumber,
                    year = extrato.CampaignYearNumber,
                    description = extrato.ProgramDescription,
                    ad = extrato.OrderId,
                    valueInPoints = extrato.Points,
                    insertDate = extrato.TransactionDate
                };

                switch (extrato.TypePoints)
                {
                    case "I":
                        ExtratoItem.type = "Indicação";
                        break;
                    case "T":
                        ExtratoItem.type = "Atividade";
                        break;
                    case "P":
                        ExtratoItem.type = "Produtividade";
                        break;
                    case "J":
                        ExtratoItem.type = "Ajustes";
                        break;
                    case "A":
                        ExtratoItem.type = "Apoio";
                        break;
                    case "ED":
                        ExtratoItem.type = "Estrelas";
                        break;
                    case "C":
                        ExtratoItem.type = "Consecutividade";
                        break;
                    case "MH":
                        ExtratoItem.type = "Migração";
                        break;
                }

                extratoExterno.Add(ExtratoItem);
            }

            return extratoExterno;
        }

        public List<PontosExtratoItens> ObterPontosCExtratoCorpo2Externo(List<PontosExtratoItens> ExtratoPontosS)
        {
            return ExtratoPontosS;
        }

        public PontosExpirarExtrato ObterPontosExpirarExtratoExterno(List<PontosExpirarExtratoItens> pontosAExpirar, int recordSetTotal)
        {
            try
            {

                var ExtratoItem = new PontosExpirarExtrato();
                if (pontosAExpirar != null)
                {
                    ExtratoItem.valueInPoints = pontosAExpirar.Count() > 0 ? pontosAExpirar.Sum(x => x.valueInPoints) : 0;
                    ExtratoItem.name = "Pontos a Vencer";
                    ExtratoItem.recordSetTotal = recordSetTotal;
                    ExtratoItem.recordSetCount = pontosAExpirar.Count();
                }
                return ExtratoItem;
            }
            catch (Exception ex)
            {
                GravarLogErro(ex.Message, "Erro ao ObterPontosExpirarExtratoExterno", "ExtratoService", "ObterPontosExpirarExtratoExterno", "jobCatalog");
                return null;
            }
        }

        public async Task<List<PontosExpirarExtratoItensMkt>> ObterPontosAExpirarExterno(int accountNumber, int mktPlaceCatalogoId, ExpirationInterval range)
        {
            try
            {
                var tokenCatalogo = await _cognitoService.SignInCatalogo();

                var catalogoUrlBase = ConfigurationManager.AppSettings["CatalogoUrlBase"];
                var ssoSubscriptionKey = ConfigurationManager.AppSettings["SsoSubscriptionKey"];

                var parameters = new KeyValuePair<string, string>[]
                {
                    new KeyValuePair<string, string>("Authorization", "Bearer " + tokenCatalogo),
                    new KeyValuePair<string, string>("ocp-apim-subscription-key", ssoSubscriptionKey),
                    new KeyValuePair<string, string>("username", accountNumber.ToString()),
                    new KeyValuePair<string, string>("catalog_id", mktPlaceCatalogoId.ToString()),
                };

                var jsonRequest = new JsonRequest(catalogoUrlBase, parameters);

                var url = "v1/participants/me/toExpire/" + (int)range;

                var balanceCatalogo = jsonRequest.Get<List<PontosExpirarExtratoItensMkt>>(url);

                foreach (var item in balanceCatalogo)
                    item.points = Math.Abs(item.points);

                return balanceCatalogo;
            }
            catch (Exception ex)
            {
                GravarLogErro(ex.ToString(), "Erro ao ObterPontosAExpirarExterno", "ExtratoService", "ObterPontosAExpirarExterno", "api");
                throw new MarketPlaceException(ex.Message);
            }
        }

        public List<PontosExpirarExtratoItens> ObterExtratoAECorpoExterno(List<PontosExpirarExtratoItensMkt> PontosAExpirar)
        {
            List<PontosExpirarExtratoItens> extratoExterno = new List<PontosExpirarExtratoItens>();

            try
            {
                if (PontosAExpirar != null)
                {
                    foreach (PontosExpirarExtratoItensMkt extratoDb in PontosAExpirar)
                    {
                        PontosExpirarExtratoItens ExtratoItem = new PontosExpirarExtratoItens();
                        ExtratoItem.valueInPoints = extratoDb.points;
                        ExtratoItem.expirationDate = extratoDb.date;

                        extratoExterno.Add(ExtratoItem);
                    }
                }
                return extratoExterno;
            }
            catch (Exception ex)
            {
                GravarLogErro(ex.Message, "Erro ao ObterExtratoCorpoExterno", "ExtratoService", "ObterExtratoCorpoExterno", "jobCatalog");
                return null;
            }
        }

        public List<PontosExpirarExtratoItens> ObterPontosAExpirar2Externo(List<PontosExpirarExtratoItens> PontosAExpirar)
        {
            return PontosAExpirar;
        }

        public PontosResgatadosExtrato ObterPontosResgatadosExtratoExterno(List<PontosResgatadosExtratoItens> ResgateEstorno, int recordSetTotal)
        {
            try
            {
                PontosResgatadosExtrato ExtratoItem = new PontosResgatadosExtrato();
                if (ResgateEstorno != null)
                {
                    ExtratoItem.valueInPoints = ResgateEstorno.Sum(x => x.points) * -1;
                    ExtratoItem.name = "Pontos Resgatados";
                    ExtratoItem.recordSetTotal = recordSetTotal;
                    ExtratoItem.recordSetCount = ResgateEstorno.Count();
                }
                return ExtratoItem;
            }
            catch (Exception ex)
            {
                GravarLogErro(ex.Message, "Erro ao ObterPontosResgatadosExtratoExterno", "ExtratoService", "ObterPontosResgatadosExtratoExterno", "jobCatalog");
                return null;
            }
        }

        public async Task<List<PontosResgatadosExtratoItens>> ObterResgatesExterno(string accountNumber, int mktPlaceCatalogoId, string accessTokenMktPlace, int mktPlaceParticipantId, string date, string impersonateUser)
        {
            if (string.IsNullOrEmpty(accessTokenMktPlace))
                accessTokenMktPlace = await _cognitoService.SignIn();
            string customerId = ConfigurationManager.AppSettings["mktPlace_clientId"];

            string datef = string.Empty;
            const string dateFormat = "yyyy-MM-dd";

            datef = DateTime.Now.ToString(dateFormat);

            var autthorization = "Bearer " + accessTokenMktPlace;

            JsonRequest jsonRequest = new JsonRequest(ConfiguracaoService.MktPlaceUrl(),
                new KeyValuePair<string, string>("Authorization", autthorization),
                new KeyValuePair<string, string>("participantid", mktPlaceParticipantId.ToString()),
                new KeyValuePair<string, string>("catalogId", mktPlaceCatalogoId.ToString()),
                new KeyValuePair<string, string>("customerId", customerId),
                new KeyValuePair<string, string>("impersonatorUser", impersonateUser),
                new KeyValuePair<string, string>("username", accountNumber.ToString()));

            try
            {
                var url = "/v1/participants/me/accountStatement/from/" + date + "/to/" + datef + "/1/999999999";

                List<PontosResgatadosExtratoItens> Resgates = new List<PontosResgatadosExtratoItens>();

                Resgates = jsonRequest.Get<List<PontosResgatadosExtratoItens>>(url);

                var redirectBalance = ConfigurationManager.AppSettings["ExpiratedBalance"];

                if (redirectBalance == "Producao")
                {
                    Resgates = Resgates.Where(x => x.transactionTypeId == 9 || x.transactionTypeId == 17).ToList();
                }
                return Resgates;

            }
            catch (Exception ex)
            {
                throw new MarketPlaceException(ex.Message);
            }

        }

        public List<PontosResgatadosExtratoItens> ObterResgates2Externo(List<PontosResgatadosExtratoItens> PontosResgatadosEx)
        {
            return PontosResgatadosEx;
        }

        public MadrinhaExtrato ObterMadrinhaExtratoCabecalhoExterno(List<MadrinhaExtratoItens> ExtratoPontos, int recordSetTotal)
        {
            try
            {
                MadrinhaExtrato ExtratoItem = new MadrinhaExtrato();
                if (ExtratoPontos != null)
                {
                    ExtratoItem.recordSetTotal = recordSetTotal;
                    ExtratoItem.recordSetCount = ExtratoPontos.Select(x => x.IndicacaoHeaderId).Distinct().Count();
                }

                return ExtratoItem;
            }
            catch (Exception ex)
            {
                GravarLogErro(ex.Message, "Erro ao ObterMadrinhaExtratoCabecalhoExterno", "ExtratoService", "ObterMadrinhaExtratoCabecalhoExterno", "jobCatalog");
                return null;
            }
        }

        public List<MadrinhaExtratoItens> ObterMadrinhaExtratoCorpoExterno(int accountNumber, string tipoPontoId, int MktPlaceCatalogoId, int cmpgnNr, int cmpgnYr)
        {

            List<MadrinhaExtratoItens> extratoExterno = new List<MadrinhaExtratoItens>();
            List<MadrinhaExtratoDB> ExtratoPontosH = new List<MadrinhaExtratoDB>();

            ExtratoPontosH = ObterHeaderDetailPorTipoIndicacao(accountNumber, tipoPontoId, MktPlaceCatalogoId);
            if (cmpgnNr > 0 || cmpgnYr > 0)
            {
                ExtratoPontosH = ExtratoPontosH.Where(x => x.CampaignNumber >= cmpgnNr && x.CampaignYearNumber >= cmpgnYr).ToList();
            }

            foreach (int idHeader in ExtratoPontosH.Select(x => x.IndicacaoHeaderId).Distinct())
            {
                MadrinhaExtratoDB itemExtratoDb = new MadrinhaExtratoDB();
                itemExtratoDb = ExtratoPontosH.FirstOrDefault(x => x.IndicacaoHeaderId == idHeader);

                MadrinhaExtratoItens ExtratoItem = new MadrinhaExtratoItens();
                ExtratoItem.IndicacaoHeaderId = itemExtratoDb.IndicacaoHeaderId;
                ExtratoItem.name = itemExtratoDb.ReferredNameDetail;
                ExtratoItem.cpEntrada = itemExtratoDb.CampaignNumber + "/" + itemExtratoDb.CampaignYearNumber;
                ExtratoItem.valueInPoints = itemExtratoDb.Points;
                ExtratoItem.description = itemExtratoDb.ProgramDescription;

                ExtratoItem.order = new List<MadrinhaExtratoPedidos>();
                foreach (MadrinhaExtratoDB extratoDbCampanhas in ExtratoPontosH.FindAll(x => x.IndicacaoHeaderId == idHeader).ToList())
                {
                    MadrinhaExtratoPedidos campanhaItem = new MadrinhaExtratoPedidos();
                    campanhaItem.campaign = extratoDbCampanhas.OrderCampaignNumberDetail;
                    campanhaItem.result = extratoDbCampanhas.StatusPoints;

                    ExtratoItem.order.Add(campanhaItem);
                }

                extratoExterno.Add(ExtratoItem);
            }

            return extratoExterno;
        }

        public List<MadrinhaExtratoItens> ObterMadrinhaExtratoCorpo2Externo(List<MadrinhaExtratoItens> ExtratoPontosM)
        {
            return ExtratoPontosM;
        }

        public List<MadrinhaExtratoDB> ObterHeaderDetailPorTipoIndicacao(int accountNumber, string tipoPonto, int mktPlaceCatalogoId)
        {
            List<MadrinhaExtratoDB> linhasExtrato = new List<MadrinhaExtratoDB>();
            using (SqlConnection cn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
            {
                SqlCommand cmd = new SqlCommand("JP_PRC_ExtratoExternoPorTipoIndicacao", cn);
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@AccountNumber", Value = accountNumber, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@StatusPoints", Value = tipoPonto, SqlDbType = SqlDbType.Char, Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@MktPlaceCatalogoId", Value = mktPlaceCatalogoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });

                cmd.CommandTimeout = 600; //10 minutos
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                linhasExtrato = new List<MadrinhaExtratoDB>();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MadrinhaExtratoDB headerDetailExtrato = new MadrinhaExtratoDB();
                        headerDetailExtrato.IndicacaoHeaderId = Convert.ToInt32(reader["Id"]);
                        headerDetailExtrato.CampaignNumber = Convert.ToInt32(reader["CampaignNumberHeader"]);
                        headerDetailExtrato.CampaignYearNumber = Convert.ToInt32(reader["CampaignYearNumberHeader"]);
                        headerDetailExtrato.Points = Convert.ToInt32(reader["Points"]);
                        headerDetailExtrato.ReferredNameDetail = reader["ReferredNameDetail"].ToString();
                        headerDetailExtrato.StatusPoints = reader["StatusPoints"].ToString();
                        headerDetailExtrato.OrderCampaignNumberDetail = Convert.ToInt32(reader["OrderCampaignNumberDetail"]);
                        headerDetailExtrato.ProgramDescription = reader["IncentiveProgramDescriptionHeader"].ToString();

                        linhasExtrato.Add(headerDetailExtrato);
                    }
                }

                cn.Close();
            }

            return linhasExtrato;

        }

        public PontosExpiradosExtrato ObterPontosExpiradosExtratoExterno(List<PontosExpiradosExtratoItens> PontosExpirados, int recordSetTotal)
        {
            try
            {
                PontosExpiradosExtrato ExtratoItem = new PontosExpiradosExtrato();
                if (PontosExpirados != null)
                {
                    ExtratoItem.valueInPoints = PontosExpirados.Count() > 0 ? PontosExpirados.Sum(x => x.valueInPoints) * -1 : 0;
                    ExtratoItem.name = "Pontos Expirados";
                    ExtratoItem.recordSetTotal = recordSetTotal;
                    ExtratoItem.recordSetCount = PontosExpirados.Count();
                }
                return ExtratoItem;
            }
            catch (Exception ex)
            {
                GravarLogErro(ex.Message, "Erro ao ObterPontosExpiradosExtratoExterno", "ExtratoService", "ObterPontosExpiradosExtratoExterno", "jobCatalog");
                return null;
            }
        }

        public async Task<List<PontosExpiradosExtratoItensMkt>> ObterPontosExpiradoExterno(int accountNumber, int mktPlaceCatalogoId, string accessTokenMktPlace, int mktPlaceParticipantId, int impersonateUser)
        {
            if (string.IsNullOrEmpty(accessTokenMktPlace))
                accessTokenMktPlace = await _cognitoService.SignIn();

            var customerId = ConfigurationManager.AppSettings["mktPlace_clientId"];

            var autthorization = "Bearer " + accessTokenMktPlace;

            var jsonRequest = new JsonRequest(ConfiguracaoService.MktPlaceUrl(),
                new KeyValuePair<string, string>("Authorization", autthorization),
                new KeyValuePair<string, string>("participantid", mktPlaceParticipantId.ToString()),
                new KeyValuePair<string, string>("catalogId", mktPlaceCatalogoId.ToString()),
                new KeyValuePair<string, string>("customerId", customerId),
                new KeyValuePair<string, string>("impersonatorUser", impersonateUser.ToString()),
                new KeyValuePair<string, string>("username", accountNumber.ToString()));

            try
            {
                var url = "/v1/participants/me/accountStatement/from/2016-07-01/to/2030-08-05/1/999999999";

                var Expirados = new List<PontosExpiradosExtratoItensMkt>();

                Expirados = jsonRequest.Get<List<PontosExpiradosExtratoItensMkt>>(url);

                var redirectBalance = ConfigurationManager.AppSettings["ExpiratedBalance"];

                if (redirectBalance == "Producao")//Homologacao Producao                
                    Expirados = Expirados.Where(x => x.transactionTypeId == 11).ToList();

                return Expirados;

            }
            catch (Exception ex)
            {
                throw new MarketPlaceException(ex.Message);
            }

        }

        public List<PontosExpiradosExtratoItens> ObterExtratoECorpoExterno(List<PontosExpiradosExtratoItensMkt> PontosAExpirar)
        {
            List<PontosExpiradosExtratoItens> extratoExterno = new List<PontosExpiradosExtratoItens>();

            try
            {
                if (PontosAExpirar != null)
                {
                    foreach (PontosExpiradosExtratoItensMkt extratoDb in PontosAExpirar)
                    {
                        PontosExpiradosExtratoItens ExtratoItem = new PontosExpiradosExtratoItens();
                        ExtratoItem.description = extratoDb.description;
                        ExtratoItem.valueInPoints = extratoDb.points * -1;
                        ExtratoItem.insertDate = extratoDb.date;

                        extratoExterno.Add(ExtratoItem);
                    }
                }
                return extratoExterno;
            }
            catch (Exception ex)
            {
                GravarLogErro(ex.Message, "Erro ao ObterExtratoECorpoExterno", "ExtratoService", "ObterExtratoECorpoExterno", "jobCatalog");
                return null;
            }
        }

        public List<PontosExpiradosExtratoItens> ObterPontosExpirado2Externo(List<PontosExpiradosExtratoItens> PontosExpiradosEx)
        {
            return PontosExpiradosEx;
        }

        #endregion

        private static void GravarLogProcessamento(string Mensagem, string Source, string Metodo, string codigo)
        {
            Console.WriteLine(DateTime.Now.ToString() + " - " + Mensagem);
            var logProcessamento = new LogProcessamentoDb()
            {
                Metodo = Metodo,
                Controller = "ExtratoService",
                Codigo = codigo,
                Mensagem = Mensagem,
                Source = Source,
                DataInclusao = DateTime.Now
            };

            var logService = new LogService();
            logService.GravarLogProcessamento(logProcessamento);
        }

        private static void GravarLogErro(string Erro, string Mensagem, string Source, string Metodo, string Codigo)
        {
            var logErro = new LogErro()
            {
                Erro = Erro,
                Mensagem = Mensagem,
                Source = Source,
                Metodo = Metodo,
                Controller = "ExtratoService",
                Pagina = string.Empty,
                Codigo = Codigo
            };

            var logErroService = new LogErroService();
            logErroService.SalvarLogErro(logErro);
        }
    }
}
