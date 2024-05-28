using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Models.MktPlace;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using GrupoLTM.WebSmart.DTO;
using GrupoLTM.WebSmart.Services.Common;
using GrupoLTM.WebSmart.DTO.MarketPlace;
using GrupoLTM.WebSmart.Infrastructure.Exceptions;
using GrupoLTM.WebSmart.Services.JwtAuth;
using System.Threading.Tasks;
using GrupoLTM.WebSmart.Infrastructure.Cache;
using System.Diagnostics;

namespace GrupoLTM.WebSmart.Services
{
    public class MarketPlaceService
    {
        private static readonly CatalogoService _catalogoService = new CatalogoService();
        private static readonly CognitoService _cognitoService = new CognitoService();

        public static string GetUrlCatalog(int accountNumber, long mktPlaceParticipantId, long mktPlaceCatalogoId)
        {
            var _catalogo = _catalogoService.ObterCatalogo(mktPlaceCatalogoId);

            if (_catalogo == null)
                throw new ApplicationException("Não foi possível localizar as informações do Catálogo");

            string mktPlaceUrlCatalog = _catalogo.Autor;

            return mktPlaceUrlCatalog;
        }

        public static string GetTokenContext(int accountNumber, long mktPlaceParticipantId, long mktPlaceCatalogoId, int? userAdminId = null)
        {
            try
            {
                var _catalogo = _catalogoService.ObterCatalogoContext(mktPlaceCatalogoId);
                var AppIdMktPlace = userAdminId > 0 ? _catalogo.Autor : _catalogo.Autor;

                if (_catalogo == null)
                    throw new ApplicationException("Não foi possível localizar as informações do Catálogo");

                //string mktPlaceUrlCatalog = _catalogo.UrlCatalogMktPlace;

                var mktPlaceToken = GetTokenContext(accountNumber.ToString(), mktPlaceParticipantId, AppIdMktPlace, userAdminId);
                string mktPlaceTokenFormat = JsonConvert.SerializeObject(mktPlaceToken);

                return mktPlaceTokenFormat;
            }
            catch (Exception ex)
            {
                gravaLogErro("Erro ao tentar gerar o token context", ex.Message, "GrupoLTM.WebSmart.Services", string.Format("GetTokenContext(accountNumber {0},mktPlaceParticipantId {1},mktPlaceCatalogoId {2},userAdminId:{3})", accountNumber.ToString(), mktPlaceParticipantId.ToString(), mktPlaceCatalogoId.ToString(), userAdminId == null ? "" : userAdminId.ToString()), "logCatalog");
                return "";
            }
        }

        public static dynamic InsertOrUpdateParticipant(Participant participant, string accessTokenMktPlace, string cpAvon, long mktPlaceCatalogoId, string appId, bool isUpdate = false, int? userAdminId = null)
        {
            var mktPlaceParticipantInsert = ConfiguracaoService.MktPlaceParticipantInsert();

            if (isUpdate)
                mktPlaceParticipantInsert += "/" + participant.ParticipantId.ToString();

            //Carrega Participante com informações base do MarkePlace
            participant.Address[0].Complement = string.IsNullOrEmpty(participant.Address[0].Complement)
                ? "Não informado"
                : participant.Address[0].Complement;

            using (var client = new HttpClient())
            {
                string participantJson = JsonConvert.SerializeObject(participant);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessTokenMktPlace);

                var tokenObject =
                    client.PostAsync(mktPlaceParticipantInsert,
                        new StringContent(participantJson, Encoding.UTF8, "application/json")).Result;

                var result = tokenObject.Content.ReadAsStringAsync().Result;

                try
                {
                    var retorno = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(result);
                    return retorno;
                }
                catch (Exception ex)
                {
                    throw new MarketPlaceException(ex.Message + " : " + result);
                }

            }
        }

