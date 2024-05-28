using GrupoLTM.WebSmart.Domain.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrupoLTM.WebSmart.Admin.Models
{
    public class SubMecanicaSimuladorModel
    {
        public int Id { get; set; }
        public int[] IdMecanicaSimulador { get; set; }
        public int[] IdCampanhaSimulador { get; set; }
        public ArrayList ArrMecanicaSimuladorId { get; set; }
        public ArrayList ArrCampanhaSimuladorId { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public HttpPostedFileBase FileArquivo { get; set; }
        public string LinkDownload { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }
}