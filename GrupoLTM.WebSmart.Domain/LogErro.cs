using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class LogErro
    {
        public int id { get; set; }
        public string Pagina { get; set; }
        public string Metodo { get; set; }
        public string Controller { get; set; }
        public string Evento { get; set; }
        public string Codigo { get; set; }
        public string Mensagem { get; set; }
        public string Erro { get; set; }
        public string Source { get; set; }
        public string UsuarioSessao { get; set; }
        public DateTime DataInclusao { get; set; }
        public string DadosEntrada { get; set; }
        public string DadosSaida { get; set; }
        public string TokenLtm { get; set; }
    }
}