        public static async Task<Model.ProductSku> GetProductSku(string productSku, string accountNumber, int mktPlaceParticipantId, int mktPlaceCatalogoId)
        {
            var participant = new Participant()
            {
                Username = accountNumber,
                ParticipantId = mktPlaceParticipantId,
                CatalogId = mktPlaceCatalogoId
            };

            if (participant.ParticipantId != 0)
                participant.CampaignId = new CatalogoCPService().ObterParticipanteCatalogo(mktPlaceParticipantId);

            string accessTokenMktPlace = await _cognitoService.SignIn();
            string mktPlaceGetProductSku = ConfiguracaoService.MktPlaceGetProductSku();
            string subscriptionKey = ConfiguracaoService.ApiManSubscriptionKey();

            mktPlaceGetProductSku = mktPlaceGetProductSku.Replace("{productSku}", productSku.Trim());

            using (var client = new HttpClient())
            {
                //Efetua Get
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessTokenMktPlace);
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                client.DefaultRequestHeaders.Add("customer_id", "306");
                client.DefaultRequestHeaders.Add("catalog_id", mktPlaceCatalogoId.ToString());
                var tokenObject = client.GetAsync(mktPlaceGetProductSku);
                var result = "";
                try
                {
                    if ((int)tokenObject.Result.StatusCode == 200)
                    {
                        result = tokenObject.Result.Content.ReadAsStringAsync().Result;
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<Model.ProductSku>(result);
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (MarketPlaceException ex)
                {
                    throw new MarketPlaceException(ex.Message + " : " + result);
                }
            }
        }

        public static async Task<Model.ProductSku> GetProductSkuByOrignalProductSku(string originalProductSku, string accountNumber, int mktPlaceParticipantId, int mktPlaceCatalogoId, int mktPlaceSupplierId)
        {
            var participant = new Participant()
            {
                Username = accountNumber,
                ParticipantId = mktPlaceParticipantId,
                CatalogId = mktPlaceCatalogoId
            };

            if (participant.ParticipantId != 0)
                participant.CampaignId = new CatalogoCPService().ObterParticipanteCatalogo(mktPlaceParticipantId);

            //var _catalogo = _catalogoService.ObterCatalogo(mktPlaceCatalogoId);

            //if (_catalogo == null)
            //    throw new ApplicationException("Não foi possível localizar as informações do Catálogo");

            string accessTokenMktPlace = await _cognitoService.SignIn();

            string mktPlaceGetOriginalProductSku = ConfiguracaoService.MktPlaceGetOriginalProductSku();
            string subscriptionKey = ConfiguracaoService.ApiManSubscriptionKey();

            mktPlaceGetOriginalProductSku = mktPlaceGetOriginalProductSku.Replace("{originalproductskuid}", originalProductSku.Trim()).Replace("{vendorid}", mktPlaceSupplierId.ToString());

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessTokenMktPlace);
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                var tokenObject = client.GetAsync(mktPlaceGetOriginalProductSku);
                var result = "";

                try
                {
                    if ((int)tokenObject.Result.StatusCode == 200)
                    {
                        result = tokenObject.Result.Content.ReadAsStringAsync().Result;
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<Model.ProductSku>(result);
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (MarketPlaceException ex)
                {
                    throw new MarketPlaceException(ex.Message + " : " + result);
                }
            }
        }

        public static async Task<dynamic> SearchParticipant(string login, string accessToken, int campaingID, string appID, int? userAdminId = null)
        {
            if (string.IsNullOrEmpty(accessToken))
                accessToken = await _cognitoService.SignIn();

            string mktPlaceParticipantSearch = string.Concat(ConfiguracaoService.MktPlaceParticipantInsert(), "/search");

            HttpClient client;

            using (client = new HttpClient())
            {
                string participantJson = JsonConvert.SerializeObject(new ParticipanteSearchModel
                {
                    CampaignId = campaingID.ToString(),
                    Login = login
                });
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var tokenObject =
                    client.PostAsync(mktPlaceParticipantSearch,
                        new StringContent(participantJson, Encoding.UTF8, "application/json")).Result;

                Debug.WriteLine(tokenObject);

                var result = tokenObject.Content.ReadAsStringAsync().Result;

                Debug.WriteLine(result);

                try
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(result);
                }
                catch (Exception ex)
                {
                    throw new MarketPlaceException(ex.Message + " : " + result);
                }
            }
        }

