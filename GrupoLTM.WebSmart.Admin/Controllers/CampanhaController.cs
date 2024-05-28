using GrupoLTM.WebSmart.Admin.Attributes;
using GrupoLTM.WebSmart.Admin.Models;
using GrupoLTM.WebSmart.Domain.Enums;
using GrupoLTM.WebSmart.Services;
using System;
using System.Linq;
using System.Web.Mvc;
using GrupoLTM.WebSmart.Infrastructure.Helpers;
using GrupoLTM.WebSmart.Infrastructure.Configuration;
using GrupoLTM.WebSmart.DTO;
using System.Data;

namespace GrupoLTM.WebSmart.Admin.Controllers
{
    public class CampanhaController : BaseController
    {
        private readonly CampanhaService _campanhaService = new CampanhaService();

        private const string METAPESSOADIRECTORY = "campanha/metaresultado/metapessoa/";
        private const string ASSOCIACAOGRUPOITEMDIRECTORY = "campanha/metaresultado/associacaogrupoitem/";
        private const string GRUPOITEMDIRECTORY = "campanha/metaresultado/grupoitem/";
        private const string FAIXAATINGIMENTOGRUPOITEM = "campanha/metaresultado/faixaatingimentogrupoitem/";
        private const string METARESULTADOPORITEM = "campanha/calculado/metaresultado/poritem/";
        private const string VENDEUGANHOUPORITEM = "campanha/calculado/vendeuganhou/poritem/";
        private const string METARESULTADOPORPESSOA = "campanha/calculado/metaresultado/porpessoa/";
        private const string METARESULTADORANKING = "campanha/calculado/metaresultado/ranking/";
        private const string METARESULTADORESULTADO = "campanha/metaresultado/resultado/";
        private const string VENDEUGANHOU = "campanha/vendeuganhou/";
        private const string VENDEUGANHOUPORPESSOA = "campanha/calculado/vendeuganhou/porpessoa/";
        private const string VENDEUGANHOURANKINGPORPESSOA = "campanha/calculado/vendeuganhou/rankingporpessoa/";

        #region "Actions"

        #region "Campanha"

        public ActionResult Campanha()
        {
            return View();
        }

        public ActionResult Configuracao()
        {
            return View();
        }

        public ActionResult Consulta()
        {
            return View();
        }

        public ActionResult VendeuGanhouCalculado(int? Id)
        {
            if (Id == null)
            {
                return RedirectToAction("passo1", "Campanha");
            }

            int CampanhaId = Id == null ? 0 : Id.Value;

            CampanhaVendeuGanhouCalculadoModel campanhaVendeuGanhouCalculadoModel = new CampanhaVendeuGanhouCalculadoModel();

            var _campanha = _campanhaService.BuscarCampanha(Convert.ToInt32(Id));

            if (_campanha != null)
            {
                campanhaVendeuGanhouCalculadoModel.CampanhaId = _campanha.Id;
                campanhaVendeuGanhouCalculadoModel.TipoCampanhaId = Convert.ToInt32(_campanha.TipoCampanhaId);
                campanhaVendeuGanhouCalculadoModel.ExibirRankingIndividual = _campanha.ExibirRankingIndividual;
            }

            ViewBag.NomeCampanha = _campanhaService.BuscarCampanha(Id.Value).Nome;
            return View(campanhaVendeuGanhouCalculadoModel);
        }

        public ActionResult VerificarPassoCampanha(int? id)
        {
            var passoAction = ObterViewUltimoPassoAtivo(id.Value);

            return RedirectToAction(passoAction, new { id = id.Value });
        }

        public string ObterViewUltimoPassoAtivo(int campanhaID)
        {
            var passoCampanha = _campanhaService.BuscarUltimoPasso(campanhaID);

            var passoAction = string.Empty;

            switch (passoCampanha.PassoId)
            {
                case (int)EnumDomain.PassosCampanha.Passo1:
                    passoAction = "Passo2";
                    break;
                case (int)EnumDomain.PassosCampanha.Passo2:
                    passoAction = "Passo3";
                    break;
                case (int)EnumDomain.PassosCampanha.Passo3:
                    passoAction = "Passo4";
                    break;
                case (int)EnumDomain.PassosCampanha.Passo4:
                    passoAction = "Passo5";
                    break;
                case (int)EnumDomain.PassosCampanha.Passo5:
                    passoAction = "Passo6";
                    break;
                default:
                    passoAction = "Passo1";
                    break;
            }

            return passoAction;
        }

