namespace GrupoLTM.WebSmart.Domain.Models
{
    public class AvonAuthentication
    {
        /// <summary>
        /// Codigo único do revendedor (será o identificador no live)
        /// </summary>
        /// 
        private string accountNumber;

        private int? userAdminId;

        public string AccountNumber
        {
            get { return accountNumber; }
            set { accountNumber = value; }
        }

        /// <summary>
        /// Chave MD5
        /// </summary>
        public string EncryptedKey { get; set; }

        public string TokenGetInfo { get; set; }

        public string t
        {
            get { return null; }
            set { TokenGetInfo = value; }
        }

        /// <summary>
        /// Código da Campanha
        /// </summary>
        public int CatalogId { get; set; }

        /// <summary>
        /// Nome da Página Extrato ou Pontos
        /// </summary>
        public string PageName { get; set; }

        /// <summary>
        /// Alteração da Avon para passar o parâmetro 'a' via queryString.
        /// </summary>
        public string a
        {
            set
            {
                accountNumber = value;
            }
            get { return null; }
        }
        /// <summary>
        /// Alteração da Avon para passar o parâmetro 'i' via queryString.
        /// </summary>
        public int? i
        {
            set
            {
                userAdminId = value;
            }
            get { return userAdminId; }
        }

        public string redirectUrlMktplace { get; set; }

        /// <summary>
        /// Numero da Campanha Corrente.
        /// </summary>
        public int? numberCampaign { get; set; }

        /// <summary>
        /// Ano da Campanha Corrente.
        /// </summary>
        public int? yearCampaign { get; set; }

        public bool preview { get; set; }
    }
}
