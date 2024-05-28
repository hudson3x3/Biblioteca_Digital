using System.Web.Mvc;
using System.Web.Routing;

namespace GrupoLTM.WebSmart.Admin
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            #region "Rotas Conteudo"

            routes.MapRoute(
              name: "ConteudoCreate",
              url: "Conteudo/Create/{pagina}",
              defaults: new { controller = "Conteudo", action = "Create", id = UrlParameter.Optional }
            );

            routes.MapRoute(
              name: "ConteudoIndex",
              url: "Conteudo/Index/{pagina}",
              defaults: new { controller = "Conteudo", action = "Index", id = UrlParameter.Optional }
            );

            #endregion

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "DownloadArquivoCreditoLive",
                url: "Monitoramento/DownloadArquivoCreditoLive/{id}/{qtdLinhas}/{qtdPontos}",
                defaults: new { controller = "Monitoramento", action = "DownloadArquivoCreditoLive" }
            );

            routes.MapRoute(
                name: "ListarAcessos",
                url: "Relatorio/ListarAcessos/{catalogoId}/{dtInicio}/{dtFim}",
                defaults: new { controller = "Relatorio", action = "ListarAcessos" }
            );

            routes.MapRoute(
                name: "RelatorioAcessos",
                url: "Relatorio/ExportarRelatorioAcessos/{catalogoId}/{dtInicio}/{dtFim}",
                defaults: new { controller = "Relatorio", action = "ExportarRelatorioAcessos" }
            );

            routes.MapRoute(
                name: "ListarPontuacaoArquivos",
                url: "Pontuacao/ListarPontuacaoArquivos/{catalogoId}/{dtInicio}/{dtFim}/{campanhaId}/{tipoArquivoId}/{statusArquivoId}",
                defaults: new { controller = "Pontuacao", action = "ListarPontuacaoArquivos" }
            );

            routes.MapRoute(
                name: "DownloadCSVArquivoPontos",
                url: "Pontuacao/DownloadCSVArquivoPontos/{catalogoId}/{dtInicio}/{dtFim}/{campanhaId}/{tipoArquivoId}/{statusArquivoId}",
                defaults: new { controller = "Pontuacao", action = "DownloadCSVArquivoPontos" }
            );
            routes.MapRoute(
                name: "DownloadCSVResgateOffline",
                url: "ResgateOffline/DownloadCSVResgateOffline/{id}",
                defaults: new { controller = "ResgateOffline", action = "DownloadCSVResgateOffline" }
            );
            routes.MapRoute(
                name: "DownloadCSVCancelamento",
                url: "Pontuacao/DownloadCSVCancelamento/{id}",
                defaults: new { controller = "Pontuacao", action = "DownloadCSVCancelamento" }
            );

            routes.MapRoute(
                name: "ListarLogErroImportacaoPontuacao",
                url: "Pontuacao/ListarLogErroImportacaoPontuacao/{dtInicio}/{dtFim}",
                defaults: new { controller = "Pontuacao", action = "ListarLogErroImportacaoPontuacao" }
            );
        }
    }
}