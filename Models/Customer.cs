using System;

namespace InfoMasterKonsole.Models
{
    public class Customer
    {
        public string Id { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? CustomerType { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
