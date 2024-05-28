using System;
using System.Reflection;
using System.ComponentModel;

namespace GrupoLTM.WebSmart.Domain.Enums
{
    public class EnumDomain
    {
        public enum TipoArquivo
        {
            [Description("Estrutura")]
            Estrurura = 1,
            [Description("Crédito de Pontos")]
            CreditoPontos = 2,
            [Description("Apuração Vendeu Ganhou")]
            ApuracaoVendeuGanhou = 3,
            [Description("Participante em Lote")]
            ParticipanteLote = 4,
            [Description("Hierarquia")]
            Hierarquia = 5,
            [Description("Indicação")]
            Indicacao = 6,
            [Description("Apoio")]
            Apoio = 7,
            [Description("Consecutividade")]
            Consecutividade = 8,
            [Description("EnvioPontuacaoLive")]
            EnvioPontuacaoLive = 9,
            [Description("RetornoPontuacaoLive")]
            RetornoPontuacaoLive = 10,
            [Description("ResgatesGerais")]
            ResgatesGerais = 11,
            [Description("ResgatesExclusivosAvon")]
            ResgatesExclusivosAvon = 12,
            [Description("SalesStructure")]
            SalesStructure = 13,
            [Description("SMSAgendamentoBaseRA")]
            SMSAgendamentoBaseRA = 14,
            [Description("Resgate Offline")]
            ResgateOffline = 15,
            [Description("Envio Resgate Primeiro Acesso")]
            EnvioResgateOffLine = 16,
            [Description("Clube das Estrelas")]
            ClubeDasEstrelas = 17,
            [Description("Forçar Primeiro Acesso")]
            ForcarPrimeiroAcesso = 18,
            [Description("Migração")]
            Migracao = 19,
            [Description("Natal")]
            Natal = 20,
            [Description("Alianca")]
            Alianca = 21
        }

        public enum StatusArquivo
        {
            [Description("Não Definido")]
            NaoDefinido = 0,
            [Description("Primeiro momento, o arquivo é incluído no SFTP, e o processo do ServiceBus consome essa arquivo, armazenando ele com o status Enviado")]
            Enviado = 1,
            [Description("Ocorre quando o arquivo não possui linhas, a coluna de Pontos estiver fora do padrão ou ocorre alguma exceção no carregamento da pontuação.")]
            ErroNaoImportado = 2,
            [Description("Ocorre quando o processamento de pontos dá algum erro de layout")]
            ErroDadosInvalidos = 3,
            [Description("Ocorre quando o arquivo e as pontuações foram processadas com sucesso, o arquivo fica aguardando aprovação na tela de aprovação de pontos do admin")]
            Processado = 4,
            [Description("Ocorre quando o processo de Envio de pontos do Live, consome os arquivos com status Sucesso(Processado), gerando os arquivos de pontos")]
            PendenteEnvioFtpLive = 5,
            [Description("Após a geração de arquivos de pontos e envio para o Live, o arquivo de Pendente de Envio, fica Pendente de Retorno")]
            PendenteRetornoFtpLive = 6,
            [Description("O processo que valida o retorno do arquivo do Live, sinaliza com este status, caso tenha algum erro no arquivo de crédito")]
            ErroRetornoFtpLive = 7,
            [Description("O processo que valida o retorno do arquivo do Live, sinaliza com este status, caso não tenha nenhum erro no arquivo de crédito")]
            SucessoRetornoFtpLive = 8,
            [Description("Ocorre quando o administrador do sistema na Avon, aprova as pontuações do arquivo de pontos")]
            Aprovado = 9,
            [Description("Ocorre quando o administrador do sistema na Avon, reprova as pontuações do arquivo de pontos por ter alguma inconsistência no resumo de pontos ou por que eles mesmos incluíram um arquivo errado no sftp")]
            Reprovado = 10,
            [Description("Ocorre quando aconteceu alguma exceção de sistema, banco de dados ou redes, durante o processo de geração dos pontos ou envio para o Live")]
            Inconsistente = 11
        }

        public enum StatusTransactionTemp
        {
            [Description("Aguardando Aprovação")]
            AguardandoAprovacao = 1,
            [Description("Aprovado")]
            Aprovado = 2,
            [Description("Processado")]
            Processado = 3,
            [Description("Reprovado")]
            Reprovado = 4,
            [Description("Processado com erros")]
            ProcessadoComErros = 5,
            [Description("Estornado")]
            Estornado = 6,
            [Description("Processando")]
            Processando = 7
        }

