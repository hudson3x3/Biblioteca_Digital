using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Text;
using GrupoLTM.WebSmart.DTO.Avon;
using GrupoLTM.WebSmart.DTO.MarketPlace;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using GrupoLTM.WebSmart.Infrastructure.Helpers;
using System.Linq;

namespace GrupoLTM.WebSmart.Services.Common
{
    public static class CommonService
    {
        public static AvonUserInfoData GetUserInfoAsync(string url, AvonIdentityToken avonToken)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Headers.Add("WCTrustedToken", avonToken.WCTrustedToken);
            request.Headers.Add("WCToken", avonToken.WCToken);

            var avonResponseProfile = new AvonUserInfoData();

            try
            {
                var webResponse = request.GetResponse();
                var webStream = webResponse.GetResponseStream();
                var responseReader = new StreamReader(webStream);
                var response = responseReader.ReadToEnd();

                if (!string.IsNullOrEmpty(response))
                {
                    var resultReturn = JsonConvert.DeserializeObject<AvonResponseUserInfo>(response);

                    if (resultReturn.header.error.message == "SUCCESS" && resultReturn.data.representatives.Count > 0)
                    {
                        var listAddress = new List<AvonAddress>();

                        dynamic resultData = JsonConvert.DeserializeObject<object>(response);
                        avonResponseProfile = resultReturn.data;

                        var avonDataParticipant = new AvonDataParticipant();

                        avonDataParticipant = resultReturn.data.representatives[0].profileAvon;

                        avonDataParticipant.accountType.accountTyp = resultData.data.representatives[0].profileAvon.accountType.accountTyp;
                        avonDataParticipant.currSlsCmpgnNr = resultData.data.representatives[0].profileAvon.currentCampaingNumber == null ? 0 : resultData.data.representatives[0].profileAvon.currentCampaingNumber;
                        avonDataParticipant.currSlsYrNr = resultData.data.representatives[0].profileAvon.currentCampaingYear == null ? 0 : resultData.data.representatives[0].profileAvon.currentCampaingYear;
                        avonDataParticipant.acctNr = resultData.data.representatives[0].profileAvon.accountNumber;

                        avonDataParticipant.frstNm = resultData.data.representatives[0].profilePersonal.firstName;
                        avonDataParticipant.lastNm = resultData.data.representatives[0].profilePersonal.lastName;
                        avonDataParticipant.gendrCd = resultData.data.representatives[0].profilePersonal.gender;
                        avonDataParticipant.emailAddrTxt = resultData.data.representatives[0].profilePersonal.email;
                        avonDataParticipant.brthdyDt = resultData.data.representatives[0].profilePersonal.dateOfBirth;

                        for (int i = 0; i < resultData.data.representatives[0].phones.Count; i++)
                        {
                            var typeDesc = resultData.data.representatives[0].phones[i].phoneTypeCD.ToString();
                            if (typeDesc == "2" &&
                                (string.IsNullOrWhiteSpace(avonDataParticipant.evngPhonStdCd) ||
                                string.IsNullOrWhiteSpace(avonDataParticipant.evngPhonNr)))
                            {
                                avonDataParticipant.evngPhonStdCd = resultData.data.representatives[0].phones[i].phoneLongDistanceCode;
                                avonDataParticipant.evngPhonNr = resultData.data.representatives[0].phones[i].phoneNumber;
                            }

                            if (typeDesc == "3" &&
                                (string.IsNullOrWhiteSpace(avonDataParticipant.mobilePhonStdCd) ||
                                string.IsNullOrWhiteSpace(avonDataParticipant.mobilePhonNr)))
                            {
                                avonDataParticipant.mobilePhonStdCd = resultData.data.representatives[0].phones[i].phoneLongDistanceCode;
                                avonDataParticipant.mobilePhonNr = resultData.data.representatives[0].phones[i].phoneNumber;
                            }
                        }

                        if (resultData.data.representatives[0].address.deliveryAddress != null)
                        {
                            var addressDelivery = new AvonAddress
                            {
                                addrLocTyp = "DELIVERY",
                                addrCntryCd = resultData.data.representatives[0].address.deliveryAddress.addressCountry.ToString(),
                                addrLine2Txt = resultData.data.representatives[0].address.deliveryAddress.addressLine1.ToString(),
                                addrZipCd = resultData.data.representatives[0].address.deliveryAddress.addressZipCode.ToString(),
                                addrLine7Txt = resultData.data.representatives[0].address.deliveryAddress.addressLine2.ToString(),
                                addrLine4Txt = resultData.data.representatives[0].address.deliveryAddress.addressLine3.ToString(),
                                addrStCd = resultData.data.representatives[0].address.deliveryAddress.addressState.ToString(),
                                addrCityNm = resultData.data.representatives[0].address.deliveryAddress.addressCity.ToString()
                            };
                            listAddress.Add(addressDelivery);
                        }

                        if (resultData.data.representatives[0].address.homeAddress != null)
                        {
                            var addressHome = new AvonAddress
                            {
                                addrLocTyp = "HOME",
                                addrCntryCd = (resultData.data.representatives[0].address.homeAddress.addressCountry != null) ? resultData.data.representatives[0].address.homeAddress.addressCountry.ToString() : "",
                                addrLine2Txt = (resultData.data.representatives[0].address.homeAddress.addressLine1 != null) ? resultData.data.representatives[0].address.homeAddress.addressLine1.ToString() : "",
                                addrZipCd = (resultData.data.representatives[0].address.homeAddress.addressZipCode != null) ? resultData.data.representatives[0].address.homeAddress.addressZipCode.ToString() : "",
                                addrLine7Txt = (resultData.data.representatives[0].address.homeAddress.addressLine2 != null) ? resultData.data.representatives[0].address.homeAddress.addressLine2.ToString() : "",
                                addrLine4Txt = (resultData.data.representatives[0].address.homeAddress.addressLine3 != null) ? resultData.data.representatives[0].address.homeAddress.addressLine3.ToString() : "",
                                addrStCd = (resultData.data.representatives[0].address.homeAddress.addressState != null) ? resultData.data.representatives[0].address.homeAddress.addressState.ToString() : "",
                                addrCityNm = (resultData.data.representatives[0].address.homeAddress.addressCity != null) ? resultData.data.representatives[0].address.homeAddress.addressCity.ToString() : ""
                            };

                            listAddress.Add(addressHome);
                        }

                        avonDataParticipant.addresses = listAddress;
                        avonDataParticipant.prsnlIdntfctnTxt = resultData.data.representatives[0].profilePersonal.cpfNumber;
                        avonDataParticipant.fldLvlCd = resultData.data.representatives[0].profilePersonal.zone;

                        responseReader.Close();

                        avonResponseProfile.representatives[0].profileAvon = avonDataParticipant;
                    }
                    else
                    {
                        var errorMessage = (string)resultReturn.header.error.message;

                        if (errorMessage == "No record found")
                            errorMessage = "Não foi possível obter as informações do participante";

                        avonResponseProfile.ErrorMessage = errorMessage;

                        return avonResponseProfile;
                    }
                }
            }
            catch (Exception ex)
            {
                var mensagem = string.Join(" => ", Helper.GetInnerExceptions(ex).Select(x => x.Message));

                var logErro = new Domain.Models.LogErro
                {
                    Erro = ex.StackTrace,
                    Mensagem = "Erro ao obter as informações do participante Avon - " + mensagem,
                    Source = ex.Source,
                    Metodo = "GetUserInfoAsync",
                    Controller = "CommonService",
                    Pagina = url,
                    Codigo = string.Empty
                };

                var logErroService = new LogErroService();

                logErroService.SalvarLogErro(logErro);

                throw new ApplicationException("Não foi possível consultar " + url, new Exception(ex.Message));
            }

