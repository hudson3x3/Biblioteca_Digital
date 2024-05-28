using System;
using System.Collections.Generic;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.Infrastructure.Cache;
using System.Linq;
using GrupoLTM.WebSmart.DTO;

namespace GrupoLTM.WebSmart.Services
{
    public class CatalogoService : BaseService<Catalogo>
    {
        public Catalogo ObterCatalogo(long mktPlaceCatalogoId)
        {
            Catalogo _catalogo = WebCache.GetCache<Catalogo>("Catalogo", mktPlaceCatalogoId);

            if (_catalogo != null)
                return _catalogo;

            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repCatalogo = context.CreateRepository<Catalogo>();
                _catalogo = repCatalogo.Filter<Catalogo>(x => x.Codigo == mktPlaceCatalogoId).FirstOrDefault();

                if (_catalogo == null)
                    return null;

                WebCache.SetCache("Catalogo", _catalogo, mktPlaceCatalogoId);

                return _catalogo;
            }
        }
        public Catalogo ObterCatalogoContext(long mktPlaceCatalogoId)
        {
            Catalogo _catalogo = new Catalogo();

            try
            {
         
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repCatalogo = context.CreateRepository<Catalogo>();
                    _catalogo = repCatalogo.Filter<Catalogo>(x => x.Codigo == mktPlaceCatalogoId).FirstOrDefault();

                    if (_catalogo == null)
                    {
                        gravaLogErro("Erro ao buscar o Catálogo no Contexto", "Catálogo NULO.", "GrupoLTM.WebSmart.Services", string.Format("ObterCatalogoContext(mktPlaceCatalogoId {0})", mktPlaceCatalogoId.ToString()), "logCatalog");
                        return null;
                    }
                    return _catalogo;
                }
            }
            catch (Exception ex)
            {
                gravaLogErro("Erro ao buscar o Catálogo no Contexto", ex.Message, "GrupoLTM.WebSmart.Services", string.Format("ObterCatalogoContext(mktPlaceCatalogoId {0})", mktPlaceCatalogoId.ToString()), "logCatalog");
                return null;
            }
        }
        private static void gravaLogErro(string Erro, string Mensagem, string Source, string Metodo, string Codigo)
        {
            var logErro = new LogErro()
            {
                Erro = Erro,
                Mensagem = Mensagem,
                Source = Source,
                Metodo = Metodo,
                Controller = "CatalogoService",
                Pagina = string.Empty,
                Codigo = Codigo
            };

            var logErroService = new LogErroService();
            logErroService.SalvarLogErro(logErro);
        }


        public Catalogo ObterCatalogoPorId(int id)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repCatalogo = context.CreateRepository<Catalogo>();
                var _catalogo = repCatalogo.Find<Catalogo>(id);

                if (_catalogo == null)
                    return null;

                return _catalogo;
            }
        }
        public Catalogo ObterCatalogoPorPerfilParticipante(short profileType)
        {
            using (var context = UnitOfWorkFactory.Create())
            {
                var repCatalogo = context.CreateRepository<Catalogo>();
                var catalogo = repCatalogo.Filter<Catalogo>(_=> _.Ativo).FirstOrDefault();

                return catalogo;
            }
        }

        public List<CatalogoModel> ListarCatalogos(int PerfilId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                var catalogosModel = new List<CatalogoModel>();
                IRepository repCatalogo = context.CreateRepository<Catalogo>();
                var catalogos = repCatalogo.Filter<Catalogo>(x => x.Ativo).ToList();

                catalogos.ForEach(delegate(Catalogo item)
                {
                    catalogosModel.Add(new CatalogoModel
                    {
                        Id = item.Id,
                        Nome = item.Nome,
                        Autor  = item.Autor,
                        Codigo = item.Codigo,
                        Qtd = item.Qtd,
                        //PrimeiroAcesso =  item.PrimeiroAcesso,
                        DataInclusao = DateTime.Now,
                        DataAlteracao = DateTime.Now,
                        PerfilId = PerfilId,
                        //ConversionRate = item.ConversionRate,
                        //RepProfileType = item.RepProfileType,
                        //Ativo = item.Ativo,
                        //MktPlaceSupplierId = item.MktPlaceSupplierId
                    });
                });

                return catalogosModel;
            }            
        }

        public void CriarCatalogo(CatalogoModel catalogoModel)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repCatalogo = context.CreateRepository<Catalogo>();
                var catalogo = new Catalogo();

                catalogo.Nome = catalogoModel.Nome;
                catalogo.Autor = catalogoModel.Autor;
                //catalogo.MktPlaceCatalogoId = catalogoModel.MktPlaceCatalogoId;
                catalogo.Codigo = catalogoModel.Codigo;
                catalogo.Qtd = catalogoModel.Qtd;
                //catalogo.PrimeiroAcesso = true;
                catalogo.DataInclusao = DateTime.Now;
                catalogo.DataAlteracao = DateTime.Now;
                catalogo.Ativo = true;
                //catalogo.IdCampanha = catalogoModel.IdCampanha;
                //catalogo.IdEmpresa = catalogoModel.IdEmpresa;
                //catalogo.IdOrigem = catalogoModel.IdOrigem;
                //catalogo.AppIdMktPlace = catalogoModel.AppIdMktPlace;
                //catalogo.RepProfileType = catalogoModel.RepProfileType;

                repCatalogo.Create(catalogo);
            }
        }

        public void EditarCatalogo(CatalogoModel catalogoModel)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repCatalogo = context.CreateRepository<Catalogo>();

                var catalogo = repCatalogo.Find<Catalogo>(catalogoModel.Id);
                catalogo.Nome = catalogoModel.Nome;
                catalogo.Autor = catalogoModel.Autor;
                //catalogo.MktPlaceCatalogoId = catalogoModel.MktPlaceCatalogoId;
                catalogo.Codigo = catalogoModel.Codigo;
                catalogo.Qtd = catalogoModel.Qtd;
                //catalogo.PrimeiroAcesso = catalogoModel.PrimeiroAcesso;
                catalogo.DataInclusao = DateTime.Now;
                catalogo.DataAlteracao = DateTime.Now;
                //catalogo.IdCampanha = catalogoModel.IdCampanha;
                //catalogo.IdEmpresa = catalogoModel.IdEmpresa;
                //catalogo.IdOrigem = catalogoModel.IdOrigem;
                //catalogo.AppIdMktPlace = catalogoModel.AppIdMktPlace;
                //catalogo.RepProfileType = catalogoModel.RepProfileType;

                repCatalogo.Update(catalogo);
            }
        }

        public bool InativarCatalogo(int catalogoId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repCatalogo = context.CreateRepository<Catalogo>();

                var catalogo = repCatalogo.Find<Catalogo>(catalogoId) ;
                catalogo.Ativo = false;

                repCatalogo.Update(catalogo);
                return true;    
            }
        }
        public CatalogoModel BuildCatalogo(Catalogo catalogo)
        {
            if (catalogo == null)
                return new CatalogoModel();
            
            //var dadosCampanha = _campanha.ListarCampanhaConfiguracao();
            var catalogoModel = new CatalogoModel();
            if (catalogo != null)
            {         
                //De Para Banco Model
                catalogoModel.Id = catalogo.Id;
                catalogoModel.Nome = catalogo.Nome;
                catalogoModel.Autor = catalogo.Autor;
                catalogoModel.Qtd = catalogo.Qtd;
                //catalogoModel.MktPlaceCatalogoId = catalogo.MktPlaceCatalogoId;
                catalogoModel.Codigo = catalogo.Codigo;
                catalogoModel.Ativo = catalogo.Ativo;
                catalogoModel.DataInclusao = DateTime.Now;
                catalogoModel.DataAlteracao = DateTime.Now;
                //catalogoModel.IdCampanha = catalogo.IdCampanha;
                //catalogoModel.IdOrigem = catalogo.IdOrigem;
                //catalogoModel.IdEmpresa = catalogo.IdEmpresa;               

                return catalogoModel;
            }
            else
            {
                return null;
            }
        }

    }
}
