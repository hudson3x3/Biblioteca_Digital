using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrupoLTM.WebSmart.DTO
{
    public class AssuntoModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public System.DateTime DataInclusao { get; set; }
        public Nullable<System.DateTime> DataAlteracao { get; set; }
        public bool Ativo { get; set; }
    }
}
