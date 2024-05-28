using System.Linq;

namespace GrupoLTM.WebSmart.Infrastructure.Helpers
{
    public static class Url
    {
        public static string Combine(params string[] items)
        {
            return items.Aggregate((current, value) => $"{current.TrimEnd('/')}/{value.TrimStart('/')}");
        }
    }
}
