namespace Reservation.Common.Models {
    /// <summary>
    /// This class contains properties defining the base response object returned from the execution of a method
    /// </summary>
    public class Response {
        private int _returnCode;

        /// <summary>
        /// this property contains true if the execution of the method was successful, else false
        /// </summary>        
        public bool IsSuccess { get; set; }

        /// <summary>
        /// this property contains the code describing the success/failure of the execution of the method
        /// </summary>        
        public int ReturnCode {
            get { return _returnCode; }
            set {
                _returnCode = value;
            }
        }
        /// <summary>
        /// this property contains the return message from the execution
        /// </summary>
        public string ReturnMessage { get; set; }

        /// <summary>
        /// this property contains any additional data describing the execution
        /// </summary>
        public object AdditionalData { get; set; }

        /// <summary>
        /// This method returns the string representation of the response
        /// </summary>
        /// <returns>
        /// Returns an object of <see cref="string"/> which is the string representation of the response
        /// </returns>
        public override string ToString() {
            return $@"IsSuccess : {IsSuccess}\n
                      ReturnCode: {ReturnCode}\n
                      ReturnMessage:{ReturnMessage}\n
                    ";
        }
    }
}
