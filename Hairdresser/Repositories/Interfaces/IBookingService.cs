namespace Hairdresser.Repositories.Interfaces
{
    public interface IBookingService
    {
        Task<List<DateTime>> GetAllAvailableTimes(string hairdresserId, int treatmentId, DateTime day);
    }
}
