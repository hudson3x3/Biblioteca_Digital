using GrupoLTM.WebSmart.DTO.MarketPlace;
using System;
using System.Linq;
using System.Collections.Generic;
using GrupoLTM.WebSmart.Infrastructure.Helpers;
using System.Net;
using GrupoLTM.WebSmart.Infrastructure.Exceptions;
using Newtonsoft.Json;
using GrupoLTM.WebSmart.Domain.Models;
using System.Configuration;
using System.Threading.Tasks;
using RestSharp;

namespace GrupoLTM.WebSmart.Services.LiveServiceMkt
{
    public class OrderService
    {
        internal static readonly bool ResgateOfflineTeste = Convert.ToBoolean(ConfigurationManager.AppSettings["ResgateOfflineTeste"]);
        private static string GetTokenMktPlace(int accountNumber, long mktPlaceParticipantId, long mktPlaceCatalogoId)
        {
            try
            {
                var accessTokenMktPlaceService = string.Empty;
                accessTokenMktPlaceService = MarketPlaceService.GetTokenContext(accountNumber, mktPlaceParticipantId, mktPlaceCatalogoId, null);

                var token = JsonConvert.DeserializeObject<dynamic>(accessTokenMktPlaceService);
                return token["access_token"].Value;
            }
            catch (Exception ex)
            {
                gravaLogErro("Erro ao tentar gerar o token de resgate offline", ex.Message, "GrupoLTM.WebSmart.Services", string.Format("Order.GetTokenMktPlace(accountNumber:{0},mktPlaceParticipantId:{1},mktPlaceCatalogoId:{2})", accountNumber.ToString(), mktPlaceParticipantId.ToString(), mktPlaceCatalogoId.ToString()), "logCatalog");
                return "";
            }
        }
        public static async Task<AuthorizeResponse> AutorizacaoPedido(AuthorizeOrder authorizeOrder)
        {
            AuthorizeResponse autorizacao = new AuthorizeResponse();

            try
            {
                if (ResgateOfflineTeste)
                {
                    autorizacao.OrderId = 9999999;
                    ErrorCode erroteste = new ErrorCode()
                    {
                        Code = "200",
                        Description = "-->> TESTE LIGADO <<--"
                    };
                    Console.WriteLine("-->> TESTE LIGADO <<--");
                    autorizacao.Errors.Add(erroteste);
                }
                else
                {
                    //PASSO 1 - GERAR TOKEN DO PARTICIPANTE
                    var token = GetTokenMktPlace(Convert.ToInt32(authorizeOrder.Participant.UserName), authorizeOrder.Participant.Id, authorizeOrder.CatalogId);

                    if (!string.IsNullOrEmpty(token))
                    {
                        //PASSO 2 - VALIDAR SALDO DO MESMO
                        var balance = await new ExtratoService().ObterSaldo(authorizeOrder.Participant.UserName, Convert.ToInt32(authorizeOrder.CatalogId), token, Convert.ToInt32(authorizeOrder.Participant.Id), "admin_job");

                        if (balance >= (authorizeOrder.Items[0].SellingPrice * authorizeOrder.Items[0].Quantity * authorizeOrder.ConversionRate[0].ConversionFactor))
                        {
                            //PASSO 3 - GERAR AUTORIZAÇÃO DO PEDIDO
                            var rAutorizacao = AutorizarPedido(authorizeOrder, token, out HttpStatusCode statusCode);

                            if (statusCode == HttpStatusCode.OK && rAutorizacao.Errors.Count == 0)
                            {
                                //PASSO 4 - CONFIRMAR PEDIDO
                                var confirmado = ConfirmarPedido(rAutorizacao, token);
                                autorizacao.OrderId = rAutorizacao.OrderId;
                                autorizacao.Errors = confirmado.Errors;
                            }
                            else if (rAutorizacao.OrderId > 0)
                            {
                                //PASSO 5 - CANCELAR PEDIDO CASO A AUTORIZAÇÃO FALHAR
                                var cancelado = CancelarPedido(rAutorizacao.OrderId, token);
                                autorizacao.OrderId = rAutorizacao.OrderId;
                                autorizacao.Errors.AddRange(cancelado.Errors);
                            }
                        }
                        else
                        {
                            //SE SALDO INSUFICIENTE PARA RESGATE
                            autorizacao.OrderId = 0;
                            ErrorCode errosaldo = new ErrorCode()
                            {
                                Code = "400",
                                Description = "Saldo Insuficiente."
                            };
                            autorizacao.Errors.Add(errosaldo);
                        }
                    }
                    else
                    {
                        //SE OCORRER ERRO AO GERAR O TOKEN
                        ErrorCode errotoken = new ErrorCode()
                        {
                            Code = "400",
                            Description = "Erro ao gerar o token"
                        };
                        autorizacao.Errors.Add(errotoken);
                    }
                }

                return autorizacao;
            }
            catch (Exception ex)
            {
                //SE OCORRER EXCEÇÃO NO PROCESSO
                ErrorCode errotoken = new ErrorCode()
                {
                    Code = "400",
                    Description = ex.Message + "Erro ao gerar o token"
                };
                autorizacao.Errors.Add(errotoken);
                gravaLogErro("Erro ao tentar autorizarPedido offline", ex.Message, "GrupoLTM.WebSmart.Services", string.Format("Order.autorizacaoPedido(accountNumber:{0},mktPlaceParticipantId:{1},Sku:{2})", authorizeOrder.Participant.UserName.ToString(), authorizeOrder.Participant.Id.ToString(), authorizeOrder.Items[0].Sku), "logCatalog");

                return autorizacao;
                throw new MarketPlaceException(ex.Message);
            }
        }

