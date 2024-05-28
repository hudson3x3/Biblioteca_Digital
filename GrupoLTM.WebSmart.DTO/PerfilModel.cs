using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrupoLTM.WebSmart.DTO
{
    public class PerfilModel
    {
        public int Id { get; set; }
        public int? PaiId { get; set; }
        public string Nome { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime DataAlteracao { get; set; }
        public bool Adm { get; set; }
        public bool Ativo { get; set; }
        public int? NivelHierarquia { get; set; }
    }
}
