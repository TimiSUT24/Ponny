namespace HairdresserClassLibrary.DTOs.User;

public record UserResponseDto : UserDto
{
    public string Role { get; set; } = string.Empty;
}
