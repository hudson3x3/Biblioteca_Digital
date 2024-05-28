using System;
using System.Collections.Generic;
using System.Linq;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.DTO;
using GrupoLTM.WebSmart.Infrastructure.Mail;

namespace GrupoLTM.WebSmart.Services
{
    public class CampanhaSimuladorService : BaseService<CampanhaSimulador>
    {
        public List<CampanhaSimuladorModel> ListarCampanhaPorPeriodo(int NumeroCampanha, int AnoCampanha)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repositoryCampanha = context.CreateRepository<CampanhaSimulador>();

                var list = repositoryCampanha.Filter<CampanhaSimulador>(x => x.Ativo == true &&
                x.CampanhaMecanicaSimulador.Count > 0 &&
                x.NumeroCampanha >= NumeroCampanha &&
                x.NumeroCampanha <= (NumeroCampanha + 2) &&
                x.AnoCampanha == AnoCampanha &&
                x.CampanhaMecanicaSimulador.Where(s => s.MecanicaSimulador.MecanicaSubMecanicaSimulador.Where(w => w.Ativo == true && w.SubMecanicaSimulador.Ativo == true && w.SubMecanicaSimulador.FatorConversaoMecanicaSimulador.Where(f => f.IdSubMecanicaSimulador == w.SubMecanicaSimulador.Id && f.FatorConversaoSimulador.Ativo == true && f.Ativo == true).Any() && w.IdCampanhaSimulador == x.Id).Any()).Count() > 0)
                .Select(x => new CampanhaSimuladorModel()
                {
                    Id = x.Id,
                    Nome = x.Nome,
                    NumeroCampanha = x.NumeroCampanha,
                    Descricao = x.Descricao,
                    DataInclusao = x.DataInclusao,
                    DataAlteracao = x.DataAlteracao,
                    Ativo = x.Ativo,
                    LinkDownload = x.LinkDownload,
                    CampanhaMecanicaSimulador = x.CampanhaMecanicaSimulador
                    .Where(w => w.Ativo == true && w.MecanicaSimulador.Ativo == true
                           && w.MecanicaSimulador.MecanicaSubMecanicaSimulador
                           .Where(a => a.Ativo == true && a.SubMecanicaSimulador.Ativo == true && a.SubMecanicaSimulador.FatorConversaoMecanicaSimulador.Any(f => f.FatorConversaoSimulador.Ativo == true && f.Ativo == true) && a.IdCampanhaSimulador == x.Id).Count() > 0)
                    .ToList()
                    .Select(s => new CampanhaMecanicaSimuladorModel()
                    {
                        MecanicaSimulador = new MecanicaSimuladorModel
                        {
                            Nome = s.MecanicaSimulador.Nome,
                            Descricao = s.MecanicaSimulador.Descricao,
                            IconeSimulador = new IconeSimuladorModel()
                            {
                                CaminhoImagem = s.MecanicaSimulador.IconeSimulador.CaminhoImagem,
                                Nome = s.MecanicaSimulador.IconeSimulador.Nome,
                                Tipo = s.MecanicaSimulador.IconeSimulador.Tipo,
                                Descricao = s.MecanicaSimulador.IconeSimulador.Descricao,
                                Ativo = s.MecanicaSimulador.IconeSimulador.Ativo
                            },
                            MecanicaSubMecanicaSimulador = s.MecanicaSimulador.MecanicaSubMecanicaSimulador.Where(w => w.Ativo == true && w.SubMecanicaSimulador.Ativo == true && w.IdCampanhaSimulador == x.Id).ToList().Select(m => new MecanicaSubMecanicaSimuladorModel()
                            {
                                SubMecanicaSimulador = new SubMecanicaSimuladorModel
                                {
                                    Nome = m.SubMecanicaSimulador.Nome,
                                    Descricao = m.SubMecanicaSimulador.Descricao,
                                    LinkDownload = m.SubMecanicaSimulador.LinkDownload,
                                    Ativo = m.SubMecanicaSimulador.Ativo,
                                    FatorConversaoMecanicaSimulador = m.SubMecanicaSimulador.FatorConversaoMecanicaSimulador.Where(w => w.FatorConversaoSimulador.Ativo == true && w.Ativo == true).Select(f => new FatorConversaoMecanicaSimuladorModel()
                                    {
                                        FatorConversaoSimulador = new FatorConversaoSimuladorModel
                                        {
                                            TipoPonto = f.FatorConversaoSimulador.TipoPonto,
                                            TipoConversao = f.FatorConversaoSimulador.TipoConversao,
                                            Id = f.FatorConversaoSimulador.Id,
                                            MultiplicadorPontos = f.FatorConversaoSimulador.MultiplicadorPontos,
                                            MultiplicadorValor = f.FatorConversaoSimulador.MultiplicadorValor,
                                            Mensagem = f.FatorConversaoSimulador.Mensagem,
                                            FatorConversaoPontosSimulador = f.FatorConversaoSimulador.FatorConversaoPontosSimulador.Where(w => w.Ativo == true).Select(r => new FatorConversaoPontosSimuladorModel()
                                            {
                                                Ativo = r.Ativo,
                                                ValorFinal = r.ValorFinal,
                                                ValorInicial = r.ValorInicial,
                                                Pontos = r.Pontos
                                            }).ToList()
                                        }
                                    }).ToList()
                                }
                            }).ToList()
                        }
                    }).ToList()
                }).ToList();

                return list;
            }
        }

        public CalculoFatorConversaoSimuladorModel ListarPontosFatorConversao(int IdFatorConversao, decimal range)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repositoryFatorConversao = context.CreateRepository<FatorConversaoSimulador>();

                var obj = repositoryFatorConversao.Filter<FatorConversaoSimulador>(x => x.Id == IdFatorConversao).FirstOrDefault();

                decimal calc = 0;

                if (obj.TipoConversao == (short)Domain.Enums.EnumMktPlace.FatorConversaoTipoConversao.Range)
                {
                    var result = obj.FatorConversaoPontosSimulador.Where(x => (x.ValorInicial <= range && x.ValorFinal >= range) && x.Ativo == true).FirstOrDefault();
                    calc = result != null ? result.Pontos : 0;
                }
                else
                {
                    //validação para não aceitar quer o valor informado seja menor que o range informado
                    if (range < obj.MultiplicadorValor)
                    {
                        return new CalculoFatorConversaoSimuladorModel()
                        {
                            Id = obj.Id,
                            Mensagem = obj.Mensagem,
                            Pontos = 0
                        };
                    }
                    //calculo para multiplicação

                    //verifica se o valor informado é decimal 
                    if ((range % 1) > 0)
                    {
                        var resultDecimal = ((range / obj.MultiplicadorValor) * obj.MultiplicadorPontos);
                        calc = Math.Ceiling(resultDecimal != null ? Convert.ToDecimal(resultDecimal) : 0);
                    }
                    else
                    {
                        var resultInt = (Math.Floor(Convert.ToDecimal((range / obj.MultiplicadorValor))) * obj.MultiplicadorPontos);
                        calc = resultInt != null ? Convert.ToInt32(resultInt) : 0;
                    }
                }

                var model = new CalculoFatorConversaoSimuladorModel()
                {
                    Id = obj.Id,
                    Mensagem = obj.Mensagem,
                    Pontos = calc
                };

                return model;
            }
        }
    }
}

