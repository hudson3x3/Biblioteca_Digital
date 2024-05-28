using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrupoLTM.WebSmart.Services
{
    public class LogAprovacaoArquivoService
    {
        private readonly LogAprovacaoArquivoRepository _LogAprovacaoArquivoRepository;

        public LogAprovacaoArquivoService()
        {
            _LogAprovacaoArquivoRepository = new LogAprovacaoArquivoRepository();
        }
        public void GravarLogAprovacaoArquivo(LogAprovacaoArquivo logAprovacaoArquivo)
        {
            _LogAprovacaoArquivoRepository.Create(logAprovacaoArquivo);
            _LogAprovacaoArquivoRepository.Dispose();
        }
    }
}
