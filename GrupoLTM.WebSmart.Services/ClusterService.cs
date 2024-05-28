using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.DTO;

namespace GrupoLTM.WebSmart.Services
{
    public class ClusterService : BaseService<Cluster>
    {
        public ClusterModel GetById(int id)
        {
            using (var context = UnitOfWorkFactory.Create())
            {
                var rep = context.CreateRepository<Cluster>();

                return rep.Filter<Cluster>(_ => _.Id == id)
                            .ToList()
                            .Select(_ => new ClusterModel
                            {
                                Id = _.Id,
                                Nome = _.Nome,
                                LoginAdmin = _.LoginAdmin,
                                DataInicio = _.DataInicio,
                                DataFim = _.DataFim,
                                DataInicioFormatado = _.DataInicio.ToString("dd/MM/yyyy"),
                                DataFimFormatado = _.DataFim.ToString("dd/MM/yyyy"),
                                DataInclusao = _.DataInclusao,
                                MensagemErroValidacao = _.MensagemErroValidacao,
                                Ativo = _.Ativo,
                                ClusterRA = _.ClusterRA.Select(c => new ClusterRAModel
                                {
                                    Id = c.Id,
                                    RA = c.RA,
                                    Nome = c.Nome
                                }).ToList(),
                                ClusterProduct = _.ClusterProducts.Select(c => new ClusterProductModel
                                {
                                    Id = c.Id,
                                    ProductSku = c.ProductSku,
                                    ProductName = c.ProductName,
                                    ProductDescription = c.ProductDescription
                                }).ToList(),
                            })
                            .FirstOrDefault();
            }
        }

        public void EncerrarCluster(int id, string loginEncerramento)
        {
            using (var context = UnitOfWorkFactory.Create())
            {
                var rep = context.CreateRepository<Cluster>();

                var entity = rep.Find<Cluster>(_ => _.Id == id);
                entity.Ativo = false;
                entity.DataAlteracao = DateTime.Now;
                entity.DataEncerramento = DateTime.Now;
                entity.LoginEncerramento = loginEncerramento;

                rep.Update(entity);
                rep.SaveChanges();
            }
        }

        public List<ClusterModel> Listar()
        {
            using (var context = UnitOfWorkFactory.Create())
            {
                var rep = context.CreateRepository<Cluster>();

                return rep.All<Cluster>()
                          .OrderByDescending(_ => _.Ativo)
                          .ThenByDescending(_ => _.DataInclusao)
                          .ToList()
                          .Select(_ => new ClusterModel
                          {
                              Id = _.Id,
                              Nome = _.Nome,
                              LoginAdmin = _.LoginAdmin,
                              DataInicio = _.DataInicio,
                              DataFim = _.DataFim,
                              DataInicioFormatado = _.DataInicio.ToString("dd/MM/yyyy"),
                              DataFimFormatado = _.DataFim.ToString("dd/MM/yyyy"),
                              DataInclusao = _.DataInclusao,
                              LoginEncerramento = _.LoginEncerramento,
                              DataEncerramento = _.DataEncerramento,
                              MensagemErroValidacao = _.MensagemErroValidacao,
                              Ativo = _.Ativo,
                              ClusterProduct = _.ClusterProducts.Select(c => new ClusterProductModel
                              {
                                  Id = c.Id,
                                  ProductSku = c.ProductSku,
                                  ProductName = c.ProductName
                              }).ToList()
                          })
                         .ToList();
            }
        }

        public void CadastrarCluster(ClusterModel model)
        {
            using (var context = UnitOfWorkFactory.Create())
            {
                var rep = context.CreateRepository<Cluster>();
                var entity = new Cluster
                {
                    Nome = model.Nome,
                    DataInicio = model.DataInicio,
                    DataFim = model.DataFim,
                    Ativo = true,
                    DataInclusao = DateTime.Now,
                    LoginAdmin = model.LoginAdmin,
                    MensagemErroValidacao = model.MensagemErroValidacao,
                    ClusterProducts = model.ClusterProduct.Select(p => new ClusterProduct
                    {
                        ProductSku = p.ProductSku.ToUpper(),
                        ProductName = p.ProductName,
                        ProductDescription = p.ProductDescription
                    }).ToList()
                };

                entity = rep.Create(entity);
                rep.SaveChanges();

                model.Id = entity.Id;

                if (!string.IsNullOrEmpty(model.ClusterIdTemp))
                    AtualizaClusterRA(model);
            }
        }

