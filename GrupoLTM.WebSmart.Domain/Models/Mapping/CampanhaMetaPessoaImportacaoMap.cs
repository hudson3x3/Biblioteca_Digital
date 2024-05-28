using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class CampanhaMetaPessoaImportacaoMap : EntityTypeConfiguration<CampanhaMetaPessoaImportacao>
    {
        public CampanhaMetaPessoaImportacaoMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Login)
                .HasMaxLength(120);

            this.Property(t => t.Erro)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("CampanhaMetaPessoaImportacao");
            this.Property(t => t.Login).HasColumnName("Login");
            this.Property(t => t.Meta).HasColumnName("Meta");
            this.Property(t => t.ArquivoId).HasColumnName("ArquivoId");
            this.Property(t => t.CampanhaId).HasColumnName("CampanhaId");
            this.Property(t => t.PeriodoId).HasColumnName("PeriodoId");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ParticipanteId).HasColumnName("ParticipanteId");
            this.Property(t => t.Erro).HasColumnName("Erro");
            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
            this.Property(t => t.DataAlteracao).HasColumnName("DataAlteracao");

            // Relationships
            this.HasOptional(t => t.Participante)
                .WithMany(t => t.CampanhaMetaPessoaImportacaos)
                .HasForeignKey(d => d.ParticipanteId);

        }
    }
}
