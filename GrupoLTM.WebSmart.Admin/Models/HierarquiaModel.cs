using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrupoLTM.WebSmart.Admin.Models
{
    public class HierarquiaModel
    {
        public int Id { get; set; }
        public int ParticipanteId { get; set; }
        public int? ParticipanteIdPai { get; set; }
        public int PeriodoId { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public bool Ativo { get; set; }

    }
}