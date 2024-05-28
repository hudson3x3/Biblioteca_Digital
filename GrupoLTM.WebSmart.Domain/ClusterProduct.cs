namespace GrupoLTM.WebSmart.Domain.Models
{
    public class ClusterProduct
    {
        public int Id { get; set; }
        public int ClusterId { get; set; }
        public string ProductSku { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }

        public virtual Cluster Cluster { get; set; } 
    }
}
