namespace Server.DTOs
{
    public record struct AssignAliasDto(Guid Id, string DefaultValue , IFormFile? ImageFile);
}