            return avonResponseProfile;
        }

        public static AvonIdentityToken GetTokenAsync(string url, AvonRequestLoginIdentity avonRequest)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(url);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var jsonContent = JsonConvert.SerializeObject(avonRequest);

                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    content.Headers.Add("logonId", avonRequest.logonId);
                    content.Headers.Add("logonPassword", avonRequest.logonPassword);

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11;

                    var response = httpClient.PostAsync(url, content).Result;

                    if (response == null || !response.IsSuccessStatusCode)
                        throw new Exception($"A requisição de token avon falhou, status: {(int)response.StatusCode} - {response.StatusCode}");

                    var stream = response.Content.ReadAsStreamAsync().Result;

                    var jsonString = string.Empty;

                    using (var reader = new StreamReader(new GZipStream(stream, CompressionMode.Decompress)))
                        jsonString = reader.ReadToEnd();

                    var token = JsonConvert.DeserializeObject<AvonIdentityToken>(jsonString);

                    return token;
                }
            }
            catch (Exception ex)
            {
                var erros = string.Join(" => ", Helper.GetInnerExceptions(ex).Select(x => x.Message));

                var message = "Não foi possível obter o token Avon";

                var logErro = new Domain.Models.LogErro
                {
                    Erro = ex.StackTrace,
                    Mensagem = $"{message} - {erros}",
                    Source = ex.Source,
                    Metodo = "GetTokenAsync",
                    Controller = "CommonService",
                    Pagina = url,
                    Codigo = ex.GetType().ToString()
                };

                var logErroService = new LogErroService();
                logErroService.SalvarLogErro(logErro);

                throw new Exception(message, ex);
            }
        }

        public static MarketPlaceResponseToken GetTokenAsync(HttpContent content, string mktPlaceAppId, string mktPlaceTokenCatalog)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", mktPlaceAppId);
                    var request = new HttpRequestMessage()
                    {
                        RequestUri = new Uri(mktPlaceTokenCatalog),
                        Method = HttpMethod.Post,
                        Content = content
                    };

                    ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                    var response = client.SendAsync(request).Result;
                    var outputDataJson = response.Content.ReadAsStringAsync().Result;

                    if (response.IsSuccessStatusCode)
                    {

                        var resultReturn = JsonConvert.DeserializeObject<MarketPlaceResponseToken>(outputDataJson);
                        return resultReturn;
                    }
                    else
                        throw new ApplicationException("Não foi possível obter o Token, tente novamente mais tarde. Response:" + outputDataJson, new Exception(outputDataJson));
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Exceção: Não foi possível obter o Token, tente novamente mais tarde. Exception:" + ex.Message, new Exception(ex.Message));
            }
        }

        public static bool ValidateMD5(string encriptedKey, string inputKey)
        {
            var token = GenerateHash(inputKey);

            return encriptedKey == token;
        }

        public static string GenerateHash(string input)
        {
            MD5 md5 = MD5.Create();

            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
                sb.Append(hash[i].ToString("X2"));

            return sb.ToString();
        }
    }
}
