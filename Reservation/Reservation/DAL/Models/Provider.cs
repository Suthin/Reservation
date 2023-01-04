namespace Reservation.DAL.Models {
    /// <summary>
    /// Represent database Provider table
    /// </summary>
    public class Provider {

        public int ProviderId { get; set; }
        public string ProviderName { get; set; } = String.Empty;
        public string? Description { get; set; }

        public int Status { get; set; }
    }
}
