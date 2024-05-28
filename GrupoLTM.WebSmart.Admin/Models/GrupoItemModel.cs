using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrupoLTM.WebSmart.Admin.Models
{
    public class GrupoItemModel
    {
        public int Id { get; set; }
        public int? PaiId { get; set; }
        public string Nome { get; set; }
        public string NomePai { get; set; }
        public string Descricao { get; set; }
        public string Codigo { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? Dataalteracao { get; set; }

    }
}