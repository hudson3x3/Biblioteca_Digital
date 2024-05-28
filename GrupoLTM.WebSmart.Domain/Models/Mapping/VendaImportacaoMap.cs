using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class VendaImportacaoMap : EntityTypeConfiguration<VendaImportacao>
    {
        public VendaImportacaoMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Login)
                .HasMaxLength(50);

            this.Property(t => t.ItemCodigo)
                .HasMaxLength(50);

            this.Property(t => t.DataVenda)
                .HasMaxLength(50);

            this.Property(t => t.Valor)
                .HasMaxLength(50);

            this.Property(t => t.CodigoVenda)
                .HasMaxLength(50);

            this.Property(t => t.CodigoComprador)
                .HasMaxLength(50);

            this.Property(t => t.NomeComprador)
                .HasMaxLength(255);

            this.Property(t => t.Mes)
                .HasMaxLength(10);

            this.Property(t => t.Ano)
                .HasMaxLength(10);

            this.Property(t => t.Erro)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("VendaImportacao");
            this.Property(t => t.Login).HasColumnName("Login");
            this.Property(t => t.ItemCodigo).HasColumnName("ItemCodigo");
            this.Property(t => t.DataVenda).HasColumnName("DataVenda");
            this.Property(t => t.Valor).HasColumnName("Valor");
            this.Property(t => t.CodigoVenda).HasColumnName("CodigoVenda");
            this.Property(t => t.CodigoComprador).HasColumnName("CodigoComprador");
            this.Property(t => t.NomeComprador).HasColumnName("NomeComprador");
            this.Property(t => t.ArquivoId).HasColumnName("ArquivoId");
            this.Property(t => t.Mes).HasColumnName("Mes");
            this.Property(t => t.Ano).HasColumnName("Ano");
            this.Property(t => t.ParticipanteId).HasColumnName("ParticipanteId");
            this.Property(t => t.GrupoItemId).HasColumnName("GrupoItemId");
            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
            this.Property(t => t.DataAlteracao).HasColumnName("DataAlteracao");
            this.Property(t => t.Erro).HasColumnName("Erro");
            this.Property(t => t.Id).HasColumnName("Id");
        }
    }
}
