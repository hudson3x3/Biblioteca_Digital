using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Models.Importacao;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrupoLTM.WebSmart.Services.Login
{
    public class ForcarPrimeiroAcessoModel
    {
        public ForcarPrimeiroAcessoModel()
        {

        }

        public ForcarPrimeiroAcessoModel(string linha)
        {
            try
            {
                _login = linha.Substring(1, 8);

            }
            catch (Exception ex)
            {
               this.Erro += "Layout do arquivo errado: falha na Posição Arquivo";
               gravaLogErro("Erro no Layout do Arquivo de Forçar Primeiro Acesso: " + ex.Message, this.Erro, "GrupoLTM.WebSmart.Services", string.Format("ForcarPrimeiroAcessoModel({0})", linha), "jobCatalog");

            }
        }

        private int _id { get; set; }
        private string _login;
        public string Login
        {
            get { return _login; }
        }
        private DateTime DataInclusao { get; set; }
        private DateTime DataEnvio { get; set; }
        public string Erro { get; set; }
        private static void gravaLogErro(string Erro, string Mensagem, string Source, string Metodo, string Codigo)
        {
            var logErro = new LogErro()
            {
                Erro = Erro,
                Mensagem = Mensagem,
                Source = Source,
                Metodo = Metodo,
                Controller = "ForcarPrimeiroAcessoModel",
                Pagina = string.Empty,
                Codigo = Codigo
            };

            var logErroService = new LogErroService();
            logErroService.SalvarLogErro(logErro);
        }
    }
}
