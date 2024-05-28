using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Enums;
using GrupoLTM.WebSmart.Admin.Models;
using GrupoLTM.WebSmart.Services.Log;
using GrupoLTM.WebSmart.Admin.Attributes;

namespace GrupoLTM.WebSmart.Admin.Controllers
{
    public class HomeController : BaseController
    {
        #region "Actions"

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListaPerfilAdm()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repPerfil = context.CreateRepository<Perfil>();

                var list = repPerfil.Filter<Perfil>(x => x.Adm == true && x.Ativo == true).Select(l => new
                {
                    l.Id,
                    l.Nome
                }).ToList();

                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ListaPeriodo()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repPeriodo = context.CreateRepository<Periodo>();

                var list = repPeriodo.Filter<Periodo>(x => x.Ativo == true).Select(l => new
                {
                    l.Id,
                    l.Nome
                }).ToList();

                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ListaPeriodoCampanha(int? IdCampanha)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repCampanhaPeriodo = context.CreateRepository<CampanhaPeriodo>();

                var list = repCampanhaPeriodo.Filter<CampanhaPeriodo>(x => x.Ativo == true && x.CampanhaId == IdCampanha.Value).Select(l => new
                {
                    l.Id,
                    l.Nome
                }).ToList();

                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ListaTipoPontuacao()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repPeriodo = context.CreateRepository<TipoPontuacao>();
                var list = repPeriodo.Filter<TipoPontuacao>(x => x.FlCampanha == false).Select(l => new
                {
                    l.Id,
                    l.Nome
                }).ToList();

                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ListaTema()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repTema = context.CreateRepository<Tema>();

