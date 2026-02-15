namespace EpsilonWebApp.Shared.Models
{
    /// <summary>
    /// Represents detailed address components returned by OpenStreetMap.
    /// </summary>
    public class OsmAddress
    {
        /// <summary>Gets or sets the road name.</summary>
        public string? Road { get; set; }
        /// <summary>Gets or sets the house number.</summary>
        public string? House_Number { get; set; }
        /// <summary>Gets or sets the city name.</summary>
        public string? City { get; set; }
        /// <summary>Gets or sets the town name.</summary>
        public string? Town { get; set; }
        /// <summary>Gets or sets the village name.</summary>
        public string? Village { get; set; }
        /// <summary>Gets or sets the suburb name.</summary>
        public string? Suburb { get; set; }
        /// <summary>Gets or sets the state or region name.</summary>
        public string? State { get; set; }
        /// <summary>Gets or sets the postal or zip code.</summary>
        public string? Postcode { get; set; }
        /// <summary>Gets or sets the country name.</summary>
        public string? Country { get; set; }
    }

    /// <summary>
    /// Represents a search result from the OpenStreetMap Nominatim API.
    /// </summary>
    public class OsmSearchResult
    {
        /// <summary>Gets or sets the full display name of the address.</summary>
        public string Display_Name { get; set; } = string.Empty;
        /// <summary>Gets or sets the detailed address components.</summary>
        public OsmAddress Address { get; set; } = new();
    }
}
