using System;
using System.Data.Entity.Migrations;
using GrupoLTM.WebSmart.Domain.Models;

namespace GrupoLTM.WebSmart.Domain.Repository.Configuration
{
    internal sealed class Configuration : DbMigrationsConfiguration<AvonDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = false;
        }


        protected override void Seed(AvonDbContext context)
        {
            //SeedTipoAcesso(context);
            //SeedTipoCadastro(context);
            //SeedTipoValidacaoPositiva(context);
            //SeedTema(context);
            //SeedConfiguracaoCampanha(context);
            //Perfil(context);
            //Estado(context);
            //StatusCampanha(context);
            //StatusFaleConosco(context);
            //Assunto(context);
            //StatusPontuacao(context);
            //TipoArquivo(context);
            //TipoCampanha(context);
            //TipoModulo(context);
            //Modulo(context);
            //TipoPontuacao(context);
            //TipoQuestionario(context);
            //TipoResposta(context);
            //TipoValidacaoPositiva(context);
            //UsuarioAdm(context);
            //StatusParticipante(context);
            //EstruturaTipo(context);
            //Estrutura(context);
            //Paticipante(context);
            //ParticipantEstrutura(context);
            //ParticipantPerfil(context);
        }

        private static void Modulo(AvonDbContext context)
        {
            context.Modulo.Add(
            new Modulo{

                Id = 6,
                ModuloPaiId = null,
                TipoModuloId = 6,
                Nome = "Banners",
                Ativo = true,
                DataInicio = DateTime.Now.AddDays(-1),
                DataFim = DateTime.Now.AddMonths(1),
                DataAlteracao = DateTime.Now,
                DataInclusao = DateTime.Now,
            });
        }

        private static void ParticipantEstrutura(AvonDbContext context)
        {
            context.ParticipanteEstrutura.AddOrUpdate(x => x.Id,
                new ParticipanteEstrutura
                {
                    Id = 1,
                    EstruturaId = 1,
                    ParticipanteId = 1,
                    PeriodoId = null,
                    Ativo = true,
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                });
        }

        private static void ParticipantPerfil(AvonDbContext context)
        {
            context.ParticipantePerfil.AddOrUpdate(x => x.Id,
                new ParticipantePerfil
            {
                Id = 1,
                PerfilId = 1,
                ParticipanteId = 1,
                Ativo = true,
                DataInclusao = DateTime.Now,
                DataAlteracao = DateTime.Now

            });
        }

        private static void EstruturaTipo(AvonDbContext context)
        {
            context.TipoEstrutura.AddOrUpdate(x => x.Id,
            new TipoEstrutura
            {
                Id = 1,
                Nome = "EPS",
                Ativo = false,
                DataInclusao = DateTime.Now,
                DataAlteracao = DateTime.Now
            },
            new TipoEstrutura
            {
                Id = 2,
                Nome = "Parceiros",
                Ativo = false,
                DataInclusao = DateTime.Now,
                DataAlteracao = DateTime.Now

            },
            new TipoEstrutura
            {
                Id = 3,
                Nome = "Empresa",
                Ativo = true,
                DataInclusao = DateTime.Now,
                DataAlteracao = DateTime.Now

            },
            new TipoEstrutura
            {
                Id = 4,
                Nome = "Área",
                Ativo = true,
                DataInclusao = DateTime.Now,
                DataAlteracao = DateTime.Now

            });
        }

        private static void Estrutura(AvonDbContext context)
        {
            context.Estrutura.AddOrUpdate(x => x.Id,
                new Estrutura
                {
                    Id = 1,
                    PaiId = null,
                    PeriodoId = null,
                    Nome = "LTM",
                    Tipo = string.Empty,
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now,
                    Ativo = true,
                    TipoEstruturaId = 3
                }
                ,
                new Estrutura
                {
                    Id = 2,
                    PaiId = 1,
                    PeriodoId = null,
                    Nome = "TI B2B",
                    Tipo = string.Empty,
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now,
                    Ativo = true,
                    TipoEstruturaId = 3
                },
                new Estrutura
                {
                    Id = 3,
                    PaiId = 1,
                    PeriodoId = null,
                    Nome = "TI B2C",
                    Tipo = string.Empty,
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now,
                    Ativo = true,
                    TipoEstruturaId = 3
                }
                );
        }

        private static void Paticipante(AvonDbContext context)
        {
            context.Participante.AddOrUpdate(x => x.Id,
                new Participante
                {
                    Id = 1,
                    Login = "llima",
                    Senha = "65945CC903AA889797543092B9E9D3B3",
                    StatusId = 2,
                    Nome = "Luiz Lima",
                    RazaoSocial = null,
                    NomeFantasia = null,
                    CNPJ = null,
                    CPF = "29502818857",
                    RG = "32197850x",
                    Sexo = "M",
                    DataNascimento = DateTime.Now,
                    Endereco = "Alameda Rio Negro",
                    Numero = "585",
                    Complemento = null,
                    Bairro = "Alphaville Industrial",
                    CEP = "06454000",
                    Cidade = "Barueri",
                    EstadoId = 26,
                    DDDCel = "11",
                    Celular = "999991111",
                    DDDTel = "11",
                    Telefone = "11238761",
                    DDDTelComercial = "11",
                    TelefoneComercial = "11244098",
                    Email = "luiz.lima@fcamara.com.br",
                    Ativo = true,
                    DataDesligamento = null,
                    DataInclusao = DateTime.Now,
                    DataAlteracao = null,
                    ParticipanteTeste = null,
                    ParticipanteVago = null,
                    OptInEmail = true,
                    OptInComunicacaoFisica = true,
                    OptInSms = true,
                    OptinAceite = true
                });
        }

        private static void StatusParticipante(AvonDbContext context)
        {
            context.StatusParticipante.AddOrUpdate(x => x.Id,
                new StatusParticipante
                {
                    Id = 1,
                    Nome = "Pré-Cadastrado",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new StatusParticipante
                {
                    Id = 2,
                    Nome = "Ativo",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new StatusParticipante
                {
                    Id = 3,
                    Nome = "Inativo",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new StatusParticipante
                {
                    Id = 4,
                    Nome = "Desligado",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                });
        }

        private static void SeedConfiguracaoCampanha(AvonDbContext context)
        {
            context.ConfiguracaoCampanha.AddOrUpdate(new ConfiguracaoCampanha
            {
                NomeCampanha = "Somos LTM",
                TipoAcessoId = 1,
                TipoCadastroId = 1,
                TipoValidacaoPositivaId = 4,
                AtivoWP = true,
                AtivoBoxSaldo = true,
                AtivoBoxVitrine = true,
                LIVEAPI_ENDPOINT = "http://azu-ltmlive.cloudapp.net/service/token",
                LIVEAPI_URL = "http://azu-ltmlive.cloudapp.net/service/",
                LIVEAPI_USERNAME = "webprovider",
                LIVEAPI_PASSWORD = "webprovider@123",
                LIVE_PROJECTCONFIGURATIONID = "1",
                LIVEAPI_CLIENTEID = "1",
                LIVEAPI_PROJECTID = "1",
                EXLOGIN = "api@b2b",
                EXSENHA = "WEBPROVIDER@10",
                EXTEMPLATE_KEYBOASVINDAS = "ExBoas",
                EXTEMPLATE_KEYESQUECISENHA = "JBS_Esqueci_Minha_Senha",
                EXTEMPLATE_KEYFALECONOSCO = "ExFale",
                EMAILCREDITOPONTOS = "amalheiros@grupoltm.com.br",
                EMAILFALECONOSCO = "rcoelho@grupoltm.com.br",
                GOOGLEANALITYCS = @"<script type='text/javascript'></script>",
                AtivoEsqueciSenhaSMS = true,
                SMSLOGIN = "cielo.api",
                SMSSENHA = "VyqdQzlfzV",
                DataInclusao = DateTime.Now,
                DataAlteracao = null,
                AtivoTema = false,
                ImgLogoCampanha = "11042016_095913.png",
                TemaId = 6,
                InstrucaoFaleConosco = @"</br><strong>0800 730 1000</strong></br>De segunda a sexta-feira, das 9 h às 19 h.",
                LIVE_URLCatalogo = "https://ltmlive.webpremios.com.br",
                EXTEMPLATE_KEYCadastroUsuarioAdm = "1389"
            });
            context.SaveChanges();
        }

        private static void SeedTipoCadastro(AvonDbContext context)
        {
            context.TipoCadastro.AddOrUpdate(cadastro => cadastro.Id,
                new TipoCadastro
                {
                    Id = 1,
                    Nome = "Aberto",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoCadastro
                {
                    Id = 2,
                    Nome = "Fechado",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                });
        }

        private static void SeedTipoAcesso(AvonDbContext context)
        {
            context.TipoAcesso.AddOrUpdate(x => x.Id,
                new TipoAcesso
                {
                    Id = 1,
                    Nome = "PF",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoAcesso
                {
                    Id = 2,
                    Nome = "PJ",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoAcesso
                {
                    Id = 3,
                    Nome = "PF/PJ",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                }
                );

            context.SaveChanges();
        }

        private static void SeedTipoValidacaoPositiva(AvonDbContext context)
        {
            context.TipoValidacaoPositiva.AddOrUpdate(tipoValidacaoPositiva => tipoValidacaoPositiva.Id,
                new TipoValidacaoPositiva
                {
                    Id = 1,
                    Nome = "CPF",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoValidacaoPositiva
                {
                    Id = 2,
                    Nome = "CNPJ (1 Campo)",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                }, new TipoValidacaoPositiva
                {
                    Id = 3,
                    Nome = "CPF ou CNPJ (1 Campo)",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoValidacaoPositiva
                {
                    Id = 4,
                    Nome = "Código cliente  (1 Campo)",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoValidacaoPositiva
                {
                    Id = 5,
                    Nome = "Email  (1 Campo)",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoValidacaoPositiva
                {
                    Id = 6,
                    Nome = "CPF e Data Nascimento (2 Campos)",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoValidacaoPositiva
                {
                    Id = 7,
                    Nome = "CNPJ e CPF (2 Campos)",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },

                new TipoValidacaoPositiva
                {
                    Id = 8,
                    Nome = "Codigo e CNPJ (2 Campos)",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                }
                );
        }

        private static void SeedTema(AvonDbContext context)
        {
            context.Tema.AddOrUpdate(tema => tema.Id, new Tema
            {
                Id = 6,
                Nome = "Azul",
                Cor = "#003F77",
                ArquivoCSS = "skin-azul.css",
                DataInclusao = DateTime.Now,
                DataAlteracao = DateTime.Now,

            }, new Tema
            {
                Id = 7,
                Nome = "Cinza",
                Cor = "#2E2E2E",
                ArquivoCSS = "skin-cinza.css",
                DataInclusao = DateTime.Now,
                DataAlteracao = DateTime.Now,

            }, new Tema
            {
                Id = 8,
                Nome = "Laranja",
                Cor = "#E87E04",
                ArquivoCSS = "skin-laranja.css",
                DataInclusao = DateTime.Now,
                DataAlteracao = DateTime.Now,

            }, new Tema
            {
                Id = 9,
                Nome = "Verde",
                Cor = "#009000",
                ArquivoCSS = "skin-verde.css",
                DataInclusao = DateTime.Now,
                DataAlteracao = DateTime.Now,

            }, new Tema
            {
                Id = 10,
                Nome = "Vermelho",
                Cor = "#AB0000",
                ArquivoCSS = "skin-vermelho.css",
                DataInclusao = DateTime.Now,
                DataAlteracao = DateTime.Now,

            }, new Tema
            {
                Id = 12,
                Nome = "Roxo",
                Cor = "#951C7B",
                ArquivoCSS = "skin-roxo.css",
                DataInclusao = DateTime.Now,
                DataAlteracao = DateTime.Now,

            }, new Tema
            {
                Id = 13,
                Nome = "Amarelo",
                Cor = "#FFFF00",
                ArquivoCSS = "skin-amarelo.css",
                DataInclusao = DateTime.Now,
                DataAlteracao = DateTime.Now,

            });
        }

        private static void Perfil(AvonDbContext context)
        {
            context.Perfil.AddOrUpdate(x => x.Id,
                new Perfil
                {
                    Id = 1,
                    PaiId = null,
                    Nome = "Administrador",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now,
                    Adm = true,
                    NivelHierarquia = null,
                    Ativo = true
                });
        }

        private static void Estado(AvonDbContext context)
        {
            context.Estado.AddOrUpdate(x => x.EstadoId,
                new Estado
                {
                    EstadoId = 1,
                    Nome = "AC",
                    Capital = "Rio Branco",
                    Regiao = "Norte",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new Estado
                {
                    EstadoId = 2,
                    Nome = "AL",
                    Capital = "Maceió",
                    Regiao = "Nordeste",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new Estado
                {
                    EstadoId = 3,
                    Nome = "AP",
                    Capital = "Macapá",
                    Regiao = "Norte",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new Estado
                {
                    EstadoId = 4,
                    Nome = "AM",
                    Capital = "Manaus",
                    Regiao = "Norte",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new Estado
                {
                    EstadoId = 5,
                    Nome = "BA",
                    Capital = "Salvador",
                    Regiao = "Nordeste",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new Estado
                {
                    EstadoId = 6,
                    Nome = "CE",
                    Capital = "Fortaleza",
                    Regiao = "Nordeste",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new Estado
                {
                    EstadoId = 7,
                    Nome = "DF",
                    Capital = "Centro-Oeste",
                    Regiao = "Brasília",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new Estado
                {
                    EstadoId = 8,
                    Nome = "ES",
                    Capital = "Vitória",
                    Regiao = "",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new Estado
                {
                    EstadoId = 9,
                    Nome = "GO",
                    Capital = "Goiânia",
                    Regiao = "Centro-Oeste",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new Estado
                {
                    EstadoId = 10,
                    Nome = "MA",
                    Capital = "São Luís",
                    Regiao = "Nordeste",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now

                },
                new Estado
                {
                    EstadoId = 11,
                    Nome = "MT",
                    Capital = "Cuiabá",
                    Regiao = "Centro-Oeste",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now

                },
                new Estado
                {
                    EstadoId = 12,
                    Nome = "MS",
                    Capital = "Campo Grande",
                    Regiao = "Centro-Oeste",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now

                },
                new Estado
                {
                    EstadoId = 13,
                    Nome = "MG",
                    Capital = "Belo Horizonte",
                    Regiao = "Sudeste",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now

                },
                new Estado
                {
                    EstadoId = 14,
                    Nome = "PR",
                    Capital = "Curitiba",
                    Regiao = "Sul",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now

                },
                new Estado
                {
                    EstadoId = 15,
                    Nome = "PB",
                    Capital = "João Pessoa",
                    Regiao = "Nordeste",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now

                },
                new Estado
                {
                    EstadoId = 16,
                    Nome = "PA",
                    Capital = "Belém",
                    Regiao = "Norte",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now

                },
                new Estado
                {
                    EstadoId = 17,
                    Nome = "PE",
                    Capital = "Recife",
                    Regiao = "Nordeste",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now

                },
                new Estado
                {
                    EstadoId = 18,
                    Nome = "PI",
                    Capital = "Teresina",
                    Regiao = "",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now

                },
                new Estado
                {
                    EstadoId = 19,
                    Nome = "RJ",
                    Capital = "Rio de Janeiro",
                    Regiao = "Sudeste",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now

                },
                new Estado
                {
                    EstadoId = 20,
                    Nome = "RN",
                    Capital = "Natal",
                    Regiao = "Nordeste",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new Estado
                {
                    EstadoId = 21,
                    Nome = "RS",
                    Capital = "Porto Alegre",
                    Regiao = "Sul",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new Estado
                {
                    EstadoId = 22,
                    Nome = "RO",
                    Capital = "Porto Velho",
                    Regiao = "Norte",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now

                }, new Estado
                {
                    EstadoId = 23,
                    Nome = "RR",
                    Capital = "Boa Vista",
                    Regiao = "Norte",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                }, new Estado
                {
                    EstadoId = 24,
                    Nome = "SC",
                    Capital = "Florianópolis",
                    Regiao = "Sul",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                }, new Estado
                {
                    EstadoId = 25,
                    Nome = "SE",
                    Capital = "Aracajú",
                    Regiao = "Nordeste",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                }, new Estado
                {
                    EstadoId = 26,
                    Nome = "SP",
                    Capital = "São Paulo",
                    Regiao = "Sudeste",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                }, new Estado
                {
                    EstadoId = 27,
                    Nome = "TO",
                    Capital = "Palmas",
                    Regiao = "Norte",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                });
        }

        private static void StatusCampanha(AvonDbContext context)
        {
            context.StatusCampanha.AddOrUpdate(x => x.Id,
                new StatusCampanha
                {
                    Id = 1,
                    Nome = "Em Configuração",
                    DataInclusao = DateTime.Now,
                    Dataalteracao = null
                },
            new StatusCampanha
                {

                    Id = 2,
                    Nome = "Publicada",
                    DataInclusao = DateTime.Now,
                    Dataalteracao = null
                },
            new StatusCampanha
                {
                    Id = 3,
                    Nome = "Cancelada",
                    DataInclusao = DateTime.Now,
                    Dataalteracao = null
                });
        }

        private static void StatusFaleConosco(AvonDbContext context)
        {
            context.StatusFaleConosco.AddOrUpdate(x => x.Id,
                new StatusFaleConosco
                {
                    Id = 1,
                    Nome = "Pendente",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = null,

                },
                new StatusFaleConosco
                {
                    Id = 2,
                    Nome = "Respondido",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = null
                });
        }

        private static void Assunto(AvonDbContext context)
        {
            context.Assunto.AddOrUpdate(x => x.Id,
                new Assunto
                {
                    Id = 1,
                    Nome = "Dúvidas",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = null,
                    Ativo = true
                },
                new Assunto
                {
                    Id = 2,
                    Nome = "Elogio",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = null,
                    Ativo = true
                },
                new Assunto
                {
                    Id = 3,
                    Nome = "Outros",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = null,
                    Ativo = true
                },
                new Assunto
                {
                    Id = 4,
                    Nome = "Pontuação",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = null,
                    Ativo = true
                },
                    new Assunto
                {
                    Id = 5,
                    Nome = "Reclamação",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = null,
                    Ativo = true
                },
                new Assunto
                {
                    Id = 6,
                    Nome = "Resgate",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = null,
                    Ativo = true
                }

                );
        }

        private static void StatusPontuacao(AvonDbContext context)
        {
            context.StatusPontuacao.AddOrUpdate(x => x.Id, new StatusPontuacao
            {
                Id = 1,
                Nome = "Em Aprovação",
                Descricao = "Pontuação Aguardando Aprovação                                                                                                                                                                                                                                 ",
                DataInclusao = DateTime.Now,
                DataAlteracao = DateTime.Now
            }, new StatusPontuacao
            {
                Id = 2,
                Nome = "Aprovada",
                Descricao = "Pontuação Aprovada",
                DataInclusao = DateTime.Now,
                DataAlteracao = DateTime.Now
            }, new StatusPontuacao
            {
                Id = 3,
                Nome = "Creditada",
                Descricao = "Pontuação Crediatada",
                DataInclusao = DateTime.Now,
                DataAlteracao = DateTime.Now
            }, new StatusPontuacao
            {
                Id = 4,
                Nome = "Inativada",
                Descricao = "Pontuação Inativada",
                DataInclusao = DateTime.Now,
                DataAlteracao = DateTime.Now
            }, new StatusPontuacao
            {
                Id = 5,
                Nome = "Excluída",
                Descricao = "Pontuação Excluída",
                DataInclusao = DateTime.Now,
                DataAlteracao = DateTime.Now
            });

        }

        private static void TipoArquivo(AvonDbContext context)
        {
            context.TipoArquivo.AddOrUpdate(x => x.Id,
                new TipoArquivo
                {
                    Id = 1,
                    Nome = "Estrutura",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoArquivo
                {
                    Id = 2,
                    Nome = "Crédito de pontos",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoArquivo
                {
                    Id = 3,
                    Nome = "Apuração Vendeu Ganhou",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now

                },
                new TipoArquivo
                {
                    Id = 4,
                    Nome = "Participante em Lote",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoArquivo
                {
                    Id = 5,
                    Nome = "Hierarquia",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoArquivo
                {
                    Id = 6,
                    Nome = "Meta por Pessoa",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoArquivo
                {
                    Id = 7,
                    Nome = "Grupo Item",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoArquivo
                {
                    Id = 8,
                    Nome = "Campanha Grupo Item Pontos",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoArquivo
                {
                    Id = 9,
                    Nome = "Meta Por Item",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoArquivo
                {
                    Id = 10,
                    Nome = "Associacao Grupo Item",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now

                },
                new TipoArquivo
                {
                    Id = 11,
                    Nome = "Upload de Vendas",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoArquivo
                {
                    Id = 12,
                    Nome = "Faixa Atingimento GrupoItem",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoArquivo
                {
                    Id = 13,
                    Nome = "Calculado Vendeu Ganhou - Por Item",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoArquivo
                {
                    Id = 14,
                    Nome = "Calculado Vendeu Ganhou - Por Pessoa",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoArquivo
                {
                    Id = 15,
                    Nome = "Calculado Vendeu Ganhou - Por Ranking por Pessoa",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoArquivo
                {
                    Id = 16,
                    Nome = "Calculado Meta Resultado - Por Item",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoArquivo
                {
                    Id = 17,
                    Nome = "Calculado Meta Resultado - Por Pessoa",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoArquivo
                {
                    Id = 18,
                    Nome = "Calculado Meta Resultado - Por Ranking por Pessoa",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoArquivo
                {
                    Id = 19,
                    Nome = "Resgates Gerais",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoArquivo
                {
                    Id = 20,
                    Nome = "Resgates Exclusivos Avon",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                });
        }

        private static void TipoCampanha(AvonDbContext context)
        {
            context.TipoCampanha.AddOrUpdate(x => x.Id,
                new TipoCampanha
                {
                    Id = 1,
                    Nome = "Meta e Resultado - Meta Por Pessoa",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoCampanha
                {
                    Id = 2,
                    Nome = "Meta e Resultado - Ranking Por Pessoa",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                }, new TipoCampanha
                {
                    Id = 3,
                    Nome = "Vendeu Ganhou - Meta Por Item",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                }, new TipoCampanha
                {
                    Id = 4,
                    Nome = "Vendeu Ganhou - Ranking Por Pessoa",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                }, new TipoCampanha
                {
                    Id = 5,
                    Nome = "Meta e Resultado - Meta Por Item",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                }, new TipoCampanha
                {
                    Id = 6,
                    Nome = "Meta e Resultado Ranking - Meta por Item",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now

                }, new TipoCampanha
                {
                    Id = 8,
                    Nome = "Meta e Resultado - Calculado Por Pessoa",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now

                }, new TipoCampanha
                {
                    Id = 9,
                    Nome = "Meta e Resultado - Calculado Ranking Por Pessoa",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                }, new TipoCampanha
                {
                    Id = 10,
                    Nome = "Vendeu Ganhou - Calculado Por Pessoa",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                }, new TipoCampanha
                {
                    Id = 11,
                    Nome = "Vendeu Ganhou - Calculado Ranking Por Pessoa",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                }, new TipoCampanha
                {
                    Id = 12,
                    Nome = "Meta e Resultado - Calculado Por Item",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                }, new TipoCampanha
                {
                    Id = 14,
                    Nome = "Vendeu Ganhou - Calculado Por Item",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                }, new TipoCampanha
                {
                    Id = 15,
                    Nome = "Vendeu Ganhou Ranking - Por Item - Calculado",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                });
        }

        private static void TipoModulo(AvonDbContext context)
        {
            context.TipoModulo.AddOrUpdate(x => x.Id,
                new TipoModulo
                {
                    Id = 1,
                    Nome = "Menu",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoModulo
                {
                    Id = 2,
                    Nome = "Notícias",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoModulo
                {
                    Id = 3,
                    Nome = "Vídeos",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoModulo
                {
                    Id = 4,
                    Nome = "Downloads",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoModulo
                {
                    Id = 5,
                    Nome = "Fotos",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoModulo
                {
                    Id = 6,
                    Nome = "Banners",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoModulo
                {
                    Id = 7,
                    Nome = "Background",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoModulo
                {
                    Id = 8,
                    Nome = "Mecânica",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoModulo
                {
                    Id = 9,
                    Nome = "Regulamento",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                });
        }

        private static void TipoPontuacao(AvonDbContext context)
        {
            context.TipoPontuacao.AddOrUpdate(x => x.Id,
                new TipoPontuacao
                {
                    Id = 1,
                    Nome = "Arquivo",
                    Descricao = "Origem: Arquivo de crédito",
                    FlCampanha = false,
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now,
                },
                new TipoPontuacao
                {
                    Id = 2,
                    Nome = "Quizz",
                    Descricao = "Origem: Quizz",
                    FlCampanha = false,
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now,
                },
                new TipoPontuacao
                {
                    Id = 3,
                    Nome = "Campanha",
                    Descricao = "Origem: Campanha",
                    FlCampanha = true,
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now,
                }
                );
        }

        private static void TipoQuestionario(AvonDbContext context)
        {
            context.TipoQuestionario.AddOrUpdate(x => x.Id,
                new TipoQuestionario
                {
                    Id = 1,
                    Nome = "Pesquisa de Satisfação",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoQuestionario
                {
                    Id = 2,
                    Nome = "Quiz",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoQuestionario
                {
                    Id = 3,
                    Nome = "Faq",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                });
        }

        private static void TipoResposta(AvonDbContext context)
        {
            context.TipoResposta.AddOrUpdate(x => x.Id,
                new TipoResposta
                {
                    Id = 1,
                    Nome = "Múltipla escolha",
                    Controle = "Radio",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoResposta
                {
                    Id = 2,
                    Nome = "Múltipla resposta",
                    Controle = "Checkbox",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoResposta
                {
                    Id = 3,
                    Nome = "Aberta",
                    Controle = "Text",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                });
        }

        private static void TipoValidacaoPositiva(AvonDbContext context)
        {
            context.TipoValidacaoPositiva.AddOrUpdate(x => x.Id,
                new TipoValidacaoPositiva
                {
                    Id = 1,
                    Nome = "CPF",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoValidacaoPositiva
                {
                    Id = 2,
                    Nome = "CNPJ (1 Campo)",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoValidacaoPositiva
                {
                    Id = 3,
                    Nome = "CPF ou CNPJ (1 Campo)",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoValidacaoPositiva
                {
                    Id = 4,
                    Nome = "Código cliente  (1 Campo)",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoValidacaoPositiva
                {
                    Id = 5,
                    Nome = "Email  (1 Campo)",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoValidacaoPositiva
                {
                    Id = 6,
                    Nome = "CPF e Data Nascimento (2 Campos)",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoValidacaoPositiva
                {
                    Id = 7,
                    Nome = "CNPJ e CPF (2 Campos)",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoValidacaoPositiva
                {
                    Id = 8,
                    Nome = "Codigo e CPF (2 Campos)",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                },
                new TipoValidacaoPositiva
                {
                    Id = 9,
                    Nome = "Codigo e CNPJ (2 Campos)",
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                }
                );
        }

        private static void UsuarioAdm(AvonDbContext context)
        {
            context.UsuarioAdm.AddOrUpdate(x => x.Id,
                new UsuarioAdm
                {
                    Id = 1,
                    PerfilId = 1,
                    Login = "llima",
                    Senha = "A7004750969E7D689761622D6C548243",
                    Nome = "Luiz Lima",
                    Email = "luiz.lima@fcamara.com.br",
                    Ativo = true,
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now
                });
        }
    }
}
