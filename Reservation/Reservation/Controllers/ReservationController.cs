using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reservation.Common.Constants;
using Reservation.Common.Models;
using Reservation.Models;
using Reservation.Services;
using Reservation.Utils;

namespace Reservation.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase {

        #region Class level Variables

        private readonly ILogger<ReservationController> _logger = LogManager.CreateLogger<ReservationController>();

        #endregion


        [HttpGet]
        [Route("availableslot")]

        /// <summary>
        /// This method is used for getting a available slots for the client.
        /// </summary>
        /// <param name="providerId">Integer, its an optional field if we are passing any value greater than 0 , will consider that as a provider id and filter based on that</param>
        /// <param name="startTimeInUtc">DateTime,its an optional field if we are passing this date, will be considering this as starting time for filtering  </param>
        /// <param name="endTimeInUtc">DateTime,its an optional field if we are passing this date, will be considering this as ending time for filtering </param>
        /// <param name="pageNo">Integer, which will represent the current page number, if we are not passing this will be considering as "1" page </param>
        /// <param name="pageSize">Integer, which will represent the no of records per page, if we are not passing this will be setting as "-1" which will return all records </param>
        /// <returns>Returns an object of <see cref="SingleResponse{T}"/> whose generic type argument is <see cref="AvailableSlot"/> which will represent Available slot details                
        ///          Returns IsSuccess = true if there are no errors and along with that will be returning list of matching Available slots <see cref="AvailableSlot"/>        
        ///          Returns IsSuccess  =   false if any error occurred  while processing.
        ///                  RetrunCode =   INTERNAL_SERVER_ERROR
        ///                  ReturnMessage= Internal Server error.        
        /// </returns>   
        public SingleResponse<AvailableSlot> GetAvailableSlots(int? providerId = null, DateTime? startTimeInUtc = null, DateTime? endTimeInUtc = null, int? pageNo = null, int? pageSize= null) {
            #region Initialization and Error Checking



            #endregion

            #region Method Logic

            try {

                //Now we can construct the required object and call the service method for saving the client provider slot

                ReservationService rsvc = new ReservationService();

                return rsvc.GetAvailableSlots(providerId, startTimeInUtc, endTimeInUtc, pageNo, pageSize, null, null);
            }
            catch (Exception ex) {

                //Log the error details
                _logger.LogError(ex, $"Error while processing ReserveSlot. Error:{ex.Message}");

                return new SingleResponse<AvailableSlot> { IsSuccess = false, ReturnCode = ResponseCodes.INTERNAL_SERVER_ERROR, ReturnMessage = "Internal server error." };

            }

            #endregion


        }

        [HttpPost]
        [Route("reserve")]

        /// <summary>
        /// This method is used reserving a slot for client. After successful reservation method will return resservationId which we can use for confirming the reservation 
        /// </summary>
        /// <param name="clientProviderSlot">ClientProviderSlot,<see cref="clientProviderSlot"/> Client provider slot details for reserving slot </param>
        /// <returns>Returns an object of <see cref="SingleResponse{T}"/> whose generic type argument is <see cref="int"/> which will represent the reservationId (ClientSlotId)               
        ///          Returns IsSuccess = true if there are no errors and along with it will return the newly created reservationId (ClientSlotId) 
        ///          Returns IsSuccess  =   false if any error occurred  while processing.
        ///                  RetrunCode =   INTERNAL_SERVER_ERROR
        ///                  ReturnMessage= Internal Server error.        
        /// </returns>   
        public SingleResponse<int> ReserveSlot([FromBody] ClientProviderSlot clientProviderSlot) {

            #region Initialization and Error Checking

            if (clientProviderSlot == null) {

                return new SingleResponse<int> { IsSuccess = false, ReturnCode = ResponseCodes.INVALID_PARAMETERS, ReturnMessage = "Invalid input parameter." };
            }
            else if (clientProviderSlot.ProviderId < 1) {
                return new SingleResponse<int> { IsSuccess = false, ReturnCode = ResponseCodes.INVALID_PARAMETERS, ReturnMessage = "Invalid input parameter: Should pass a valid ProviderId." };
            }
            else if (clientProviderSlot.ClientId < 1) {
                return new SingleResponse<int> { IsSuccess = false, ReturnCode = ResponseCodes.INVALID_PARAMETERS, ReturnMessage = "Invalid input parameter: Should pass a valid ClientId." };
            }
            else if (clientProviderSlot.StartTimeInUtc < DateTime.UtcNow.AddDays(1)) {
                return new SingleResponse<int> { IsSuccess = false, ReturnCode = ResponseCodes.INVALID_PARAMETERS, ReturnMessage = "Invalid input parameter: Reservations must be made at least 24 hours in advance." };
            }


            #endregion

            #region Method Logic

            try {

                //TO DO get the ClientId from token instead of passing that as parameter 

                //Now we can construct the required object and call the service method for saving the client provider slot

                ReservationService rsvc = new ReservationService();

                return rsvc.ReserveSlot(clientProviderSlot);
            }
            catch (Exception ex) {

                //Log the error details
                _logger.LogError(ex, $"Error while processing ReserveSlot. Error:{ex.Message}");

                return new SingleResponse<int> { IsSuccess = false, ReturnCode = ResponseCodes.INTERNAL_SERVER_ERROR, ReturnMessage = "Internal server error." };

            }

            #endregion

        }

