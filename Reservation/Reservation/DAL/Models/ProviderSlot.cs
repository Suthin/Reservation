namespace Reservation.DAL.Models {
    /// <summary>
    /// Represent database ProviderSlot table
    /// </summary>
    public class ProviderSlot:CommonBase {

        public int ProviderSlotId { get; set; }
        public int ProviderId { get; set; }
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
       
        public Provider Provider { get; set; }
    }
}
