//using System.Data.Entity.ModelConfiguration;

//namespace GrupoLTM.WebSmart.Domain.Models.Mapping
//{
//    public class UsuarioAdmAlunoMap : EntityTypeConfiguration<UsuarioAdmAluno>
//    {
//        public UsuarioAdmAlunoMap()
//        {
//            // Primary Key
//            this.HasKey(t => t.Id);

//            // Properties
//            // Table & Column Mappings
//            this.ToTable("UsuarioAdmAluno");
//            this.Property(t => t.Id).HasColumnName("Id");
//            this.Property(t => t.UsuarioAdmId).HasColumnName("UsuarioAdmId");
//            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
//            this.Property(t => t.DataAlteracao).HasColumnName("DataAlteracao");
//            this.Property(t => t.Matricula).HasColumnName("Matricula");
//            this.Property(t => t.SerieCursar).HasColumnName("SerieCursar");
//            this.Property(t => t.Periodo).HasColumnName("Periodo");

//            // Relationships
//            this.HasRequired(t => t.UsuarioAdm)
//                .WithMany(t => t.UsuarioAdmAluno)
//                .HasForeignKey(d => d.UsuarioAdmId);

//        }
//    }
//}
