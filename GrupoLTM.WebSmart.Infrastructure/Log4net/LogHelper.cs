using GrupoLTM.WebSmart.DTO;
//using LTM.Logging;
using System.Runtime.CompilerServices;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace Log4NetExample
{
    public class LogHelper
    {
        /// <summary>
        /// http://www.thomaslevesque.com/2012/06/13/using-c-5-caller-info-attributes-when-targeting-earlier-versions-of-the-net-framework/
        /// https://msdn.microsoft.com/pt-br/library/system.runtime.compilerservices.callerfilepathattribute%28v=vs.110%29.aspx
        /// </summary>
        /// <param name="fileName">Valor definido em momento de compilacao</param>
        /// <returns></returns>
        public static log4net.ILog GetLogger([CallerFilePath] string fileName = "")
        {
            return log4net.LogManager.GetLogger(fileName);
        }

        //public static void Acesso(string correlationId, LogAcessoModel logAcesso)
        //{
        //    var log = GetLogger();

        //    var item = new CorrelatedLog(correlationId: correlationId)
        //    {
        //        { "ParticipanteId", logAcesso.ParticipanteId },
        //        { "Participante", logAcesso.Participante },
        //        { "AccountNumber", logAcesso.AccountNumber },
        //        { "Pagina", logAcesso.Pagina },
        //        { "DataInclusao", logAcesso.DataInclusao },
        //        { "IP", logAcesso.IP },
        //        { "Catalogo", logAcesso.Catalogo },
        //        { "severity", "Information" },
        //    };

        //    log.Info(item);
        //}

        //public static void Impersonate(string correlationId, LogAcessoImpersonateModel logImpersonate)
        //{
        //    var log = GetLogger();

        //    var item = new CorrelatedLog(correlationId: correlationId)
        //    {
        //        { "AccountNumber",  logImpersonate.AccountNumber },
        //        { "ParticipanteId",  logImpersonate.ParticipanteId },
        //        { "ParticipanteNome",  logImpersonate.ParticipanteNome },
        //        { "AccountNumberImpersonate",  logImpersonate.AccountNumberImpersonate },
        //        { "ParticipanteIdImpersonate",  logImpersonate.ParticipanteIdImpersonate },
        //        { "ParticipanteNomeImpersonate",  logImpersonate.ParticipanteNomeImpersonate },
        //        { "CatalogoId",  logImpersonate.CatalogoId },
        //        { "IP",  logImpersonate.IP },
        //        { "Pagina",  logImpersonate.Pagina },
        //    };

        //    log.Info(item);
        //}

        //public static void Erro(string correlationId, LogErroModel logErro)
        //{
        //    var log = GetLogger();

        //    var item = new CorrelatedLog(correlationId: correlationId)
        //    {
        //        { "ParticipanteId", logErro.ParticipanteId },
        //        { "Participante", logErro.Participante },
        //        { "AccountNumber", logErro.AccountNumber },
        //        { "Entrada", logErro.ParametroDeEntrada },
        //        { "Saida", logErro.ParametroDeSaida },
        //        { "Servico", logErro.Servico },
        //        { "DescricaoErro", logErro.Erro },
        //        { "StackTrace", logErro.StackTrace },
        //        { "IP", logErro.IP },
        //        { "DataInclusao", logErro.DataInclusao },
        //        { "message", logErro.Erro },
        //        { "severity", "Error" },
        //    };

        //    log.Error(item);
        //}
    }
}
