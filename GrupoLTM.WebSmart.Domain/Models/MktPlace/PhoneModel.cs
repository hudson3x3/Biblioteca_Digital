using GrupoLTM.WebSmart.Domain.Enums;

namespace GrupoLTM.WebSmart.Domain.Models.MktPlace
{
    public class PhoneModel
    {
        public string Ddd { get; set; }
        public string Number { get; set; }
        public EnumMktPlace.PhoneType PhoneType { get; set; }
    }
}
