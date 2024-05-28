using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrupoLTM.WebSmart.DTO
{
    public class ParticipanteQuestionarioModel
    {
        public int Id { get; set; }
        public int ParticipanteId { get; set; }
        public int QuestionarioId { get; set; }
        public int PerguntaId { get; set; }
        public int RespostaId { get; set; }
        public int TipoDePergunta { get; set; }
        public string RespostaTexto { get; set; }
        public bool respostaCorreta { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime DataAlteracao { get; set; }
    }
}
