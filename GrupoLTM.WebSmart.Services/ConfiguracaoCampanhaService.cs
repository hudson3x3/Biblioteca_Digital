using GrupoLTM.WebSmart.Domain.Enums;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrupoLTM.WebSmart.Infrastructure.Cache;

namespace GrupoLTM.WebSmart.Services
{
    public class ConfiguracaoCampanhaService : BaseService<ConfiguracaoCampanha>
    {
        [Cache]
        public ConfiguracaoCampanhaModel ListarCampanhaConfiguracao()
        {
            var configuracaoCampanha = WebCache.GetCache<ConfiguracaoCampanhaModel>("Campanha");

            if (configuracaoCampanha != null)
                return configuracaoCampanha;

            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository rep = context.CreateRepository<ConfiguracaoCampanha>();
                var configuracaoCampanhaDB = rep.All<ConfiguracaoCampanha>().FirstOrDefault();

                ConfiguracaoCampanhaModel model = new ConfiguracaoCampanhaModel();
                if (configuracaoCampanhaDB != null)
                {
                    model.Id = configuracaoCampanhaDB.Id;
                    model.NomeCampanha = configuracaoCampanhaDB.NomeCampanha;
                    model.TipoAcessoId = configuracaoCampanhaDB.TipoAcessoId;
                    model.TipoCadastroId = configuracaoCampanhaDB.TipoCadastroId;
                    model.TipoValidacaoPositivaId = configuracaoCampanhaDB.TipoValidacaoPositivaId;
                    model.TemaId = configuracaoCampanhaDB.TemaId;
                    model.AtivoWP = configuracaoCampanhaDB.AtivoWP;
                    model.AtivoBoxSaldo = configuracaoCampanhaDB.AtivoBoxSaldo;
                    model.AtivoBoxVitrine = configuracaoCampanhaDB.AtivoBoxVitrine;
                    model.LIVEAPI_ENDPOINT = configuracaoCampanhaDB.LIVEAPI_ENDPOINT;
                    model.LIVEAPI_URL = configuracaoCampanhaDB.LIVEAPI_URL;
                    model.LIVEAPI_USERNAME = configuracaoCampanhaDB.LIVEAPI_USERNAME;
                    model.LIVEAPI_PASSWORD = configuracaoCampanhaDB.LIVEAPI_PASSWORD;
                    model.LIVEAPI_COOKIENAME = configuracaoCampanhaDB.LIVEAPI_COOKIENAME;
                    model.LIVE_PROJECTCONFIGURATIONID = configuracaoCampanhaDB.LIVE_PROJECTCONFIGURATIONID;
                    model.LIVEAPI_CLIENTID = configuracaoCampanhaDB.LIVEAPI_CLIENTEID;
                    model.LIVEAPI_PROJECTID = configuracaoCampanhaDB.LIVEAPI_PROJECTID;
                    model.EXLOGIN = configuracaoCampanhaDB.EXLOGIN;
                    model.EXSENHA = configuracaoCampanhaDB.EXSENHA;
                    model.EXTEMPLATE_KEYBOASVINDAS = configuracaoCampanhaDB.EXTEMPLATE_KEYBOASVINDAS;
                    model.EXTEMPLATE_KEYESQUECISENHA = configuracaoCampanhaDB.EXTEMPLATE_KEYESQUECISENHA;
                    model.EXTEMPLATE_KEYFALECONOSCO = configuracaoCampanhaDB.EXTEMPLATE_KEYFALECONOSCO;
                    model.EMAILCREDITOPONTOS = configuracaoCampanhaDB.EMAILCREDITOPONTOS;
                    model.EMAILFALECONOSCO = configuracaoCampanhaDB.EMAILFALECONOSCO;
                    model.GOOGLEANALITYCS = configuracaoCampanhaDB.GOOGLEANALITYCS;
                    model.AtivoEsqueciSenhaSMS = configuracaoCampanhaDB.AtivoEsqueciSenhaSMS;
                    model.SMSLOGIN = configuracaoCampanhaDB.SMSLOGIN;
                    model.SMSSENHA = configuracaoCampanhaDB.SMSSENHA;
                    model.DataInclusao = configuracaoCampanhaDB.DataInclusao;
                    model.DataAlteracao = configuracaoCampanhaDB.DataAlteracao;
                    model.ImgLogoCampanha = configuracaoCampanhaDB.ImgLogoCampanha;
                    model.AtivoTema = configuracaoCampanhaDB.AtivoTema;
                    model.ArquivoCSS = configuracaoCampanhaDB.Tema.ArquivoCSS;
                    model.InstrucaoFaleConosco = configuracaoCampanhaDB.InstrucaoFaleConosco;
                    model.LIVE_URLCatalogo = configuracaoCampanhaDB.LIVE_URLCatalogo;
                    model.EXTEMPLATE_KEYCadastroUsuarioAdm = configuracaoCampanhaDB.EXTEMPLATE_KEYCadastroUsuarioAdm;

                    WebCache.SetCache<ConfiguracaoCampanhaModel>("Campanha", model);

                    return model;
                }
                else
                {
                    return new ConfiguracaoCampanhaModel();
                }
            }
        }

