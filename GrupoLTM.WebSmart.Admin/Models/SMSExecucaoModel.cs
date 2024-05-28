using GrupoLTM.WebSmart.Domain.Models;
using System;
using static GrupoLTM.WebSmart.Domain.Enums.EnumDomain;

namespace GrupoLTM.WebSmart.Admin.Models
{
    public class SMSExecucaoModel
    {
        public Guid Id { get; set; }
        public Guid SMSAgendamentoId { get; set; }
        public string StatusMensagem { get; set; }
        public DateTime InicioExecucao { get; set; }
        public DateTime? FimExecucao { get; set; }
        public SMSExecucaoStatus Status { get; set; }
        public bool Sucesso { get; set; }
    }
}