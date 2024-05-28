using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GrupoLTM.WebSmart.Admin.Models
{
    public class CampanhaPasso1Model
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string DataInicio { get; set; }
        public string DataFim { get; set; }
        public bool? ResultadoCascata { get; set; }
        public bool? CalcularPelaHierarquia { get; set; }
        public bool? ExibirPerido { get; set; }
        
    }
}
