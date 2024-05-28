using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrupoLTM.WebSmart.DTO
{
    public class CampanhaPeriodoModel
    {
        public int Id { get; set; }
        public int CampanhaId { get; set; }
        public string Nome { get; set; }
        public string TipoCampanha { get; set; }
        public bool Ativo { get; set; }
        public bool Apurado { get; set; }
        public DateTime? PeriodoDe { get; set; }
        public DateTime? PeriodoAte { get; set; }
        public DateTime? DataFechamento { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }
}
