using System.Collections.Generic;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.Infrastructure.Cache;
using System.Linq;
using GrupoLTM.WebSmart.DTO;
using System;
using AutoMapper;

namespace GrupoLTM.WebSmart.Services
{
    public class CatalogoCPService : BaseService<CatalogoCP>
    {
        [Cache]
        public CatalogoCP ObterCatalogoCP(string CP, long mktPlaceCatalogoId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repCatalogo = context.CreateRepository<CatalogoCP>();
                var _catalogo = repCatalogo.Filter<CatalogoCP>(x => x.CP == CP && x.Catalogo.Codigo == mktPlaceCatalogoId && x.Ativo).FirstOrDefault();

                if (_catalogo == null)
                    return null;

                var catalogoCP = new CatalogoCP()
                {
                    Id = _catalogo.Id,
                    Ativo = _catalogo.Ativo,
                    CatalogoId = _catalogo.CatalogoId,
                    Catalogo = _catalogo.Catalogo,
                    CP = _catalogo.CP,
                    ProfileId = _catalogo.ProfileId,
                    DataInicio = _catalogo.DataInicio,
                    DataFim = _catalogo.DataFim
                };

                return catalogoCP;
            }
        }

        public List<CatalogoCPModel> ObterCPsPorCatalogoId(int catalogoId)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repCatalogo = context.CreateRepository<CatalogoCP>();
                    var _catalogo = repCatalogo.Filter<CatalogoCP>(x => x.CatalogoId == catalogoId).ToList();

                    return CreateModel(_catalogo);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CatalogoCPModel ObterCPsPorCatalogoCPId(int catalogoCPId)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repCatalogo = context.CreateRepository<CatalogoCP>();
                    var _catalogo = repCatalogo.Filter<CatalogoCP>(x => x.Id == catalogoCPId && x.Ativo == true).FirstOrDefault();

                    return CreateModel(_catalogo);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Salvar(List<CatalogoCPModel> cpsModel)
        {
            try
            {
                var cps = CreateEntity(cpsModel);

  

                if (cps != null)
                {
                    using (IUnitOfWork context = UnitOfWorkFactory.Create())
                    {
                        IRepository repCatalogo = context.CreateRepository<CatalogoCP>();

                        foreach (var cp in cps)
                        {
                            if (cp.Id > 0)
                                repCatalogo.Update(cp);
                            else
                                repCatalogo.Create(cp);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void Remover(int[] ids)
        {
            try
            {
                if (ids == null || ids.Length == 0)
                    return;

                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repCatalogo = context.CreateRepository<CatalogoCP>();
                    repCatalogo.Delete<CatalogoCP>(x => ids.Contains(x.Id));
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public int ObterParticipanteCatalogo(long mktPlaceParticipanteId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repCatalogo = context.CreateRepository<ParticipanteCatalogo>();
                var _participanteCatalogo = repCatalogo.Filter<ParticipanteCatalogo>(x => x.CodigoMktPlace == mktPlaceParticipanteId && x.Ativo == true).FirstOrDefault();

                if (_participanteCatalogo == null) return 0;

                return _participanteCatalogo.Catalogo.Id;
                //return _participanteCatalogo.Catalogo.IdCampanha;

            }
        }

        public List<CatalogoCP> ObterCatalogosCPsAtivo()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repCatalogo = context.CreateRepository<CatalogoCP>();
                var _catalogo = repCatalogo.Filter<CatalogoCP>(x => x.Ativo).ToList();

                if (_catalogo == null)
                    return null;

                var catalogosCPs = new List<CatalogoCP>();

                foreach (var catCP in _catalogo)
                {
                    var catalogoCP = new CatalogoCP()
                    {
                        Id = catCP.Id,
                        Ativo = catCP.Ativo,
                        CatalogoId = catCP.CatalogoId,
                        Catalogo = catCP.Catalogo,
                        CP = catCP.CP,
                        ProfileId = catCP.ProfileId,
                        DataInicio = catCP.DataInicio,
                        DataFim = catCP.DataFim,
                    };

                    catalogosCPs.Add(catalogoCP);
                }

                return catalogosCPs;
            }
        }

        #region "Métodos Privados"

        private static CatalogoCPModel CreateModel(CatalogoCP catalogoCP)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CatalogoCP, CatalogoCPModel>();
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<CatalogoCP, CatalogoCPModel>(catalogoCP);
        }

        private static List<CatalogoCPModel> CreateModel(List<CatalogoCP> catalogosCPs)
        {
            List<CatalogoCPModel> lstRetorno = new List<CatalogoCPModel>();

            foreach(var catalogoCP in catalogosCPs)
                lstRetorno.Add(CreateModel(catalogoCP));

            return lstRetorno;
        }

        private static CatalogoCP CreateEntity(CatalogoCPModel catalogoCPModel)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CatalogoCPModel, CatalogoCP>();
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<CatalogoCPModel, CatalogoCP>(catalogoCPModel);
        }

        private static List<CatalogoCP> CreateEntity(List<CatalogoCPModel> catalogosCPsModel)
        {
            List<CatalogoCP> lstRetorno = new List<CatalogoCP>();

            if (catalogosCPsModel is null)
            {
                return null;
            }  

            foreach (var catalogoCPModel in catalogosCPsModel)
                lstRetorno.Add(CreateEntity(catalogoCPModel));

            return lstRetorno;
        }

        #endregion

    }
}
