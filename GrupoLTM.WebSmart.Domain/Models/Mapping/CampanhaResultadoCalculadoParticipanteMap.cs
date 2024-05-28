using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class CampanhaResultadoCalculadoParticipanteMap : EntityTypeConfiguration<CampanhaResultadoCalculadoParticipante>
    {
        public CampanhaResultadoCalculadoParticipanteMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("CampanhaResultadoCalculadoParticipante");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CampanhaId).HasColumnName("CampanhaId");
            this.Property(t => t.CampanhaPerfilId).HasColumnName("CampanhaPerfilId");
            this.Property(t => t.CampanhaEstruturaId).HasColumnName("CampanhaEstruturaId");
            this.Property(t => t.CampanhaPeriodoId).HasColumnName("CampanhaPeriodoId");
            this.Property(t => t.ParticipanteId).HasColumnName("ParticipanteId");
            this.Property(t => t.GrupoItemId).HasColumnName("GrupoItemId");
            this.Property(t => t.Meta).HasColumnName("Meta");
            this.Property(t => t.Efetivo).HasColumnName("Efetivo");
            this.Property(t => t.PercentualAtingimento).HasColumnName("PercentualAtingimento");
            this.Property(t => t.PosicaoRanking).HasColumnName("PosicaoRanking");
            this.Property(t => t.Pontos).HasColumnName("Pontos");
            this.Property(t => t.Ativo).HasColumnName("Ativo");
            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
            this.Property(t => t.DataAlteracao).HasColumnName("DataAlteracao");
            this.Property(t => t.DataInativacao).HasColumnName("DataInativacao");

            // Relationships
            this.HasRequired(t => t.Campanha)
                .WithMany(t => t.CampanhaResultadoCalculadoParticipantes)
                .HasForeignKey(d => d.CampanhaId);
            this.HasRequired(t => t.CampanhaEstrutura)
                .WithMany(t => t.CampanhaResultadoCalculadoParticipantes)
                .HasForeignKey(d => d.CampanhaEstruturaId);
            this.HasRequired(t => t.CampanhaPerfil)
                .WithMany(t => t.CampanhaResultadoCalculadoParticipantes)
                .HasForeignKey(d => d.CampanhaPerfilId);
            this.HasRequired(t => t.CampanhaPeriodo)
                .WithMany(t => t.CampanhaResultadoCalculadoParticipantes)
                .HasForeignKey(d => d.CampanhaPeriodoId);
            this.HasOptional(t => t.GrupoItem)
                .WithMany(t => t.CampanhaResultadoCalculadoParticipantes)
                .HasForeignKey(d => d.GrupoItemId);
            this.HasRequired(t => t.Participante)
                .WithMany(t => t.CampanhaResultadoCalculadoParticipantes)
                .HasForeignKey(d => d.ParticipanteId);

        }
    }
}
