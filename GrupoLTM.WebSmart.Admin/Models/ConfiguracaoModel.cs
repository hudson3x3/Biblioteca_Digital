using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrupoLTM.WebSmart.Admin.Models
{
    public class ConfiguracaoModel
    {
        public int Id { get; set; }
        public string NomeCampanha { get; set; }
        public int TipoAcessoId { get; set; }
        public int TipoCadastroId { get; set; }
        public int TipoValidacaoPositivaId { get; set; }
        public int? TemaId { get; set; }
        public bool? AtivoWP { get; set; }
        public bool? AtivoBoxSaldo { get; set; }
        public bool? AtivoBoxVitrine { get; set; }
        public string LIVEAPI_ENDPOINT { get; set; }
        public string LIVEAPI_URL { get; set; }
        public string LIVEAPI_USERNAME { get; set; }
        public string LIVEAPI_PASSWORD { get; set; }
        public string LIVEAPI_COOKIENAME { get; set; }
        public string LIVEAPI_CLIENTEID { get; set; }
        public string LIVEAPI_PROJECTID { get; set; }
        public string LIVE_PROJECTCONFIGURATIONID { get; set; }
        public string LIVE_URLCatalogo { get; set; }
        public string EXLOGIN { get; set; }
        public string EXSENHA { get; set; }
        public string EXTEMPLATE_KEYBOASVINDAS { get; set; }
        public string EXTEMPLATE_KEYESQUECISENHA { get; set; }
        public string EXTEMPLATE_KEYFALECONOSCO { get; set; }
        public string EMAILCREDITOPONTOS { get; set; }
        public string EMAILFALECONOSCO { get; set; }
        public string GOOGLEANALITYCS { get; set; }
        public bool? AtivoEsqueciSenhaSMS { get; set; }
        public string SMSLOGIN { get; set; }
        public string SMSSENHA { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        //public bool? AtivoTema { get; set; }
        public string ImgLogoCampanha { get; set; }
        public HttpPostedFileBase FileImagem { get; set; }
        public string InstrucaoFaleConosco { get; set; }
        public string EXTEMPLATE_KEYCadastroUsuarioAdm { get; set; }

    }
}