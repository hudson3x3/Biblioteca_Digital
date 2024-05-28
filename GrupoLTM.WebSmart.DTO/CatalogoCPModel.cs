using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrupoLTM.WebSmart.DTO
{
    public class CatalogoCPModel
    {
        public int Id { get; set; }
        public string CP { get; set; }
        public int ProfileId { get; set; }
        public int CatalogoId { get; set; }
        public bool Ativo { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public long? Resgates { get; set; }
        public DateTime? DataAlteracao { get; set; }

    }
}
