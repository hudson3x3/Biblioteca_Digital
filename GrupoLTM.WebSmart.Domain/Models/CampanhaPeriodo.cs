using System;
using System.Collections.Generic;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class CampanhaPeriodo
    {
        public CampanhaPeriodo()
        {
            this.CampanhaAssociacaoGrupoItems = new List<CampanhaAssociacaoGrupoItem>();
            this.CampanhaFaixaAtingimentoes = new List<CampanhaFaixaAtingimento>();
            this.CampanhaFaixaAtingimentoes1 = new List<CampanhaFaixaAtingimento>();
            this.CampanhaFaixaAtingimentoGrupoItems = new List<CampanhaFaixaAtingimentoGrupoItem>();
            this.CampanhaFaixaAtingimentoGrupoItems1 = new List<CampanhaFaixaAtingimentoGrupoItem>();
            this.CampanhaGrupoItemPontos = new List<CampanhaGrupoItemPonto>();
            this.CampanhaLogArquivoes = new List<CampanhaLogArquivo>();
            this.CampanhaMetaGrupoItemPerfils = new List<CampanhaMetaGrupoItemPerfil>();
            this.CampanhaMetaParticipantes = new List<CampanhaMetaParticipante>();
            this.CampanhaMetaPessoas = new List<CampanhaMetaPessoa>();
            this.CampanhaPeriodoParticipanteEstruturas = new List<CampanhaPeriodoParticipanteEstrutura>();
            this.CampanhaPeriodoParticipantePerfils = new List<CampanhaPeriodoParticipantePerfil>();
            this.FaixaAtingementoes = new List<FaixaAtingemento>();
            this.CampanhaPeriodoParticipanteHierarquias = new List<CampanhaPeriodoParticipanteHierarquia>();
            this.CampanhaPeriodoParticipantePerfils1 = new List<CampanhaPeriodoParticipantePerfil>();
            this.CampanhaResultadoCalculadoParticipantes = new List<CampanhaResultadoCalculadoParticipante>();
            this.CampanhaResultadoParticipantes = new List<CampanhaResultadoParticipante>();
            this.Faixas = new List<Faixa>();
            this.PontuacaoCampanhaPeriodoes = new List<PontuacaoCampanhaPeriodo>();
        }

        public int Id { get; set; }
        public int CampanhaId { get; set; }
        public string Nome { get; set; }
        public DateTime PeriodoDe { get; set; }
        public DateTime PeriodoAte { get; set; }
        public DateTime DataFechamento { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime Dataalteracao { get; set; }
        public bool Apurado { get; set; }
        public virtual Campanha Campanha { get; set; }
        public virtual ICollection<CampanhaAssociacaoGrupoItem> CampanhaAssociacaoGrupoItems { get; set; }
        public virtual ICollection<CampanhaFaixaAtingimento> CampanhaFaixaAtingimentoes { get; set; }
        public virtual ICollection<CampanhaFaixaAtingimento> CampanhaFaixaAtingimentoes1 { get; set; }
        public virtual ICollection<CampanhaFaixaAtingimentoGrupoItem> CampanhaFaixaAtingimentoGrupoItems { get; set; }
        public virtual ICollection<CampanhaFaixaAtingimentoGrupoItem> CampanhaFaixaAtingimentoGrupoItems1 { get; set; }
        public virtual ICollection<CampanhaGrupoItemPonto> CampanhaGrupoItemPontos { get; set; }
        public virtual ICollection<CampanhaLogArquivo> CampanhaLogArquivoes { get; set; }
        public virtual ICollection<CampanhaMetaGrupoItemPerfil> CampanhaMetaGrupoItemPerfils { get; set; }
        public virtual ICollection<CampanhaMetaParticipante> CampanhaMetaParticipantes { get; set; }
        public virtual ICollection<CampanhaMetaPessoa> CampanhaMetaPessoas { get; set; }
        public virtual ICollection<CampanhaPeriodoParticipanteEstrutura> CampanhaPeriodoParticipanteEstruturas { get; set; }
        public virtual ICollection<CampanhaPeriodoParticipantePerfil> CampanhaPeriodoParticipantePerfils { get; set; }
        public virtual ICollection<FaixaAtingemento> FaixaAtingementoes { get; set; }
        public virtual ICollection<CampanhaPeriodoParticipanteHierarquia> CampanhaPeriodoParticipanteHierarquias { get; set; }
        public virtual ICollection<CampanhaPeriodoParticipantePerfil> CampanhaPeriodoParticipantePerfils1 { get; set; }
        public virtual ICollection<CampanhaResultadoCalculadoParticipante> CampanhaResultadoCalculadoParticipantes { get; set; }
        public virtual ICollection<CampanhaResultadoParticipante> CampanhaResultadoParticipantes { get; set; }
        public virtual ICollection<Faixa> Faixas { get; set; }
        public virtual ICollection<PontuacaoCampanhaPeriodo> PontuacaoCampanhaPeriodoes { get; set; }
    }
}
