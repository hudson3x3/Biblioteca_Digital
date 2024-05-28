using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class LogAcao
    {
        public int Id { get; set; }
        public string TokenLtm { get; set; }
        public int LogTipoId { get; set; }
        public string Evento { get; set; }
        public string Descricao { get; set; }
        public string DadosEntrada { get; set; }
        public string DadosSaida { get; set; }
        public Nullable<Double> TempoProcessamento { get; set; }
        public DateTime DataInclusao { get; set; }
        public LogTipo Tipo { get; set; }
        public LogRaiz LogRaiz { get; set; }

    }
}
