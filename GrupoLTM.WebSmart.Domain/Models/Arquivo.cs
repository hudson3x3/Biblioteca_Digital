using System;
using System.Collections.Generic;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class Arquivo
    {
        public Arquivo()
        {
            CampanhaMetaGrupoItemPerfils = new List<CampanhaMetaGrupoItemPerfil>();
            CampanhaGrupoItemPontos = new List<CampanhaGrupoItemPonto>();
            CampanhaLogArquivoes = new List<CampanhaLogArquivo>();
            CampanhaMetaParticipantes = new List<CampanhaMetaParticipante>();
            CampanhaResultadoParticipantes = new List<CampanhaResultadoParticipante>();
            ParticipanteHierarquiaImportacaos = new List<ParticipanteHierarquiaImportacao>();
            ParticipanteImportacaos = new List<ParticipanteImportacao>();
            Lotes = new List<Lote>();
            Pontuacoes = new List<Pontuacao>();
        }

        public int Id { get; set; }
        public int TipoArquivoId { get; set; }
        public int StatusArquivoId { get; set; }
        public string Nome { get; set; }
        public string NomeGerado { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataTerminoProcessamento { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public bool Ativo { get; set; }
        public int? CampanhaId { get; set; }
        public int? UsuarioAdmId { get; set; }
        public string CaminhoArquivo { get; set; }
        public int? Sequencial { get; set; }
        public int? QuantidadeLinhas { get; set; }
        public int? QuantidadeLinhasErro { get; set; }
        public int? QuantidadeRevendedorasProcessadas { get; set; }
        public int? CatalogoId { get; set; }
        public long? PontosDisponiveis { get; set; }
        public long? PontosCancelados { get; set; }
        public long? PontosPendentes { get; set; }
        public string CaminhoLogDetalhado { get; set; }
        public string CaminhoLogErro { get; set; }
        public string CP { get; set; }
        public bool CSVGeradoArquivo { get; set; }
        public string CaminhoCSV { get; set; }
        public bool ArquivoParticionado { get; set; }

        public List<ArquivoCreditoLote> ListagemArquivoGrupo { get; set; }

        /// <summary>
        /// Tempo reportado em segundos
        /// </summary>
        public int? TempoProcessamento { get; set; }

        /// <summary>
        /// Indica se o extrato foi processado
        /// </summary>
        public bool ExtratoProcessado { get; set; }

        public virtual StatusArquivo StatusArquivo { get; set; }
        public virtual TipoArquivo TipoArquivo { get; set; }
        public virtual ICollection<CampanhaMetaGrupoItemPerfil> CampanhaMetaGrupoItemPerfils { get; set; }
        public virtual ICollection<CampanhaGrupoItemPonto> CampanhaGrupoItemPontos { get; set; }
        public virtual ICollection<CampanhaLogArquivo> CampanhaLogArquivoes { get; set; }
        public virtual ICollection<CampanhaMetaParticipante> CampanhaMetaParticipantes { get; set; }
        public virtual ICollection<CampanhaResultadoParticipante> CampanhaResultadoParticipantes { get; set; }
        public virtual ICollection<ParticipanteHierarquiaImportacao> ParticipanteHierarquiaImportacaos { get; set; }
        public virtual ICollection<ParticipanteImportacao> ParticipanteImportacaos { get; set; }
        public virtual ICollection<Lote> Lotes { get; set; }
        public virtual ICollection<Pontuacao> Pontuacoes { get; set; }
        public virtual ICollection<SalesStructureImportacao> SalesStructureImportacaos { get; set; }
        public virtual ICollection<LogAprovacaoArquivo> LogAprovacaoArquivos { get; set; }

        public virtual ICollection<ArquivoCreditoLote> ArquivoCreditoLote { get; set; }

        public virtual ICollection<CatalogoCP> CatalogosCP { get; set; }
    }
}
