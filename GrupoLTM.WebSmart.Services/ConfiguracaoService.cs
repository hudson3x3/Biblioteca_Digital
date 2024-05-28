
using System;
using System.Configuration;

namespace GrupoLTM.WebSmart.Services
{
    public static class ConfiguracaoService
    {
        static readonly CatalogoService _catalogoService = new CatalogoService();

        #region General

        public static string GetUrlSite()
        {
            return ConfigurationManager.AppSettings["urlSite"].ToString();
        }

        public static string GetDatabaseConnection()
        {
            return ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
        }
        public static string GetDatabaseConnectionLiveSupplier()
        {
            return ConfigurationManager.ConnectionStrings["GrupoLTMWebSmartLiveSupplier"].ConnectionString;
        }

        public static string GetDatabaseConnectionProcessMktPlace()
        {
            return ConfigurationManager.ConnectionStrings["GrupoLTMWebSmartProcess"].ConnectionString;
        }
        public static int ApprovalByPassConfigurationRequestId()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["ApprovalByPassConfigurationRequestId"]);
        }
        public static int ApprovalByPassProjectId()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["ApprovalByPassProjectId"]);
        }

        //public static bool SkipFirstAccessCatalog(long mktPlaceCatalogoId)
        //{
        //    return _catalogoService.ObterCatalogo(mktPlaceCatalogoId).PrimeiroAcesso;
        //}

        public static string GetMD5keyLTM()
        {
            return ConfigurationManager.AppSettings["MD5keyLTM"].ToString();
        }

        #endregion

        #region MarketPlace

        public static long MktPlaceClientId()
        {
            return Convert.ToInt64(ConfigurationManager.AppSettings["mktPlace_clientId"].ToString());
        }

        public static int MktPlaceCatalogoLogarComo()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["mktPlaceCatalogoLogarComo"].ToString());
        }
        public static string MktPlaceTokenCatalog()
        {
            return ConfigurationManager.AppSettings["mktPlace_tokenCatalog"].ToString();
        }

        public static string MktPlaceParticipantInsert()
        {
            return ConfigurationManager.AppSettings["mktPlace_ParticipantInsert"].ToString();
        }

        public static string MktPlaceGetProductSku()
        {
            return ConfigurationManager.AppSettings["mktPlace_GetProductSku"].ToString();
        }

        public static string MktPlaceGetOriginalProductSku()
        {
            return ConfigurationManager.AppSettings["mktPlace_GetOriginalProductSku"].ToString();
        }

        public static string MktPlaceUrl()
        {
            return ConfigurationManager.AppSettings["mktPlace_Url"].ToString();
        }        

        public static string bankingUrl()
        {
            return ConfigurationManager.AppSettings["Payment.Microservice.Server"].ToString();
        }

        #endregion

        #region Avon

        public static string LogonId()
        {
            return ConfigurationManager.AppSettings["logonIdAvon"].ToString();
        }
        public static string LogonPassword()
        {
            return ConfigurationManager.AppSettings["logonPasswordAvon"].ToString();
        }

        public static string UserIdAvon()
        {
            return ConfigurationManager.AppSettings["userIdAvon"].ToString();
        }

        public static string PassAvon()
        {
            return ConfigurationManager.AppSettings["passAvon"].ToString();
        }

        public static string DevkeyAvon()
        {
            return ConfigurationManager.AppSettings["devkeyAvon"].ToString();
        }

        public static string UrlGetProfileAvon()
        {
            return ConfigurationManager.AppSettings["urlGetProfileAvon"].ToString();
        }

        public static string UrlNewGetProfileAvon()
        {
            return ConfigurationManager.AppSettings["urlNewGetProfileAvon"].ToString();
        }

        public static string UrlNewTokenAvon()
        {
            return ConfigurationManager.AppSettings["urlGetNewTokenAvon"].ToString();
        }

        public static string UrlGetTokenAvon()
        {
            return ConfigurationManager.AppSettings["urlGetTokenAvon"].ToString();
        }
        public static string UrlGetValidarTokenAvon()
        {
            return ConfigurationManager.AppSettings["urlGetValidarTokenAvon"].ToString();
        }
        

        public static string UrlLoginAvon()
        {
            return ConfigurationManager.AppSettings["urlLoginAvon"].ToString();
        }

        #endregion

        #region OAuth

        public static string OAuthAppId()
        {
            return ConfigurationManager.AppSettings["oAuth_appId"].ToString();
        }

        public static string OAuthAppIdOld()
        {
            return ConfigurationManager.AppSettings["oAuth_appId_old"].ToString();
        }

        public static string OAuthAppSecret()
        {
            return ConfigurationManager.AppSettings["oAuth_Secret"].ToString();
        }

        public static string OAuthAppSecretOld()
        {
            return ConfigurationManager.AppSettings["oAuth_Secret_old"].ToString();
        }

        public static string OAuthUrl()
        {
            return ConfigurationManager.AppSettings["oAuth_url"].ToString();
        }

        public static string ApiManSubscriptionKey()
        {
            return ConfigurationManager.AppSettings["apiMan_subscriptionKey"].ToString();
        }

        public static string ApiAvonUrl()
        {
            return ConfigurationManager.AppSettings["apiAvon_url"].ToString();
        }

        public static string UsernameToken()
        {
            return ConfigurationManager.AppSettings["username_token"].ToString();
        }

        public static string PasswordToken()
        {
            return ConfigurationManager.AppSettings["password_token"].ToString();
        }

        #endregion

        #region GenericSupplier
        public static string GSLogin()
        {
            return ConfigurationManager.AppSettings["GSLogin"].ToString();
        }
        public static string GSPass()
        {
            return ConfigurationManager.AppSettings["GSPass"].ToString();
        }
        public static string GSBaseUrl()
        {
            return ConfigurationManager.AppSettings["GSBaseUrl"].ToString();
        }
        public static string GSClusteredProductsUrl()
        {
            return ConfigurationManager.AppSettings["GSClusteredProductsUrl"].ToString();
        }
        #endregion
    }
}
