using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Server.Domain;
using Server.DTOs;

namespace Server.Configuration
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Template, TemplateAddDto>().ReverseMap();
            CreateMap<TemplateAddDto, TemplateTranslationDTO>().ReverseMap();
            CreateMap<Group, GroupAddDto>().ReverseMap();
            CreateMap<GroupAddDto, GroupTranslationDTO>().ReverseMap();
            CreateMap<Alias, AliasAddDtos>().ReverseMap();
            CreateMap<AliasAddDtos, AliasTranslationDTO>().ReverseMap();
            CreateMap<Alias, AssignAliasDto>().ReverseMap(); ;
            CreateMap<TypeSettings, TypeSettingsAddDto>().ReverseMap(); ;

        }
     
    }
}
