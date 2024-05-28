using GrupoLTM.WebSmart.DTO;
using System;
using System.Threading.Tasks;

namespace GrupoLTM.WebSmart.Services.Interface
{
    public interface IIntegradorService
    {
        Task<string> AutenticarAsync(string username, string password);

        Task<double> ObterSaldoSimplesAsync();

        Task<SaldoModel> ObterSaldoAsync();

        Task<ExtratoModel> ObterExtratoAsync();
    }
}
