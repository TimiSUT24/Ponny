namespace Hairdresser.DTOs
{
    public record UserRespondDto : UserDTO
    {
        public string Role { get; set; } = string.Empty;
    }
}
