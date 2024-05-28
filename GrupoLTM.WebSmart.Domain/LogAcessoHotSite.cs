using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class LogAcessoHotSite
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public int CatalogId { get; set; }
        public string PageName { get; set; }
        public int IdAdmin { get; set; }
        public int numberCampaign { get; set; }
        public int yearCampaign { get; set; }
        public string IP { get; set; }
        public DateTime DataInclusao { get; set; }
        
    }
}