        public enum Perfis
        {
            Administrador = 1,
            Aluno = 2,
            Revendedoras = 3
        }

        public enum TipoModulo
        {
            Menu = 1,
            Noticias = 2,
            Videos = 3,
            Downloads = 4,
            Fotos = 5,
            Banners = 6,
            Background = 7,
            Mecanica = 8,
            Regulamento = 9,
        }

        public enum ModuloFixo
        {
            Menu = 1,
            Noticias = 2,
            Videos = 3,
            Downloads = 4,
            Fotos = 5,
            Banners = 6,
            Background = 7,
            Mecanica = 8,
            Regulamento = 9
        }

        public enum TipoQuestionario
        {
            [Description("Pesquisa de satisfação")]
            PesquisaDeSatisfação = 1,
            [Description("Quiz")]
            Quiz = 2,
            [Description("Faq")]
            Faq = 3,
        }

        public enum TipoResposta
        {
            [Description("Múltipla Escolha")]
            MultiplaEscolha = 1,
            [Description("Única Escolha")]
            UnicaEscolha = 2,
            [Description("Aberta")]
            Aberta = 3,
        }

        public enum LogTipo
        {
            [Description("Admin")]
            Admin = 1,
            [Description("Site")]
            Site = 2,
            [Description("Api")]
            Api = 3
        }

        public enum StatusArquivoDescricao
        {
            [Description("Não Definido")]
            NaoDefinido = 0,
            [Description("Enviado")]
            Enviado = 1,
            [Description("Erro de importação")]
            ErroNaoImportado = 2,
            [Description("Erro de layout")]
            ErroDadosInvalidos = 3,
            [Description("Processado")]
            Processado = 4,
            [Description("Pendente Envio Live")]
            PendenteEnvioFtpLive = 5,
            [Description("Pendente de Retorno")]
            PendenteRetornoFtpLive = 6,
            [Description("Erro retorno de crédito")]
            ErroRetornoFtpLive = 7,
            [Description("Sucesso retorno de crédito")]
            SucessoRetornoFtpLive = 8,
            [Description("Aprovado")]
            Aprovado = 9,
            [Description("Reprovado")]
            Reprovado = 10,
            [Description("Inconsistente")]
            Inconsistente = 11
        }