        /// <summary>
        /// Carrega Participante com informações base do MarkePlace
        /// </summary>
        public static Participant SetBaseInformation(Participant participant, string cpAvon, long mktPlaceCatalogoId)
        {
            var catalogoCPService = new CatalogoCPService().ObterCatalogoCP(cpAvon, mktPlaceCatalogoId);

            if (catalogoCPService == null || catalogoCPService.Catalogo == null)
                throw new InvalidOperationException(string.Format("Catálogo não encontrado. mktPlaceCatalogoId '{0}', cpAvon '{1}'.", mktPlaceCatalogoId, cpAvon));

            //participant.CampaignId = catalogoCPService.Catalogo.IdCampanha;
            participant.ProfileId = catalogoCPService.ProfileId;
            participant.ClientId = ConfiguracaoService.MktPlaceClientId();

            return participant;
        }

        public static Participant SetProfileInformation(Participant participant, string cpAvon, long mktPlaceCatalogoId)
        {
            var catalogoCPService = new CatalogoCPService().ObterCatalogoCP(cpAvon, mktPlaceCatalogoId);

            if (catalogoCPService == null || catalogoCPService.Catalogo == null)
                throw new InvalidOperationException(string.Format("Catálogo não encontrado. mktPlaceCatalogoId '{0}', cpAvon '{1}'.", mktPlaceCatalogoId, cpAvon));

            participant.ProfileId = catalogoCPService.ProfileId;

            return participant;
        }

        #region Private Methods

        /// <summary>
        /// Com consulta no banco por catalogoId
        /// </summary>
        //public static MarketPlaceResponseToken GetToken(string username, long participantIdMktPlace, int mktPlaceCatalogoId)
        //{
        //    var catalogo = new CatalogoService().ObterCatalogo(mktPlaceCatalogoId);
        //    return GetToken(username, participantIdMktPlace, catalogo.AppIdMktPlaceAdmin);
        //}

        /// <summary>
        /// Sem consulta no banco com appID já consultado anteriormente
        /// </summary>
        //public static MarketPlaceResponseToken GetToken(string username, long participantIdMktPlace, string appID, int? userAdminId = null)
        //{
        //    var participant = new Participant()
        //    {
        //        Username = username,
        //        ParticipantId = participantIdMktPlace
        //    };
        //
        //    if (participant.ParticipantId != 0)
        //        participant.CampaignId = new CatalogoCPService().ObterParticipanteCatalogo(participantIdMktPlace);
        //
        //    return GetToken(participant, participant.CampaignId, appID, userAdminId);
        //}
        /// <summary>
        /// Sem consulta no banco com appID já consultado anteriormente
        /// </summary>
        public static MarketPlaceResponseToken GetTokenContext(string username, long participantIdMktPlace, string appID, int? userAdminId = null)
        {
            var participant = new Participant()
            {
                Username = username,
                ParticipantId = participantIdMktPlace
            };

            if (participant.ParticipantId != 0)
                participant.CampaignId = new CatalogoCPService().ObterParticipanteCatalogo(participantIdMktPlace);

            return GetTokenContext(participant, participant.CampaignId, appID, userAdminId);
        }