                var list = repTema.All<Tema>().Select(l => new
                {
                    l.Id,
                    l.Nome,
                    l.Cor
                }).ToList();

                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ListaMenu(int PaiId, int UsuarioAdmId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repMenu = context.CreateRepository<Menu>();
                List<MenuModel> listMenuModel = new List<MenuModel>();
                var intMenu = PaiId;

                foreach (var item in repMenu.Filter<UsuarioAdmMenu>(
                        x => x.Ativo == true
                        && x.UsuarioAdmId == UsuarioAdmId
                        && x.Menu.MenuPaiId == intMenu
                    ).ToList())
                {
                    listMenuModel.Add(new MenuModel
                    {
                        Ativo = item.Menu.Ativo,
                        DataAlteracao = item.Menu.DataAlteracao,
                        DataFim = item.Menu.DataFim,
                        Id = item.Menu.Id,
                        Link = item.Menu.Link,
                        MenuPaiId = item.Menu.MenuPaiId,
                        Nome = item.Menu.Nome,
                        Target = item.Menu.Target,
                        Icone = item.Menu.Icone,
                        Titulo = item.Menu.Titulo
                    });

                    //AddMenuList(listMenuModel, item.Menu.Id, UsuarioAdmId);

                    foreach (var itemFilho in repMenu.Filter<UsuarioAdmMenu>(
                        x => x.Ativo == true
                        && x.UsuarioAdmId == UsuarioAdmId
                        && x.Menu.MenuPaiId == item.Menu.Id
                        ).ToList())
                    {
                        listMenuModel.Add(new MenuModel
                        {
                            Ativo = itemFilho.Menu.Ativo,
                            DataAlteracao = itemFilho.Menu.DataAlteracao,
                            DataFim = itemFilho.Menu.DataFim,
                            Id = itemFilho.Menu.Id,
                            Link = itemFilho.Menu.Link,
                            MenuPaiId = itemFilho.Menu.MenuPaiId,
                            Nome = item.Menu.Nome + " --> " + itemFilho.Menu.Nome,
                            Target = itemFilho.Menu.Target,
                            Icone = itemFilho.Menu.Icone,
                            Titulo = itemFilho.Menu.Titulo
                        });
                    }
                }


                return Json(listMenuModel, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ListaMenuSite()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repMenu = context.CreateRepository<Menu>();
                List<MenuModel> listMenuModel = new List<MenuModel>();

                var Menus = new int?[] {
                    (int?)Domain.Enums.EnumDomain.Menu.MenuPrincipal,
                    (int?)Domain.Enums.EnumDomain.Menu.MenuRedeSocial,
                    (int?)Domain.Enums.EnumDomain.Menu.MenuRodape,
                    (int?)Domain.Enums.EnumDomain.Menu.MenuSuperior
                };

                foreach (var item in repMenu.Filter<Menu>(
                     x => Menus.Contains(x.MenuPaiId)))
                {
                    listMenuModel.Add(new MenuModel
                    {
                        Ativo = item.Ativo,
                        DataAlteracao = item.DataAlteracao,
                        DataFim = item.DataFim,
                        Id = item.Id,
                        Link = item.Link,
                        MenuPaiId = item.MenuPaiId,
                        Nome = item.Nome,
                        Target = item.Target,
                        Icone = item.Icone,
                        Titulo = item.Titulo
                    });

                    foreach (var itemFilho in repMenu.Filter<Menu>(
                        x => x.Ativo == true
                        && x.MenuPaiId == item.Id
                        ).ToList())
                    {
                        listMenuModel.Add(new MenuModel
                        {
                            Ativo = itemFilho.Ativo,
                            DataAlteracao = itemFilho.DataAlteracao,
                            DataFim = itemFilho.DataFim,
                            Id = itemFilho.Id,
                            Link = itemFilho.Link,
                            MenuPaiId = itemFilho.MenuPaiId,
                            Nome = item.Nome + " --> " + itemFilho.Nome,
                            Target = itemFilho.Target,
                            Icone = itemFilho.Icone,
                            Titulo = itemFilho.Titulo
                        });
                    }
                }
                return Json(listMenuModel, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ListaCampanhaSimulador()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository rep = context.CreateRepository<CampanhaSimulador>();

                List<CampanhaSimuladorModel> listCampanhaSimuladorModel = rep.Filter<CampanhaSimulador>(
                        x => x.Ativo == true
                    ).Select(x => new CampanhaSimuladorModel
                    {
                        Ativo = x.Ativo,
                        Id = x.Id,
                        Nome = x.Nome,
                        NumeroCampanha = x.NumeroCampanha
                    }).ToList();

                return Json(listCampanhaSimuladorModel, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ListaMecanicaSimulador()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository rep = context.CreateRepository<MecanicaSimulador>();

                List<MecanicaSimuladorModel> listMecanicaSimuladorModel = rep.Filter<MecanicaSimulador>(
                        x => x.Ativo == true
                    ).Select(x => new MecanicaSimuladorModel
                    {
                        Ativo = x.Ativo,
                        Id = x.Id,
                        Nome = x.Nome
                    }).ToList();

                return Json(listMecanicaSimuladorModel, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ListaSubMecanicaSimulador()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository rep = context.CreateRepository<SubMecanicaSimulador>();

                List<SubMecanicaSimuladorModel> listSubMecanicaSimuladorModel = rep.Filter<SubMecanicaSimulador>(
                        x => x.Ativo == true
                    ).Select(x => new SubMecanicaSimuladorModel
                    {
                        Ativo = x.Ativo,
                        Id = x.Id,
                        Descricao = x.Descricao
                    }).ToList();

                return Json(listSubMecanicaSimuladorModel, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ListaPerfil(int? PaiId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repPerfil = context.CreateRepository<Perfil>();

                var list = repPerfil.Filter<Perfil>(
                    x => x.Adm == false && x.Ativo == true && (x.   PaiId == PaiId || PaiId == 0))
                    .Select(l => new
                    {
                        l.Id,
                        l.Nome
                    }).ToList();

                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ListaGrupoItem(int? PaiId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repGrupoItem = context.CreateRepository<GrupoItem>();

                //var list = repGrupoItem.Filter<GrupoItem>(
                //    x => x.Ativo == true && (x.PaiId == PaiId || PaiId == 0))
                //    .Select(l => new
                //    {
                //        l.Id,
                //        l.Nome
                //    }).ToList();

                var list = repGrupoItem.Filter<GrupoItem>(
                 x => x.Ativo == true && x.PaiId == (int?)null)
                 .Select(l => new
                 {
                     l.Id,
                     l.Nome
                 }).ToList();


                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        //public ActionResult ListaEstrutura(int? PaiId)
        //{
        //    using (IUnitOfWork context = UnitOfWorkFactory.Create())
        //    {
        //        IRepository repEstrutura = context.CreateRepository<Estrutura>();

        //        var list = repEstrutura.Filter<Estrutura>(
        //            x => x.Ativo == true && (x.PaiId == PaiId || PaiId == 0))
        //            .Select(l => new
        //            {
        //                l.Id,
        //                l.Nome
        //            }).ToList();

        //        return Json(list, JsonRequestBehavior.AllowGet);
        //    }
        //}

        public ActionResult ListaEstruturaPorTipo(int? TipoEstruturaId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repEstrutura = context.CreateRepository<Estrutura>();

                var list = repEstrutura.Filter<Estrutura>(
                    x => x.Ativo == true && (x.TipoEstruturaId == TipoEstruturaId || TipoEstruturaId == 0))
                    .Select(l => new
                    {
                        l.Id,
                        l.Nome
                    }).ToList();

                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ListaTipoQuestionario()
        {
            var list = from EnumDomain.TipoQuestionario e in Enum.GetValues(typeof(EnumDomain.TipoQuestionario))
                       select new { Id = e, Nome = GrupoLTM.WebSmart.Infrastructure.Helpers.StringHelper.GetEnumDescription<EnumDomain.TipoQuestionario>(e) };

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListaTipoResposta()
        {
            var list = from EnumDomain.TipoResposta e in Enum.GetValues(typeof(EnumDomain.TipoResposta))
                       select new { Id = e, Nome = GrupoLTM.WebSmart.Infrastructure.Helpers.StringHelper.GetEnumDescription<EnumDomain.TipoResposta>(e) };

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PartialArquivo(EnumDomain.TipoArquivo eTipoArquivo, int? campanhaId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repArquivo = context.CreateRepository<Arquivo>();
                int intTipoArquivoId = Convert.ToInt32(eTipoArquivo);
                var list = repArquivo.Filter<Arquivo>(x => x.TipoArquivoId == intTipoArquivoId && (x.CampanhaId == campanhaId || campanhaId == 0)).OrderBy(x => x.DataInclusao).OrderByDescending(x => x.DataInclusao).ToList();

                List<ArquivoModel> listArquivoModel = new List<ArquivoModel>();
                foreach (var item in list)
                {
                    listArquivoModel.Add(new ArquivoModel
                    {
                        DataInclusao = item.DataInclusao,
                        Id = item.Id,
                        Nome = item.Nome,
                        NomeGerado = item.NomeGerado,
                        StatusArquivo = item.StatusArquivo.Nome,
                        eTipoArquivo = (EnumDomain.TipoArquivo)item.TipoArquivoId
                    });
                }
                return PartialView("PartialArquivo", listArquivoModel);
            }
        }

        //[OutputCache(Duration = 1200, VaryByParam = "none")]
        public ActionResult PartialMenuAdmin()
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    //Variáveis
                    IRepository repMenu = context.CreateRepository<Menu>();
                    List<MenuModel> listMenuModel = new List<MenuModel>();
                    List<MenuUsuarioModel> listAdmUsuarioMenu = new List<MenuUsuarioModel>();

                    //Menu Admin
                    var intMenu = Convert.ToInt32(EnumDomain.Menu.MenuAdmin);

                    //Usuário
                    var usuarioAdmId = Helpers.LoginHelper.GetLoginModel()?.Id ?? 0;

                    //Lista menus ligado ao usuário
                    var list = (from usuarioAdmMenu in repMenu.Filter<UsuarioAdmMenu>(
                            x => x.Ativo == true
                            && x.UsuarioAdmId == usuarioAdmId)
                                select new
                                {
                                    usuarioAdmMenu.Menu.Id,
                                    usuarioAdmMenu.Menu.MenuPaiId,
                                    usuarioAdmMenu.Menu.Nome,
                                    usuarioAdmMenu.Menu.Titulo,
                                    usuarioAdmMenu.Menu.Link,
                                    usuarioAdmMenu.Menu.Target,
                                    usuarioAdmMenu.Menu.Icone,
                                }).ToList();

                    //Adiciona os menu do usuário na MenuUsuarioModel
                    foreach (var item in list)
                    {
                        listAdmUsuarioMenu.Add(new MenuUsuarioModel
                        {
                            Icone = item.Icone,
                            Id = item.Id,
                            Link = item.Link,
                            MenuPaiId = item.MenuPaiId,
                            Nome = item.Nome,
                            Target = item.Target,
                            Titulo = item.Titulo,
                        });
                    }

                    //Lista todos os menus do Pai MenuAdmin
                    var listMenuAdmin = (from menu in repMenu.Filter<Menu>(
                            x => x.Ativo == true
                            && x.MenuPaiId == intMenu
                            && x.UsuarioAdmMenu.Any(y => y.UsuarioAdmId == usuarioAdmId))
                                         select new
                                         {
                                             menu.Id,
                                             menu.MenuPaiId,
                                             menu.Nome,
                                             menu.Titulo,
                                             menu.Link,
                                             menu.Target,
                                             menu.Icone,
                                             menu.Ordem
                                         }).ToList();

                    //Adiciona os menus do Pai Admin
                    foreach (var item in listMenuAdmin.OrderBy(x => x.Ordem))
                    {
                        //Verificar o menu está na lista de menu usuário
                        if (listMenuAdmin.Where(x => x.Id == item.Id).Count() > 0)
                        {
                            listMenuModel.Add(new MenuModel
                            {
                                Id = item.Id,
                                Link = item.Link,
                                MenuPaiId = item.MenuPaiId,
                                Nome = item.Nome,
                                Target = item.Target,
                                Icone = item.Icone,
                                Titulo = item.Titulo
                            });

                            //Adiciona menu list
                            AddMenuList(listAdmUsuarioMenu, listMenuModel, item.Id);
                        }
                    }

                    return PartialView("PartialMenu", listMenuModel);
                }
            }
            catch (Exception ex)
            {
                var log = new LogControllerModel
                {
                    Error = ex.Message,
                    Date = DateTime.Now,
                    Class = "HomeController",
                    Method = "PartialMenuAdmin",
                    StackTrace = ex.StackTrace,
                    Source = ex.ToString()
                };

                //TODO: Update DataDog
                //GrayLogService.LogError(log);

                return Redirect("Index");
            }
        }

        public ActionResult ListaEstado()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repEstado = context.CreateRepository<Estado>();
                var list = repEstado.All<Estado>().Select(l => new
                {
                    l.EstadoId,
                    l.Nome
                }).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ListaTipoEstrutura()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repTipoEstrutura = context.CreateRepository<TipoEstrutura>();
                var list = repTipoEstrutura.Filter<TipoEstrutura>(x => x.Ativo == true).Select(l => new
                {
                    l.Id,
                    l.Nome
                }).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ListaStatusParticipante()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repStatusParticipante = context.CreateRepository<StatusParticipante>();
                var list = repStatusParticipante.All<StatusParticipante>().Select(l => new
                {
                    l.Id,
                    l.Nome
                }).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ListaTipoAcesso()
        {
            var list = from EnumDomain.TipoAcesso e in Enum.GetValues(typeof(EnumDomain.TipoAcesso))
                       select new { Id = e, Nome = GrupoLTM.WebSmart.Infrastructure.Helpers.StringHelper.GetEnumDescription<EnumDomain.TipoAcesso>(e) };

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListaTipoCadastro()
        {
            var list = from EnumDomain.TipoCadastro e in Enum.GetValues(typeof(EnumDomain.TipoCadastro))
                       select new { Id = e, Nome = GrupoLTM.WebSmart.Infrastructure.Helpers.StringHelper.GetEnumDescription<EnumDomain.TipoCadastro>(e) };

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListaTipoValidacaoPositiva()
        {
            var list = from EnumDomain.TipoValidacaoPositiva e in Enum.GetValues(typeof(EnumDomain.TipoValidacaoPositiva))
                       select new { Id = e, Nome = GrupoLTM.WebSmart.Infrastructure.Helpers.StringHelper.GetEnumDescription<EnumDomain.TipoValidacaoPositiva>(e) };

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListaNivelHierarquia()
        {
            Dictionary<int, int> list = new Dictionary<int, int>();
            for (int i = 1; i < 11; i++)
            {
                list.Add(i, i);
            }
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListaTipoCampanha()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repTipoCampanha = context.CreateRepository<TipoCampanha>();

                var list = repTipoCampanha.Filter<TipoCampanha>(
                    x => x.Ativo == true)
                    .Select(l => new
                    {
                        l.Id,
                        l.Nome
                    }).ToList().OrderBy(x => x.Nome);

                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ListaMeses()
        {
            var list = from EnumDomain.Meses e in Enum.GetValues(typeof(EnumDomain.Meses))
                       select new { Id = e, Nome = GrupoLTM.WebSmart.Infrastructure.Helpers.StringHelper.GetEnumDescription<EnumDomain.Meses>(e) };

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region "Actions Posts"


        #endregion

        #region "Internal Functions"

        internal void AddMenuList(List<MenuUsuarioModel> listAdmUsuarioMenu, List<MenuModel> listMenuModel, int MenuPaiId)
        {

            //Adiciona o menu filho, passando o pai
            foreach (var item in listAdmUsuarioMenu.Where(x => x.MenuPaiId == MenuPaiId).ToList())
            {
                listMenuModel.Add(new MenuModel
                {
                    Id = item.Id,
                    Link = item.Link,
                    MenuPaiId = item.MenuPaiId,
                    Nome = item.Nome,
                    Target = item.Target,
                    Icone = item.Icone,
                    Titulo = item.Titulo
                });

                if (listAdmUsuarioMenu.Where(x => x.MenuPaiId == item.Id).Count() > 0)
                {
                    //Recursivo
                    AddMenuList(listAdmUsuarioMenu, listMenuModel, item.Id);
                }
            }
        }

        #endregion
    }
}
