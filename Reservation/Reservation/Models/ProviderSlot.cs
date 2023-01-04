namespace Reservation.Models {
    public class ProviderSlot {
      
        public int ProviderId { get; set; }
        public DateTime StartTimeInUtc { get; set; }
        public DateTime EndTimeInUtc { get; set; }        
        
    }
}
