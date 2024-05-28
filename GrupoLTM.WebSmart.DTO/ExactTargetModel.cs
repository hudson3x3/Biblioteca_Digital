using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrupoLTM.WebSmart.DTO
{
    public class ExactTargetModel
    {
         public string CustomerKeyApi { get; set; }
         public string UserNameApi { get; set; }
         public string PasswordApi { get; set; }
         public List<ExactTargetDestinatarioModel> Destinatarios { get; set; }   
    }

    public class ExactTargetDestinatarioModel
    {
        public string Email { get; set; }
        public List<KeyValuePair<string, string>> Parametros { get; set; }
    }
}
