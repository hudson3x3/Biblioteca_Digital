using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public class ClusterRATemp
    {
        public long Id { get; set; }
        public Guid ClusterIdTemp { get; set; }
        public string RA { get; set; }
        public string Nome { get; set; }
        public DateTime DataInclusao { get; set; }
    }
}
