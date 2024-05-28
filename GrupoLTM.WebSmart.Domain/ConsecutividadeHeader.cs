using System;
using System.Collections.Generic;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class ConsecutividadeHeader
    {
        public int Id { get; set; }
        public string ProgramDescriptionHeader { get; set; }
        public string RepresentativeNumberHeader { get; set; }
        public long Points { get; set; }
        public string StatusPoints { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public int StatusId { get; set; }
        public int LoteId { get; set; }
        public virtual StatusDetail StatusDetail { get; set; }
        public virtual Lote Lote { get; set; }
        public virtual ICollection<ConsecutividadeDetail> ConsecutividadeDetails { get; set; }
    }
}
