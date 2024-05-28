using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class ConteudoPerfil
    {
        public int Id { get; set; }
        public int ConteudoId { get; set; }
        public int PerfilId { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInclusao { get; set; }
        public Nullable<DateTime> DataAlteracao { get; set; }
        public virtual Conteudo Conteudo { get; set; }
        public virtual Perfil Perfil { get; set; }
    }
}
