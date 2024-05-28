using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrupoLTM.WebSmart.Domain.Enums;
using GrupoLTM.WebSmart.Infrastructure.Cache;
using GrupoLTM.WebSmart.Infrastructure.Configuration;
using GrupoLTM.WebSmart.Infrastructure.Helpers;

namespace GrupoLTM.WebSmart.Services
{
    public class ConteudoService : BaseService<Conteudo>
    {
        [Cache]
        public List<WebSmart.DTO.ConteudoModel> ListarConteudo(int perfilId, int estruturaId, EnumDomain.ModuloFixo modulo)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repConteudo = context.CreateRepository<Conteudo>();
                List<WebSmart.DTO.ConteudoModel> listConteudoModel = new List<WebSmart.DTO.ConteudoModel>();
              
                List<WebSmart.Domain.Models.Conteudo> listConteudo = new List<WebSmart.Domain.Models.Conteudo>();
                listConteudo = repConteudo.Filter<Conteudo>(x => x.Ativo == true
                    && (EnumDomain.ModuloFixo)x.ModuloId == modulo
                    && x.ConteudoPerfil.Any(cp => cp.PerfilId == perfilId && cp.Ativo)
                    && x.ConteudoEstrutura.Any(ce => ce.EstruturaId == estruturaId && ce.Ativo)
                    && (x.DataInicio == null || DateTime.Now > x.DataInicio && x.DataFim > DateTime.Now)
                    )
                    .OrderBy(x => x.Nome)
                    .ToList();

                foreach (var item in listConteudo)
                {
                    listConteudoModel.Add(new ConteudoModel
                    {
                        Ativo = item.Ativo,
                        DataAlteracao = item.DataAlteracao,
                        DataInclusao = item.DataInclusao,
                        DataInicio = item.DataInicio,
                        DataFim = item.DataFim,
                        Nome = item.Nome,
                        Id = item.Id,
                        ModuloId = item.ModuloId,
                        Titulo = item.Titulo,
                        Descricao = item.Descricao,
                        Pretexto = item.Pretexto,
                        Texto = item.Texto,
                        LinkAcesso = item.LinkAcesso,
                        Alt = item.Alt,
                        ImgP = item.ImgP,
                        ImgM = item.ImgM,
                        imgG = item.imgG,
                        LinkDownload = item.LinkDownload,
                        AtivoHome = item.AtivoHome
                    });
                }

