using System;
using System.Collections.Generic;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class Campanha
    {
        public Campanha()
        {
            this.CampanhaConteudoes = new List<CampanhaConteudo>();
            this.CampanhaEstrutura = new List<CampanhaEstrutura>();
            this.CampanhaGrupoItems = new List<CampanhaGrupoItem>();
            this.CampanhaPerfil = new List<CampanhaPerfil>();
            this.CampanhaPeriodo = new List<CampanhaPeriodo>();
            this.CampanhaLogArquivoes = new List<CampanhaLogArquivo>();
            this.CampanhaMetaGrupoItemPerfils = new List<CampanhaMetaGrupoItemPerfil>();
            this.CampanhaPeriodoParticipanteEstruturas = new List<CampanhaPeriodoParticipanteEstrutura>();
            this.CampanhaPeriodoParticipantePerfils = new List<CampanhaPeriodoParticipantePerfil>();
            this.CampanhaPassoes = new List<CampanhaPasso>();
            this.CampanhaPeriodoParticipanteHierarquias = new List<CampanhaPeriodoParticipanteHierarquia>();
            this.CampanhaResultadoCalculadoParticipantes = new List<CampanhaResultadoCalculadoParticipante>();
            this.CampanhaPeriodoParticipanteHierarquias1 = new List<CampanhaPeriodoParticipanteHierarquia>();
            this.CampanhaPeriodoParticipantePerfils1 = new List<CampanhaPeriodoParticipantePerfil>();
            this.PontuacaoCampanhaPeriodoes = new List<PontuacaoCampanhaPeriodo>();
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public DateTime DataInclusao { get; set; }
        public Nullable<DateTime> Dataalteracao { get; set; }
        public int StatusCampanhaId { get; set; }
        public Nullable<int> TipoCampanhaId { get; set; }
        public bool ResultadoCascata { get; set; }
        public bool ExibirPeriodo { get; set; }
        public bool CalcularPelaHierarquia { get; set; }
        public bool ExibirRankingIndividual { get; set; }
        public virtual ICollection<CampanhaConteudo> CampanhaConteudoes { get; set; }
        public virtual ICollection<CampanhaEstrutura> CampanhaEstrutura { get; set; }
        public virtual ICollection<CampanhaGrupoItem> CampanhaGrupoItems { get; set; }
        public virtual ICollection<CampanhaPerfil> CampanhaPerfil { get; set; }
        public virtual ICollection<CampanhaPeriodo> CampanhaPeriodo { get; set; }
        public virtual StatusCampanha StatusCampanha { get; set; }
        public virtual TipoCampanha TipoCampanha { get; set; }
        public virtual ICollection<CampanhaLogArquivo> CampanhaLogArquivoes { get; set; }
        public virtual ICollection<CampanhaMetaGrupoItemPerfil> CampanhaMetaGrupoItemPerfils { get; set; }
        public virtual ICollection<CampanhaPeriodoParticipanteEstrutura> CampanhaPeriodoParticipanteEstruturas { get; set; }
        public virtual ICollection<CampanhaPeriodoParticipantePerfil> CampanhaPeriodoParticipantePerfils { get; set; }
        public virtual ICollection<CampanhaPasso> CampanhaPassoes { get; set; }
        public virtual ICollection<CampanhaPeriodoParticipanteHierarquia> CampanhaPeriodoParticipanteHierarquias { get; set; }
        public virtual ICollection<CampanhaResultadoCalculadoParticipante> CampanhaResultadoCalculadoParticipantes { get; set; }
        public virtual ICollection<CampanhaPeriodoParticipanteHierarquia> CampanhaPeriodoParticipanteHierarquias1 { get; set; }
        public virtual ICollection<CampanhaPeriodoParticipantePerfil> CampanhaPeriodoParticipantePerfils1 { get; set; }
        public virtual ICollection<PontuacaoCampanhaPeriodo> PontuacaoCampanhaPeriodoes { get; set; }
    }
}
