using GrupoLTM.WebSmart.DTO;
using GrupoLTM.WebSmart.Infrastructure.ExactTarget;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using CasaNovaCom.WebCampanhas.Infra.Servicos;


namespace GrupoLTM.WebSmart.Infrastructure.Mail
{
    public class EmailExactTarget
    {

        public bool EnviarEmail(ExactTargetModel dados)
        {

            string requestID = String.Empty;
            string status = String.Empty;
    
            var client = new SoapClient();
            client.ClientCredentials.UserName.UserName = dados.UserNameApi;
            client.ClientCredentials.UserName.Password = dados.PasswordApi;

            var tsd = new TriggeredSendDefinition();
            tsd.CustomerKey = dados.CustomerKeyApi;

            var ts = new TriggeredSend();
            ts.TriggeredSendDefinition = tsd;

            //Destinatário do Email
            if (dados.Destinatarios != null && dados.Destinatarios.Count() > 0)
            {
                ts.Subscribers = new Subscriber[dados.Destinatarios.Count()];
                for (int i = 0; i < dados.Destinatarios.Count(); i++)
                {
                    ts.Subscribers[i] = new Subscriber();
                    ts.Subscribers[i].EmailAddress = dados.Destinatarios[i].Email;
                    ts.Subscribers[i].SubscriberKey = dados.Destinatarios[i].Email;
                    //Parametro
                    if (dados.Destinatarios[i].Parametros != null && dados.Destinatarios[i].Parametros.Count > 0)
                    {
                        ts.Subscribers[i].Attributes = new ExactTarget.Attribute[dados.Destinatarios[i].Parametros.Count];
                        for (int x = 0; x < dados.Destinatarios[i].Parametros.Count; x++)
                        {
                            ts.Subscribers[i].Attributes[x] = new ExactTarget.Attribute();
                            ts.Subscribers[i].Attributes[x].Name = dados.Destinatarios[i].Parametros[x].Key;
                            ts.Subscribers[i].Attributes[x].Value = dados.Destinatarios[i].Parametros[x].Value;
                        }
                    }
                }
            }
            else
            {
                throw new Exception("Os dados do destinário não foram encontrados");
            }

            try
            {

                CreateResult[] results = client.Create(new CreateOptions(), new APIObject[] { ts }, out requestID, out status);

                TriggeredSendCreateResult tscr = (TriggeredSendCreateResult)results[0];

                if (status != "OK")
                {
                    throw new Exception("Houve uma falha ao tentar enviar o e-mail.");
                }
                else
                {
                    return true;
                }

            }
            catch
            {
                throw new Exception("Houve uma falha ao tentar enviar o e-mail.");
            }

        }



    }
}
