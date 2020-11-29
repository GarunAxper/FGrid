using Microsoft.EntityFrameworkCore;

namespace FGrid.Persistence.Models
{
    [Owned]
    public class Address
    {
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
    }
}