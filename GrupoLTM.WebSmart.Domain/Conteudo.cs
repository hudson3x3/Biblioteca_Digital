using System;
using System.Collections.Generic;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class Conteudo
    {
        public Conteudo()
        {
            this.ConteudoEstrutura = new List<ConteudoEstrutura>();
            this.ConteudoImagem = new List<ConteudoImagem>();
            this.ConteudoPerfil = new List<ConteudoPerfil>();
        }

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
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public DateTime DataInclusao { get; set; }
        public Nullable<DateTime> DataAlteracao { get; set; }
        public bool Ativo { get; set; }
        public Nullable<bool> AtivoHome { get; set; }
        public virtual Modulo Modulo { get; set; }
        public virtual ICollection<ConteudoEstrutura> ConteudoEstrutura { get; set; }
        public virtual ICollection<ConteudoImagem> ConteudoImagem { get; set; }
        public virtual ICollection<ConteudoPerfil> ConteudoPerfil { get; set; }
    }
}
