using GrupoLTM.WebSmart.Infrastructure.Exceptions;
using GrupoLTM.WebSmart.Services.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace GrupoLTM.WebSmart.Services.JwtAuth
{
    public class CognitoService
    {
        public async Task<string> SignIn()
        {
            try
            {
                var CognitoClientId = ConfigurationManager.AppSettings["CognitoClientId"];
                var CognitoScope = ConfigurationManager.AppSettings["CognitoScope"];
                var CognitoBasicAuth = ConfigurationManager.AppSettings["CognitoBasicAuth"];
                var CognitoUrlBase = new Uri(ConfigurationManager.AppSettings["CognitoUrlBase"]);
                var CognitoPathAuth = ConfigurationManager.AppSettings["CognitoPathAuth"];

                var requestContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("client_id", CognitoClientId),
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("scope", $"{CognitoScope}")
            });

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{CognitoUrlBase}{CognitoPathAuth}")
                {
                    Content = requestContent
                };

                using (var client = new HttpClient { BaseAddress = CognitoUrlBase })
                {
                    client.DefaultRequestHeaders.Add("Authorization", CognitoBasicAuth);

                    ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                    var response = await client.SendAsync(requestMessage);

                    if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.OK)
                    {
                        var result = JsonConvert.DeserializeObject<TokenCognito>(await response.Content.ReadAsStringAsync());
                        return result.AccessToken;
                    }
                    else
                        throw new Exception("O request do token SSO retornou um status inválido, status: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                throw new MarketPlaceException("Não foi possível obter o token SSO", ex);
            }
        }

        public async Task<string> SignInCatalogo()
        {
            var ssoClientId = ConfigurationManager.AppSettings["SsoClientId"];
            var ssoScope = ConfigurationManager.AppSettings["SsoScope"];
            var ssoSecret = ConfigurationManager.AppSettings["SsoSecret"];
            var ssoUrlBase = ConfigurationManager.AppSettings["SsoUrlBase"];
            var ssoPathAuth = ConfigurationManager.AppSettings["SsoPathAuth"];

            var requestContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("client_id", ssoClientId),
                new KeyValuePair<string, string>("client_secret", ssoSecret),
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("scope", ssoScope)
            });

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ssoUrlBase + ssoPathAuth)
            {
                Content = requestContent
            };

            using (var client = new HttpClient { BaseAddress = new Uri(ssoUrlBase) })
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                var response = await client.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<TokenCognito>(content);

                    return result.AccessToken;
                }

                return null;
            }
        }
    }
}