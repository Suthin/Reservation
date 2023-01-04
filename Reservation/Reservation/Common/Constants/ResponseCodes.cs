namespace Reservation.Common.Constants {
    /// <summary>
    /// This class contains properties defining error codes
    /// </summary>
    public static class ResponseCodes {
        public const int SUCCESS = 0;                           // No errors in the code
        public const int INTERNAL_SERVER_ERROR = 1;             // Internal Server Error.
        public const int UNKNOWN_ERROR = 2;                     // Unknown error
        public const int INVALID_TOKEN = 3;                     // Invalid Token.
        public const int INVALID_IDENTIFIER = 4;                // Invalid identifier.
        public const int INVALID_PARAMETERS = 5;                // Invalid Method parameters
        public const int DUPLICATE_RECORD = 6;                  // Duplicate records
        public const int ALREADY_COMPLETED = 7;                 // Required action is already completed.
        public const int INVALID_STATE = 8;                 // Required action is already completed.
    }
}
