using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repository;
using System;
using System.Transactions;


namespace GrupoLTM.WebSmart.Services
{
    public class ParticipanteCatalogoService : BaseService<ParticipanteCatalogo>
    {
        public ParticipanteCatalogo Inserir(ParticipanteCatalogo participanteCatalogo)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repLog = context.CreateRepository<ParticipanteCatalogo>();

                    using (TransactionScope scope = new TransactionScope())
                    {
                        repLog.Create(participanteCatalogo);
                        repLog.SaveChanges();
                        scope.Complete();
                    }
                }

                return participanteCatalogo;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetUltimoAcesso(string Login, int CatalogoId)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repParticipanteCatalogo = context.CreateRepository<ParticipanteCatalogo>();

                    var participanteCatalogo = repParticipanteCatalogo.Find<ParticipanteCatalogo>(x => x.Participante.Login == Login && x.Catalogo.Codigo == CatalogoId);

                    if (participanteCatalogo != null)
                    {
                        participanteCatalogo.DataUltimoAcesso = DateTime.Now;
                        repParticipanteCatalogo.Update<ParticipanteCatalogo>(participanteCatalogo);
                        repParticipanteCatalogo.SaveChanges();
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ParticipanteCatalogo GetByLoginCatalogo(string Login, int CatalogoId)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repParticipanteCatalogo = context.CreateRepository<ParticipanteCatalogo>();

                    var participanteCatalogo = repParticipanteCatalogo.Find<ParticipanteCatalogo>(x => x.Participante.Login == Login && x.Catalogo.Codigo == CatalogoId);
                    return participanteCatalogo;
                }

            }
            catch (Exception ex)
            {
                return null;
                throw ex;

            }
        }

    }
}
