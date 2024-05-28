using AutoMapper;
using GrupoLTM.WebSmart.Admin.Models;
using GrupoLTM.WebSmart.Domain.Models;
using System.Linq;

namespace GrupoLTM.WebSmart.Admin.Configuration.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            ViewModelToDomain();
            DomainToViewModel();   
        }

        private void ViewModelToDomain()
        {
            CreateMap<SMSAgendamentoModel, SMSAgendamento>()
                .ForMember("Ativo", opt => opt.Ignore())
                .ForSourceMember("MktPlaceCatalogo", opt => opt.Ignore())
                .ForSourceMember("RecorrenciaDescricao", opt => opt.Ignore())
                .ForSourceMember("TipoBase", opt => opt.Ignore())
                .ForSourceMember("ArquivoUploadBaseRA", opt => opt.Ignore())
                .ForSourceMember("UploadBaseRANome", opt => opt.Ignore())
                .ForSourceMember("UploadBaseRANomeGerado", opt => opt.Ignore())
                .ForSourceMember("ProgramasId", opt => opt.Ignore());
                //.ForSourceMember("TipoBaseImagem", opt => opt.Ignore())
               // .ForSourceMember("UploadBaseRA", opt => opt.Ignore())
               // .ForSourceMember("UploadBaseImagem", opt => opt.Ignore());

            CreateMap<SMSExecucaoModel, SMSExecucao>()
                .ForMember("SMSAgendamento", opt => opt.Ignore());

        }

        private void DomainToViewModel()
        {
            CreateMap<SMSAgendamento, SMSAgendamentoModel>()
                .ForMember("MktPlaceCatalogo", opt => opt.Ignore())
                .ForMember("RecorrenciaDescricao", opt => opt.Ignore())
                .ForMember("TipoBase", opt => opt.Ignore())
                .ForMember("ArquivoUploadBaseRA", opt => opt.Ignore())
                .ForMember("UploadBaseRANome", opt => opt.Ignore())
                .ForMember("UploadBaseRANomeGerado", opt => opt.Ignore())
                .ForMember("ProgramasId", opt => opt.Ignore())
                //.ForMember("TipoBaseImagem", opt => opt.Ignore())
                //.ForMember("UploadBaseRA", opt => opt.Ignore())
                //.ForMember("UploadBaseImagem", opt => opt.Ignore())
                .ForSourceMember("Ativo", opt => opt.Ignore());

            CreateMap<SMSExecucao, SMSExecucaoModel>()
                .ForSourceMember("SMSAgendamento", opt => opt.Ignore());
        }
    }
}