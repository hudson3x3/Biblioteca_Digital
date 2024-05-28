using GrupoLTM.WebSmart.DTO;
using GrupoLTM.WebSmart.Infrastructure.Exceptions;
using GrupoLTM.WebSmart.Infrastructure.Helpers;
using System;
using System.Linq;

namespace GrupoLTM.WebSmart.Services.Log
{
    public class LogProcessamento
    {
        public static void Log(string mensagem, string nomeArquivo, string classe, string metodo)
        {
            Console.WriteLine(DateTime.Now.ToString() + " - " + mensagem);

            var log = new LogProcessamentoModel
            {
                Date = DateTime.Now,
                Punch = nomeArquivo,
                Class = classe,
                Message = mensagem,
                Method = metodo
            };

            //TODO: Update DataDog
            //GrayLogService.Log(log);
        }

        public static void LogRequest(string mensagem, string nomeArquivo, string requestType, string url, object body, string token, string extraInfo = null)
        {
            Console.WriteLine(DateTime.Now.ToString() + " - " + mensagem);

            var logServiceBus = new LogProcessamentoModel
            {
                Date = DateTime.Now,
                Punch = nomeArquivo,
                Message = mensagem,
                Url = url,
                Body = body,
                Token = token,
                RequestType = requestType,
                ExtraInfo = extraInfo
            };

            //TODO: Update DataDog
            //GrayLogService.Log(logServiceBus);
        }

        public static void LogResponse(string mensagem, string nomeArquivo, string status, string response)
        {
            Console.WriteLine(DateTime.Now.ToString() + " - " + mensagem);

            var logServiceBus = new LogProcessamentoModel
            {
                Date = DateTime.Now,
                Punch = nomeArquivo,
                Message = mensagem,
                Status = status,
                Response = response
            };

            //TODO: Update DataDog
            //GrayLogService.Log(logServiceBus);
        }

        public static void Log(string mensagem, string nomeArquivo, string classe, string metodo, object data)
        {
            Console.WriteLine(DateTime.Now.ToString() + " - " + mensagem);

            var log = new LogProcessamentoModel
            {
                Date = DateTime.Now,
                Punch = nomeArquivo,
                Class = classe,
                Message = mensagem,
                Method = metodo,
                Data = data
            };

            //TODO: Update DataDog
            //GrayLogService.Log(log);
        }

        public static void LogErro(string mensagem, string erro, string classe, string metodo, string arquivo)
        {
            Console.WriteLine(DateTime.Now.ToString() + " - " + mensagem);

            var log = new LogProcessamentoModel
            {
                Date = DateTime.Now,
                Punch = arquivo,
                Message = mensagem,
                Class = classe,
                Method = metodo,
                Error = erro
            };

            //TODO: Update DataDog
            //GrayLogService.LogError(log);
        }

        public static void LogErro(string mensagem, string classe, string metodo, Exception exception, string arquivo = "Application")
        {
            Console.WriteLine($"{DateTime.Now} - {mensagem}: {exception.Message}");

            var erros = string.Join("; ", exception.GetInnerExceptions().Select(x => x.Message));

            var log = new LogProcessamentoModel
            {
                Date = DateTime.Now,
                Punch = arquivo,
                Message = mensagem,
                Class = classe,
                Method = metodo,
                Error = erros,
                Source = exception.Source,
                StackTrace = exception.StackTrace
            };

            //TODO: Update DataDog
            //GrayLogService.LogError(log);
        }

        public static void LogErro(string mensagem, ProcessamentoException exception, object data = null)
        {
            Console.WriteLine($"{DateTime.Now} - {mensagem}: {exception.Message}");

            var erros = string.Join("; ", exception.GetInnerExceptions().Select(x => x.Message));

            var log = new LogProcessamentoModel
            {
                Date = DateTime.Now,
                Message = mensagem,
                Error = erros,
                Punch = exception.NomeArquivo,
                Class = exception.Target.Class,
                Method = exception.Target.Method,
                Source = exception.Source,
                StackTrace = exception.StackTrace,
                Data = data ?? exception.Data
            };

            //TODO: Update DataDog
            //GrayLogService.LogError(log);
        }
    }
}