using Reservation.Common.Models;
using Reservation.Models;

namespace Reservation.Services.Interfaces {
    /// <summary>
    /// Interface for Reservation service
    /// </summary>
    public interface IReservationService {

        /// <summary>
        /// This method will help for retrieving available slots.
        /// </summary>
        /// <param name="providerId">Integer,Provider identifier</param>
        /// <param name="startTime">DateTime, For filtering the data based on date time</param>
        /// <param name="endTime">DateTime, For filtering the data based on date time</param>
        /// <param name="page">Integer, which will represent the current page number, if we are not passing this will be considering as "1" page </param>
        /// <param name="pageCount">Integer, which will represent the no of records per page, if we are not passing this will be setting as "-1" which will return all records </param>
        /// <param name="sortColumn">string,Which will represent the column name which we can use for sorting, if we are not passing this, will be sorting with default column and here its "Start Date" </param>
        /// <param name="sortDirection">string,Which will represent sorting direction which we can use for soring, if we are not passing this will be sorting with default direction and here its "Ascending" </param>
        /// <returns>Returns an object of <see cref="SingleResponse{T}"/> whose generic type argument is <see cref="AvailableSlot"/> which will represent Available Slots                
        ///          Returns IsSuccess = true if there are no errors and along with that will be returning AvailableSlot <see cref="AvailableSlot"/>        
        ///          Returns IsSuccess  =   false if any error occurred  while processing.
        ///                  RetrunCode =   INTERNAL_SERVER_ERROR
        ///                  ReturnMessage= Internal Server error.
        ///         
        /// </returns>    
        SingleResponse<AvailableSlot> GetAvailableSlots(int? providerId, DateTime? startTime, DateTime? endTime, int? page = null, int? pageCount = null, string? sortColumn = null, string? sortDirection = null);

        /// <summary>
        /// This method will add a new provider slot 
        /// </summary>
        /// <param name="providerSlot">ProviderSlot,(<see cref="ProviderSlot"/>) which represents Provider Slot details,</param>
        /// <returns>Returns an object of <see cref="Response"/>.                
        ///          Returns IsSuccess = true if there are no errors        
        ///          Returns IsSuccess  =   false if any error occurred  while processing.
        ///                  RetrunCode =   INTERNAL_SERVER_ERROR
        ///                  ReturnMessage= Internal Server error.        
        /// </returns>          
        Response AddProviderSlot(Models.ProviderSlot providerSlot);

        /// <summary>
        /// This method will help for reserving a slot for a client
        /// </summary>
        /// <param name="clientProviderSlot">ClientSlot,(<see cref="ClientProviderSlot"/>) which represents Client Provider Slot details,</param>
        /// <returns>Returns an object of <see cref="Response"/>.                
        ///          Returns IsSuccess = true if there are no errors        
        ///          Returns IsSuccess  =   false if any error occurred  while processing.
        ///                  RetrunCode =   INTERNAL_SERVER_ERROR
        ///                  ReturnMessage= Internal Server error.        
        /// </returns>          
        SingleResponse<int> ReserveSlot(ClientProviderSlot clientProviderSlot);

        /// <summary>
        /// This method will help confirm the reservation.
        /// </summary>
        /// <param name="confirmSlot">ConfirmSlot,(<see cref="ConfirmSlot"/>) which represents Client Provider Slot confirmation details,</param>
        /// <returns>Returns an object of <see cref="Response"/>.                
        ///          Returns IsSuccess = true if there are no errors        
        ///          Returns IsSuccess  =   false if any error occurred  while processing.
        ///                  RetrunCode =   INTERNAL_SERVER_ERROR
        ///                  ReturnMessage= Internal Server error.        
        /// </returns>          
        Response ConfirmReservation(ConfirmSlot confirmSlot);
    }
}
