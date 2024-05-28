using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class ConfiguracaoCampanhaMap : EntityTypeConfiguration<ConfiguracaoCampanha>
    {
        public ConfiguracaoCampanhaMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.NomeCampanha)
                .HasMaxLength(100);

            this.Property(t => t.LIVEAPI_ENDPOINT)
                .HasMaxLength(255);

            this.Property(t => t.LIVEAPI_URL)
                .HasMaxLength(255);

            this.Property(t => t.LIVEAPI_USERNAME)
                .HasMaxLength(50);

            this.Property(t => t.LIVEAPI_PASSWORD)
                .HasMaxLength(50);

            this.Property(t => t.LIVEAPI_COOKIENAME)
                .HasMaxLength(50);

            this.Property(t => t.LIVE_PROJECTCONFIGURATIONID)
                .HasMaxLength(50);

            this.Property(t => t.LIVEAPI_CLIENTEID)
                .HasMaxLength(50);

            this.Property(t => t.LIVEAPI_PROJECTID)
                .HasMaxLength(50);

            this.Property(t => t.EXLOGIN)
                .HasMaxLength(50);

            this.Property(t => t.EXSENHA)
                .HasMaxLength(50);

            this.Property(t => t.EXTEMPLATE_KEYBOASVINDAS)
                .HasMaxLength(50);

            this.Property(t => t.EXTEMPLATE_KEYESQUECISENHA)
                .HasMaxLength(50);

            this.Property(t => t.EXTEMPLATE_KEYFALECONOSCO)
                .HasMaxLength(50);

            this.Property(t => t.EMAILCREDITOPONTOS)
                .HasMaxLength(255);

            this.Property(t => t.EMAILFALECONOSCO)
                .HasMaxLength(255);

            this.Property(t => t.SMSLOGIN)
                .HasMaxLength(50);

            this.Property(t => t.SMSSENHA)
                .HasMaxLength(50);

            this.Property(t => t.ImgLogoCampanha)
                .HasMaxLength(50);

            this.Property(t => t.InstrucaoFaleConosco)
                .HasMaxLength(255);

            //this.Property(t => t.LIVE_URLCatalogo)
            //    .HasMaxLength(100);

            //this.Property(t => t.EXTEMPLATE_KEYCadastroUsuarioAdm)
            //    .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("ConfiguracaoCampanha");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.NomeCampanha).HasColumnName("NomeCampanha");
            this.Property(t => t.TipoAcessoId).HasColumnName("TipoAcessoId");
            this.Property(t => t.TipoCadastroId).HasColumnName("TipoCadastroId");
            this.Property(t => t.TipoValidacaoPositivaId).HasColumnName("TipoValidacaoPositivaId");
            this.Property(t => t.AtivoWP).HasColumnName("AtivoWP");
            this.Property(t => t.AtivoBoxSaldo).HasColumnName("AtivoBoxSaldo");
            this.Property(t => t.AtivoBoxVitrine).HasColumnName("AtivoBoxVitrine");
            this.Property(t => t.LIVEAPI_ENDPOINT).HasColumnName("LIVEAPI_ENDPOINT");
            this.Property(t => t.LIVEAPI_URL).HasColumnName("LIVEAPI_URL");
            this.Property(t => t.LIVEAPI_USERNAME).HasColumnName("LIVEAPI_USERNAME");
            this.Property(t => t.LIVEAPI_PASSWORD).HasColumnName("LIVEAPI_PASSWORD");
            this.Property(t => t.LIVEAPI_COOKIENAME).HasColumnName("LIVEAPI_COOKIENAME");
            this.Property(t => t.LIVE_PROJECTCONFIGURATIONID).HasColumnName("LIVE_PROJECTCONFIGURATIONID");
            this.Property(t => t.LIVEAPI_CLIENTEID).HasColumnName("LIVEAPI_CLIENTEID");
            this.Property(t => t.LIVEAPI_PROJECTID).HasColumnName("LIVEAPI_PROJECTID");
            this.Property(t => t.EXLOGIN).HasColumnName("EXLOGIN");
            this.Property(t => t.EXSENHA).HasColumnName("EXSENHA");
            this.Property(t => t.EXTEMPLATE_KEYBOASVINDAS).HasColumnName("EXTEMPLATE_KEYBOASVINDAS");
            this.Property(t => t.EXTEMPLATE_KEYESQUECISENHA).HasColumnName("EXTEMPLATE_KEYESQUECISENHA");
            this.Property(t => t.EXTEMPLATE_KEYFALECONOSCO).HasColumnName("EXTEMPLATE_KEYFALECONOSCO");
            this.Property(t => t.EMAILCREDITOPONTOS).HasColumnName("EMAILCREDITOPONTOS");
            this.Property(t => t.EMAILFALECONOSCO).HasColumnName("EMAILFALECONOSCO");
            this.Property(t => t.GOOGLEANALITYCS).HasColumnName("GOOGLEANALITYCS");
            this.Property(t => t.AtivoEsqueciSenhaSMS).HasColumnName("AtivoEsqueciSenhaSMS");
            this.Property(t => t.SMSLOGIN).HasColumnName("SMSLOGIN");
            this.Property(t => t.SMSSENHA).HasColumnName("SMSSENHA");
            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
            this.Property(t => t.DataAlteracao).HasColumnName("DataAlteracao");
            this.Property(t => t.AtivoTema).HasColumnName("AtivoTema");
            this.Property(t => t.ImgLogoCampanha).HasColumnName("ImgLogoCampanha");
            this.Property(t => t.TemaId).HasColumnName("TemaId");
            this.Property(t => t.InstrucaoFaleConosco).HasColumnName("InstrucaoFaleConosco");
            //this.Property(t => t.LIVE_URLCatalogo).HasColumnName("LIVE_URLCatalogo");
            //this.Property(t => t.EXTEMPLATE_KEYCadastroUsuarioAdm).HasColumnName("EXTEMPLATE_KEYCadastroUsuarioAdm");

            // Relationships
            this.HasOptional(t => t.Tema)
                .WithMany(t => t.ConfiguracaoCampanhas)
                .HasForeignKey(d => d.TemaId);
            this.HasRequired(t => t.TipoAcesso)
                .WithMany(t => t.ConfiguracaoCampanhas)
                .HasForeignKey(d => d.TipoAcessoId);
            this.HasRequired(t => t.TipoCadastro)
                .WithMany(t => t.ConfiguracaoCampanhas)
                .HasForeignKey(d => d.TipoCadastroId);
            this.HasRequired(t => t.TipoValidacaoPositiva)
                .WithMany(t => t.ConfiguracaoCampanhas)
                .HasForeignKey(d => d.TipoValidacaoPositivaId);

        }
    }
}
