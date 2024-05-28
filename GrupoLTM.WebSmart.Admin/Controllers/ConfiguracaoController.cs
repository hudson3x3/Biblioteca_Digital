using GrupoLTM.WebSmart.Admin.Attributes;
using GrupoLTM.WebSmart.Admin.Models;
using GrupoLTM.WebSmart.Domain.Enums;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.Infrastructure.Configuration;
using GrupoLTM.WebSmart.Infrastructure.Helpers;
using System;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;

namespace GrupoLTM.WebSmart.Admin.Controllers
{
    public class ConfiguracaoController : BaseController
    {
        #region "Actions"

        public ActionResult Index()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repConfiguracaoCampanha = context.CreateRepository<ConfiguracaoCampanha>();
                var configuracaoCampanha = repConfiguracaoCampanha.All<ConfiguracaoCampanha>().FirstOrDefault();
                ConfiguracaoModel configuracaoCampanhaModel = new ConfiguracaoModel();
                if (configuracaoCampanha != null)
                {
                    configuracaoCampanhaModel.Id = configuracaoCampanha.Id;
                    configuracaoCampanhaModel.NomeCampanha = configuracaoCampanha.NomeCampanha;
                    configuracaoCampanhaModel.TipoAcessoId = configuracaoCampanha.TipoAcessoId;
                    configuracaoCampanhaModel.TipoCadastroId = configuracaoCampanha.TipoCadastroId;
                    configuracaoCampanhaModel.TipoValidacaoPositivaId = configuracaoCampanha.TipoValidacaoPositivaId;
                    configuracaoCampanhaModel.TemaId = configuracaoCampanha.TemaId;
                    configuracaoCampanhaModel.AtivoWP = configuracaoCampanha.AtivoWP;
                    configuracaoCampanhaModel.AtivoBoxSaldo = configuracaoCampanha.AtivoBoxSaldo;
                    configuracaoCampanhaModel.AtivoBoxVitrine = configuracaoCampanha.AtivoBoxVitrine;
                    configuracaoCampanhaModel.LIVEAPI_ENDPOINT = configuracaoCampanha.LIVEAPI_ENDPOINT;
                    configuracaoCampanhaModel.LIVEAPI_URL = configuracaoCampanha.LIVEAPI_URL;
                    configuracaoCampanhaModel.LIVEAPI_USERNAME = configuracaoCampanha.LIVEAPI_USERNAME;
                    configuracaoCampanhaModel.LIVEAPI_PASSWORD = configuracaoCampanha.LIVEAPI_PASSWORD;
                    configuracaoCampanhaModel.LIVEAPI_COOKIENAME = configuracaoCampanha.LIVEAPI_COOKIENAME;
                    configuracaoCampanhaModel.LIVE_PROJECTCONFIGURATIONID = configuracaoCampanha.LIVE_PROJECTCONFIGURATIONID;
                    configuracaoCampanhaModel.LIVEAPI_PROJECTID = configuracaoCampanha.LIVEAPI_PROJECTID;
                    configuracaoCampanhaModel.LIVEAPI_CLIENTEID = configuracaoCampanha.LIVEAPI_CLIENTEID;
                    configuracaoCampanhaModel.LIVE_URLCatalogo = configuracaoCampanha.LIVE_URLCatalogo;
                    configuracaoCampanhaModel.EXLOGIN = configuracaoCampanha.EXLOGIN;
                    configuracaoCampanhaModel.EXSENHA = configuracaoCampanha.EXSENHA;
                    configuracaoCampanhaModel.EXTEMPLATE_KEYBOASVINDAS = configuracaoCampanha.EXTEMPLATE_KEYBOASVINDAS;
                    configuracaoCampanhaModel.EXTEMPLATE_KEYESQUECISENHA = configuracaoCampanha.EXTEMPLATE_KEYESQUECISENHA;
                    configuracaoCampanhaModel.EXTEMPLATE_KEYFALECONOSCO = configuracaoCampanha.EXTEMPLATE_KEYFALECONOSCO;
                    configuracaoCampanhaModel.EMAILCREDITOPONTOS = configuracaoCampanha.EMAILCREDITOPONTOS;
                    configuracaoCampanhaModel.EMAILFALECONOSCO = configuracaoCampanha.EMAILFALECONOSCO;
                    configuracaoCampanhaModel.GOOGLEANALITYCS = configuracaoCampanha.GOOGLEANALITYCS;
                    configuracaoCampanhaModel.AtivoEsqueciSenhaSMS = configuracaoCampanha.AtivoEsqueciSenhaSMS;
                    configuracaoCampanhaModel.SMSLOGIN = configuracaoCampanha.SMSLOGIN;
                    configuracaoCampanhaModel.SMSSENHA = configuracaoCampanha.SMSSENHA;
                    configuracaoCampanhaModel.DataInclusao = configuracaoCampanha.DataInclusao;
                    configuracaoCampanhaModel.DataAlteracao = configuracaoCampanha.DataAlteracao;
                    configuracaoCampanhaModel.ImgLogoCampanha = configuracaoCampanha.ImgLogoCampanha;
                    configuracaoCampanhaModel.InstrucaoFaleConosco = configuracaoCampanha.InstrucaoFaleConosco;
                    configuracaoCampanhaModel.EXTEMPLATE_KEYCadastroUsuarioAdm = configuracaoCampanha.EXTEMPLATE_KEYCadastroUsuarioAdm;

                    return View(configuracaoCampanhaModel);
                }
                else
                {
                    return View(new ConfiguracaoModel());
                }
            }
        }

        #endregion

        #region "Actions Post"

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Gravar(ConfiguracaoModel configuracaoModel)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repConfiguracaoCampanha = context.CreateRepository<ConfiguracaoCampanha>();
                    IRepository repMenu = context.CreateRepository<Menu>();
                    var configuracaoCampanha = repConfiguracaoCampanha.All<ConfiguracaoCampanha>().FirstOrDefault();

                    var data = new object();

                    //Logo
                    if (configuracaoModel.FileImagem != null)
                    {

                        var uploadFileResult = UploadFile.Upload(
                            configuracaoModel.FileImagem,
                            Settings.Extensoes.ExtensoesPermitidasImagens,
                            int.Parse(Settings.TamanhoArquivos.TamanhoMaximoKBImagem),
                            "original/");

                        if (!uploadFileResult.arquivoSalvo)
                        {
                            data = new { ok = false, msg = "Não foi possível gravar a Imagem. " + uploadFileResult.mensagem };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            //Converte a Imagem 100x46 (Tamanho do Logo)
                            ConverterImagens(configuracaoModel.FileImagem.InputStream,
                                uploadFileResult.nomeArquivoGerado, "logo");

                            //Nome da imagem gerada
                            configuracaoModel.ImgLogoCampanha = uploadFileResult.nomeArquivoGerado;
                        }
                    }
                    else
                    {
                        //Se não enviar a imagem, mantem a do banco
                        configuracaoModel.ImgLogoCampanha = configuracaoCampanha.ImgLogoCampanha;
                    }


                    if (configuracaoCampanha != null)
                    {
                        //Atualiza o objeto Campanha
                        configuracaoCampanha.NomeCampanha = configuracaoModel.NomeCampanha;
                        configuracaoCampanha.TipoAcessoId = configuracaoModel.TipoAcessoId;
                        configuracaoCampanha.TipoCadastroId = configuracaoModel.TipoCadastroId;
                        configuracaoCampanha.TipoValidacaoPositivaId = configuracaoModel.TipoValidacaoPositivaId;
                        configuracaoCampanha.TemaId = configuracaoModel.TemaId;
                        configuracaoCampanha.AtivoWP = configuracaoModel.AtivoWP;
                        configuracaoCampanha.AtivoBoxSaldo = configuracaoModel.AtivoBoxSaldo;
                        configuracaoCampanha.AtivoBoxVitrine = configuracaoModel.AtivoBoxVitrine;

                        if (configuracaoModel.AtivoWP != null && (bool)configuracaoModel.AtivoWP)
                        {
                            if (string.IsNullOrEmpty(configuracaoModel.LIVE_URLCatalogo))
                            {
                                return Json(new { ok = false, msg = "Preencha o campo LIVE_URLCatálogo." }, JsonRequestBehavior.AllowGet);
                            }
                        }

                        var menuExtrato = repMenu.Find<Menu>(x => x.Link == "/Catalogo/Index/Extrato");
                        var menuCatalogo = repMenu.Find<Menu>(x => x.Link == "/Catalogo/Index/Catalogo");

                        if (configuracaoModel.AtivoWP.Value)
                        {
                            configuracaoCampanha.LIVEAPI_ENDPOINT = configuracaoModel.LIVEAPI_ENDPOINT;
                            configuracaoCampanha.LIVEAPI_URL = configuracaoModel.LIVEAPI_URL;
                            configuracaoCampanha.LIVEAPI_USERNAME = configuracaoModel.LIVEAPI_USERNAME;
                            configuracaoCampanha.LIVEAPI_PASSWORD = configuracaoModel.LIVEAPI_PASSWORD;
                            configuracaoCampanha.LIVEAPI_COOKIENAME = configuracaoModel.LIVEAPI_COOKIENAME;
                            configuracaoCampanha.LIVE_PROJECTCONFIGURATIONID = configuracaoModel.LIVE_PROJECTCONFIGURATIONID;
                            configuracaoCampanha.LIVEAPI_PROJECTID = configuracaoModel.LIVEAPI_PROJECTID;
                            configuracaoCampanha.LIVEAPI_CLIENTEID = configuracaoModel.LIVEAPI_CLIENTEID;
                            configuracaoCampanha.EMAILCREDITOPONTOS = configuracaoModel.EMAILCREDITOPONTOS;
                            configuracaoCampanha.LIVE_URLCatalogo = configuracaoModel.LIVE_URLCatalogo;
                            menuExtrato.Ativo = true;
                            menuCatalogo.Ativo = true;
                        }
                        else
                        {
                            menuExtrato.Ativo = false;
                            menuCatalogo.Ativo = false;
                        }


                        using (TransactionScope scope = new TransactionScope())
                        {
                            repMenu.Update(menuExtrato);
                            repMenu.Update(menuCatalogo);
                            repMenu.SaveChanges();
                            scope.Complete();
                        }

                        configuracaoCampanha.EXLOGIN = configuracaoModel.EXLOGIN;
                        configuracaoCampanha.EXSENHA = configuracaoModel.EXSENHA;
                        configuracaoCampanha.EXTEMPLATE_KEYBOASVINDAS = configuracaoModel.EXTEMPLATE_KEYBOASVINDAS;
                        configuracaoCampanha.EXTEMPLATE_KEYESQUECISENHA = configuracaoModel.EXTEMPLATE_KEYESQUECISENHA;
                        configuracaoCampanha.EXTEMPLATE_KEYFALECONOSCO = configuracaoModel.EXTEMPLATE_KEYFALECONOSCO;
                        configuracaoCampanha.EXTEMPLATE_KEYCadastroUsuarioAdm = configuracaoModel.EXTEMPLATE_KEYCadastroUsuarioAdm;
                        configuracaoCampanha.EMAILFALECONOSCO = configuracaoModel.EMAILFALECONOSCO;
                        configuracaoCampanha.GOOGLEANALITYCS = configuracaoModel.GOOGLEANALITYCS;
                        configuracaoCampanha.AtivoEsqueciSenhaSMS = configuracaoModel.AtivoEsqueciSenhaSMS;
                        configuracaoCampanha.SMSLOGIN = configuracaoModel.SMSLOGIN;
                        configuracaoCampanha.SMSSENHA = configuracaoModel.SMSSENHA;
                        configuracaoCampanha.DataAlteracao = DateTime.Now;
                        configuracaoCampanha.ImgLogoCampanha = configuracaoModel.ImgLogoCampanha;
                        configuracaoCampanha.InstrucaoFaleConosco = configuracaoModel.InstrucaoFaleConosco;

                        using (TransactionScope scope = new TransactionScope())
                        {
                            repConfiguracaoCampanha.Update(configuracaoCampanha);
                            repConfiguracaoCampanha.SaveChanges();
                            scope.Complete();
                        }

                        //Atualiza a sessão com as novas variáveis
                        var loginModel = Helpers.LoginHelper.GetLoginModel();
                        CarregaConfiguracaoCampanha(loginModel);

                        data = new { ok = true, msg = "Dados salvos com sucesso." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //Cadastra o objeto Campanha
                        ConfiguracaoCampanha ConfiguracaoCampanha = new ConfiguracaoCampanha();
                        configuracaoCampanha.Id = configuracaoModel.Id;
                        configuracaoCampanha.NomeCampanha = configuracaoModel.NomeCampanha;
                        configuracaoCampanha.TipoAcessoId = configuracaoModel.TipoAcessoId;
                        configuracaoCampanha.TipoCadastroId = configuracaoModel.TipoCadastroId;
                        configuracaoCampanha.TipoValidacaoPositivaId = configuracaoModel.TipoValidacaoPositivaId;
                        configuracaoCampanha.TemaId = configuracaoModel.TemaId;
                        configuracaoCampanha.AtivoWP = configuracaoModel.AtivoWP;
                        configuracaoCampanha.AtivoBoxSaldo = configuracaoModel.AtivoBoxSaldo;
                        configuracaoCampanha.AtivoBoxVitrine = configuracaoModel.AtivoBoxVitrine;
                        configuracaoCampanha.LIVEAPI_ENDPOINT = configuracaoModel.LIVEAPI_ENDPOINT;
                        configuracaoCampanha.LIVEAPI_URL = configuracaoModel.LIVEAPI_URL;
                        configuracaoCampanha.LIVEAPI_USERNAME = configuracaoModel.LIVEAPI_USERNAME;
                        configuracaoCampanha.LIVEAPI_PASSWORD = configuracaoModel.LIVEAPI_PASSWORD;
                        configuracaoCampanha.LIVEAPI_COOKIENAME = configuracaoModel.LIVEAPI_COOKIENAME;
                        configuracaoCampanha.LIVE_PROJECTCONFIGURATIONID = configuracaoModel.LIVE_PROJECTCONFIGURATIONID;
                        configuracaoCampanha.LIVEAPI_PROJECTID = configuracaoModel.LIVEAPI_PROJECTID;
                        configuracaoCampanha.LIVEAPI_CLIENTEID = configuracaoModel.LIVEAPI_CLIENTEID;
                        configuracaoCampanha.LIVE_URLCatalogo = configuracaoModel.LIVE_URLCatalogo;
                        configuracaoCampanha.EXLOGIN = configuracaoModel.EXLOGIN;
                        configuracaoCampanha.EXSENHA = configuracaoModel.EXSENHA;
                        configuracaoCampanha.EXTEMPLATE_KEYBOASVINDAS = configuracaoModel.EXTEMPLATE_KEYBOASVINDAS;
                        configuracaoCampanha.EXTEMPLATE_KEYESQUECISENHA = configuracaoModel.EXTEMPLATE_KEYESQUECISENHA;
                        configuracaoCampanha.EXTEMPLATE_KEYFALECONOSCO = configuracaoModel.EXTEMPLATE_KEYFALECONOSCO;
                        configuracaoCampanha.EMAILCREDITOPONTOS = configuracaoModel.EMAILCREDITOPONTOS;
                        configuracaoCampanha.EMAILFALECONOSCO = configuracaoModel.EMAILFALECONOSCO;
                        configuracaoCampanha.EXTEMPLATE_KEYCadastroUsuarioAdm = configuracaoModel.EXTEMPLATE_KEYCadastroUsuarioAdm;
                        configuracaoCampanha.GOOGLEANALITYCS = configuracaoModel.GOOGLEANALITYCS;
                        configuracaoCampanha.AtivoEsqueciSenhaSMS = configuracaoModel.AtivoEsqueciSenhaSMS;
                        configuracaoCampanha.SMSLOGIN = configuracaoModel.SMSLOGIN;
                        configuracaoCampanha.SMSSENHA = configuracaoModel.SMSSENHA;
                        configuracaoCampanha.DataInclusao = DateTime.Now;
                        configuracaoCampanha.DataAlteracao = DateTime.Now;
                        configuracaoCampanha.ImgLogoCampanha = configuracaoModel.ImgLogoCampanha;
                        configuracaoCampanha.InstrucaoFaleConosco = configuracaoModel.InstrucaoFaleConosco;

                        using (TransactionScope scope = new TransactionScope())
                        {
                            repConfiguracaoCampanha.Create(ConfiguracaoCampanha);
                            repConfiguracaoCampanha.SaveChanges();
                            scope.Complete();
                        }

                        //Atualiza a sessão com as novas variáveis
                        var loginModel = Helpers.LoginHelper.GetLoginModel();
                        CarregaConfiguracaoCampanha(loginModel);

                        data = new { ok = true, msg = "Dados salvos com sucesso." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível salvar os dados." + exc.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Internal Functions

        internal bool ConverterImagens(Stream arquivo, string name, string diretorio)
        {
            try
            {
                bool bln = false;

                var imagem = new Imagem();

                bln = imagem.ConverteImagem(arquivo, name, diretorio, 264, 132);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        #endregion
    }
}
