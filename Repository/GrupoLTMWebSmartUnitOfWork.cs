using System;
using System.Configuration;
using GrupoLTM.WebSmart.Domain.Models;
using System.Diagnostics;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using GrupoLTM.WebSmart.Domain.Models.Mapping;
using GrupoLTM.WebSmart.Domain.Models.Importacao;

namespace GrupoLTM.WebSmart.Domain.Repository
{
    public class AvonDbContext : DbContext, IDisposable, IUnitOfWork
    {
        public AvonDbContext(string connectionString = "LibraryWebSmart") : base(connectionString)
        {
            Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            this.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
            Database.Connection.ConnectionString =
                ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
            Database.Log = s => Debug.WriteLine(s);

        }

        public AvonDbContext()
            : base("LibraryWebSmart")
        {
            Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            this.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
            Database.Log = s => Debug.WriteLine(s);

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new ArquivoMap());
            modelBuilder.Configurations.Add(new SMSAgendamentoImagemMap());
            modelBuilder.Configurations.Add(new AssuntoMap());
            modelBuilder.Configurations.Add(new CampanhaMap());
            modelBuilder.Configurations.Add(new CampanhaAssociacaoGrupoItemMap());
            modelBuilder.Configurations.Add(new CampanhaAssociacaoGrupoItemImportacaoMap());
            modelBuilder.Configurations.Add(new CampanhaCalculadaMetaResultadoGrupoItemImportacaoMap());
            modelBuilder.Configurations.Add(new CampanhaCalculadaMetaResultadoPessoaImportacaoMap());
            modelBuilder.Configurations.Add(new CampanhaCalculadaMetaResultadoRankingImportacaoMap());
            modelBuilder.Configurations.Add(new CampanhaCalculadaVendeuGanhouGrupoItemImportacaoMap());
            modelBuilder.Configurations.Add(new CampanhaCalculadaVendeuGanhouPessoaImportacaoMap());
            modelBuilder.Configurations.Add(new CampanhaCalculadaVendeuGanhouRankingImportacaoMap());
            modelBuilder.Configurations.Add(new CampanhaConteudoMap());
            modelBuilder.Configurations.Add(new CampanhaEstruturaMap());
            modelBuilder.Configurations.Add(new CampanhaFaixaAtingimentoMap());
            modelBuilder.Configurations.Add(new CampanhaFaixaAtingimentoGrupoItemMap());
            modelBuilder.Configurations.Add(new CampanhaFaixaAtingimentoGrupoItemImportacaoMap());
            modelBuilder.Configurations.Add(new CampanhaGrupoItemMap());
            modelBuilder.Configurations.Add(new CampanhaGrupoItemPontoMap());
            modelBuilder.Configurations.Add(new CampanhaGrupoItemPontosImportacaoMap());
            modelBuilder.Configurations.Add(new CampanhaLogArquivoMap());
            modelBuilder.Configurations.Add(new CampanhaMetaGrupoItemPerfilMap());
            modelBuilder.Configurations.Add(new CampanhaMetaGrupoItemPerfilImportacaoMap());
            modelBuilder.Configurations.Add(new CampanhaMetaParticipanteMap());
            modelBuilder.Configurations.Add(new CampanhaMetaPessoaMap());
            modelBuilder.Configurations.Add(new CampanhaMetaPessoaImportacaoMap());
            modelBuilder.Configurations.Add(new CampanhaPassoMap());
            modelBuilder.Configurations.Add(new CampanhaPerfilMap());
            modelBuilder.Configurations.Add(new CampanhaPeriodoMap());
            modelBuilder.Configurations.Add(new CampanhaPeriodoParticipanteEstruturaMap());
            modelBuilder.Configurations.Add(new CampanhaPeriodoParticipanteHierarquiaMap());
            modelBuilder.Configurations.Add(new CampanhaPeriodoParticipantePerfilMap());
            modelBuilder.Configurations.Add(new CampanhaResultadoCalculadoParticipanteMap());
            modelBuilder.Configurations.Add(new CampanhaResultadoParticipanteMap());
            modelBuilder.Configurations.Add(new ConfiguracaoCampanhaMap());
            modelBuilder.Configurations.Add(new ConteudoMap());
            modelBuilder.Configurations.Add(new ConteudoEstruturaMap());
            modelBuilder.Configurations.Add(new ConteudoImagemMap());
            modelBuilder.Configurations.Add(new ConteudoPerfilMap());
            modelBuilder.Configurations.Add(new DisparoEmailMap());
            modelBuilder.Configurations.Add(new DisparoEmailTipoMap());
            modelBuilder.Configurations.Add(new EstadoMap());
            modelBuilder.Configurations.Add(new EstruturaMap());
            modelBuilder.Configurations.Add(new FaixaMap());
            modelBuilder.Configurations.Add(new FaixaAtingementoMap());
            modelBuilder.Configurations.Add(new FaleConoscoMap());
            modelBuilder.Configurations.Add(new GrupoItemMap());
            modelBuilder.Configurations.Add(new GrupoItemImportacaoMap());
            //modelBuilder.Configurations.Add(new LogMap());
            modelBuilder.Configurations.Add(new LogErroMap());
            //modelBuilder.Configurations.Add(new LogPontuacaoMap());

