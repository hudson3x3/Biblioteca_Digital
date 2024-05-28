using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrupoLTM.WebSmart.Domain.Enums;
using System.Transactions;
using System.IO;
using System.Net;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using GrupoLTM.WebSmart.Infrastructure.Cache;
using GrupoLTM.WebSmart.Infrastructure.Data;
using GrupoLTM.WebSmart.Infrastructure.Excel;
using GrupoLTM.WebSmart.Infrastructure.Configuration;
using GrupoLTM.WebSmart.Infrastructure.Helpers;

namespace GrupoLTM.WebSmart.Services
{
    public class CampanhaService : BaseService<Campanha>
    {
        #region "Campanha"

        public List<Campanha> ListarCampanhas()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repositoryCampanha = context.CreateRepository<Campanha>();

                return repositoryCampanha.All<Campanha>().ToList();
            }
        }

        public static String ListarNomeStatusCampanhas(int statusCampanhaId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repositoryStatusCampanha = context.CreateRepository<WebSmart.Domain.Models.StatusCampanha>();

                return repositoryStatusCampanha.Filter<WebSmart.Domain.Models.StatusCampanha>(
                    x => (x.Id == statusCampanhaId)
                    ).FirstOrDefault().Nome;
            }
        }

        public List<DTO.CampanhaModel> ListarCampanhasPorTipoStatus(int? tipoCampanhaId, int? statusCampanhaId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repositoryCampanha = context.CreateRepository<Campanha>();

                var list = repositoryCampanha.Filter<Campanha>(
                    x => (x.TipoCampanhaId == tipoCampanhaId || tipoCampanhaId == null)
                    && (x.StatusCampanhaId == statusCampanhaId || statusCampanhaId == null)
                    ).ToList().OrderBy(x => x.Nome);


                List<DTO.CampanhaModel> listCampanhaModel = new List<DTO.CampanhaModel>();

                foreach (var item in list)
                {
                    var tipoCampanha = (item.TipoCampanhaId != null ? item.TipoCampanha.Nome : "");

                    listCampanhaModel.Add(new DTO.CampanhaModel
                    {
                        Id = item.Id,
                        Nome = item.Nome,
                        Descricao = item.Descricao,
                        StatusCampanha = item.StatusCampanha.Nome,
                        TipoCampanha = tipoCampanha,
                        DataInclusao = item.DataInclusao,
                        DataInicio = item.DataInicio,
                        DataFim = item.DataFim,
                    });
                }

                return listCampanhaModel;
            }
        }
        public List<WebSmart.DTO.CampanhaPeriodoModel> ListarCampanhaPeriodoNaoApurado(int CampanhaId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repCampanhaPeriodo = context.CreateRepository<CampanhaPeriodo>();

                var list = repCampanhaPeriodo.Filter<CampanhaPeriodo>(x => x.Ativo == true && x.CampanhaId == CampanhaId && x.Apurado == false).ToList();

                List<DTO.CampanhaPeriodoModel> listCampanhaPeriodoModel = new List<CampanhaPeriodoModel>();

                foreach (var item in list)
                {
                    listCampanhaPeriodoModel.Add(new CampanhaPeriodoModel
                    {
                        CampanhaId = item.CampanhaId,
                        DataAlteracao = item.Dataalteracao,
                        DataFechamento = item.DataFechamento,
                        DataInclusao = item.DataInclusao,
                        Id = item.Id,
                        Nome = item.Nome,
                        PeriodoAte = item.PeriodoAte,
                        PeriodoDe = item.PeriodoDe
                    });
                }

                return listCampanhaPeriodoModel;
            }
        }

        public List<WebSmart.DTO.CampanhaPeriodoModel> ListarCampanhaPeriodo(int CampanhaId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repCampanhaPeriodo = context.CreateRepository<CampanhaPeriodo>();

                var list = repCampanhaPeriodo.Filter<CampanhaPeriodo>(x => x.Ativo == true && x.CampanhaId == CampanhaId).ToList();

                List<DTO.CampanhaPeriodoModel> listCampanhaPeriodoModel = new List<CampanhaPeriodoModel>();

                foreach (var item in list)
                {
                    listCampanhaPeriodoModel.Add(new CampanhaPeriodoModel
                    {
                        CampanhaId = item.CampanhaId,
                        DataAlteracao = item.Dataalteracao,
                        DataFechamento = item.DataFechamento,
                        DataInclusao = item.DataInclusao,
                        Id = item.Id,
                        Nome = item.Nome,
                        PeriodoAte = item.PeriodoAte,
                        PeriodoDe = item.PeriodoDe,
                        Apurado = item.Apurado
                    });
                }

                return listCampanhaPeriodoModel;
            }
        }

        public CampanhaPeriodoModel BuscarCampanhaPeriodo(int CampanhaPeriodoId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repCampanhaPeriodo = context.CreateRepository<CampanhaPeriodo>();

                CampanhaPeriodoModel campanhaPeriodoModel = new CampanhaPeriodoModel();

                var _campanhaPeriodo = repCampanhaPeriodo.Filter<CampanhaPeriodo>(x => x.Id == CampanhaPeriodoId && x.Ativo == true).FirstOrDefault();

                if (_campanhaPeriodo != null)
                {
                    campanhaPeriodoModel.Ativo = _campanhaPeriodo.Ativo;
                    campanhaPeriodoModel.CampanhaId = _campanhaPeriodo.CampanhaId;
                    campanhaPeriodoModel.DataFechamento = _campanhaPeriodo.DataFechamento;
                    campanhaPeriodoModel.DataInclusao = _campanhaPeriodo.DataInclusao;
                    campanhaPeriodoModel.Id = _campanhaPeriodo.Id;
                    campanhaPeriodoModel.Nome = _campanhaPeriodo.Nome;
                    campanhaPeriodoModel.PeriodoAte = _campanhaPeriodo.PeriodoAte;
                    campanhaPeriodoModel.PeriodoDe = _campanhaPeriodo.PeriodoDe;
                    campanhaPeriodoModel.Apurado = Convert.ToBoolean(_campanhaPeriodo.Apurado);
                }
                return campanhaPeriodoModel;

            }
        }

        public List<WebSmart.DTO.CampanhaLogArquivoModel> ListarCampanhaLogArquivo(int CampanhaId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repCampanhaLogArquivo = context.CreateRepository<CampanhaLogArquivo>();
                List<DTO.CampanhaLogArquivoModel> listCampanhaLogArquivoModel = new List<CampanhaLogArquivoModel>();

                //Lista a Campanha
                var _campanha = BuscarCampanha(CampanhaId);

                //Verifica a campanha é não nula
                if (_campanha != null)
                {
                    //Lista o tipo da Campanha
                    int tipoCampanhaId = Convert.ToInt32(_campanha.TipoCampanhaId);

                    //Lista os arquivos por periodo e tipo de campanha
                    var list = repCampanhaLogArquivo.Filter<CampanhaLogArquivo>(
                    x => x.Ativo == true
                    && x.CampanhaId == _campanha.Id
                    && x.Campanha.TipoCampanhaId == tipoCampanhaId
                    ).ToList();

                    foreach (var item in list)
                    {
                        //Adiciona o arquivo de acordo com o Campanha, Tipo de Campanha
                        listCampanhaLogArquivoModel.Add(new CampanhaLogArquivoModel
                        {
                            ArquivoId = item.ArquivoId,
                            CampanhaId = item.CampanhaId,
                            CampanhaPeriodoId = item.CampanhaPeriodoId,
                            CampanhaPeriodo = item.CampanhaPeriodo.Nome,
                            PeriodoDe = item.CampanhaPeriodo.PeriodoDe,
                            PeriodoAte = item.CampanhaPeriodo.PeriodoAte,
                            DataFechamento = item.CampanhaPeriodo.DataFechamento,
                            DataInclusao = item.DataInclusao,
                            Id = item.Id,
                            Nome = item.Arquivo.Nome,
                            NomeGerado = item.Arquivo.NomeGerado,
                            CaminhoArquivo =
                                BuscarCaminhoCampanhaLogArquivo((EnumDomain.TipoCampanha) item.Campanha.TipoCampanhaId,
                                    null),
                            UrlAccess =
                                Settings.Caminho.StoragePath +
                                BuscarCaminhoCampanhaLogArquivo((EnumDomain.TipoCampanha) item.Campanha.TipoCampanhaId,
                                    null) + item.Arquivo.NomeGerado + Settings.Caminho.StorageToken
                        });
                    }

                    return listCampanhaLogArquivoModel;

                }
                else
                {
                    return listCampanhaLogArquivoModel;
                }

            }
        }

        public List<WebSmart.DTO.CampanhaLogArquivoModel> ListarCampanhaLogArquivo(int CampanhaId, int TipoArquivoId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repCampanhaLogArquivo = context.CreateRepository<CampanhaLogArquivo>();
                List<DTO.CampanhaLogArquivoModel> listCampanhaLogArquivoModel = new List<CampanhaLogArquivoModel>();

                //Lista a Campanha
                var _campanha = BuscarCampanha(CampanhaId);

                //Verifica a campanha é não nula
                if (_campanha != null)
                {
                    //Lista o tipo da Campanha
                    int tipoCampanhaId = Convert.ToInt32(_campanha.TipoCampanhaId);

                    //Lista os arquivos por periodo e tipo de campanha
                    var list = repCampanhaLogArquivo.Filter<CampanhaLogArquivo>(
                    x => x.Ativo == true
                    && x.CampanhaId == _campanha.Id
                    && x.Campanha.TipoCampanhaId == tipoCampanhaId
                    && x.Arquivo.TipoArquivoId == TipoArquivoId
                    ).ToList();

                    foreach (var item in list)
                    {
                        //Adiciona o arquivo de acordo com o Campanha, Tipo de Campanha
                        listCampanhaLogArquivoModel.Add(new CampanhaLogArquivoModel
                        {
                            ArquivoId = item.ArquivoId,
                            CampanhaId = item.CampanhaId,
                            CampanhaPeriodoId = item.CampanhaPeriodoId,
                            CampanhaPeriodo = item.CampanhaPeriodo.Nome,
                            PeriodoDe = item.CampanhaPeriodo.PeriodoDe,
                            PeriodoAte = item.CampanhaPeriodo.PeriodoAte,
                            DataFechamento = item.CampanhaPeriodo.DataFechamento,
                            DataInclusao = item.DataInclusao,
                            Id = item.Id,
                            Nome = item.Arquivo.Nome,
                            NomeGerado = item.Arquivo.NomeGerado,
                            CaminhoArquivo =
                                BuscarCaminhoCampanhaLogArquivo((EnumDomain.TipoCampanha) item.Campanha.TipoCampanhaId,
                                    TipoArquivoId),
                            UrlAccess =
                                Settings.Caminho.StoragePath +
                                BuscarCaminhoCampanhaLogArquivo((EnumDomain.TipoCampanha) item.Campanha.TipoCampanhaId,
                                    TipoArquivoId) + item.Arquivo.NomeGerado + Settings.Caminho.StorageToken
                        });
                    }

                    return listCampanhaLogArquivoModel;

                }
                else
                {
                    return listCampanhaLogArquivoModel;
                }

            }
        }

        public void CadastrarCampanhaPeriodo(CampanhaPeriodoModel campanhaPeriodoModel)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repositoryCampanhaPeriodo = context.CreateRepository<CampanhaPeriodo>();

                CampanhaPeriodo _campanhaPeridodo = new CampanhaPeriodo
                {
                    Ativo = true,
                    CampanhaId = campanhaPeriodoModel.CampanhaId,
                    Dataalteracao = DateTime.Now,
                    DataInclusao = DateTime.Now,
                    DataFechamento = Convert.ToDateTime(campanhaPeriodoModel.DataFechamento),
                    PeriodoAte = Convert.ToDateTime(campanhaPeriodoModel.PeriodoAte),
                    PeriodoDe = Convert.ToDateTime(campanhaPeriodoModel.PeriodoDe),
                    Nome = campanhaPeriodoModel.Nome
                };

                using (TransactionScope scope = new TransactionScope())
                {
                    repositoryCampanhaPeriodo.Create(_campanhaPeridodo);
                    repositoryCampanhaPeriodo.SaveChanges();
                    scope.Complete();
                }
            }
        }

        public CampanhaConteudoModel ObterRegulamento(int id)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                var model = new CampanhaConteudoModel();

                IRepository repositoryCampanhaConteudo = context.CreateRepository<CampanhaConteudo>();

                var regulamento = repositoryCampanhaConteudo.Filter<CampanhaConteudo>(x => x.CampanhaId == id && x.Ativo && x.TipoconteudoId == (int)EnumDomain.TipoConteudo.Regulamento).FirstOrDefault();

                if (regulamento == null) throw new KeyNotFoundException(string.Format("Campanha com o Id {0} não encontrada.", id));

                model.Id = regulamento.Id;
                model.Arquivo = regulamento.Arquivo;
                model.Ativo = regulamento.Ativo;
                model.CampanhaId = regulamento.CampanhaId;
                model.Nome = regulamento.Nome;
                model.Ordem = regulamento.Ordem ?? 0;
                model.PreTexto = regulamento.PreTexto;
                model.Texto = regulamento.Texto;
                model.TipoConteudoId = regulamento.TipoconteudoId;
                model.Titulo = regulamento.Titulo;

                return model;
            }
        }

        public CampanhaConteudoModel ObterConteudo(int CampanhaId, EnumDomain.TipoConteudo tipoConteudo)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                var model = new CampanhaConteudoModel();

                IRepository repositoryCampanhaConteudo = context.CreateRepository<CampanhaConteudo>();

                var conteudo = repositoryCampanhaConteudo.Filter<CampanhaConteudo>(x => x.CampanhaId == CampanhaId && x.Ativo && x.TipoconteudoId == (int)tipoConteudo).FirstOrDefault();

                if (conteudo == null) throw new KeyNotFoundException(string.Format("Campanha com o Id {0} não encontrada.", CampanhaId));

                model.Id = conteudo.Id;
                model.Arquivo = conteudo.Arquivo;
                model.Ativo = conteudo.Ativo;
                model.CampanhaId = conteudo.CampanhaId;
                model.Nome = conteudo.Nome;
                model.Ordem = conteudo.Ordem ?? 0;
                model.PreTexto = conteudo.PreTexto;
                model.Texto = conteudo.Texto;
                model.TipoConteudoId = conteudo.TipoconteudoId;
                model.Titulo = conteudo.Titulo;

                return model;
            }
        }

        public void InativarCampanhaPeriodo(int Id)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repositoryCampanhaPeriodo = context.CreateRepository<CampanhaPeriodo>();

                var _campanhaPeriodo = repositoryCampanhaPeriodo.Find<CampanhaPeriodo>(Id);

                if (_campanhaPeriodo == null) throw new KeyNotFoundException(string.Format("Campanha Período com o Id {0} não encontrada.", _campanhaPeriodo.Id));

                if (_campanhaPeriodo.Id == 0) throw new KeyNotFoundException(string.Format("Campanha Período o Id {0} não encontrada.", _campanhaPeriodo.Id));

                if (_campanhaPeriodo != null)
                {
                    _campanhaPeriodo.Ativo = false;
                }

                repositoryCampanhaPeriodo.Update<CampanhaPeriodo>(_campanhaPeriodo);
                repositoryCampanhaPeriodo.SaveChanges();
            }
        }

        public Campanha BuscarCampanha(int campanhaId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repositoryCampanha = context.CreateRepository<Campanha>();

                var campanha = repositoryCampanha.Find<Campanha>(x => x.Id == campanhaId);

                return campanha;
            }
        }

        public bool CampanhaPublicada(int CampanhaId)
        {
            var campanha = BuscarCampanha(CampanhaId);

            if (campanha == null)
            {
                return false;
            }

            if (campanha.StatusCampanhaId == (int)EnumDomain.StatusCampanha.Publicada)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public CampanhaPasso BuscarUltimoPasso(int campanhaId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repositoryCampanhaPasso = context.CreateRepository<CampanhaPasso>();

                var passosDaCampanha = repositoryCampanhaPasso.Filter<CampanhaPasso>(x => x.CampanhaId == campanhaId && x.Ativo).ToList();

                if (passosDaCampanha == null) throw new KeyNotFoundException(string.Format("Campanha sem passo configurado.", campanhaId));
                if (passosDaCampanha.Count == 0) throw new KeyNotFoundException(string.Format("Campanha sem passo configurado.", campanhaId));

                var idUltimoPasso = passosDaCampanha.Max(x => x.Id);

                var campanhaPasso = repositoryCampanhaPasso.Find<CampanhaPasso>(x => x.Id == idUltimoPasso);

                if (campanhaPasso == null) throw new KeyNotFoundException(string.Format("Campanha com o Id {0} não encontrada.", campanhaId));

                return campanhaPasso;
            }
        }

        public List<WebSmart.DTO.CampanhaModel> ListarCampanhasPorPagina(int perfilId, int estruturaId, int pagina, int quantidadeRegistros, out int total)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repConteudo = context.CreateRepository<Campanha>();
                IRepository repCampanhaConteudo = context.CreateRepository<CampanhaConteudo>();

                List<WebSmart.DTO.CampanhaModel> campanhaModel = new List<WebSmart.DTO.CampanhaModel>();
                List<WebSmart.Domain.Models.Campanha> campanhas = new List<WebSmart.Domain.Models.Campanha>();

                campanhas = repConteudo.Filter<Campanha>(x => x.StatusCampanhaId == (int)EnumDomain.StatusCampanha.Publicada
                    && x.CampanhaPerfil.Any(cp => cp.PerfilId == perfilId && cp.Ativo)
                    && x.CampanhaEstrutura.Any(ce => ce.EstruturaId == estruturaId && ce.Ativo)
                    && (x.DataInicio == null || DateTime.Now > x.DataInicio && x.DataFim > DateTime.Now)
                    , out total, pagina, quantidadeRegistros)
                    .OrderBy(x => x.DataFim)
                    .ToList();

                foreach (var item in campanhas)
                {
                    var nomeImagemBanner = string.Empty;
                    var bannerHome = repCampanhaConteudo.Filter<CampanhaConteudo>(x => x.CampanhaId == item.Id && x.Ativo && x.TipoconteudoId == (int)EnumDomain.TipoConteudo.BannerHome).FirstOrDefault();

                    if (bannerHome != null)
                        if (!(string.IsNullOrWhiteSpace(bannerHome.Arquivo)))
                            nomeImagemBanner = bannerHome.Arquivo;

                    campanhaModel.Add(new CampanhaModel
                    {
                        Id = item.Id,
                        Descricao = item.Descricao.Substring(0, Math.Min(255, item.Descricao.Length)),
                        DataFim = item.DataFim,
                        DataInclusao = item.DataInclusao,
                        DataInicio = item.DataInicio,
                        Nome = item.Nome,
                        StatusCampanhaId = item.StatusCampanhaId,
                        TipoCampanhaId = item.TipoCampanhaId,
                        ImagemBannerHome =
                            Settings.Caminho.StoragePath + "banner/" + nomeImagemBanner + Settings.Caminho.StorageToken,
                        Url = StringHelper.URLFriendly(item.Nome)
                    });
                }
                return campanhaModel;
            }
        }

        public List<WebSmart.DTO.CampanhaModel> ListarCampanhas(int perfilId, int estruturaId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repConteudo = context.CreateRepository<Campanha>();
                IRepository repCampanhaConteudo = context.CreateRepository<CampanhaConteudo>();

                List<WebSmart.DTO.CampanhaModel> campanhaModel = new List<WebSmart.DTO.CampanhaModel>();
                List<WebSmart.Domain.Models.Campanha> campanhas = new List<WebSmart.Domain.Models.Campanha>();

                campanhas = repConteudo.Filter<Campanha>(x => x.StatusCampanhaId == (int)EnumDomain.StatusCampanha.Publicada
                    && x.CampanhaPerfil.Any(cp => cp.PerfilId == perfilId && cp.Ativo)
                    && x.CampanhaEstrutura.Any(ce => ce.EstruturaId == estruturaId && ce.Ativo)
                    && (x.DataInicio == null || DateTime.Now > x.DataInicio && x.DataFim > DateTime.Now)
                    )
                    .OrderBy(x => x.DataFim)
                    .ToList();

                foreach (var item in campanhas)
                {
                    var nomeImagemBanner = string.Empty;
                    var bannerHome = repCampanhaConteudo.Filter<CampanhaConteudo>(x => x.CampanhaId == item.Id && x.Ativo && x.TipoconteudoId == (int)EnumDomain.TipoConteudo.BannerHome).FirstOrDefault();

                    if (bannerHome != null)
                        if (!(string.IsNullOrWhiteSpace(bannerHome.Arquivo)))
                            nomeImagemBanner = bannerHome.Arquivo;

                    campanhaModel.Add(new CampanhaModel
                    {
                        Id = item.Id,
                        Descricao = item.Descricao.Substring(0, Math.Min(30, item.Descricao.Length)),
                        DataFim = item.DataFim,
                        DataInclusao = item.DataInclusao,
                        DataInicio = item.DataInicio,
                        Nome = item.Nome,
                        StatusCampanhaId = item.StatusCampanhaId,
                        TipoCampanhaId = item.TipoCampanhaId,
                        ImagemBannerHome = Settings.Caminho.StoragePath + "banner/" + nomeImagemBanner + Settings.Caminho.StorageToken,
                        Url = StringHelper.URLFriendly(item.Nome)
                    });
                }
                return campanhaModel;
            }
        }

        public void IncluirPasso(int campanhaId, int passoId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository rep = context.CreateRepository<CampanhaPasso>();

                var _passo = rep.Find<CampanhaPasso>(x => x.CampanhaId == campanhaId && x.PassoId == passoId);

                if (_passo == null)
                {
                    CampanhaPasso _campanha = new CampanhaPasso
                    {
                        Ativo = true,
                        CampanhaId = campanhaId,
                        DataInclusao = DateTime.Now,
                        PassoId = passoId
                    };

                    rep.Create(_campanha);
                    rep.SaveChanges();
                }
                else
                {
                    _passo.DataAtualizacao = DateTime.Now;
                    rep.Update<CampanhaPasso>(_passo);
                    rep.SaveChanges();
                }
            }
        }

        public Campanha AtualizarCampanha(CampanhaModel campanha)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repositoryCampanha = context.CreateRepository<Campanha>();

                var _campanha = repositoryCampanha.Find<Campanha>(x => x.Id == campanha.Id);

                if (_campanha == null) throw new KeyNotFoundException(string.Format("Campanha com o Id {0} não encontrada.", campanha.Id));

                if (_campanha.Id == 0) throw new KeyNotFoundException(string.Format("Campanha com o Id {0} não encontrada.", campanha.Id));

                if (campanha.Nome != null)
                    _campanha.Nome = campanha.Nome;

                if (campanha.DataFim != DateTime.MinValue)
                    _campanha.DataFim = campanha.DataFim;

                if (campanha.DataInicio != DateTime.MinValue)
                    _campanha.DataInicio = campanha.DataInicio;

                if (campanha.Descricao != null)
                    _campanha.Descricao = campanha.Descricao;

                if (campanha.TipoCampanhaId != null)
                    _campanha.TipoCampanhaId = campanha.TipoCampanhaId;

                if (campanha.DataAlteracao != null)
                    _campanha.Dataalteracao = campanha.DataAlteracao;

                if (campanha.StatusCampanhaId != null)
                    _campanha.StatusCampanhaId = Convert.ToInt32(campanha.StatusCampanhaId);

                if (campanha.ResultadoCascata != null)
                    _campanha.ResultadoCascata = Convert.ToBoolean(campanha.ResultadoCascata);

                if (campanha.ExibirRankingIndividual != null)
                    _campanha.ExibirRankingIndividual = Convert.ToBoolean(campanha.ExibirRankingIndividual);

                repositoryCampanha.Update<Campanha>(_campanha);
                repositoryCampanha.SaveChanges();

                return _campanha;
            }
        }

        public Campanha SalvarCampanha(CampanhaModel campanha)
        {

            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository rep = context.CreateRepository<Campanha>();
                Campanha _campanha = new Campanha
                {
                    DataInicio = campanha.DataInicio,
                    DataFim = campanha.DataFim,
                    Nome = campanha.Nome,
                    Descricao = campanha.Descricao,
                    StatusCampanhaId = Convert.ToInt32(campanha.StatusCampanhaId),
                    TipoCampanhaId = campanha.TipoCampanhaId,
                    DataInclusao = DateTime.Now,
                    ResultadoCascata = Convert.ToBoolean(campanha.ResultadoCascata)
                };

                var campanhaCadastrada = rep.Create(_campanha);
                rep.SaveChanges();

                return campanhaCadastrada;
            }
        }

        public void CadastrarCampanhaConteudo(CampanhaConteudoModel campanhaConteudo)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository rep = context.CreateRepository<Campanha>();
                CampanhaConteudo _campanhaConteudoBanner = new CampanhaConteudo
                {
                    CampanhaId = campanhaConteudo.CampanhaId,
                    Nome = campanhaConteudo.Nome,
                    TipoconteudoId = campanhaConteudo.TipoConteudoId,
                    Ordem = campanhaConteudo.Ordem,
                    Ativo = campanhaConteudo.Ativo,
                    Arquivo = campanhaConteudo.Arquivo,
                    Texto = campanhaConteudo.Texto
                };

                using (TransactionScope scope = new TransactionScope())
                {
                    rep.Create(_campanhaConteudoBanner);
                    rep.SaveChanges();
                    scope.Complete();
                }
            }
        }

        public CampanhaConteudo BuscarCampanhaConteudo(int campanhaId, int tipoConteudoId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repositoryCampanhaConteudo = context.CreateRepository<CampanhaConteudo>();

                var campanhaConteudo = repositoryCampanhaConteudo.Find<CampanhaConteudo>(x => x.CampanhaId == campanhaId && x.TipoconteudoId == tipoConteudoId
                   && x.Ativo);

                if (campanhaConteudo != null)
                    return campanhaConteudo;

                return new CampanhaConteudo();

            }
        }

        public List<WebSmart.DTO.CampanhaEstruturaModel> BuscarCampanhaEstrutura(int campanhaId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {

                List<CampanhaEstruturaModel> CampanhaEstrutura = new List<CampanhaEstruturaModel>();

                IRepository repositoryCampanhaEstrutura = context.CreateRepository<CampanhaEstrutura>();

                var _campanhaEstruturas = repositoryCampanhaEstrutura.Filter<CampanhaEstrutura>(x => x.CampanhaId == campanhaId && x.Ativo == true);

                foreach (CampanhaEstrutura campanhaEstrutura in _campanhaEstruturas)
                {
                    CampanhaEstrutura.Add(new CampanhaEstruturaModel
                    {
                        Id = campanhaEstrutura.Id,
                        Nome = campanhaEstrutura.Estrutura.Nome,
                        CampanhaId = campanhaEstrutura.CampanhaId,
                        EstruturaId = campanhaEstrutura.EstruturaId,
                        Ativo = campanhaEstrutura.Ativo,
                        DataAlteracao = campanhaEstrutura.DataAlteracao,
                        DataInclusao = campanhaEstrutura.DataInclusao,
                        TipoEstrutura = campanhaEstrutura.Estrutura.TipoEstruturaId.Value,
                        Participa = campanhaEstrutura.Participa
                    });
                }

                return CampanhaEstrutura;

            }
        }

        public List<WebSmart.DTO.CampanhaEstruturaModel> BuscarCampanhaEstruturaParticipante(int campanhaId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {

                List<CampanhaEstruturaModel> CampanhaEstrutura = new List<CampanhaEstruturaModel>();

                IRepository repositoryCampanhaEstrutura = context.CreateRepository<CampanhaEstrutura>();

                var _campanhaEstruturas = repositoryCampanhaEstrutura.Filter<CampanhaEstrutura>(x => x.CampanhaId == campanhaId && x.Ativo == true && x.Participa);

                foreach (CampanhaEstrutura campanhaEstrutura in _campanhaEstruturas)
                {
                    CampanhaEstrutura.Add(new CampanhaEstruturaModel
                    {
                        Id = campanhaEstrutura.Id,
                        Nome = campanhaEstrutura.Estrutura.Nome,
                        CampanhaId = campanhaEstrutura.CampanhaId,
                        EstruturaId = campanhaEstrutura.EstruturaId,
                        Ativo = campanhaEstrutura.Ativo,
                        DataAlteracao = campanhaEstrutura.DataAlteracao,
                        DataInclusao = campanhaEstrutura.DataInclusao,
                        TipoEstrutura = campanhaEstrutura.Estrutura.TipoEstruturaId.Value,
                        Participa = campanhaEstrutura.Participa
                    });
                }

                return CampanhaEstrutura;

            }
        }



        public List<WebSmart.DTO.CampanhaPerfilModel> BuscarCampanhaPerfil(int campanhaId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {

                List<CampanhaPerfilModel> CampanhaEstrutura = new List<CampanhaPerfilModel>();

                IRepository repositoryCampanhaPerfil = context.CreateRepository<CampanhaPerfil>();

                var _campanhaPerfis = repositoryCampanhaPerfil.Filter<CampanhaPerfil>(x => x.CampanhaId == campanhaId && x.Ativo == true);


                foreach (CampanhaPerfil campanhaPerfil in _campanhaPerfis)
                {
                    CampanhaEstrutura.Add(new CampanhaPerfilModel
                    {
                        Id = campanhaPerfil.Id,
                        Nome = campanhaPerfil.Perfil.Nome,
                        CampanhaId = campanhaPerfil.CampanhaId,
                        PerfilId = campanhaPerfil.PerfilId,
                        Ativo = campanhaPerfil.Ativo,
                        DataAlteracao = campanhaPerfil.DataAlteracao,
                        DataInclusao = campanhaPerfil.DataInclusao,
                        Participa = campanhaPerfil.Participa
                    });
                };

                return CampanhaEstrutura;

            }
        }

        public List<WebSmart.DTO.CampanhaPerfilModel> BuscarCampanhaPerfilParticipante(int campanhaId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {

                List<CampanhaPerfilModel> CampanhaEstrutura = new List<CampanhaPerfilModel>();

                IRepository repositoryCampanhaPerfil = context.CreateRepository<CampanhaPerfil>();

                var _campanhaPerfis = repositoryCampanhaPerfil.Filter<CampanhaPerfil>(x => x.CampanhaId == campanhaId && x.Ativo == true && x.Participa);


                foreach (CampanhaPerfil campanhaPerfil in _campanhaPerfis)
                {
                    CampanhaEstrutura.Add(new CampanhaPerfilModel
                    {
                        Id = campanhaPerfil.Id,
                        Nome = campanhaPerfil.Perfil.Nome,
                        CampanhaId = campanhaPerfil.CampanhaId,
                        PerfilId = campanhaPerfil.PerfilId,
                        Ativo = campanhaPerfil.Ativo,
                        DataAlteracao = campanhaPerfil.DataAlteracao,
                        DataInclusao = campanhaPerfil.DataInclusao,
                        Participa = campanhaPerfil.Participa
                    });
                };

                return CampanhaEstrutura;

            }
        }

        public void InativarCampanhaConteudo(int campanhaId, int tipoConteudoId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repositoryCampanhaConteudo = context.CreateRepository<CampanhaConteudo>();

                var _campanhaConteudo = BuscarCampanhaConteudo(campanhaId, tipoConteudoId);

                if (_campanhaConteudo.Id > 0)
                {
                    _campanhaConteudo.Ativo = false;
                    repositoryCampanhaConteudo.Update<CampanhaConteudo>(_campanhaConteudo);
                    repositoryCampanhaConteudo.SaveChanges();
                }

            }
        }

        public void SalvarAtualizarCampanhaEstrutura(int idCampanha, int idTipoestrutra, int[] EstruturaId, int[] EstruturaViewId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repositoryCamapnhaEstrutura = context.CreateRepository<CampanhaEstrutura>();

                //Verifica se já existem Campanhas Cadastradas antes de Salvar

                var _campanhaEstrutura = BuscarCampanhaEstrutura(idCampanha);

                //se encontradas campanhas estruturas ativas, as mesmas devem ser destivadas antes da inserção das novas
                if (_campanhaEstrutura.Count > 0)
                {
                    foreach (CampanhaEstruturaModel campanhaEsturura in _campanhaEstrutura)
                    {
                        //Consulta o Id da campanha estrutura
                        var _esturuturaCampanha = repositoryCamapnhaEstrutura.Find<CampanhaEstrutura>(x => x.Id == campanhaEsturura.Id);
                        _esturuturaCampanha.Ativo = false;
                        repositoryCamapnhaEstrutura.Update<CampanhaEstrutura>(_esturuturaCampanha);
                    }

                }
                //Insere as novas linhas de esturtura (Estruturas que Participam)
                foreach (int id in EstruturaId)
                {
                    CampanhaEstrutura estrutura = new CampanhaEstrutura();
                    estrutura.CampanhaId = idCampanha;
                    estrutura.EstruturaId = id;
                    estrutura.Ativo = true;
                    estrutura.Participa = true;
                    estrutura.DataInclusao = System.DateTime.Now;
                    estrutura.DataAlteracao = System.DateTime.Now;
                    repositoryCamapnhaEstrutura.Create<CampanhaEstrutura>(estrutura);
                }

                //Insere as novas linhas de esturtura (Estruturas que Visualizam)
                if (EstruturaViewId != null)
                {
                    foreach (int id in EstruturaViewId)
                    {
                        CampanhaEstrutura estrutura = new CampanhaEstrutura();
                        estrutura.CampanhaId = idCampanha;
                        estrutura.EstruturaId = id;
                        estrutura.Ativo = true;
                        estrutura.Participa = false;
                        estrutura.DataInclusao = System.DateTime.Now;
                        estrutura.DataAlteracao = System.DateTime.Now;
                        repositoryCamapnhaEstrutura.Create<CampanhaEstrutura>(estrutura);
                    }
                }

                repositoryCamapnhaEstrutura.SaveChanges();
            }

        }

        public void SalvarAtualizarCampanhaPerfil(int idCampanha, int[] PerfilId, int[] PerfilViewId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repositoryCamapnhaPerfil = context.CreateRepository<CampanhaPerfil>();

                //Verifica se já existem Campanhas Cadastradas antes de Salvar

                var _campanhaPerfil = BuscarCampanhaPerfil(idCampanha);

                //se encontradas campanhas estruturas ativas, as mesmas devem ser destivadas antes da inserção das novas
                if (_campanhaPerfil.Count > 0)
                {
                    foreach (CampanhaPerfilModel campanhaPerfil in _campanhaPerfil)
                    {
                        //Consulta o Id da campanha estrutura
                        var _perfilCampanha = repositoryCamapnhaPerfil.Find<CampanhaPerfil>(x => x.Id == campanhaPerfil.Id);
                        _perfilCampanha.Ativo = false;
                        repositoryCamapnhaPerfil.Update<CampanhaPerfil>(_perfilCampanha);
                    }

                }
                //Insere as novas linhas de esturtura (Quem Participa)
                foreach (int id in PerfilId)
                {
                    CampanhaPerfil perfil = new CampanhaPerfil();
                    perfil.CampanhaId = idCampanha;
                    perfil.PerfilId = id;
                    perfil.Ativo = true;
                    perfil.DataInclusao = System.DateTime.Now;
                    perfil.DataAlteracao = System.DateTime.Now;
                    perfil.Participa = true;
                    repositoryCamapnhaPerfil.Create<CampanhaPerfil>(perfil);
                }

                //Insere as novas linhas de esturtura (Quem Visualiza)
                if (PerfilViewId != null)
                {
                    foreach (int id in PerfilViewId)
                    {
                        CampanhaPerfil perfil = new CampanhaPerfil();
                        perfil.CampanhaId = idCampanha;
                        perfil.PerfilId = id;
                        perfil.Ativo = true;
                        perfil.DataInclusao = System.DateTime.Now;
                        perfil.DataAlteracao = System.DateTime.Now;
                        perfil.Participa = false;
                        repositoryCamapnhaPerfil.Create<CampanhaPerfil>(perfil);
                    }
                }

                repositoryCamapnhaPerfil.SaveChanges();
            }

        }

        //protected SelectList getCampanha(bool isMarketPlaceIdReturned = false)
        //{
        //    List<SelectListItem> itens = new List<SelectListItem>();

        //    var retorno = _campanhaService.ListarCatalogo().Where(x => x.Id != 1).OrderBy(x => x.Id).ToList();

        //    //retorno.ForEach(catalogo =>
        //    //{
        //    //    itens.Add(new SelectListItem()
        //    //    {
        //    //        Value = isMarketPlaceIdReturned ? catalogo.MktPlaceCatalogoId.ToString() : catalogo.Id.ToString(),
        //    //        Text = catalogo.Nome
        //    //    });
        //    //});

        //    return isMarketPlaceIdReturned ? new SelectList(retorno, "MktPlaceCatalogoId", "Nome") : new SelectList(retorno, "Id", "Nome");
        //}

        #endregion

        #region "Meta e Resultado"

        public static bool ImportarArquivoMetaPessoa(DataTable dtMetaPessoa, int ArquivoId, int CampanhaId, int CampanhaPeriodoId)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString))
            {
                bulkCopy.DestinationTableName =
                    "dbo.CampanhaMetaPessoaImportacao";
                try
                {
                    dtMetaPessoa.Columns.Add("ArquivoId");
                    dtMetaPessoa.Columns.Add("CampanhaId");
                    dtMetaPessoa.Columns.Add("PeriodoId");
                    foreach (DataRow dr in dtMetaPessoa.Rows)
                    {
                        dr["CampanhaId"] = CampanhaId;
                        dr["ArquivoId"] = ArquivoId;
                        dr["PeriodoId"] = CampanhaPeriodoId;
                    }
                    bulkCopy.WriteToServer(dtMetaPessoa);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public static bool ProcessaMetaGrupoItemArquivo(int ArquivoId, int CampanhaPeriodoId, out int countErro, int usuarioId, int CampanhaId)
        {
            countErro = 0;
            bool blnSucesso = false;

            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Imp_CampanhaMetaGrupoItem";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@ImportadoSucesso", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output });
            listParam.Add(new SqlParameter { ParameterName = "@CountErro", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output });
            listParam.Add(new SqlParameter { ParameterName = "@UsuarioAdmId", Value = usuarioId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@ArquivoId", Value = ArquivoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@CampanhaPeriodoId", Value = CampanhaPeriodoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@CampanhaId", Value = CampanhaId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            DataProvider.NonqueryProc(proc, listParam);

            if ((Boolean)listParam[0].Value)
            {
                blnSucesso = true;
            }
            else
            {
                blnSucesso = false;
            }
            countErro = (int)listParam[1].Value;

            return blnSucesso;
        }

        public static bool ProcessaFaixaGrupoItemArquivo(int ArquivoId, int CampanhaPeriodoId, out int countErro, int usuarioId, int CampanhaId)
        {
            countErro = 0;
            bool blnSucesso = false;

            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Imp_CampanhaFaixaGrupoItem";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@ImportadoSucesso", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output });
            listParam.Add(new SqlParameter { ParameterName = "@CountErro", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output });
            listParam.Add(new SqlParameter { ParameterName = "@UsuarioAdmId", Value = usuarioId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@ArquivoId", Value = ArquivoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@CampanhaPeriodoId", Value = CampanhaPeriodoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@CampanhaId", Value = CampanhaId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            DataProvider.NonqueryProc(proc, listParam);

            if ((Boolean)listParam[0].Value)
            {
                blnSucesso = true;
            }
            else
            {
                blnSucesso = false;
            }
            countErro = (int)listParam[1].Value;

            return blnSucesso;
        }

        public static bool ProcessaMetaPessoaArquivo(int ArquivoId, int periodoId, out int countErro, int usuarioId, int CampanhaId)
        {
            countErro = 0;
            bool blnSucesso = false;

            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Imp_CampanhaMetaPessoa";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@ImportadoSucesso", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output });
            listParam.Add(new SqlParameter { ParameterName = "@CountErro", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output });
            listParam.Add(new SqlParameter { ParameterName = "@UsuarioAdmId", Value = usuarioId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@ArquivoId", Value = ArquivoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@PeriodoId", Value = periodoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@CampanhaId", Value = CampanhaId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            DataProvider.NonqueryProc(proc, listParam);

            if ((Boolean)listParam[0].Value)
            {
                blnSucesso = true;
            }
            else
            {
                blnSucesso = false;
            }
            countErro = (int)listParam[1].Value;

            return blnSucesso;
        }

        public static DataTable AprovaPontuacaoCampanha(int campanhaId, int campanhaPeriodoId, int usuarioId, int tipoPontuacaoId, int statusPontuacaoId)
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Ins_AprovarPontuacaoCampanha";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@CampanhaId", Value = campanhaId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@CampanhaPeriodoId", Value = campanhaPeriodoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@UsuarioId", Value = usuarioId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@TipoPontuacaoId", Value = tipoPontuacaoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@StatusPontuacao", Value = statusPontuacaoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            DataTable table = DataProvider.SelectStoreProcedure(proc, listParam);

            return table;
        }

        public static List<AprovacaoResultadoCalculadoModel> SelecionaResultadoCalculadoAprovao(int campanhaId, int campanhaPeriodoId)
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Sel_AprovacaoResultadoCalculado";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@CampanhaId", Value = campanhaId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@CampanhaPeriodoId", Value = campanhaPeriodoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });

            DataTable table = DataProvider.SelectStoreProcedure(proc, listParam);

            var resultado = table.AsEnumerable()
                                .Select(row => new AprovacaoResultadoCalculadoModel
                                {
                                    CampanhaResultadoCalculadoParticipanteId = row.Field<int>(0),
                                    CampanhaId = row.Field<int>(1),
                                    CampanhaPeriodoId = row.Field<int>(2),
                                    PeriodoNome = row.Field<string>(3),
                                    Periodo = row.Field<string>(4),
                                    Perfil = row.Field<string>(5),
                                    Item = row.Field<string>(6),
                                    Login = row.Field<string>(7),
                                    Nome = row.Field<string>(8),
                                    Posicao = row.Field<string>(9),
                                    Meta = row.Field<string>(10),
                                    Efetivo = row.Field<string>(11),
                                    Pontos = row.Field<double>(12)
                                })
                                .ToList();
            return resultado;
        }


        public static string ExportaArquivoMetaPessoaErro(int ArquivoId)
        {
            string strDownloadArquivoErro = "";

            DataSet ds = new DataSet();
            ds.Tables.Add(ListaImportacaoMetaPessoaErro(ArquivoId));

            string strArquivoGerado = "";
            ExcelExport.WriteXLSFile(
                ds,
                "campanha/metaresultado/metapessoa/erro/",
                out strArquivoGerado);

            strDownloadArquivoErro = "campanha/metaresultado/metapessoa/erro/" + strArquivoGerado;

            return strDownloadArquivoErro;
        }

        public static string ExportaArquivoMetaGrupoItemErro(int ArquivoId)
        {
            string strDownloadArquivoErro = "";

            DataSet ds = new DataSet();
            ds.Tables.Add(ListaImportacaoMetaGrupoItemErro(ArquivoId));

            string strArquivoGerado = "";
            ExcelExport.WriteXLSFile(
                ds,
                "campanha/metaresultado/grupoitem/erro/",
                out strArquivoGerado);

            strDownloadArquivoErro = "campanha/metaresultado/grupoitem/erro/" + strArquivoGerado;

            return strDownloadArquivoErro;
        }

        public static string ExportaArquivoFaixaGrupoItemErro(int ArquivoId)
        {
            string strDownloadArquivoErro = "";

            DataSet ds = new DataSet();
            ds.Tables.Add(ListaImportacaoFaixaGrupoItemErro(ArquivoId));

            string strArquivoGerado = "";
            ExcelExport.WriteXLSFile(
                ds,
                "campanha/metaresultado/faixaatingimentogrupoitem/Erro/",
                out strArquivoGerado);

            strDownloadArquivoErro = "campanha/metaresultado/faixaatingimentogrupoitem/Erro/" + strArquivoGerado;

            return strDownloadArquivoErro;
        }

        public static DataTable ListaImportacaoMetaGrupoItemErro(int ArquivoId)
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Sel_ImpCampanhaMetaGrupoItemErro";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@ArquivoId", Value = ArquivoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            DataTable table = DataProvider.SelectStoreProcedure(proc, listParam);
            return table;
        }

        public static DataTable ListaImportacaoFaixaGrupoItemErro(int ArquivoId)
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Sel_ImpCampanhaFaixaGrupoItemErro";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@ArquivoId", Value = ArquivoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            DataTable table = DataProvider.SelectStoreProcedure(proc, listParam);
            return table;
        }

        public static DataTable ListaImportacaoMetaPessoaErro(int ArquivoId)
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Sel_ImpMetaPessoaErro";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@ArquivoId", Value = ArquivoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            DataTable table = DataProvider.SelectStoreProcedure(proc, listParam);
            return table;
        }


        public static bool ImportarArquivoAssociacaoGrupoItem(DataTable dtAssociacaoGrupoItem, int ArquivoId, int CampanhaId, int CampanhaPeriodoId)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString))
            {
                bulkCopy.DestinationTableName =
                    "dbo.CampanhaAssociacaoGrupoItemImportacao";
                try
                {
                    dtAssociacaoGrupoItem.Columns.Add("ArquivoId");
                    dtAssociacaoGrupoItem.Columns.Add("CampanhaId");
                    dtAssociacaoGrupoItem.Columns.Add("CampanhaPeriodoId");
                    foreach (DataRow dr in dtAssociacaoGrupoItem.Rows)
                    {
                        dr["CampanhaId"] = CampanhaId;
                        dr["ArquivoId"] = ArquivoId;
                        dr["CampanhaPeriodoId"] = CampanhaPeriodoId;
                    }
                    bulkCopy.WriteToServer(dtAssociacaoGrupoItem);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public static bool ProcessaAssociacaoGrupoItemArquivo(int ArquivoId, int periodoId, out int countErro, int usuarioId, int CampanhaId)
        {
            countErro = 0;
            bool blnSucesso = false;

            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Imp_CampanhaAssociacaoGrupoItem";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@ImportadoSucesso", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output });
            listParam.Add(new SqlParameter { ParameterName = "@CountErro", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output });
            listParam.Add(new SqlParameter { ParameterName = "@UsuarioAdmId", Value = usuarioId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@ArquivoId", Value = ArquivoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@PeriodoId", Value = periodoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@CampanhaId", Value = CampanhaId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            DataProvider.NonqueryProc(proc, listParam);

            if ((Boolean)listParam[0].Value)
            {
                blnSucesso = true;
            }
            else
            {
                blnSucesso = false;
            }
            countErro = (int)listParam[1].Value;

            return blnSucesso;
        }

        public static string ExportaArquivoAssociacaoGrupoItemErro(int ArquivoId)
        {
            string strDownloadArquivoErro = "";

            DataSet ds = new DataSet();
            ds.Tables.Add(ListaImportacaoAssociacaoGrupoItemErro(ArquivoId));

            string strArquivoGerado = "";
            ExcelExport.WriteXLSFile(
                ds,
                "campanha/metaresultado/associacaogrupoitem/erro/",
                out strArquivoGerado);

            strDownloadArquivoErro = "campanha/metaresultado/associacaogrupoitem/erro/" + strArquivoGerado;

            return strDownloadArquivoErro;
        }

        public static DataTable ListaImportacaoAssociacaoGrupoItemErro(int ArquivoId)
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Sel_ImpAssociacaoGrupoItemErro";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@ArquivoId", Value = ArquivoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            DataTable table = DataProvider.SelectStoreProcedure(proc, listParam);
            return table;
        }



        public static bool ImportarArquivoGrupoItem(DataTable dtGrupoItem, int ArquivoId, int CampanhaId, int CampanhaPeriodoId)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString))
            {
                bulkCopy.DestinationTableName =
                    "dbo.CampanhaMetaGrupoItemPerfilImportacao";
                try
                {
                    dtGrupoItem.Columns.Add("CampanhaId");
                    dtGrupoItem.Columns.Add("ArquivoId");
                    dtGrupoItem.Columns.Add("CampanhaPeriodoId");
                    foreach (DataRow dr in dtGrupoItem.Rows)
                    {
                        dr["CampanhaId"] = CampanhaId;
                        dr["ArquivoId"] = ArquivoId;
                        dr["CampanhaPeriodoId"] = CampanhaPeriodoId;
                    }
                    bulkCopy.WriteToServer(dtGrupoItem);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public static bool ImportarArquivoFaixaGrupoItem(DataTable dtGrupoItem, int ArquivoId, int CampanhaId, int CampanhaPeriodoId)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString))
            {
                bulkCopy.DestinationTableName =
                    "dbo.CampanhaFaixaAtingimentoGrupoItemImportacao";
                try
                {
                    dtGrupoItem.Columns.Add("CampanhaId");
                    dtGrupoItem.Columns.Add("ArquivoId");
                    dtGrupoItem.Columns.Add("CampanhaPeriodoId");
                    foreach (DataRow dr in dtGrupoItem.Rows)
                    {
                        dr["CampanhaId"] = CampanhaId;
                        dr["ArquivoId"] = ArquivoId;
                        dr["CampanhaPeriodoId"] = CampanhaPeriodoId;
                    }
                    bulkCopy.WriteToServer(dtGrupoItem);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        #region "Calculada Por Item"

        public bool ImportarArquivoCalculadaMetaResultadoPorItem(DataTable dtArquivo, int CampanhaPeriodoId, int CampanhaId, int ArquivoId)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString))
            {
                bulkCopy.DestinationTableName =
                    "dbo.CampanhaCalculadaMetaResultadoGrupoItemImportacao";
                try
                {
                    dtArquivo.Columns.Add("ArquivoId");
                    dtArquivo.Columns.Add("CampanhaId");
                    dtArquivo.Columns.Add("CampanhaPeriodoId");
                    foreach (DataRow dr in dtArquivo.Rows)
                    {
                        dr["ArquivoId"] = ArquivoId;
                        dr["CampanhaId"] = CampanhaId;
                        dr["CampanhaPeriodoId"] = CampanhaPeriodoId;
                    }
                    bulkCopy.WriteToServer(dtArquivo);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public bool ProcessaCampanhaCalculadaMetaResultadoPorItemArquivo(int ArquivoId, int CampanhaPeriodoId, int CampanhaId, out int countErro)
        {
            countErro = 0;
            bool blnSucesso = false;

            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Imp_CampanhaCalculadoMetaResultadoGrupoItem";

            List<SqlParameter> listParam = new List<SqlParameter>();

            //In
            listParam.Add(new SqlParameter { ParameterName = "@ArquivoId", Value = ArquivoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@CampanhaId", Value = CampanhaId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@CampanhaPeriodoId", Value = CampanhaPeriodoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });

            //Out
            listParam.Add(new SqlParameter { ParameterName = "@ImportadoSucesso", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output });
            listParam.Add(new SqlParameter { ParameterName = "@CountErro", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output });

            DataProvider.NonqueryProc(proc, listParam);

            if ((Boolean)listParam[3].Value)
            {
                blnSucesso = true;
            }
            else
            {
                blnSucesso = false;
            }
            countErro = (int)listParam[4].Value;

            return blnSucesso;
        }

        public string ExportaArquivoCalculadaMetaResultadoPorItemErro(int ArquivoId)
        {
            string strDownloadArquivoErro = "";

            DataSet ds = new DataSet();
            ds.Tables.Add(ListaImportacaoCalculadaMetaResultadoPorItemErro(ArquivoId));

            string strArquivoGerado = "";
            ExcelExport.WriteXLSFile(
                ds,
                "campanha/calculado/metaresultado/poritem/erro/",
                out strArquivoGerado);

            strDownloadArquivoErro = "campanha/calculado/metaresultado/poritem/erro/" + strArquivoGerado;

            return strDownloadArquivoErro;
        }

        public DataTable ListaImportacaoCalculadaMetaResultadoPorItemErro(int ArquivoId)
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Sel_ImpCalculadaMetaResultadoPorItemErro";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@ArquivoId", Value = ArquivoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            DataTable table = DataProvider.SelectStoreProcedure(proc, listParam);
            return table;
        }

        #endregion

        #region "Calculada Por Pessoa"

        public bool ImportarArquivoCalculadaMetaResultadoPorPessoa(DataTable dtArquivo, int CampanhaPeriodoId, int CampanhaId, int ArquivoId)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString))
            {
                bulkCopy.DestinationTableName =
                    "dbo.CampanhaCalculadaMetaResultadoPessoaImportacao";
                try
                {
                    dtArquivo.Columns.Add("ArquivoId");
                    dtArquivo.Columns.Add("CampanhaId");
                    dtArquivo.Columns.Add("CampanhaPeriodoId");
                    foreach (DataRow dr in dtArquivo.Rows)
                    {
                        dr["ArquivoId"] = ArquivoId;
                        dr["CampanhaId"] = CampanhaId;
                        dr["CampanhaPeriodoId"] = CampanhaPeriodoId;
                    }
                    bulkCopy.WriteToServer(dtArquivo);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public bool ProcessaCampanhaCalculadaMetaResultadoPorPessoaArquivo(int ArquivoId, int CampanhaPeriodoId, int CampanhaId, out int countErro)
        {
            countErro = 0;
            bool blnSucesso = false;

            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Imp_CampanhaCalculadoMetaResultadoPorPessoa";

            List<SqlParameter> listParam = new List<SqlParameter>();

            //In
            listParam.Add(new SqlParameter { ParameterName = "@ArquivoId", Value = ArquivoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@CampanhaId", Value = CampanhaId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@CampanhaPeriodoId", Value = CampanhaPeriodoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });

            //Out
            listParam.Add(new SqlParameter { ParameterName = "@ImportadoSucesso", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output });
            listParam.Add(new SqlParameter { ParameterName = "@CountErro", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output });

            DataProvider.NonqueryProc(proc, listParam);

            if ((Boolean)listParam[3].Value)
            {
                blnSucesso = true;
            }
            else
            {
                blnSucesso = false;
            }
            countErro = (int)listParam[4].Value;

            return blnSucesso;
        }

        public string ExportaArquivoCalculadaMetaResultadoPorPessoaErro(int ArquivoId)
        {
            string strDownloadArquivoErro = "";

            DataSet ds = new DataSet();
            ds.Tables.Add(ListaImportacaoCalculadaMetaResultadoPorPessoaErro(ArquivoId));

            string strArquivoGerado = "";
            ExcelExport.WriteXLSFile(
                ds,
                "campanha/calculado/metaresultado/porpessoa/erro/",
                out strArquivoGerado);

            strDownloadArquivoErro = "campanha/calculado/metaresultado/porpessoa/erro/" + strArquivoGerado;

            return strDownloadArquivoErro;
        }

        public DataTable ListaImportacaoCalculadaMetaResultadoPorPessoaErro(int ArquivoId)
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Sel_ImpCalculadaMetaResultadoPessoaErro";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@ArquivoId", Value = ArquivoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            DataTable table = DataProvider.SelectStoreProcedure(proc, listParam);
            return table;
        }


        #endregion

        #region "Calculada Por Ranking"

        public bool ImportarArquivoCalculadaMetaResultadoPorRanking(DataTable dtArquivo, int CampanhaPeriodoId, int CampanhaId, int ArquivoId)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString))
            {
                bulkCopy.DestinationTableName =
                    "dbo.CampanhaCalculadaMetaResultadoRankingImportacao";
                try
                {
                    dtArquivo.Columns.Add("ArquivoId");
                    dtArquivo.Columns.Add("CampanhaId");
                    dtArquivo.Columns.Add("CampanhaPeriodoId");
                    foreach (DataRow dr in dtArquivo.Rows)
                    {
                        dr["ArquivoId"] = ArquivoId;
                        dr["CampanhaId"] = CampanhaId;
                        dr["CampanhaPeriodoId"] = CampanhaPeriodoId;
                    }
                    bulkCopy.WriteToServer(dtArquivo);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public bool ProcessaCampanhaCalculadaMetaResultadoPorRankingArquivo(int ArquivoId, int CampanhaPeriodoId, int CampanhaId, out int countErro)
        {
            countErro = 0;
            bool blnSucesso = false;

            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Imp_CampanhaCalculadoMetaResultadoRanking";

            List<SqlParameter> listParam = new List<SqlParameter>();

            //In
            listParam.Add(new SqlParameter { ParameterName = "@ArquivoId", Value = ArquivoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@CampanhaId", Value = CampanhaId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@CampanhaPeriodoId", Value = CampanhaPeriodoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });

            //Out
            listParam.Add(new SqlParameter { ParameterName = "@ImportadoSucesso", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output });
            listParam.Add(new SqlParameter { ParameterName = "@CountErro", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output });

            DataProvider.NonqueryProc(proc, listParam);

            if ((Boolean)listParam[3].Value)
            {
                blnSucesso = true;
            }
            else
            {
                blnSucesso = false;
            }
            countErro = (int)listParam[4].Value;

            return blnSucesso;
        }

        public string ExportaArquivoCalculadaMetaResultadoPorRankingErro(int ArquivoId)
        {
            string strDownloadArquivoErro = "";

            DataSet ds = new DataSet();
            ds.Tables.Add(ListaImportacaoCalculadaMetaResultadoRankingErro(ArquivoId));

            string strArquivoGerado = "";
            ExcelExport.WriteXLSFile(
                ds,
                "campanha/calculado/metaresultado/ranking/erro/",
                out strArquivoGerado);

            strDownloadArquivoErro = "campanha/calculado/metaresultado/ranking/erro/" + strArquivoGerado;

            return strDownloadArquivoErro;
        }

        public DataTable ListaImportacaoCalculadaMetaResultadoRankingErro(int ArquivoId)
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Sel_ImpCalculadaMetaResultadoRankingErro";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@ArquivoId", Value = ArquivoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            DataTable table = DataProvider.SelectStoreProcedure(proc, listParam);
            return table;
        }


        #endregion

        #endregion

        #region "Vendeu Ganhou"

        public bool ImportarArquivoCampanhaGrupoItemPontos(DataTable dtCampanhaGrupoItemPontos, int CampanhaPeriodoId, int CampanhaId, int ArquivoId)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString))
            {
                bulkCopy.DestinationTableName =
                    "dbo.CampanhaGrupoItemPontosImportacao";
                try
                {
                    dtCampanhaGrupoItemPontos.Columns.Add("ArquivoId");
                    dtCampanhaGrupoItemPontos.Columns.Add("CampanhaId");
                    dtCampanhaGrupoItemPontos.Columns.Add("CampanhaPeriodoId");
                    foreach (DataRow dr in dtCampanhaGrupoItemPontos.Rows)
                    {
                        dr["ArquivoId"] = ArquivoId;
                        dr["CampanhaId"] = CampanhaId;
                        dr["CampanhaPeriodoId"] = CampanhaPeriodoId;
                    }
                    bulkCopy.WriteToServer(dtCampanhaGrupoItemPontos);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public bool ProcessaCampanhaGrupoItemPontosArquivo(int ArquivoId, int CampanhaPeriodoId, int CampanhaId, out int countErro)
        {
            countErro = 0;
            bool blnSucesso = false;

            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Imp_CampanhaGrupoItemPontos";

            List<SqlParameter> listParam = new List<SqlParameter>();

            //In
            listParam.Add(new SqlParameter { ParameterName = "@ArquivoId", Value = ArquivoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@CampanhaId", Value = CampanhaId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@CampanhaPeriodoId", Value = CampanhaPeriodoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });

            //Out
            listParam.Add(new SqlParameter { ParameterName = "@ImportadoSucesso", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output });
            listParam.Add(new SqlParameter { ParameterName = "@CountErro", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output });

            DataProvider.NonqueryProc(proc, listParam);

            if ((Boolean)listParam[3].Value)
            {
                blnSucesso = true;
            }
            else
            {
                blnSucesso = false;
            }
            countErro = (int)listParam[4].Value;

            return blnSucesso;
        }

        public string ExportaArquivoGrupoItemPontosErro(int ArquivoId)
        {
            string strDownloadArquivoErro = "";

            DataSet ds = new DataSet();
            ds.Tables.Add(ListaImportacaoGrupoItemPontosErro(ArquivoId));

            string strArquivoGerado = "";
            ExcelExport.WriteXLSFile(
                ds,
                "campanha/vendeuganhou/erro/",
                out strArquivoGerado);

            strDownloadArquivoErro = "campanha/vendeuganhou/erro/" + strArquivoGerado;

            return strDownloadArquivoErro;
        }

        public DataTable ListaImportacaoGrupoItemPontosErro(int ArquivoId)
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Sel_ImpCampanhaGrupoItemPontosErro";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@ArquivoId", Value = ArquivoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            DataTable table = DataProvider.SelectStoreProcedure(proc, listParam);
            return table;
        }

        public List<WebSmart.DTO.CampanhaPeriodoModel> ListarCampanhaPeriodoVendeuGanhou(int CampanhaId)
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Sel_CampanhaPeriodoGrupoItemPontos";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@CampanhaId", Value = CampanhaId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            DataTable table = DataProvider.SelectStoreProcedure(proc, listParam);

            List<DTO.CampanhaPeriodoModel> listCampanhaPeriodoModel = new List<CampanhaPeriodoModel>();

            foreach (DataRow item in table.Rows)
            {
                listCampanhaPeriodoModel.Add(new CampanhaPeriodoModel
                {
                    CampanhaId = item.Field<Int32>("CampanhaId"),
                    DataAlteracao = item.Field<DateTime?>("Dataalteracao"),
                    DataFechamento = item.Field<DateTime?>("DataFechamento"),
                    DataInclusao = item.Field<DateTime>("DataInclusao"),
                    Id = item.Field<Int32>("Id"),
                    Nome = item.Field<String>("Nome"),
                    PeriodoAte = item.Field<DateTime?>("PeriodoAte"),
                    PeriodoDe = item.Field<DateTime?>("PeriodoDe")
                });
            }
            return listCampanhaPeriodoModel;
        }

        public DataTable ListarCampanhaGrupoItemPontosConfiguracao(int CampanhaPeriodoId)
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Sel_ImpCampanhaGrupoItemPontosConfiguracao";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@CampanhaPeriodoId", Value = CampanhaPeriodoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            DataTable table = DataProvider.SelectStoreProcedure(proc, listParam);

            return table;
        }

        public bool InativarCampanhaGrupoItemPontosConfiguracao(int CampanhaPeriodoId)
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Del_ImpCampanhaGrupoItemPontos";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@CampanhaPeriodoId", Value = CampanhaPeriodoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });

            DataProvider.NonqueryProc(proc, listParam);

            return true;
        }

        #region "Calculada Por Item"

        public bool ImportarArquivoCalculadaVendeuGanhouPorItem(DataTable dtArquivo, int CampanhaPeriodoId, int CampanhaId, int ArquivoId)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString))
            {
                bulkCopy.DestinationTableName =
                    "dbo.CampanhaCalculadaVendeuGanhouGrupoItemImportacao";
                try
                {
                    dtArquivo.Columns.Add("ArquivoId");
                    dtArquivo.Columns.Add("CampanhaId");
                    dtArquivo.Columns.Add("CampanhaPeriodoId");
                    foreach (DataRow dr in dtArquivo.Rows)
                    {
                        dr["ArquivoId"] = ArquivoId;
                        dr["CampanhaId"] = CampanhaId;
                        dr["CampanhaPeriodoId"] = CampanhaPeriodoId;
                    }
                    bulkCopy.WriteToServer(dtArquivo);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public bool ProcessaCampanhaCalculadaVendeuGanhouPorItemArquivo(int ArquivoId, int CampanhaPeriodoId, int CampanhaId, out int countErro)
        {
            countErro = 0;
            bool blnSucesso = false;

            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Imp_CampanhaCalculadoVendeuGanhouGrupoItem";

            List<SqlParameter> listParam = new List<SqlParameter>();

            //In
            listParam.Add(new SqlParameter { ParameterName = "@ArquivoId", Value = ArquivoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@CampanhaId", Value = CampanhaId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@CampanhaPeriodoId", Value = CampanhaPeriodoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });

            //Out
            listParam.Add(new SqlParameter { ParameterName = "@ImportadoSucesso", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output });
            listParam.Add(new SqlParameter { ParameterName = "@CountErro", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output });

            DataProvider.NonqueryProc(proc, listParam);

            if ((Boolean)listParam[3].Value)
            {
                blnSucesso = true;
            }
            else
            {
                blnSucesso = false;
            }
            countErro = (int)listParam[4].Value;

            return blnSucesso;
        }

        public string ExportaArquivoCalculadaVendeuGanhouPorItemErro(int ArquivoId)
        {
            string strDownloadArquivoErro = "";

            DataSet ds = new DataSet();
            ds.Tables.Add(ListaImportacaoCalculadaVendeuGanhouPorItemErro(ArquivoId));

            string strArquivoGerado = "";
            ExcelExport.WriteXLSFile(
                ds,
                "campanha/calculado/vendeuganhou/poritem/erro/",
                out strArquivoGerado);

            strDownloadArquivoErro = "campanha/calculado/vendeuganhou/poritem/erro/" + strArquivoGerado;

            return strDownloadArquivoErro;
        }

        public DataTable ListaImportacaoCalculadaVendeuGanhouPorItemErro(int ArquivoId)
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Sel_ImpCalculadaVendeuGanhouPorItemErro";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@ArquivoId", Value = ArquivoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            DataTable table = DataProvider.SelectStoreProcedure(proc, listParam);
            return table;
        }

        #endregion

        #region "Calculada Por Pessoa"

        public bool ImportarArquivoCalculadaVendeuGanhouPorPessoa(DataTable dtArquivo, int CampanhaPeriodoId, int CampanhaId, int ArquivoId)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString))
            {
                bulkCopy.DestinationTableName =
                    "dbo.CampanhaCalculadaVendeuGanhouPessoaImportacao";
                try
                {
                    dtArquivo.Columns.Add("ArquivoId");
                    dtArquivo.Columns.Add("CampanhaId");
                    dtArquivo.Columns.Add("CampanhaPeriodoId");
                    foreach (DataRow dr in dtArquivo.Rows)
                    {
                        dr["ArquivoId"] = ArquivoId;
                        dr["CampanhaId"] = CampanhaId;
                        dr["CampanhaPeriodoId"] = CampanhaPeriodoId;
                    }
                    bulkCopy.WriteToServer(dtArquivo);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public bool ProcessaCampanhaCalculadaVendeuGanhouPorPessoaArquivo(int ArquivoId, int CampanhaPeriodoId, int CampanhaId, out int countErro)
        {
            countErro = 0;
            bool blnSucesso = false;

            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Imp_CampanhaCalculadoVendeuGanhouPorPessoa";

            List<SqlParameter> listParam = new List<SqlParameter>();

            //In
            listParam.Add(new SqlParameter { ParameterName = "@ArquivoId", Value = ArquivoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@CampanhaId", Value = CampanhaId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@CampanhaPeriodoId", Value = CampanhaPeriodoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });

            //Out
            listParam.Add(new SqlParameter { ParameterName = "@ImportadoSucesso", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output });
            listParam.Add(new SqlParameter { ParameterName = "@CountErro", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output });

            DataProvider.NonqueryProc(proc, listParam);

            if ((Boolean)listParam[3].Value)
            {
                blnSucesso = true;
            }
            else
            {
                blnSucesso = false;
            }
            countErro = (int)listParam[4].Value;

            return blnSucesso;
        }

        public string ExportaArquivoCalculadaVendeuGanhouPorPessoaErro(int ArquivoId)
        {
            string strDownloadArquivoErro = "";

            DataSet ds = new DataSet();
            ds.Tables.Add(ListaImportacaoCalculadaVendeuGanhouPorPessoaErro(ArquivoId));

            string strArquivoGerado = "";
            ExcelExport.WriteXLSFile(
                ds,
                "campanha/calculado/vendeuganhou/porpessoa/erro/",
                out strArquivoGerado);

            strDownloadArquivoErro = "campanha/calculado/vendeuganhou/porpessoa/erro/" + strArquivoGerado;

            return strDownloadArquivoErro;
        }

        public DataTable ListaImportacaoCalculadaVendeuGanhouPorPessoaErro(int ArquivoId)
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Sel_ImpCalculadaVendeuGanhouPessoaErro";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@ArquivoId", Value = ArquivoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            DataTable table = DataProvider.SelectStoreProcedure(proc, listParam);
            return table;
        }


        #endregion

        #region "Calculada Por Ranking Pessoa"

        public bool ImportarArquivoCalculadaVendeuGanhouPorRanking(DataTable dtArquivo, int CampanhaPeriodoId, int CampanhaId, int ArquivoId)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString))
            {
                bulkCopy.DestinationTableName =
                    "dbo.CampanhaCalculadaVendeuGanhouRankingImportacao";
                try
                {
                    dtArquivo.Columns.Add("ArquivoId");
                    dtArquivo.Columns.Add("CampanhaId");
                    dtArquivo.Columns.Add("CampanhaPeriodoId");
                    foreach (DataRow dr in dtArquivo.Rows)
                    {
                        dr["ArquivoId"] = ArquivoId;
                        dr["CampanhaId"] = CampanhaId;
                        dr["CampanhaPeriodoId"] = CampanhaPeriodoId;
                    }
                    bulkCopy.WriteToServer(dtArquivo);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public bool ProcessaCampanhaCalculadaVendeuGanhouPorRankingArquivo(int ArquivoId, int CampanhaPeriodoId, int CampanhaId, out int countErro)
        {
            countErro = 0;
            bool blnSucesso = false;

            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Imp_CampanhaCalculadoVendeuGanhouRankingPorPessoa";

            List<SqlParameter> listParam = new List<SqlParameter>();

            //In
            listParam.Add(new SqlParameter { ParameterName = "@ArquivoId", Value = ArquivoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@CampanhaId", Value = CampanhaId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@CampanhaPeriodoId", Value = CampanhaPeriodoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });

            //Out
            listParam.Add(new SqlParameter { ParameterName = "@ImportadoSucesso", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output });
            listParam.Add(new SqlParameter { ParameterName = "@CountErro", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output });

            DataProvider.NonqueryProc(proc, listParam);

            if ((Boolean)listParam[3].Value)
            {
                blnSucesso = true;
            }
            else
            {
                blnSucesso = false;
            }
            countErro = (int)listParam[4].Value;

            return blnSucesso;
        }

        public string ExportaArquivoCalculadaVendeuGanhouPorRankingErro(int ArquivoId)
        {
            string strDownloadArquivoErro = "";

            DataSet ds = new DataSet();
            ds.Tables.Add(ListaImportacaoCalculadaVendeuGanhouPorRankingErro(ArquivoId));

            string strArquivoGerado = "";
            ExcelExport.WriteXLSFile(
                ds,
                "campanha/calculado/vendeuganhou/rankingporpessoa/erro/",
                out strArquivoGerado);

            strDownloadArquivoErro = "campanha/calculado/vendeuganhou/rankingporpessoa/erro/" + strArquivoGerado;

            return strDownloadArquivoErro;
        }

        public DataTable ListaImportacaoCalculadaVendeuGanhouPorRankingErro(int ArquivoId)
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Sel_ImpCalculadaVendeuRankingErro";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@ArquivoId", Value = ArquivoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            DataTable table = DataProvider.SelectStoreProcedure(proc, listParam);
            return table;
        }


        #endregion

        #endregion

        #region "Vendeu Ganhou Ranking"

        public List<WebSmart.DTO.CampanhaFaixaAtingimentoModel> ListarCampanhaFaixaAtingimentoModel(int CampanhaId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repCampanhaFaixaAtingimento = context.CreateRepository<CampanhaFaixaAtingimento>();

                var list = repCampanhaFaixaAtingimento.Filter<CampanhaFaixaAtingimento>(
                    x => x.Ativo == true && x.CampanhaId == CampanhaId).OrderBy(x => x.CampanhaPeriodo.Nome).ToList();

                List<DTO.CampanhaFaixaAtingimentoModel> listCampanhaFaixaAtingimentoModel = new List<CampanhaFaixaAtingimentoModel>();

                foreach (var item in list)
                {
                    listCampanhaFaixaAtingimentoModel.Add(new CampanhaFaixaAtingimentoModel
                    {
                        Id = item.Id,
                        CampanhaId = item.CampanhaId,
                        CampanhaPeriodoId = item.CampanhaPeriodoId,
                        CampanhaEstruturaId = item.CampanhaEstruturaId,
                        CampanhaPerfilId = item.CampanhaPerfilId,
                        ValorDe = item.ValorDe,
                        ValorAte = item.ValorAte,
                        CalculaAtingimentoPercentual = item.CalculaAtingimentoPercentual,
                        Pontos = item.Pontos,
                        Ativo = item.Ativo,
                        DataInclusao = item.DataInclusao,
                        DataAlteracao = item.DataAlteracao,
                        Perfil = item.CampanhaPerfil.Perfil.Nome,
                        Estrutura = item.CampanhaEstrutura.Estrutura.Nome,
                        CampanhaPeriodo = item.CampanhaPeriodo.Nome
                    });
                }

                return listCampanhaFaixaAtingimentoModel;
            }
        }

        public void CadastrarCampanhaFaixaAtingimento(CampanhaFaixaAtingimentoModel campanhaFaixaAtingimento)
        {
            if (campanhaFaixaAtingimento.ValorDe > campanhaFaixaAtingimento.ValorAte) throw new InvalidDataException("Valor De não pode ser superior ao valor Até");


            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repositoryCampanhaFaixaAtingimento = context.CreateRepository<CampanhaFaixaAtingimento>();
                IRepository repositoryCampanha = context.CreateRepository<Campanha>();

                var campanha = repositoryCampanha.Single<Campanha>(x => x.Id == campanhaFaixaAtingimento.CampanhaId);


                //Verifica se a campanha é do tipo Ranking
                if (campanha.TipoCampanhaId == (int)EnumDomain.TipoCampanha.MetaEResultadoRankingParticipante
                    && campanha.TipoCampanhaId == (int)EnumDomain.TipoCampanha.MetaEResultadoRankingPorPessoaCalculado
                    && campanha.TipoCampanhaId == (int)EnumDomain.TipoCampanha.VendeuGanhouRanking
                    && campanha.TipoCampanhaId == (int)EnumDomain.TipoCampanha.VendeuGanhouRankingPorPessoaCalculado)
                {
                    var count = repositoryCampanhaFaixaAtingimento.Filter<CampanhaFaixaAtingimento>(
                        x => x.CampanhaId == campanhaFaixaAtingimento.CampanhaId
                        && x.CampanhaPeriodoId == campanhaFaixaAtingimento.CampanhaPeriodoId
                        && x.CampanhaPerfilId == campanhaFaixaAtingimento.CampanhaPerfilId
                        && x.CampanhaEstruturaId == campanhaFaixaAtingimento.CampanhaEstruturaId
                        && x.Ativo == true
                        ).Count();

                    if (count > 0) throw new InvalidDataException(string.Format("Faixa já cadastrada para as informações informadas."));

                }


                //Valida se o range já está sendo utilizado
                var countRangeUtilizado = repositoryCampanhaFaixaAtingimento.Filter<CampanhaFaixaAtingimento>(
                    x => x.CampanhaId == campanhaFaixaAtingimento.CampanhaId
                        && x.CampanhaPeriodoId == campanhaFaixaAtingimento.CampanhaPeriodoId
                        && x.CampanhaPerfilId == campanhaFaixaAtingimento.CampanhaPerfilId
                        && x.CampanhaEstruturaId == campanhaFaixaAtingimento.CampanhaEstruturaId
                        && x.Ativo == true
                        && ((campanhaFaixaAtingimento.ValorDe >= x.ValorDe &&
                            campanhaFaixaAtingimento.ValorDe <= x.ValorAte)
                        || (campanhaFaixaAtingimento.ValorAte >= x.ValorDe &&
                            campanhaFaixaAtingimento.ValorAte <= x.ValorAte))
                ).Count();

                if (countRangeUtilizado > 0) throw new InvalidDataException(string.Format("O Valor De/Ate da Faixa já está sendo utilizado."));

                CampanhaFaixaAtingimento _campanhaFaixaAtingimento = new CampanhaFaixaAtingimento
                {
                    CampanhaPeriodoId = campanhaFaixaAtingimento.CampanhaPeriodoId,
                    CampanhaEstruturaId = campanhaFaixaAtingimento.CampanhaEstruturaId,
                    CampanhaPerfilId = campanhaFaixaAtingimento.CampanhaPerfilId,
                    ValorDe = campanhaFaixaAtingimento.ValorDe,
                    ValorAte = campanhaFaixaAtingimento.ValorAte,
                    CalculaAtingimentoPercentual = campanhaFaixaAtingimento.CalculaAtingimentoPercentual,
                    Pontos = campanhaFaixaAtingimento.Pontos,
                    Ativo = true,
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now,
                    CampanhaId = campanhaFaixaAtingimento.CampanhaId
                };

                using (TransactionScope scope = new TransactionScope())
                {
                    repositoryCampanhaFaixaAtingimento.Create(_campanhaFaixaAtingimento);
                    repositoryCampanhaFaixaAtingimento.SaveChanges();
                    scope.Complete();
                }
            }
        }

        public void InativarCampanhaFaixaAtingimento(int Id)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repositoryCampanhaFaixaAtingimento = context.CreateRepository<CampanhaFaixaAtingimento>();

                var _CampanhaFaixaAtingimento = repositoryCampanhaFaixaAtingimento.Find<CampanhaFaixaAtingimento>(Id);

                //Verifica se existe registro
                if (_CampanhaFaixaAtingimento == null) throw new KeyNotFoundException(string.Format("Campanha Faixa Atingimento com o Id {0} não encontrada.", _CampanhaFaixaAtingimento.Id));

                //Verifica se o Id é <> 0
                if (_CampanhaFaixaAtingimento.Id == 0) throw new KeyNotFoundException(string.Format("Campanha Faixa Atingimento o Id {0} não encontrada.", _CampanhaFaixaAtingimento.Id));

                //Verifica se o periodo já foi apurado
                if ((bool)_CampanhaFaixaAtingimento.CampanhaPeriodo.Apurado) throw new KeyNotFoundException("Não foi possível realizar a operação, período já apurado.");

                //Atualiza o objeto
                if (_CampanhaFaixaAtingimento != null)
                {
                    _CampanhaFaixaAtingimento.Ativo = false;
                }

                //Salva
                repositoryCampanhaFaixaAtingimento.Update<CampanhaFaixaAtingimento>(_CampanhaFaixaAtingimento);
                repositoryCampanhaFaixaAtingimento.SaveChanges();
            }
        }

        #endregion

        #region Resultados
        public CampanhaResultadoModel ListaResultadosCampanhaPerfil(int campanhaId, int campanhaPeriodoId, int participanteId, int prfilPAiId, int perfilFilhoId, bool rankingIndividual, int inicio, int qtde, out int totalResult)
        {
            CampanhaResultadoModel resultado = new CampanhaResultadoModel();
            PerfilService _perfilService = new PerfilService();
            CampanhaService _campanhaService = new CampanhaService();
            int total;

            var resultadodetalhe = ListarResultadoCampanhaDetalhe(participanteId, campanhaPeriodoId, campanhaId, prfilPAiId, perfilFilhoId, rankingIndividual, inicio, qtde, out total);
            totalResult = total;
            if (resultadodetalhe.Count > 0)
            {
                // Consulta o tipo da campanha
                var perfil = _perfilService.ListarPorId(perfilFilhoId);

                resultado.CampanhaId = campanhaId;
                resultado.TipoCampanha = (int)_campanhaService.ListarPorId(campanhaId).TipoCampanhaId;
                resultado.Perfil = perfil.Nome;
                resultado.PerfilId = perfil.Id;
                resultado.TotalRegistros = total;
                resultado.ResultadoDetalhe = resultadodetalhe;
            }

            return resultado;
        }
        public List<CampanhaResultadoDetalheModel> ListarResultadoCampanhaDetalhe(int participanteId, int campanhaPeriodoId, int campanhaId, int perfilPaiId, int perfilFilhoId, bool rankingIndividual, int? inicio, int? quantidade, out int total)
        {
            List<CampanhaResultadoDetalheModel> result = new List<CampanhaResultadoDetalheModel>();
            List<CampanhaResultadoDetalheModel> resultado = new List<CampanhaResultadoDetalheModel>();
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "[JP_Sel_CampanhaPeriodoResultado]";
            total = 0;

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@ParticipanteId", Value = participanteId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@CampanhaPeriodoId", Value = campanhaPeriodoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@CampanhaId", Value = campanhaId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@PerfilPaiId", Value = perfilPaiId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@PerfilFilhoId ", Value = perfilFilhoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@Inicio", Value = inicio, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@RankingIndividual", Value = rankingIndividual, SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@Quantidade", Value = quantidade, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@Total", Value = total, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output });
            DataTable table = DataProvider.SelectStoreProcedure(proc, listParam);

            //de para
            foreach (DataRow row in table.Rows)
            {
                resultado.Add(new CampanhaResultadoDetalheModel
                {
                    Estrutura = (string)row["ESTRUTURA"],
                    NomePai = (string)row["NOMEPAI"],
                    Nome = (string)row["NOME"],
                    GupoItemPai = (string)row["GRUPOITEMPAI"],
                    GrupoItem = (string)row["GRUPOITEM"],
                    Meta = (double)row["META"],
                    Efetivo = (double)row["EFETIVO"],
                    Atingimento = (double)row["ATINGIMENTO"],
                    PosicaoRanking = (int)row["POSICAORANKING"],
                    Pontos = (double)row["PONTOS"]
                });
            }
            total = Convert.ToInt32(listParam[8].Value);
            if (resultado.Count > 0)
            {
                //Ordena os Registros de acordo com o tipo da campanha
                var campannha = BuscarCampanha(campanhaId);
                if (campannha.TipoCampanhaId == (int)EnumDomain.TipoCampanha.MetaEResultadoRankingParticipante ||
                   campannha.TipoCampanhaId == (int)EnumDomain.TipoCampanha.MetaEResultadoRankingPorPessoaCalculado ||
                   campannha.TipoCampanhaId == (int)EnumDomain.TipoCampanha.VendeuGanhouRanking ||
                   campannha.TipoCampanhaId == (int)EnumDomain.TipoCampanha.VendeuGanhouRankingPorPessoaCalculado
                    )
                {
                    result = resultado.OrderBy(x => x.PosicaoRanking).ToList();
                }
                else
                {
                    result = resultado.OrderBy(x => x.Nome).ToList();
                }
            }

            return result;
        }

        public List<WebSmart.DTO.PerfilModel> ListarPerfilCampanhaHierarquia(int campanhaPeriodoId, int participanteId)
        {
            //Variáveis
            List<PerfilModel> PerfisResultado = new List<PerfilModel>();

            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                //declaração dos repositórios
                IRepository repPerfil = context.CreateRepository<Perfil>();
                IRepository repCampanha = context.CreateRepository<Campanha>();

                //Verifica se a campanha do período em ques'~ao deve exibir o resultado Cascara
                var resultCascata = repCampanha.Single<Campanha>(x => x.CampanhaPeriodo.Any(p => p.Id == campanhaPeriodoId && p.Ativo == true)).ResultadoCascata;

                //Carrega o Perfil pai da hierarquia
                var perfilPai = repPerfil.Single<Perfil>(x => x.ParticipantePerfil.Any(p => p.ParticipanteId == participanteId && p.Ativo == true)
                                                         && x.Ativo == true
                                                         && x.CampanhaPeriodoParticipantePerfil.Any(cppp => cppp.PerfilId == x.Id));


                if (perfilPai != null)
                {
                    //Carrega os Perfis Válidos para a campanha.
                    var perfisHierarquia = repPerfil.Filter<Perfil>(x => x.Ativo == true
                                                                       && x.Adm == false
                                                                       && x.CampanhaPerfil.Any(cp => cp.PerfilId == x.Id
                                                                                               && cp.Ativo == true
                                                                                               && cp.Participa == true)
                                                                       && x.CampanhaPeriodoParticipantePerfil.Any(cppp => cppp.Ativo == true
                                                                                                                  && cppp.CampanhaPeriodoId == campanhaPeriodoId)
                                                                       && x.NivelHierarquia >= perfilPai.NivelHierarquia).OrderBy(x => x.NivelHierarquia).ToList();

                    if (perfisHierarquia.Count() > 0)
                    {
                        foreach (Perfil p in perfisHierarquia)
                        {
                            PerfisResultado.Add(new PerfilModel
                            {
                                Id = p.Id,
                                PaiId = perfilPai.Id,
                                Nome = p.Nome,
                                DataInclusao = p.DataInclusao,
                                DataAlteracao = (DateTime)p.DataAlteracao,
                                Adm = p.Adm,
                                Ativo = p.Ativo,
                                NivelHierarquia = p.NivelHierarquia
                            });
                        }

                        if (resultCascata == false)
                        {
                            PerfisResultado = PerfisResultado.Where(x => x.Id == perfilPai.Id).ToList();
                        }
                    }
                }
            }
            return PerfisResultado;
        }

        public string BuscarLayoutResultadoCampanha(int tipoCampanha)
        {
            string LayoutCampanha = "";

            //VENDEU GANHOU POR ITEM
            if (tipoCampanha == (int)EnumDomain.TipoCampanha.VendeuGanhou || tipoCampanha == (int)EnumDomain.TipoCampanha.VendeuGanhouPorItemPorItemCalculado)
            {
                LayoutCampanha = StringHelper.GetEnumDescription<EnumDomain.LayoutResultadoCampanha>(EnumDomain.LayoutResultadoCampanha.VendeuGanhouItem);
            }

            //VENDEU GANHOU POR PESSOA
            if (tipoCampanha == (int)EnumDomain.TipoCampanha.VendeuGanhouPorPessoaCalculado)
            {
                LayoutCampanha = StringHelper.GetEnumDescription<EnumDomain.LayoutResultadoCampanha>(EnumDomain.LayoutResultadoCampanha.VendeuGanhouPessoa);
            }

            //VENDEU GANHOU POR RANKING
            if (tipoCampanha == (int)EnumDomain.TipoCampanha.VendeuGanhouRanking || tipoCampanha == (int)EnumDomain.TipoCampanha.VendeuGanhouRankingPorPessoaCalculado)
            {
                LayoutCampanha = StringHelper.GetEnumDescription<EnumDomain.LayoutResultadoCampanha>(EnumDomain.LayoutResultadoCampanha.VendeuGanhouRanking);
            }

            //META RESULTADO POR ITEM
            if (tipoCampanha == (int)EnumDomain.TipoCampanha.MetaEResultadoItens || tipoCampanha == (int)EnumDomain.TipoCampanha.MetaResultadoPorItemCalculado)
            {
                LayoutCampanha = StringHelper.GetEnumDescription<EnumDomain.LayoutResultadoCampanha>(EnumDomain.LayoutResultadoCampanha.MetaResultadoItem);
            }

            //META RESULTADO POR PESSOA
            if (tipoCampanha == (int)EnumDomain.TipoCampanha.MetaEResultadoParticipante || tipoCampanha == (int)EnumDomain.TipoCampanha.MetaEResultadoPorPessoaCalculado)
            {
                LayoutCampanha = StringHelper.GetEnumDescription<EnumDomain.LayoutResultadoCampanha>(EnumDomain.LayoutResultadoCampanha.MetaResultadoPessoa);
            }

            //META RESULTADO POR RANKING
            if (tipoCampanha == (int)EnumDomain.TipoCampanha.MetaEResultadoRankingParticipante || tipoCampanha == (int)EnumDomain.TipoCampanha.MetaEResultadoRankingPorPessoaCalculado)
            {
                LayoutCampanha = StringHelper.GetEnumDescription<EnumDomain.LayoutResultadoCampanha>(EnumDomain.LayoutResultadoCampanha.MetaResultadoRanking);
            }

            return LayoutCampanha;
        }

        public bool BuscaCampanhaPasso(int campanhaId, Domain.Enums.EnumDomain.PassosCampanha EPasso)
        {
            int passoId = ((int)EPasso);

            bool bln = false;

            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Sel_CampanhaPasso";

            List<SqlParameter> listParam = new List<SqlParameter>();

            //In
            listParam.Add(new SqlParameter { ParameterName = "@CampanhaId", Value = campanhaId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@PassoId", Value = passoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });

            //Out
            listParam.Add(new SqlParameter { ParameterName = "@Retorno", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output });

            DataProvider.NonqueryProc(proc, listParam);

            if ((Boolean)listParam[2].Value)
            {
                bln = true;
            }
            else
            {
                bln = false;
            }

            return bln;

        }

        public List<CampanhaModel> ObterCampanhasPorCatalogoTipoArquivo(int? catalogoId, int? idTipoArquivo, int? anoCampanha)
        {
            List<CampanhaModel> list = new List<CampanhaModel>();

            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "[JP_PRC_ListarCampanhasPorCatalogoTipoArquivo]";
            //total = 0;

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@catalogoId", Value = catalogoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@tipoArquivoId", Value = idTipoArquivo, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@anoCampanha", Value = anoCampanha, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            
            DataTable table = DataProvider.SelectStoreProcedure(proc, listParam);

            //de para
            foreach (DataRow row in table.Rows)
            {
                list.Add(new GrupoLTM.WebSmart.DTO.CampanhaModel
                {
                    Id = (int)row["Id"],
                    Nome = (string)row["Nome"]

                });
            }

            return list;

        }


        public bool ResetCampanha(int campanhaId)
        {
            bool bln = false;

            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_UPD_ResetCampanha";

            List<SqlParameter> listParam = new List<SqlParameter>();

            //In
            listParam.Add(new SqlParameter { ParameterName = "@CampanhaId", Value = campanhaId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });

            //Out
            listParam.Add(new SqlParameter { ParameterName = "@Processado", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output });

            DataProvider.NonqueryProc(proc, listParam);

            if ((Boolean)listParam[1].Value)
            {
                bln = true;
            }
            else
            {
                bln = false;
            }

            return bln;

        }

        #endregion

        #region "Internal Functions"

        public string BuscarCaminhoCampanhaLogArquivo(Domain.Enums.EnumDomain.TipoCampanha ETipoCampanha, int? TipoArquivoId)
        {
            string caminho = "";

            switch (ETipoCampanha)
            {
                //CALCULADOS
                case EnumDomain.TipoCampanha.MetaEResultadoPorPessoaCalculado:
                    caminho = Settings.Caminho.StoragePath + "campanha/calculado/metaresultado/porpessoa/";
                    break;
                case EnumDomain.TipoCampanha.MetaEResultadoRankingPorPessoaCalculado:
                    caminho = Settings.Caminho.StoragePath + "campanha/calculado/metaresultado/ranking/";
                    break;
                case EnumDomain.TipoCampanha.VendeuGanhouPorPessoaCalculado:
                    caminho = Settings.Caminho.StoragePath + "campanha/calculado/vendeuganhou/porpessoa/";
                    break;
                case EnumDomain.TipoCampanha.VendeuGanhouRankingPorPessoaCalculado:
                    caminho = Settings.Caminho.StoragePath + "campanha/calculado/vendeuganhou/rankingporpessoa/";
                    break;
                case EnumDomain.TipoCampanha.MetaResultadoPorItemCalculado:
                    caminho = Settings.Caminho.StoragePath + "campanha/calculado/metaresultado/poritem/";
                    break;
                case EnumDomain.TipoCampanha.VendeuGanhouPorItemPorItemCalculado:
                    caminho = Settings.Caminho.StoragePath + "campanha/calculado/vendeuganhou/poritem/";
                    break;

                //NÃO CALCULADOS

               



                default:
                    break;
            }

            return caminho;
        }


        #endregion
    }

}
