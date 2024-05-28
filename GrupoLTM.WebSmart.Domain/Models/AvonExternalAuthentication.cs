namespace GrupoLTM.WebSmart.Domain.Models
{
    public class AvonExternalAuthentication
    {
        /// <summary>
        /// Codigo único do revendedor (será o identificador no live)
        /// </summary>
        public string AccountNumber { get; set; }
        /// <summary>
        /// Codigo do Catálogo
        /// </summary>
        public int  CatalogId { get; set; }
        /// <summary>
        /// TokenGetInfo - da base da Avon
        /// </summary>
        public string TokenGetInfo { get; set; }
        public string pageName { get; set; }
        public string imperAcctNr { get; set; }
        public string tipoPorId { get; set; }
        public string campanha { get; set; }
        public int cmpgnNr { get; set; }
        public int cmpgnYr { get; set; }
        public string Top2ProgramName { get; set; }

        public int lastRecord { get; set; }
        public int initRecord { get; set; }
        public string order_by { get; set; }
        public string Order_by { get; set; }
        public int? period { get; set; }
        public string TokenCgt { get; set; }

        public string CurrentAccountNumber => string.IsNullOrEmpty(imperAcctNr) ? AccountNumber : imperAcctNr;
    }
}
