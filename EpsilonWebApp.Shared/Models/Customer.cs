using System.ComponentModel.DataAnnotations;

namespace EpsilonWebApp.Shared.Models
{
    public class Customer
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Η Επωνυμία Εταιρείας είναι υποχρεωτική")]
        public string? CompanyName { get; set; }

        public string? ContactName { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }

        [Phone(ErrorMessage = "Μη έγκυρη μορφή τηλεφώνου")]
        [RegularExpression(@"^(\+)?([\s-]?\d){7,15}[\s-]?$", ErrorMessage = "Το τηλέφωνο πρέπει να περιέχει από 7 έως 15 ψηφία και προαιρετικά '+' ή κενά")]
        public string? Phone { get; set; }
    }
}
