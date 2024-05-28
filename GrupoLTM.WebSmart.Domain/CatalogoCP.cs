using System;
using System.Collections.Generic;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class CatalogoCP
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
        public virtual Catalogo Catalogo { get; set; }

        public virtual ICollection<ParticipanteCP> ParticipanteCPs { get; set; }
    }
}
