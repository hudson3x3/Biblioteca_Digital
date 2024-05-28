using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrupoLTM.WebSmart.Domain.Enums;
using System.Transactions;
using System.IO;
using System.Net;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using GrupoLTM.WebSmart.Infrastructure.Cache;
using GrupoLTM.WebSmart.Infrastructure.Data;
using GrupoLTM.WebSmart.Infrastructure.Excel;
using GrupoLTM.WebSmart.Infrastructure.Configuration;
using GrupoLTM.WebSmart.Infrastructure.Helpers;
using GrupoLTM.WebSmart.Services.Interface;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Threading;
using Newtonsoft.Json;

namespace GrupoLTM.WebSmart.Services
{
    public class IntegradorService : IIntegradorService
    {
        private static readonly string _campanha = "41";
        private static readonly string _subscriptionKey = "6718a8d70c3442e7b69514dad9265994";
        private static readonly string _urlApiMktp = "https://cloudloyaltyapimanprd.azure-api.net/cloudloyalty/v1/";

        public async Task<string> AutenticarAsync(string username, string password)
        {
            HttpClient client = new HttpClient();
            var values = new Dictionary<string, string>
            {
                {"grant_type", "password"},
                {"campaign_id", "30296"},
                {"username", "smartbot01"},
                {"password", "smartbot01"}
            };

            var content = new FormUrlEncodedContent(values);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "ZGE3OTZkMWUtMGQzZC00MDcwLTg4ZDctOWZjZDhmZWJiYWEzOmUobWpAYSY5");
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);

            var response = await client.PostAsync(string.Concat(_urlApiMktp, "access-token"), content);

            var responseString = await response.Content.ReadAsStringAsync();
            var authResponse = new { access_token = "", token_type = "", expires_in = "" };

            authResponse = JsonConvert.DeserializeAnonymousType(responseString, authResponse);

            client.Dispose();

            return authResponse.access_token;
        }

        public async Task<SaldoModel> ObterSaldoAsync()
        {
            HttpClient client = new HttpClient();
            var token = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.First(c => c.Type == "ApiMktpToken").Value;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);

            var response = await client.GetAsync(string.Concat(_urlApiMktp, "participants/me/balance"));

            var responseString = await response.Content.ReadAsStringAsync();
            var saldoTemp = new
            {
                pointsValueOnhold = 0.0,
                totalPointsValue = 0.0,
                totalCreditPointsValue = 0.0,
                totalDebitPointsValue = 0.0,
                pointsValue = 0.0
            };

            saldoTemp = JsonConvert.DeserializeAnonymousType(responseString, saldoTemp);

            SaldoModel saldo = new SaldoModel()
            {
                SaldoAtual = saldoTemp.pointsValue,
                SaldoBloqueado = saldoTemp.pointsValueOnhold,
                SaldoTotal = saldoTemp.totalPointsValue,
                TotalDebito = saldoTemp.totalDebitPointsValue,
                TotalCredito = saldoTemp.totalCreditPointsValue
            };

            client.Dispose();

            return saldo;
        }

        public async Task<ExtratoModel> ObterExtratoAsync()
        {
            HttpClient client = new HttpClient();
            var token = await this.AutenticarAsync("", "");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);

            var response = await client.GetAsync(string.Concat(_urlApiMktp, "participants/me/extract?startDate=2017-02-14&endDate=2018-08-28&_offset=0&_limit=50"));

            var responseString = await response.Content.ReadAsStringAsync();

            var extratoTemp = new[]
            {
                new
                {
                    authorizationCode = "",
                    date = "",
                    description = "",
                    valuePoints = 0.0,
                    type = ""
                }
            };

            extratoTemp = JsonConvert.DeserializeAnonymousType(responseString, extratoTemp);
            var saldo = await ObterSaldoAsync();

            var extrato = new ExtratoModel()
            {
                Itens = new List<ExtratoItemModel>(),
                SaldoAtual = saldo.SaldoAtual,
                TotalCredito = saldo.TotalCredito,
                TotalDebito = saldo.TotalDebito
            };

            extratoTemp.ToList().ForEach(e => extrato.Itens.Add(new ExtratoItemModel()
            {
                Data = Convert.ToDateTime(e.date),
                Descricao = e.description,
                Tipo = e.type,
                Valor = e.valuePoints
            }));

            client.Dispose();

            return extrato;
        }

        public async Task<double> ObterSaldoSimplesAsync()
        {
            HttpClient client = new HttpClient();
            var token = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.First(c => c.Type == "ApiMktpToken").Value;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);

            var response = await client.GetAsync(string.Concat(_urlApiMktp, "participants/me/simpleBalance"));

            var responseString = await response.Content.ReadAsStringAsync();
            var saldoResponse = new { pointsValue = 0.0 };

            saldoResponse = JsonConvert.DeserializeAnonymousType(responseString, saldoResponse);

            client.Dispose();

            return saldoResponse.pointsValue;
        }
    }

}
