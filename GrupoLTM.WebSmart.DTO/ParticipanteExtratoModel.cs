using System.Collections.Generic;

namespace GrupoLTM.WebSmart.DTO
{
    public class ParticipanteExtratoModel
    {
        public string Login { get; set; }

        public int MktPlaceParticipantId { get; set; }

        public int MktPlaceCatalogoId { get; set; }

        public List<ParticipanteCatalogoModel> Catalogos { get; set; }

    }
}
