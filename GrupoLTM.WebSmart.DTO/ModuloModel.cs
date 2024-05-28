using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrupoLTM.WebSmart.DTO
{
    public class ModuloModel
    {
        public int Id { get; set; }
        public int? ModuloPaiId { get; set; }
        public int TipoModuloId { get; set; }
        public string Nome { get; set; }
        public System.DateTime DataInicio { get; set; }
        public System.DateTime DataFim { get; set; }
        public System.DateTime DataInclusao { get; set; }
        public System.DateTime? DataAlteracao { get; set; }
        public bool Ativo { get; set; }

    }
}