        [HttpPost]
        [Route("confirm")]
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
        public Response ConfirmSlot([FromBody] ConfirmSlot confirmSlot) {

            #region Initialization and Error Checking

            if (confirmSlot == null) {

                return new Response { IsSuccess = false, ReturnCode = ResponseCodes.INVALID_PARAMETERS, ReturnMessage = "Invalid input parameter." };
            }
            else if (confirmSlot.ClientSlotId < 1) {
                return new Response { IsSuccess = false, ReturnCode = ResponseCodes.INVALID_PARAMETERS, ReturnMessage = "Invalid input parameter: Should pass a valid Client SlotId." };
            }
            else if (confirmSlot.ClientId < 1) {
                return new Response { IsSuccess = false, ReturnCode = ResponseCodes.INVALID_PARAMETERS, ReturnMessage = "Invalid input parameter: Should pass a valid ClientId." };
            }



            #endregion

            #region Method Logic

            try {

                //TO DO get the ClientId from token instead of passing that as parameter 

                //Now we can construct the required object and call the service method for confirming reservation

                ReservationService rsvc = new ReservationService();

                return rsvc.ConfirmReservation(confirmSlot);
            }
            catch (Exception ex) {

                //Log the error details
                _logger.LogError(ex, $"Error while processing ConfirmSlot. Error:{ex.Message}");

                return new Response { IsSuccess = false, ReturnCode = ResponseCodes.INTERNAL_SERVER_ERROR, ReturnMessage = "Internal server error." };

            }

            #endregion

        }

        [HttpPost]
        [Route("schedule")]
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
        public Response AddSchedule([FromBody] ProviderSlot providerSlot) {
            #region Initialization and Error Checking

            if (providerSlot == null) {

                return new Response { IsSuccess = false, ReturnCode = ResponseCodes.INVALID_PARAMETERS, ReturnMessage = "Invalid input parameter." };
            }
            else if (providerSlot.ProviderId < 1) {
                return new Response { IsSuccess = false, ReturnCode = ResponseCodes.INVALID_PARAMETERS, ReturnMessage = "Invalid input parameter: Should pass a valid ProviderId." };

            }
            else if (providerSlot.StartTimeInUtc > providerSlot.EndTimeInUtc) {
                return new Response { IsSuccess = false, ReturnCode = ResponseCodes.INVALID_PARAMETERS, ReturnMessage = "Invalid input parameter: Start time should be greater than end time." };
            }
            else if (providerSlot.StartTimeInUtc < DateTime.UtcNow) {
                return new Response { IsSuccess = false, ReturnCode = ResponseCodes.INVALID_PARAMETERS, ReturnMessage = "Invalid input parameter: Start time should be greater than current UTC time." };
            }


            #endregion

            #region Method Logic

            try {

                //TO DO get the ProviderId from token instead of passing that as parameter 

                //Now we can construct the required object and call the service method for saving the provider slot

                ReservationService rsvc = new ReservationService();

                return rsvc.AddProviderSlot(providerSlot);
            }
            catch (Exception ex) {

                //Log the error details
                _logger.LogError(ex, $"Error while processing AddSchedule. Error:{ex.Message}");

                return new Response { IsSuccess = false, ReturnCode = ResponseCodes.INTERNAL_SERVER_ERROR, ReturnMessage = "Internal server error." };

            }

            #endregion



        }
    }
}
