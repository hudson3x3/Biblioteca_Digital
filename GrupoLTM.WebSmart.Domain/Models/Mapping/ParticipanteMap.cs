using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class ParticipanteMap : EntityTypeConfiguration<Participante>
    {
        public ParticipanteMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            
            this.Property(t => t.Login)
                .HasMaxLength(255);

            this.Property(t => t.Senha)
                .HasMaxLength(255);

            this.Property(t => t.Nome)
                .HasMaxLength(510);

            this.Property(t => t.RazaoSocial)
                .HasMaxLength(255);

            this.Property(t => t.NomeFantasia)
                .HasMaxLength(255);

            this.Property(t => t.CNPJ)
                .HasMaxLength(20);

            this.Property(t => t.CPF)
                .HasMaxLength(254);

            this.Property(t => t.RG)
                .HasMaxLength(15);

            this.Property(t => t.Sexo)
                .IsFixedLength()
                .HasMaxLength(10);

            this.Property(t => t.Endereco)
                .HasMaxLength(255);

            this.Property(t => t.Numero)
                .HasMaxLength(50);

            this.Property(t => t.Complemento)
                .HasMaxLength(255);

            this.Property(t => t.PontoReferencia)
                .HasMaxLength(255);

            this.Property(t => t.Bairro)
                .HasMaxLength(250);

            this.Property(t => t.CEP)
                .HasMaxLength(20);

            this.Property(t => t.Cidade)
                .HasMaxLength(250);

            this.Property(t => t.DDDCel)
                .HasMaxLength(4);

            this.Property(t => t.Celular)
                .HasMaxLength(20);

            this.Property(t => t.DDDTel)
                .HasMaxLength(4);

            this.Property(t => t.Telefone)
                .HasMaxLength(20);

            this.Property(t => t.DDDTelComercial)
                .HasMaxLength(4);

            this.Property(t => t.TelefoneComercial)
                .HasMaxLength(20);

            this.Property(t => t.Email)
                .HasMaxLength(320);

            // Table & Column Mappings
            this.ToTable("Participante");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Login).HasColumnName("Login");
            this.Property(t => t.Senha).HasColumnName("Senha");
            this.Property(t => t.StatusId).HasColumnName("StatusId");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.RazaoSocial).HasColumnName("RazaoSocial");
            this.Property(t => t.NomeFantasia).HasColumnName("NomeFantasia");
            this.Property(t => t.CNPJ).HasColumnName("CNPJ");
            this.Property(t => t.CPF).HasColumnName("CPF");
            this.Property(t => t.RG).HasColumnName("RG");
            this.Property(t => t.Sexo).HasColumnName("Sexo");
            this.Property(t => t.DataNascimento).HasColumnName("DataNascimento");
            this.Property(t => t.Endereco).HasColumnName("Endereco");
            this.Property(t => t.Numero).HasColumnName("Numero");
            this.Property(t => t.Complemento).HasColumnName("Complemento");
            this.Property(t => t.Bairro).HasColumnName("Bairro");
            this.Property(t => t.CEP).HasColumnName("CEP");
            this.Property(t => t.Cidade).HasColumnName("Cidade");
            this.Property(t => t.EstadoId).HasColumnName("EstadoId");
            this.Property(t => t.DDDCel).HasColumnName("DDDCel");
            this.Property(t => t.Celular).HasColumnName("Celular");
            this.Property(t => t.DDDTel).HasColumnName("DDDTel");
            this.Property(t => t.Telefone).HasColumnName("Telefone");
            this.Property(t => t.DDDTelComercial).HasColumnName("DDDTelComercial");
            this.Property(t => t.TelefoneComercial).HasColumnName("TelefoneComercial");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.Ativo).HasColumnName("Ativo");
            this.Property(t => t.DataCadastro).HasColumnName("DataCadastro");
            this.Property(t => t.DataDesligamento).HasColumnName("DataDesligamento");
            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
            this.Property(t => t.DataAlteracao).HasColumnName("DataAlteracao");
            this.Property(t => t.ParticipanteTeste).HasColumnName("ParticipanteTeste");
            this.Property(t => t.ParticipanteVago).HasColumnName("ParticipanteVago");
            this.Property(t => t.OptInEmail).HasColumnName("OptInEmail");
            this.Property(t => t.OptInComunicacaoFisica).HasColumnName("OptInComunicacaoFisica");
            this.Property(t => t.OptInSms).HasColumnName("OptInSms");
            this.Property(t => t.OptinAceite).HasColumnName("OptinAceite");
            this.Property(t => t.SimuladorVisualizado).HasColumnName("SimuladorVisualizado");

            // Relationships
            this.HasOptional(t => t.Estado)
                .WithMany(t => t.Participantes)
                .HasForeignKey(d => d.EstadoId);

            this.HasRequired(t => t.StatusParticipante)
                .WithMany(t => t.Participantes)
                .HasForeignKey(d => d.StatusId);



        }
    }
}
