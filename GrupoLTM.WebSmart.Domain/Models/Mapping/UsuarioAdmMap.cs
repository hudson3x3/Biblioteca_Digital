using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class UsuarioAdmMap : EntityTypeConfiguration<UsuarioAdm>
    {
        public UsuarioAdmMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Login)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Senha)
                .IsRequired()
                .HasMaxLength(65);

            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(255);

           // this.Property(t => t.Periodo)
           //.IsRequired()
           //.HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("UsuarioAdm");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PerfilId).HasColumnName("PerfilId");
            this.Property(t => t.Login).HasColumnName("Login");
            this.Property(t => t.Senha).HasColumnName("Senha");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.Ativo).HasColumnName("Ativo");
            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
            this.Property(t => t.DataAlteracao).HasColumnName("DataAlteracao");
            this.Property(t => t.Matricula).HasColumnName("Matricula");
            this.Property(t => t.SerieCursar).HasColumnName("SerieCursar");
            this.Property(t => t.Periodo).HasColumnName("Periodo");

            // Relationships
            this.HasRequired(t => t.Perfil)
                .WithMany(t => t.UsuarioAdms)
                .HasForeignKey(d => d.PerfilId);

        }
    }
}
