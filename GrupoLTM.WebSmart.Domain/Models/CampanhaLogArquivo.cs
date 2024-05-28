using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class CampanhaLogArquivo
    {
        public int Id { get; set; }
        public int CampanhaPeriodoId { get; set; }
        public int CampanhaId { get; set; }
        public int ArquivoId { get; set; }
        public DateTime DataInclusao { get; set; }
        public Nullable<DateTime> DataInativacao { get; set; }
        public bool Ativo { get; set; }
        public virtual Arquivo Arquivo { get; set; }
        public virtual Campanha Campanha { get; set; }
        public virtual CampanhaPeriodo CampanhaPeriodo { get; set; }
    }
}
