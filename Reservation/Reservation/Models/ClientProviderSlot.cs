namespace Reservation.Models {
    public class ClientProviderSlot {

        public int ProviderId { get; set; }

        public int ClientId { get; set; }

        public DateTime StartTimeInUtc { get; set; }


    }
}
