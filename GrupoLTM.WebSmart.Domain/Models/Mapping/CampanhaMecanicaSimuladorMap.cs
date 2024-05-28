using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class CampanhaMecanicaSimuladorMap : EntityTypeConfiguration<CampanhaMecanicaSimulador>
    {
        public CampanhaMecanicaSimuladorMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("CampanhaMecanicaSimulador");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
            this.Property(t => t.DataAlteracao).HasColumnName("DataAlteracao");
            this.Property(t => t.Ativo).HasColumnName("Ativo");

            // Relationships
            this.HasRequired(t => t.MecanicaSimulador)
                .WithMany(t => t.CampanhaMecanicaSimulador)
                .HasForeignKey(t => t.IdMecanicaSimulador);

            this.HasRequired(t => t.CampanhaSimulador)
                .WithMany(t => t.CampanhaMecanicaSimulador)
                .HasForeignKey(t => t.IdCampanhaSimulador);


        }
    }
}