                return listConteudoModel;

            }
        }

        public List<WebSmart.DTO.ConteudoModel> ListarConteudo(int perfilId, int estruturaId, EnumDomain.ModuloFixo modulo, int pagina, int quantidadeRegistros, out int total)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repConteudo = context.CreateRepository<Conteudo>();
                List<WebSmart.DTO.ConteudoModel> listConteudoModel = new List<WebSmart.DTO.ConteudoModel>();
                ConteudoRepository repository = context.ConteudoRepository();

                List<WebSmart.Domain.Models.Conteudo> listConteudo = new List<WebSmart.Domain.Models.Conteudo>();
                listConteudo = repository.Filter(x => x.Ativo == true
                    && (EnumDomain.ModuloFixo)x.ModuloId == modulo
                    && x.ConteudoPerfil.Any(cp => cp.PerfilId == perfilId && cp.Ativo)
                    && x.ConteudoEstrutura.Any(ce => ce.EstruturaId == estruturaId && ce.Ativo)
                    && (x.DataInicio == null || DateTime.Now > x.DataInicio && x.DataFim > DateTime.Now)
                    , out total, pagina, quantidadeRegistros)
                    .OrderBy(x => x.Nome)
                    .ToList();

                foreach (var item in listConteudo)
                {
                    listConteudoModel.Add(new ConteudoModel
                    {
                        Ativo = item.Ativo,
                        DataAlteracao = item.DataAlteracao,
                        DataInclusao = item.DataInclusao,
                        DataInicio = item.DataInicio,
                        DataFim = item.DataFim,
                        Nome = item.Nome,
                        Id = item.Id,
                        ModuloId = item.ModuloId,
                        Titulo = item.Titulo,
                        Descricao = item.Descricao,
                        Pretexto = item.Pretexto,
                        Texto = item.Texto,
                        LinkAcesso = item.LinkAcesso,
                        Alt = item.Alt,
                        ImgP = item.ImgP,
                        ImgM = item.ImgM,
                        imgG = item.imgG,
                        LinkDownload = item.LinkDownload,
                        AtivoHome = item.AtivoHome
                    });
                }

                return listConteudoModel;

            }
        }

        public WebSmart.DTO.ConteudoModel ListarConteudo(int conteudoId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repConteudo = context.CreateRepository<Conteudo>();
                WebSmart.DTO.ConteudoModel conteudoModel = new WebSmart.DTO.ConteudoModel();

                WebSmart.Domain.Models.Conteudo conteudo = new WebSmart.Domain.Models.Conteudo();
                conteudo = repConteudo.Filter<Conteudo>(x => x.Ativo == true
                    && x.Id == conteudoId
                    && (x.DataInicio == null || DateTime.Now > x.DataInicio && x.DataFim > DateTime.Now)
                    ).FirstOrDefault();
                
                if (conteudo != null)
                {
                    conteudoModel.Ativo = conteudo.Ativo;
                    conteudoModel.DataAlteracao = conteudo.DataAlteracao;
                    conteudoModel.DataInclusao = conteudo.DataInclusao;
                    conteudoModel.DataInicio = conteudo.DataInicio;
                    conteudoModel.DataFim = conteudo.DataFim;
                    conteudoModel.Nome = conteudo.Nome;
                    conteudoModel.ModuloId = conteudo.ModuloId;
                    conteudoModel.Titulo = conteudo.Titulo;
                    conteudoModel.Id = conteudo.Id;
                    conteudoModel.Descricao = conteudo.Descricao;
                    conteudoModel.Pretexto = conteudo.Pretexto;
                    conteudoModel.Texto = conteudo.Texto;
                    conteudoModel.LinkAcesso = conteudo.LinkAcesso;
                    conteudoModel.Alt = conteudo.Alt;
                    conteudoModel.ImgP = conteudo.ImgP;
                    conteudoModel.ImgM = conteudo.ImgM;
                    conteudoModel.imgG = conteudo.imgG;
                    conteudoModel.LinkDownload = conteudo.LinkDownload;
                    conteudoModel.AtivoHome = conteudo.AtivoHome;
                }

                return conteudoModel;

            }
        }

        public List<WebSmart.DTO.ConteudoModel> ListarConteudoDestaque(int perfilId, int estruturaId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repConteudo = context.CreateRepository<Conteudo>();
                List<WebSmart.DTO.ConteudoModel> listConteudoModel = new List<WebSmart.DTO.ConteudoModel>();

                List<WebSmart.Domain.Models.Conteudo> listConteudo = new List<WebSmart.Domain.Models.Conteudo>();
                listConteudo = repConteudo.Filter<Conteudo>(x => x.Ativo == true
                    && x.ConteudoPerfil.Any(cp => cp.PerfilId == perfilId && cp.Ativo)
                    && x.ConteudoEstrutura.Any(ce => ce.EstruturaId == estruturaId && ce.Ativo)
                    && (x.DataInicio == null || DateTime.Now > x.DataInicio && x.DataFim > DateTime.Now)
                    && x.AtivoHome == true
                    )
                    .OrderByDescending(x => x.DataInclusao)
                    .ToList();

                foreach (var item in listConteudo)
                {
                    listConteudoModel.Add(new ConteudoModel
                    {
                        Ativo = item.Ativo,
                        DataAlteracao = item.DataAlteracao,
                        DataInclusao = item.DataInclusao,
                        DataInicio = item.DataInicio,
                        DataFim = item.DataFim,
                        Nome = item.Nome,
                        Id = item.Id,
                        ModuloId = item.ModuloId,
                        Titulo = item.Titulo,
                        Descricao = item.Descricao,
                        Pretexto = item.Pretexto,
                        Texto = item.Texto,
                        LinkAcesso = item.LinkAcesso,
                        Alt = item.Alt,
                        ImgP = item.ImgP,
                        ImgM = item.ImgM,
                        imgG = item.imgG,
                        LinkDownload = item.LinkDownload,
                        AtivoHome = item.AtivoHome,
                        TituloURLFriendly = StringHelper.URLFriendly(item.Titulo)
                    });
                }

                return listConteudoModel;

            }
        }

        public List<WebSmart.DTO.ConteudoModel> ListarNoticias(int perfilId, int estruturaId, EnumDomain.ModuloFixo modulo, int pagina, int quantidadeRegistros, out int total)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                ConteudoRepository repository = context.ConteudoRepository();
                List<WebSmart.DTO.ConteudoModel> listConteudoModel = new List<WebSmart.DTO.ConteudoModel>();

                List<WebSmart.Domain.Models.Conteudo> listConteudo = new List<WebSmart.Domain.Models.Conteudo>();
                listConteudo = repository.Filter(x => x.Ativo == true
                    && (EnumDomain.ModuloFixo)x.ModuloId == modulo
                    && x.ConteudoPerfil.Any(cp => cp.PerfilId == perfilId && cp.Ativo)
                    && x.ConteudoEstrutura.Any(ce => ce.EstruturaId == estruturaId && ce.Ativo)
                    && (x.DataInicio == null || DateTime.Now > x.DataInicio && x.DataFim > DateTime.Now)
                    , out total, pagina, quantidadeRegistros).ToList();

                foreach (var item in listConteudo)
                {
                    listConteudoModel.Add(new ConteudoModel
                    {
                        Ativo = item.Ativo,
                        DataAlteracao = item.DataAlteracao,
                        DataInclusao = item.DataInclusao,
                        DataInicio = item.DataInicio,
                        DataFim = item.DataFim,
                        Nome = item.Nome,
                        Id = item.Id,
                        ModuloId = item.ModuloId,
                        Titulo = item.Titulo,
                        Descricao = item.Descricao,
                        Pretexto = item.Pretexto,
                        Texto = item.Texto,
                        LinkAcesso = item.LinkAcesso,
                        Alt = item.Alt,
                        ImgP = item.ImgP,
                        ImgM = item.ImgM,
                        imgG = item.imgG,
                        LinkDownload = item.LinkDownload,
                        AtivoHome = item.AtivoHome
                    });
                }

                return listConteudoModel;

            }
        }

        [Cache]
        public List<WebSmart.DTO.ConteudoModel> ObterBackgroundDeslogado()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repConteudo = context.CreateRepository<Conteudo>();
                List<WebSmart.DTO.ConteudoModel> listConteudoModel = new List<WebSmart.DTO.ConteudoModel>();

                List<WebSmart.Domain.Models.Conteudo> listConteudo = new List<WebSmart.Domain.Models.Conteudo>();
                listConteudo = repConteudo.Filter<Conteudo>(x => x.Ativo == true
                    && (EnumDomain.ModuloFixo)x.ModuloId == EnumDomain.ModuloFixo.Background
                    && x.ConteudoPerfil.Where(y=>y.Ativo).Count() == 0
                    && x.ConteudoEstrutura.Where(ce=>ce.Ativo).Count() == 0
                    && (DateTime.Now > x.DataInicio && x.DataFim > DateTime.Now)).OrderBy(x => x.Nome).ToList();

                foreach (var item in listConteudo)
                {
                    listConteudoModel.Add(new ConteudoModel
                    {
                        Ativo = item.Ativo,
                        DataAlteracao = item.DataAlteracao,
                        DataInclusao = item.DataInclusao,
                        DataInicio = item.DataInicio,
                        DataFim = item.DataFim,
                        Nome = item.Nome,
                        Id = item.Id,
                        ModuloId = item.ModuloId,
                        Titulo = item.Titulo,
                        Descricao = item.Descricao,
                        Pretexto = item.Pretexto,
                        Texto = item.Texto,
                        LinkAcesso = item.LinkAcesso,
                        Alt = item.Alt,
                        ImgP = item.ImgP,
                        ImgM = item.ImgM,
                        imgG = item.imgG,
                        LinkDownload = item.LinkDownload,
                        AtivoHome = item.AtivoHome
                    });
                }

                return listConteudoModel;

            }
        }

        public List<WebSmart.DTO.EstadoModel> ObterEstados()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repConteudo = context.CreateRepository<Estado>();
                List<WebSmart.DTO.EstadoModel> _listEstadoModel = new List<WebSmart.DTO.EstadoModel>();

                List<WebSmart.Domain.Models.Estado> listConteudo = new List<WebSmart.Domain.Models.Estado>();
                listConteudo = repConteudo.All<Estado>().OrderBy(x => x.Nome).ToList();

                foreach (var item in listConteudo)
                {
                    _listEstadoModel.Add(new EstadoModel
                    {
                       Id = item.EstadoId,
                       Nome = item.Nome,
                       Regiao = item.Regiao,
                       Capital = item.Capital,
                       DataInclusao = item.DataInclusao,
                       DataAlteracao = item.DataAlteracao                      
                    });
                }

                return _listEstadoModel;

            }
        }

    }
}
