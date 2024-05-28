using System;

namespace GrupoLTM.WebSmart.Infrastructure.Exceptions
{
    public class MarketPlaceException : Exception
    {
        public MarketPlaceException(string message, Exception inner = null) : base(message, inner) { }
    }
}
