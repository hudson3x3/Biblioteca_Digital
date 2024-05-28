using GrupoLTM.WebSmart.Admin.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using static GrupoLTM.WebSmart.Domain.Enums.EnumDomain;

namespace GrupoLTM.WebSmart.Admin.Models
{
    public class SMSAgendamentoModel
    {
        public SMSAgendamentoModel()
        {
            ProgramasId = new int[] { };
        }

        [Required]
        public Guid Id { get; set; }

        [Display(Name = "Período Fechado de")]
        public int PeriodoFechado { get; set; }
        public string TipoEnvioMensagem { get; set; }

        [Display(Name = "Upload da imagem")]
        [FileType("jpg")]
        public HttpPostedFileBase ArquivoUploadBaseImagem { get; set; }


        [Display(Name = "Catálogo")]
        public long? MktPlaceCatalogoId { get; set; }

        [Display(Name = "Catálogo")]
        public string MktPlaceCatalogo { get; set; }

        [Required]
        [Display(Name = "Título")]
        [MaxLength(64)]
        public string Titulo { get; set; }

        [Required]
        [Display(Name = "Texto da mensagem")]
        public string TextoMensagem { get; set; }

        [Required]
        [Display(Name = "Recorrência")]
        public int Recorrencia { get; set; }

        [Display(Name = "Recorrência")]
        public string RecorrenciaDescricao { get; set; }

        [Display(Name = "Início do período de crédito")]
        public DateTime? InicioPeriodoCredito { get; set; }

        [DataMaiorQue("InicioPeriodoCredito")]
        [Display(Name = "Fim do período de crédito")]
        [DisplayFormat(DataFormatString = "dd/MM/yyyy HH:00")]
        public DateTime? FimPeriodoCredito { get; set; }

        [Required]
        [DataMaiorQue]
        [Display(Name = "Início dos disparos")]
        public DateTime InicioDisparos { get; set; }

        [Display(Name = "Valor mínimo de saldo disponível")]
        [Range(0, 999999, ErrorMessage = "O campo {0} deve ser entre {1} e {2}.")]
        public decimal ValorMinimoDeSaldoDisponivel { get; set; }

        [Display(Name = "Valor mínimo de crédito no período")]
        [Range(0, 999999, ErrorMessage = "O campo {0} deve ser entre {1} e {2}.")]
        public decimal ValorMinimoDeCreditoNoPeriodo { get; set; }

        [Display(Name = "Upload da base de RA")]
        [FileType("csv")]
        public HttpPostedFileBase ArquivoUploadBaseRA { get; set; }

        public int? UploadBaseRAId { get; set; }
        public string UploadBaseRANome { get; set; }
        public string UploadBaseRANomeGerado { get; set; }

        public bool Upload { get; set; }

        [Display(Name = "Tipo de base")]
        public string TipoBase => Upload ? "Upload" : "Dinâmica";
        
        [Required]
        [Display(Name = "Programas")]
        public int[] ProgramasId { get; set; }

        [Display(Name = "Ano")]
        public int Ano { get; set; }

        public int SMSTipoId { get; set; }

        public DateTime? ProximaExecucao { get; set; }
        public SMSAgendamentoStatus Status { get; set; }

        

    }
}