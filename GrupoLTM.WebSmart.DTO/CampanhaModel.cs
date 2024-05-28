using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrupoLTM.WebSmart.DTO
{
    public class CampanhaModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public System.DateTime DataInicio { get; set; }
        public System.DateTime DataFim { get; set; }
        public System.DateTime DataInclusao { get; set; }
        public System.DateTime? DataAlteracao { get; set; }
        public int? StatusCampanhaId { get; set; }
        public int? TipoCampanhaId { get; set; }
        public string StatusCampanha { get; set; }
        public string TipoCampanha { get; set; }
        public string ImagemBannerHome { get; set; }
        public string ImagemMecanicaMobile { get; set; }
        public string ImagemBanner { get; set; }
        public string Url { get; set; }
        public string Regulamento { get; set; }
        public bool? ResultadoCascata { get; set; }
        public bool? ExibirRankingIndividual { get; set; }
    }
}
