namespace GrupoLTM.WebSmart.DTO
{
    public class ResultRequestApi
    {
        public string AccountNumber { get; set; }
        public string Api { get; set; }
        public object Content { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
        public int TempoResposta { get; set; }
        public bool Success { get; set; }
    }
}
