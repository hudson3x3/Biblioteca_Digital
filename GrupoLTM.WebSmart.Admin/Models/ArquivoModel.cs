using GrupoLTM.WebSmart.Domain.Enums;
using System;

namespace GrupoLTM.WebSmart.Admin.Models
{
    public class ArquivoModel
    {
        public int Id { get; set; }
        public EnumDomain.TipoArquivo eTipoArquivo { get; set; }
        public string Nome { get; set; }
        public string NomeGerado { get; set; }
        public string StatusArquivo { get; set; }
        public DateTime DataInclusao { get; set; }
    }
}