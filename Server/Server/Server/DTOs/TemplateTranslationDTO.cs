namespace Server.DTOs
{
    public record struct TemplateTranslationDTO(string Name, List<GroupTranslationDTO> Groups);
}
