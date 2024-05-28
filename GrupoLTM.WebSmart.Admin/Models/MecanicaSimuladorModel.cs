using GrupoLTM.WebSmart.Domain.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrupoLTM.WebSmart.Admin.Models
{
    public class MecanicaSimuladorModel
    {
        public MecanicaSimuladorModel()
        {
            IdCampanhaSimulador = new int[] { };
            IdSubMecanicaSimulador = new int[] { };
            IconeSimuladorList = new List<IconeSimuladorModel>();
        }

        public int Id { get; set; }
        public int? IdIconeSimulador { get; set; }
        public int[] IdCampanhaSimulador { get; set; }
        public ArrayList ArrCampanhaSimuladorId { get; set; }
        public int[] IdSubMecanicaSimulador { get; set; }
        public ArrayList ArrSubMecanicaSimuladorId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public List<IconeSimuladorModel> IconeSimuladorList { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }
}