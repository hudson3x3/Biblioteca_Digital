using GrupoLTM.WebSmart.Domain.Models.Live;
using GrupoLTM.WebSmart.Domain.Models.Mapping.Live;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace GrupoLTM.WebSmart.Domain.Repository.Live
{
    public class AvonProcessDBContext : DbContext, IDisposable, IUnitOfWorkProcess
    {
        public AvonProcessDBContext(string connectionString = "GrupoLTMWebSmartProcess")
            : base(connectionString)
        {
            Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            Configuration.EnsureTransactionsForFunctionsAndCommands = false;
            Database.Connection.ConnectionString =
                ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
        }

        public AvonProcessDBContext()
            : base("GrupoLTMWebSmartProcess")
        {
            Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            Configuration.EnsureTransactionsForFunctionsAndCommands = false;
        }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            
            modelBuilder.Configurations.Add(new RequestCreditMap());
            modelBuilder.Configurations.Add(new TransactionItemTempMap());
            modelBuilder.Configurations.Add(new TransactionTempMap());
            modelBuilder.Configurations.Add(new StatusTransactionTempMap());
            modelBuilder.Entity<RequestCredit>().Ignore(t => t.Body);
            base.OnModelCreating(modelBuilder);

        }

        public DbSet<RequestCredit> RequestCredit { get; set; }
        public DbSet<TransactionItemTemp> TransactionItemTemp { get; set; }
        public DbSet<TransactionTemp> TransactionTemp { get; set; }
        public DbSet<StatusTransactionTemp> StatusTransactionTemp { get; set; }

        public RepositoryLive<TEntity> CreateRepository<TEntity>()
        {
            return new RepositoryLive<TEntity>(this);
        }

        public ConteudoRepositoryLive ConteudoRepository()
        {
            return new ConteudoRepositoryLive(this);
        }
    }
}