        public void AtualizarCluster(ClusterModel model)
        {
            using (var context = UnitOfWorkFactory.Create())
            using (var scope = new TransactionScope())
            {
                var rep = context.CreateRepository<Cluster>();
                var entity = rep.Single<Cluster>(_ => _.Id == model.Id);

                entity.Nome = model.Nome;
                entity.DataInicio = model.DataInicio;
                entity.DataFim = model.DataFim;
                entity.DataAlteracao = DateTime.Now;
                entity.MensagemErroValidacao = model.MensagemErroValidacao;

                rep.Update(entity);

                rep.Delete<ClusterProduct>(_ => _.ClusterId == model.Id);

                model.ClusterRAExcluidos.ForEach(id => rep.Delete<ClusterRA>(_ => _.Id == id));

                model.ClusterProduct.ForEach(p =>
                {
                    rep.Create(new ClusterProduct
                    {
                        ClusterId = model.Id,
                        ProductSku = p.ProductSku.ToUpper(),
                        ProductName = p.ProductName,
                        ProductDescription = p.ProductDescription
                    });
                });

                rep.SaveChanges();
                scope.Complete();
            }

            if (!string.IsNullOrEmpty(model.ClusterIdTemp))
                AtualizaClusterRA(model);
        }

        public ClusterRAModel BuscarParticipante(string raParticipante, string tempClusterId)
        {
            ClusterRAModel retorno = null;

            using (var context = UnitOfWorkFactory.Create())
            {
                var rep = context.CreateRepository<ClusterRATemp>();
                var entityTemp = rep.Filter<ClusterRATemp>(_ =>
                                            _.RA == raParticipante
                                            && _.ClusterIdTemp.ToString().Equals(tempClusterId, StringComparison.InvariantCultureIgnoreCase))
                                    .FirstOrDefault();

                if (entityTemp != null)
                {
                    retorno = new ClusterRAModel
                    {
                        Id = entityTemp.Id,
                        RA = entityTemp.RA,
                        Nome = entityTemp.Nome
                    };
                }
            }

            return retorno;
        }

        public ClusterRAModel BuscarParticipante(string raParticipante, int clusterId)
        {
            ClusterRAModel retorno = null;
            using (var context = UnitOfWorkFactory.Create())
            {
                var rep = context.CreateRepository<ClusterRA>();

                var entity = rep.Filter<ClusterRA>(_ => _.RA == raParticipante && _.ClusterId == clusterId).FirstOrDefault();
                if (entity != null)
                {
                    retorno = new ClusterRAModel
                    {
                        Id = entity.Id,
                        ClusterId = entity.ClusterId,
                        RA = entity.RA,
                        Nome = entity.Nome
                    };
                }
            }

            return retorno;
        }

        public void ExcluirParticipanteTemp(string idCLusterRA)
        {
            using (var context = UnitOfWorkFactory.Create())
            {
                var rep = context.CreateRepository<ClusterRATemp>();

                var entityTemp = rep.Filter<ClusterRATemp>(_ => _.Id.ToString().Equals(idCLusterRA, StringComparison.InvariantCultureIgnoreCase))
                                    .FirstOrDefault();

                rep.Delete(entityTemp);
                rep.SaveChanges();
            }
        }

        public void ExcluirParticipante(string idClusterRA)
        {
            using (var context = UnitOfWorkFactory.Create())
            {
                var rep = context.CreateRepository<ClusterRA>();

                var entityTemp = rep.Filter<ClusterRA>(_ => _.Id.ToString().Equals(idClusterRA, StringComparison.InvariantCultureIgnoreCase))
                                    .FirstOrDefault();

                rep.Delete(entityTemp);
                rep.SaveChanges();
            }
        }

