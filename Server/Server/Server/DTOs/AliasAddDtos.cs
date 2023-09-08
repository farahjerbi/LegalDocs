using Server.Domain;

namespace Server.DTOs
{

    public record struct AliasAddDtos(
    Guid Id, string? EntityName, string? Title, Guid? Group_FK,
    string? Type, string? Display, string? Source, TypeSettingsAddDto TypeSetting);

}
