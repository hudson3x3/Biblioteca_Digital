using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using GrupoLTM.WebSmart.Domain.Enums;

namespace GrupoLTM.WebSmart.Admin.Models
{
    public class QuestionarioModel
    {
        public int Id { get; set; }
        public int TipoQuestionarioId { get; set; }
        public string TipoQuestionario { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public int[] PerfilId { get; set; }
        public int[] EstruturaId { get; set; }
        public ArrayList ArrPerfilId { get; set; }
        public ArrayList ArrEstruturaId { get; set; }
        
    }
}