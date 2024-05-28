using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Infrastructure.Helpers;
using GrupoLTM.WebSmart.Services.Model;
using Newtonsoft.Json;

namespace GrupoLTM.WebSmart.Services
{
    public class LiveService
    {
        private readonly string _urlOrigem = ConfigurationManager.AppSettings["live_urlOrigem"];
        private readonly string _clientId = ConfigurationManager.AppSettings["live_clientId"];
        private readonly string _campaignId = ConfigurationManager.AppSettings["live_campaignId"];
        private readonly string _billingCode = ConfigurationManager.AppSettings["live_billingCode"];
        private readonly string _subscriptionKey = ConfigurationManager.AppSettings["live_subscriptionKey"];

        public void ObterOrigens(List<ProgramaIncentivo> programasIncentivo)
        {
            var token = GetToken();

            var origens = ObterOrigensLive(token.AccessToken);

            var naoCadastrados = programasIncentivo.Where(x => !origens.Any(y => y.Name == x.Nome)).ToList();

            if (naoCadastrados.Any())
                CadastrarOrigensLive(naoCadastrados, token.AccessToken);

            if (naoCadastrados.Count < programasIncentivo.Count)
            {
                var updateExistentes = programasIncentivo.Where(x => !naoCadastrados.Any(y => y.Id == x.Id)).ToList();

                foreach (var programa in updateExistentes)
                {
                    var origem = origens.First(item => item.Name == programa.Nome);

                    programa.IdOrigem = origem.OriginId;
                }
            }
        }

        public List<OrigemLive> ObterOrigensLive(string token)
        {
            try
            {
                var urlOrigem = _urlOrigem + $"?campaignId={_campaignId}&statusId=1&clientId={_clientId}";

                using (var client = new HttpClient())
                {
                    ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);

                    var result = client.GetAsync(urlOrigem).Result;

                    result.EnsureSuccessStatusCode();

                    var content = result.Content.ReadAsStringAsync().Result;

                    var retorno = JsonConvert.DeserializeObject<ResponseOrigensLive>(content);

                    return retorno.Origins;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Não foi possível obter a lista de origens do live", ex);
            }
        }

        public void CadastrarOrigensLive(List<ProgramaIncentivo> list, string token)
        {
            try
            {
                var nomes = list.Select(x => x.Nome).Distinct();

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);

                    foreach (var nome in nomes)
                    {
                        var origem = new OrigemLive(nome, _clientId, _campaignId, _billingCode);

                        var json = JsonConvert.SerializeObject(origem);

                        var body = new StringContent(json, Encoding.UTF8, "application/json");

                        var result = client.PostAsync(_urlOrigem, body).Result;

                        var content = result.Content.ReadAsStringAsync().Result;

                        if (result.IsSuccessStatusCode)
                        {
                            var retorno = JsonConvert.DeserializeObject<ResponseOrigensLive>(content);

                            var projectId = 30316;

                            if (retorno.OriginId == 0)
                                throw new ApplicationException("Houve um erro ao cadastrar a origem no live: " + nome);

                            foreach (var item in list.Where(x => x.Nome == nome))
                            {
                                item.IdOrigem = retorno.OriginId;

                                InsertAccount(projectId, retorno.OriginId);
                            }

                        }
                        else
                        {
                            var contentMessage = string.IsNullOrWhiteSpace(content) ? string.Empty : content;

                            var exMessage = string.Format(CultureInfo.InvariantCulture, "Request error: {0} ({1}).{2}", (int)result.StatusCode, result.ReasonPhrase, contentMessage);

                            throw new ApplicationException("Houve um erro ao cadastrar a origem no live: " + nome, new HttpRequestException(exMessage));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Não foi possível cadastrar as origens do live", ex);
            }
        }

        private void InsertAccount(long projectId, long AccountHolderId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfiguracaoService.GetDatabaseConnectionLiveSupplier()))
                {
                    SqlCommand cmd = new SqlCommand("JP_Insert_Account", conn);
                    cmd.CommandTimeout = 2400; // 40 minuitos
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@ProjectId", Value = projectId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@AccountHolderId", Value = AccountHolderId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });

                    conn.Open();

                    cmd.ExecuteNonQuery();

                    conn.Close();
                    cmd.Dispose();

                }
            }
            catch (Exception ex)
            {
                GravarLogErro("Erro na execução do InsertAccount:", ex.Message, "GrupoLTM.Avon.MMA.BLOBtoBulkServiceBUS", "GrupoLTM.WebSmart.Services", "jobCatalog");
            }
        }    

        private static void GravarLogErro(string Erro, string Mensagem, string Source, string Metodo, string Codigo)
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

        public Token GetToken()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var urlToken = ConfigurationManager.AppSettings["live_urlToken"];
                    var grant_type = ConfigurationManager.AppSettings["live_grant_type"];
                    var client_secret = ConfigurationManager.AppSettings["live_client_secret"];
                    var scope = ConfigurationManager.AppSettings["live_scope"];
                    var client_id = ConfigurationManager.AppSettings["live_client_id"];

                    var request = new HttpRequestMessage()
                    {
                        RequestUri = new Uri(urlToken),
                        Method = HttpMethod.Post,
                        Content = new FormUrlEncodedContent(new Dictionary<string, string>
                        {
                            {"scope", scope},
                            {"client_id", client_id},
                            {"grant_type", grant_type},
                            {"client_secret", client_secret},
                        })
                    };

                    ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                    var response = client.SendAsync(request).Result;

                    response.EnsureSuccessStatusCode();

                    var result = response.Content.ReadAsStringAsync().Result;

                    var token = JsonConvert.DeserializeObject<Token>(result);

                    return token;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Não foi possível obter o token do live", ex);
            }
        }
    }
}