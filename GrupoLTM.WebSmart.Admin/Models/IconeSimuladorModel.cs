using GrupoLTM.WebSmart.Domain.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrupoLTM.WebSmart.Admin.Models
{
    public class IconeSimuladorModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Tipo { get; set; }
        public string CaminhoImagem { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }
}