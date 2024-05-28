using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrupoLTM.WebSmart.DTO
{
    public class CampanhaResultadoModel
    {
        private List<CampanhaResultadoDetalheModel> _ResultadoDetalhe = new List<CampanhaResultadoDetalheModel>();

        public int CampanhaId { get; set; }
        public int TipoCampanha { get; set; }
        public int PerfilId { get; set; }
        public string Perfil { get; set; }
        public int NivelPerfil { get; set; }
        public int TotalRegistros { get; set; }
        public List<CampanhaResultadoDetalheModel> ResultadoDetalhe { get { return _ResultadoDetalhe; } set { _ResultadoDetalhe = value; } }
    }
}
