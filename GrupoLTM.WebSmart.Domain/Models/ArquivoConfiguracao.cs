using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class ArquivoConfiguracao
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Caminho { get; set; }
        public string Extensao { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public int TipoArquivoId { get; set; }
        public int? SequencialArquivo { get; set; }

        public virtual TipoArquivo TipoArquivo { get; set; }
    }
}
