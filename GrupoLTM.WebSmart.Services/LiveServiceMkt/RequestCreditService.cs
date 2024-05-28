using GrupoLTM.WebSmart.Domain.Enums;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.DTO;
using GrupoLTM.WebSmart.Infrastructure.Exceptions;
using GrupoLTM.WebSmart.Infrastructure.Helpers;
using GrupoLTM.WebSmart.Services.Log;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;

namespace GrupoLTM.WebSmart.Services.LiveServiceMkt
{
    public class RequestCreditService
    {
        private readonly string urlCredito = ConfigurationManager.AppSettings["live_baseUrl"];
        private readonly string subscriptionKey = ConfigurationManager.AppSettings["live_subscriptionKey"];
        private readonly string requesterCredit = ConfigurationManager.AppSettings["requesterCredit"];
        private readonly int profileId = Convert.ToInt32(ConfigurationManager.AppSettings["profileId"]);
        private readonly int bankLotTypeId = Convert.ToInt32(ConfigurationManager.AppSettings["bankLotTypeId"]);
        private readonly int loteEnvioCredito = Convert.ToInt32(ConfigurationManager.AppSettings["loteEnvioCredito"]);
        private readonly LiveService liveService = new LiveService();

        public void ProcessarEnvioCredito(List<PontuacaoEnvioModel> pontuacoes, string arquivoNome)
        {
            var sequencial = 1;
            var totalItens = pontuacoes.Count;
            var totalPontos = pontuacoes.Sum(x => x.Pontos);

            LogProcessamento.Log($"Processando envio de {totalPontos} pontos de crédito do arquivo " + arquivoNome, arquivoNome, "RequestCreditService", "ProcessarEnvioCredito");

            try
            {
                if (pontuacoes != null && pontuacoes.Any())
                {
                    using (var context = UnitOfWorkFactory.Create())
                    {
                        var repositorio = context.CreateRepository<ArquivoConfiguracao>();

                        //Consulta Configuracoes de Arquivo
                        var arquivoConfiguracao = repositorio.Find<ArquivoConfiguracao>(x => x.TipoArquivoId == (int)EnumDomain.TipoArquivo.EnvioPontuacaoLive && x.Ativo);

                        //Monta Sequencial arquivo
                        sequencial = arquivoConfiguracao.SequencialArquivo ?? sequencial;

                        var lotesDataExp = AgruparEmLotes(pontuacoes, arquivoNome);

                        foreach (var loteDataExp in lotesDataExp)
                        {
                            var loteItens = AgruparEmLotesPorRange(loteDataExp.Value, loteEnvioCredito, arquivoNome);

                            foreach (var itens in loteItens)
                            {
                                sequencial++;

                                var status = EnviarCreditos(itens, sequencial, arquivoNome, out string mensagemErro, out int bankLotId);

                                //Atualiza Sequencial Arquivo - Arquivo Configuracao
                                arquivoConfiguracao.SequencialArquivo = sequencial;
                                repositorio.Update(arquivoConfiguracao);

                                var programaIncentivo = itens.First().IdProgramaIncentivo;

                                PontuacaoService.AtualizaStatusPontuacao(itens,
                                    EnumDomain.StatusPontuacao.PendenteEnvio,
                                    status,
                                    mensagemErro,
                                    bankLotId,
                                    programaIncentivo,
                                    arquivoNome);
                            }
                        }
                    }
                }
            }
            catch (ProcessamentoException ex)
            {
                LogProcessamento.LogErro("Erro ao processar envio de crédito - " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                LogProcessamento.LogErro("Erro ao processar envio de crédito - " + ex.Message, ex.ToProcException(arquivoNome));
            }
        }

        private EnumDomain.StatusPontuacao EnviarCreditos(List<PontuacaoEnvioModel> pontuacoes, int sequencial, string arquivoNome, out string mensagemErro, out int bankLotId)
        {
            mensagemErro = string.Empty;

            var pontosLote = pontuacoes.Sum(x => x.Pontos);

            LogProcessamento.Log($"Enviando lote com {pontosLote} pontos, sequencial {sequencial}", arquivoNome, "RequestCreditService", "ProcessarEnvioCredito");

            //Efetuar Credito
            var creditoEnviado = RequisitarCredito(pontuacoes, sequencial, arquivoNome, out string status, out string content, out bankLotId);

            if (creditoEnviado)
            {
                var mensagem = $"Requisição de crédito enviado com sucesso, sequencial: {sequencial} - bankLotId: {bankLotId}";
                LogProcessamento.Log(mensagem, arquivoNome, "RequestCreditService", "EnviarCreditos", content);
            }
            else
            {
                mensagemErro = content;
                LogProcessamento.LogErro("Erro ao requisitar crédito", $"{status} - {content}", "RequestCreditService", "EnviarCreditos", arquivoNome);
            }

            var statusCredito = creditoEnviado ? EnumDomain.StatusPontuacao.Enviado : EnumDomain.StatusPontuacao.Rejeitada;

            return statusCredito;
        }

        private bool RequisitarCredito(List<PontuacaoEnvioModel> itens, int sequencial, string arquivoNome, out string status, out string content, out int bankLotId)
        {
            const string dateFormat = "dd/MM/yyyy";

            try
            {
                var token = liveService.GetToken().AccessToken;

                var body = MontarBody(itens, sequencial, dateFormat);

                var response = RequestMktplace(token, body.ToJson());

                LogProcessamento.LogRequest("Request Crédito - sequencial: " + sequencial, arquivoNome, "POST", urlCredito, body, token, "Content:" + response.Content);

                status = $"{(int)response.StatusCode} - {response.StatusDescription}";

                content = response.Content;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var resultado = JObject.Parse(content);
                    var value = resultado.GetValue("bankLotId");
                    bankLotId = value.ToObject<int>();
                }
                else
                {
                    bankLotId = 0;
                }

                return response.IsSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao requisitar o crédito do arquivo " + arquivoNome, ex);
            }
        }

