using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repositories;
using GrupoLTM.WebSmart.Services.Common;
using System;
using System.Linq;
using System.Web;
using GrupoLTM.WebSmart.DTO.Avon;
using static GrupoLTM.WebSmart.Domain.Enums.EnumDomain;
using GrupoLTM.WebSmart.Infrastructure.Cache;
using GrupoLTM.WebSmart.Domain.Models.MktPlace;
using Email = GrupoLTM.WebSmart.Infrastructure.Mail.Email;
using GrupoLTM.WebSmart.Infrastructure.Helpers;

namespace GrupoLTM.WebSmart.Services
{
    public class AvonService
    {
        public static bool ValidateMD5(AvonAuthentication avonAuthentication)
        {
            var accountNumber = avonAuthentication.AccountNumber;
            var encriptedKey = avonAuthentication.EncryptedKey;
            int userAdminId = avonAuthentication.i == null ? 0 : Convert.ToInt32(avonAuthentication.i);
            var inputkey = "";

            if (userAdminId == 0)
                inputkey = string.Format("{0}{1}", accountNumber, ConfiguracaoService.GetMD5keyLTM());
            else
                inputkey = string.Format("{0}{1}{2}", accountNumber, ConfiguracaoService.GetMD5keyLTM(), userAdminId.ToString());
            return CommonService.ValidateMD5(encriptedKey, inputkey);
        }

        public static Catalogo GetCatalogoUserInfo(AvonUserInfoDataRepresentatives userAvon)
        {
            try
            {
                var catalogoService = new CatalogoService();

                Catalogo catalogo = null;

                short profile;

                if (!string.IsNullOrEmpty(userAvon.profileAvon.repPrftblty?.repPrftbltyTyp) && short.TryParse(userAvon.profileAvon.repPrftblty.repPrftbltyTyp, out profile))
                {
                    catalogo = catalogoService.ObterCatalogoPorPerfilParticipante(profile);
                }
                else if (userAvon.profileAvon.repClubLevel != null)
                {
                    switch (userAvon.profileAvon.repClubLevel.repclubLevelCD)
                    {
                        case "C" when userAvon.profileAvon.isRepVip:
                                catalogo = catalogoService.ObterCatalogoPorPerfilParticipante((int)ClassificacaoPerfilAvon.QuatroEstrelas);
                            break;
                        case "P":
                            catalogo = catalogoService.ObterCatalogoPorPerfilParticipante((int)ClassificacaoPerfilAvon.CincoEstrelas);
                            break;
                        default:
                            break;
                    }
                }

                return catalogo;
            }
            catch (Exception ex)
            {
                var logErro = new LogErro
                {
                    Erro = ex.StackTrace,
                    Mensagem = ex.Message + " - GetCatalogoUserInfo",
                    Source = ex.Source,
                    Metodo = "GetCatalogoUserInfo",
                    Controller = "AvonService",
                    Pagina = HttpContext.Current.Request.Url.ToString(),
                    Codigo = string.Empty
                };

                var logErroService = new LogErroService();
                logErroService.SalvarLogErro(logErro);

                return null;
            }
        }

