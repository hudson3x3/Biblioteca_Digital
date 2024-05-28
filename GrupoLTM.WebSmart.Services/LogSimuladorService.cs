using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace GrupoLTM.WebSmart.Services
{
    public class LogSimuladorService : BaseService<LogSimulador>
    {
        public bool SalvarLogSimulador(LogSimulador logSimulador)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository rep = context.CreateRepository<LogSimulador>();
                    LogSimulador _logSimulador = new LogSimulador
                    {

                        ParticipanteId = logSimulador.ParticipanteId,
                        Participante = logSimulador.Participante,
                        IP = logSimulador.IP,
                        AccountNumber = logSimulador.AccountNumber,
                        DadosEntrada = logSimulador.DadosEntrada,
                        DadosSaida = logSimulador.DadosSaida,
                        Metodo = logSimulador.Metodo,
                        tokenLTM = logSimulador.tokenLTM,
                        Source = logSimulador.Source,
                        DataInclusao = DateTime.Now

                    };

                    using (TransactionScope scope = new TransactionScope())
                    {
                        rep.Create(_logSimulador);
                        rep.SaveChanges();
                        scope.Complete();
                    }
                }
                return true;

            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }
    }
}
