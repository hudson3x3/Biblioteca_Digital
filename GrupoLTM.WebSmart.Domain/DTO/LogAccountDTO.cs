using System;

namespace GrupoLTM.WebSmart.Domain.DTO
{
    public class LogAccountDTO
    {
        public int Id { get; set; }
        public string LoginAdmin { get; set; }
        public string AccountNumber { get; set; }
        public string NameAccountNumber { get; set; }
        public string IP { get; set; }
        public DateTime DataInclusao { get; set; }
        public int CatalogoId { get; set; }
        public string Catalogo { get; set; }
    }
}