            modelBuilder.Configurations.Add(new LogRaizMap());
            modelBuilder.Configurations.Add(new LogProcessamentoMap());
            modelBuilder.Configurations.Add(new LogAcaoMap());
            modelBuilder.Configurations.Add(new LogTipoMap());

            modelBuilder.Configurations.Add(new MenuMap());
            modelBuilder.Configurations.Add(new MenuEstruturaMap());
            modelBuilder.Configurations.Add(new MenuPerfilMap());
            modelBuilder.Configurations.Add(new ModuloMap());
            modelBuilder.Configurations.Add(new ModuloEstruturaMap());
            modelBuilder.Configurations.Add(new ModuloPerfilMap());
            modelBuilder.Configurations.Add(new ParticipanteMap());
            modelBuilder.Configurations.Add(new ParticipanteEstruturaMap());
            modelBuilder.Configurations.Add(new ParticipanteHierarquiaMap());
            modelBuilder.Configurations.Add(new ParticipanteHierarquiaImportacaoMap());
            modelBuilder.Configurations.Add(new ParticipanteImportacaoMap());
            modelBuilder.Configurations.Add(new ParticipantePerfilMap());
            modelBuilder.Configurations.Add(new ParticipanteQuestionarioMap());
            modelBuilder.Configurations.Add(new PassoMap());
            modelBuilder.Configurations.Add(new PerfilMap());
            modelBuilder.Configurations.Add(new PerguntaMap());
            modelBuilder.Configurations.Add(new PeriodoMap());
            modelBuilder.Configurations.Add(new PontuacaoMap());
            modelBuilder.Configurations.Add(new ResgateOffLineMap());
            modelBuilder.Configurations.Add(new ForcarPrimeiroAcessoCatalogoMap());
            modelBuilder.Configurations.Add(new PontuacaoCampanhaPeriodoMap());
            modelBuilder.Configurations.Add(new PontuacaoImportacaoMap());
            modelBuilder.Configurations.Add(new PontuacaoParticipanteQuestionarioMap());
            modelBuilder.Configurations.Add(new QuestionarioMap());
            modelBuilder.Configurations.Add(new QuestionarioEstruturaMap());
            modelBuilder.Configurations.Add(new QuestionarioPerfilMap());
            modelBuilder.Configurations.Add(new RespostaMap());
            modelBuilder.Configurations.Add(new StatusArquivoMap());
            modelBuilder.Configurations.Add(new StatusCampanhaMap());
            modelBuilder.Configurations.Add(new StatusFaleConoscoMap());
            modelBuilder.Configurations.Add(new StatusParticipanteMap());
            modelBuilder.Configurations.Add(new StatusPontuacaoMap());
            modelBuilder.Configurations.Add(new TemaMap());
            modelBuilder.Configurations.Add(new TipoAcessoMap());
            modelBuilder.Configurations.Add(new TipoArquivoMap());
            modelBuilder.Configurations.Add(new TipoCadastroMap());
            modelBuilder.Configurations.Add(new TipoCampanhaMap());
            modelBuilder.Configurations.Add(new TipoConteudoMap());
            modelBuilder.Configurations.Add(new TipoEstruturaMap());
            modelBuilder.Configurations.Add(new TipoModuloMap());
            modelBuilder.Configurations.Add(new TipoPontuacaoMap());
            modelBuilder.Configurations.Add(new TipoQuestionarioMap());
            modelBuilder.Configurations.Add(new TipoRespostaMap());
            modelBuilder.Configurations.Add(new TipoValidacaoPositivaMap());
            modelBuilder.Configurations.Add(new UsuarioAdmMap());
            modelBuilder.Configurations.Add(new UsuarioAdmMenuMap());
            //modelBuilder.Configurations.Add(new UsuarioAdmAlunoMap());
            modelBuilder.Configurations.Add(new VendaMap());
            modelBuilder.Configurations.Add(new VendaImportacaoMap());
            modelBuilder.Configurations.Add(new VendaLogArquivoMap());

