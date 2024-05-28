using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class CampanhaCalculadaVendeuGanhouPessoaImportacaoMap : EntityTypeConfiguration<CampanhaCalculadaVendeuGanhouPessoaImportacao>
    {
        public CampanhaCalculadaVendeuGanhouPessoaImportacaoMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Login)
                .HasMaxLength(255);

            this.Property(t => t.Efetivo)
                .HasMaxLength(255);

            this.Property(t => t.Estrutura)
                .HasMaxLength(255);

            this.Property(t => t.Perfil)
                .HasMaxLength(255);

            this.Property(t => t.Pontos)
                .HasMaxLength(255);

            this.Property(t => t.Erro)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("CampanhaCalculadaVendeuGanhouPessoaImportacao");
            this.Property(t => t.Login).HasColumnName("Login");
            this.Property(t => t.Efetivo).HasColumnName("Efetivo");
            this.Property(t => t.Estrutura).HasColumnName("Estrutura");
            this.Property(t => t.Perfil).HasColumnName("Perfil");
            this.Property(t => t.Pontos).HasColumnName("Pontos");
            this.Property(t => t.ArquivoId).HasColumnName("ArquivoId");
            this.Property(t => t.CampanhaId).HasColumnName("CampanhaId");
            this.Property(t => t.CampanhaPeriodoId).HasColumnName("CampanhaPeriodoId");
            this.Property(t => t.CampanhaEstruturaId).HasColumnName("CampanhaEstruturaId");
            this.Property(t => t.CampanhaPerfilId).HasColumnName("CampanhaPerfilId");
            this.Property(t => t.EstruturaId).HasColumnName("EstruturaId");
            this.Property(t => t.PerfilId).HasColumnName("PerfilId");
            this.Property(t => t.ParticipanteId).HasColumnName("ParticipanteId");
            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
            this.Property(t => t.DataAlteracao).HasColumnName("DataAlteracao");
            this.Property(t => t.Erro).HasColumnName("Erro");
            this.Property(t => t.ParticipanteEstruturaId).HasColumnName("ParticipanteEstruturaId");
            this.Property(t => t.ParticipantePerfilId).HasColumnName("ParticipantePerfilId");
            this.Property(t => t.Id).HasColumnName("Id");
        }
    }
}
