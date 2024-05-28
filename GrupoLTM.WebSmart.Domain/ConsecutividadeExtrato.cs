using System;
using System.Collections.Generic;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class ConsecutividadeExtrato
    {
        public ConsecutividadeExtrato()
        {
        }
        public ConsecutividadeExtratoDB consecutividadeExtratoDB { get; set; }
        public ConsecutividadeExtratoExterno consecutividadeExtratoExterno { get; set; }
    }

    public class ConsecutividadeExtratoExterno
    {
        public string programa { get; set; }
        public long pontos_consecutividade { get; set; }
        public long pontos_pendentes { get; set; }
        public long pontos_cancelados { get; set; }
        public long pontos_liberados { get; set; }
        public List<ConsecutividadeExtratoCampanhas> campanhas { get; set; }
    }
    public class ConsecutividadeExtratoCampanhas
    {
        public int cp { get; set; }
        public int ano { get; set; }
        public string pedido_enviado { get; set; }
        public string pedido_pago { get; set; }
        public string atingiu_objetivo { get; set; }
        public string devolucao { get; set; }
    }

    public class ConsecutividadeExtratoDB
    {
        public int Id { get; set; }
        public string ProgramDescriptionHeader { get; set; }
        public string RepresentativeNumberHeader { get; set; }
        public long Points { get; set; }
        public string StatusPoints { get; set; }
        public int ConsecutividadeDetailId { get; set; }
        public int CampaignYearDetail { get; set; }
        public int CampaignNumberDetail { get; set; }
        public string OrderSentCodeDetail { get; set; }
        public string OrderPaymentStatusDetail { get; set; }
        public string OrderObjectiveStatusDetail { get; set; }
        public string OrderDevolutionStatusDetail { get; set; }

    }
}
