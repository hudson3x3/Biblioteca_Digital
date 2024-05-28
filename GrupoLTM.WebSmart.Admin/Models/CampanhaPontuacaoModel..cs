using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrupoLTM.WebSmart.Admin.Models
{
    public class CampanhaPontuacaoModel
    {
        public int? Id { get; set; }
        public int? CampanhaId { get; set; }
        public int? CampanhaPeriodoId { get; set; }
        public int? ParticipanteId { get; set; }
        public string Login { get; set; }
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public int? TipoPontuacaoId { get; set; }
        public string TipoPontuacao { get; set; }
        public int? StatusPontuacaoId { get; set; }
        public string StatusPontuacao { get; set; }
        public string DescricaoPonto { get; set; }
        public float Ponto { get; set; }
        public string Periodo { get; set; }
        public string DataInclusao { get; set; }
    }
}