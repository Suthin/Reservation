namespace Reservation.Models {
    public class AvailableSlot {

        public List<AvailableSlotDetail> AvailableProviderSlots { get; set; }

        public int TotalAvailableProviderSlots { get; set; }

        public List<SlotDetail> ReservedSlots { get; set; }

        public int TotalReservedSlots { get; set; }

    }
    public class AvailableSlotDetail: ProviderSlot {

        public string ProviderName { get; set; }=String.Empty;

    }

    public class SlotDetail {

        public int ProviderId { get; set; }
        public string ProviderName { get; set; } = String.Empty;
        public DateTime StartTimeInUtc { get; set; }
        public DateTime EndTimeInUtc { get; set; }
        public int Status { get; set; }

    }


}
