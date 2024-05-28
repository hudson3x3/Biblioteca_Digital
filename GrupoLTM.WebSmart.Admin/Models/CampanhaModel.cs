using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrupoLTM.WebSmart.Admin.Models
{
    public class CampanhaModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public System.DateTime DataInicio { get; set; }
        public System.DateTime DataFim { get; set; }
        public System.DateTime DataInclusao { get; set; }
        public System.DateTime? DataAlteracao { get; set; }
        public int StatusCampanhaId { get; set; }
        public int TipoCampanhaId { get; set; }
    }
}