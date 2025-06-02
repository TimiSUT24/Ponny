namespace HairdresserClassLibrary.DTOs.User;

public record UserRespondDto : UserDto
{
    public string Role { get; set; } = string.Empty;
}
