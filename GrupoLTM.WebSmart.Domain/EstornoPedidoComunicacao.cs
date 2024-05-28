using GrupoLTM.WebSmart.Domain.Enums;
using GrupoLTM.WebSmart.Infrastructure.Helpers;
using static GrupoLTM.WebSmart.Domain.Enums.EnumDomain;

namespace GrupoLTM.Avon.MMA.Estornos.SmsEstorno.Models
{
    public class EstornoPedidoComunicacao
    {
        public int EstornoPedidoId { get; set; }
        
        public long PedidoId { get; set; }

        public long PedidoPaiId { get; set; }

        public string NomeProduto { get; set; }

        public string DDD { get; set; }

        public string Celular { get; set; }

        public string Email { get; set; }

        public string ParticipanteNome { get; set; }

        public EstornoMotivo Motivo { get; set; }

        public EnvioSmsStatus EnvioSmsStatus { get; set; }

        public EnvioEmailStatus EnvioEmailStatus { get; set; }

        public bool? SmsErro { get; set; }

        public bool? EmailErro { get; set; }

        public string NumeroCompleto => "55" + DDD.OnlyNumbers() + Celular.OnlyNumbers();
    }
}
