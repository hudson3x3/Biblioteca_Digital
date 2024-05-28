using System.Collections.Generic;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.Infrastructure.Cache;
using System.Linq;
using GrupoLTM.WebSmart.Infrastructure.ExtensionMethods;
using GrupoLTM.WebSmart.DTO;
using System;

namespace GrupoLTM.WebSmart.Services
{
    public class ProgramaIncentivoService : BaseService<ProgramaIncentivo>
    {
        public ProgramaIncentivo ObterProgramaIncentivo(string Nome)
        {
            ProgramaIncentivo _programaIncentivo = null;

            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repProgramaIncentivo = context.CreateRepository<ProgramaIncentivo>();
                _programaIncentivo = repProgramaIncentivo.Filter<ProgramaIncentivo>(x => x.Nome == Nome).FirstOrDefault();

                if (_programaIncentivo == null)
                    return null;

                return _programaIncentivo;
            }
        }

        public ProgramaIncentivo ObterProgramaIncentivoPorId(int id)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repProgramaIncentivo = context.CreateRepository<ProgramaIncentivo>();
                var _programaIncentivo = repProgramaIncentivo.Find<ProgramaIncentivo>(id);

                if (_programaIncentivo == null)
                    return null;

                return _programaIncentivo;
            }
        }

        public bool ObterProgramaIncentivoCategoriaArquivoPorId(int ProgramaIncentivoId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repProgramaIncentivoCatalogoArquivo = context.CreateRepository<ProgramaIncentivoCatalogoArquivo>();
                var programaIncentivoCatalogoArquivo = repProgramaIncentivoCatalogoArquivo.Find<ProgramaIncentivoCatalogoArquivo>(x => x.ProgramaIncentivoId == ProgramaIncentivoId);
                bool bExcluir = programaIncentivoCatalogoArquivo == null ? true : false;
                //se o programa de incentivo, estiver relacionado à um Arquivo/Punsh não poderá excluir ou editar o nome do mesmo.

                return bExcluir;
            }
        }

        public List<ProgramaIncentivo> GetAll()
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repArquivo = context.CreateRepository<ProgramaIncentivo>();

                    var lstProgramaIncentivo = repArquivo.All<ProgramaIncentivo>().ToList();

                    return lstProgramaIncentivo;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ProgramaIncentivoModel> ListarProgramaIncentivos()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                var programaIncentivosModel = new List<ProgramaIncentivoModel>();
                IRepository repProgramaIncentivo = context.CreateRepository<ProgramaIncentivo>();
                var programaIncentivos = repProgramaIncentivo.Filter<ProgramaIncentivo>(x => 1 == 1).ToList();

                IRepository repProgramaIncentivoCatalogoArquivo = context.CreateRepository<ProgramaIncentivoCatalogoArquivo>();

                programaIncentivos.ForEach(delegate (ProgramaIncentivo item)
                {
                    var programaIncentivoCatalogoArquivo = repProgramaIncentivoCatalogoArquivo.Find<ProgramaIncentivoCatalogoArquivo>(x => x.ProgramaIncentivoId == item.Id);

                    bool bExcluir = programaIncentivoCatalogoArquivo == null ? true : false;

                    programaIncentivosModel.Add(new ProgramaIncentivoModel
                    {
                        Id = item.Id,
                        Nome = item.Nome,
                        IdOrigem = item.IdOrigem,
                        Ano = item.Ano,
                        EditarExcluir = bExcluir
                    });
                });

                return programaIncentivosModel;
            }
        }

        public void CriarProgramaIncentivo(ProgramaIncentivoModel programaIncentivoModel)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repProgramaIncentivo = context.CreateRepository<ProgramaIncentivo>();
                var programaIncentivo = new ProgramaIncentivo();

                programaIncentivo.Nome = programaIncentivoModel.Nome;
                programaIncentivo.IdOrigem = programaIncentivoModel.IdOrigem;
                programaIncentivo.Ano = programaIncentivoModel.Ano;

                repProgramaIncentivo.Create(programaIncentivo);
            }
        }

        public void EditarProgramaIncentivo(ProgramaIncentivoModel programaIncentivoModel)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repProgramaIncentivo = context.CreateRepository<ProgramaIncentivo>();
                var bEditarExcluir = ObterProgramaIncentivoCategoriaArquivoPorId(programaIncentivoModel.Id);

                var programaIncentivo = repProgramaIncentivo.Find<ProgramaIncentivo>(programaIncentivoModel.Id);
                if (bEditarExcluir)
                    programaIncentivo.Nome = programaIncentivoModel.Nome; //não pode editar o nome do programa de incentivo se tiver arquivo relacionado
                programaIncentivo.IdOrigem = programaIncentivoModel.IdOrigem;
                programaIncentivo.Ano = programaIncentivoModel.Ano;

                repProgramaIncentivo.Update(programaIncentivo);
            }
        }

        public bool DeletarProgramaIncentivo(int programaIncentivoId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repProgramaIncentivoCatalogoArquivo = context.CreateRepository<ProgramaIncentivoCatalogoArquivo>();
                var programaIncentivoCatalogoArquivo = repProgramaIncentivoCatalogoArquivo.Find<ProgramaIncentivoCatalogoArquivo>(x => x.ProgramaIncentivoId == programaIncentivoId);

                if (programaIncentivoCatalogoArquivo == null)
                {
                    IRepository repProgramaIncentivo = context.CreateRepository<ProgramaIncentivo>();
                    repProgramaIncentivo.Delete<ProgramaIncentivo>(x => x.Id == programaIncentivoId);
                    return true;
                }
                else
                {
                    return false;
                }


            }
        }

        public void AtualizarIdOrigem(List<ProgramaIncentivo> programaIncentivos)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                context.ProgramaIncentivoRepository().AtualizarIdOrigem(programaIncentivos);
            }
        }
    }
}
