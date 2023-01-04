

namespace Reservation.Common.Models {
    /// <summary>
    /// This class contains properties that hold the result of a method execution
    /// </summary>
    /// <typeparam name="T">type of the data returned from method execution</typeparam>    
    public class SingleResponse<T> : Response {
        /// <summary>
        /// this property holds the data from the method execution
        /// </summary>
        public T Result { get; set; }

        /// <summary>
        /// Constructor for a SingleResponse with no parameters
        /// </summary>
        public SingleResponse() {
        }

        /// <summary>
        /// Constructor for a SingleResponse from a <see cref="Response"/>
        /// </summary>
        /// <param name="response">Response used to construct the SingleResponse</param>
        public SingleResponse(Response response) {
            this.IsSuccess = response.IsSuccess;
            this.ReturnCode = response.ReturnCode;
            this.ReturnMessage = response.ReturnMessage;
            this.AdditionalData = response.AdditionalData;
        }

        /// <summary>
        /// Method to convert a single reponse of one type to another type in case IsSuccess = false
        /// Method does not copy the Result field from the input object
        /// </summary>
        /// <typeparam name="T1">Type of the new response message</typeparam>
        /// <returns>return a new object of the type <see cref="SingleResponse&lt;T1&gt;"/></returns>
        public SingleResponse<T1> ConvertToSingleResponseOf<T1>() {
            return new SingleResponse<T1> {
                IsSuccess = this.IsSuccess,
                ReturnCode = this.ReturnCode,
                ReturnMessage = this.ReturnMessage,
                AdditionalData = this.AdditionalData
            };
        }

        /// <summary>
        /// This method gets the string representation of the response
        /// </summary>
        /// <returns>
        /// Returns an object of <see cref="string"/> which is the string representation of the response
        /// </returns>
        public override string ToString() {
            return $@"{base.ToString()}\n
                       Result:{Result}
                    ";
        }
    }
}
