using System;
using System.Collections.Generic;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class GrupoItem
    {
        public GrupoItem()
        {
            this.CampanhaAssociacaoGrupoItems = new List<CampanhaAssociacaoGrupoItem>();
            this.CampanhaAssociacaoGrupoItemImportacaos = new List<CampanhaAssociacaoGrupoItemImportacao>();
            this.CampanhaFaixaAtingimentoGrupoItems = new List<CampanhaFaixaAtingimentoGrupoItem>();
            this.CampanhaGrupoItem = new List<CampanhaGrupoItem>();
            this.CampanhaMetaGrupoItemPerfils = new List<CampanhaMetaGrupoItemPerfil>();
            this.CampanhaResultadoCalculadoParticipantes = new List<CampanhaResultadoCalculadoParticipante>();
            this.GrupoItem1 = new List<GrupoItem>();
        }

        public int Id { get; set; }
        public Nullable<int> PaiId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Codigo { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInclusao { get; set; }
        public Nullable<DateTime> DataAlteracao { get; set; }
        public Nullable<DateTime> DataInativacao { get; set; }
        public virtual ICollection<CampanhaAssociacaoGrupoItem> CampanhaAssociacaoGrupoItems { get; set; }
        public virtual ICollection<CampanhaAssociacaoGrupoItemImportacao> CampanhaAssociacaoGrupoItemImportacaos { get; set; }
        public virtual ICollection<CampanhaFaixaAtingimentoGrupoItem> CampanhaFaixaAtingimentoGrupoItems { get; set; }
        public virtual ICollection<CampanhaGrupoItem> CampanhaGrupoItem { get; set; }
        public virtual ICollection<CampanhaMetaGrupoItemPerfil> CampanhaMetaGrupoItemPerfils { get; set; }
        public virtual ICollection<CampanhaResultadoCalculadoParticipante> CampanhaResultadoCalculadoParticipantes { get; set; }
        public virtual ICollection<GrupoItem> GrupoItem1 { get; set; }
        public virtual GrupoItem GrupoItem2 { get; set; }
    }
}
