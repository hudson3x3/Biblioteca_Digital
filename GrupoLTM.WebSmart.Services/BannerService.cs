using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.DTO;
using GrupoLTM.WebSmart.Infrastructure.Storage.Azure.Blob;
using GrupoLTM.WebSmart.Services.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static GrupoLTM.WebSmart.Domain.Enums.EnumDomain;

namespace GrupoLTM.WebSmart.Services
{
    public class BannerService
    {
        public List<Banner> ObterBanners()
        {
            using (var context = UnitOfWorkFactory.Create())
            {
                var rep = context.CreateRepository<Banner>();

                var banners = rep.Filter<Banner>(x => x.Ativo).IncludeEntity(x => x.UsuarioAlteracao).ToList();

                return banners;
            }
        }

        public List<Banner> ObterBannersPreview()
        {
            using (var context = UnitOfWorkFactory.Create())
            {
                var rep = context.CreateRepository<BannerPreview>();

                var bannersPreview = rep.All<BannerPreview>().ToList();

                var banners = bannersPreview.Select(x => new Banner
                {
                    Vendor = x.Vendor,
                    Imagem = x.Imagem,
                    Tipo = x.Tipo,
                    Dominio = x.Dominio
                });

                return banners.ToList();
            }
        }

        public void AtualizarBanners(List<BannerModel> banners, int usuarioId)
        {
            var modificacoes = new List<BannerModificacaoModel>();

            try
            {
                using (var context = UnitOfWorkFactory.Create())
                {
                    var repositorio = context.CreateRepository<Banner>();

                    var ids = banners.Select(x => x.Id);

                    var bannersDb = repositorio.Filter<Banner>(x => ids.Contains(x.Id)).ToList();

                    foreach (var bannerDb in bannersDb)
                    {
                        var banner = banners.First(x => x.Id == bannerDb.Id);

                        var dominio = (TipoDominio)banner.Dominio;

                        if (bannerDb.Vendor != banner.Vendor || bannerDb.Dominio != dominio || banner.Imagem != null)
                        {
                            if (banner.Imagem != null)
                            {
                                var novoNome = UploadBanner(banner);

                                modificacoes.Add(new BannerModificacaoModel(bannerDb.Id, "Imagem", bannerDb.Imagem, novoNome, usuarioId));

                                bannerDb.Imagem = novoNome;
                            }

                            if (bannerDb.Vendor != banner.Vendor)
                            {
                                modificacoes.Add(new BannerModificacaoModel(bannerDb.Id, "Vendor", bannerDb.Vendor, banner.Vendor, usuarioId));
                                bannerDb.Vendor = banner.Vendor;
                            }

                            if (bannerDb.Dominio != dominio)
                            {
                                modificacoes.Add(new BannerModificacaoModel(bannerDb.Id, "Dominio", bannerDb.Dominio, banner.Dominio, usuarioId));
                                bannerDb.Dominio = dominio;
                            }

                            bannerDb.DataInicio = DateTime.Now;
                            bannerDb.DataAlteracao = DateTime.Now;
                            bannerDb.UsuarioIdAlteracao = usuarioId;
                        }
                    }

                    repositorio.UpdateRange(bannersDb);
                    repositorio.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            var log = new LogBannerModel
            {
                UsuarioId = usuarioId,
                Modificacoes = modificacoes,
                Method = "AtualizarBanners",
                Class = "BannerService",
                Message = "Atualização de banners",
            };

            //TODO: Update DataDog
            //GrayLogService.Log(log);
        }

        public void CadastrarBannersPreview(List<BannerModel> bannersPreview, int usuarioId)
        {
            var modificacoes = new List<BannerModificacaoModel>();

            try
            {
                using (var context = UnitOfWorkFactory.Create())
                {
                    var repositorioBanner = context.CreateRepository<Banner>();
                    var repositorioPreview = context.CreateRepository<BannerPreview>();

                    var tipos = bannersPreview.Select(x => x.Tipo);

                    var bannersDb = repositorioPreview.Filter<BannerPreview>(x => tipos.Contains((int)x.Tipo)).ToList();

                    foreach (var bannerPreviewDb in bannersDb)
                    {
                        var bannerPreview = bannersPreview.First(x => x.Tipo == (int)bannerPreviewDb.Tipo);

                        if (bannerPreview.Imagem != null)
                        {
                            var novoNome = UploadBanner(bannerPreview, true);

                            modificacoes.Add(new BannerModificacaoModel(bannerPreview.Id, "Imagem", bannerPreviewDb.Imagem, novoNome, usuarioId));

                            bannerPreviewDb.Imagem = novoNome;
                        }
                        else
                        {
                            var bannerPrd = repositorioBanner.Find<Banner>(x => x.Tipo == bannerPreviewDb.Tipo);
                            bannerPreviewDb.Imagem = bannerPrd.Imagem;
                        }

                        if (bannerPreviewDb.Vendor != bannerPreview.Vendor)
                        {
                            modificacoes.Add(new BannerModificacaoModel(bannerPreview.Id, "Vendor", bannerPreviewDb.Vendor, bannerPreview.Vendor, usuarioId));
                            bannerPreviewDb.Vendor = bannerPreview.Vendor;
                        }

                        if (bannerPreviewDb.Dominio != (TipoDominio)bannerPreview.Dominio)
                        {
                            modificacoes.Add(new BannerModificacaoModel(bannerPreview.Id, "Dominio", bannerPreviewDb.Dominio, bannerPreview.Dominio, usuarioId));
                            bannerPreviewDb.Dominio = (TipoDominio)bannerPreview.Dominio;
                        }
                    }

                    repositorioPreview.UpdateRange(bannersDb);

                    var bannersAdd = bannersPreview.Where(x => bannersDb.All(y => x.Tipo != (int)y.Tipo));

                    if (bannersAdd.Any())
                    {
                        foreach (var banner in bannersAdd)
                        {
                            var bannerPreview = new BannerPreview
                            {
                                Imagem = banner.NomeImagem,
                                Tipo = (TipoBanner)banner.Tipo,
                                Dominio = (TipoDominio)banner.Dominio,
                                Vendor = banner.Vendor
                            };

                            if (banner.Imagem != null)
                            {
                                bannerPreview.Imagem = UploadBanner(banner, true);
                                modificacoes.Add(new BannerModificacaoModel(bannerPreview.Id, "Imagem", bannerPreview.Imagem, bannerPreview.Imagem, usuarioId));
                            }

                            repositorioPreview.Create(bannerPreview);
                        }
                    }

                    repositorioPreview.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            var log = new LogBannerModel
            {
                UsuarioId = usuarioId,
                Modificacoes = modificacoes,
                Method = "CadastrarBannersPreview",
                Class = "BannerService",
                Message = "Atualização de preview dos banners",
            };

            //TODO: Update DataDog
            //GrayLogService.Log(log);
        }

        private string UploadBanner(BannerModel banner, bool preview = false)
        {
            var storage = new Storage();

            var extension = Path.GetExtension(banner.Imagem.FileName);

            var nomeArquivo = $"{banner.Nome}_{DateTime.Now:dd-MM-yyyy_HH-mm-ss}{extension}";

            if (preview)
                nomeArquivo = "Preview_" + nomeArquivo;

            storage.UploadImageBanner(banner.Imagem.InputStream, nomeArquivo);

            return nomeArquivo;
        }
    }
}
