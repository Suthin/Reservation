namespace Reservation.DAL.Models {
    /// <summary>
    /// Represent database client table
    /// </summary>
    public class Client : CommonBase {
        public int ClientId { get; set; }
        public string ClientName { get; set; } = String.Empty;
        public string? Description { get; set; }

    }
}
