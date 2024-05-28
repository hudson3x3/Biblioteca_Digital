using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public class AliancaHeader
    {
        public int Id { get; set; }

        public int ArquivoId { get; set; }

        public string LoyaltyProgram { get; set; }

        public string FileType { get; set; }

        public DateTime CreateDate { get; set; }

        public int FileSequence { get; set; }

        public DateTime DataInclusao { get; set; }

        public DateTime? DataAlteracao { get; set; }
    }
}
