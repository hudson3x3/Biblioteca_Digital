
using GrupoLTM.WebSmart.DTO.MarketPlace;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;

namespace GrupoLTM.WebSmart.Infrastructure.Configuration
{
    public static class Settings
    {
        public static class EmailConfiguracao
        {
            #region Disparos Administrativos
            public static string DisplayFrom
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("Display"); }
            }
            public static string From
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("From"); }
            }
            public static string To
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("To"); }
            }
            public static string SMTP
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("SMTP"); }
            }
            public static string Login
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("EmailSMTP"); }
            }
            public static string Senha
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("SenhaSMTP"); }
            }
            public static string Porta
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("PortaSMTP"); }
            }
            public static string TemplateEmail
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("TemplateEmail"); }
            }
            public static string UrlSite
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("UrlSite"); }
            }
            public static string EmailSubjectAdm
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("EmailSubjectAdm"); }
            }
            public static string EmailSubjectSite
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("EmailSubjectSite"); }
            }
            #endregion

            #region Disparo Crédito de Pontos
            public static string CreditoTo
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("CreditoTo"); }
            }
            public static string CreditoDisplay
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("CreditoDisplay"); }
            }
            public static string CreditoSMTP
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("CreditoSMTP"); }
            }
            public static string CreditoFrom
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("CreditoFrom"); }
            }
            public static string CreditoEmailSMTP
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("CreditoEmailSMTP"); }
            }
            public static string CreditoSenhaSMTP
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("CreditoSenhaSMTP"); }
            }

            public static string CreditoPortaSMTP
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("CreditoPortaSMTP"); }
            }
            public static string CreditoEmailSubject
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("CreditoEmailSubject"); }
            }
            #endregion

            public static class SendGrid
            {
                public static string Url { get { return ConfigurationManagerHelper.GetValueFromAppSettings("sendGridUrl"); } }
                public static string Key { get { return ConfigurationManagerHelper.GetValueFromAppSettings("sendGridKey"); } }
                public static string PathMail { get { return ConfigurationManagerHelper.GetValueFromAppSettings("sendGridPathMail"); } }
                public static string TemplateId { get { return ConfigurationManagerHelper.GetValueFromAppSettings("sendGridTemplateId"); } }
                public static string TemplateIdEstorno { get { return ConfigurationManagerHelper.GetValueFromAppSettings("sendGridTemplateIdEstorno"); } }
                public static string FromName { get { return ConfigurationManagerHelper.GetValueFromAppSettings("sendGridFromName"); } }
                public static string FromEmail { get { return ConfigurationManagerHelper.GetValueFromAppSettings("sendGridFromEmail"); } }
            }
        }

        public static class Caminho
        {

            public static string BaseUrl
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("BaseUrl"); }
            }

            public static string ContentSite
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("Content.Site"); }
            }

            public static string ContentAdmin
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("Content.Admin"); }
            }

            public static string StorageToken
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("Storage.Token"); }
            }

            public static string StorageLogToken
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("Storage.LogToken"); }
            }

            public static string StorageLogPath
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("Storage.LogPath"); }
            }

            public static string StoragePath
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("Storage.Path"); }
            }

            public static string StorageAdmin
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("Storage.Admin"); }
            }

            public static string CaminhoFisicoArquivo
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("CaminhoFisicoArquivo"); }
            }

            public static string CaminhoFisicoArquivoEnviado
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("CaminhoFisicoArquivoEnviado"); }
            }

            public static string CaminhoFisicoArquivoRetorno
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("CaminhoFisicoArquivoRetorno"); }
            }

            public static string CaminhoEstornoCsv
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("Storage.EstornoCsv"); }
            }
            
            public static string CaminhoEstornoCsvErro
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("Storage.EstornoCsvErro"); }
            }

            public static string ArquivoTextoSmsEstorno
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("ArquivoSmsEstorno"); }
            }
        }

        public static class Autenticador
        {
            public static string Habilitado
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("AutenticadorHabilitado"); }
            }

            public static string CPFTeste
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("CPFTeste"); }
            }
        }

        public static class Extensoes
        {
            public static string[] ExtensoesPermitidasImagens
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("ExtensoesPermitidasImagens").Split(','); }
            }
            public static string[] ExtensoesPermitidasArquivos
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("ExtensoesPermitidasArquivos").Split(','); }
            }
            public static string[] ExtensoesPermitidasArquivosMetas
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("ExtensoesPermitidasArquivosMetas").Split(','); }
            }
            public static string[] ExtensoesPermitidasArquivosRegulamento
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("ExtensoesPermitidasArquivosRegulamento").Split(','); }
            }
        }

        public static class TamanhoArquivos
        {
            public static string TamanhoMaximoKBImagem
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("TamanhoMaximoKBImagem"); }
            }
            public static string TamanhoMaximoKBExcel
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("TamanhoMaximoKBExcel"); }
            }

            public static string TamanhoMaximoKBPdf
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("TamanhoMaximoKBPdf"); }
            }
        }

        public static class Personalizacao
        {
            public static string TituloSite
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("TituloSite"); }
            }
        }

        public static class PontuacaoSFTP
        {
            public static string Ftp
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("ftp"); }
            }

            public static string FtpLogin
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("ftpLogin"); }
            }

            public static string FtpSenha
            {
                get { return ConfigurationManagerHelper.GetValueFromAppSettings("ftpSenha"); }
            }
        }

        public static class SMS
        {
            #region SMS
            public static string authenticationtokenMovile { get { return ConfigurationManagerHelper.GetValueFromAppSettings("authenticationtokenMovile"); } }
            public static string usernameMovile { get { return ConfigurationManagerHelper.GetValueFromAppSettings("usernameMovile"); } }
            #endregion
            #region WhatsApp
            public static string authenticationtokenMovileWhatsApp { get { return ConfigurationManagerHelper.GetValueFromAppSettings("authenticationtokenMovileWhatsApp"); } }
            public static string usernameMovileWhatsApp { get { return ConfigurationManagerHelper.GetValueFromAppSettings("usernameMovileWhatsApp"); } }
            public static string keyblob { get { return ConfigurationManagerHelper.GetValueFromAppSettings("keyBlob"); } }
            #endregion
            public static string AppId { get { return ConfigurationManagerHelper.GetValueFromAppSettings("SMSAppId"); } }
            public static string Template { get { return ConfigurationManagerHelper.GetValueFromAppSettings("SMSTemplate"); } }
            public static int QtdLote { get { return Convert.ToInt32(ConfigurationManagerHelper.GetValueFromAppSettings("SMSQtdLote")); } }
            public static bool Test { get { return Convert.ToBoolean(ConfigurationManagerHelper.GetValueFromAppSettings("SMSTest")); } }
            public static string TestCelular { get { return ConfigurationManagerHelper.GetValueFromAppSettings("SMSTestCelular"); } }

            public static class TagsDinamicas
            {
                private static bool _propriedadesSetadas = false;
                private static string[] _tags;
                public static string[] Tags
                {
                    get
                    {
                        if (!_propriedadesSetadas)
                            SetarPropriedades();

                        return _tags;
                    }
                }

                private static Dictionary<string, int> _tamanhos;
                public static Dictionary<string, int> Tamanhos
                {
                    get
                    {
                        if (!_propriedadesSetadas)
                            SetarPropriedades();

                        return _tamanhos;
                    }
                }

                private static void SetarPropriedades()
                {
                    var tags = new List<string>();
                    var tamanhos = new Dictionary<string, int>();

                    var configTagsTamanhos = ConfigurationManagerHelper.GetValueFromAppSettings("SMSTagDinamicas").Split(',');

                    foreach (var configTagTamanho in configTagsTamanhos)
                    {
                        var prop = configTagTamanho.Split('|');
                        var key = prop[0];
                        var tamanho = Convert.ToInt32(prop[1]);

                        tags.Add(key);
                        tamanhos.Add(key, tamanho);
                    }

                    _tags = tags.ToArray();
                    _tamanhos = tamanhos;
                }
            }

            public static class TagsDinamicasAvulso
            {
                private static bool _propriedadesSetadas = false;
                private static string[] _tags;
                public static string[] Tags
                {
                    get
                    {
                        if (!_propriedadesSetadas)
                            SetarPropriedades();

                        return _tags;
                    }
                }

                private static Dictionary<string, int> _tamanhos;
                public static Dictionary<string, int> Tamanhos
                {
                    get
                    {
                        if (!_propriedadesSetadas)
                            SetarPropriedades();

                        return _tamanhos;
                    }
                }

                private static void SetarPropriedades()
                {
                    var tags = new List<string>();
                    var tamanhos = new Dictionary<string, int>();

                    var configTagsTamanhos = ConfigurationManagerHelper.GetValueFromAppSettings("SMSTagDinamicasAvulso").Split(',');

                    foreach (var configTagTamanho in configTagsTamanhos)
                    {
                        var prop = configTagTamanho.Split('|');
                        var key = prop[0];
                        var tamanho = Convert.ToInt32(prop[1]);

                        tags.Add(key);
                        tamanhos.Add(key, tamanho);
                    }

                    _tags = tags.ToArray();
                    _tamanhos = tamanhos;
                }
            }

            public static class Jaiminho
            {
                public static string Url { get { return ConfigurationManagerHelper.GetValueFromAppSettings("SMSJaiminhoUrl"); } }
                public static string PathLote { get { return ConfigurationManagerHelper.GetValueFromAppSettings("SMSJaiminhoPathLote"); } }
                public static string PathStatus { get { return ConfigurationManagerHelper.GetValueFromAppSettings("SMSJaiminhoPathStatus"); } }
            }

            public static class Movile
            {
                public static string Url { get { return ConfigurationManagerHelper.GetValueFromAppSettings("SMSMovileUrl"); } }
                public static string UrlStatus { get { return ConfigurationManagerHelper.GetValueFromAppSettings("SMSMovileUrlStatus"); } }
                public static string PathLoteSMS { get { return ConfigurationManagerHelper.GetValueFromAppSettings("SMSMovilePathLote"); } }
                public static string PathLoteWhatsapp { get { return ConfigurationManagerHelper.GetValueFromAppSettings("WHATSAPPMovilePathLote"); } }
                public static string keyBlob { get { return ConfigurationManagerHelper.GetValueFromAppSettings("keyBlobHML"); } }


            }
        }
    }

    internal class ConfigurationManagerHelper
    {
        internal static string GetValueFromAppSettings(string key)
        {
            if (null == key)
            {
                throw new ArgumentNullException("key");
            }

            string setting = ConfigurationManager.AppSettings[key];

            if (string.IsNullOrWhiteSpace(setting))
            {
                string message = string.Format(
                    "A key '{0}' está vazia ou não foi encontrada no arquivo .config",
                    key);

                throw new ArgumentNullException(message);

            }

            return setting;
        }

        internal static string GetConnectionString(string key)
        {
            if (null == key)
            {
                throw new ArgumentNullException("key");
            }

            string setting = ConfigurationManager.ConnectionStrings[key].ConnectionString;

            if (string.IsNullOrWhiteSpace(setting))
            {
                string message = string.Format(
                    "A connection String '{0}' está vazia ou não foi encontrada no arquivo .config",
                    key);

                throw new ArgumentNullException(message);
            }

            return setting;
        }

    }
}
