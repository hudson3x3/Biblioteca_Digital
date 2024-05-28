using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class ParticipanteCPMap : EntityTypeConfiguration<ParticipanteCP>
    {
        public ParticipanteCPMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ParticipanteId);
            this.Property(t => t.CatalogoCPId);
            this.Property(t => t.Ativo);
            this.Property(t => t.DataInclusao);
            this.Property(t => t.DataAlteracao);

            // Table & Column Mappings
            this.ToTable("ParticipanteCP");
            this.Property(t => t.ParticipanteId).HasColumnName("ParticipanteId");
            this.Property(t => t.Ativo).HasColumnName("Ativo");
            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
            this.Property(t => t.DataAlteracao).HasColumnName("DataAlteracao");

            this.HasRequired(t => t.Participante)
                .WithMany(t => t.ParticipanteCPs)
                .HasForeignKey(d => d.ParticipanteId);

            this.HasRequired(t => t.CatalogoCP)
                .WithMany(t => t.ParticipanteCPs)
                .HasForeignKey(d => d.CatalogoCPId);

        }
    }
}
