using System;
using System.Collections.Generic;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class Catalogo
    {
        public Catalogo()
        {
            this.ParticipanteCatalogos = new List<ParticipanteCatalogo>();
            this.Pontuacoes = new List<Pontuacao>();
            this.CatalogoCPs = new List<CatalogoCP>();
        }

        public int Id { get; set; }

        public string Nome { get; set; }

        //public int IdCampanha { get; set; }

        //public long IdOrigem { get; set; }

        //public int IdEmpresa { get; set; }

        //public long MktPlaceCatalogoId { get; set; } ///substituir pelo Codigo

        public long Codigo { get; set; }

        //public bool PrimeiroAcesso { get; set; }

        public DateTime DataInclusao { get; set; } //substituir por DataInclusao

        public DateTime DataAlteracao { get; set; } //substituir por DataAlteracao

        public bool Ativo { get; set; }

        //public string AppIdMktPlace { get; set; }
        public string Autor { get; set; }

        public int Qtd { get; set; }

        //public short? RepProfileType  { get; set; }       

        public virtual ICollection<ParticipanteCatalogo> ParticipanteCatalogos { get; set; }
        public virtual ICollection<Pontuacao> Pontuacoes { get; set; }
        public virtual ICollection<CatalogoCP> CatalogoCPs { get; set; }
        public virtual ICollection<ResgatesAvon> ResgatesAvon { get; set; }

        public bool Expired()
        {
            return (DateTime.Now < this.DataInclusao || DateTime.Now > this.DataAlteracao) ? true : false;
        }
        //public decimal ConversionRate { get; set; }
        //public long? MktPlaceSupplierId { get; set; }

    }
}
