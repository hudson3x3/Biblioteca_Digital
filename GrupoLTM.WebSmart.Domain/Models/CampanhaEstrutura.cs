using System;
using System.Collections.Generic;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class CampanhaEstrutura
    {
        public CampanhaEstrutura()
        {
            this.CampanhaAssociacaoGrupoItems = new List<CampanhaAssociacaoGrupoItem>();
            this.CampanhaMetaGrupoItemPerfils = new List<CampanhaMetaGrupoItemPerfil>();
            this.FaixaAtingementoes = new List<FaixaAtingemento>();
            this.CampanhaFaixaAtingimentoes = new List<CampanhaFaixaAtingimento>();
            this.CampanhaFaixaAtingimentoGrupoItems = new List<CampanhaFaixaAtingimentoGrupoItem>();
            this.CampanhaGrupoItemPontos = new List<CampanhaGrupoItemPonto>();
            this.CampanhaResultadoCalculadoParticipantes = new List<CampanhaResultadoCalculadoParticipante>();
            this.Faixas = new List<Faixa>();
        }

        public int Id { get; set; }
        public int CampanhaId { get; set; }
        public int EstruturaId { get; set; }
        public bool Ativo { get; set; }
        public bool Participa { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime DataAlteracao { get; set; }
        public virtual Campanha Campanha { get; set; }
        public virtual ICollection<CampanhaAssociacaoGrupoItem> CampanhaAssociacaoGrupoItems { get; set; }
        public virtual ICollection<CampanhaMetaGrupoItemPerfil> CampanhaMetaGrupoItemPerfils { get; set; }
        public virtual ICollection<FaixaAtingemento> FaixaAtingementoes { get; set; }
        public virtual ICollection<CampanhaFaixaAtingimento> CampanhaFaixaAtingimentoes { get; set; }
        public virtual ICollection<CampanhaFaixaAtingimentoGrupoItem> CampanhaFaixaAtingimentoGrupoItems { get; set; }
        public virtual ICollection<CampanhaGrupoItemPonto> CampanhaGrupoItemPontos { get; set; }
        public virtual ICollection<CampanhaResultadoCalculadoParticipante> CampanhaResultadoCalculadoParticipantes { get; set; }
        public virtual Estrutura Estrutura { get; set; }
        public virtual ICollection<Faixa> Faixas { get; set; }
    }
}
