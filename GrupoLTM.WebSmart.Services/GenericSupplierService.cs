using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Services.Common;
using System;
using System.Linq;
using System.Web;
using GrupoLTM.WebSmart.DTO.Avon;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GrupoLTM.WebSmart.Services
{
    public class GenericSupplierService
    {
        public static List<ProductResult> SearhClusteredProducts(string CodeAvon, string productName)
        {
            List<ProductResult> productResultList = new List<ProductResult>();

            ProductSkuClusteredSearchRequest productSkuClusteredSearchRequest = new ProductSkuClusteredSearchRequest();

            productSkuClusteredSearchRequest.CodeAvon = CodeAvon ?? string.Empty;
            productSkuClusteredSearchRequest.Name = productName ?? string.Empty;

            var GSBaseUrl = ConfiguracaoService.GSBaseUrl();
            var GSClusteredProductsUrl = ConfiguracaoService.GSClusteredProductsUrl();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(GSBaseUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("basic", GsAuthorizationToken());

                    var jsonContent = JsonConvert.SerializeObject(productSkuClusteredSearchRequest);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    HttpResponseMessage response = client.PostAsync(GSBaseUrl + GSClusteredProductsUrl, content).Result;
                    
                    if (response != null)
                    {
                        var stream = response.Content.ReadAsStreamAsync().Result;
                        var jsonString = new StreamReader(stream).ReadToEnd();

                        GSResponse<List<ProductResult>> gsResponse = JsonConvert.DeserializeObject<GSResponse<List<ProductResult>>>(jsonString);

                        if (gsResponse.Success && gsResponse.Errors.Count == 0)
                        {
                            foreach (var productResult in gsResponse.Data)
                            {
                                productResultList.Add(productResult);
                            }
                        }
                    }
                }

                return productResultList;
            }
            catch (Exception ex)
            {
                var logErro = new LogErro
                {
                    Erro = ex.StackTrace,
                    Mensagem = ex.Message + " - SearhClusteredProducts CodeAvon: " + CodeAvon + " productName: " + productName,
                    Source = ex.Source,
                    Metodo = "SearhClusteredProducts",
                    Controller = "ClusterController",
                    Pagina = HttpContext.Current.Request.Url.ToString(),
                    Codigo = string.Empty
                };

                var logErroService = new LogErroService();
                logErroService.SalvarLogErro(logErro);

                Exception showException = ex;
                while (showException.InnerException != null)
                    showException = showException.InnerException;

                throw new ApplicationException(showException.Message);
            }
        }

        private static string GsAuthorizationToken()
        {
            var userAnsPassBytes = Encoding.UTF8.GetBytes(ConfiguracaoService.GSLogin() + ":" + ConfiguracaoService.GSPass());
            return (Convert.ToBase64String(userAnsPassBytes));
        }

        private static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
    }
}
