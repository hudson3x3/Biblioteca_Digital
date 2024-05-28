using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public class Log
    {
        public int Id { get; set; }
        public Nullable<int> IdParticipante { get; set; }
        public string LogDescricao { get; set; }
        public string Ip { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string TipoLog { get; set; }
        public string HostName { get; set; }
        public DateTime DataInclusao { get; set; }
        public Nullable<DateTime> DataAlteracao { get; set; }
        public string Metodo { get; set; }
        public string Browser { get; set; }
        public string Versao { get; set; }
        public string Pagina { get; set; }
        public string Evento { get; set; }
        public string Acao { get; set; }
        public string Objeto { get; set; }
        public Nullable<bool> Teste { get; set; }
    }
}
