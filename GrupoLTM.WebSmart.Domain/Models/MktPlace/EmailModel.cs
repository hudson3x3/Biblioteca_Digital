using GrupoLTM.WebSmart.Domain.Enums;

namespace GrupoLTM.WebSmart.Domain.Models.MktPlace
{
    public class EmailModel
    {
        public string EmailText { get; set; }
        public EnumMktPlace.EmailModelType EmailType { get; set; }
    }
}