        private static AuthorizeResponse AutorizarPedido(AuthorizeOrder authorizeOrder, string token, out HttpStatusCode statusCode)
        {
            var url = ConfiguracaoService.MktPlaceUrl() + "v1/orders/authorize";
            var client = new RestClient(url);

            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("ImpersonatorUser", authorizeOrder.Participant.Id.ToString());
            request.AddParameter("application/json", authorizeOrder.ToJson(), ParameterType.RequestBody);

            var response = client.Execute(request);

            statusCode = response.StatusCode;

            var result = JsonConvert.DeserializeObject<AuthorizeResponse>(response.Content);

            if (result is null)
                throw new Exception("Erro na requisição de autorização, response: " + response.Content.ToJson());

            return result;
        }


        private static ConfirmResponse ConfirmarPedido(AuthorizeResponse autorizacao, string accessTokenMktPlace)
        {

            try
            {
                var authorization = "Bearer " + accessTokenMktPlace;
                JsonRequest jsonRequest = new JsonRequest(ConfiguracaoService.MktPlaceUrl(),
                    new KeyValuePair<string, string>("Authorization", authorization),
                    new KeyValuePair<string, string>("Content-Type", "application/json"));

                HttpStatusCode responseStatusCode;

                var url = "v1/orders/confirm";
                ConfirmRequest confirmar = new ConfirmRequest
                {
                    ParentOrderId = autorizacao.OrderId
                };

                ConfirmResponse confirmado = new ConfirmResponse();

                confirmado = jsonRequest.Post<ConfirmResponse>(url, confirmar, out responseStatusCode);
                confirmado.OrderId = confirmar.ParentOrderId;

                if (autorizacao.Errors.Count() > 0)
                {
                    if (autorizacao.OrderId > 0)
                    {
                        var cancelado = CancelarPedido(autorizacao.OrderId, accessTokenMktPlace);
                    }
                    confirmado.Errors.AddRange(autorizacao.Errors);
                    confirmado.StatusConfirm = (Int32)StatusConfirmOrder.Error;
                }

                return confirmado;
            }
            catch (Exception ex)
            {
                throw new MarketPlaceException(ex.Message);
            }
        }
        private static CancelResponse CancelarPedido(long ParentOrderId, string accessTokenMktPlace)
        {
            CancelRequest cancelar = new CancelRequest
            {
                ParentOrderId = ParentOrderId,
                MotiveCancel = MotiveCancel.AUTOMATIC
            };

            var authorization = "Bearer " + accessTokenMktPlace;
            JsonRequest jsonRequest = new JsonRequest(ConfiguracaoService.MktPlaceUrl(),
                new KeyValuePair<string, string>("Authorization", authorization),
                new KeyValuePair<string, string>("Content-Type", "application/json"));
            HttpStatusCode responseStatusCode;
            try
            {
                var url = "v1/orders/cancel";
                CancelResponse result = jsonRequest.Post<CancelResponse>(url, cancelar, out responseStatusCode);
                result.Errors.Add(new ErrorCode
                {
                    Code = "400",
                    Description = "Pedido Cancelado"
                });
                return result;
            }
            catch (Exception ex)
            {
                gravaLogErro("Erro ao tentar cancelar o pedido", ex.Message, "GrupoLTM.WebSmart.Services", string.Format("Order.GetTokenMktPlace(ParentOrderId:{0},accessTokenMktPlace:{1})", ParentOrderId.ToString(), accessTokenMktPlace.ToString()), "logCatalog");
                throw new MarketPlaceException(ex.Message);
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
                Controller = "OrderServices",
                Pagina = string.Empty,
                Codigo = Codigo
            };

            var logErroService = new LogErroService();
            logErroService.SalvarLogErro(logErro);
        }
    }
}
