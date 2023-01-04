namespace Reservation.Utils {
    /// <summary>
    /// This class contains the methods required for creating the default ILogger instance based on the passed in category name/type.      
    /// </summary>
    public class LogManager {
        private static ILoggerFactory _factory = null;

        private LogManager() {

        }

        /// <summary>
        /// This property holds the ILoggerFactory instance which is required for Creating ILogger instances .
        /// The value of this property should be set during application start up.
        /// </summary>
        public static ILoggerFactory LoggerFactory {
            get {
                if (_factory == null) {
                    _factory = new LoggerFactory();
                }
                return _factory;
            }
            set { _factory = value; }
        }

        public static ILogger<T> CreateLogger<T>() => LoggerFactory.CreateLogger<T>();

        public static ILogger CreateLogger(Type type) => LoggerFactory.CreateLogger(type);

        public static ILogger CreateLogger(string categoryName) => LoggerFactory.CreateLogger(categoryName);
    }
}
