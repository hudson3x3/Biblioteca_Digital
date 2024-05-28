using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class ConteudoEstrutura
    {
        public int Id { get; set; }
        public int ConteudoId { get; set; }
        public int EstruturaId { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInclusao { get; set; }
        public Nullable<DateTime> DataAlteracao { get; set; }
        public virtual Conteudo Conteudo { get; set; }
        public virtual Estrutura Estrutura { get; set; }
    }
}
