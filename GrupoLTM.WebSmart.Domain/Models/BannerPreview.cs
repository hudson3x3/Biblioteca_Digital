using static GrupoLTM.WebSmart.Domain.Enums.EnumDomain;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public class BannerPreview
    {
        public int Id { get; set; }

        public string Vendor { get; set; }

        public string Imagem { get; set; }

        public TipoBanner Tipo { get; set; }

        public TipoDominio Dominio { get; set; }
    }
}
