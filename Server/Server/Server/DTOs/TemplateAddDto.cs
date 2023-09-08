using Server.Controllers;

namespace Server.DTOs
{
    public record struct TemplateAddDto(
        string Id, string Name, string State ,string Style, int GridDisplay, Guid IdLanguage, Guid IdDoc, List<GroupAddDto> Groups );
}
