using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.DTO;
using System;
using System.Linq;

namespace GrupoLTM.WebSmart.Services
{
    public class ResgatesAvonService
    {
        public ResgatesAvonService() { }

        public Domain.Models.ResgatesAvon Incluir(RedemptionModel model)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repResgateAvon = context.CreateRepository<Domain.Models.ResgatesAvon>();

                // Verifica se já existe arquivo para essa data
                var resgateAvonOld = repResgateAvon.Filter<Domain.Models.ResgatesAvon>(x =>
                    x.CatalogoId == model.CatalogoId &&
                    x.DataProcessamento.Day == model.DataProcessamento.Day &&
                    x.DataProcessamento.Month == model.DataProcessamento.Month &&
                    x.DataProcessamento.Year == model.DataProcessamento.Year &&
                    x.Ativo).OrderByDescending(x => x.Id).FirstOrDefault();

                if(resgateAvonOld != null)
                {
                    // Atualiza URLs

                    if (string.IsNullOrEmpty(model.UrlArquivoResgatesGerais) && !string.IsNullOrEmpty(resgateAvonOld.UrlArquivoResgatesGerais))
                        model.UrlArquivoResgatesGerais = resgateAvonOld.UrlArquivoResgatesGerais;

                    if (string.IsNullOrEmpty(model.UrlArquivoResgatesAvon) && !string.IsNullOrEmpty(resgateAvonOld.UrlArquivoResgatesAvon))
                        model.UrlArquivoResgatesAvon = resgateAvonOld.UrlArquivoResgatesAvon;

                    // Inativa 
                    resgateAvonOld.Ativo = false;

                    // Atualiza registro
                    repResgateAvon.Update<Domain.Models.ResgatesAvon>(resgateAvonOld);
                }

                Domain.Models.ResgatesAvon resgateAvon = new Domain.Models.ResgatesAvon();

                resgateAvon.CatalogoId = model.CatalogoId;
                resgateAvon.UrlArquivoResgatesGerais = model.UrlArquivoResgatesGerais;
                resgateAvon.UrlArquivoResgatesAvon = model.UrlArquivoResgatesAvon;
                resgateAvon.QtdResgatesGerais = model.QtdResgatesGerais;
                resgateAvon.QtdResgatesAvon = model.QtdResgatesAvon;
                resgateAvon.DataProcessamento = model.DataProcessamento;
                resgateAvon.DataInclusao = DateTime.Now;
                resgateAvon.DataAlteracao = DateTime.Now;
                resgateAvon.Ativo = model.Ativo;

                repResgateAvon.Create<Domain.Models.ResgatesAvon>(resgateAvon);
                repResgateAvon.SaveChanges();

                return resgateAvon;
            }
        }
    }
}