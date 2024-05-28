using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrupoLTM.WebSmart.DTO
{
    public class CatalogoModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        //public long MktPlaceCatalogoId { get; set; }
        public long Codigo { get; set; }
        //public bool PrimeiroAcesso { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime DataAlteracao { get; set; }
        //public int IdCampanha { get; set; }
        //public long IdOrigem { get; set; }
        //public int IdEmpresa { get; set; }
        //public string AppIdMktPlace { get; set; }
        public string Autor { get; set; }
        public int Qtd { get; set; }
        public int PerfilId { get; set; }
        //public decimal ConversionRate { get; set; }
        //public short? RepProfileType  { get; set; }
        //public long? MktPlaceSupplierId { get; set; }
    }
}
