using Reservation.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Reservation.xUnitTest.Library {
    public class ReservationServiceTestSuite {

        [Fact]
        public void GetAvailableSlots_WithoutAnyParameter() {
            var reservationService = new ReservationService();
            var result = reservationService.GetAvailableSlots(null,null,null,null,null,null,null);

            
            Assert.True(result.IsSuccess, "Returning all results.");
        }
    }
}
