namespace GrupoLTM.WebSmart.Admin.Models
{
    public class LoginModel
    {
        public int Id { get; set; }
        public int PerfilId { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public bool Ativo { get; set; }
        public bool EmailRecuperacaoEnviado { get; set; }

        public string NomeCampanha { get; set; }
        public int TipoAcessoId { get; set; }
        public int TipoCadastroId { get; set; }
        public int TipoValidacaoPositivaId { get; set; }
        public string Logo { get; set; }

        public bool? AtivoWP { get; set; }
        public bool? AtivoBoxSaldo { get; set; }
        public bool? AtivoBoxVitrine { get; set; }
    }
}