        public static string GetDescription(Enum input)
        {
            Type type = input.GetType();
            MemberInfo[] memInfo = type.GetMember(input.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = (object[])memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return input.ToString();
        }
        public enum StatusParticipante
        {
            [Description("Pré-Cadastrado")]
            PreCadastrado = 1,
            [Description("Ativo")]
            Ativo = 2,
            [Description("Inativo")]
            Inativo = 3,
            [Description("Desligado")]
            Desligado = 4,
        }

        public enum TipoAcesso
        {
            [Description("PF")]
            PF = 1,
            [Description("PJ")]
            PJ = 2,
            [Description("PF/PJ")]
            PFPJ = 3,
        }

        public enum TipoCadastro
        {
            [Description("Aberto")]
            Aberto = 1,
            [Description("Fechado")]
            Fechado = 2,
        }

        public enum TipoValidacaoPositiva
        {
            [Description("CPF")]
            CPF = 1,
            [Description("CNPJ (1 Campo)")]
            CNPJ1Campo = 2,
            [Description("CPF ou CNPJ (1 Campo)")]
            CPFouCNPJ1Campo = 3,
            [Description("Código cliente  (1 Campo)")]
            Codigocliente1Campo = 4,
            [Description("Email  (1 Campo)")]
            Email1Campo = 5,
            [Description("CPF e Data Nascimento (2 Campos)")]
            CPFeDataNascimento2Campos = 6,
            [Description("CNPJ e CPF (2 Campos)")]
            CNPJeCPF2Campos = 7,
            [Description("Codigo e CPF (2 Campos)")]
            CodigoeCPF2Campos = 8,
            [Description("Codigo e CNPJ (2 Campos)")]
            CodigoeCNPJ2Campos = 9
        }

        public enum TipoPontuacao
        {
            [Description("Crédido de Pontos")]
            CreditoArquivo = 1,
            [Description("Quizz")]
            Quizz = 2,
            [Description("Campanha")]
            Campanha = 3
        }

        public enum StatusPontuacao
        {
            [Description("Pendente Envio")]
            PendenteEnvio = 1,
            [Description("Enviado")]
            Enviado = 2,
            [Description("Creditada")]
            Creditada = 3,
            [Description("Reprovada")]
            Reprovada = 4,
            [Description("Excluída")]
            Excluida = 5,
            [Description("Pendente Retorno Ftp Live")]
            PendenteRetornoFtpLive = 7,
            [Description("Rejeitada")]
            Rejeitada = 8,
            [Description("Aguardando Aprovação")]
            AguardandoAprovacao = 9,
            [Description("Aguardando Processamento")]
            AguardandoProcessamento = 10,
            [Description("Em processamento")]
            EmProcessamento = 11,
            [Description("Criada")]
            Criada = 12,
            [Description("Validada")]
            Validada = 13,
            [Description("Participantes criados")]
            ParticipantesCriados = 14,
            [Description("Cancelada")]
            Cancelada = 15,
            [Description("Processada com erros")]
            ProcessadaComErros = 16
        }

        public enum BankLotProcessStatus
        {
            [Description("Pontuação pendente de processamento")]
            ReadyToProcess = 1,
            [Description("Pontuação em processamento")]
            Processing = 2,
            [Description("Pontuação processada com sucesso")]
            Processed = 3,
            [Description("Pontuação processada com erros")]
            ProcessedWithErrors = 4,
            [Description("Pontuação processada parcialmente")]
            PartiallyProcessed = 5,
            [Description("Origem não tem saldo suficiente para processar todo o lote")]
            InsufficientOriginBalance = 6,
            [Description("Ocorreu um erro inesperado")]
            InternalError = 7,
            [Description("Pontuação pendente de processamento")]
            Created = 8,
            [Description("Pontuação cancelada")]
            Canceled = 9,
            [Description("Pontuação validada com sucesso")]
            Validated = 10,
            [Description("Participantes criados com sucesso")]
            ParticipantsCreated = 11,
            [Description("Está sob análise de Estorno")]
            UnderReversalReview = 12,
            [Description("Pontuação em processamento de reversão")]
            ReversalProcessing = 13,
            [Description("Pontuação revertida com sucesso")]
            Reversed = 14,
            [Description("Pontuação revertida com erros")]
            ReversedWithErrors = 15,
        }

        public enum Meses
        {
            [Description("Janeiro")]
            Janeiro = 1,
            [Description("Fevereiro")]
            Fevereiro = 2,
            [Description("Março")]
            Marco = 3,
            [Description("Abril")]
            Abril = 4,
            [Description("Maio")]
            Maio = 5,
            [Description("Junho")]
            Junho = 6,
            [Description("Julho")]
            Julho = 7,
            [Description("Agosto")]
            Agosto = 8,
            [Description("Setembro")]
            Setembro = 9,
            [Description("Outubro")]
            Outubro = 10,
            [Description("Novembro")]
            Novembro = 11,
            [Description("Dezembro")]
            Dezembro = 12
        }

        public enum Menu
        {
            MenuAdmin = 1,
            MenuPrincipal = 2,
            MenuRodape = 44,
            MenuSuperior = 46,
            MenuRedeSocial = 47
        }
        public enum StatusFaleConosco
        {
            Pendente = 1,
            Respondido = 2
        }

        public enum LayoutResultadoCampanha
        {
            [Description("VendeuGanhouPessoa")]
            VendeuGanhouPessoa = 1,
            [Description("VendeuGanhouItem")]
            VendeuGanhouItem = 2,
            [Description("VendeuGanhouRanking")]
            VendeuGanhouRanking = 3,
            [Description("MetaResultadoPessoa")]
            MetaResultadoPessoa = 4,
            [Description("MetaResultadoItem")]
            MetaResultadoItem = 5,
            [Description("MetaResultadoRanking")]
            MetaResultadoRanking = 6
        }

        public enum StatusCampanha
        {
            [Description("Em configuração")]
            EmConfiguracao = 1,
            [Description("Publicada")]
            Publicada = 2,
            [Description("Cancelada")]
            Cancelada = 3
        }

        public enum PassosCampanha
        {
            [Description("Dados Campanha")]
            Passo1 = 1,
            [Description("Seleção Estrutura")]
            Passo2 = 2,
            [Description("Seleção Perfil")]
            Passo3 = 3,
            [Description("Banner")]
            Passo4 = 4,
            [Description("")]
            Passo5 = 5,
            [Description("")]
            Passo6 = 6
        }

        public enum TipoCampanha
        {
            [Description("Meta e Resultado - Meta Por Pessoa")]
            MetaEResultadoParticipante = 1,
            [Description("Meta e Resultado - Ranking Por Pessoa")]
            MetaEResultadoRankingParticipante = 2,
            [Description("Vendeu Ganhou")]
            VendeuGanhou = 3,
            [Description("Vendeu Ganhou - Ranking Por Pessoa")]
            VendeuGanhouRanking = 4,
            [Description("Meta e Resultado - Meta Por Item")]
            MetaEResultadoItens = 5,

            [Description("Meta e Resultado - Calculado Por Pessoa")]
            MetaEResultadoPorPessoaCalculado = 8,
            [Description("Meta e Resultado - Calculado Ranking Por Pessoa")]
            MetaEResultadoRankingPorPessoaCalculado = 9,
            [Description("Meta e Resultado - Calculado Por Item")]
            MetaResultadoPorItemCalculado = 12,

            [Description("Vendeu Ganhou - Calculado Por Pessoa")]
            VendeuGanhouPorPessoaCalculado = 10,
            [Description("Vendeu Ganhou - Calculado Ranking Por Pessoa")]
            VendeuGanhouRankingPorPessoaCalculado = 11,
            [Description("Vendeu Ganhou - Calculado Por Item")]
            VendeuGanhouPorItemPorItemCalculado = 14,
        }

        public enum TipoConteudo
        {
            [Description("Banner Home")]
            BannerHome = 1,
            [Description("Imagem Mecânica")]
            ImagemMecanica = 2,
            [Description("Imagem Mecânica Mobile")]
            ImagemMecanicaMobile = 3,
            [Description("Regulamento")]
            Regulamento = 4
        }

        public enum Passo
        {
            Passo1 = 1,
            Passo2 = 2,
            Passo3 = 3,
            Passo4 = 4,
            Passo5 = 5
        }

        public enum TipoIconeSimulador
        {
            Mecanica = 1
        }

        public enum DisparoEmailTipo
        {
            [Description("E-mail Participante Boas Vindas")]
            EmailParticipanteBoasVindas = 1
        }

        public enum ELogTipo : int
        {
            [Description("Processamento")]
            Processamento = 1,
            [Description("Erro")]
            Erro = 2
        }

        public enum EEvento
        {
            Get_Avon,
            Get_Avon_User,
            Post_MktPlace,
            Update_User
        }
        public enum TipoMensagem
        {
            [Description("Sms")]
            sms = 0,
            [Description("WhatsApp")]
            whatsApp = 1,
        }

        public enum SMSTipoRecorrencia
        {
            [Description("Único")]
            Unico = 0,
            [Description("Diário")]
            Diario = 24,
            [Description("Semanal")]
            Semanal = 7 * 24,
            [Description("Mensal")]
            Mensal = 30 * 24
        }

        public enum SMSTipoBase
        {
            [Description("Dinâmica")]
            Dinamica = 0,
            [Description("Upload")]
            Upload = 1
        }

        public enum SMSAgendamentoStatus
        {
            [Description("Aguardando início")]
            AguardandoInicio = 0,

            [Description("Durante período de disparo")]
            DurantePeriodoDeDisparo = 1,

            [Description("Finalizado")]
            Finalizado = 2
        }

        public enum SMSExecucaoStatus
        {
            [Description("Executando")]
            Executando = 1,
            [Description("Sucesso")]
            Sucesso = 3,
            [Description("Erro")]
            Falha = 4
        }
        public enum SMSStatusCodeMovile
        {
            [Description("Enviado para Disparador")] //Enviado para o Disparador
            EnviadoDisparador = 99,
            [Description("Entregue na Operadora")] //Entregue na Operadora
            entregue = 2,
            [Description("Entregue com Sucesso")] //Entregue com Sucesso
            DELIVERED_SUCCESS = 4,
            [Description("Lido com Sucesso")] //Lido com Sucesso
            READ_SUCCESS = 5,
            [Description("Expirado antes de ser entregue")] //Expirado antes de ser entregue
            expirado = 101,
            [Description("Erro de comunicação com a operadora")] //Erro de comunicação com a operadora
            falha = 102,
            [Description("Operadora rejeitou a mensagem")] //Operadora rejeitou a mensagem
            rejeitado = 103,
            [Description("Entrega com Erro")] //Entrega com Erro
            NOT_DELIVERED = 104,
            [Description("O limite de mensagens, foi execidido")] //O limite de mensagens, foi execidido
            execidido = 201,
            [Description("O número de destino é inválido")] //O número de destino é inválido
            invalido = 202,
            [Description("O número de destino está na lista bloqueada")] //O número de destino está na lista bloqueada
            blacklist = 203,
            [Description("O número de destino solicitou opt-out, e não quer receber mais mensagens desta sub conta")] //O número de destino solicitou opt-out, e não quer receber mais mensagens desta sub conta
            bloqueado = 204,
            [Description("O número de destino já recebeu a quantidade máxima de mensagens")] //O número de destino já recebeu a quantidade máxima de mensagens
            maxima = 205,
            [Description("O texto da mensagem contém palavras que não são aceitas pela operadora.")] //O texto da mensagem contém palavras que não são aceitas pela operadora.
            bloqueadoInvalido = 207,
            [Description("Ocorreu um erro na plataforma da Wavy.")] //Enviado para o Disparador
            error = 207,
        }
        public enum SMSStatusCode
        {
            [Description("OK")] //00 Ok
            OK = 0,
            [Description("Agendado")] //01 Scheduled
            Agendado = 1,
            [Description("Enviado")] //02 Sent
            Enviado = 2,
            [Description("Entregue com Confirmação")] //03 Delivered
            EntregueComConfirmacao = 3,
            [Description("Não recebido")] //04 Not Received
            NaoRecebido = 4,
            [Description("Bloqueado - sem cobertura")] //05 Blocked - No Coverage
            BloqueadoSemCobertura = 5,
            [Description("Bloqueado - Black List")]//06 Blocked - Black listed
            BloqueadoBlackList = 6,
            [Description("Bloqueado - Número Inválido")] //07 Blocked - Invalid Number
            BloqueadoNumeroInvalido = 7,
            [Description("Bloqueado - Conteúdo não permitido")] //08 Blocked - Content not allowed
            BloqueadoConteudoNaoPermitido = 8,
            [Description("Bloqueado")] //09	Blocked
            Bloqueado = 9,
            [Description("Status de Mensagem não encontrada")] //Error
            Error = 10,
            [Description("Enviado para Disparador")] //Enviado para o Disparador
            EnviadoDisparador = 99,
            [Description("Executando Envio")]
            Executando = 101,
            [Description("Processado com Sucesso")]
            Sucesso = 103,
            [Description("Erro no processamento")]
            Falha = 104,
            [Description("Não processado")]
            NaoProcessado = 105
        }

        public enum ClassificacaoPerfilAvon
        {
            [Description("1 Estrela")]
            UmaEstrela = 1,
            [Description("2 Estrelas")]
            DuasEstrelas = 2,
            [Description("3 Estrelas")]
            TresEstrelas = 3,
            [Description("4 Estrelas")]
            QuatroEstrelas = 4,
            [Description("5 Estrelas")]
            CincoEstrelas = 5
        }

        public enum EstornoStatus
        {
            [Description("Em Processo")]
            EmProcesso = 1,
            [Description("Processado")]
            Processado = 2,
            [Description("Inconsistente")]
            Inconsistente = 3,
            [Description("Aprovado")]
            Aprovado = 4,
            [Description("Reprovado")]
            Reprovado = 5,
            [Description("Processo de estorno")]
            EmProcessoLive = 6,
            [Description("Estorno realizado")]
            EstornoRealizadoSucesso = 7,
            [Description("Estornado com erros")]
            EstornoRealizadoErro = 8,
            [Description("Erro ao estornar")]
            EstornoErro = 9,
            [Description("Comunicação enviada")]
            ComunicacaoEnviada = 10,
            [Description("Erro ao comunicacar o participante")]
            ComunicacaoErro = 11,
        }

        public enum EnvioSmsStatus
        {
            NaoEnviar = 0,
            PendenteEnvio = 1,
            EnviadoSucesso = 2,
            EnviadoErro = 3
        }

        public enum EnvioEmailStatus
        {
            NaoEnviar = 0,
            PendenteEnvio = 1,
            EnviadoSucesso = 2,
            EnviadoErro = 3
        }

        public enum SendStatusMovile
        {
            [Description("Número de telefone não cadastrado")]
            EMPTY_DESTINATION_NUMBER = 0,

            [Description("Enviado com sucesso a operadora")]
            SENT_SUCCESS = 2,

            [Description("Expirado antes de ser enviado para o aparelho")]
            EXPIRED = 101,

            [Description("Erro de comunicação com a operadora")]
            CARRIER_COMMUNICATION_ERROR = 102,

            [Description("A operadora rejeitou a mensagem")]
            REJECTED_BY_CARRIER = 103,

            [Description("O limite de envio de mensagens foi atingido")]
            NO_CREDIT = 201,

            [Description("O número de destino é inválido")]
            INVALID_DESTINATION_NUMBER = 202,

            [Description("O número de destino está na lista negra")]
            BLACKLISTED = 203,

            [Description("O número de destino optou por não receber suas mensagens")]
            DESTINATION_BLOCKED_BY_OPTOUT = 204,

            [Description("O número de destino já recebeu a quantidade máxima de mensagens")]
            DESTINATION_MESSAGE_LIMIT_REACHED = 205,

            [Description("O texto da mensagem contém palavras que não são aceitas pela operadora")]
            INVALID_MESSAGE_TEXT = 207,

            [Description("Houve um erro interno na plataforma do Movile")]
            INTERNAL_ERROR = 301,

            [Description("Houve um erro interno na aplicação")]
            INTERNAL_ERROR_APPLICATION = 601
        }

        public enum DeliveredStatusMovile
        {
            [Description("Entregue com sucesso no dispositivo")]
            DELIVERED_SUCCESS = 4,

            [Description("A operadora aceitou a mensagem, mas não foi capaz de entregá-la ao dispositivo")]
            NOT_DELIVERED = 104,
        }

        public enum EstornoTipo
        {
            Quantidade = 1,
            Valor = 2
        }

        public enum EstornoMotivo
        {
            [Description("Erro no Serviço do Parceiro")]
            ErroServicoParceiro = 1,
            [Description("Pagamento recusado")]
            PagamentoRecusado = 2,
            [Description("Produto não entregue")]
            ProdutoNaoEntregue = 3,
            [Description("Cancelamento de Pedido")]
            CancelamentoPedido = 4,
            [Description("Produto danificado")]
            ProdutoDanificado = 5,
            [Description("Mudou-se")]
            Mudou = 6,
            [Description("Ausente")]
            Ausente = 7,
            [Description("Não procurado")]
            NãoProcurado = 8,
            [Description("Recusado")]
            Recusado = 9,
            [Description("Não existe o número indicado")]
            NãoExisteNumeroIndicado = 10,
            [Description("Desconhecido")]
            Desconhecido = 11,
            [Description("Endereço insuficiente")]
            EnderecoInsuficiente = 12,
            [Description("Indisponibilidade")]
            Indisponibilidade = 13,
            [Description("Estorno de frete")]
            EstornoDeFrete = 14,
            [Description("Pedido Teste")]
            PedidoTeste = 15,
            [Description("Arrependimento do Cliente")]
            ArrependimentoCliente = 16,
            [Description("Pedido Bloqueado")]
            PedidoBloqueado = 17,
            [Description("Pedido Extraviado")]
            PedidoExtraviado = 18,
            [Description("Pedido não gerado")]
            PedidoNaoGerado = 19,
            [Description("Desconto no Produto")]
            DescontoProduto = 20,
            [Description("Automático")]
            Automatico = 21,
            [Description("Pedido não efetivado")]
            PedidoNaoEfetivado = 22,
            [Description("Reclamação não atendida dentro do prazo pelo parceiro")]
            ReclamaçãoNaoAtendidaNoPraoParceiro = 23,
            [Description("Dificuldade na região")]
            DificuldadeRegião = 24,
            [Description("Frete indevido")]
            FreteIndevido = 25,
            [Description("Email inválido")]
            EmailInvalido = 26,
            [Description("Destinatario ausente")]
            DestinatarioAusente = 27,
            [Description("Pedido roubado")]
            PedidoRoubado = 28,
            [Description("Não atende a região")]
            NãoAtendeRegiao = 31,
            [Description("Não retirou o produto nos Correios")]
            NaoRetirouProdutoCorreios = 32,
            [Description("Endereço não localizado")]
            EnderecoNaoLocalizado = 33,
            [Description("Pedido não gerado no parceiro")]
            PedidoNaoGeradoParceiro = 35,
            [Description("Pedido não processado pelo parceiro")]
            PedidoNaoProcessadoParceiro = 36,
            [Description("Estorno de frete")]
            EstornoFrete = 39,
            [Description("Não cumprimento do SLA")]
            NaoCumprimentoSLA = 42,
            [Description("Pedido avariado")]
            PedidoAvariado = 43,
            [Description("Desconto no produto")]
            DescontoNoProduto = 46,
            [Description("Pedido não migrado")]
            PedidoNaoMigrado = 47,
            [Description("Erro no POS")]
            ErroPOS = 48,
            [Description("Pontuação expirada")]
            PontuacaoExpirada = 50,
        }
        
        public enum TipoBanner
        {
            [Description("Banner1")]
            Banner1 = 1,

            [Description("Banner2")]
            Banner2 = 2,

            [Description("Banner3")]
            Banner3 = 3,

            [Description("Banner4")]
            Banner4 = 4,

            [Description("Banner5")]
            Banner5 = 5,

            [Description("Banner6")]
            Banner6 = 6,

            [Description("Banner7")]
            Banner7 = 7,

            [Description("Banner8")]
            Banner8 = 8,

            [Description("Banner9")]
            Banner9 = 9,

            [Description("Card1")]
            Card1 = 10,

            [Description("Card2")]
            Card2 = 11,

            [Description("Card3")]
            Card3 = 12,

            [Description("Card4")]
            Card4 = 13,

            [Description("Card5")]
            Card5 = 14,
        }

        public enum TipoDominio
        {
            [Description("Catálogo")]
            Catalogo = 1,

            [Description("Site Avon")]
            SiteAvon = 2,

            [Description("Avon Comigo")]
            AvonComigo = 3,
            
            [Description("Youtube")]
            Youtube = 4,
        }
    }

