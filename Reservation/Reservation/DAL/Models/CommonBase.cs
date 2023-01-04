namespace Reservation.DAL.Models {
    /// <summary>
    /// Its the base class for all models
    /// </summary>
    public class CommonBase {

        public int Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime LastUpdatedOn { get; set; }


    }
}
