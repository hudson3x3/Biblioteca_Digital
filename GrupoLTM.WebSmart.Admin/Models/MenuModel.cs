using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrupoLTM.WebSmart.Admin.Models
{
    public class MenuModel
    {
        public int Id { get; set; }
        public int? MenuPaiId { get; set; }
        public string MenuPai { get; set; }
        public string Nome { get; set; }
        public string Link { get; set; }
        public string Target { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public bool Ativo { get; set; }
        public string Icone { get; set; }
        public string Titulo { get; set; }
        public int? Ordem { get; set; }

        public int[] PerfilId { get; set; }
        public int[] EstruturaId { get; set; }
        public ArrayList ArrPerfilId { get; set; }
        public ArrayList ArrEstruturaId { get; set; }
    }

    public class MenuUsuarioModel
    {
        public int Id { get; set; }
        public int? MenuPaiId { get; set; }
        public string Nome { get; set; }
        public string Titulo { get; set; }
        public string Link { get; set; }
        public string Target { get; set; }
        public string Icone { get; set; }
    }
}