namespace ClinicBooking.Domain.Entities;

public class Clinic
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public ICollection<Provider> Providers { get; set; } = new List<Provider>();
}
