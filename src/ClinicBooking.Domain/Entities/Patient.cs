namespace ClinicBooking.Domain.Entities;

public class Patient
{
    public long Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string Region { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    public string DateOfBirth { get; set; } = string.Empty;

    public string Gender {get; set; } = string.Empty;
    public string FolderNumber { get; set; } = string.Empty;
    public int IdNumber { get; set; }
    public string PassportNumber { get; set; } = string.Empty;

}
