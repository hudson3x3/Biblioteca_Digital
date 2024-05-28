using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class FaleConosco
    {
        public int Id { get; set; }
        public int AssuntoId { get; set; }
        public int StatusFaleConoscoId { get; set; }
        public Nullable<int> ParticipanteId { get; set; }
        public Nullable<int> UsuarioAdminId { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string DDDTel { get; set; }
        public string Telefone { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public string Resposta { get; set; }
        public Nullable<DateTime> DataResposta { get; set; }
        public Nullable<DateTime> DataInclusao { get; set; }
        public Nullable<DateTime> DataAlteracao { get; set; }
        public Nullable<bool> Ativo { get; set; }
        public virtual Assunto Assunto { get; set; }
        public virtual Participante Participante { get; set; }
        public virtual StatusFaleConosco StatusFaleConosco { get; set; }
        public virtual UsuarioAdm UsuarioAdm { get; set; }
    }
}
