using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrupoLTM.WebSmart.DTO
{
    public class CampanhaSimuladorModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int NumeroCampanha { get; set; }
        public string Descricao { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public bool Ativo { get; set; }
        public int SomaPontos { get; set; }
        public string LinkDownload { get; set; }
        public virtual List<CampanhaMecanicaSimuladorModel> CampanhaMecanicaSimulador { get; set; }
    }
}
