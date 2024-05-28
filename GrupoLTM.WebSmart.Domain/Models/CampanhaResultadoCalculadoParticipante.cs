using System;
using System.Collections.Generic;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class CampanhaResultadoCalculadoParticipante
    {
        public CampanhaResultadoCalculadoParticipante()
        {
            this.Pontuacaos = new List<Pontuacao>();
            this.PontuacaoCampanhaPeriodoes = new List<PontuacaoCampanhaPeriodo>();
        }

        public int Id { get; set; }
        public int CampanhaId { get; set; }
        public int CampanhaPerfilId { get; set; }
        public int CampanhaEstruturaId { get; set; }
        public int CampanhaPeriodoId { get; set; }
        public int ParticipanteId { get; set; }
        public Nullable<int> GrupoItemId { get; set; }
        public Nullable<double> Meta { get; set; }
        public Nullable<double> Efetivo { get; set; }
        public Nullable<double> PercentualAtingimento { get; set; }
        public Nullable<int> PosicaoRanking { get; set; }
        public Nullable<double> Pontos { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInclusao { get; set; }
        public Nullable<DateTime> DataAlteracao { get; set; }
        public Nullable<DateTime> DataInativacao { get; set; }
        public virtual Campanha Campanha { get; set; }
        public virtual CampanhaEstrutura CampanhaEstrutura { get; set; }
        public virtual CampanhaPerfil CampanhaPerfil { get; set; }
        public virtual CampanhaPeriodo CampanhaPeriodo { get; set; }
        public virtual GrupoItem GrupoItem { get; set; }
        public virtual Participante Participante { get; set; }
        public virtual ICollection<Pontuacao> Pontuacaos { get; set; }
        public virtual ICollection<PontuacaoCampanhaPeriodo> PontuacaoCampanhaPeriodoes { get; set; }
    }
}