            modelBuilder.Configurations.Add(new ApoioImportacaoMap());
            modelBuilder.Configurations.Add(new ApoioDetailMap());

            modelBuilder.Configurations.Add(new IndicacaoImportacaoMap());
            modelBuilder.Configurations.Add(new IndicacaoHeaderMap());
            modelBuilder.Configurations.Add(new IndicacaoDetailMap());

            modelBuilder.Configurations.Add(new ConsecutividadeImportacaoMap());
            modelBuilder.Configurations.Add(new ConsecutividadeHeaderMap());
            modelBuilder.Configurations.Add(new ConsecutividadeDetailMap());

            modelBuilder.Configurations.Add(new TipoOrigemMap());
            modelBuilder.Configurations.Add(new PontuacaoArquivosMap());

            modelBuilder.Configurations.Add(new ImportacaoErroMap());
            modelBuilder.Configurations.Add(new LoteMap());
            modelBuilder.Configurations.Add(new LogIntegracaoMap());

            modelBuilder.Configurations.Add(new IndicacaoHeaderExtratoMap());
            modelBuilder.Configurations.Add(new IndicacaoDetailExtratoMap());
            modelBuilder.Configurations.Add(new IndicacaoHeaderLoteMap());
            modelBuilder.Configurations.Add(new ArquivoConfiguracaoMap());
            modelBuilder.Configurations.Add(new PontuacaoRecebidaMap());
            modelBuilder.Configurations.Add(new ResgatesAvonMap());
            modelBuilder.Configurations.Add(new CatalogoCPMap());
            modelBuilder.Configurations.Add(new ParticipanteCPMap());
            modelBuilder.Configurations.Add(new LogDetalhadoMap());
            modelBuilder.Configurations.Add(new ApoioExtratoMap());
            modelBuilder.Configurations.Add(new LogAprovacaoArquivoMap());
            modelBuilder.Configurations.Add(new SalesStructureImportacaoMap());
            modelBuilder.Configurations.Add(new LogAccountMap());
            modelBuilder.Configurations.Add(new LogAcessoHotSiteMap());
            modelBuilder.Configurations.Add(new MetaRAMap());

            // sms
            modelBuilder.Configurations.Add(new SMSAgendamentoMap());
            modelBuilder.Configurations.Add(new SMSExecucaoMap());
            modelBuilder.Configurations.Add(new SMSAgendamentoRAMap());
            modelBuilder.Configurations.Add(new SMSAgendamentoProgramaIncentivoMap());
            modelBuilder.Configurations.Add(new SMSAgendamentoAvulsoMap());
            modelBuilder.Configurations.Add(new SMSTipoMap());

            modelBuilder.Configurations.Add(new ProgramaIncentivoMap());
            modelBuilder.Configurations.Add(new ProgramaIncentivoCatalogoArquivoMap());

