namespace Server.DTOs
{
    public record struct TypeSettingsAddDto(string FieldType, string DefaultValue, bool ReadOnly, bool Required);

}
