using GrupoLTM.WebSmart.DTO;
using System;
using System.Net;
using RestSharp;
using GrupoLTM.WebSmart.Infrastructure.Configuration;
namespace GrupoLTM.WebSmart.Infrastructure.Mail
{
    public class EmailSendGrid
    {
        public bool EnviarEmail(SendGridModel dados)
        {
            var client = new RestClient(Settings.EmailConfiguracao.SendGrid.Url);
            var key = "bearer " + Settings.EmailConfiguracao.SendGrid.Key;
            var request = new RestRequest(Settings.EmailConfiguracao.SendGrid.PathMail, Method.POST);
            var content = new
            {
                template_id = dados.template_id,
                from = dados.from,
                personalizations = dados.personalizations
            };

            string authorization = "bearer " + Settings.EmailConfiguracao.SendGrid.Key;
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Authorization", key);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("cache-control", "no-cache");
            request.AddJsonBody(content);
            try
            {
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted)
                {
                    return true;
                }
                else
                {
                    throw new Exception("Houve uma falha ao tentar enviar o e-mail.");
                }
            }
            catch
            {
                throw new Exception("Houve uma falha ao tentar enviar o e-mail.");
            }
        }
    }
}