            modelBuilder.Configurations.Add(new LogSimuladorMap());
            modelBuilder.Configurations.Add(new CampanhaSimuladorMap());
            modelBuilder.Configurations.Add(new MecanicaSimuladorMap());
            modelBuilder.Configurations.Add(new IconeSimuladorMap());
            modelBuilder.Configurations.Add(new FatorConversaoSimuladorMap());
            modelBuilder.Configurations.Add(new FatorConversaoPontosSimuladorMap());
            modelBuilder.Configurations.Add(new SubMecanicaSimuladorMap());
            modelBuilder.Configurations.Add(new CampanhaMecanicaSimuladorMap());
            modelBuilder.Configurations.Add(new FatorConversaoMecanicaSimuladorMap());
            modelBuilder.Configurations.Add(new MecanicaSubMecanicaSimuladorMap());

            modelBuilder.Configurations.Add(new ClusterMap());
            modelBuilder.Configurations.Add(new ClusterRAMap());
            modelBuilder.Configurations.Add(new ClusterProductMap());
            modelBuilder.Configurations.Add(new ClusterRATempMap());

            modelBuilder.Configurations.Add(new ArquivoClubeEstrelasRegisterMap());
            modelBuilder.Configurations.Add(new ClubeEstrelasRegister1ExtratoMap());
            modelBuilder.Configurations.Add(new ClubeEstrelasRegister2ExtratoMap());
            modelBuilder.Configurations.Add(new ClubeEstrelasProgramDescriptionMap());

            modelBuilder.Configurations.Add(new MigracaoHeaderMap());
            modelBuilder.Configurations.Add(new MigracaoRegister1Map());
            modelBuilder.Configurations.Add(new MigracaoRegister2Map());
            modelBuilder.Configurations.Add(new MigracaoRegister1ExtratoMap());
            modelBuilder.Configurations.Add(new MigracaoRegister2ExtratoMap());

            modelBuilder.Configurations.Add(new EstornoMap());
            modelBuilder.Configurations.Add(new EstornoLoteMap());
            modelBuilder.Configurations.Add(new EstornoPedidoMap());
            modelBuilder.Configurations.Add(new EstornoRequisicaoMap());
            modelBuilder.Configurations.Add(new EstornoSmsMap());
            modelBuilder.Configurations.Add(new EstornoEmailMap());            

            modelBuilder.Configurations.Add(new ItemCallBackWebHookMap());

            modelBuilder.Configurations.Add(new NatalHeaderMap());
            modelBuilder.Configurations.Add(new NatalDetailMap());

            modelBuilder.Configurations.Add(new AliancaHeaderMap());
            modelBuilder.Configurations.Add(new AliancaDetailMap());

            modelBuilder.Configurations.Add(new BannerMap());
            modelBuilder.Configurations.Add(new BannerPreviewMap());
        }

        public Repository<TEntity> CreateRepository<TEntity>()
        {
            return new Repository<TEntity>(this);
        }

        public ConteudoRepository ConteudoRepository()
        {
            return new ConteudoRepository(this);
        }

        public QuestionarioRepository QuestionarioRepository()
        {
            return new QuestionarioRepository(this);
        }

        public ArquivoRepository ArquivoRepository()
        {
            return new ArquivoRepository(this);
        }

        public ProgramaIncentivoRepository ProgramaIncentivoRepository()
        {
            return new ProgramaIncentivoRepository(this);
        }

        public DbSet<Arquivo> Arquivo { get; set; }
        public DbSet<Assunto> Assunto { get; set; }
        public DbSet<Catalogo> Catalogo { get; set; }
        public DbSet<Campanha> Campanha { get; set; }
        public DbSet<CampanhaAssociacaoGrupoItem> CampanhaAssociacaoGrupoItem { get; set; }
        public DbSet<CampanhaAssociacaoGrupoItemImportacao> CampanhaAssociacaoGrupoItemImportacao { get; set; }

        public DbSet<CampanhaCalculadaMetaResultadoGrupoItemImportacao>
            CampanhaCalculadaMetaResultadoGrupoItemImportacao
        { get; set; }

