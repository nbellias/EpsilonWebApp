namespace EpsilonWebApp.Shared.Models
{
    public class OsmAddress
    {
        public string? Road { get; set; }
        public string? House_Number { get; set; }
        public string? City { get; set; }
        public string? Town { get; set; }
        public string? Village { get; set; }
        public string? Suburb { get; set; }
        public string? State { get; set; }
        public string? Postcode { get; set; }
        public string? Country { get; set; }
    }

    public class OsmSearchResult
    {
        public string Display_Name { get; set; } = string.Empty;
        public OsmAddress Address { get; set; } = new();
    }
}
