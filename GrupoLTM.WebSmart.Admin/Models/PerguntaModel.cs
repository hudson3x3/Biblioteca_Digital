using GrupoLTM.WebSmart.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrupoLTM.WebSmart.Admin.Models
{
    public class PerguntaModel
    {
        public int Id { get; set; }
        public int QuestionarioId { get; set; }
        public string Questionario { get; set; }
        public EnumDomain.TipoQuestionario TipoQuestionario { get; set; }
        public int TipoRespostaId { get; set; }
        public string TipoResposta { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public bool AtivoPontos { get; set; }
        public double? ValorPontos { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }
}