        public DbSet<CampanhaCalculadaMetaResultadoPessoaImportacao> CampanhaCalculadaMetaResultadoPessoaImportacao
        {
            get;
            set;
        }

        public DbSet<CampanhaCalculadaMetaResultadoRankingImportacao> CampanhaCalculadaMetaResultadoRankingImportacao
        {
            get;
            set;
        }

        public DbSet<CampanhaCalculadaVendeuGanhouGrupoItemImportacao> CampanhaCalculadaVendeuGanhouGrupoItemImportacao
        {
            get;
            set;
        }

        public DbSet<CampanhaCalculadaVendeuGanhouPessoaImportacao> CampanhaCalculadaVendeuGanhouPessoaImportacao
        {
            get;
            set;
        }

        public DbSet<CampanhaCalculadaVendeuGanhouRankingImportacao> CampanhaCalculadaVendeuGanhouRankingImportacao
        {
            get;
            set;
        }

        public DbSet<CampanhaConteudo> CampanhaConteudo { get; set; }
        public DbSet<CampanhaEstrutura> CampanhaEstrutura { get; set; }
        public DbSet<CampanhaFaixaAtingimento> CampanhaFaixaAtingimento { get; set; }
        public DbSet<CampanhaFaixaAtingimentoGrupoItem> CampanhaFaixaAtingimentoGrupoItems { get; set; }

        public DbSet<CampanhaFaixaAtingimentoGrupoItemImportacao> CampanhaFaixaAtingimentoGrupoItemImportacao
        {
            get;
            set;
        }

        public DbSet<CampanhaGrupoItem> CampanhaGrupoItem { get; set; }
        public DbSet<CampanhaGrupoItemPonto> CampanhaGrupoItemPonto { get; set; }
        public DbSet<CampanhaGrupoItemPontosImportacao> CampanhaGrupoItemPontosImportacao { get; set; }
        public DbSet<CampanhaLogArquivo> CampanhaLogArquivo { get; set; }
        public DbSet<CampanhaMetaGrupoItemPerfil> CampanhaMetaGrupoItemPerfil { get; set; }
        public DbSet<CampanhaMetaGrupoItemPerfilImportacao> CampanhaMetaGrupoItemPerfilImportacao { get; set; }
        public DbSet<CampanhaMetaParticipante> CampanhaMetaParticipante { get; set; }
        public DbSet<CampanhaMetaPessoa> CampanhaMetaPessoa { get; set; }
        public DbSet<CampanhaMetaPessoaImportacao> CampanhaMetaPessoaImportacao { get; set; }
        public DbSet<CampanhaPasso> CampanhaPasso { get; set; }
        public DbSet<CampanhaPerfil> CampanhaPerfil { get; set; }
        public DbSet<CampanhaPeriodo> CampanhaPeriodo { get; set; }
        public DbSet<LogProcessamentoDb> LogProcessamento { get; set; }
        public DbSet<CampanhaPeriodoParticipanteEstrutura> CampanhaPeriodoParticipanteEstrutura { get; set; }
        public DbSet<CampanhaPeriodoParticipanteHierarquia> CampanhaPeriodoParticipanteHierarquia { get; set; }
        public DbSet<CampanhaPeriodoParticipantePerfil> CampanhaPeriodoParticipantePerfil { get; set; }
        public DbSet<CampanhaResultadoCalculadoParticipante> CampanhaResultadoCalculadoParticipante { get; set; }
        public DbSet<CampanhaResultadoParticipante> CampanhaResultadoParticipante { get; set; }
        public DbSet<ConfiguracaoCampanha> ConfiguracaoCampanha { get; set; }
        public DbSet<Conteudo> Conteudo { get; set; }
        public DbSet<ConteudoEstrutura> ConteudoEstrutura { get; set; }
        public DbSet<ConteudoImagem> ConteudoImagem { get; set; }
        public DbSet<ConteudoPerfil> ConteudoPerfil { get; set; }
        public DbSet<DisparoEmail> DisparoEmail { get; set; }
        public DbSet<DisparoEmailTipo> DisparoEmailTipo { get; set; }
        public DbSet<Estado> Estado { get; set; }
        public DbSet<Estrutura> Estrutura { get; set; }
        public DbSet<Faixa> Faixa { get; set; }
        public DbSet<FaixaAtingemento> FaixaAtingemento { get; set; }
        public DbSet<FaleConosco> FaleConosco { get; set; }
        public DbSet<GrupoItem> GrupoItem { get; set; }
        public DbSet<GrupoItemImportacao> GrupoItemImportacao { get; set; }
        public DbSet<Log> Log { get; set; }

