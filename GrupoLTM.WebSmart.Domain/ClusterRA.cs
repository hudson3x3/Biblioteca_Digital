namespace GrupoLTM.WebSmart.Domain.Models
{
    public class ClusterRA
    {
        public long Id { get; set; }
        public int ClusterId { get; set; }
        public string RA { get; set; }
        public string Nome { get; set; }

        public virtual Cluster Cluster { get; set; }
    }
}
