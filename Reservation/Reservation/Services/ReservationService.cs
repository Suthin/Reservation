using Microsoft.EntityFrameworkCore;
using Reservation.Common.Constants;
using Reservation.Common.Enums;
using Reservation.Common.Models;
using Reservation.DAL;
using Reservation.DAL.Models;
using Reservation.Models;
using Reservation.Services.Interfaces;
using Reservation.Utils;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Reservation.Services {
    /// <summary>
    /// Reservation Service methods
    /// </summary>
    public class ReservationService : IReservationService {

        private readonly ILogger<ReservationService> _logger = LogManager.CreateLogger<ReservationService>();

        private readonly object reservationLock = new object();


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
        public SingleResponse<AvailableSlot> GetAvailableSlots(int? providerId, DateTime? startTime, DateTime? endTime, int? page = null, int? pageCount = null, string? sortColumn = null, string? sortDirection = null) {


            try {

                #region Guard Clauses 

                if (page == null || (page.HasValue && page.Value < 1)) {
                    //Set the default page as 1
                    page = 1;
                }

                if (pageCount == null) {
                    //Set the default page size as 100
                    pageCount = 100;
                }

                if (string.IsNullOrWhiteSpace(sortColumn)) {

                    sortColumn = "StartTime";

                }
                if (string.IsNullOrWhiteSpace(sortDirection)) {

                    sortColumn = "DESC";

                }



                #endregion

                #region Method Logic


                bool showAllRecords = false;


                if (pageCount != null && pageCount.HasValue && pageCount.Value == -1) {

                    showAllRecords = true;
                }



                using (var context = new ReservationDBContext()) {


                    AvailableSlot resAvailableSlot = new AvailableSlot();


                    //TO DO Sorting


                    //First we can get the provider slots.


                    #region Provider Slots


                    //Filter only active slots
                    var providerSlotQuery = context.ProviderSlots.Include("Provider").Where(p => p.Status == (int) CommonStatus.Active); ;

                    if (providerId != null && providerId.HasValue && providerId.Value > 0) {

                        //Filter based on providerId
                        providerSlotQuery = providerSlotQuery.Where(p => p.ProviderId == providerId);
                    }

                    if (startTime != null && startTime.HasValue) {
                        //Filter based on start time

                        providerSlotQuery = providerSlotQuery.Where(p => p.StartTime >= startTime.Value);
                    }
                    if (endTime != null && endTime.HasValue) {

                        //Filter based on end time

                        providerSlotQuery = providerSlotQuery.Where(p => p.StartTime >= endTime.Value);
                    }

                    //get the total no of records
                    var totalCountProviderSlot = providerSlotQuery.Count();

                    resAvailableSlot.TotalAvailableProviderSlots = totalCountProviderSlot;




                    if (showAllRecords) {

                        pageCount = totalCountProviderSlot;

                    }

                    int offsetProviderSlot = (page.Value - 1) * pageCount.Value;



                    var resProviderSlots = providerSlotQuery.OrderBy(p => p.StartTime).Skip(offsetProviderSlot).Take(pageCount.Value).Select(p => new AvailableSlotDetail {
                        ProviderId = p.ProviderId,
                        ProviderName = p.Provider.ProviderName,
                        StartTimeInUtc = p.StartTime,
                        EndTimeInUtc = p.EndTime
                    }).ToList();

                    if (resProviderSlots != null && resProviderSlots.Any()) {

                        resAvailableSlot.AvailableProviderSlots = resProviderSlots;
                    }


                    #endregion


                    //Now we can get the Client reserved slots.


                    #region Reserved Slots

                    //filter only active and not expired records

                    var clientSlotQuery = context.ClientSlots.Include("ProviderSlot").Include("ProviderSlot.Provider").Where(p => p.Status == (int) CommonStatus.Active || (p.Status == (int) CommonStatus.Pending && p.CreatedOn > DateTime.UtcNow.AddMinutes(-30))); ;

                    if (providerId != null && providerId.HasValue && providerId.Value > 0) {

                        //Filter based on providerId
                        clientSlotQuery = clientSlotQuery.Where(p => p.ProviderSlot.ProviderId == providerId);
                    }

                    if (startTime != null && startTime.HasValue) {

                        //Filter based on start time
                        clientSlotQuery = clientSlotQuery.Where(p => p.StartTime >= startTime.Value);
                    }
                    if (endTime != null && endTime.HasValue) {

                        //filter based on end time
                        clientSlotQuery = clientSlotQuery.Where(p => p.StartTime >= endTime.Value);
                    }

                    //get the total no of records
                    var totalCountClientSlot = clientSlotQuery.Count();

                    resAvailableSlot.TotalReservedSlots = totalCountClientSlot;

                    if (showAllRecords) {

                        pageCount = totalCountClientSlot;

                    }

                    int offsetClientSlot = (page.Value - 1) * pageCount.Value;



                    var resClientSlots = clientSlotQuery.OrderBy(p => p.StartTime).Skip(offsetClientSlot).Take(pageCount.Value).Select(p => new SlotDetail {
                        StartTimeInUtc = p.StartTime,
                        EndTimeInUtc = p.EndTime,
                        ProviderId = p.ProviderSlot.ProviderId,
                        ProviderName = p.ProviderSlot.Provider.ProviderName,
                        Status = p.Status

                    }).ToList();

                    if (resClientSlots != null && resClientSlots.Any()) {

                        resAvailableSlot.ReservedSlots = resClientSlots;
                    }


                    #endregion



                    return new SingleResponse<AvailableSlot> { IsSuccess = true, ReturnCode = ResponseCodes.SUCCESS, Result = resAvailableSlot };
                }



                #endregion
            }
            catch (Exception ex) {

                //Log the error details
                _logger.LogError(ex, $"Error while processing GetAvailableSlots. Error:{ex.Message}");

                return new SingleResponse<AvailableSlot> { IsSuccess = false, ReturnCode = ResponseCodes.INTERNAL_SERVER_ERROR, ReturnMessage = "Internal server error." };


            }
        }

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
        public Response AddProviderSlot(Models.ProviderSlot providerSlot) {

            try {

                #region Guard Clauses 

                if (providerSlot == null || providerSlot.ProviderId < 1 || providerSlot.StartTimeInUtc > providerSlot.EndTimeInUtc || providerSlot.StartTimeInUtc < DateTime.UtcNow) {
                    //Log message invalid provider slot                   
                    _logger.LogError($"Error while processing AddProviderSlot. Error:Invalid input parameter");
                    return new Response { IsSuccess = false, ReturnCode = ResponseCodes.INVALID_PARAMETERS, ReturnMessage = "Invalid input parameter." };
                }


                #endregion

                #region Method Logic


                using (var context = new ReservationDBContext()) {

                    //Validate the providerId.

                    var currentProvider = context.Providers.FirstOrDefault(p => p.ProviderId == providerSlot.ProviderId && p.Status == (int) CommonStatus.Active);

                    if (currentProvider == null) {

                        //Log message invalid provider Id                   
                        _logger.LogError($"Error while processing AddProviderSlot. Error:Invalid providerId.");

                        return new Response { IsSuccess = false, ReturnCode = ResponseCodes.INVALID_IDENTIFIER, ReturnMessage = "Invalid providerId." };
                    }

                    //Validate the start time and end time

                    //for checking any overlapping of provider active slots.
                    //1. Start or end time should not be in between an existing active slots


                    var exisitngProviderSlots = context.ProviderSlots.Where(p => p.ProviderId == providerSlot.ProviderId && p.Status == (int) CommonStatus.Active && ((p.StartTime <= providerSlot.StartTimeInUtc && p.EndTime >= providerSlot.EndTimeInUtc) || (p.StartTime >= providerSlot.StartTimeInUtc && p.EndTime >= providerSlot.StartTimeInUtc)));

                    if (exisitngProviderSlots != null && exisitngProviderSlots.Any()) {

                        //Log message invalid provider Id                   
                        _logger.LogError($"Error while processing AddProviderSlot. Error:Overlapping with an existing slot.");

                        return new Response { IsSuccess = false, ReturnCode = ResponseCodes.INVALID_PARAMETERS, ReturnMessage = "Overlapping with an existing slot.." };

                    }


                    //Create new client slot object for saving

                    DAL.Models.ProviderSlot providerSlotNew = new DAL.Models.ProviderSlot {
                        EndTime = providerSlot.EndTimeInUtc,
                        ProviderId = providerSlot.ProviderId,
                        StartTime = providerSlot.StartTimeInUtc,
                        Status = (int) CommonStatus.Active,
                        CreatedOn = DateTime.UtcNow,
                        LastUpdatedOn = DateTime.UtcNow
                    };

                    context.ProviderSlots.Add(providerSlotNew);
                    context.SaveChanges();
                }

                return new Response { IsSuccess = true, ReturnCode = ResponseCodes.SUCCESS };

                #endregion
            }
            catch (Exception ex) {

                //Log the error details
                _logger.LogError(ex, $"Error while processing AddProviderSlot. Error:{ex.Message}");

                return new Response { IsSuccess = false, ReturnCode = ResponseCodes.INTERNAL_SERVER_ERROR, ReturnMessage = "Internal server error." };


            }
        }

        /// <summary>
        /// This method will help for reserving a slot for a client
        /// </summary>
        /// <param name="clientProviderSlot">ClientSlot,(<see cref="ClientProviderSlot"/>) which represents Client Provider Slot details,</param>
        /// <returns>Returns an object of <see cref="SingleResponse{T}"/> whose generic type argument is <see cref="int"/> which will represent the reservationId (ClientSlotId)               
        ///          Returns IsSuccess = true if there are no errors and along with it will return the newly created reservationId (ClientSlotId) 
        ///          Returns IsSuccess  =   false if any error occurred  while processing.
        ///                  RetrunCode =   INTERNAL_SERVER_ERROR
        ///                  ReturnMessage= Internal Server error.        
        /// </returns>           
        public SingleResponse<int> ReserveSlot(ClientProviderSlot clientProviderSlot) {

            try {

                #region Guard Clauses 

                if (clientProviderSlot == null || clientProviderSlot.ProviderId < 1 || clientProviderSlot.ClientId < 1 || clientProviderSlot.StartTimeInUtc <= DateTime.UtcNow.AddDays(1)) {
                    //Log message invalid provider slot                   
                    _logger.LogError($"Error while processing ReserveSlot. Error:Invalid input parameter");
                    return new SingleResponse<int> { IsSuccess = false, ReturnCode = ResponseCodes.INVALID_PARAMETERS, ReturnMessage = "Invalid input parameter." };
                }


                #endregion

                #region Method Logic


                using (var context = new ReservationDBContext()) {


                    //we can set the end time
                    DateTime endTime = clientProviderSlot.StartTimeInUtc.AddMinutes(15);


                    //Validate the provider slot

                    var currentProvider = context.ProviderSlots.FirstOrDefault(p => p.ProviderId == clientProviderSlot.ProviderId && p.StartTime <= clientProviderSlot.StartTimeInUtc && p.EndTime >= endTime && p.Status == (int) CommonStatus.Active);

                    if (currentProvider == null) {

                        //Log message invalid provider Id                   
                        _logger.LogError($"Error while processing ReserveSlot. Error:Invalid providerId or provider slot is not available for the current request.");

                        return new SingleResponse<int> { IsSuccess = false, ReturnCode = ResponseCodes.INVALID_IDENTIFIER, ReturnMessage = "Invalid providerId or provider slot is not available for the current request." };
                    }

                    //Validate the Client

                    var currentClient = context.Clients.FirstOrDefault(p => p.ClientId == clientProviderSlot.ClientId && p.Status == (int) CommonStatus.Active);

                    if (currentClient == null) {

                        //Log message invalid provider Id                   
                        _logger.LogError($"Error while processing ReserveSlot. Error:Invalid ClientId.");

                        return new SingleResponse<int> { IsSuccess = false, ReturnCode = ResponseCodes.INVALID_IDENTIFIER, ReturnMessage = "Invalid ClientId." };
                    }


                    //TO DO update the lock logic instead of locking entire

                    lock (reservationLock) {

                        //Now we should validate the current booking for the same slot.
                        var exisitngBooking = context.ClientSlots.FirstOrDefault(p => p.ProviderSlotId == currentProvider.ProviderSlotId && (p.Status == (int) CommonStatus.Active || p.Status == (int) CommonStatus.Pending) && ((p.StartTime <= clientProviderSlot.StartTimeInUtc && clientProviderSlot.StartTimeInUtc < p.EndTime) || (p.StartTime < endTime && endTime <= p.EndTime)));

                        if (exisitngBooking != null) {



                            if (exisitngBooking.Status == (int) CommonStatus.Pending && exisitngBooking.CreatedOn <= DateTime.UtcNow.AddMinutes(-30)) {

                                //Now we can make the existing reservation as expired and create new reservation. 

                                //Set the status as active
                                exisitngBooking.Status = (int) CommonStatus.Expired;
                                exisitngBooking.LastUpdatedOn = DateTime.UtcNow;

                                context.SaveChanges();


                            }
                            else {


                                //Log message invalid provider Id                   
                                _logger.LogError($"Error while processing ReserveSlot. Error: Selected slot is not available.");

                                return new SingleResponse<int> { IsSuccess = false, ReturnCode = ResponseCodes.INVALID_PARAMETERS, ReturnMessage = "Selected slot is not available." };
                            }

                        }

                        //Create new client slot object for saving

                        ClientSlot clientSlot = new ClientSlot {
                            ClientId = clientProviderSlot.ClientId,
                            StartTime = clientProviderSlot.StartTimeInUtc,
                            EndTime = endTime,
                            ProviderSlotId = currentProvider.ProviderSlotId,
                            Status = (int) CommonStatus.Pending,
                            CreatedOn = DateTime.UtcNow,
                            LastUpdatedOn = DateTime.UtcNow,
                        };


                        context.ClientSlots.Add(clientSlot);
                        context.SaveChanges();

                        return new SingleResponse<int> { IsSuccess = true, ReturnCode = ResponseCodes.SUCCESS, Result = clientSlot.ClientSlotId };
                    }
                }



                #endregion
            }
            catch (Exception ex) {

                //Log the error details
                _logger.LogError(ex, $"Error while processing ReserveSlot. Error:{ex.Message}");

                return new SingleResponse<int> { IsSuccess = false, ReturnCode = ResponseCodes.INTERNAL_SERVER_ERROR, ReturnMessage = "Internal server error." };


            }
        }

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
        public Response ConfirmReservation(ConfirmSlot confirmSlot) {

            try {

                #region Guard Clauses 

                if (confirmSlot == null || confirmSlot.ClientId < 1 || confirmSlot.ClientSlotId < 1) {
                    //Log message invalid provider slot                   
                    _logger.LogError($"Error while processing ConfirmReservation. Error:Invalid input parameter");
                    return new Response { IsSuccess = false, ReturnCode = ResponseCodes.INVALID_PARAMETERS, ReturnMessage = "Invalid input parameter." };
                }


                #endregion

                #region Method Logic


                using (var context = new ReservationDBContext()) {




                    //Validate the client slot

                    var currentClientSlot = context.ClientSlots.FirstOrDefault(p => p.ClientSlotId == confirmSlot.ClientSlotId && p.ClientId == confirmSlot.ClientId);

                    if (currentClientSlot == null) {

                        //Log message invalid provider Id                   
                        _logger.LogError($"Error while processing ConfirmReservation. Error:Invalid Client SlotId or ClientId.");

                        return new Response { IsSuccess = false, ReturnCode = ResponseCodes.INVALID_IDENTIFIER, ReturnMessage = "Invalid Client SlotId or ClientId." };
                    }

                    if (currentClientSlot.Status == (int) CommonStatus.Active) {

                        return new Response { IsSuccess = true, ReturnCode = ResponseCodes.ALREADY_COMPLETED, ReturnMessage = "Slot is already confirmed." };
                    }
                    else if (currentClientSlot.Status == (int) CommonStatus.Pending) {


                        if (currentClientSlot.CreatedOn <= DateTime.UtcNow.AddMinutes(-30)) {

                            //Set the status as active
                            currentClientSlot.Status = (int) CommonStatus.Expired;
                            currentClientSlot.LastUpdatedOn = DateTime.UtcNow;

                            context.SaveChanges();

                            return new Response { IsSuccess = false, ReturnCode = ResponseCodes.INVALID_STATE, ReturnMessage = "Reservation is already expired." };


                        }
                        else {

                            //Set the status as active
                            currentClientSlot.Status = (int) CommonStatus.Active;
                            currentClientSlot.LastUpdatedOn = DateTime.UtcNow;

                            context.SaveChanges();

                            return new Response { IsSuccess = true, ReturnCode = ResponseCodes.SUCCESS };
                        }


                    }
                    else {

                        return new Response { IsSuccess = false, ReturnCode = ResponseCodes.INVALID_STATE, ReturnMessage = "Slot state is invalid." };
                    }



                }



                #endregion
            }
            catch (Exception ex) {

                //Log the error details
                _logger.LogError(ex, $"Error while processing ConfirmReservation. Error:{ex.Message}");

                return new Response { IsSuccess = false, ReturnCode = ResponseCodes.INTERNAL_SERVER_ERROR, ReturnMessage = "Internal server error." };


            }
        }

    }
}
