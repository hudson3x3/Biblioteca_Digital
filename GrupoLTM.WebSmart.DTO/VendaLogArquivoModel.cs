using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrupoLTM.WebSmart.DTO
{
    public class VendaLogArquivoModel
    {
        public int Id { get; set; }
        public int Mes { get; set; }
        public int Ano { get; set; }
        public int ArquivoId { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataInativacao { get; set; }
        public string Nome { get; set; }
        public string NomeGerado { get; set; }


    }
}
