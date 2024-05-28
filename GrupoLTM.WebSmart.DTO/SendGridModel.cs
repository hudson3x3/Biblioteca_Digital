using System.Collections.Generic;

namespace GrupoLTM.WebSmart.DTO
{
    public class SendGridModel
    {
        public string template_id { get; set; }
        public From from { get; set; }
        public List<Personalizations> personalizations { get; set; }
    }

    public class Personalizations
    {
        public List<Destinatario> To { get; set; }
        public ParamDinamico dynamic_template_data { get; set; }
    }

    public class ParamDinamico
    {
        public string NOME { get; set; }
        public string LOGIN { get; set; }
        public string SENHA { get; set; }
        public string ParticipanteNome { get; set; }
        public string PedidoNumero { get; set; }
        public string Motivo { get; set; }
    }

    public class Destinatario
    {
        public string name { get; set; }
        public string email { get; set; }
    }

    public class From
    {
        public string name { get; set; }
        public string email { get; set; }
    }
}