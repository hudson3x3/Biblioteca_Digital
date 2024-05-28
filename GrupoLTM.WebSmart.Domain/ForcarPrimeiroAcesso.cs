using System;
using System.Collections.Generic;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class ForcarPrimeiroAcesso
    {
        public ForcarPrimeiroAcesso()
        {

        }

        public int Id { get; set; }
        public string Login { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataEnvio { get; set; }
        public int ArquivoId { get; set; }
        public string Erro { get; set; }
        public long? IdPedido { get; set; }
        public int? quantity { get; set; }
    }
}
