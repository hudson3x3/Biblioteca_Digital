using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrupoLTM.WebSmart.DTO
{
    public class CampanhaEstruturaModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int CampanhaId { get; set; }
        public int EstruturaId { get; set; }
        public int TipoEstrutura { get; set; }
        public bool Ativo { get; set; }

        public bool Participa { get; set; }
        public System.DateTime DataInclusao { get; set; }
        public System.DateTime DataAlteracao { get; set; }

    }
}
