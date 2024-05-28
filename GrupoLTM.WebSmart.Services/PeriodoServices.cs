using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrupoLTM.WebSmart.Services
{
    public class PeriodoServices : BaseService<Periodo>
    {
        public List<PeriodoModel> ListaPeriodo()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repPeriodo = context.CreateRepository<Periodo>();
                List<PeriodoModel> listPeriodoModel = new List<PeriodoModel>();
                foreach (var item in repPeriodo.Filter<Periodo>(x => x.Ativo == true).OrderBy(x => x.Nome).ToList())
                {
                    listPeriodoModel.Add(new PeriodoModel
                    {
                        DataInclusao = item.DataInclusao,
                        DataAlteracao = item.DataAlteracao,
                        Ativo = item.Ativo,
                        Id = item.Id,
                        Nome = item.Nome,
                        valor = item.valor
                    });
                }
                return listPeriodoModel;
            }
        }
    }
}
