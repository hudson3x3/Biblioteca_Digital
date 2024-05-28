using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class LogIntegracao
    {
        public int Id { get; set; }
        public int ParticipanteId { get; set; }
        public string Evento { get; set; }
        public string Login { get; set; }
        public string JsonAvon { get; set; }
        public string JsonMP { get; set; }
        public string JsonMPUpdateAntes { get; set; }
        public string JsonMPUpdateDepois { get; set; }
        public string CP { get; set; }
        public DateTime DataInclusao { get; set; }
    }
}
