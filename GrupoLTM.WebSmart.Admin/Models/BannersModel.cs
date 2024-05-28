using System.Linq;
using GrupoLTM.WebSmart.DTO;
using System.Collections.Generic;
using GrupoLTM.WebSmart.Domain.Models;
using static GrupoLTM.WebSmart.Domain.Enums.EnumDomain;
using System.Web.Mvc;
using System;
using GrupoLTM.WebSmart.Infrastructure.Helpers;

namespace GrupoLTM.WebSmart.Admin.Models
{
    public class BannersModel
    {
        public BannersModel()
        {
        }

        public BannersModel(List<Banner> banners, string urlBlob)
        {
            Banner1 = ModelBanner(banners, TipoBanner.Banner1, urlBlob);
            Banner2 = ModelBanner(banners, TipoBanner.Banner2, urlBlob);
            Banner3 = ModelBanner(banners, TipoBanner.Banner3, urlBlob);
            Banner4 = ModelBanner(banners, TipoBanner.Banner4, urlBlob);
            Banner5 = ModelBanner(banners, TipoBanner.Banner5, urlBlob);
            Banner6 = ModelBanner(banners, TipoBanner.Banner6, urlBlob);
            Banner7 = ModelBanner(banners, TipoBanner.Banner7, urlBlob);
            Banner8 = ModelBanner(banners, TipoBanner.Banner8, urlBlob);
            Banner9 = ModelBanner(banners, TipoBanner.Banner9, urlBlob);
            Card1 = ModelBanner(banners, TipoBanner.Card1, urlBlob);
            Card2 = ModelBanner(banners, TipoBanner.Card2, urlBlob);
            Card3 = ModelBanner(banners, TipoBanner.Card3, urlBlob);
            Card4 = ModelBanner(banners, TipoBanner.Card4, urlBlob);
            Card5 = ModelBanner(banners, TipoBanner.Card5, urlBlob);
        }

        public BannerModel Banner1 { get; set; }

        public BannerModel Banner2 { get; set; }

        public BannerModel Banner3 { get; set; }

        public BannerModel Banner4 { get; set; }

        public BannerModel Banner5 { get; set; }

        public BannerModel Banner6 { get; set; }

        public BannerModel Banner7 { get; set; }

        public BannerModel Banner8 { get; set; }

        public BannerModel Banner9 { get; set; }

        public BannerModel Card1 { get; set; }

        public BannerModel Card2 { get; set; }

        public BannerModel Card3 { get; set; }

        public BannerModel Card4 { get; set; }

        public BannerModel Card5 { get; set; }

        public List<BannerModel> Banners => new List<BannerModel> {
            Banner1, Banner2, Banner3, Banner4, Banner5, Banner6, Banner7, Banner8, Banner9, Card1, Card2, Card3, Card4, Card5 };

        public List<SelectListItem> Dominios
        {
            get            
            {
                var list = new List<SelectListItem>();

                foreach (TipoDominio item in Enum.GetValues(typeof(TipoDominio)))
                {
                    var description = Helper.GetDescription(item);

                    list.Add(new SelectListItem { Text = description, Value = ((int)item).ToString() });
                }

                return list;
            }
        }

        private BannerModel ModelBanner(List<Banner> banners, TipoBanner tipoBanner, string urlBlob)
        {
            var banner = banners.First(x => x.Tipo == tipoBanner);

            var usuarioAlteracao = string.Empty;

            if (banner.UsuarioAlteracao != null)
            {
                var usuario = banner.UsuarioAlteracao.Nome;

                var nomes = usuario.Split(' ');

                var nome = nomes[0] + (nomes.Length > 1 ? " " + nomes[1] : string.Empty);

                usuarioAlteracao = $"Última alteração: {nome} - {banner.DataAlteracao?.ToString("dd/MM/yyyy HH:mm")}";
            }

            return new BannerModel(banner.Id, banner.Nome, banner.Imagem, banner.Vendor, (int)banner.Tipo, (int)banner.Dominio, urlBlob, usuarioAlteracao);
        }
    }
}