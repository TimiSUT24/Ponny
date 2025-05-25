namespace Hairdresser.DTOs.User
{
    public record UserRespondDto : UserDTO
    {
        public string Role { get; set; } = string.Empty;
    }
}
