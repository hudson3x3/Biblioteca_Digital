using System;
using static GrupoLTM.WebSmart.Domain.Enums.EnumDomain;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public class Banner
    {
        public int Id { get; set; }

        public string Titulo { get; set; }

        public string Nome { get; set; }

        public string Vendor { get; set; }

        public string Imagem { get; set; }

        public TipoBanner Tipo { get; set; }

        public TipoDominio Dominio { get; set; }

        public bool Ativo { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime DataInclusao { get; set; }

        public DateTime? DataAlteracao { get; set; }

        public int? UsuarioIdAlteracao { get; set; }

        public virtual UsuarioAdm UsuarioAlteracao { get; set; }

    }
}
