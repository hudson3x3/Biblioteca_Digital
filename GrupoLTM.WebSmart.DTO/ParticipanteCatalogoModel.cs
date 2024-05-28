using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrupoLTM.WebSmart.DTO
{
    public partial class ParticipanteCatalogoModel
    {
        public ParticipanteCatalogoModel()
        {
        }

        public int Id { get; set; }

        public int ParticipanteId { get; set; }
        public int CatalogoId { get; set; }
        public long CodigoMktPlace { get; set; }

        public Nullable<bool> Ativo { get; set; }
        public DateTime DataInclusao { get; set; }
        public Nullable<DateTime> DataAlteracao { get; set; }

        public CatalogoModel Catalogo { get; set; }
    }
}