        /// <summary>
        /// Gera token do MarketPlace
        /// </summary>
        //public static MarketPlaceResponseToken GetToken(Participant Participant, long campaingID, string appID, int? userAdminId)
        //{
        //    long cacheKey = long.Parse(String.Format("{0}{1}", Participant.ParticipantId, Participant.CatalogId));
        //    var mktPlaceTokenCatalog = ConfiguracaoService.MktPlaceTokenCatalog();
        //
        //    // Busca Token no Cache
        //    var token = WebCache.GetCache<MarketPlaceResponseToken>("Token", cacheKey);
        //
        //    if (token != null)
        //        return token;
        //
        //    return GetTokenContext(Participant, campaingID, appID, userAdminId);
        //}
        //
        /// <summary>
        /// Gera token do MarketPlace
        /// </summary>
        public static MarketPlaceResponseToken GetTokenContext(Participant Participant, long campaingID, string appID, int? userAdminId)
        {
            try
            {
                long cacheKey = long.Parse(String.Format("{0}{1}", Participant.ParticipantId, Participant.CatalogId));
                var mktPlaceTokenCatalog = ConfiguracaoService.MktPlaceTokenCatalog();

                //tipo TOKEN
                HttpContent content;
                if (Participant.ParticipantId == 0)
                {
                    if (userAdminId > 0)
                    {
                        content = new FormUrlEncodedContent(new Dictionary<String, String>
                    {
                        {"grant_type", "client_credentials"},
                        {"campaign_Id", campaingID.ToString()},
                        {"impersonator_user", userAdminId > 0 ? userAdminId.ToString() : "0"}
                    });
                    }
                    else
                    {
                        content = new FormUrlEncodedContent(new Dictionary<String, String>
                    {
                        {"grant_type", "client_credentials"},
                        {"campaign_Id", campaingID.ToString()}
                    });
                    }
                }
                else
                {
                    if (userAdminId > 0)
                    {
                        content = new FormUrlEncodedContent(new Dictionary<String, String>
                    {
                        {"grant_type", "password"},
                        {"campaign_Id", Participant.CampaignId.ToString()},
                        {"username", Participant.ParticipantId.ToString()},
                        {"impersonator_user", userAdminId > 0 ? userAdminId.ToString() : "0"}
                    });
                    }
                    else
                    {
                        content = new FormUrlEncodedContent(new Dictionary<String, String>
                    {
                        {"grant_type", "password"},
                        {"campaign_Id", Participant.CampaignId.ToString()},
                        {"username", Participant.ParticipantId.ToString()}
                    });
                    }
                }

                string mktPlaceAppId = appID;

                var tokenObject = CommonService.GetTokenAsync(content, mktPlaceAppId, mktPlaceTokenCatalog);

                if (!String.IsNullOrEmpty(tokenObject.access_token))
                {
                    return tokenObject;
                }
                else
                {
                    gravaLogErro("Erro ao tentar gerar o token context", "Não foi possível Localizar o Usuário na Avon", "GrupoLTM.WebSmart.Services", string.Format("GetTokenContext(Participant: {0},campaingID: {1},appID: {2},userAdminId: {3})", Participant.Username.ToString(), campaingID.ToString(), appID.ToString(), userAdminId.ToString()), "logCatalog");
                    throw new MarketPlaceException("Não foi possível Localizar o Usuário na Avon");
                }
            }
            catch (Exception ex)
            {
                gravaLogErro("Erro ao tentar gerar o token context", ex.Message, "GrupoLTM.WebSmart.Services", string.Format("GetTokenContext(Participant: {0},campaingID: {1},appID: {2},userAdminId: {3})", Participant.Username.ToString(), campaingID.ToString(), appID.ToString(), userAdminId.ToString()), "logCatalog");
                return null;
            }
        }
        #endregion

        #region privateMetods

        private static void gravaLogErro(string Erro, string Mensagem, string Source, string Metodo, string Codigo)
        {
            var logErro = new LogErro()
            {
                Erro = Erro,
                Mensagem = Mensagem,
                Source = Source,
                Metodo = Metodo,
                Controller = "MarketPlaceService",
                Pagina = string.Empty,
                Codigo = Codigo
            };

            var logErroService = new LogErroService();
            logErroService.SalvarLogErro(logErro);
        }

        #endregion
    }
}
