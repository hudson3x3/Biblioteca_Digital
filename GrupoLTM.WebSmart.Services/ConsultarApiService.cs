using System;
using System.Net;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Web.Helpers;
using RestSharp;
using GrupoLTM.WebSmart.Infrastructure.Helpers;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Models.MktPlace;
using GrupoLTM.WebSmart.DTO;
using GrupoLTM.WebSmart.Services.Log;

namespace GrupoLTM.WebSmart.Services
{
    public class ConsultarApiService
    {
        public ResultRequestApi ConsultarApi(RequestApi model)
        {
            var apiResult = new ResultRequestApi()
            {
                Api = model.AccountNumber,
                AccountNumber = model.AccountNumber,
                Success = false,
            };

            try
            {
                var timeout = (int)TimeSpan.FromSeconds(60).TotalMilliseconds;

                var timer = new Stopwatch();

                timer.Start();

                var response = RequisitarApi(model, timeout);

                timer.Stop();

                if (response.StatusCode == 0 && timer.Elapsed.TotalMilliseconds >= timeout)
                    response.StatusCode = HttpStatusCode.RequestTimeout;

                apiResult.Status = (int)response.StatusCode;
                apiResult.TempoResposta = timer.Elapsed.Seconds;

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        apiResult.Content = DeserializeExtrato(response.Content, model.Api);
                        apiResult.Message = "Status OK";
                        apiResult.Success = true;
                        break;

                    case HttpStatusCode.RequestTimeout:
                        apiResult.Message = "A requisição excedeu o tempo limite de resposa (API Timeout)";
                        apiResult.Content = apiResult.Message;
                        break;

                    case HttpStatusCode.PreconditionFailed:
                        apiResult.Message = response.Content;
                        apiResult.Content = response.Content;
                        break;

                    case HttpStatusCode.BadRequest:
                    case HttpStatusCode.InternalServerError:
                        apiResult.Message = "A requisição retornou um erro";
                        apiResult.Content = response.Content;
                        break;

                    case HttpStatusCode.Unauthorized:
                        apiResult.Message = "O token de api informado é inválido";
                        apiResult.Content = response.Content;
                        break;

                    default:
                        apiResult.Message = "Tipo de retorno desconhecido";
                        apiResult.Content = response.Content;
                        break;
                }
            }
            catch (Exception ex)
            {
                var log = new LogProcessamentoModel
                {
                    Message = $"Não foi possível consultar a api: {model.Api}: {ex.Message}",
                    Class = "ConsultarApiService",
                    Method = "ConsultarApi",
                    Source = ex.StackTrace,
                    Date = DateTime.Now,
                    Data = model
                };

                //TODO: Update DataDog
                //GrayLogService.LogError(log);

                apiResult.Message = "Erro interno na aplicação";
                apiResult.Content = ex.Message;
            }

            return apiResult;
        }

        private IRestResponse RequisitarApi(RequestApi model, int timeout)
        {
            var urlApi = ConfigurationManager.AppSettings["apiAvon_url"];
            var apiSubscriptionKey = ConfigurationManager.AppSettings["apiMan_subscriptionKey"];

            var client = new RestClient(urlApi);

            var request = new RestRequest(model.Api, model.Type == "get" ? Method.GET : Method.POST);

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Ocp-Apim-Subscription-key", apiSubscriptionKey);

            var body = new
            {
                catalogId = 1,
                pageName = "extrato",
                grant_type = "password",
                tokenGetInfo = "InternoLTM",
                model.AccountNumber,
                mktPlaceCatalogoId = model.MktPlaceCatalogoId,
                mktPlaceParticipantId = model.MktPlaceParticipantId
            };

            request.AddParameter("application/json", body.ToJson(), ParameterType.RequestBody);

            var avonAuthentication = new AvonAuthentication
            {
                AccountNumber = model.AccountNumber,
                TokenGetInfo = "InternoLTM",
                PageName = "extrato",
                CatalogId = 1,
                i = 0,
            };

            var token = OAuthService.GetToken(avonAuthentication);

            request.AddHeader("Authorization", "Bearer " + token);

            client.Timeout = timeout;

            var response = client.Execute(request);

            return response;
        }

        private object DeserializeExtrato(string content, string api)
        {
            Type type;

            var url = api.Replace("extrato/", string.Empty).Replace("?noCheck=true", string.Empty);

            var endpoint = url.Split('/')[0];

            switch (endpoint)
            {
                case "participant":
                    type = typeof(ParticipanteModel);
                    break;

                case "GetExtratoHomeMMA":
                    type = typeof(List<HomeMMAExtratoExterno>);
                    break;

                case "GetExtratoPontosDisponiveis":
                    type = typeof(PontosExtrato);
                    break;

                case "GetExtratoPontosPendentes":
                    type = typeof(PontosExtrato);
                    break;

                case "GetExtratoPontosExpirar":
                    type = typeof(PontosExpirarExtrato);
                    break;

                case "GetExtratoPontosResgatados":
                    type = typeof(PontosResgatadosExtrato);
                    break;

                case "GetExtratoPontosExpirados":
                    type = typeof(PontosExpiradosExtrato);
                    break;

                case "GetExtratoConsecutividade":
                    type = typeof(List<ConsecutividadeExtratoExterno>);
                    break;

                case "GetExtratoMigracao":
                    type = typeof(List<MigracaoExtratoExterno>);
                    break;

                case "GetExtratoMadrinha":
                    type = typeof(MadrinhaExtrato);
                    break;

                case "GetSaldo":
                    type = typeof(decimal);
                    break;

                case "Resgatometro":
                    type = typeof(ResgatesCampanha);
                    break;

                case "GetNatal":
                    type = typeof(NatalExtratoExterno);
                    break;

                case "GetSummary":
                    type = typeof(ExtratoTotalizado);
                    break;

                case "GetSummaryExternal":
                    type = typeof(List<HeaderExtrato>);
                    break;

                case "GetBalance":
                    type = typeof(decimal?);
                    break;

                case "DebitCurrencyBalance":
                    type = typeof(ReversedBalance);
                    break;

                case "DebitCashBalance":
                    type = typeof(ReversePontosMaisCash);
                    break;

                case "ExpirationDate":
                    type = typeof(string);
                    break;

                case "ExpiratedBalance":
                    type = typeof(string);
                    break;

                case "GetHeaderByType":
                    type = typeof(List<HeaderExtrato>);
                    break;

                default:
                    throw new Exception("API não configurada: " + api);
            }

            var obj = Json.Decode(content, type);

            return obj;
        }
    }
}
