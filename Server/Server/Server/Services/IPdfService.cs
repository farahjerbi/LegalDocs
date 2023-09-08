using Server.DTOs;

namespace Server.Services
{
    public interface IPdfService
    {
        public string RenderStandardTemplate(TemplateAddDto templateDto);

    }
}
