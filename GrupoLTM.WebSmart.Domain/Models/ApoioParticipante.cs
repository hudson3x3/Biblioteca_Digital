using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public class ApoioParticipante
    {
        public int Id { get; set; }
        public string RA { get; set; }
        public decimal SaldoPontos { get; set; }
        public string DDD_Live { get; set; }
        public string Celular_Live { get; set; }
        public string DDD_FTP { get; set; }
        public string Celular_FTP { get; set; }
        public DateTime DataProcessamento { get; set; }
    }
}
