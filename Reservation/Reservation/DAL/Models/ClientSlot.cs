namespace Reservation.DAL.Models {
    /// <summary>
    /// Represent database clientSlot table
    /// </summary>
    public class ClientSlot:CommonBase {

        public int ClientSlotId { get; set; }
        public int ProviderSlotId { get; set; }
        public int ClientId { get; set; }
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }        

        public ProviderSlot ProviderSlot { get; set; }

        public Client Client { get; set; }
    }
}
