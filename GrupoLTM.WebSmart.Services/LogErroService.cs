using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.DTO;
using GrupoLTM.WebSmart.Infrastructure.Helpers;
using GrupoLTM.WebSmart.Services.Log;
using System;
using System.Diagnostics;
using System.Linq;
using System.Transactions;

namespace GrupoLTM.WebSmart.Services
{
    public class LogErroService : BaseService<LogErro>
    {
        public static void LogErro(Exception exception, string mensagem, string controller, string metodo, string codigo)
        {
            try
            {
                var erro = string.Join(" => ", Helper.GetInnerExceptions(exception).Select(x => x.Message));

                var logErro = new LogErro
                {
                    Codigo = codigo,
                    Controller = controller,
                    Erro = erro,
                    Mensagem = mensagem,
                    Metodo = metodo,
                    Source = exception.StackTrace,
                    DataInclusao = DateTime.Now
                };

                using (var context = UnitOfWorkFactory.Create())
                {
                    var rep = context.CreateRepository<LogErro>();
                    rep.Create(logErro);
                    rep.SaveChanges();
                }

                var logGrayLog = new LogProcessamentoModel
                {
                    Class = controller,
                    Date = DateTime.Now,
                    Message = mensagem,
                    Method = metodo,
                    Source = exception.StackTrace
                };

                //TODO: Update DataDog
                //GrayLogService.LogError(logGrayLog);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Não foi possível gravar o log de erro: " + ex.Message);
            }
        }

        public bool SalvarLogErro(LogErro logErro)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository rep = context.CreateRepository<LogErro>();
                    LogErro _logErro = new LogErro
                    {
                        Codigo = logErro.Codigo,
                        Controller = logErro.Controller,
                        Erro = logErro.Erro,
                        Evento = logErro.Evento,
                        Mensagem = logErro.Mensagem,
                        Metodo = logErro.Metodo,
                        Pagina = logErro.Pagina,
                        DadosEntrada = logErro.DadosEntrada,
                        Source = logErro.Source,
                        UsuarioSessao = logErro.UsuarioSessao,
                        DataInclusao = DateTime.Now,
                        TokenLtm = logErro.TokenLtm
                    };

                    using (TransactionScope scope = new TransactionScope())
                    {
                        rep.Create(_logErro);
                        rep.SaveChanges();
                        scope.Complete();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Não foi possível gravar o log de erro: " + ex.Message);
                return false;
            }
        }
    }
}
