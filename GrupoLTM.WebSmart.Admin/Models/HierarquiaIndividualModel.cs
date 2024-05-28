using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrupoLTM.WebSmart.Admin.Models
{
    public class HierarquiaIndividualModel
    {
        public int ParticipanteId { get; set; }
        public int? ParticipanteIdPai { get; set; }
        public int PeriodoId { get; set; }
        public int PerfilID { get; set; }
        public int NivelHierarquia { get; set; }
        public int? PerfilPaiID { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public bool EstaNaHierarquia { get; set; }
        public string Perfil { get; set; }
    }
}