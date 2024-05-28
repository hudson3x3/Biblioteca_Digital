using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrupoLTM.WebSmart.Domain.Enums;
using System.Transactions;
using System.IO;
using System.Net;


namespace GrupoLTM.WebSmart.Services
{
    public class FaleConoscoService : BaseService<FaleConosco>
    {
        public bool SalvarFaleConosco(FaleConoscoModel faleconosco)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository rep = context.CreateRepository<FaleConosco>();
                    FaleConosco _faleConosco = new FaleConosco
                    {
                        ParticipanteId = faleconosco.ParticipanteId,
                        AssuntoId = faleconosco.AssuntoId,
                        StatusFaleConoscoId = faleconosco.StatusFaleConoscoId,
                        UsuarioAdminId = faleconosco.UsuarioAdminId,
                        Nome = faleconosco.Nome,
                        Email = faleconosco.Email,
                        DDDTel = faleconosco.DDDTel,
                        Telefone = faleconosco.Telefone.Replace("-",""),
                        Codigo = faleconosco.Codigo,
                        Descricao = faleconosco.Descricao,
                        Resposta = faleconosco.Resposta,
                        DataResposta = faleconosco.DataResposta,
                        Ativo = faleconosco.Ativo,
                        DataInclusao = DateTime.Now,
                        DataAlteracao = DateTime.Now
                    };

                    using (TransactionScope scope = new TransactionScope())
                    {
                        rep.Create(_faleConosco);
                        rep.SaveChanges();
                        scope.Complete();
                    }

                    ConfiguracaoCampanhaService _configuracaoCampanhaService = new ConfiguracaoCampanhaService();
                    //EnviarSalesForce(_faleConosco, _configuracaoCampanhaService.ListarCampanhaConfiguracao().NomeCampanha.ToString());
                    //EnviarSalesForce(_faleConosco, "VESTE_A_CAMISA_BRF");
                    EnviarFaleConoscoProxis(_faleConosco, _configuracaoCampanhaService.ListarCampanhaConfiguracao().NomeCampanha.ToString());

                }
                
                

                return true;

            }
            catch (Exception exc)
            {
                return false;
            }
        }

        public List<AssuntoModel> ListarAssunto()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository rep = context.CreateRepository<Assunto>();
                List<AssuntoModel> listAssuntoModel = new List<AssuntoModel>();
                foreach (var item in rep.Filter<Assunto>(x => x.Ativo == true).OrderBy(x => x.Nome).ToList())
                {
                    listAssuntoModel.Add(new AssuntoModel
                    {
                        DataInclusao = item.DataInclusao,
                        DataAlteracao = item.DataAlteracao,
                        Ativo = item.Ativo,
                        Id = item.Id,
                        Nome = item.Nome
                    });
                }
                return listAssuntoModel;
            }
        }

        public void EnviarFaleConoscoProxis(FaleConosco faleConosco, string campanha)
        {
            FaleConoscoService _faleConoscoService = new FaleConoscoService();
            var myRequest = (HttpWebRequest)WebRequest.Create("https://www.salesforce.com/servlet/servlet.WebToCase?encoding=UTF-8");
            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";

            string dados = string.Empty;

            dados = "orgid=00Di0000000HDCL&" +
                        "retURL=&" +
                        "00Ni0000001SHyp="+ campanha +"&" +
                        "00Ni0000009loO6=" + faleConosco.ParticipanteId.ToString() + "&" +
                        "name=" + faleConosco.Nome + "&" +
                        "00Ni0000001SHC4=" + (faleConosco.Codigo != null ? faleConosco.Codigo : "") + "&" +
                        "email=" + (faleConosco.Email != null ? faleConosco.Email : "") + "&" +
                        "phone=" + faleConosco.DDDTel + faleConosco.Telefone + "&" +
                        "subject=" + _faleConoscoService.ListarAssunto().Where(x => x.Id == faleConosco.AssuntoId).FirstOrDefault().Nome + "&" +
                        "description=" + faleConosco.Descricao + "&" +
                        "submit=Enviar";

            var encoding = new UTF8Encoding();
            byte[] postData = encoding.GetBytes(dados);
            myRequest.ContentLength = postData.Length;
            using (var dataStream = myRequest.GetRequestStream())
            {
                dataStream.Write(postData, 0, postData.Length);
            }
            string result;
            using (var response = myRequest.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                }
            }
        }


        //public static void EnviarSalesForce(FaleConosco faleConosco, string campanha)
        //{
        //    FaleConoscoService _faleConoscoService = new FaleConoscoService();
        //    var myRequest = (HttpWebRequest)WebRequest.Create("https://www.salesforce.com/servlet/servlet.WebToCase?encoding=UTF-8");
        //    myRequest.Method = "POST";
        //    myRequest.ContentType = "application/x-www-form-urlencoded";
        //    var ocorrenciaTipo = _faleConoscoService.ListarAssunto().Where(x => x.Id == faleConosco.AssuntoId).FirstOrDefault().Nome;
        //    var dados = "orgid=00Di0000000HDCL&" +
        //                "retURL=http://&" +
        //                "name=" + faleConosco.Nome + "&" +
        //                "00Ni0000001SHC4=" + faleConosco.ParticipanteId.ToString() + "&" +
        //                "email=" + faleConosco.Email + "&" +
        //                "00Ni0000001SHlt=" + ocorrenciaTipo + "&" +
        //                "description=" + faleConosco.Descricao + "&" +
        //                "00Ni0000001SHyp="+ campanha + "&" +
        //                "submit=Enviar";
        //    var encoding = new UTF8Encoding();
        //    byte[] postData = encoding.GetBytes(dados);
        //    myRequest.ContentLength = postData.Length;
        //    using (var dataStream = myRequest.GetRequestStream())
        //    {
        //        dataStream.Write(postData, 0, postData.Length);
        //    }
        //    string result;
        //    using (var response = myRequest.GetResponse())
        //    {
        //        using (var reader = new StreamReader(response.GetResponseStream()))
        //        {
        //            result = reader.ReadToEnd();
        //        }
        //    }
        //}
    }
}
