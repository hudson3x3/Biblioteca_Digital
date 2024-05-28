using System;
using System.Collections.Generic;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class Estrutura
    {
        public Estrutura()
        {
            this.CampanhaAssociacaoGrupoItemImportacaos = new List<CampanhaAssociacaoGrupoItemImportacao>();
            this.CampanhaEstruturas = new List<CampanhaEstrutura>();
            this.CampanhaPeriodoParticipanteEstruturas = new List<CampanhaPeriodoParticipanteEstrutura>();
            this.ConteudoEstruturas = new List<ConteudoEstrutura>();
            this.Estrutura1 = new List<Estrutura>();
            this.MenuEstruturas = new List<MenuEstrutura>();
            this.ModuloEstruturas = new List<ModuloEstrutura>();
            this.ParticipanteEstruturas = new List<ParticipanteEstrutura>();
            this.ParticipanteImportacaos = new List<ParticipanteImportacao>();
            this.QuestionarioEstruturas = new List<QuestionarioEstrutura>();
        }

        public int Id { get; set; }
        public Nullable<int> PaiId { get; set; }
        public Nullable<int> PeriodoId { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }
        public DateTime DataInclusao { get; set; }
        public Nullable<DateTime> DataAlteracao { get; set; }
        public bool Ativo { get; set; }
        public Nullable<int> TipoEstruturaId { get; set; }
        public virtual ICollection<CampanhaAssociacaoGrupoItemImportacao> CampanhaAssociacaoGrupoItemImportacaos { get; set; }
        public virtual ICollection<CampanhaEstrutura> CampanhaEstruturas { get; set; }
        public virtual ICollection<CampanhaPeriodoParticipanteEstrutura> CampanhaPeriodoParticipanteEstruturas { get; set; }
        public virtual ICollection<ConteudoEstrutura> ConteudoEstruturas { get; set; }
        public virtual ICollection<Estrutura> Estrutura1 { get; set; }
        public virtual Estrutura Estrutura2 { get; set; }
        public virtual Periodo Periodo { get; set; }
        public virtual ICollection<MenuEstrutura> MenuEstruturas { get; set; }
        public virtual ICollection<ModuloEstrutura> ModuloEstruturas { get; set; }
        public virtual ICollection<ParticipanteEstrutura> ParticipanteEstruturas { get; set; }
        public virtual ICollection<ParticipanteImportacao> ParticipanteImportacaos { get; set; }
        public virtual ICollection<QuestionarioEstrutura> QuestionarioEstruturas { get; set; }
        public virtual TipoEstrutura TipoEstrutura { get; set; }
    }
}
