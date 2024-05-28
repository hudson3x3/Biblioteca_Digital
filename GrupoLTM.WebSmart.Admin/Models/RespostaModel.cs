using GrupoLTM.WebSmart.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrupoLTM.WebSmart.Admin.Models
{
    public class RespostaModel
    {
        public int Id { get; set; }
        public int QuestionarioId { get; set; }
        public EnumDomain.TipoQuestionario TipoQuestionario { get; set; }
        public string Questionario { get; set; }
        public int PerguntaId { get; set; }
        public string Pergunta { get; set; }
        public EnumDomain.TipoResposta TipoResposta { get; set; }
        public string Nome { get; set; }
        public int? Ordem { get; set; }
        public bool Ativo { get; set; }
        public bool? RespostaCorreta { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }
}