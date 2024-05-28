using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class CampanhaConteudo
    {
        public int Id { get; set; }
        public int CampanhaId { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public int TipoconteudoId { get; set; }
        public Nullable<int> Ordem { get; set; }
        public string Titulo { get; set; }
        public string Texto { get; set; }
        public string PreTexto { get; set; }
        public string Arquivo { get; set; }
        public virtual Campanha Campanha { get; set; }
        public virtual TipoConteudo TipoConteudo { get; set; }
    }
}
