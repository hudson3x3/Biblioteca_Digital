using System.Collections.Generic;

namespace GrupoLTM.WebSmart.Admin.Models
{
    public class SMSExecucoesModel
    {
        public SMSExecucoesModel()
        {
            Execucoes = new SMSExecucaoModel[] { };
        }

        public SMSAgendamentoModel Agendamento { get; set; }
        public IEnumerable<SMSExecucaoModel> Execucoes { get; set; }
    }
}