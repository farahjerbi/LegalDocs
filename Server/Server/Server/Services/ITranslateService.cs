using Newtonsoft.Json.Linq;
using Server.DTOs;

namespace Server.Services
{
    public interface ITranslateService
    {
        Task<TemplateAddDto> TranslateTemplate(TemplateAddDto template, string language);

    }
}
