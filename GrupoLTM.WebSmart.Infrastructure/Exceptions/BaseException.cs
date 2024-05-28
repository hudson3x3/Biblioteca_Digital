using System;

namespace GrupoLTM.WebSmart.Infrastructure.Exceptions
{
    public abstract class BaseException : Exception
    {
        public string Code { get; private set; }
        private Exception _inner;

        public BaseException(string message = "", Exception inner = null, string code = "")
            : base(message, inner)
        {
            Code = code == string.Empty ? HResult.ToString() : string.Empty;
            Source = inner?.Source;
            _inner = inner;
        }

        public override string StackTrace
        {
            get
            {
                return _inner?.StackTrace;
            }
        }
    }

    /// <summary>
    /// Exception somente para pegar o HResult
    /// </summary>
    public class SistemaException : BaseException
    {
        public SistemaException(string message = "", Exception inner = null)
            : base(message, inner)
        {
        }
    }
}
