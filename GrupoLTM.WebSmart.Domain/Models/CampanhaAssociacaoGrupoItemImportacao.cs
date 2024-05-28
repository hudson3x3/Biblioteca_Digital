using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class CampanhaAssociacaoGrupoItemImportacao
    {
        public string Perfil { get; set; }
        public string Estrutura { get; set; }
        public string GrupoItem { get; set; }
        public int ArquivoId { get; set; }
        public Nullable<int> CampanhaId { get; set; }
        public Nullable<int> PeriodoId { get; set; }
        public int Id { get; set; }
        public Nullable<int> GrupoItemId { get; set; }
        public Nullable<int> PerfilId { get; set; }
        public Nullable<int> EstruturaId { get; set; }
        public string Erro { get; set; }
        public Nullable<DateTime> DataInclusao { get; set; }
        public Nullable<DateTime> DataAlteracao { get; set; }
        public virtual Estrutura Estrutura1 { get; set; }
        public virtual GrupoItem GrupoItem1 { get; set; }
        public virtual Perfil Perfil1 { get; set; }
    }
}