        private IRestResponse RequestMktplace(string token, string body)
        {
            var request = new RestRequest(Method.POST);

            request.AddHeader("Authorization", "Bearer " + token);
            request.AddHeader("Ocp-Apim-Subscription-Key", subscriptionKey);
            request.AddHeader("Content-Type", "application/json");

            request.AddParameter("application/json", body, ParameterType.RequestBody);

            var client = new RestClient(urlCredito);

            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                token = liveService.GetToken().AccessToken;
                request.Parameters[0].Value = "Bearer " + token;
                response = client.Execute(request);
            }

            return response;
        }

        private PontuacaoEnvioRequest MontarBody(List<PontuacaoEnvioModel> itens, int sequencial, string dateFormat)
        {
            var itensRequest = itens.Select(x => new PontuacaoItemRequest { login = x.Login, name = x.Nome, externalCode = x.Id, description = x.Descricao, points = x.Pontos }).ToList();

            var envPrd = ConfigurationManager.AppSettings["env"] == "prd";

            var fist = itens.First();
            var dataExpiracao = fist.DataExpiracao.ToString(dateFormat);
            var originId = envPrd ? fist.IdOrigem : 2032550342;
            var applicationVariantId = envPrd ? fist.MktPlaceCatalogoId : 40322;

            var objBody = new PontuacaoEnvioRequest
            {
                applicationVariantId = applicationVariantId,
                createParticipant = true,
                participantStatusId = (int)EnumMktPlace.ParticipantStatus.Active,
                expirationDate = dataExpiracao,
                originId = originId,
                requester = requesterCredit,
                clientId = fist.IdEmpresa,
                bankLotTypeId = bankLotTypeId,
                profileId = profileId,
                processOrigin = (int)EnumMktPlace.CreditOrigin.SemAprovacaoServiceNow,
                points = itens.Sum(x => x.Pontos),
                quantityItems = itensRequest.Count,
                loginTypeId = (int)EnumMktPlace.LogonType.RegistrationRe,
                fileId = sequencial,
                items = itensRequest
            };

            return objBody;
        }

        private Dictionary<DateTime, List<PontuacaoEnvioModel>> AgruparEmLotes(List<PontuacaoEnvioModel> pontuacoes, string arquivoNome)
        {
            var totalItens = pontuacoes.Count;
            var totalPontos = pontuacoes.Sum(x => x.Pontos);

            var lotes = pontuacoes.GroupBy(x => x.DataExpiracao).ToDictionary(x => x.Key, y => y.ToList());

            var totalPontosLote = lotes.Sum(x => x.Value.Sum(y => y.Pontos));

            if (totalPontosLote != totalPontos)
                throw new ProcessamentoException($"Agrupamento data de expiração incorreto, total: {totalPontos} - totalLote: {totalPontosLote}", arquivoNome);

            var totalItensLote = lotes.Sum(x => x.Value.Count);

            if (totalItensLote != totalItens)
                throw new ProcessamentoException($"Quantidade de itens no lote incorreto, total Itens: {totalItens} - total Itens Lote: {totalItensLote}", arquivoNome);

            return lotes;
        }

        private List<List<PontuacaoEnvioModel>> AgruparEmLotesPorRange(List<PontuacaoEnvioModel> pontuacoes, int range, string arquivoNome)
        {
            var totalItens = pontuacoes.Count;
            var totalPontos = pontuacoes.Sum(x => x.Pontos);

            //separa em listas com o valor do parâmetro [rangeArquivo]
            var lotes = pontuacoes.Select((item, index) => new { item, index })
                              .GroupBy(key => key.index / range, value => value.item)
                              .Select(grp => grp.ToList()).ToList();

            var totalPontosLote = lotes.Sum(x => x.Sum(y => y.Pontos));

            if (totalPontosLote != totalPontos)
                throw new ProcessamentoException($"Agrupamento por range incorreto, total: {totalPontos} - totalLote: {totalPontosLote}", arquivoNome);

            var totalItensLote = lotes.Sum(x => x.Count);

            if (totalItensLote != totalItens)
                throw new ProcessamentoException($"Quantidade de itens no lote incorreto, total Itens: {totalItens} - total Itens Lote: {totalItensLote}", arquivoNome);

            return lotes;
        }
    }
}
