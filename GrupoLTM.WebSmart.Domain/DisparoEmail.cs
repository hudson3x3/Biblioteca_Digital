using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class DisparoEmail
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public bool Enviado { get; set; }
        public Nullable<DateTime> DataEnvio { get; set; }
        public Nullable<int> IdParticipante { get; set; }
        public int IdDisparoEmailTipo { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInclusao { get; set; }
        public Nullable<DateTime> DataAlteracao { get; set; }
        public virtual DisparoEmailTipo DisparoEmailTipo { get; set; }
        public virtual Participante Participante { get; set; }
    }
}
