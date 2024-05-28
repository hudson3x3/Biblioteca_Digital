using System;
using System.Collections.Generic;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class SMSAgendamentoImagem
    {
        public SMSAgendamentoImagem()
        {
          // this.Pontuacoes = new List<Pontuacao>();
        }

        public int Id { get; set; }
        public string NomeImagem { get; set; }
        public DateTime DataInclusao { get; set; }
        public Nullable<DateTime> DataAlteracao { get; set; }
        public string CaminhoArquivo { get; set; }

        public Guid SMSAgendamentoId { get; set; }

      //  public virtual ICollection<Arquivo> SmsAgendamento { get; set; }


    }
}
