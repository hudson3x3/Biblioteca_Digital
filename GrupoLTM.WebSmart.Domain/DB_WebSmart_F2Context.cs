using System.Data.Entity;
using GrupoLTM.WebSmart.Domain.Models.Mapping;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class DB_WebSmart_F2Context : DbContext
    {
        static DB_WebSmart_F2Context()
        {
            Database.SetInitializer<DB_WebSmart_F2Context>(null);
        }

        public DB_WebSmart_F2Context()
            : base("Name=DB_WebSmart_F2Context")
        {
        }

        public DbSet<Arquivo> Arquivo { get; set; }
        public DbSet<Assunto> Assunto { get; set; }
        public DbSet<Campanha> Campanha { get; set; }
        public DbSet<CampanhaAssociacaoGrupoItem> CampanhaAssociacaoGrupoItem { get; set; }
        public DbSet<CampanhaAssociacaoGrupoItemImportacao> CampanhaAssociacaoGrupoItemImportacao { get; set; }
        public DbSet<CampanhaCalculadaMetaResultadoGrupoItemImportacao> CampanhaCalculadaMetaResultadoGrupoItemImportacao { get; set; }
        public DbSet<CampanhaCalculadaMetaResultadoPessoaImportacao> CampanhaCalculadaMetaResultadoPessoaImportacao { get; set; }
        public DbSet<CampanhaCalculadaMetaResultadoRankingImportacao> CampanhaCalculadaMetaResultadoRankingImportacao { get; set; }
        public DbSet<CampanhaCalculadaVendeuGanhouGrupoItemImportacao> CampanhaCalculadaVendeuGanhouGrupoItemImportacao { get; set; }
        public DbSet<CampanhaCalculadaVendeuGanhouPessoaImportacao> CampanhaCalculadaVendeuGanhouPessoaImportacao { get; set; }
        public DbSet<CampanhaCalculadaVendeuGanhouRankingImportacao> CampanhaCalculadaVendeuGanhouRankingImportacao { get; set; }
        public DbSet<CampanhaConteudo> CampanhaConteudo { get; set; }
        public DbSet<CampanhaEstrutura> CampanhaEstrutura { get; set; }
        public DbSet<CampanhaFaixaAtingimento> CampanhaFaixaAtingimento { get; set; }
        public DbSet<CampanhaFaixaAtingimentoGrupoItem> CampanhaFaixaAtingimentoGrupoItems { get; set; }
        public DbSet<CampanhaFaixaAtingimentoGrupoItemImportacao> CampanhaFaixaAtingimentoGrupoItemImportacao { get; set; }
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
        //public DbSet<Log> Log { get; set; }
        public DbSet<LogErro> LogErro { get; set; }
        //public DbSet<LogPontuacao> LogPontuacao { get; set; }

        public DbSet<LogRaiz> LogRaiz { get; set; }
        public DbSet<LogAcao> LogAcao { get; set; }
        public DbSet<LogIntegracao> LogIntegracao { get; set; }
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ArquivoMap());
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
            modelBuilder.Configurations.Add(new LogIntegracaoMap());
            //modelBuilder.Configurations.Add(new LogPontuacaoMap());
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
        }
    }
}
