
namespace Reservation.Common.Models {
    /// <summary>
    /// This class contains properties describing a list of data returned from a method execution
    /// </summary>
    /// <typeparam name="T">type of the data returned from method execution</typeparam>

    public class ListResponse<T> : Response {
        /// <summary>
        /// this property contains the list of data returned from method execution
        /// </summary>
        public List<T> Result { get; set; }

        /// <summary>
        /// this property contains total count of data returned
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// this property contains the page number of data returned
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// this property contains the total count of the data returned as pages
        /// </summary>
        public int PageCount { get; set; }
    }
}
