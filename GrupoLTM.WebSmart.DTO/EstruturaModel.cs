using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrupoLTM.WebSmart.DTO
{
    public class EstruturaModel
    {
        public int Id { get; set; }
        public int? PaiId { get; set; }
        public int? PeriodoId { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime DataAlteracao { get; set; }
        public bool Ativo { get; set; }
    }
}
