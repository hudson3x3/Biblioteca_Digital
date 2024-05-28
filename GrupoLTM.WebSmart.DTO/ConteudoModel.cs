using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace GrupoLTM.WebSmart.DTO
{
    public class ConteudoModel
    {
        public int Id { get; set; }
        public int ModuloId { get; set; }
        public string Nome { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string Pretexto { get; set; }
        public string Texto { get; set; }
        public string LinkAcesso { get; set; }
        public string Alt { get; set; }
        public string ImgP { get; set; }
        public string ImgM { get; set; }
        public string imgG { get; set; }
        public string LinkDownload { get; set; }
        public System.DateTime DataInicio { get; set; }
        public System.DateTime DataFim { get; set; }
        public System.DateTime DataInclusao { get; set; }
        public System.DateTime? DataAlteracao { get; set; }
        public bool Ativo { get; set; }
        public bool? AtivoHome { get; set; }
        public string TituloURLFriendly { get; set; }
    }
}
