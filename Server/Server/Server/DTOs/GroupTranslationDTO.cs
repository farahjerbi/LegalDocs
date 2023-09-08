namespace Server.DTOs
{
     public record struct GroupTranslationDTO(string TitleSection, List<AliasTranslationDTO> Aliases);
}
