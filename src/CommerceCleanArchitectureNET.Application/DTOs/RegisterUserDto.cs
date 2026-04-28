namespace CommerceCleanArchitectureNET.Application.DTOs
{
    public record RegisterUserDto(
        string Email,
        string Password,
        string FirstName,
        string LastName
    );
}