        public string ObterLogoCampanha()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository rep = context.CreateRepository<ConfiguracaoCampanha>();
                var configuracaoCampanha = rep.All<ConfiguracaoCampanha>().FirstOrDefault();

                string logo = "";
                if (configuracaoCampanha != null)
                {
                    logo = configuracaoCampanha.ImgLogoCampanha;
                }

                return logo;
            }
        }

        public string ObterCSSCampanha()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository rep = context.CreateRepository<ConfiguracaoCampanha>();
                var configuracaoCampanha = rep.All<ConfiguracaoCampanha>().FirstOrDefault();

                string css = "";
                if (configuracaoCampanha != null)
                {
                    css = configuracaoCampanha.Tema.ArquivoCSS;
                }

                return css;
            }
        }

        public string ObterGoogleAnalytics()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository rep = context.CreateRepository<ConfiguracaoCampanha>();
                var configuracaoCampanha = rep.All<ConfiguracaoCampanha>().FirstOrDefault();

                string analytics = "";
                if (configuracaoCampanha != null)
                {
                    analytics = configuracaoCampanha.GOOGLEANALITYCS;
                }

                return analytics;
            }
        }

        public string ObterValidacaoPositiva()
        {
            ConfiguracaoCampanhaModel _configuracao = new ConfiguracaoCampanhaModel();
            _configuracao = ListarCampanhaConfiguracao();

            string retorno = "";

            switch ((EnumDomain.TipoValidacaoPositiva)_configuracao.TipoValidacaoPositivaId)
            {
                case EnumDomain.TipoValidacaoPositiva.CPF:
                    retorno = EnumDomain.TipoValidacaoPositiva.CPF.ToString();
                    break;
                case EnumDomain.TipoValidacaoPositiva.CNPJ1Campo:
                    retorno = EnumDomain.TipoValidacaoPositiva.CNPJ1Campo.ToString();
                    break;
                case EnumDomain.TipoValidacaoPositiva.CPFouCNPJ1Campo:
                    retorno = EnumDomain.TipoValidacaoPositiva.CPFouCNPJ1Campo.ToString();
                    break;
                case EnumDomain.TipoValidacaoPositiva.Codigocliente1Campo:
                    retorno = EnumDomain.TipoValidacaoPositiva.Codigocliente1Campo.ToString();
                    break;
                case EnumDomain.TipoValidacaoPositiva.Email1Campo:
                    retorno = EnumDomain.TipoValidacaoPositiva.Email1Campo.ToString();
                    break;
                case EnumDomain.TipoValidacaoPositiva.CPFeDataNascimento2Campos:
                    retorno = EnumDomain.TipoValidacaoPositiva.CPFeDataNascimento2Campos.ToString();
                    break;
                case EnumDomain.TipoValidacaoPositiva.CNPJeCPF2Campos:
                    retorno = EnumDomain.TipoValidacaoPositiva.CNPJeCPF2Campos.ToString();
                    break;
                case EnumDomain.TipoValidacaoPositiva.CodigoeCPF2Campos:
                    retorno = EnumDomain.TipoValidacaoPositiva.CodigoeCPF2Campos.ToString();
                    break;
                case EnumDomain.TipoValidacaoPositiva.CodigoeCNPJ2Campos:
                    retorno = EnumDomain.TipoValidacaoPositiva.CodigoeCNPJ2Campos.ToString();
                    break;
                default:
                    break;
            }

            return retorno;
        }
    }
}
