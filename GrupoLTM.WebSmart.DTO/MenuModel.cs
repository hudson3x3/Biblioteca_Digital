using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrupoLTM.WebSmart.DTO
{
    public class MenuModel
    {
        public int Id { get; set; }
        public int? MenuPaiId { get; set; }
        public string Nome { get; set; }
        public string Titulo { get; set; }
        public string Link { get; set; }
        public string Target { get; set; }
        public string Icone { get; set; }
        public System.DateTime? DataInicio { get; set; }
        public System.DateTime? DataFim { get; set; }
        public System.DateTime DataInclusao { get; set; }
        public System.DateTime? DataAlteracao { get; set; }
        public List<MenuModel> SubMenu { get; set; }
        public bool Ativo { get; set; }
    }
}
