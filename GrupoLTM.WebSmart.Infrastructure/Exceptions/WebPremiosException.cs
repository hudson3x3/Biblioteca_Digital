using System;

namespace GrupoLTM.WebSmart.Infrastructure.Exceptions
{
    public class WebPremiosException : BaseException
    {
        public WebPremiosException(int httpCode, string message = "", Exception inner = null) : base(message, inner)
        {
        }
    }
}
