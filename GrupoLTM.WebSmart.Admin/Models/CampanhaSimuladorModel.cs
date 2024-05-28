using GrupoLTM.WebSmart.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrupoLTM.WebSmart.Admin.Models
{
    public class CampanhaSimuladorModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public int NumeroCampanha { get; set; }
        public int AnoCampanha { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public HttpPostedFileBase FileArquivo { get; set; }
        public string LinkDownload { get; set; }
    }
}