        private static void AtualizaClusterRA(ClusterModel model)
        {
            using (var cn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
            {
                var cmd = new SqlCommand("JP_PRC_AtualizaClusterRA", cn);
                cmd.Parameters.Add("@idClusterTemp", SqlDbType.UniqueIdentifier).Value = new Guid(model.ClusterIdTemp);
                cmd.Parameters.Add("@clusterId", SqlDbType.Int).Value = model.Id;
                cmd.Parameters.Add("@excluirBaseAtual", SqlDbType.Bit).Value = model.SubstituirParticipantes;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                cmd.ExecuteNonQuery();

                cn.Close();
                cmd.Dispose();
            }
        }

        public async Task<int> UploadParticipantesAsync(Stream file, string tempClusterId)
        {
            var clusterIdTemp = new Guid(tempClusterId);

            var dtList = new List<DataTable>();
            var dataInclusao = DateTime.Now;

            var dt = CreateCluterRATable();

            var contadorLista = 0;
            var totalLinhas = 0;

            var sr = new StreamReader(file);

            while (!sr.EndOfStream)
            {
                var ra = sr.ReadLine();

                if (string.IsNullOrWhiteSpace(ra) || ra.Length < 8) continue;
                if (string.IsNullOrWhiteSpace(ra.Substring(0, 8))) continue;

                totalLinhas++;

                var row = dt.NewRow();
                row["ClusterIdTemp"] = clusterIdTemp;
                row["RA"] = ra.Substring(0, 8);
                row["Nome"] = ra.Length > 8 ? ra.Substring(8).Trim() : "";
                row["DataInclusao"] = dataInclusao;
                dt.Rows.Add(row);

                contadorLista++;

                if (contadorLista < 100000) continue;

                dtList.Add(dt);

                dt = CreateCluterRATable();

                contadorLista = 0;
            }

            if (contadorLista > 0)
                dtList.Add(dt);

            await InserirClusterRAAsync(dtList);

            return totalLinhas;
        }

        private static async Task InserirClusterRAAsync(IEnumerable<DataTable> dtList)
        {
            using (var cn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
            {
                cn.Open();
                var transaction = cn.BeginTransaction();
                try
                {
                    var cmd = new SqlCommand("JP_PRC_DropIndexClusterRATemp", cn, transaction)
                    {
                        CommandTimeout = 3600,
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.ExecuteNonQuery();

                    var asyncTasks = dtList.Select(dtItem =>
                        Task.Run(async delegate
                            {
                                var bulkCopy =
                                    new SqlBulkCopy(cn, SqlBulkCopyOptions.TableLock, transaction)
                                    {
                                        BatchSize = 4000,
                                        BulkCopyTimeout = 3600,
                                        DestinationTableName = "ClusterRATemp"
                                    };

                                bulkCopy.ColumnMappings.Add("ClusterIdTemp", "ClusterIdTemp");
                                bulkCopy.ColumnMappings.Add("RA", "RA");
                                bulkCopy.ColumnMappings.Add("Nome", "Nome");
                                bulkCopy.ColumnMappings.Add("DataInclusao", "DataInclusao");

                                await bulkCopy.WriteToServerAsync(dtItem);
                            }
                        ));

                    await Task.WhenAll(asyncTasks.ToList());

                    cmd = new SqlCommand("JP_PRC_CreateIndexClusterRATemp", cn, transaction)
                    {
                        CommandTimeout = 3600,
                        CommandType = CommandType.StoredProcedure
                    };
                    await cmd.ExecuteNonQueryAsync();

                    transaction.Commit();
                    cn.Close();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private static DataTable CreateCluterRATable()
        {
            var dt = new DataTable("ClusterRATemp");
            dt.Columns.Add("ClusterIdTemp", typeof(Guid));
            dt.Columns.Add("RA");
            dt.Columns.Add("Nome");
            dt.Columns.Add("DataInclusao", typeof(DateTime));

            return dt;
        }

        public ClusterModel ValidarCluster(int clusterId, string ra, string productSku, out string errorMessage)
        {
            errorMessage = "";

            using (var context = UnitOfWorkFactory.Create())
            {
                var rep = context.CreateRepository<Cluster>();

                var entityCluster = rep.Single<Cluster>(_ => _.Id == clusterId);
                if (entityCluster == null)
                {
                    errorMessage = "Cluster inexistente";
                    return null;
                }

                if (!entityCluster.Ativo
                    || entityCluster.DataInicio.Date > DateTime.Now.Date
                    || entityCluster.DataFim.Date < DateTime.Now.Date)
                {
                    errorMessage = "Cluster inativo ou fora da vigência";
                    return null;
                }

                var entityClusterRA = rep.Filter<ClusterRA>(_ => _.ClusterId == clusterId && _.RA == ra).ToList();
                if (!entityClusterRA.Any())
                {
                    errorMessage = "Usuário não pertence ao cluster";
                    return null;
                }

                var entityClusterProduct = rep.Filter<ClusterProduct>(_ => _.ClusterId == clusterId && _.ProductSku.ToUpper() == productSku).ToList();
                if (!entityClusterProduct.Any())
                {
                    errorMessage = "SKU inexistente";
                    return null;
                }
                
                return new ClusterModel
                {
                    Id = entityCluster.Id,
                    Nome = entityCluster.Nome,
                    DataInicio = entityCluster.DataInicio,
                    DataFim = entityCluster.DataFim
                };
            }
        }

        public IList<ClusterModel> BuscarPorParticipante(string ra, out string errorMessage)
        {
            errorMessage = "";

            using (var context = UnitOfWorkFactory.Create())
            {
                var rep = context.CreateRepository<ClusterRA>();
                var repCluster = context.CreateRepository<Cluster>();

                var entityRa = rep.Filter<ClusterRA>(_ => _.RA == ra);
                if (entityRa == null || !entityRa.Any())
                {
                    errorMessage = "Usuário não pertence a nenhum cluster ativo.";
                    return null;
                }

                var clusterIds = entityRa.Select(_ => _.ClusterId).ToList();

                var entityCluster = repCluster
                                      .Filter<Cluster>(_ => clusterIds.Contains(_.Id))
                                      .IncludeEntity(_ => _.ClusterProducts)
                                      .ToList();

                entityCluster = entityCluster.Where(_ => _.Ativo
                                                         || _.DataInicio.Date > DateTime.Now.Date
                                                         || _.DataFim.Date < DateTime.Now.Date)
                                             .ToList();
                if (!entityCluster.Any())
                {
                    errorMessage = "Usuário não pertence a nenhum cluster ativo.";
                    return null;
                }

                return entityCluster.Select(_ => new ClusterModel
                {
                    Id = _.Id,
                    Nome = _.Nome,
                    DataInicio = _.DataInicio,
                    DataFim = _.DataFim,
                    Ativo = _.Ativo,
                    ClusterProduct = _.ClusterProducts.Select(p => new ClusterProductModel
                    {
                        ProductSku = p.ProductSku
                    }).ToList()
                }).ToList();
            }
        }
    }
}
