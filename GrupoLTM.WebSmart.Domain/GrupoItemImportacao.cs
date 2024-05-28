using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class GrupoItemImportacao
    {
        public string Grupo { get; set; }
        public string GrupoDescricao { get; set; }
        public string GrupoCodigo { get; set; }
        public string Item { get; set; }
        public string ItemDescricao { get; set; }
        public string ItemCodigo { get; set; }
        public int ArquivoId { get; set; }
        public Nullable<int> GrupoItemPaiId { get; set; }
        public Nullable<int> GrupoItemId { get; set; }
        public DateTime DataInclusao { get; set; }
        public Nullable<DateTime> DataAlteracao { get; set; }
        public string Erro { get; set; }
        public int Id { get; set; }
    }
}
