namespace Server.DTOs
{
 
    public record struct GroupAddDto(
        string GroupId, string TitleSection, bool IsRepeatCard, bool Disabled, int GridDisplay,List<AliasAddDtos> Aliases);

}
