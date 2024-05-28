using System;
using System.Collections.Generic;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public class Cluster
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public string LoginAdmin { get; set; }
        public string LoginEncerramento { get; set; }
        public DateTime? DataEncerramento { get; set; }
        public string MensagemErroValidacao { get; set; }

        public virtual ICollection<ClusterRA> ClusterRA { get; set; }
        public virtual ICollection<ClusterProduct> ClusterProducts { get; set; }
    }
}
