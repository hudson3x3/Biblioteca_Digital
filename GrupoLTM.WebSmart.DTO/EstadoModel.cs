using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrupoLTM.WebSmart.DTO
{
    public class EstadoModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Regiao { get; set; }
        public string Capital { get; set; }
        public System.DateTime DataInclusao { get; set; }
        public Nullable<System.DateTime> DataAlteracao { get; set; }
    }
}
