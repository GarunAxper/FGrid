using System;

namespace FGrid.Persistence.Models
{
    public class TestUser
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public Gender Gender { get; set; }
        public int Age { get; set; }
        public string Phone { get; set; }
        public Address Address { get; set; }
        public string Notes { get; set; }
        public DateTime CreationDate { get; set; }
    }
}