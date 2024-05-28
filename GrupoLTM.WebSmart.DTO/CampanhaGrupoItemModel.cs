using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrupoLTM.WebSmart.DTO
{
    public class CampanhaGrupoItemModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Codigo { get; set; }
        public System.DateTime? DataInclusao { get; set; }
        public System.DateTime? DataAlteracao { get; set; }
        public System.DateTime? DataInativacao { get; set; }

    }
}
