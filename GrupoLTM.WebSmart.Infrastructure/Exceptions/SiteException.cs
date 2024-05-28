using System;

namespace GrupoLTM.WebSmart.Infrastructure.Exceptions
{
    public class SiteException : Exception
    {
        public SiteException(string message) : base(message) { }
    }
}
