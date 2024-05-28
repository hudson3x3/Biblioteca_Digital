using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class ConteudoImagem
    {
        public int Id { get; set; }
        public int ConteudoId { get; set; }
        public string imgP { get; set; }
        public string imgM { get; set; }
        public string imgG { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime DataAlteracao { get; set; }
        public bool Ativo { get; set; }
        public virtual Conteudo Conteudo { get; set; }
    }
}
