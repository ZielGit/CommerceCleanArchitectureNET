namespace CommerceCleanArchitectureNET.Application.DTOs
{
    public record AuthResponseDto(
        string Token,
        UserDto User
    );
}
