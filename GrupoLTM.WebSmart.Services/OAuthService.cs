using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Services.Log;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace GrupoLTM.WebSmart.Services
{
    public class OAuthService
    {
        public static string GetToken(AvonAuthentication avonAuthentication)
        {
            var basicToken = ConfiguracaoService.OAuthAppId();
            var urlToken = ConfiguracaoService.OAuthUrl();
            var apiManSubKeyToken = ConfiguracaoService.ApiManSubscriptionKey();
            var username = ConfiguracaoService.UsernameToken();
            var password = ConfiguracaoService.PasswordToken();

            try
            {
                using (var client = new HttpClient())
                {
                    var accountNumber = avonAuthentication.AccountNumber.ToString();
                    var catalogId = avonAuthentication.CatalogId.ToString();
                    var pageName = avonAuthentication.PageName.ToString();
                    var userAdminId = avonAuthentication.i.ToString();
                    var redirectUrlMktplace = avonAuthentication.redirectUrlMktplace != null ? avonAuthentication.redirectUrlMktplace.ToString() : "";

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicToken);
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiManSubKeyToken);

                    var request = new HttpRequestMessage()
                    {
                        RequestUri = new Uri(urlToken),
                        Method = HttpMethod.Post,
                        Content = new FormUrlEncodedContent(new Dictionary<String, String>
                        {
                            {"grant_type", "password"},
                            {"accountNumber", accountNumber },
                            {"catalogId", catalogId},
                            {"pageName", pageName},
                            {"userAdminId", userAdminId},
                            {"redirectUrlMktplace", redirectUrlMktplace},
                            {"username", username},
                            {"password", password},
                        }),
                    };

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    var response = client.SendAsync(request).Result;
                    var result = response.Content.ReadAsStringAsync().Result;

                    string access_token;

                    if (response.IsSuccessStatusCode)
                    {
                        var tokenObject = JsonConvert.DeserializeObject<Model.Token>(result);
                        access_token = tokenObject.AccessToken;
                    }
                    else
                    {
                        throw new Exception("Não foi possível gerar o token OAuth MMA: " + result);
                    }

                    return access_token;
                }
            }
            catch (Exception ex)
            {
                var info = new
                {
                    basicToken,
                    urlToken,
                    apiManSubKeyToken,
                    username,
                    password,
                };

                var log = new LogControllerModel
                {
                    Error = ex.Message,
                    Date = DateTime.Now,
                    Class = "OAuthService",
                    Method = "GetToken",
                    StackTrace = ex.StackTrace,
                    Source = ex.ToString(),
                    Data = info
                };

                //Update DataDog
                //GrayLogService.LogError(log);

                return null;
            }
        }

        public static string CheckParticipant(string accountNumber, int? i, long mktPlaceCatalogoId, string tk, out bool hasInvalidEmail)
        {
            hasInvalidEmail = false;

            //get Participant - pre-cadastro marketplace            
            string urlapi = ConfiguracaoService.ApiAvonUrl() + "/participant" + "?accountNumber=" + accountNumber.ToString() + "&mktPlaceCatalogoId=" + mktPlaceCatalogoId.ToString() + (i == null ? "" : "&userAdminId=" + i.ToString());
            var access_token = "";
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tk);
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", ConfiguracaoService.ApiManSubscriptionKey());

                    Console.WriteLine("29. token:" + tk);
                    var request = new HttpRequestMessage()
                    {
                        RequestUri = new Uri(urlapi),
                        Method = HttpMethod.Get
                    };

                    client.Timeout = TimeSpan.FromMinutes(5);

                    var response = client.SendAsync(request).Result;
                    Console.WriteLine("30. participant:" + response);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadAsStringAsync().Result;

                        try
                        {
                            var tokenObject = JsonConvert.DeserializeObject<Model.TokenType>(result);
                            access_token = tokenObject.AccessToken;
                            hasInvalidEmail = tokenObject.HasInvalidEmail;
                            Console.WriteLine("31. access_token:" + access_token);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("33. Erro OAUthService.CheckParticipant:" + ex.Message);
                            return ("Ocorreu um erro na autenticação do Catálogo:" + ex.Message + " : " + result);
                            throw ex;
                        }
                    }
                    else
                    {
                        var result = response.Content.ReadAsStringAsync().Result;
                        Console.WriteLine("34. Não foi possível gerar o token OAuth MMA: " + result);
                        return result;
                    }
                }
                Console.WriteLine("[Services.FirstAccessParticipant] OAuthService.CheckParticipant: access_token =" + access_token.ToString());

                return access_token != "" ? "" : "Ocorreu um erro no Catálogo";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
