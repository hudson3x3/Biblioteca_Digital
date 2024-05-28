using GrupoLTM.WebSmart.DTO;
using GrupoLTM.WebSmart.Infrastructure.Configuration;
using GrupoLTM.WebSmart.Infrastructure.Helpers;
using RestSharp;
using System;

namespace GrupoLTM.WebSmart.Services
{
    public class EmailSendGridService
    {
        public bool EnviarEmail(SendGridModel dados)
        {
            var client = new RestClient(Settings.EmailConfiguracao.SendGrid.Url);

            var request = new RestRequest(Settings.EmailConfiguracao.SendGrid.PathMail, Method.POST);

            var content = new
            {
                from = dados.from,
                template_id = dados.template_id,
                personalizations = dados.personalizations
            };

            var authorization = "bearer " + Settings.EmailConfiguracao.SendGrid.Key;

            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Authorization", authorization);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("cache-control", "no-cache");
            request.AddJsonBody(content);

            var response = client.Execute(request);

            if (!response.IsSuccessStatusCode())
                throw new Exception("Não foi possível enviar o e-mail: " + response.Content);

            return true;
        }
    }
}