    public class EnumMktPlace
    {
        public enum ParticipantStatus
        {
            Active = 1,
            SignPendant = 2,
            Inactive = 3,
            Wait = 4,
            PendingValidation = 5,
            PendingRegistration = 6,
            PendingPayment = 7,
            CancelledCard = 8,
            StandByWithoutPrize = 9,
            PasswordSignPendant = 10,
        }

        public enum LogonType : int
        {
            CPF = 1,
            WpCard = 2,
            Email = 3,
            CNPJ = 4,
            RegistrationRe = 5,
            Bankcard = 6,
            PdvCode = 7,
            Dossier = 8
        }

        public enum MaritalStatusModelType
        {
            Casado = 1,
            Divorciado = 2,
            Solteiro = 3,
            Viuvo = 4,
            Separado = 5,
            NaoInformado = 6
        }

        public enum PersonType
        {
            Individual = 1,
            Company = 2
        }

        public enum GenderType
        {
            Uninformed = 0,
            Male = 1,
            Female = 2
        }

        public enum PhoneType
        {
            Home = 1,
            Commercial = 2,
            Cellular = 3
        }

        public enum EmailModelType
        {
            Personal = 1,
            Commercial = 2
        }

        public enum FatorConversaoTipoValor
        {
            Quantidade = 0,
            Real = 1
        }