        public static AvonUserInfoDataRepresentatives GetNewUserInfo(long accountNumberAvon, string admin)
        {
            try
            {
                var chave = "GetUserInfo_";

                var _avonUserInfo = ObterAvonUserInfoDataRepresentativesCache(accountNumberAvon.ToString(), chave);

                if (_avonUserInfo != null)
                {
                    return _avonUserInfo;
                }
                else
                {
                    var logonIdAvon = ConfiguracaoService.LogonId();
                    var logonPasswordAvon = ConfiguracaoService.LogonPassword();
                    var urlTokenAvon = ConfiguracaoService.UrlNewTokenAvon();
                    var urlUserInfo = ConfiguracaoService.UrlNewGetProfileAvon() + $"?acctNr={accountNumberAvon}&langCd=pt_BR";
                    var profile = new AvonUserInfoData();

                    var avonToken = new AvonRequestLoginIdentity
                    {
                        logonId = logonIdAvon,
                        logonPassword = logonPasswordAvon
                    };

                    var tokenObject = CommonService.GetTokenAsync(urlTokenAvon, avonToken);

                    if (tokenObject != null)
                        profile = CommonService.GetUserInfoAsync(urlUserInfo, tokenObject);

                    if (profile.representatives?.FirstOrDefault() != null)
                    {
                        LimparAvonUserInfoDataRepresentativesCache(accountNumberAvon.ToString(), chave);
                        GravarAvonUserInfoDataRepresentativesCache(accountNumberAvon.ToString(), chave, profile.representatives.FirstOrDefault());
                    }
                    else
                    {
                        return new AvonUserInfoDataRepresentatives { ErrorMessage = profile.ErrorMessage };
                    }

                    return profile.representatives.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                //TODO NewLog

                //var mensagem = string.Join(" => ", Helper.GetInnerExceptions(ex).Select(x => x.Message));

                //var logErro = new LogErro
                //{
                //    Erro = ex.StackTrace,
                //    Mensagem = $"GetNewUserInfo {accountNumberAvon} - {admin} - Message: {mensagem}",
                //    Source = ex.Source,
                //    Metodo = "GetNewUserInfo",
                //    Controller = "OAuthService",
                //    Pagina = HttpContext.Current.Request.Url.ToString(),
                //    Codigo = ex.GetType().ToString()
                //};

                //new LogErroService().SalvarLogErro(logErro);

                var showException = ex;

                while (showException.InnerException != null)
                    showException = showException.InnerException;

                throw new ApplicationException(showException.Message);
            }
        }

        public static bool AtualizarParticipanteMarketplace(AvonUserInfoDataRepresentatives userAvon, string accountNumber, int? idAdmin)
        {
            var hasInvalidEmail = false;

            try
            {
                var primeiroAcessoService = new PrimeiroAcessoService();
                var _catalogoService = new CatalogoService();
                var _ParticipantRepository = new ParticipantRepository();

                var participanteCatalogo = primeiroAcessoService.ObterParticipanteCatalogo(Convert.ToInt64(accountNumber), idAdmin, out var catalogId);

                var mktPlaceCatalogoId = _catalogoService.ObterCatalogoContext(catalogId).Codigo;

                var participante = _ParticipantRepository.GetAllByLogin(accountNumber);

                var atualizarEmail = false;

                if (userAvon.profileAvon.emailAddrTxt != null)
                {
                    atualizarEmail = !Email.ValidarEmail(userAvon.profileAvon.emailAddrTxt);

                    if (!atualizarEmail)
                    {
                        atualizarEmail = participante.Email != userAvon.profileAvon.emailAddrTxt;
                    }
                }

                var atualizarCP = false;

                var CPAVON = userAvon.profileAvon.currSlsCmpgnNr + "/" + userAvon.profileAvon.currSlsYrNr;

                var catalogoCP = new CatalogoCPService().ObterCatalogoCP(CPAVON, mktPlaceCatalogoId);

                if (participante != null && participante.ParticipanteCPs.Count > 0)
                {
                    var CPltmId = participante.ParticipanteCPs.Max(x => x.CatalogoCPId);
                    atualizarCP = CPltmId != catalogoCP.Id;
                }

                if (participanteCatalogo == null || atualizarEmail || atualizarCP)
                {
                    var avonAuthentication = new AvonAuthentication
                    {
                        AccountNumber = accountNumber,
                        CatalogId = catalogId,
                        PageName = "extrato",
                        i = idAdmin
                    };

                    var token = OAuthService.GetToken(avonAuthentication);

                    var result = OAuthService.CheckParticipant(accountNumber, idAdmin, mktPlaceCatalogoId, token, out hasInvalidEmail);

                    if (!string.IsNullOrEmpty(result))
                        throw new ApplicationException(result);
                }

                return hasInvalidEmail;
            }
            catch (Exception ex)
            {
                gravaLogErro(ex.Message, "Erro ao atualizar o participante no Marketplace: " + accountNumber, "GrupoLTM.WebSmart.Services.AvonService", "ValidarParticipante", "jobCatalog");
                throw ex;
            }
        }

        public static void ValidarEmailParticipante(ref Participant participante)
        {
            if (participante.Emails.Any(x => !Email.ValidarEmail(x.EmailText)))
            {
                foreach (var email in participante.Emails)
                    email.EmailText = "teste@ltm.digital";

                participante.HasInvalidEmail = true;
            }
        }

        #region Redis

        private static AvonUserInfoDataRepresentatives ObterAvonUserInfoDataRepresentativesCache(string accountNumber, string chave)
        {
            CacheAttribute cacheAttribute = new CacheAttribute();
            return cacheAttribute.GetHashObjectExtrato<AvonUserInfoDataRepresentatives>(chave + "_" + accountNumber);
        }

        private static void GravarAvonUserInfoDataRepresentativesCache(string accountNumber, string chave, object avonUserInfoDataRepresentatives)
        {
            CacheAttribute cacheAttribute = new CacheAttribute();
            cacheAttribute.StoreObjectExtrato(chave + "_" + accountNumber, avonUserInfoDataRepresentatives, "CacheAvonUserInfoDataRepresentatives");
        }

        private static void LimparAvonUserInfoDataRepresentativesCache(string accountNumber, string chave)
        {
            CacheAttribute cacheAttribute = new CacheAttribute();
            cacheAttribute.KeyDeleteAsync(chave + "_" + accountNumber);
        }

        private static void gravaLogErro(string Erro, string Mensagem, string Source, string Metodo, string Codigo)
        {
            var logErro = new LogErro()
            {
                Erro = Erro,
                Mensagem = Mensagem,
                Source = Source,
                Metodo = Metodo,
                Controller = "LoginController",
                Pagina = string.Empty,
                Codigo = Codigo
            };

            var logErroService = new LogErroService();
            logErroService.SalvarLogErro(logErro);
        }

        #endregion
    }
}
