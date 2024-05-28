using GrupoLTM.WebSmart.Domain.Enums;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace GrupoLTM.WebSmart.Services
{
    public class QuestionarioService : BaseService<Questionario>
    {
        public List<WebSmart.DTO.QuestionarioModel> ListaQuestionariosAtivos(int tipoQuestionario, DateTime data, int perfilID, int estruturaID, int participanteID, int pagina, int quantidadeRegistros, out int total)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repConteudo = context.CreateRepository<Questionario>();
                IRepository repParticipanteQuestionario = context.CreateRepository<ParticipanteQuestionario>();
                QuestionarioRepository repository = context.QuestionarioRepository();

                List<WebSmart.DTO.QuestionarioModel> listQuestionarioModel = new List<WebSmart.DTO.QuestionarioModel>();

                var lista = repository.Filter(x => x.Ativo == true
                    && (x.DataInicio <= data && x.DataFim >= data)
                    && x.TipoQuestionarioId == tipoQuestionario
                    && x.QuestionarioPerfil.Any(qp => qp.PerfilId == perfilID && qp.Ativo)
                    && x.QuestionarioEstrutura.Any(qe => qe.EstruturaId == estruturaID && qe.Ativo)
                    , out total, pagina, quantidadeRegistros)
                    .ToList();

                lista.ForEach(delegate(Questionario questionario)
                {

                    var questionarioRespondido = repParticipanteQuestionario.Filter<ParticipanteQuestionario>(x => x.Ativo
                        && x.ParticipanteId == participanteID
                        && x.QuestionarioId == questionario.Id).ToList();

                    listQuestionarioModel.Add(new WebSmart.DTO.QuestionarioModel
                    {
                        Ativo = questionario.Ativo,
                        DataAlteracao = questionario.DataAlteracao.Value,
                        DataFim = questionario.DataFim,
                        DataInclusao = questionario.DataInclusao,
                        DataInicio = questionario.DataInicio,
                        Descricao = questionario.Descricao,
                        Id = questionario.Id,
                        Nome = questionario.Nome,
                        TipoQuestionarioId = questionario.TipoQuestionarioId,
                        QuestionarioJaRespondido = (questionarioRespondido.Count() > 0)
                    });
                });

                return listQuestionarioModel;
            }
        }

        public List<WebSmart.DTO.QuestionarioModel> ListaQuestionariosStatus(int tipoQuestionario, DateTime data, int perfilID, int estruturaID, int participanteID, int pagina, int quantidadeRegistros, out int total)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repConteudo = context.CreateRepository<Questionario>();
                IRepository repParticipanteQuestionario = context.CreateRepository<ParticipanteQuestionario>();
                QuestionarioRepository repository = context.QuestionarioRepository();

                List<WebSmart.DTO.QuestionarioModel> listQuestionarioModel = new List<WebSmart.DTO.QuestionarioModel>();

                var lista = repository.Filter(x => x.Ativo == true
                    && x.TipoQuestionarioId == tipoQuestionario
                    && x.QuestionarioPerfil.Any(qp => qp.PerfilId == perfilID && qp.Ativo)
                    && x.QuestionarioEstrutura.Any(qe => qe.EstruturaId == estruturaID && qe.Ativo)
                    , out total, pagina, quantidadeRegistros)
                    .ToList();

                lista.ForEach(delegate(Questionario questionario)
                {

                    var questionarioRespondido = repParticipanteQuestionario.Filter<ParticipanteQuestionario>(x => x.Ativo
                        && x.ParticipanteId == participanteID
                        && x.QuestionarioId == questionario.Id).ToList();

                    listQuestionarioModel.Add(new WebSmart.DTO.QuestionarioModel
                    {
                        Ativo = questionario.Ativo,
                        DataAlteracao = questionario.DataAlteracao.Value,
                        DataFim = questionario.DataFim,
                        DataInclusao = questionario.DataInclusao,
                        DataInicio = questionario.DataInicio,
                        Descricao = questionario.Descricao,
                        Id = questionario.Id,
                        Nome = questionario.Nome,
                        TipoQuestionarioId = questionario.TipoQuestionarioId,
                        QuestionarioJaRespondido = (questionarioRespondido.Count() > 0)
                    });
                });

                return listQuestionarioModel;
            }
        }

        public List<WebSmart.DTO.QuestionarioModel> ListaQuestionarios(DateTime data, int perfilID, int estruturaID, EnumDomain.TipoQuestionario tipoQuestionario,
            int participanteID)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repConteudo = context.CreateRepository<Questionario>();
                IRepository repParticipanteQuestionario = context.CreateRepository<ParticipanteQuestionario>();
                List<WebSmart.DTO.QuestionarioModel> listQuestionarioModel = new List<WebSmart.DTO.QuestionarioModel>();

                var lista = repConteudo.Filter<Questionario>(x => x.Ativo == true
                    && (x.DataInicio <= data && x.DataFim >= data)
                    && (EnumDomain.TipoQuestionario)x.TipoQuestionarioId == tipoQuestionario
                    && x.QuestionarioPerfil.Any(qp => qp.PerfilId == perfilID && qp.Perfil.Ativo)
                    && x.QuestionarioEstrutura.Any(qe => qe.EstruturaId == estruturaID && qe.Estrutura.Ativo))
                    .OrderBy(x => x.Nome)
                    .ToList();

                lista.ForEach(delegate(Questionario questionario)
                {
                    var questionarioRespondido = repParticipanteQuestionario.Filter<ParticipanteQuestionario>(x => x.Ativo
                        && x.ParticipanteId == participanteID
                        && x.QuestionarioId == questionario.Id).ToList();

                    listQuestionarioModel.Add(new WebSmart.DTO.QuestionarioModel
                    {
                        Ativo = questionario.Ativo,
                        DataAlteracao = questionario.DataAlteracao.Value,
                        DataFim = questionario.DataFim,
                        DataInclusao = questionario.DataInclusao,
                        DataInicio = questionario.DataInicio,
                        Descricao = questionario.Descricao,
                        Id = questionario.Id,
                        Nome = questionario.Nome,
                        TipoQuestionarioId = questionario.TipoQuestionarioId,
                        QuestionarioJaRespondido = (questionarioRespondido.Count() > 0)
                    });
                });

                return listQuestionarioModel;
            }
        }

        public List<WebSmart.DTO.QuestionarioPerguntasModel> CarregarPerguntas(int questionarioID, int participanteID)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repConteudo = context.CreateRepository<Pergunta>();
                IRepository repQuestionarioParticipante = context.CreateRepository<ParticipanteQuestionario>();
                List<WebSmart.DTO.QuestionarioPerguntasModel> listQuestionarioPerguntasModel = new List<WebSmart.DTO.QuestionarioPerguntasModel>();

                var lista = repConteudo.Filter<Pergunta>(x => x.Ativo == true
                    && x.QuestionarioId == questionarioID)
                    .OrderBy(x => x.Id)
                    .ToList();

                int i = 1;

                var quantidadeRespostasQuestionario = repQuestionarioParticipante.Filter<ParticipanteQuestionario>(x => x.Ativo
                    && x.ParticipanteId == participanteID
                    && x.QuestionarioId == questionarioID).ToList();

                lista.ForEach(delegate(Pergunta pergunta)
                {
                    var respostas = ListaRespostasPorPergunta(pergunta.Id);

                    listQuestionarioPerguntasModel.Add(new WebSmart.DTO.QuestionarioPerguntasModel
                    {
                        Ativo = pergunta.Ativo,
                        AtivoPontos = pergunta.AtivoPontos,
                        NumeroPergunta = i,
                        DataAlteracao = pergunta.DataAlteracao.Value,
                        DataInclusao = pergunta.DataInclusao,
                        Id = pergunta.Id,
                        PerguntaResposta = respostas,
                        Nome = pergunta.Nome,
                        QuestionarioId = pergunta.QuestionarioId,
                        TipoDeRespostaId = pergunta.TipoRespostaId,
                        ValorPontos = (pergunta.ValorPontos) == null ? 0 : pergunta.ValorPontos.Value,
                        QuestionarioJaRespondido = (quantidadeRespostasQuestionario.Count() > 0)
                    });

                    i += 1;
                });

                return listQuestionarioPerguntasModel;
            }

        }

        public List<WebSmart.DTO.QuestionarioPerguntasModel> CarregarPerguntasERespostas(int questionarioID, int participanteID)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repConteudo = context.CreateRepository<Pergunta>();
                IRepository repQuestionarioParticipante = context.CreateRepository<ParticipanteQuestionario>();
                List<WebSmart.DTO.QuestionarioPerguntasModel> listQuestionarioPerguntasModel = new List<WebSmart.DTO.QuestionarioPerguntasModel>();

                var lista = repConteudo.Filter<Pergunta>(x => x.Ativo == true
                    && x.QuestionarioId == questionarioID)
                    .OrderBy(x => x.Id)
                    .ToList();

                int i = 1;

                var quantidadeRespostasQuestionario = repQuestionarioParticipante.Filter<ParticipanteQuestionario>(x => x.Ativo
                    && x.ParticipanteId == participanteID
                    && x.QuestionarioId == questionarioID).ToList();

                lista.ForEach(delegate(Pergunta pergunta)
                {
                    var respostas = ListaRespostasPorPergunta(pergunta.Id);
                    List<int> lstRespostaId = quantidadeRespostasQuestionario.Where(f => f.PerguntaId == pergunta.Id).Select(x => x.RespostaId).ToList();

                    string respostaTexto = string.Empty;
                    var participanteQuestionario = pergunta.ParticipanteQuestionario.FirstOrDefault(x => x.Ativo);
                    if (participanteQuestionario != null)
                    {
                        respostaTexto = participanteQuestionario.RespostaTexto;
                    }

                    var questionarioPerguntasModel = new WebSmart.DTO.QuestionarioPerguntasModel
                    {
                        Ativo = pergunta.Ativo,
                        AtivoPontos = pergunta.AtivoPontos,
                        NumeroPergunta = i,
                        DataAlteracao = pergunta.DataAlteracao.Value,
                        DataInclusao = pergunta.DataInclusao,
                        Id = pergunta.Id,
                        PerguntaResposta = respostas,
                        Nome = pergunta.Nome,
                        RespostaTexto = respostaTexto,
                        QuestionarioId = pergunta.QuestionarioId,
                        TipoDeRespostaId = pergunta.TipoRespostaId,
                        ValorPontos = (pergunta.ValorPontos) == null ? 0 : pergunta.ValorPontos.Value,
                        RespostaId = lstRespostaId.FirstOrDefault(),
                        LstRespostaId = lstRespostaId,
                        QuestionarioJaRespondido = (quantidadeRespostasQuestionario.Count() > 0)
                    };

                    foreach (var item in lstRespostaId)
                    {
                        var respostaModel = respostas.FirstOrDefault(f => f.respostaID == item);
                        if (respostaModel != null)
                            questionarioPerguntasModel.Acertou = respostaModel.RespostaCerta;
                    }

                    listQuestionarioPerguntasModel.Add(questionarioPerguntasModel);

                    i += 1;
                });

                return listQuestionarioPerguntasModel;
            }

        }

        private List<WebSmart.DTO.RespostaModel> ListaRespostasPorPergunta(int perguntaID)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repConteudo = context.CreateRepository<Resposta>();
                List<WebSmart.DTO.RespostaModel> listQuestionarioRespostasModel = new List<WebSmart.DTO.RespostaModel>();


                var lista = repConteudo.Filter<Resposta>(x => x.Ativo == true
                    && x.PerguntaId == perguntaID)
                    .OrderBy(x => x.ordem)
                    .ToList();

                lista.ForEach(delegate(Resposta resposta)
                {
                    listQuestionarioRespostasModel.Add(new WebSmart.DTO.RespostaModel
                    {
                        Ativo = resposta.Ativo,
                        DataAlteracao = resposta.DataAlteracao.Value,
                        DataInclusao = resposta.DataInclusao,
                        Nome = resposta.Nome,
                        perguntaID = resposta.PerguntaId,
                        RespostaCerta = resposta.RespostaCorreta.Value,
                        respostaID = resposta.Id
                    });
                });

                return listQuestionarioRespostasModel;
            }
        }

        public DTO.RespostaModel ObterResposta(int respostaID)
        {
            var respostaModel = new DTO.RespostaModel();

            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repConteudo = context.CreateRepository<Resposta>();

                var resposta = repConteudo.Find<Resposta>(respostaID);

                respostaModel = new DTO.RespostaModel
                {
                    Ativo = resposta.Ativo,
                    DataAlteracao = resposta.DataAlteracao.Value,
                    DataInclusao = resposta.DataInclusao,
                    Nome = resposta.Nome,
                    perguntaID = resposta.PerguntaId,
                    RespostaCerta = resposta.RespostaCorreta.Value,
                    respostaID = resposta.Id,
                    TipoDePergunta = resposta.Pergunta.TipoRespostaId
                };
            }

            return respostaModel;
        }

        public DTO.PerguntaModel ObterPergunta(int respostaID)
        {
            var perguntaModel = new DTO.PerguntaModel();

            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repConteudo = context.CreateRepository<Pergunta>();

                var pergunta = repConteudo.Find<Pergunta>(respostaID);

                if (pergunta.ValorPontos != null)
                    perguntaModel = new DTO.PerguntaModel
                    {
                        Ativo = pergunta.Ativo,
                        DataAlteracao = pergunta.DataAlteracao.Value,
                        DataInclusao = pergunta.DataInclusao,
                        AtivoPontos = pergunta.AtivoPontos,
                        id = pergunta.Id,
                        nome = pergunta.Nome,
                        questionarioID = pergunta.QuestionarioId,
                        tipoRespostaID = pergunta.TipoRespostaId,
                        ValorPontos = pergunta.ValorPontos.Value
                    };
            }

            return perguntaModel;
        }

        public void GravarRespostas(List<DTO.ParticipanteQuestionarioModel> respostasParticipante, int questionarioID, int participanteID, bool questionarioPontua, string descricaoPontuacao)
        {
            //Tudo dentro do bloco do contexto, pois se tiver algum erro interno, todas as operações são canceladas. 
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    IRepository repConteudo = context.CreateRepository<ParticipanteQuestionario>();
                    IRepository repPergunta = context.CreateRepository<Pergunta>();
                    IRepository repPontuacaoParticipanteQuestionario = context.CreateRepository<PontuacaoParticipanteQuestionario>();
                    var listaPontuacaoParticipanteQuestionario = new List<PontuacaoParticipanteQuestionario>();

                    //Verifica se o participante já tem resposta para este questionário
                    var respostasAtivas = repConteudo.Filter<ParticipanteQuestionario>(x => x.Ativo
                        && x.QuestionarioId == questionarioID
                        && x.ParticipanteId == participanteID).ToList();

                    //Se já tiver inativa todas, para inserir as novas
                    foreach (ParticipanteQuestionario resposta in respostasAtivas)
                    {
                        resposta.Ativo = false;
                        resposta.DataAlteracao = DateTime.Now;
                        repConteudo.Update<ParticipanteQuestionario>(resposta);
                    }

                    double pontos = 0;

                    respostasParticipante.ForEach(delegate(DTO.ParticipanteQuestionarioModel participanteQuestionario)
                    {
                        var idPaticipanteQuestionario = repConteudo.Create(new ParticipanteQuestionario
                        {
                            Ativo = true,
                            DataAlteracao = null,
                            DataInclusao = DateTime.Now,
                            ParticipanteId = participanteID,
                            PerguntaId = participanteQuestionario.PerguntaId,
                            QuestionarioId = questionarioID,
                            RespostaId = participanteQuestionario.RespostaId,
                            RespostaTexto = string.IsNullOrWhiteSpace(participanteQuestionario.RespostaTexto) ? null : participanteQuestionario.RespostaTexto
                        });

                        if (questionarioPontua)
                        {
                            if (participanteQuestionario.TipoDePergunta != (int)EnumDomain.TipoResposta.UnicaEscolha)
                            {
                                if (participanteQuestionario.respostaCorreta)
                                {
                                    var perguntaPontuacao = repPergunta.Find<Pergunta>(participanteQuestionario.PerguntaId);

                                    if (perguntaPontuacao != null)
                                    {
                                        //Cria a lista para a tabela de referência
                                        listaPontuacaoParticipanteQuestionario.Add(new PontuacaoParticipanteQuestionario
                                        {
                                            ParticipanteQuestionarioID = idPaticipanteQuestionario.Id,
                                        });

                                        if (perguntaPontuacao.ValorPontos != null)
                                            pontos += perguntaPontuacao.ValorPontos.Value;
                                    }
                                }
                            }
                        }
                    });


                    if (questionarioPontua)
                    {
                        var perguntasMultiplasRespostas = respostasParticipante.Where(x => x.TipoDePergunta == (int)EnumDomain.TipoResposta.UnicaEscolha && x.respostaCorreta)
                                                                               .GroupBy(x => new { x.PerguntaId, x.respostaCorreta })
                                                                               .Select(y => new { PerguntaID = y.Key.PerguntaId, RespostasCorretas = y.Count(s => s.respostaCorreta) });

                        foreach (var perguntas in perguntasMultiplasRespostas)
                        {
                            IRepository repResposta = context.CreateRepository<Resposta>();

                            var quantidadeCorretas = repResposta.Filter<Resposta>(x => x.PerguntaId == perguntas.PerguntaID).Where(x => x.RespostaCorreta.Value && x.Ativo).Count();

                            if (perguntas.RespostasCorretas == quantidadeCorretas)
                            {
                                var perguntaPontuacao = repPergunta.Find<Pergunta>(x => x.Ativo && x.AtivoPontos);

                                if (perguntaPontuacao != null)
                                {
                                    if (perguntaPontuacao.ValorPontos != null)
                                        pontos += perguntaPontuacao.ValorPontos.Value;
                                }
                            }
                        }


                        if (pontos > 0)
                        {
                            IRepository repPontuacao = context.CreateRepository<Pontuacao>();

                            var idPontuacao = repPontuacao.Create<Pontuacao>(new Pontuacao
                            {
                                DataAlteracao = DateTime.Now,
                                DataInclusao = DateTime.Now,
                                Descricao = descricaoPontuacao,
                                ParticipanteId = participanteID,
                                Pontos = pontos,
                                StatusPontuacaoId = (int)EnumDomain.StatusPontuacao.PendenteEnvio,
                                TipoPontuacaoId = (int)EnumDomain.TipoPontuacao.Quizz
                            });

                            listaPontuacaoParticipanteQuestionario.ForEach(delegate(PontuacaoParticipanteQuestionario item)
                            {
                                repPontuacaoParticipanteQuestionario.Create<PontuacaoParticipanteQuestionario>(new PontuacaoParticipanteQuestionario
                                {
                                    ParticipanteQuestionarioID = item.ParticipanteQuestionarioID,
                                    Ativo = true,
                                    DataInclusao = DateTime.Now,
                                    PontuacaoID = idPontuacao.Id
                                });
                            });

                        }
                    }

                    scope.Complete();
                }
            }
        }
    }
}
