using System;
using System.Collections.Generic;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class HomeMMAExtrato
    {
        public HomeMMAExtrato()
        {
        }
        public HomeMMAExtratoExterno homeMMAExtratoExterno { get; set; }
    }

    public class HomeMMAExtratoExterno
    {
        public DateTime atualizacao { get; set; }
        public decimal? saldo { get; set; }
        public List<HomeMMAExtratoItens> itens { get; set; }
    }
    public class HomeMMAExtratoItens
    {
        public decimal? PontosDisponíveis { get; set; }
        public int PontosAvencer { get; set; }
        public int PontosAreceber { get; set; }
        public decimal? PontosVencidos { get; set; }
        public decimal? PontosResgatados { get; set; }
    }
}
