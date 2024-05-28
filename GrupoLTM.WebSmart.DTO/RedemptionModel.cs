using System;

namespace GrupoLTM.WebSmart.DTO
{
    public class RedemptionModel
    {
        public int Id { get; set; }
        public int CatalogoId { get; set; }
        public string UrlArquivoResgatesGerais { get; set; }
        public string UrlArquivoResgatesAvon { get; set; }
        public int QtdResgatesGerais { get; set; }
        public int QtdResgatesAvon { get; set; }
        public DateTime DataProcessamento { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime DataAlteracao { get; set; }
        public bool Ativo { get; set; }
    }
}