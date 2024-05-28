using System;
using System.Diagnostics;
using PostSharp.Aspects;
using GrupoLTM.WebSmart.DTO;
using GrupoLTM.WebSmart.Services.Importacao;
using GrupoLTM.WebSmart.Services.Log;

namespace GrupoLTM.WebSmart.Services.Atributos
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Method)]
    public class LogExecutionTimeAttribute : MethodInterceptionAspect
    {
        public override void OnInvoke(MethodInterceptionArgs args)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                args.Proceed();
            }
            finally
            {
                stopwatch.Stop();

                var classe = args.Method.DeclaringType.Name;

                var nomePrograma = DefinirPrograma(classe);

                var log = new LogExecutionTimeModel
                {
                    Method = args.Method.Name,
                    Class = args.Method.DeclaringType.Name,
                    Time = stopwatch.Elapsed,
                    ProgramName = nomePrograma,
                    Date = DateTime.Now,
                };

                //TODO: Update DataDog
                //GrayLogService.LogExecutionTime(log);
            }
        }

        private string DefinirPrograma(string nomeService)
        {
            switch (nomeService)
            {
                case nameof(IndicacaoImportacaoService):
                    return "Indicação";

                case nameof(ConsecutividadeImportacaoService):
                    return "Consecutividade";

                case nameof(ApoioImportacaoService):
                    return "Apoio";

                case nameof(ClubeEstrelasImportacaoService):
                    return "Clube das estrelas";

                case nameof(MigracaoImportacaoService):
                    return "Migração";

                case nameof(EstornoImportacaoService):
                    return "Estorno";

                case nameof(NatalImportacaoService):
                    return "Natal";

                case nameof(AliancaImportacaoService):
                    return "Alianca";

                default: 
                    return "Não identificado";
            }
        }
    }
}
