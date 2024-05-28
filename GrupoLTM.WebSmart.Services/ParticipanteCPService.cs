using GrupoLTM.WebSmart.Domain.Repositories;

namespace GrupoLTM.WebSmart.Services
{
    public class ParticipanteCPService
    {
        public bool CheckParticipanteCP(int participanteId, string avonCP)
        {
            var _participanteCPRepository = new ParticipanteCPRepository();
            var check = _participanteCPRepository.CheckParticipanteCP(participanteId, avonCP);
            _participanteCPRepository.Dispose();
            return check;
        }

       
    }
}
