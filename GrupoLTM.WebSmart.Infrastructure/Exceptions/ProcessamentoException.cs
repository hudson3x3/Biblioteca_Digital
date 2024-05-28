using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace GrupoLTM.WebSmart.Infrastructure.Exceptions
{
    public class ProcessamentoException : Exception
    {
        public string NomeArquivo { get; private set; }

        public Target Target => GetTarget();

        public override IDictionary Data { get; }

        public ProcessamentoException(string mensagem, Exception inner = null) : base(mensagem, inner)
        {
            Data = inner != null && inner.Data.Count > 0 ? inner.Data : new Dictionary<string, object>();
        }

        public ProcessamentoException(string mensagem, string nomeArquivo, Exception inner = null) : base(mensagem, inner)
        {
            NomeArquivo = nomeArquivo;
            Data = inner != null && inner.Data.Count > 0 ? inner.Data : new Dictionary<string, object>();
        }

        public ProcessamentoException(string mensagem, IDictionary data, Exception inner = null) : base(mensagem, inner)
        {
            Data = data != null && data.Count > 0 ? data : new Dictionary<string, object>();
        }

        public void AdicionarNomeArquivo(string name)
        {
            NomeArquivo = name;
        }

        private Target GetTarget()
        {
            var methodName = string.Empty;
            var className = string.Empty;
            var line = 0;

            try
            {
                var name = string.Empty;

                var stack = new StackTrace(GetBaseException(), true);

                if (stack.FrameCount > 0)
                {
                    var frame = stack.GetFrames().FirstOrDefault(x => x.GetMethod().DeclaringType.FullName.ToLower().Contains("grupoltm"));

                    if (frame is null)
                        frame = stack.GetFrames().Last();

                    var method = frame.GetMethod();

                    methodName = ExtractBracketed(method.Name);
                    className = method.ReflectedType.Name;
                    line = frame.GetFileLineNumber();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                methodName = "Não identificado";
                className = "Não identificado";
            }

            return new Target(methodName, className, line);
        }

        private string ExtractBracketed(string method)
        {
            try
            {
                var value = method.IndexOf('<') > -1 ? Regex.Match(method, @"\<([^>]*)\>").Groups[1].Value : method;

                if (string.IsNullOrEmpty(value))
                    return "'Emtpy'";
                else
                    return value;
            }
            catch
            {
                return method;
            }
        }
    }

    public class Target
    {
        public Target(string methodName, string className, int line)
        {
            Class = className;
            Method = $"{methodName} - Linha(código): {line}";
        }

        public string Method { get; set; }

        public string Class { get; set; }
    }

}
