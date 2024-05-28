namespace GrupoLTM.WebSmart.DTO
{
    public class RequestApi
    {
        public string AccountNumber { get; set; }
        public string Api { get; set; }
        public string Type { get; set; }
        public int ParticipantId { get; set; }
        public int CatalogoId { get; set; }
        public int MktPlaceCatalogoId { get; set; }
        public int MktPlaceParticipantId { get; set; }
    }
}
