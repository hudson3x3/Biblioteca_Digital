using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class ParticipantePerfilMap : EntityTypeConfiguration<ParticipantePerfil>
    {
        public ParticipantePerfilMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("ParticipantePerfil");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PerfilId).HasColumnName("PerfilId");
            this.Property(t => t.ParticipanteId).HasColumnName("ParticipanteId");
            this.Property(t => t.Ativo).HasColumnName("Ativo");
            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
            this.Property(t => t.DataAlteracao).HasColumnName("DataAlteracao");

            // Relationships
            this.HasRequired(t => t.Participante)
                .WithMany(t => t.ParticipantePerfil)
                .HasForeignKey(d => d.ParticipanteId);
            this.HasRequired(t => t.Perfil)
                .WithMany(t => t.ParticipantePerfil)
                .HasForeignKey(d => d.PerfilId);

        }
    }
}
