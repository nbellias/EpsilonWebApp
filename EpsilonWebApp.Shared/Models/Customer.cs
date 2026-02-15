using System.ComponentModel.DataAnnotations;

namespace EpsilonWebApp.Shared.Models
{
    /// <summary>
    /// Represents a customer entity in the system.
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// Gets or sets the unique identifier for the customer.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the company. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Η Επωνυμία Εταιρείας είναι υποχρεωτική")]
        public string? CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the name of the primary contact person.
        /// </summary>
        public string? ContactName { get; set; }

        /// <summary>
        /// Gets or sets the street address.
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        public string? City { get; set; }

        /// <summary>
        /// Gets or sets the region or state.
        /// </summary>
        public string? Region { get; set; }

        /// <summary>
        /// Gets or sets the postal or zip code.
        /// </summary>
        public string? PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        public string? Country { get; set; }

        /// <summary>
        /// Gets or sets the phone number. Must be between 7 and 15 digits.
        /// </summary>
        [Phone(ErrorMessage = "Μη έγκυρη μορφή τηλεφώνου")]
        [RegularExpression(@"^(\+)?([\s-]?\d){7,15}[\s-]?$", ErrorMessage = "Το τηλέφωνο πρέπει να περιέχει από 7 έως 15 ψηφία και προαιρετικά '+' ή κενά")]
        public string? Phone { get; set; }
    }
}
