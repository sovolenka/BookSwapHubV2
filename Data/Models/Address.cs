using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public class Address
{
    [Key]
    public long Id { get; set; }
    
    public string Street { get; set; } = string.Empty;

    [MinLength(2)]
    [MaxLength(2000)]
    public string City { get; set; } = string.Empty;

    [MinLength(2)]
    [MaxLength(2000)]
    public string State { get; set; } = string.Empty;
    public string Zip { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"Street: {Street}, City: {City}, State: {State}, Zip: {Zip}, Country: {Country}";
    }
}