        [HttpGet]
        public ActionResult ListaCampanhaLogArquivo(int CampanhaId, int TipoArquivoId)
        {
            var list = _campanhaService.ListarCampanhaLogArquivo(CampanhaId, TipoArquivoId).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObterUltimoPasso(string id)
        {
            try
            {
                int campanhaId;
                var data = new object();

                if (!Int32.TryParse(id, out campanhaId))
                {
                    data = new { ok = false, msg = "CampanhaID inválido" };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }

                var passoCampanha = _campanhaService.BuscarUltimoPasso(campanhaId);

                data = new { ok = true, msg = "", passoCampanha = passoCampanha.PassoId };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var data = new { ok = false, msg = ex.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult ListaCampanhaPerfil(int CampanhaId)
        {
            var list = _campanhaService.BuscarCampanhaPerfil(CampanhaId).Select(l => new
            {
                l.Id,
                l.Nome
            }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ListaCampanhaPerfilParticipante(int CampanhaId)
        {
            var list = _campanhaService.BuscarCampanhaPerfilParticipante(CampanhaId).Select(l => new
            {
                l.Id,
                l.Nome
            }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ListaCampanhaEstrutura(int CampanhaId)
        {
            var list = _campanhaService.BuscarCampanhaEstrutura(CampanhaId).Select(l => new
            {
                l.Id,
                l.Nome
            }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ListaCampanhaEstruturaParticipante(int CampanhaId)
        {
            var list = _campanhaService.BuscarCampanhaEstruturaParticipante(CampanhaId).Select(l => new
            {
                l.Id,
                l.Nome
            }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ListarCampanhas()
        {
            try
            {
                var campanhas = _campanhaService.ListarCampanhas().Select(l => new
                {
                    l.Id,
                    l.Nome
                }).OrderBy(x => x.Nome).ToList();

                var data = new { ok = true, msg = "", campanhas = campanhas };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var data = new { ok = false, msg = ex.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult ListarCampanhasPorTipoStatus(int? TipoCampanhaId, int? StatusCampanhaId)
        {
            try
            {
                var campanhas = _campanhaService.ListarCampanhasPorTipoStatus(TipoCampanhaId, StatusCampanhaId);

                var data = new { ok = true, msg = "", campanhas = campanhas };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var data = new { ok = false, msg = ex.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult ListarCampanhaPeriodo(int CampanhaId)
        {
            var list = _campanhaService.ListarCampanhaPeriodo(CampanhaId).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ListarCampanhaPeriodoNaoApurado(int CampanhaId)
        {
            var list = _campanhaService.ListarCampanhaPeriodoNaoApurado(CampanhaId).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Passo1(int? id)
        {
            ViewBag.CampanhaId = "";

            if (id.HasValue)
            {
                var campanha = _campanhaService.BuscarCampanha(id.Value);

                if (campanha == null)
                    return RedirectToAction("Index", "Home");

                var model = new CampanhaPasso1Model
                {
                    DataFim = campanha.DataFim.ToString(),
                    DataInicio = campanha.DataInicio.ToString(),
                    Descricao = campanha.Descricao,
                    Nome = campanha.Nome,
                    Id = campanha.Id,
                    ResultadoCascata = campanha.ResultadoCascata,
                    CalcularPelaHierarquia = campanha.CalcularPelaHierarquia,
                    ExibirPerido = campanha.ExibirPeriodo
                };

                ViewBag.CampanhaId = campanha.Id;
                ViewBag.NomeCampanha = campanha.Nome;

                return View(model);
            }

            ViewBag.NomeCampanha = "Nova";

            return View(new CampanhaPasso1Model());
        }

        public ActionResult Passo2(int? Id)
        {
            //Verifica se existe passo
            if (Id == null)
            {
                return RedirectToAction("passo1", "Campanha");
            }

            int CampanhaId = Id == null ? 0 : Id.Value;

            CampanhaPasso2Model Passo2Model = new CampanhaPasso2Model();

            var _campanhaEstrutura = _campanhaService.BuscarCampanhaEstrutura(CampanhaId);

            if (_campanhaEstrutura.Count > 0)
            {
                Passo2Model.EstruturaId = _campanhaEstrutura.Where(x => x.Participa == true).Select(a => a.EstruturaId).ToArray();
                Passo2Model.EstruturaViewId = _campanhaEstrutura.Where(x => x.Participa == false).Select(a => a.EstruturaId).ToArray();
                Passo2Model.TipoEstruturaId = _campanhaEstrutura.Where(t => t.Participa).Select(a => a.TipoEstrutura).First();
            }

            Passo2Model.CampanhaId = CampanhaId;
            ViewBag.NomeCampanha = _campanhaService.BuscarCampanha(Id.Value).Nome;

            return View(Passo2Model);
        }

        public ActionResult Passo3(int? Id)
        {
            //Verifica se existe Campanha
            if (Id == null)
            {
                return RedirectToAction("passo1", "Campanha");
            }

            //Valida Sequencia dos Passos
            var ultimoPasso = _campanhaService.BuscarUltimoPasso(Convert.ToInt32(Id));
            var passoAtual = Convert.ToInt32(Domain.Enums.EnumDomain.PassosCampanha.Passo3);
            if (ultimoPasso != null)
            {
                if ((ultimoPasso.PassoId + 1) < passoAtual)
                {
                    var action = "passo" + Convert.ToString((ultimoPasso.PassoId + 1));
                    return RedirectToAction(action, "Campanha", new { Id = Id });
                }
            }

            //Carrega Campanha
            int CampanhaId = Id == null ? 0 : Id.Value;
            CampanhaPasso3Model Passo3Model = new CampanhaPasso3Model();

            var _campanhaPerfil = _campanhaService.BuscarCampanhaPerfil(CampanhaId);

            if (_campanhaPerfil.Count > 0)
            {
                Passo3Model.PerfilId = _campanhaPerfil.Where(x => x.Participa == true).Select(a => a.PerfilId).ToArray();
                Passo3Model.PerfilViewId = _campanhaPerfil.Where(x => x.Participa == false).Select(a => a.PerfilId).ToArray();
            }

            Passo3Model.CampanhaId = CampanhaId;
            ViewBag.NomeCampanha = _campanhaService.BuscarCampanha(Id.Value).Nome;

            return View(Passo3Model);
        }

        public ActionResult Passo4(int? Id)
        {
            //Verifica se existe Campanha
            if (Id == null)
            {
                return RedirectToAction("passo1", "Campanha");
            }

            //Valida Sequencia dos Passos
            var ultimoPasso = _campanhaService.BuscarUltimoPasso(Convert.ToInt32(Id));
            var passoAtual = Convert.ToInt32(Domain.Enums.EnumDomain.PassosCampanha.Passo4);
            if (ultimoPasso != null)
            {
                if ((ultimoPasso.PassoId + 1) < passoAtual)
                {
                    var action = "passo" + Convert.ToString((ultimoPasso.PassoId + 1));
                    return RedirectToAction(action, "Campanha", new { Id = Id });
                }
            }


            //Carrega Campanha
            if (Id.HasValue)
            {
                var banner = _campanhaService.BuscarCampanhaConteudo(Id.Value, (int)EnumDomain.TipoConteudo.BannerHome);
                var mecanica = _campanhaService.BuscarCampanhaConteudo(Id.Value, (int)EnumDomain.TipoConteudo.ImagemMecanica);
                var mecanicaMobile = _campanhaService.BuscarCampanhaConteudo(Id.Value, (int)EnumDomain.TipoConteudo.ImagemMecanicaMobile);
                var regulamento = _campanhaService.BuscarCampanhaConteudo(Id.Value, (int)EnumDomain.TipoConteudo.Regulamento);

                var model = new CampanhaPasso4Model
                {
                    CampanhaId = Id.Value,
                    LinkBannerHome = banner.Arquivo,
                    LinkImagemMecanica = mecanica.Arquivo,
                    LinkImagemMecanicaMobile = mecanicaMobile.Arquivo,
                    Regulamento = regulamento.Texto
                };

                ViewBag.NomeCampanha = _campanhaService.BuscarCampanha(Id.Value).Nome;
                return View(model);
            }
            return View(new CampanhaPasso4Model());
        }

        public ActionResult Passo5(int? Id)
        {
            //Verifica se existe Campanha
            if (Id == null)
            {
                return RedirectToAction("passo1", "Campanha");
            }

            //Valida Sequencia dos Passos
            var ultimoPasso = _campanhaService.BuscarUltimoPasso(Convert.ToInt32(Id));
            var passoAtual = Convert.ToInt32(Domain.Enums.EnumDomain.PassosCampanha.Passo5);
            if (ultimoPasso != null)
            {
                if ((ultimoPasso.PassoId + 1) < passoAtual)
                {
                    var action = "passo" + Convert.ToString((ultimoPasso.PassoId + 1));
                    return RedirectToAction(action, "Campanha", new { Id = Id });
                }
            }


            if (Id.HasValue)
            {
                var campanha = _campanhaService.BuscarCampanha(Id.Value);
                var passo = _campanhaService.BuscarUltimoPasso(Convert.ToInt32(Id));

                //Passo 5 ou 6, verifica o tipo da campanha e faz o redirect
                if (passo.PassoId >= (int)EnumDomain.PassosCampanha.Passo5)
                {
                    return RedirecionarPorTipoCampanha(Convert.ToInt32(Id), Convert.ToInt32(campanha.TipoCampanhaId));
                }
                else
                {
                    //Passo 4, Carrega passo 5
                    var model = new CampanhaPasso5Model
                    {
                        Id = campanha.Id,
                        Campanha = campanha.Nome,
                        TipoCampanhaId = campanha.TipoCampanhaId
                    };
                    ViewBag.NomeCampanha = _campanhaService.BuscarCampanha(Id.Value).Nome;
                    return View(model);
                }
            }
            return View(new CampanhaPasso5Model());
        }

        public ActionResult Passo6(int? Id)
        {
            //Verifica se a campanha existe
            if (Id == null)
            {
                return RedirectToAction("passo1", "Campanha");
            }

            //Valida Sequencia dos Passos
            var ultimoPasso = _campanhaService.BuscarUltimoPasso(Convert.ToInt32(Id));
            var passoAtual = Convert.ToInt32(Domain.Enums.EnumDomain.PassosCampanha.Passo6);
            if (ultimoPasso != null)
            {
                if ((ultimoPasso.PassoId + 1) < passoAtual)
                {
                    var action = "passo" + Convert.ToString((ultimoPasso.PassoId + 1));
                    return RedirectToAction(action, "Campanha", new { Id = Id });
                }
            }


            if (Id.HasValue)
            {
                var campanha = _campanhaService.BuscarCampanha(Id.Value);

                var model = new CampanhaPasso6Model
                {
                    Id = campanha.Id,
                    Campanha = campanha.Nome,
                    StatusCampanhaId = campanha.StatusCampanhaId
                };

                ViewBag.NomeCampanha = _campanhaService.BuscarCampanha(Id.Value).Nome;
                return View(model);
            }
            return View(new CampanhaPasso6Model());
        }

        public ActionResult ListaStatusCampanha()
        {
            var list = from EnumDomain.StatusCampanha e in Enum.GetValues(typeof(EnumDomain.StatusCampanha))
                       select new { Id = e, Nome = GrupoLTM.WebSmart.Infrastructure.Helpers.StringHelper.GetEnumDescription<EnumDomain.StatusCampanha>(e) };

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region "Meta e Resultado"

        public ActionResult MetaResultadoRanking(int? Id)
        {
            if (Id.HasValue)
            {
                var campanha = _campanhaService.BuscarCampanha(Id.Value);

                var model = new CampanhaUploadMetaPessoaModel
                {
                    CampanhaId = campanha.Id,
                    TipoCampanhaId = campanha == null ? 0 : campanha.TipoCampanhaId.Value,
                    ExibirRankingIndividual = campanha.ExibirRankingIndividual
                };

                ViewBag.NomeCampanha = _campanhaService.BuscarCampanha(Id.Value).Nome;
                return View(model);
            }
            return View(new CampanhaUploadMetaPessoaModel());
        }

        public ActionResult MetaResultadoCalculado(int? Id)
        {
            if (Id == null)
            {
                return RedirectToAction("passo1", "Campanha");
            }

            int CampanhaId = Id == null ? 0 : Id.Value;

            CampanhaMetaResultadoCalculadoModel campanhaMetaResultadoCalculadoModel = new CampanhaMetaResultadoCalculadoModel();

            var _campanha = _campanhaService.BuscarCampanha(Convert.ToInt32(Id));

            if (_campanha != null)
            {
                campanhaMetaResultadoCalculadoModel.CampanhaId = _campanha.Id;
                campanhaMetaResultadoCalculadoModel.TipoCampanhaId = Convert.ToInt32(_campanha.TipoCampanhaId);
                campanhaMetaResultadoCalculadoModel.ExibirRankingIndividual = _campanha.ExibirRankingIndividual;
            }

            ViewBag.NomeCampanha = _campanhaService.BuscarCampanha(Id.Value).Nome;
            return View(campanhaMetaResultadoCalculadoModel);
        }

        //Meta e Resultado Ranking Upload de Meta por Pessoa
        public ActionResult Passo5_2_1(int? Id)
        {
            if (Id.HasValue)
            {
                var model = new CampanhaUploadMetaPessoaModel
                {
                    CampanhaId = Id.Value
                };

                return View(model);
            }
            return View(new CampanhaUploadMetaPessoaModel());
        }

        public ActionResult MetaResultado(int? Id)
        {

            int CampanhaId = Id == null ? 0 : Id.Value;
            CampanhaPasso2Model Passo2Model = new CampanhaPasso2Model();

            var _campanha = _campanhaService.BuscarCampanha(CampanhaId);

            Passo2Model.CampanhaId = CampanhaId;

            if (Id.HasValue)
            {
                var model = new CampanhaMetaResultadoModel
                {
                    Id = Id.Value,
                    TipoCampanhaId = _campanha.TipoCampanhaId == null ? 0 : _campanha.TipoCampanhaId.Value
                };

                ViewBag.NomeCampanha = _campanhaService.BuscarCampanha(Id.Value).Nome;
                return View(model);
            }
            return View(new CampanhaMetaResultadoModel());
        }

        #endregion

        #region "Vendeu Ganhou"

        public ActionResult VendeuGanhou(int? Id)
        {
            if (Id.HasValue)
            {
                var campanha = _campanhaService.BuscarCampanha(Id.Value);

                //Verifica se a campanha é do Tipo Vendeu Ganhou
                if (Convert.ToInt32(campanha.TipoCampanhaId) != (int)EnumDomain.TipoCampanha.VendeuGanhou)
                {
                    return RedirecionarPorTipoCampanha(Convert.ToInt32(Id), Convert.ToInt32(campanha.TipoCampanhaId));
                }
                else
                {
                    //Se sim, retorna a View
                    var model = new CampanhaVendeuGanhouModel
                    {
                        Id = Id.Value
                    };
                    ViewBag.NomeCampanha = _campanhaService.BuscarCampanha(Id.Value).Nome;
                    return View(model);
                }
            }

            return View(new CampanhaVendeuGanhouModel());
        }

        [HttpGet]
        public ActionResult ListarCampanhaPeriodoVendeuGanhou(int CampanhaId)
        {
            var list = _campanhaService.ListarCampanhaPeriodoVendeuGanhou(CampanhaId).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region "Vendeu Ganhou Ranking"

        public ActionResult VendeuGanhouRanking(int? Id)
        {
            if (Id.HasValue)
            {
                var campanha = _campanhaService.BuscarCampanha(Id.Value);

                //Verifica se a campanha é do Tipo Vendeu Ganhou Ranking
                if (Convert.ToInt32(campanha.TipoCampanhaId) != (int)EnumDomain.TipoCampanha.VendeuGanhouRanking)
                {
                    return RedirecionarPorTipoCampanha(Convert.ToInt32(Id), Convert.ToInt32(campanha.TipoCampanhaId));
                }
                else
                {
                    //Se sim, retorna a View
                    var model = new CampanhaVendeuGanhouRankingModel
                    {
                        Id = Id.Value,
                        ExibirRankingIndividual = campanha.ExibirRankingIndividual

                    };

                    ViewBag.NomeCampanha = _campanhaService.BuscarCampanha(Id.Value).Nome;
                    return View(model);
                }
            }
            return View(new CampanhaVendeuGanhouRankingModel());
        }

        [HttpGet]
        public ActionResult ListarCampanhaFaixaAtingimento(int CampanhaId)
        {
            var list = _campanhaService.ListarCampanhaFaixaAtingimentoModel(CampanhaId).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #endregion

        #region "Actions Posts"

        #region "Campanha"

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false)]
        public ActionResult Passo1(CampanhaPasso1Model campanhaModel)
        {
            try
            {
                var data = new object();

                DateTime dataInicio;
                if (!DateTime.TryParse(campanhaModel.DataInicio, out dataInicio))
                {
                    data = new { ok = false, msg = "Data início inválida." };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }

                DateTime dataFim;
                if (!DateTime.TryParse(campanhaModel.DataFim, out dataFim))
                {
                    data = new { ok = false, msg = "Data fim inválida." };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }

                if (dataFim < dataInicio)
                {
                    data = new { ok = false, msg = "Data fim precisa ser maior que a data de início da campanha." };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }

                var campanha = new GrupoLTM.WebSmart.Domain.Models.Campanha();

                if (campanhaModel.Id == 0)
                {
                    campanha = _campanhaService.SalvarCampanha(new GrupoLTM.WebSmart.DTO.CampanhaModel
                    {
                        DataFim = dataFim,
                        DataInicio = dataInicio,
                        Descricao = campanhaModel.Descricao,
                        Nome = campanhaModel.Nome,
                        TipoCampanhaId = null,
                        StatusCampanhaId = (int)EnumDomain.StatusCampanha.EmConfiguracao,
                        ResultadoCascata = campanhaModel.ResultadoCascata
                    });

                    _campanhaService.IncluirPasso(campanha.Id, (int)EnumDomain.PassosCampanha.Passo1);
                }
                else
                {
                    campanha = _campanhaService.AtualizarCampanha(new GrupoLTM.WebSmart.DTO.CampanhaModel
                    {
                        Id = campanhaModel.Id,
                        DataFim = dataFim,
                        DataInicio = dataInicio,
                        Descricao = campanhaModel.Descricao,
                        Nome = campanhaModel.Nome,
                        TipoCampanhaId = null,
                        //StatusCampanhaId = (int)EnumDomain.StatusCampanha.EmConfiguracao,
                        ResultadoCascata = campanhaModel.ResultadoCascata
                    });
                }

                ViewBag.CampanhaId = campanha.Id;

                var passoAction = ObterViewUltimoPassoAtivo(campanha.Id);

                data = new { ok = true, msg = string.Format("{0}/{1}", passoAction, campanha.Id) };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível salvar os dados. " + exc.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false)]
        public ActionResult Passo2(CampanhaPasso2Model campanhaModel)
        {
            try
            {
                var data = new object();

                var campanha = new GrupoLTM.WebSmart.Domain.Models.Campanha();

                campanha.Id = campanhaModel.CampanhaId;

                //Faz o reset da configuração da campanha, caso o retorno for false, já existe pontuação atribuída. Não permitir a alteração e emitir mensagem para o participante
                if (!_campanhaService.ResetCampanha(campanhaModel.CampanhaId))
                {
                    data = new { ok = false, msg = "Não foi possível salvar os dados. Não é permitida a alteração das estruturas já configuradas para campanhas com pontuações aprovadas.", id = campanha.Id };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }

                //Estruturas que participam
                int[] CampanhaEstruturas = campanhaModel.EstruturaId != null ? campanhaModel.EstruturaId.Where(x => x != 0).ToArray() : null;

                //Estrutuas que visualizam
                int[] CampanhaEstruturasView = campanhaModel.EstruturaViewId != null ? campanhaModel.EstruturaViewId.Where(x => x != 0).ToArray() : null;

                //Valida se alguma Estrutura foi selecionada.
                if (campanhaModel.EstruturaId == null || CampanhaEstruturas.Count() <= 0)
                {
                    data = new { ok = false, msg = "Nenhuma estrutura selecionada.", id = campanha.Id };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }

                if (campanhaModel.CampanhaId != 0)
                {
                    //Salva estruturas (Participam e Visualizam)
                    _campanhaService.SalvarAtualizarCampanhaEstrutura(campanhaModel.CampanhaId, campanhaModel.TipoEstruturaId, CampanhaEstruturas, CampanhaEstruturasView);

                    //Verifica se a campanha já passou pelo passo atual, caso sim envia para o passo de configuração, Passo 5
                    if (_campanhaService.BuscaCampanhaPasso(campanhaModel.CampanhaId, EnumDomain.PassosCampanha.Passo2))
                    {
                        data = new { ok = true, msg = string.Format("{0}/{1}", "Passo5", campanha.Id) };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //Redireciona para o próximo passo
                        _campanhaService.IncluirPasso(campanha.Id, (int)EnumDomain.PassosCampanha.Passo2);

                        var passoAction = ObterViewUltimoPassoAtivo(campanha.Id);

                        data = new { ok = true, msg = string.Format("{0}/{1}", passoAction, campanha.Id) };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }

                }

                data = new { ok = false, msg = "Não foi possível salvar os dados. ", id = campanha.Id };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível salvar os dados. " + exc.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false)]
        public ActionResult Passo3(CampanhaPasso3Model campanhaModel)
        {
            try
            {
                var data = new object();

                var campanha = new GrupoLTM.WebSmart.Domain.Models.Campanha();

                campanha.Id = campanhaModel.CampanhaId;

                //Faz o reset da configuração da campanha, caso o retorno for false, já existe pontuação atribuída. Não permitir a alteração e emitir mensagem para o participante
                if (!_campanhaService.ResetCampanha(campanhaModel.CampanhaId))
                {
                    data = new { ok = false, msg = "Não foi possível salvar os dados. Não é permitida a alteração dos perfis já configurados para campanhas com pontuações aprovadas.", id = campanha.Id };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }

                int[] CampanhaPerfil = campanhaModel.PerfilId != null ? campanhaModel.PerfilId.Where(x => x != 0).ToArray() : null;
                int[] CampanhaPerfilView = campanhaModel.PerfilViewId != null ? campanhaModel.PerfilViewId.Where(x => x != 0).ToArray() : null;


                //Valida se algum Perfil foi selecionada.
                if (campanhaModel.PerfilId == null || CampanhaPerfil.Count() <= 0)
                {
                    data = new { ok = false, msg = "Nenhum Perfil selecionado.", id = campanha.Id };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }

                if (campanhaModel.CampanhaId != 0)
                {
                    //Salva perfis (Participam e Visualizam)
                    _campanhaService.SalvarAtualizarCampanhaPerfil(campanhaModel.CampanhaId, CampanhaPerfil, CampanhaPerfilView);

                    //Verifica se a campanha já passou pelo passo atual, caso sim envia para o passo de configuração, Passo 5
                    if (_campanhaService.BuscaCampanhaPasso(campanhaModel.CampanhaId, EnumDomain.PassosCampanha.Passo3))
                    {
                        data = new { ok = true, msg = string.Format("{0}/{1}", "Passo5", campanha.Id) };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //Redireciona para o próximo passo
                        _campanhaService.IncluirPasso(campanha.Id, (int)EnumDomain.PassosCampanha.Passo3);

                        var passoAction = ObterViewUltimoPassoAtivo(campanha.Id);

                        data = new { ok = true, msg = string.Format("{0}/{1}", passoAction, campanha.Id) };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }

                data = new { ok = false, msg = "Não foi possível salvar os dados. ", id = campanha.Id };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível salvar os dados. " + exc.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false)]
        public ActionResult Passo4(CampanhaPasso4Model campanhaModel)
        {
            try
            {
                var data = new object();

                //upload do banner
                if (campanhaModel.FileBannerHome != null)
                {
                    var uploadFileBannerResult = UploadFile.Upload(
                        campanhaModel.FileBannerHome,
                        Settings.Extensoes.ExtensoesPermitidasArquivos,
                        int.Parse(Settings.TamanhoArquivos.TamanhoMaximoKBImagem),
                        "banner/");

                    if (!uploadFileBannerResult.arquivoSalvo)
                    {
                        data = new { ok = false, msg = "Não foi possível gravar o banner da home. " + uploadFileBannerResult.mensagem };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //inativa o conteudo anterior
                        _campanhaService.InativarCampanhaConteudo(campanhaModel.CampanhaId, (int)EnumDomain.TipoConteudo.BannerHome);
                        //seta o nome do arquivo
                        campanhaModel.LinkBannerHome = uploadFileBannerResult.nomeArquivoGerado;
                    }
                }
                else
                {
                    if (campanhaModel.LinkBannerHome == null)
                    {
                        data = new { ok = false, msg = "Por favor, selecione o campo do banner da home." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }

                //upload da imagem da mecânica
                if (campanhaModel.FileImagemMecanica != null)
                {
                    var uploadFileMecanicaResult = UploadFile.Upload(
                        campanhaModel.FileImagemMecanica,
                        Settings.Extensoes.ExtensoesPermitidasArquivos,
                        int.Parse(Settings.TamanhoArquivos.TamanhoMaximoKBImagem),
                        "mecanica/");

                    if (!uploadFileMecanicaResult.arquivoSalvo)
                    {
                        data = new { ok = false, msg = "Não foi possível gravar a imagem da mecânica. " + uploadFileMecanicaResult.mensagem };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //inativa o conteudo anterior
                        _campanhaService.InativarCampanhaConteudo(campanhaModel.CampanhaId, (int)EnumDomain.TipoConteudo.ImagemMecanica);
                        //seta o nome do arquivo
                        campanhaModel.LinkImagemMecanica = uploadFileMecanicaResult.nomeArquivoGerado;
                    }
                }
                else
                {
                    if (campanhaModel.LinkImagemMecanica == null)
                    {
                        data = new { ok = false, msg = "Por favor, selecione a imagem da mecânica." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }

                //upload da imagem da mecânica mobile
                if (campanhaModel.FileImagemMecanicaMobile != null)
                {
                    var uploadFileMecanicaMResult = UploadFile.Upload(
                        campanhaModel.FileImagemMecanicaMobile,
                        Settings.Extensoes.ExtensoesPermitidasArquivos,
                        int.Parse(Settings.TamanhoArquivos.TamanhoMaximoKBImagem),
                        "mecanicaMobile/");

                    if (!uploadFileMecanicaMResult.arquivoSalvo)
                    {
                        data = new { ok = false, msg = "Não foi possível gravar a imagem da mecânica mobile. " + uploadFileMecanicaMResult.mensagem };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //inativa o conteudo anterior
                        _campanhaService.InativarCampanhaConteudo(campanhaModel.CampanhaId, (int)EnumDomain.TipoConteudo.ImagemMecanicaMobile);
                        //seta o nome do arquivo
                        campanhaModel.LinkImagemMecanicaMobile = uploadFileMecanicaMResult.nomeArquivoGerado;
                    }
                }
                else
                {
                    if (campanhaModel.LinkImagemMecanicaMobile == null)
                    {
                        data = new { ok = false, msg = "Por favor, selecione a imagem da mecânica mobile." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }

                if (campanhaModel.FileBannerHome != null && campanhaModel.LinkBannerHome != null)
                {
                    _campanhaService.CadastrarCampanhaConteudo(new GrupoLTM.WebSmart.DTO.CampanhaConteudoModel
                    {
                        CampanhaId = campanhaModel.CampanhaId,
                        Nome = campanhaModel.LinkBannerHome,
                        TipoConteudoId = (int)EnumDomain.TipoConteudo.BannerHome,
                        Ativo = true,
                        Ordem = 1,
                        Arquivo = campanhaModel.LinkBannerHome
                    });
                }

                if (campanhaModel.FileImagemMecanica != null && campanhaModel.LinkImagemMecanica != null)
                {
                    _campanhaService.CadastrarCampanhaConteudo(new GrupoLTM.WebSmart.DTO.CampanhaConteudoModel
                    {
                        CampanhaId = campanhaModel.CampanhaId,
                        Nome = campanhaModel.LinkImagemMecanica,
                        TipoConteudoId = (int)EnumDomain.TipoConteudo.ImagemMecanica,
                        Ativo = true,
                        Ordem = 1,
                        Arquivo = campanhaModel.LinkImagemMecanica
                    });
                }

                if (campanhaModel.FileImagemMecanicaMobile != null && campanhaModel.LinkImagemMecanicaMobile != null)
                {
                    _campanhaService.CadastrarCampanhaConteudo(new GrupoLTM.WebSmart.DTO.CampanhaConteudoModel
                    {
                        CampanhaId = campanhaModel.CampanhaId,
                        Nome = campanhaModel.LinkImagemMecanicaMobile,
                        TipoConteudoId = (int)EnumDomain.TipoConteudo.ImagemMecanicaMobile,
                        Ativo = true,
                        Ordem = 1,
                        Arquivo = campanhaModel.LinkImagemMecanicaMobile
                    });
                }

                if (campanhaModel.Regulamento != null)
                {
                    //inativa todos os regulamento cadastrados para essa campanha.
                    _campanhaService.InativarCampanhaConteudo(campanhaModel.CampanhaId, (int)EnumDomain.TipoConteudo.Regulamento);

                    //cadastra o regulamento
                    _campanhaService.CadastrarCampanhaConteudo(new GrupoLTM.WebSmart.DTO.CampanhaConteudoModel
                    {
                        CampanhaId = campanhaModel.CampanhaId,
                        Nome = "Regulamento",
                        TipoConteudoId = (int)EnumDomain.TipoConteudo.Regulamento,
                        Ativo = true,
                        Ordem = 1,
                        Texto = campanhaModel.Regulamento
                    });
                }

                _campanhaService.IncluirPasso(campanhaModel.CampanhaId, (int)EnumDomain.PassosCampanha.Passo4);

                var passoAction = ObterViewUltimoPassoAtivo(campanhaModel.CampanhaId);

                data = new { ok = true, msg = string.Format("{0}/{1}", passoAction, campanhaModel.CampanhaId) };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível salvar os dados. " + exc.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false)]
        public ActionResult Passo5(CampanhaPasso5Model campanhaModel)
        {
            try
            {
                var data = new object();

                //Atualiza o Tipo da Campanha
                _campanhaService.AtualizarCampanha(new GrupoLTM.WebSmart.DTO.CampanhaModel
                {
                    Id = campanhaModel.Id,
                    TipoCampanhaId = campanhaModel.TipoCampanhaId,
                    DataAlteracao = DateTime.Now
                });

                //Inclui o passo 5
                _campanhaService.IncluirPasso(campanhaModel.Id, (int)EnumDomain.PassosCampanha.Passo5);

                //Busca o Action para a View de acordo com o Tipo da Campanha
                var action = BuscarRedirecionarPorTipoCampanha(campanhaModel.Id, Convert.ToInt32(campanhaModel.TipoCampanhaId));

                data = new { ok = true, msg = "Dados Salvos com sucesso.", link = action };

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível salvar os dados. " + exc.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false)]
        public ActionResult Passo6(CampanhaPasso6Model campanhaModel)
        {
            try
            {
                var data = new object();

                //Atualiza o Status da Campanha
                _campanhaService.AtualizarCampanha(new GrupoLTM.WebSmart.DTO.CampanhaModel
                {
                    Id = campanhaModel.Id,
                    StatusCampanhaId = Convert.ToInt32(campanhaModel.StatusCampanhaId),
                    DataAlteracao = DateTime.Now
                });

                //Inclui o passo 6
                _campanhaService.IncluirPasso(campanhaModel.Id, (int)EnumDomain.PassosCampanha.Passo6);

                data = new { ok = true, msg = "Dados salvos com sucesso." };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível salvar os dados. " + exc.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false)]
        public ActionResult CadastrarCampanhaPeriodo(CampanhaPeriodoModel campanhaPeriodoModel)
        {
            try
            {
                var data = new object();

                _campanhaService.CadastrarCampanhaPeriodo(new CampanhaPeriodoModel
                {
                    Ativo = true,
                    CampanhaId = campanhaPeriodoModel.CampanhaId,
                    DataAlteracao = DateTime.Now,
                    DataInclusao = DateTime.Now,
                    DataFechamento = campanhaPeriodoModel.DataFechamento,
                    Nome = campanhaPeriodoModel.Nome,
                    PeriodoAte = campanhaPeriodoModel.PeriodoAte,
                    PeriodoDe = campanhaPeriodoModel.PeriodoDe
                });

                data = new { ok = true, msg = "Dados salvos com sucesso." };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível salvar os dados. " + exc.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false)]
        public ActionResult InativarCampanhaPeriodo(int Id)
        {
            try
            {
                var data = new object();

                //Verifica se o período já foi apurado
                if (CampanhaPeriodoApurado(Id))
                {
                    data = new { ok = false, msg = "Não foi possível realizar a operação, período já apurado." };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }

                //Inativa o período
                _campanhaService.InativarCampanhaPeriodo(Id);

                //Retorna mensagem de sucesso
                data = new { ok = true, msg = "Dados salvos com sucesso." };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível salvar os dados.  " + exc.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false)]
        public ActionResult SalvarConfiguracaoVisualizacaoRanking(CampanhaConfiguracaoVisualizacaoRankingModel campanhaConfiguracaoVisualizacaoRankingModel)
        {
            try
            {
                var data = new object();

                var blnExibirRankingIndividual = true;

                if (campanhaConfiguracaoVisualizacaoRankingModel.arrExibirRankingIndividual.Count() > 0)
                {
                    blnExibirRankingIndividual = (((int)campanhaConfiguracaoVisualizacaoRankingModel.arrExibirRankingIndividual.GetValue(0)) > 0 ? true : false);
                }

                //Atualiza o Tipo da Campanha
                _campanhaService.AtualizarCampanha(new GrupoLTM.WebSmart.DTO.CampanhaModel
                {
                    Id = campanhaConfiguracaoVisualizacaoRankingModel.CampanhaId,
                    ExibirRankingIndividual = blnExibirRankingIndividual,
                    DataAlteracao = DateTime.Now
                });

                data = new { ok = true, msg = "Dados Salvos com sucesso." };

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível salvar os dados. " + exc.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion

  
        #region "Vendeu Ganhou Ranking"

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false)]
        public ActionResult InativarCampanhaFaixaAtingimento(int Id)
        {
            try
            {
                _campanhaService.InativarCampanhaFaixaAtingimento(Id);
                var data = new { ok = true, msg = "Configuração excluída com sucesso." };
                return Json(data, JsonRequestBehavior.AllowGet);

            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível excluir a configuração. " + exc.ToString() };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false)]
        public ActionResult CadastrarCampanhaFaixaAtingimento(CampanhaFaixaAtingimentoModel campanhaFaixaAtingimentoModel)
        {
            try
            {
                var data = new object();

                //Verifica se o período já foi apurado
                if (CampanhaPeriodoApurado(campanhaFaixaAtingimentoModel.CampanhaPeriodoId))
                {
                    data = new { ok = false, msg = "Não foi possível realizar a operação, período já apurado." };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }

                foreach (int item in campanhaFaixaAtingimentoModel.CampanhaEstruturaIdArr)
                {
                    if (item > 0)
                    {
                        _campanhaService.CadastrarCampanhaFaixaAtingimento(new CampanhaFaixaAtingimentoModel
                        {
                            Ativo = true,
                            CalculaAtingimentoPercentual = 0,
                            CampanhaEstruturaId = item,
                            CampanhaId = campanhaFaixaAtingimentoModel.CampanhaId,
                            CampanhaPerfilId = campanhaFaixaAtingimentoModel.CampanhaPerfilId,
                            CampanhaPeriodoId = campanhaFaixaAtingimentoModel.CampanhaPeriodoId,
                            DataAlteracao = DateTime.Now,
                            DataInclusao = DateTime.Now,
                            Pontos = campanhaFaixaAtingimentoModel.Pontos,
                            ValorAte = campanhaFaixaAtingimentoModel.ValorAte,
                            ValorDe = campanhaFaixaAtingimentoModel.ValorDe
                        });
                    }
                }

                data = new { ok = true, msg = "Dados salvos com sucesso." };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível salvar os dados. " + exc.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion


        #endregion

        #region "Internal Functions"

        #region "Campanha"

        internal RedirectToRouteResult RedirecionarPorTipoCampanha(int CampanhaId, int TipoCampanhaId)
        {
            switch ((EnumDomain.TipoCampanha)TipoCampanhaId)
            {
                //Meta e Resultado
                case EnumDomain.TipoCampanha.MetaEResultadoParticipante:
                    return RedirectToAction("MetaResultado", "Campanha", new { Id = CampanhaId });
                case EnumDomain.TipoCampanha.MetaEResultadoItens:
                    return RedirectToAction("MetaResultado", "Campanha", new { Id = CampanhaId });
                case EnumDomain.TipoCampanha.MetaEResultadoRankingParticipante:
                    return RedirectToAction("MetaResultadoRanking", "Campanha", new { Id = CampanhaId });
                //Vendeu Ganhou
                case EnumDomain.TipoCampanha.VendeuGanhou:
                    return RedirectToAction("VendeuGanhou", "Campanha", new { Id = CampanhaId });
                case EnumDomain.TipoCampanha.VendeuGanhouRanking:
                    return RedirectToAction("VendeuGanhouRanking", "Campanha", new { Id = CampanhaId });
                //Vendeu Ganhou Calculado
                case EnumDomain.TipoCampanha.VendeuGanhouPorItemPorItemCalculado:
                    return RedirectToAction("VendeuGanhouCalculado", "Campanha", new { Id = CampanhaId });
                case EnumDomain.TipoCampanha.VendeuGanhouPorPessoaCalculado:
                    return RedirectToAction("VendeuGanhouCalculado", "Campanha", new { Id = CampanhaId });
                case EnumDomain.TipoCampanha.VendeuGanhouRankingPorPessoaCalculado:
                    return RedirectToAction("VendeuGanhouCalculado", "Campanha", new { Id = CampanhaId });
                //Meta Resultado Calculado
                case EnumDomain.TipoCampanha.MetaResultadoPorItemCalculado:
                    return RedirectToAction("MetaResultadoCalculado", "Campanha", new { Id = CampanhaId });
                case EnumDomain.TipoCampanha.MetaEResultadoPorPessoaCalculado:
                    return RedirectToAction("MetaResultadoCalculado", "Campanha", new { Id = CampanhaId });
                case EnumDomain.TipoCampanha.MetaEResultadoRankingPorPessoaCalculado:
                    return RedirectToAction("MetaResultadoCalculado", "Campanha", new { Id = CampanhaId });
                default:
                    return RedirectToAction("Consulta", "Campanha");
            }
        }

        internal string BuscarRedirecionarPorTipoCampanha(int CampanhaId, int TipoCampanhaId)
        {
            switch ((EnumDomain.TipoCampanha)TipoCampanhaId)
            {
                //Meta e Resultado
                case EnumDomain.TipoCampanha.MetaEResultadoParticipante:
                    return Url.Action("MetaResultado", "Campanha", new { Id = CampanhaId });
                case EnumDomain.TipoCampanha.MetaEResultadoItens:
                    return Url.Action("MetaResultado", "Campanha", new { Id = CampanhaId });
                case EnumDomain.TipoCampanha.MetaEResultadoRankingParticipante:
                    return Url.Action("MetaResultadoRanking", "Campanha", new { Id = CampanhaId });
                //Vendeu Ganhou
                case EnumDomain.TipoCampanha.VendeuGanhou:
                    return Url.Action("VendeuGanhou", "Campanha", new { Id = CampanhaId });
                case EnumDomain.TipoCampanha.VendeuGanhouRanking:
                    return Url.Action("VendeuGanhouRanking", "Campanha", new { Id = CampanhaId });
                //Vendeu Ganhou Calculado
                case EnumDomain.TipoCampanha.VendeuGanhouPorItemPorItemCalculado:
                    return Url.Action("VendeuGanhouCalculado", "Campanha", new { Id = CampanhaId });
                case EnumDomain.TipoCampanha.VendeuGanhouPorPessoaCalculado:
                    return Url.Action("VendeuGanhouCalculado", "Campanha", new { Id = CampanhaId });
                case EnumDomain.TipoCampanha.VendeuGanhouRankingPorPessoaCalculado:
                    return Url.Action("VendeuGanhouCalculado", "Campanha", new { Id = CampanhaId });
                //Meta e Resultado Calculado
                case EnumDomain.TipoCampanha.MetaResultadoPorItemCalculado:
                    return Url.Action("MetaResultadoCalculado", "Campanha", new { Id = CampanhaId });
                case EnumDomain.TipoCampanha.MetaEResultadoPorPessoaCalculado:
                    return Url.Action("MetaResultadoCalculado", "Campanha", new { Id = CampanhaId });
                case EnumDomain.TipoCampanha.MetaEResultadoRankingPorPessoaCalculado:
                    return Url.Action("MetaResultadoCalculado", "Campanha", new { Id = CampanhaId });
                default:
                    return Url.Action("Consulta", "Campanha");
            }
        }

        internal bool CampanhaPeriodoApurado(int CampanhaPeriodoId)
        {
            bool blnApurado = false;

            blnApurado = _campanhaService.BuscarCampanhaPeriodo(CampanhaPeriodoId).Apurado;

            return blnApurado;
        }

        #endregion

        #region "Vendeu Ganhou"


        #endregion

        #region "Meta e Resultado"


      

        #endregion

        #endregion
    }
}