        public enum FatorConversaoTipoConversao
        {
            Multiplicador = 0,
            Range = 1
        }

        public enum FatorConversaoTipoComparacao
        {
            Exato = 0,
            Minimo = 1,
        }


        public enum ExpirationInterval
        {
            [Description("nos próximos 7 dias")]
            Proximos7Dias = 0,
            [Description("nos próximos 15 dias")]
            Proximos15Dias = 1,
            [Description("nos próximos 30 dias")]
            Proximos30Dias = 2,
            [Description("nos próximos 60 dias")]
            Proximos60Dias = 3,
            [Description("nos próximos 90 dias")]
            Proximos90Dias = 4,
            [Description("nos próximos 6 meses")]
            Proximos6Meses = 5,
            [Description("no próximo 1 ano")]
            Proximo1Ano = 6
        }

        public enum EnumStatusTransactionTemp
        {

            AguardandoAprovacao = 1,
            Aprovado = 2,
            Processado = 3,
            Reprovado = 4,
            ProcessadoComErros = 5,
            Estornado = 6,
            Processando = 7
        }

        public enum CreditOrigin
        {
            AprovacaoServiceNow = 1,
            SemAprovacaoServiceNow = 4
        }

        public enum OriginAccountHolder
        {
            [Description("OriginAccountHolderId")]
            OriginAccountHolderId = 2016546059
        }
    }
}