        public DbSet<LogErro> LogErro { get; set; }

        //public DbSet<LogPontuacao> LogPontuacao { get; set; }
        public DbSet<LogTipo> LogTipo { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<MenuEstrutura> MenuEstrutura { get; set; }
        public DbSet<MenuPerfil> MenuPerfil { get; set; }
        public DbSet<Modulo> Modulo { get; set; }
        public DbSet<ModuloEstrutura> ModuloEstrutura { get; set; }
        public DbSet<ModuloPerfil> ModuloPerfil { get; set; }
        public DbSet<Participante> Participante { get; set; }
        public DbSet<ParticipanteEstrutura> ParticipanteEstrutura { get; set; }
        public DbSet<ParticipanteHierarquia> ParticipanteHierarquia { get; set; }
        public DbSet<ParticipanteHierarquiaImportacao> ParticipanteHierarquiaImportacao { get; set; }
        public DbSet<ParticipanteImportacao> ParticipanteImportacao { get; set; }
        public DbSet<ParticipantePerfil> ParticipantePerfil { get; set; }
        public DbSet<ParticipanteQuestionario> ParticipanteQuestionario { get; set; }
        public DbSet<Passo> Passo { get; set; }
        public DbSet<Perfil> Perfil { get; set; }
        public DbSet<Pergunta> Pergunta { get; set; }
        public DbSet<Periodo> Periodo { get; set; }
        public DbSet<Pontuacao> Pontuacao { get; set; }
        public DbSet<ResgateOffLine> ResgateOffLine { get; set; }
        public DbSet<ForcarPrimeiroAcessoCatalogo> ForcarPrimeiroAcessoCatalogo { get; set; }
        public DbSet<PontuacaoCampanhaPeriodo> PontuacaoCampanhaPeriodo { get; set; }
        public DbSet<PontuacaoImportacao> PontuacaoImportacao { get; set; }
        public DbSet<PontuacaoParticipanteQuestionario> PontuacaoParticipanteQuestionario { get; set; }
        public DbSet<Questionario> Questionario { get; set; }
        public DbSet<QuestionarioEstrutura> QuestionarioEstrutura { get; set; }
        public DbSet<QuestionarioPerfil> QuestionarioPerfil { get; set; }
        public DbSet<Resposta> Resposta { get; set; }
        public DbSet<StatusArquivo> StatusArquivo { get; set; }
        public DbSet<StatusCampanha> StatusCampanha { get; set; }
        public DbSet<StatusFaleConosco> StatusFaleConosco { get; set; }
        public DbSet<StatusParticipante> StatusParticipante { get; set; }
        public DbSet<StatusPontuacao> StatusPontuacao { get; set; }
        public DbSet<Tema> Tema { get; set; }
        public DbSet<TipoAcesso> TipoAcesso { get; set; }
        public DbSet<TipoArquivo> TipoArquivo { get; set; }
        public DbSet<TipoCadastro> TipoCadastro { get; set; }
        public DbSet<TipoCampanha> TipoCampanha { get; set; }
        public DbSet<TipoConteudo> TipoConteudo { get; set; }
        public DbSet<TipoEstrutura> TipoEstrutura { get; set; }
        public DbSet<TipoModulo> TipoModulo { get; set; }
        public DbSet<TipoPontuacao> TipoPontuacao { get; set; }
        public DbSet<TipoQuestionario> TipoQuestionario { get; set; }
        public DbSet<TipoResposta> TipoResposta { get; set; }
        public DbSet<TipoValidacaoPositiva> TipoValidacaoPositiva { get; set; }
        public DbSet<UsuarioAdm> UsuarioAdm { get; set; }
        public DbSet<ForcarPrimeiroAcesso> ForcarPrimeiroAcesso { get; set; }
        public DbSet<UsuarioAdmMenu> UsuarioAdmMenu { get; set; }
        //public DbSet<UsuarioAdmAluno> UsuarioAdmAluno { get; set; }
        public DbSet<Venda> Venda { get; set; }
        public DbSet<VendaImportacao> VendaImportacao { get; set; }
        public DbSet<VendaLogArquivo> VendaLogArquivo { get; set; }

        public DbSet<IndicacaoHeaderExtrato> IndicacaoHeaderExtrato { get; set; }
        public DbSet<IndicacaoDetailExtrato> IndicacaoDetailExtrato { get; set; }

        public DbSet<ImportacaoErro> ImportacaoErro { get; set; }

        public DbSet<ArquivoConfiguracao> ArquivoConfiguracao { get; set; }

        public DbSet<PontuacaoRecebida> PontuacaoRecebida { get; set; }

        public DbSet<ResgatesAvon> ResgatesAvon { get; set; }

        public DbSet<CatalogoCP> CatalogoCP { get; set; }

        public DbSet<ParticipanteCP> ParticipanteCP { get; set; }
        public DbSet<ParticipanteCatalogo> ParticipanteCatalogo { get; set; }
        public DbSet<LogDetalhado> LogDetalhado { get; set; }
        public DbSet<LogIntegracao> LogIntegracao { get; set; }

        public DbSet<LogAprovacaoArquivo> LogAprovacaoArquivo { get; set; }
        public DbSet<SalesStructureImportacao> SalesStructureImportacao { get; set; }
        public DbSet<LogAccount> LogAccount { get; set; }
        public DbSet<LogAcessoHotSite> LogAcessoHotSite { get; set; }

        public DbSet<CampanhaSimulador> CampanhaSimulador { get; set; }
        public DbSet<LogSimulador> LogSimulador { get; set; }
        public DbSet<MecanicaSimulador> MecanicaSimulador { get; set; }
        public DbSet<IconeSimulador> IconeSimulador { get; set; }
        public DbSet<FatorConversaoSimulador> FatorConversaoSimulador { get; set; }
        public DbSet<SubMecanicaSimulador> SubMecanicaSimulador { get; set; }
        public DbSet<CampanhaMecanicaSimulador> CampanhaMecanicaSimulador { get; set; }
        public DbSet<FatorConversaoMecanicaSimulador> FatorConversaoMecanicaSimulador { get; set; }
        public DbSet<FatorConversaoPontosSimulador> FatorConversaoPontosSimulador { get; set; }
        public DbSet<MecanicaSubMecanicaSimulador> MecanicaSubMecanicaSimulador { get; set; }

        public DbSet<MetaRA> MetaRA { get; set; }

        public DbSet<SMSAgendamento> SMSAgendamento { get; set; }
        public DbSet<SMSExecucao> SMSExecucao { get; set; }
        public DbSet<SMSAgendamentoRA> SMSAgendamentoRA { get; set; }
        public DbSet<SMSAgendamentoAvulso> SMSAgendamentoAvulso { get; set; }
        public DbSet<SMSTipo> SMSTipo { get; set; }
        public DbSet<ProgramaIncentivo> ProgramaIncentivo { get; set; }
        public DbSet<ProgramaIncentivoCatalogoArquivo> ProgramaIncentivoCatalogoArquivo { get; set; }

        public DbSet<Cluster> Clusters { get; set; }
        public DbSet<ClusterRA> ClustersRA { get; set; }
        public DbSet<ClusterProduct> ClusterProducts { get; set; }
        public DbSet<ClusterRATemp> ClustersRATemp { get; set; }
        public DbSet<ItemCallBackWebHook> ItemCallBackWebHook { get; set; }


    }


}